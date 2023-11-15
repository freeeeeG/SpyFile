using System;
using UnityEngine;

// Token: 0x020005DB RID: 1499
[AddComponentMenu("KMonoBehaviour/scripts/ConduitOverflow")]
public class ConduitOverflow : KMonoBehaviour, ISecondaryOutput
{
	// Token: 0x06002535 RID: 9525 RVA: 0x000CAF68 File Offset: 0x000C9168
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = component.GetRotatedOffset(this.portInfo.offset);
		int cell2 = Grid.OffsetCell(cell, rotatedOffset);
		Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.secondaryOutput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, cell2, base.gameObject);
		networkManager.AddToNetworks(this.secondaryOutput.Cell, this.secondaryOutput, true);
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x000CB02C File Offset: 0x000C922C
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryOutput.Cell, this.secondaryOutput, true);
		Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06002537 RID: 9527 RVA: 0x000CB088 File Offset: 0x000C9288
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
		if (!flowManager.HasConduit(this.inputCell))
		{
			return;
		}
		ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
		if (contents.mass <= 0f)
		{
			return;
		}
		int cell = this.outputCell;
		ConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		if (contents2.mass > 0f)
		{
			cell = this.secondaryOutput.Cell;
			contents2 = flowManager.GetContents(cell);
		}
		if (contents2.mass <= 0f)
		{
			float num = flowManager.AddElement(cell, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
			if (num > 0f)
			{
				flowManager.RemoveElement(this.inputCell, num);
			}
		}
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x000CB150 File Offset: 0x000C9350
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000CB160 File Offset: 0x000C9360
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		return this.portInfo.offset;
	}

	// Token: 0x04001552 RID: 5458
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04001553 RID: 5459
	private int inputCell;

	// Token: 0x04001554 RID: 5460
	private int outputCell;

	// Token: 0x04001555 RID: 5461
	private FlowUtilityNetwork.NetworkItem secondaryOutput;
}
