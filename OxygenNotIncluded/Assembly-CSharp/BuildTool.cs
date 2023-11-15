using System;
using System.Collections.Generic;
using FMOD.Studio;
using Rendering;
using STRINGS;
using UnityEngine;

// Token: 0x0200080C RID: 2060
public class BuildTool : DragTool
{
	// Token: 0x06003B26 RID: 15142 RVA: 0x00148CB1 File Offset: 0x00146EB1
	public static void DestroyInstance()
	{
		BuildTool.Instance = null;
	}

	// Token: 0x06003B27 RID: 15143 RVA: 0x00148CB9 File Offset: 0x00146EB9
	protected override void OnPrefabInit()
	{
		BuildTool.Instance = this;
		this.tooltip = base.GetComponent<ToolTip>();
		this.buildingCount = UnityEngine.Random.Range(1, 14);
		this.canChangeDragAxis = false;
	}

	// Token: 0x06003B28 RID: 15144 RVA: 0x00148CE4 File Offset: 0x00146EE4
	protected override void OnActivateTool()
	{
		this.lastDragCell = -1;
		if (this.visualizer != null)
		{
			this.ClearTilePreview();
			UnityEngine.Object.Destroy(this.visualizer);
		}
		this.active = true;
		base.OnActivateTool();
		Vector3 vector = base.ClampPositionToWorld(PlayerController.GetCursorPos(KInputManager.GetMousePos()), ClusterManager.Instance.activeWorld);
		this.visualizer = GameUtil.KInstantiate(this.def.BuildingPreview, vector, Grid.SceneLayer.Ore, null, LayerMask.NameToLayer("Place"));
		KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.visibilityType = KAnimControllerBase.VisibilityType.Always;
			component.isMovable = true;
			component.Offset = this.def.GetVisualizerOffset();
			component.name = component.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
		}
		if (!this.facadeID.IsNullOrWhiteSpace() && this.facadeID != "DEFAULT_FACADE")
		{
			this.visualizer.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.facadeID));
		}
		Rotatable component2 = this.visualizer.GetComponent<Rotatable>();
		if (component2 != null)
		{
			this.buildingOrientation = this.def.InitialOrientation;
			component2.SetOrientation(this.buildingOrientation);
		}
		this.visualizer.SetActive(true);
		this.UpdateVis(vector);
		base.GetComponent<BuildToolHoverTextCard>().currentDef = this.def;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
		if (component == null)
		{
			this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
		}
		else
		{
			component.SetLayer(LayerMask.NameToLayer("Place"));
		}
		GridCompositor.Instance.ToggleMajor(true);
	}

	// Token: 0x06003B29 RID: 15145 RVA: 0x00148E94 File Offset: 0x00147094
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.lastDragCell = -1;
		if (!this.active)
		{
			return;
		}
		this.active = false;
		GridCompositor.Instance.ToggleMajor(false);
		this.buildingOrientation = Orientation.Neutral;
		this.HideToolTip();
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
		this.ClearTilePreview();
		UnityEngine.Object.Destroy(this.visualizer);
		if (new_tool == SelectTool.Instance)
		{
			Game.Instance.Trigger(-1190690038, null);
		}
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06003B2A RID: 15146 RVA: 0x00148F0F File Offset: 0x0014710F
	public void Activate(BuildingDef def, IList<Tag> selected_elements)
	{
		this.selectedElements = selected_elements;
		this.def = def;
		this.viewMode = def.ViewMode;
		ResourceRemainingDisplayScreen.instance.SetResources(selected_elements, def.CraftRecipe);
		PlayerController.Instance.ActivateTool(this);
		this.OnActivateTool();
	}

	// Token: 0x06003B2B RID: 15147 RVA: 0x00148F4D File Offset: 0x0014714D
	public void Activate(BuildingDef def, IList<Tag> selected_elements, string facadeID)
	{
		this.facadeID = facadeID;
		this.Activate(def, selected_elements);
	}

	// Token: 0x06003B2C RID: 15148 RVA: 0x00148F5E File Offset: 0x0014715E
	public void Deactivate()
	{
		this.selectedElements = null;
		SelectTool.Instance.Activate();
		this.def = null;
		this.facadeID = null;
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06003B2D RID: 15149 RVA: 0x00148F89 File Offset: 0x00147189
	public int GetLastCell
	{
		get
		{
			return this.lastCell;
		}
	}

	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x06003B2E RID: 15150 RVA: 0x00148F91 File Offset: 0x00147191
	public Orientation GetBuildingOrientation
	{
		get
		{
			return this.buildingOrientation;
		}
	}

	// Token: 0x06003B2F RID: 15151 RVA: 0x00148F9C File Offset: 0x0014719C
	private void ClearTilePreview()
	{
		if (Grid.IsValidBuildingCell(this.lastCell) && this.def.IsTilePiece)
		{
			GameObject gameObject = Grid.Objects[this.lastCell, (int)this.def.TileLayer];
			if (this.visualizer == gameObject)
			{
				Grid.Objects[this.lastCell, (int)this.def.TileLayer] = null;
			}
			if (this.def.isKAnimTile)
			{
				GameObject x = null;
				if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
				{
					x = Grid.Objects[this.lastCell, (int)this.def.ReplacementLayer];
				}
				if ((gameObject == null || gameObject.GetComponent<Constructable>() == null) && (x == null || x == this.visualizer))
				{
					World.Instance.blockTileRenderer.RemoveBlock(this.def, false, SimHashes.Void, this.lastCell);
					World.Instance.blockTileRenderer.RemoveBlock(this.def, true, SimHashes.Void, this.lastCell);
					TileVisualizer.RefreshCell(this.lastCell, this.def.TileLayer, this.def.ReplacementLayer);
				}
			}
		}
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x001490DD File Offset: 0x001472DD
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		this.UpdateVis(cursorPos);
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x00149100 File Offset: 0x00147300
	private void UpdateVis(Vector3 pos)
	{
		string text;
		bool flag = this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, out text);
		bool flag2 = this.def.IsValidReplaceLocation(pos, this.buildingOrientation, this.def.ReplacementLayer, this.def.ObjectLayer);
		flag = (flag || flag2);
		if (this.visualizer != null)
		{
			Color c = Color.white;
			float strength = 0f;
			if (!flag)
			{
				c = Color.red;
				strength = 1f;
			}
			this.SetColor(this.visualizer, c, strength);
		}
		int num = Grid.PosToCell(pos);
		if (this.def != null)
		{
			Vector3 vector = Grid.CellToPosCBC(num, this.def.SceneLayer);
			this.visualizer.transform.SetPosition(vector);
			base.transform.SetPosition(vector - Vector3.up * 0.5f);
			if (this.def.IsTilePiece)
			{
				this.ClearTilePreview();
				if (Grid.IsValidBuildingCell(num))
				{
					GameObject gameObject = Grid.Objects[num, (int)this.def.TileLayer];
					if (gameObject == null)
					{
						Grid.Objects[num, (int)this.def.TileLayer] = this.visualizer;
					}
					if (this.def.isKAnimTile)
					{
						GameObject x = null;
						if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
						{
							x = Grid.Objects[num, (int)this.def.ReplacementLayer];
						}
						if (gameObject == null || (gameObject.GetComponent<Constructable>() == null && x == null))
						{
							TileVisualizer.RefreshCell(num, this.def.TileLayer, this.def.ReplacementLayer);
							if (this.def.BlockTileAtlas != null)
							{
								int renderLayer = LayerMask.NameToLayer("Overlay");
								BlockTileRenderer blockTileRenderer = World.Instance.blockTileRenderer;
								blockTileRenderer.SetInvalidPlaceCell(num, !flag);
								if (this.lastCell != num)
								{
									blockTileRenderer.SetInvalidPlaceCell(this.lastCell, false);
								}
								blockTileRenderer.AddBlock(renderLayer, this.def, flag2, SimHashes.Void, num);
							}
						}
					}
				}
			}
			if (this.lastCell != num)
			{
				this.lastCell = num;
			}
		}
	}

	// Token: 0x06003B32 RID: 15154 RVA: 0x00149344 File Offset: 0x00147544
	public PermittedRotations? GetPermittedRotations()
	{
		if (this.visualizer == null)
		{
			return null;
		}
		Rotatable component = this.visualizer.GetComponent<Rotatable>();
		if (component == null)
		{
			return null;
		}
		return new PermittedRotations?(component.permittedRotations);
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x00149393 File Offset: 0x00147593
	public bool CanRotate()
	{
		return !(this.visualizer == null) && !(this.visualizer.GetComponent<Rotatable>() == null);
	}

	// Token: 0x06003B34 RID: 15156 RVA: 0x001493BC File Offset: 0x001475BC
	public void TryRotate()
	{
		if (this.visualizer == null)
		{
			return;
		}
		Rotatable component = this.visualizer.GetComponent<Rotatable>();
		if (component == null)
		{
			return;
		}
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Rotate", false));
		this.buildingOrientation = component.Rotate();
		if (Grid.IsValidBuildingCell(this.lastCell))
		{
			Vector3 pos = Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building);
			this.UpdateVis(pos);
		}
		if (base.Dragging && this.lastDragCell != -1)
		{
			this.TryBuild(this.lastDragCell);
		}
	}

	// Token: 0x06003B35 RID: 15157 RVA: 0x00149449 File Offset: 0x00147649
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.RotateBuilding))
		{
			this.TryRotate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06003B36 RID: 15158 RVA: 0x00149466 File Offset: 0x00147666
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		this.TryBuild(cell);
	}

	// Token: 0x06003B37 RID: 15159 RVA: 0x00149470 File Offset: 0x00147670
	private void TryBuild(int cell)
	{
		if (this.visualizer == null)
		{
			return;
		}
		if (cell == this.lastDragCell && this.buildingOrientation == this.lastDragOrientation)
		{
			return;
		}
		if (Grid.PosToCell(this.visualizer) != cell)
		{
			if (this.def.BuildingComplete.GetComponent<LogicPorts>())
			{
				return;
			}
			if (this.def.BuildingComplete.GetComponent<LogicGateBase>())
			{
				return;
			}
		}
		this.lastDragCell = cell;
		this.lastDragOrientation = this.buildingOrientation;
		this.ClearTilePreview();
		Vector3 pos = Grid.CellToPosCBC(cell, Grid.SceneLayer.Building);
		GameObject gameObject = null;
		PlanScreen.Instance.LastSelectedBuildingFacade = this.facadeID;
		bool flag = DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild);
		string text;
		if (!flag)
		{
			gameObject = this.def.TryPlace(this.visualizer, pos, this.buildingOrientation, this.selectedElements, this.facadeID, 0);
		}
		else if (this.def.IsValidBuildLocation(this.visualizer, pos, this.buildingOrientation, false) && this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, out text))
		{
			gameObject = this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, 293.15f, this.facadeID, false, GameClock.Instance.GetTime());
		}
		if (gameObject == null && this.def.ReplacementLayer != ObjectLayer.NumLayers)
		{
			GameObject replacementCandidate = this.def.GetReplacementCandidate(cell);
			if (replacementCandidate != null && !this.def.IsReplacementLayerOccupied(cell))
			{
				BuildingComplete component = replacementCandidate.GetComponent<BuildingComplete>();
				if (component != null && component.Def.Replaceable && this.def.CanReplace(replacementCandidate) && (component.Def != this.def || this.selectedElements[0] != replacementCandidate.GetComponent<PrimaryElement>().Element.tag))
				{
					string text2;
					if (!flag)
					{
						gameObject = this.def.TryReplaceTile(this.visualizer, pos, this.buildingOrientation, this.selectedElements, this.facadeID, 0);
						Grid.Objects[cell, (int)this.def.ReplacementLayer] = gameObject;
					}
					else if (this.def.IsValidBuildLocation(this.visualizer, pos, this.buildingOrientation, true) && this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, true, out text2))
					{
						gameObject = this.InstantBuildReplace(cell, pos, replacementCandidate);
					}
				}
			}
		}
		this.PostProcessBuild(flag, pos, gameObject);
	}

	// Token: 0x06003B38 RID: 15160 RVA: 0x00149720 File Offset: 0x00147920
	private GameObject InstantBuildReplace(int cell, Vector3 pos, GameObject tile)
	{
		if (tile.GetComponent<SimCellOccupier>() == null)
		{
			UnityEngine.Object.Destroy(tile);
			return this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, 293.15f, this.facadeID, false, GameClock.Instance.GetTime());
		}
		tile.GetComponent<SimCellOccupier>().DestroySelf(delegate
		{
			UnityEngine.Object.Destroy(tile);
			GameObject builtItem = this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, 293.15f, this.facadeID, false, GameClock.Instance.GetTime());
			this.PostProcessBuild(true, pos, builtItem);
		});
		return null;
	}

	// Token: 0x06003B39 RID: 15161 RVA: 0x001497C0 File Offset: 0x001479C0
	private void PostProcessBuild(bool instantBuild, Vector3 pos, GameObject builtItem)
	{
		if (builtItem == null)
		{
			return;
		}
		if (!instantBuild)
		{
			Prioritizable component = builtItem.GetComponent<Prioritizable>();
			if (component != null)
			{
				if (BuildMenu.Instance != null)
				{
					component.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
				}
				if (PlanScreen.Instance != null)
				{
					component.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
				}
			}
		}
		if (this.def.MaterialsAvailable(this.selectedElements, ClusterManager.Instance.activeWorld) || DebugHandler.InstantBuildMode)
		{
			this.placeSound = GlobalAssets.GetSound("Place_Building_" + this.def.AudioSize, false);
			if (this.placeSound != null)
			{
				this.buildingCount = this.buildingCount % 14 + 1;
				Vector3 pos2 = pos;
				pos2.z = 0f;
				EventInstance instance = SoundEvent.BeginOneShot(this.placeSound, pos2, 1f, false);
				if (this.def.AudioSize == "small")
				{
					instance.setParameterByName("tileCount", (float)this.buildingCount, false);
				}
				SoundEvent.EndOneShot(instance);
			}
		}
		else
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, pos, 1.5f, false, false);
		}
		Rotatable component2 = builtItem.GetComponent<Rotatable>();
		if (component2 != null)
		{
			component2.SetOrientation(this.buildingOrientation);
		}
		if (this.def.OnePerWorld)
		{
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
		}
	}

	// Token: 0x06003B3A RID: 15162 RVA: 0x0014993E File Offset: 0x00147B3E
	protected override DragTool.Mode GetMode()
	{
		return DragTool.Mode.Brush;
	}

	// Token: 0x06003B3B RID: 15163 RVA: 0x00149944 File Offset: 0x00147B44
	private void SetColor(GameObject root, Color c, float strength)
	{
		KBatchedAnimController component = root.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.TintColour = c;
		}
	}

	// Token: 0x06003B3C RID: 15164 RVA: 0x0014996D File Offset: 0x00147B6D
	private void ShowToolTip()
	{
		ToolTipScreen.Instance.SetToolTip(this.tooltip);
	}

	// Token: 0x06003B3D RID: 15165 RVA: 0x0014997F File Offset: 0x00147B7F
	private void HideToolTip()
	{
		ToolTipScreen.Instance.ClearToolTip(this.tooltip);
	}

	// Token: 0x06003B3E RID: 15166 RVA: 0x00149994 File Offset: 0x00147B94
	public void Update()
	{
		if (this.active)
		{
			KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetLayer(LayerMask.NameToLayer("Place"));
			}
		}
	}

	// Token: 0x06003B3F RID: 15167 RVA: 0x001499CE File Offset: 0x00147BCE
	public override string GetDeactivateSound()
	{
		return "HUD_Click_Deselect";
	}

	// Token: 0x06003B40 RID: 15168 RVA: 0x001499D5 File Offset: 0x00147BD5
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
	}

	// Token: 0x06003B41 RID: 15169 RVA: 0x001499DE File Offset: 0x00147BDE
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
	}

	// Token: 0x06003B42 RID: 15170 RVA: 0x001499E8 File Offset: 0x00147BE8
	public void SetToolOrientation(Orientation orientation)
	{
		if (this.visualizer != null)
		{
			Rotatable component = this.visualizer.GetComponent<Rotatable>();
			if (component != null)
			{
				this.buildingOrientation = orientation;
				component.SetOrientation(orientation);
				if (Grid.IsValidBuildingCell(this.lastCell))
				{
					Vector3 pos = Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building);
					this.UpdateVis(pos);
				}
				if (base.Dragging && this.lastDragCell != -1)
				{
					this.TryBuild(this.lastDragCell);
				}
			}
		}
	}

	// Token: 0x04002721 RID: 10017
	[SerializeField]
	private TextStyleSetting tooltipStyle;

	// Token: 0x04002722 RID: 10018
	private int lastCell = -1;

	// Token: 0x04002723 RID: 10019
	private int lastDragCell = -1;

	// Token: 0x04002724 RID: 10020
	private Orientation lastDragOrientation;

	// Token: 0x04002725 RID: 10021
	private IList<Tag> selectedElements;

	// Token: 0x04002726 RID: 10022
	private BuildingDef def;

	// Token: 0x04002727 RID: 10023
	private Orientation buildingOrientation;

	// Token: 0x04002728 RID: 10024
	private string facadeID;

	// Token: 0x04002729 RID: 10025
	private ToolTip tooltip;

	// Token: 0x0400272A RID: 10026
	public static BuildTool Instance;

	// Token: 0x0400272B RID: 10027
	private bool active;

	// Token: 0x0400272C RID: 10028
	private int buildingCount;
}
