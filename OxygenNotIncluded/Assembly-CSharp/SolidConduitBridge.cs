using System;
using UnityEngine;

// Token: 0x0200068D RID: 1677
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitBridge")]
public class SolidConduitBridge : ConduitBridgeBase
{
	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06002CEA RID: 11498 RVA: 0x000EE8CB File Offset: 0x000ECACB
	public bool IsDispensing
	{
		get
		{
			return this.dispensing;
		}
	}

	// Token: 0x06002CEB RID: 11499 RVA: 0x000EE8D4 File Offset: 0x000ECAD4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		SolidConduit.GetFlowManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	// Token: 0x06002CEC RID: 11500 RVA: 0x000EE91D File Offset: 0x000ECB1D
	protected override void OnCleanUp()
	{
		SolidConduit.GetFlowManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06002CED RID: 11501 RVA: 0x000EE93C File Offset: 0x000ECB3C
	private void ConduitUpdate(float dt)
	{
		this.dispensing = false;
		float num = 0f;
		if (this.operational && !this.operational.IsOperational)
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		if (flowManager.IsConduitFull(this.inputCell) && flowManager.IsConduitEmpty(this.outputCell))
		{
			Pickupable pickupable = flowManager.GetPickupable(flowManager.GetContents(this.inputCell).pickupableHandle);
			if (pickupable == null)
			{
				flowManager.RemovePickupable(this.inputCell);
				base.SendEmptyOnMassTransfer();
				return;
			}
			float num2 = pickupable.PrimaryElement.Mass;
			if (this.desiredMassTransfer != null)
			{
				num2 = this.desiredMassTransfer(dt, pickupable.PrimaryElement.Element.id, pickupable.PrimaryElement.Mass, pickupable.PrimaryElement.Temperature, pickupable.PrimaryElement.DiseaseIdx, pickupable.PrimaryElement.DiseaseCount, pickupable);
			}
			if (num2 == 0f)
			{
				base.SendEmptyOnMassTransfer();
				return;
			}
			if (num2 < pickupable.PrimaryElement.Mass)
			{
				Pickupable pickupable2 = pickupable.Take(num2);
				flowManager.AddPickupable(this.outputCell, pickupable2);
				this.dispensing = true;
				num = pickupable2.PrimaryElement.Mass;
				if (this.OnMassTransfer != null)
				{
					this.OnMassTransfer(pickupable2.PrimaryElement.ElementID, num, pickupable2.PrimaryElement.Temperature, pickupable2.PrimaryElement.DiseaseIdx, pickupable2.PrimaryElement.DiseaseCount, pickupable2);
				}
			}
			else
			{
				Pickupable pickupable3 = flowManager.RemovePickupable(this.inputCell);
				if (pickupable3)
				{
					flowManager.AddPickupable(this.outputCell, pickupable3);
					this.dispensing = true;
					num = pickupable3.PrimaryElement.Mass;
					if (this.OnMassTransfer != null)
					{
						this.OnMassTransfer(pickupable3.PrimaryElement.ElementID, num, pickupable3.PrimaryElement.Temperature, pickupable3.PrimaryElement.DiseaseIdx, pickupable3.PrimaryElement.DiseaseCount, pickupable3);
					}
				}
			}
		}
		if (num == 0f)
		{
			base.SendEmptyOnMassTransfer();
		}
	}

	// Token: 0x04001A72 RID: 6770
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001A73 RID: 6771
	private int inputCell;

	// Token: 0x04001A74 RID: 6772
	private int outputCell;

	// Token: 0x04001A75 RID: 6773
	private bool dispensing;
}
