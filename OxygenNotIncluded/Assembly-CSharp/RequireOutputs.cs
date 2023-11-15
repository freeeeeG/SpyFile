using System;
using UnityEngine;

// Token: 0x02000926 RID: 2342
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireOutputs")]
public class RequireOutputs : KMonoBehaviour
{
	// Token: 0x060043ED RID: 17389 RVA: 0x0017CDF4 File Offset: 0x0017AFF4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ScenePartitionerLayer scenePartitionerLayer = null;
		Building component = base.GetComponent<Building>();
		this.utilityCell = component.GetUtilityOutputCell();
		this.conduitType = component.Def.OutputConduitType;
		switch (component.Def.OutputConduitType)
		{
		case ConduitType.Gas:
			scenePartitionerLayer = GameScenePartitioner.Instance.gasConduitsLayer;
			break;
		case ConduitType.Liquid:
			scenePartitionerLayer = GameScenePartitioner.Instance.liquidConduitsLayer;
			break;
		case ConduitType.Solid:
			scenePartitionerLayer = GameScenePartitioner.Instance.solidConduitsLayer;
			break;
		}
		this.UpdateConnectionState(true);
		this.UpdatePipeRoomState(true);
		if (scenePartitionerLayer != null)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("RequireOutputs", base.gameObject, this.utilityCell, scenePartitionerLayer, delegate(object data)
			{
				this.UpdateConnectionState(false);
			});
		}
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.UpdatePipeState), ConduitFlowPriority.First);
	}

	// Token: 0x060043EE RID: 17390 RVA: 0x0017CECC File Offset: 0x0017B0CC
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		IConduitFlow conduitFlow = this.GetConduitFlow();
		if (conduitFlow != null)
		{
			conduitFlow.RemoveConduitUpdater(new Action<float>(this.UpdatePipeState));
		}
		base.OnCleanUp();
	}

	// Token: 0x060043EF RID: 17391 RVA: 0x0017CF0C File Offset: 0x0017B10C
	private void UpdateConnectionState(bool force_update = false)
	{
		this.connected = this.IsConnected(this.utilityCell);
		if (this.connected != this.previouslyConnected || force_update)
		{
			this.operational.SetFlag(RequireOutputs.outputConnectedFlag, this.connected);
			this.previouslyConnected = this.connected;
			StatusItem status_item = null;
			switch (this.conduitType)
			{
			case ConduitType.Gas:
				status_item = Db.Get().BuildingStatusItems.NeedGasOut;
				break;
			case ConduitType.Liquid:
				status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
				break;
			case ConduitType.Solid:
				status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
				break;
			}
			this.hasPipeGuid = this.selectable.ToggleStatusItem(status_item, this.hasPipeGuid, !this.connected, this);
		}
	}

	// Token: 0x060043F0 RID: 17392 RVA: 0x0017CFDC File Offset: 0x0017B1DC
	private bool OutputPipeIsEmpty()
	{
		if (this.ignoreFullPipe)
		{
			return true;
		}
		bool result = true;
		if (this.connected)
		{
			result = this.GetConduitFlow().IsConduitEmpty(this.utilityCell);
		}
		return result;
	}

	// Token: 0x060043F1 RID: 17393 RVA: 0x0017D010 File Offset: 0x0017B210
	private void UpdatePipeState(float dt)
	{
		this.UpdatePipeRoomState(false);
	}

	// Token: 0x060043F2 RID: 17394 RVA: 0x0017D01C File Offset: 0x0017B21C
	private void UpdatePipeRoomState(bool force_update = false)
	{
		bool flag = this.OutputPipeIsEmpty();
		if (flag != this.previouslyHadRoom || force_update)
		{
			this.operational.SetFlag(RequireOutputs.pipesHaveRoomFlag, flag);
			this.previouslyHadRoom = flag;
			StatusItem status_item = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
			if (this.conduitType == ConduitType.Solid)
			{
				status_item = Db.Get().BuildingStatusItems.SolidConduitBlockedMultiples;
			}
			this.pipeBlockedGuid = this.selectable.ToggleStatusItem(status_item, this.pipeBlockedGuid, !flag, null);
		}
	}

	// Token: 0x060043F3 RID: 17395 RVA: 0x0017D0A0 File Offset: 0x0017B2A0
	private IConduitFlow GetConduitFlow()
	{
		switch (this.conduitType)
		{
		case ConduitType.Gas:
			return Game.Instance.gasConduitFlow;
		case ConduitType.Liquid:
			return Game.Instance.liquidConduitFlow;
		case ConduitType.Solid:
			return Game.Instance.solidConduitFlow;
		default:
			global::Debug.LogWarning("GetConduitFlow() called with unexpected conduitType: " + this.conduitType.ToString());
			return null;
		}
	}

	// Token: 0x060043F4 RID: 17396 RVA: 0x0017D10C File Offset: 0x0017B30C
	private bool IsConnected(int cell)
	{
		return RequireOutputs.IsConnected(cell, this.conduitType);
	}

	// Token: 0x060043F5 RID: 17397 RVA: 0x0017D11C File Offset: 0x0017B31C
	public static bool IsConnected(int cell, ConduitType conduitType)
	{
		ObjectLayer layer = ObjectLayer.NumLayers;
		switch (conduitType)
		{
		case ConduitType.Gas:
			layer = ObjectLayer.GasConduit;
			break;
		case ConduitType.Liquid:
			layer = ObjectLayer.LiquidConduit;
			break;
		case ConduitType.Solid:
			layer = ObjectLayer.SolidConduit;
			break;
		}
		GameObject gameObject = Grid.Objects[cell, (int)layer];
		return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
	}

	// Token: 0x04002D12 RID: 11538
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002D13 RID: 11539
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002D14 RID: 11540
	public bool ignoreFullPipe;

	// Token: 0x04002D15 RID: 11541
	private int utilityCell;

	// Token: 0x04002D16 RID: 11542
	private ConduitType conduitType;

	// Token: 0x04002D17 RID: 11543
	private static readonly Operational.Flag outputConnectedFlag = new Operational.Flag("output_connected", Operational.Flag.Type.Requirement);

	// Token: 0x04002D18 RID: 11544
	private static readonly Operational.Flag pipesHaveRoomFlag = new Operational.Flag("pipesHaveRoom", Operational.Flag.Type.Requirement);

	// Token: 0x04002D19 RID: 11545
	private bool previouslyConnected = true;

	// Token: 0x04002D1A RID: 11546
	private bool previouslyHadRoom = true;

	// Token: 0x04002D1B RID: 11547
	private bool connected;

	// Token: 0x04002D1C RID: 11548
	private Guid hasPipeGuid;

	// Token: 0x04002D1D RID: 11549
	private Guid pipeBlockedGuid;

	// Token: 0x04002D1E RID: 11550
	private HandleVector<int>.Handle partitionerEntry;
}
