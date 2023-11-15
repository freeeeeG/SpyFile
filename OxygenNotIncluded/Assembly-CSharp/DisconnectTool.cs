using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000815 RID: 2069
public class DisconnectTool : FilteredDragTool
{
	// Token: 0x06003B89 RID: 15241 RVA: 0x0014AB71 File Offset: 0x00148D71
	public static void DestroyInstance()
	{
		DisconnectTool.Instance = null;
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x0014AB7C File Offset: 0x00148D7C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DisconnectTool.Instance = this;
		this.disconnectVisPool = new GameObjectPool(new Func<GameObject>(this.InstantiateDisconnectVis), this.singleDisconnectMode ? 1 : 10);
		if (this.singleDisconnectMode)
		{
			this.lineModeMaxLength = 2;
		}
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x0014ABC8 File Offset: 0x00148DC8
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x0014ABD5 File Offset: 0x00148DD5
	protected override DragTool.Mode GetMode()
	{
		if (!this.singleDisconnectMode)
		{
			return DragTool.Mode.Box;
		}
		return DragTool.Mode.Line;
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x0014ABE2 File Offset: 0x00148DE2
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		if (this.singleDisconnectMode)
		{
			upPos = base.SnapToLine(upPos);
		}
		this.RunOnRegion(downPos, upPos, new Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections>(this.DisconnectCellsAction));
		this.ClearVisualizers();
	}

	// Token: 0x06003B8E RID: 15246 RVA: 0x0014AC0F File Offset: 0x00148E0F
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.lastRefreshedCell = -1;
	}

