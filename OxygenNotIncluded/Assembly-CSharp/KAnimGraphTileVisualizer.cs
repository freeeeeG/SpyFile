using System;
using KSerialization;
using UnityEngine;

// Token: 0x020004BF RID: 1215
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGraphTileVisualizer")]
public class KAnimGraphTileVisualizer : KMonoBehaviour, ISaveLoadable, IUtilityItem
{
	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06001B94 RID: 7060 RVA: 0x000939CF File Offset: 0x00091BCF
	// (set) Token: 0x06001B95 RID: 7061 RVA: 0x000939D7 File Offset: 0x00091BD7
	public UtilityConnections Connections
	{
		get
		{
			return this._connections;
		}
		set
		{
			this._connections = value;
			base.Trigger(-1041684577, this._connections);
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06001B96 RID: 7062 RVA: 0x000939F8 File Offset: 0x00091BF8
	public IUtilityNetworkMgr ConnectionManager
	{
		get
		{
			switch (this.connectionSource)
			{
			case KAnimGraphTileVisualizer.ConnectionSource.Gas:
				return Game.Instance.gasConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
				return Game.Instance.liquidConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
				return Game.Instance.electricalConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Logic:
				return Game.Instance.logicCircuitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Tube:
				return Game.Instance.travelTubeSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Solid:
				return Game.Instance.solidConduitSystem;
			default:
				return null;
			}
		}
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x00093A70 File Offset: 0x00091C70
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.connectionManager = this.ConnectionManager;
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.connectionManager.SetConnections(this.Connections, cell, this.isPhysicalBuilding);
		Building component = base.GetComponent<Building>();
		TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x00093ADC File Offset: 0x00091CDC
	protected override void OnCleanUp()
	{
		if (this.connectionManager != null && !this.skipCleanup)
		{
			this.skipRefresh = true;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.connectionManager.ClearCell(cell, this.isPhysicalBuilding);
			Building component = base.GetComponent<Building>();
			TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
		}
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x00093B48 File Offset: 0x00091D48
	[ContextMenu("Refresh")]
	public void Refresh()
	{
		if (this.connectionManager == null || this.skipRefresh)
		{
			return;
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.Connections = this.connectionManager.GetConnections(cell, this.isPhysicalBuilding);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			string text = this.connectionManager.GetVisualizerString(cell);
			if (base.GetComponent<BuildingUnderConstruction>() != null && component.HasAnimation(text + "_place"))
			{
				text += "_place";
			}
			if (text != null && text != "")
			{
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x00093C08 File Offset: 0x00091E08
	public int GetNetworkID()
	{
		UtilityNetwork network = this.GetNetwork();
		if (network == null)
		{
			return -1;
		}
		return network.id;
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x00093C28 File Offset: 0x00091E28
	private UtilityNetwork GetNetwork()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.connectionManager.GetNetworkForDirection(cell, Direction.None);
	}

	// Token: 0x06001B9C RID: 7068 RVA: 0x00093C54 File Offset: 0x00091E54
	public UtilityNetwork GetNetworkForDirection(Direction d)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.connectionManager.GetNetworkForDirection(cell, d);
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x00093C80 File Offset: 0x00091E80
	public void UpdateConnections(UtilityConnections new_connections)
	{
		this._connections = new_connections;
		if (this.connectionManager != null)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.connectionManager.SetConnections(new_connections, cell, this.isPhysicalBuilding);
		}
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x00093CC0 File Offset: 0x00091EC0
	public KAnimGraphTileVisualizer GetNeighbour(Direction d)
	{
		KAnimGraphTileVisualizer result = null;
		Vector2I vector2I;
		Grid.PosToXY(base.transform.GetPosition(), out vector2I);
		int num = -1;
		switch (d)
		{
		case Direction.Up:
			if (vector2I.y < Grid.HeightInCells - 1)
			{
				num = Grid.XYToCell(vector2I.x, vector2I.y + 1);
			}
			break;
		case Direction.Right:
			if (vector2I.x < Grid.WidthInCells - 1)
			{
				num = Grid.XYToCell(vector2I.x + 1, vector2I.y);
			}
			break;
		case Direction.Down:
			if (vector2I.y > 0)
			{
				num = Grid.XYToCell(vector2I.x, vector2I.y - 1);
			}
			break;
		case Direction.Left:
			if (vector2I.x > 0)
			{
				num = Grid.XYToCell(vector2I.x - 1, vector2I.y);
			}
			break;
		}
		if (num != -1)
		{
			ObjectLayer layer;
			switch (this.connectionSource)
			{
			case KAnimGraphTileVisualizer.ConnectionSource.Gas:
				layer = ObjectLayer.GasConduitTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
				layer = ObjectLayer.LiquidConduitTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
				layer = ObjectLayer.WireTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Logic:
				layer = ObjectLayer.LogicWireTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Tube:
				layer = ObjectLayer.TravelTubeTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Solid:
				layer = ObjectLayer.SolidConduitTile;
				break;
			default:
				throw new ArgumentNullException("wtf");
			}
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (gameObject != null)
			{
				result = gameObject.GetComponent<KAnimGraphTileVisualizer>();
			}
		}
		return result;
	}

	// Token: 0x04000F58 RID: 3928
	[Serialize]
	private UtilityConnections _connections;

	// Token: 0x04000F59 RID: 3929
	public bool isPhysicalBuilding;

	// Token: 0x04000F5A RID: 3930
	public bool skipCleanup;

	// Token: 0x04000F5B RID: 3931
	public bool skipRefresh;

	// Token: 0x04000F5C RID: 3932
	public KAnimGraphTileVisualizer.ConnectionSource connectionSource;

	// Token: 0x04000F5D RID: 3933
	[NonSerialized]
	public IUtilityNetworkMgr connectionManager;

	// Token: 0x0200116D RID: 4461
	public enum ConnectionSource
	{
		// Token: 0x04005C40 RID: 23616
		Gas,
		// Token: 0x04005C41 RID: 23617
		Liquid,
		// Token: 0x04005C42 RID: 23618
		Electrical,
		// Token: 0x04005C43 RID: 23619
		Logic,
		// Token: 0x04005C44 RID: 23620
		Tube,
		// Token: 0x04005C45 RID: 23621
		Solid
	}
}
