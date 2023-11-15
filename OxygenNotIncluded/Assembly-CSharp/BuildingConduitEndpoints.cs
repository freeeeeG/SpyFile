using System;
using UnityEngine;

// Token: 0x020005A8 RID: 1448
[AddComponentMenu("KMonoBehaviour/scripts/BuildingConduitEndpoints")]
public class BuildingConduitEndpoints : KMonoBehaviour
{
	// Token: 0x06002368 RID: 9064 RVA: 0x000C219F File Offset: 0x000C039F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.AddEndpoint();
	}

	// Token: 0x06002369 RID: 9065 RVA: 0x000C21AD File Offset: 0x000C03AD
	protected override void OnCleanUp()
	{
		this.RemoveEndPoint();
		base.OnCleanUp();
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x000C21BC File Offset: 0x000C03BC
	public void RemoveEndPoint()
	{
		if (this.itemInput != null)
		{
			if (this.itemInput.ConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemInput.Cell, this.itemInput, true);
			}
			else
			{
				Conduit.GetNetworkManager(this.itemInput.ConduitType).RemoveFromNetworks(this.itemInput.Cell, this.itemInput, true);
			}
			this.itemInput = null;
		}
		if (this.itemOutput != null)
		{
			if (this.itemOutput.ConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemOutput.Cell, this.itemOutput, true);
			}
			else
			{
				Conduit.GetNetworkManager(this.itemOutput.ConduitType).RemoveFromNetworks(this.itemOutput.Cell, this.itemOutput, true);
			}
			this.itemOutput = null;
		}
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x000C2298 File Offset: 0x000C0498
	public void AddEndpoint()
	{
		Building component = base.GetComponent<Building>();
		BuildingDef def = component.Def;
		this.RemoveEndPoint();
		if (def.InputConduitType != ConduitType.None)
		{
			int utilityInputCell = component.GetUtilityInputCell();
			this.itemInput = new FlowUtilityNetwork.NetworkItem(def.InputConduitType, Endpoint.Sink, utilityInputCell, base.gameObject);
			if (def.InputConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.AddToNetworks(utilityInputCell, this.itemInput, true);
			}
			else
			{
				Conduit.GetNetworkManager(def.InputConduitType).AddToNetworks(utilityInputCell, this.itemInput, true);
			}
		}
		if (def.OutputConduitType != ConduitType.None)
		{
			int utilityOutputCell = component.GetUtilityOutputCell();
			this.itemOutput = new FlowUtilityNetwork.NetworkItem(def.OutputConduitType, Endpoint.Source, utilityOutputCell, base.gameObject);
			if (def.OutputConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.AddToNetworks(utilityOutputCell, this.itemOutput, true);
				return;
			}
			Conduit.GetNetworkManager(def.OutputConduitType).AddToNetworks(utilityOutputCell, this.itemOutput, true);
		}
	}

	// Token: 0x04001434 RID: 5172
	private FlowUtilityNetwork.NetworkItem itemInput;

	// Token: 0x04001435 RID: 5173
	private FlowUtilityNetwork.NetworkItem itemOutput;
}
