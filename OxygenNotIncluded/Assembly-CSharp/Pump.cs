using System;
using UnityEngine;

// Token: 0x02000675 RID: 1653
[AddComponentMenu("KMonoBehaviour/scripts/Pump")]
public class Pump : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06002BD7 RID: 11223 RVA: 0x000E8CAF File Offset: 0x000E6EAF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.consumer.EnableConsumption(false);
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x000E8CC3 File Offset: 0x000E6EC3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.elapsedTime = 0f;
		this.pumpable = this.UpdateOperational();
		this.dispenser.GetConduitManager().AddConduitUpdater(new Action<float>(this.OnConduitUpdate), ConduitFlowPriority.LastPostUpdate);
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x000E8D00 File Offset: 0x000E6F00
	protected override void OnCleanUp()
	{
		this.dispenser.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.OnConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x000E8D24 File Offset: 0x000E6F24
	public void Sim1000ms(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime >= 1f)
		{
			this.pumpable = this.UpdateOperational();
			this.elapsedTime = 0f;
		}
		if (this.operational.IsOperational && this.pumpable)
		{
			this.operational.SetActive(true, false);
			return;
		}
		this.operational.SetActive(false, false);
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x000E8D94 File Offset: 0x000E6F94
	private bool UpdateOperational()
	{
		Element.State state = Element.State.Vacuum;
		ConduitType conduitType = this.dispenser.conduitType;
		if (conduitType != ConduitType.Gas)
		{
			if (conduitType == ConduitType.Liquid)
			{
				state = Element.State.Liquid;
			}
		}
		else
		{
			state = Element.State.Gas;
		}
		bool flag = this.IsPumpable(state, (int)this.consumer.consumptionRadius);
		StatusItem status_item = (state == Element.State.Gas) ? Db.Get().BuildingStatusItems.NoGasElementToPump : Db.Get().BuildingStatusItems.NoLiquidElementToPump;
		this.noElementStatusGuid = this.selectable.ToggleStatusItem(status_item, this.noElementStatusGuid, !flag, null);
		this.operational.SetFlag(Pump.PumpableFlag, !this.storage.IsFull() && flag);
		return flag;
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x000E8E38 File Offset: 0x000E7038
	private bool IsPumpable(Element.State expected_state, int radius)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		for (int i = 0; i < (int)this.consumer.consumptionRadius; i++)
		{
			for (int j = 0; j < (int)this.consumer.consumptionRadius; j++)
			{
				int num2 = num + j + Grid.WidthInCells * i;
				if (Grid.Element[num2].IsState(expected_state))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x000E8EA0 File Offset: 0x000E70A0
	private void OnConduitUpdate(float dt)
	{
		this.conduitBlockedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.ConduitBlocked, this.conduitBlockedStatusGuid, this.dispenser.blocked, null);
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06002BDE RID: 11230 RVA: 0x000E8ED4 File Offset: 0x000E70D4
	public ConduitType conduitType
	{
		get
		{
			return this.dispenser.conduitType;
		}
	}

	// Token: 0x040019B9 RID: 6585
	public static readonly Operational.Flag PumpableFlag = new Operational.Flag("vent", Operational.Flag.Type.Requirement);

	// Token: 0x040019BA RID: 6586
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040019BB RID: 6587
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040019BC RID: 6588
	[MyCmpGet]
	private ElementConsumer consumer;

	// Token: 0x040019BD RID: 6589
	[MyCmpGet]
	private ConduitDispenser dispenser;

	// Token: 0x040019BE RID: 6590
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040019BF RID: 6591
	private const float OperationalUpdateInterval = 1f;

	// Token: 0x040019C0 RID: 6592
	private float elapsedTime;

	// Token: 0x040019C1 RID: 6593
	private bool pumpable;

	// Token: 0x040019C2 RID: 6594
	private Guid conduitBlockedStatusGuid;

	// Token: 0x040019C3 RID: 6595
	private Guid noElementStatusGuid;
}
