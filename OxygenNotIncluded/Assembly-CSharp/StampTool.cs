using System;
using System.Collections;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;

// Token: 0x0200082F RID: 2095
public class StampTool : InterfaceTool
{
	// Token: 0x06003CD9 RID: 15577 RVA: 0x00151210 File Offset: 0x0014F410
	public static void DestroyInstance()
	{
		StampTool.Instance = null;
		StampTool.previewPool = null;
		StampTool.placerPool = null;
		StampTool.previewPoolTransform = null;
		StampTool.placerPoolTransform = null;
	}

	// Token: 0x06003CDA RID: 15578 RVA: 0x00151230 File Offset: 0x0014F430
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		StampTool.Instance = this;
		StampTool.placerPool = new GameObjectPool(new Func<GameObject>(StampTool.InstantiatePlacer), 0);
		StampTool.previewPool = new HashMapObjectPool<Tag, Building>(new Func<Tag, Building>(StampTool.InstantiatePreview), 0);
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x0015126C File Offset: 0x0014F46C
	private void Update()
	{
		this.RefreshPreview(Grid.PosToCell(this.GetCursorPos()));
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x00151280 File Offset: 0x0014F480
	public void Activate(TemplateContainer template, bool SelectAffected = false, bool DeactivateOnStamp = false)
	{
		this.selectAffected = SelectAffected;
		this.deactivateOnStamp = DeactivateOnStamp;
		if (this.stampTemplate == template || template == null || template.cells == null)
		{
			return;
		}
		this.stampTemplate = template;
		PlayerController.Instance.ActivateTool(this);
		base.StartCoroutine(this.InitializePlacementVisual());
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x001512CF File Offset: 0x0014F4CF
	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x001512DB File Offset: 0x0014F4DB
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		this.Stamp(cursor_pos);
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x001512F0 File Offset: 0x0014F4F0
	private void Stamp(Vector2 pos)
	{
		if (!this.ready)
		{
			return;
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(-this.stampTemplate.info.size.X / 2f), 0);
		int cell2 = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(this.stampTemplate.info.size.X / 2f), 0);
		int cell3 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(-this.stampTemplate.info.size.Y / 2f));
		int cell4 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(this.stampTemplate.info.size.Y / 2f));
		if (!Grid.IsValidBuildingCell(cell) || !Grid.IsValidBuildingCell(cell2) || !Grid.IsValidBuildingCell(cell4) || !Grid.IsValidBuildingCell(cell3))
		{
			return;
		}
		this.ready = false;
		bool pauseOnComplete = SpeedControlScreen.Instance.IsPaused;
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
		if (this.stampTemplate.cells != null)
		{
			for (int i = 0; i < this.buildingPreviews.Count; i++)
			{
				StampTool.ClearTilePreview(this.buildingPreviews[i]);
			}
			List<GameObject> list = new List<GameObject>();
			for (int j = 0; j < this.stampTemplate.cells.Count; j++)
			{
				for (int k = 0; k < 34; k++)
				{
					GameObject gameObject = Grid.Objects[Grid.XYToCell((int)(pos.x + (float)this.stampTemplate.cells[j].location_x), (int)(pos.y + (float)this.stampTemplate.cells[j].location_y)), k];
					if (gameObject != null && !list.Contains(gameObject))
					{
						list.Add(gameObject);
					}
				}
			}
			if (list != null)
			{
				foreach (GameObject gameObject2 in list)
				{
					if (gameObject2 != null)
					{
						Util.KDestroyGameObject(gameObject2);
					}
				}
			}
		}
		TemplateLoader.Stamp(this.stampTemplate, pos, delegate
		{
			this.CompleteStamp(pauseOnComplete);
		});
		if (this.selectAffected)
		{
			DebugBaseTemplateButton.Instance.ClearSelection();
			if (this.stampTemplate.cells != null)
			{
				for (int l = 0; l < this.stampTemplate.cells.Count; l++)
				{
					DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int)(pos.x + (float)this.stampTemplate.cells[l].location_x), (int)(pos.y + (float)this.stampTemplate.cells[l].location_y)));
				}
			}
		}
		if (this.deactivateOnStamp)
		{
			base.DeactivateTool(null);
		}
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x00151620 File Offset: 0x0014F820
	private void CompleteStamp(bool pause)
	{
		if (pause)
		{
			SpeedControlScreen.Instance.Pause(true, false);
		}
		this.ready = true;
		this.OnDeactivateTool(null);
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x0015163F File Offset: 0x0014F83F
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (base.gameObject.activeSelf)
		{
			return;
		}
		this.ReleasePlacementVisual();
		this.placementCell = Grid.InvalidCell;
		this.stampTemplate = null;
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x0015166E File Offset: 0x0014F86E
	private IEnumerator InitializePlacementVisual()
	{
		this.ReleasePlacementVisual();
		this.rootCellPlacer = StampTool.placerPool.GetInstance();
		for (int i = 0; i < this.stampTemplate.cells.Count; i++)
		{
			Cell cell = this.stampTemplate.cells[i];
			if (cell.location_x != 0 || cell.location_y != 0)
			{
				GameObject instance = StampTool.placerPool.GetInstance();
				instance.transform.SetParent(this.rootCellPlacer.transform);
				instance.transform.localPosition = new Vector3((float)cell.location_x, (float)cell.location_y);
				instance.SetActive(true);
				this.childCellPlacers.Add(instance);
			}
		}
		if (this.stampTemplate.buildings != null)
		{
			yield return this.InitializeBuildingPlacementVisuals();
		}
		yield break;
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x0015167D File Offset: 0x0014F87D
	private IEnumerator InitializeBuildingPlacementVisuals()
	{
		foreach (Prefab prefab in this.stampTemplate.buildings)
		{
			Building instance = StampTool.previewPool.GetInstance(prefab.id);
			Rotatable component = instance.GetComponent<Rotatable>();
			if (component != null)
			{
				component.SetOrientation(prefab.rotationOrientation);
			}
			instance.transform.SetParent(this.rootCellPlacer.transform);
			instance.transform.SetLocalPosition(new Vector2((float)prefab.location_x, (float)prefab.location_y));
			instance.gameObject.SetActive(true);
			this.buildingPreviews.Add(instance);
		}
		yield return null;
		for (int i = 0; i < this.stampTemplate.buildings.Count; i++)
		{
			Prefab prefab2 = this.stampTemplate.buildings[i];
			Building building = this.buildingPreviews[i];
			string text = "";
			if ((prefab2.connections & 1) != 0)
			{
				text += "L";
			}
			if ((prefab2.connections & 2) != 0)
			{
				text += "R";
			}
			if ((prefab2.connections & 4) != 0)
			{
				text += "U";
			}
			if ((prefab2.connections & 8) != 0)
			{
				text += "D";
			}
			if (text == "")
			{
				text = "None";
			}
			KBatchedAnimController component2 = building.GetComponent<KBatchedAnimController>();
			if (component2 != null && component2.HasAnimation(text))
			{
				string text2 = text + "_place";
				bool flag = component2.HasAnimation(text2);
				component2.Play(flag ? text2 : text, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
		yield break;
	}

	// Token: 0x06003CE4 RID: 15588 RVA: 0x0015168C File Offset: 0x0014F88C
	private void ReleasePlacementVisual()
	{
		if (this.rootCellPlacer == null)
		{
			return;
		}
		this.rootCellPlacer.SetActive(false);
		for (int i = this.childCellPlacers.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = this.childCellPlacers[i];
			gameObject.transform.SetParent(StampTool.placerPoolTransform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.SetActive(false);
			StampTool.placerPool.ReleaseInstance(gameObject);
			this.childCellPlacers.RemoveAt(i);
		}
		for (int j = this.buildingPreviews.Count - 1; j >= 0; j--)
		{
			Building building = this.buildingPreviews[j];
			StampTool.ClearTilePreview(building);
			building.transform.SetParent(StampTool.previewPoolTransform);
			building.transform.localPosition = Vector3.zero;
			building.gameObject.SetActive(false);
			StampTool.previewPool.ReleaseInstance(building.Def.PrefabID, building);
			this.buildingPreviews.RemoveAt(j);
		}
		this.rootCellPlacer.transform.SetParent(StampTool.placerPoolTransform);
		this.rootCellPlacer.transform.position = Vector3.zero;
		StampTool.placerPool.ReleaseInstance(this.rootCellPlacer);
		this.rootCellPlacer = null;
	}

	// Token: 0x06003CE5 RID: 15589 RVA: 0x001517D8 File Offset: 0x0014F9D8
	private static void ClearTilePreview(Building b)
	{
		int cell = Grid.PosToCell(b.transform.position);
		if (!b.Def.IsTilePiece || !Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		if (b.gameObject == Grid.Objects[cell, (int)b.Def.TileLayer])
		{
			Grid.Objects[cell, (int)b.Def.TileLayer] = null;
		}
		if (!b.Def.isKAnimTile)
		{
			return;
		}
		if (b.Def.BlockTileAtlas != null)
		{
			World.Instance.blockTileRenderer.RemoveBlock(b.Def, false, SimHashes.Void, cell);
		}
		TileVisualizer.RefreshCell(cell, b.Def.TileLayer, ObjectLayer.NumLayers);
	}

	// Token: 0x06003CE6 RID: 15590 RVA: 0x00151898 File Offset: 0x0014FA98
	private static void UpdateTileRendering(int newCell, Building b)
	{
		StampTool.ClearTilePreview(b);
		if (!b.Def.IsTilePiece || !Grid.IsValidBuildingCell(newCell))
		{
			return;
		}
		if (Grid.Objects[newCell, (int)b.Def.TileLayer] == null)
		{
			Grid.Objects[newCell, (int)b.Def.TileLayer] = b.gameObject;
		}
		if (!b.Def.isKAnimTile)
		{
			return;
		}
		if (b.Def.BlockTileAtlas != null)
		{
			World.Instance.blockTileRenderer.AddBlock(b.gameObject.layer, b.Def, false, SimHashes.Void, newCell);
		}
		TileVisualizer.RefreshCell(newCell, b.Def.TileLayer, ObjectLayer.NumLayers);
	}

	// Token: 0x06003CE7 RID: 15591 RVA: 0x00151958 File Offset: 0x0014FB58
	public void RefreshPreview(int new_placement_cell)
	{
		if (Grid.IsValidCell(new_placement_cell) && new_placement_cell != this.placementCell)
		{
			for (int i = 0; i < this.buildingPreviews.Count; i++)
			{
				Building building = this.buildingPreviews[i];
				Vector3 localPosition = building.transform.localPosition;
				StampTool.UpdateTileRendering(Grid.OffsetCell(new_placement_cell, (int)localPosition.x, (int)localPosition.y), building);
			}
			this.placementCell = new_placement_cell;
			this.rootCellPlacer.transform.SetPosition(Grid.CellToPosCBC(this.placementCell, this.visualizerLayer));
			this.rootCellPlacer.SetActive(true);
		}
	}

	// Token: 0x06003CE8 RID: 15592 RVA: 0x001519F8 File Offset: 0x0014FBF8
	private static Building InstantiatePreview(Tag previewId)
	{
		GameObject gameObject = Assets.TryGetPrefab(previewId);
		if (gameObject == null)
		{
			return null;
		}
		Building component = gameObject.GetComponent<Building>();
		if (component == null)
		{
			return null;
		}
		if (StampTool.previewPoolTransform == null)
		{
			StampTool.previewPoolTransform = new GameObject("Preview Pool").transform;
		}
		GameObject gameObject2 = component.Def.BuildingPreview;
		if (gameObject2 == null)
		{
			gameObject2 = BuildingLoader.Instance.CreateBuildingPreview(component.Def);
		}
		int num = LayerMask.NameToLayer("Place");
		Building component2 = GameUtil.KInstantiate(gameObject2, Vector3.zero, Grid.SceneLayer.Ore, null, num).GetComponent<Building>();
		KBatchedAnimController component3 = component2.GetComponent<KBatchedAnimController>();
		if (component3 != null)
		{
			component3.visibilityType = KAnimControllerBase.VisibilityType.Always;
			component3.isMovable = true;
			component3.Offset = component.Def.GetVisualizerOffset();
			component3.name = component3.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
			component3.TintColour = Color.white;
			component3.SetLayer(num);
		}
		component2.transform.SetParent(StampTool.previewPoolTransform);
		component2.gameObject.SetActive(false);
		return component2;
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x00151B18 File Offset: 0x0014FD18
	private static GameObject InstantiatePlacer()
	{
		if (StampTool.placerPoolTransform == null)
		{
			StampTool.placerPoolTransform = new GameObject("Stamp Placer Pool").transform;
		}
		GameObject gameObject = Util.KInstantiate(StampTool.Instance.PlacerPrefab, StampTool.placerPoolTransform.gameObject, null);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x040027C0 RID: 10176
	public static StampTool Instance;

	// Token: 0x040027C1 RID: 10177
	private static HashMapObjectPool<Tag, Building> previewPool;

	// Token: 0x040027C2 RID: 10178
	private static GameObjectPool placerPool;

	// Token: 0x040027C3 RID: 10179
	private static Transform previewPoolTransform;

	// Token: 0x040027C4 RID: 10180
	private static Transform placerPoolTransform;

	// Token: 0x040027C5 RID: 10181
	public TemplateContainer stampTemplate;

	// Token: 0x040027C6 RID: 10182
	public GameObject PlacerPrefab;

	// Token: 0x040027C7 RID: 10183
	private bool ready = true;

	// Token: 0x040027C8 RID: 10184
	private int placementCell = Grid.InvalidCell;

	// Token: 0x040027C9 RID: 10185
	private bool selectAffected;

	// Token: 0x040027CA RID: 10186
	private bool deactivateOnStamp;

	// Token: 0x040027CB RID: 10187
	private GameObject rootCellPlacer;

	// Token: 0x040027CC RID: 10188
	private List<GameObject> childCellPlacers = new List<GameObject>();

	// Token: 0x040027CD RID: 10189
	private List<Building> buildingPreviews = new List<Building>();
}
