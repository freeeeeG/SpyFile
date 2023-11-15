using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D9 RID: 1497
[AddComponentMenu("KMonoBehaviour/scripts/ConduitBridge")]
public class ConduitBridge : ConduitBridgeBase, IBridgedNetworkItem
{
	// Token: 0x0600252B RID: 9515 RVA: 0x000CACA9 File Offset: 0x000C8EA9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.accumulator = Game.Instance.accumulators.Add("Flow", this);
	}

	// Token: 0x0600252C RID: 9516 RVA: 0x000CACCC File Offset: 0x000C8ECC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.type).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x000CAD1B File Offset: 0x000C8F1B
	protected override void OnCleanUp()
	{
		Conduit.GetFlowManager(this.type).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x000CAD58 File Offset: 0x000C8F58
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.type);
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
		float num = contents.mass;
		if (this.desiredMassTransfer != null)
		{
			num = this.desiredMassTransfer(dt, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount, null);
		}
		if (num > 0f)
		{
			int disease_count = (int)(num / contents.mass * (float)contents.diseaseCount);
			float num2 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
			if (num2 <= 0f)
			{
				base.SendEmptyOnMassTransfer();
				return;
			}
			flowManager.RemoveElement(this.inputCell, num2);
			Game.Instance.accumulators.Accumulate(this.accumulator, contents.mass);
			if (this.OnMassTransfer != null)
			{
				this.OnMassTransfer(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, null);
				return;
			}
		}
		else
		{
			base.SendEmptyOnMassTransfer();
		}
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x000CAE8C File Offset: 0x000C908C
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
		UtilityNetwork networkForCell = networkManager.GetNetworkForCell(this.inputCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
		networkForCell = networkManager.GetNetworkForCell(this.outputCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x000CAED4 File Offset: 0x000C90D4
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		bool flag = false;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
		return flag || networks.Contains(networkManager.GetNetworkForCell(this.inputCell)) || networks.Contains(networkManager.GetNetworkForCell(this.outputCell));
	}

	// Token: 0x06002531 RID: 9521 RVA: 0x000CAF1B File Offset: 0x000C911B
	public int GetNetworkCell()
	{
		return this.inputCell;
	}

	// Token: 0x0400154C RID: 5452
	[SerializeField]
	public ConduitType type;

	// Token: 0x0400154D RID: 5453
	private int inputCell;

	// Token: 0x0400154E RID: 5454
	private int outputCell;

	// Token: 0x0400154F RID: 5455
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
}