	// Token: 0x06003B8F RID: 15247 RVA: 0x0014AC20 File Offset: 0x00148E20
	private void DisconnectCellsAction(int cell, GameObject objectOnCell, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
	{
		Building component = objectOnCell.GetComponent<Building>();
		KAnimGraphTileVisualizer component2 = objectOnCell.GetComponent<KAnimGraphTileVisualizer>();
		if (component2 != null)
		{
			UtilityConnections new_connections = utilityComponent.GetNetworkManager().GetConnections(cell, false) & ~removeConnections;
			component2.UpdateConnections(new_connections);
			component2.Refresh();
		}
		TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
		utilityComponent.GetNetworkManager().ForceRebuildNetworks();
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x0014AC8C File Offset: 0x00148E8C
	private void RunOnRegion(Vector3 pos1, Vector3 pos2, Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections> action)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(pos1, pos2), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(pos1, pos2), false);
		Vector2I vector2I = new Vector2I((int)regularizedPos.x, (int)regularizedPos.y);
		Vector2I vector2I2 = new Vector2I((int)regularizedPos2.x, (int)regularizedPos2.y);
		for (int i = vector2I.x; i < vector2I2.x; i++)
		{
			for (int j = vector2I.y; j < vector2I2.y; j++)
			{
				int num = Grid.XYToCell(i, j);
				if (Grid.IsVisible(num))
				{
					for (int k = 0; k < 45; k++)
					{
						GameObject gameObject = Grid.Objects[num, k];
						if (!(gameObject == null))
						{
							string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
							if (base.IsActiveLayer(filterLayerFromGameObject))
							{
								Building component = gameObject.GetComponent<Building>();
								if (!(component == null))
								{
									IHaveUtilityNetworkMgr component2 = component.Def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>();
									if (!component2.IsNullOrDestroyed())
									{
										UtilityConnections connections = component2.GetNetworkManager().GetConnections(num, false);
										UtilityConnections utilityConnections = (UtilityConnections)0;
										if ((connections & UtilityConnections.Left) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, -1, 0))
										{
											utilityConnections |= UtilityConnections.Left;
										}
										if ((connections & UtilityConnections.Right) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 1, 0))
										{
											utilityConnections |= UtilityConnections.Right;
										}
										if ((connections & UtilityConnections.Up) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 0, 1))
										{
											utilityConnections |= UtilityConnections.Up;
										}
										if ((connections & UtilityConnections.Down) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 0, -1))
										{
											utilityConnections |= UtilityConnections.Down;
										}
										if (utilityConnections > (UtilityConnections)0)
										{
											action(num, gameObject, component2, utilityConnections);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x0014AE58 File Offset: 0x00149058
	private bool IsInsideRegion(Vector2I min, Vector2I max, int cell, int xoff, int yoff)
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.OffsetCell(cell, xoff, yoff), out num, out num2);
		return num >= min.x && num < max.x && num2 >= min.y && num2 < max.y;
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x0014AEA0 File Offset: 0x001490A0
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		if (!base.Dragging)
		{
			return;
		}
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		if (this.singleDisconnectMode)
		{
			cursorPos = base.SnapToLine(cursorPos);
		}
		int num = Grid.PosToCell(cursorPos);
		if (this.lastRefreshedCell == num)
		{
			return;
		}
		this.lastRefreshedCell = num;
		this.ClearVisualizers();
		this.RunOnRegion(this.downPos, cursorPos, new Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections>(this.VisualizeAction));
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x0014AF18 File Offset: 0x00149118
	private GameObject InstantiateDisconnectVis()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.singleDisconnectMode ? this.disconnectVisSingleModePrefab : this.disconnectVisMultiModePrefab, Grid.SceneLayer.FXFront, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x0014AF40 File Offset: 0x00149140
	private void VisualizeAction(int cell, GameObject objectOnCell, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
	{
		if ((removeConnections & UtilityConnections.Down) != (UtilityConnections)0)
		{
			this.CreateVisualizer(cell, Grid.CellBelow(cell), true);
		}
		if ((removeConnections & UtilityConnections.Right) != (UtilityConnections)0)
		{
			this.CreateVisualizer(cell, Grid.CellRight(cell), false);
		}
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x0014AF6C File Offset: 0x0014916C
	private void CreateVisualizer(int cell1, int cell2, bool rotate)
	{
		foreach (DisconnectTool.VisData visData in this.visualizersInUse)
		{
			if (visData.Equals(cell1, cell2))
			{
				return;
			}
		}
		Vector3 a = Grid.CellToPosCCC(cell1, Grid.SceneLayer.FXFront);
		Vector3 b = Grid.CellToPosCCC(cell2, Grid.SceneLayer.FXFront);
		GameObject instance = this.disconnectVisPool.GetInstance();
		instance.transform.rotation = Quaternion.Euler(0f, 0f, (float)(rotate ? 90 : 0));
		instance.transform.SetPosition(Vector3.Lerp(a, b, 0.5f));
		instance.SetActive(true);
		this.visualizersInUse.Add(new DisconnectTool.VisData(cell1, cell2, instance));
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x0014B03C File Offset: 0x0014923C
	private void ClearVisualizers()
	{
		foreach (DisconnectTool.VisData visData in this.visualizersInUse)
		{
			visData.go.SetActive(false);
			this.disconnectVisPool.ReleaseInstance(visData.go);
		}
		this.visualizersInUse.Clear();
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x0014B0B0 File Offset: 0x001492B0
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		this.ClearVisualizers();
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x0014B0BF File Offset: 0x001492BF
	protected override string GetConfirmSound()
	{
		return "OutletDisconnected";
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x0014B0C6 File Offset: 0x001492C6
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x0014B0D0 File Offset: 0x001492D0
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x0400273B RID: 10043
	[SerializeField]
	private GameObject disconnectVisSingleModePrefab;

	// Token: 0x0400273C RID: 10044
	[SerializeField]
	private GameObject disconnectVisMultiModePrefab;

	// Token: 0x0400273D RID: 10045
	private GameObjectPool disconnectVisPool;

	// Token: 0x0400273E RID: 10046
	private List<DisconnectTool.VisData> visualizersInUse = new List<DisconnectTool.VisData>();

	// Token: 0x0400273F RID: 10047
	private int lastRefreshedCell;

	// Token: 0x04002740 RID: 10048
	private bool singleDisconnectMode = true;

	// Token: 0x04002741 RID: 10049
	public static DisconnectTool Instance;

	// Token: 0x020015EC RID: 5612
	public struct VisData
	{
		// Token: 0x060088D9 RID: 35033 RVA: 0x0030FEE6 File Offset: 0x0030E0E6
		public VisData(int cell1, int cell2, GameObject go)
		{
			this.cell1 = cell1;
			this.cell2 = cell2;
			this.go = go;
		}

		// Token: 0x060088DA RID: 35034 RVA: 0x0030FEFD File Offset: 0x0030E0FD
		public bool Equals(int cell1, int cell2)
		{
			return (this.cell1 == cell1 && this.cell2 == cell2) || (this.cell1 == cell2 && this.cell2 == cell1);
		}

		// Token: 0x040069FB RID: 27131
		public readonly int cell1;

		// Token: 0x040069FC RID: 27132
		public readonly int cell2;

		// Token: 0x040069FD RID: 27133
		public GameObject go;
	}
}
