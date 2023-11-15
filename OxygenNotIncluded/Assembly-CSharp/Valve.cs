using System;
using KSerialization;
using UnityEngine;

// Token: 0x020006B3 RID: 1715
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Valve")]
public class Valve : Workable, ISaveLoadable
{
	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06002EA0 RID: 11936 RVA: 0x000F65F1 File Offset: 0x000F47F1
	public float QueuedMaxFlow
	{
		get
		{
			if (this.chore == null)
			{
				return -1f;
			}
			return this.desiredFlow;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06002EA1 RID: 11937 RVA: 0x000F6607 File Offset: 0x000F4807
	public float DesiredFlow
	{
		get
		{
			return this.desiredFlow;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06002EA2 RID: 11938 RVA: 0x000F660F File Offset: 0x000F480F
	public float MaxFlow
	{
		get
		{
			return this.valveBase.MaxFlow;
		}
	}

	// Token: 0x06002EA3 RID: 11939 RVA: 0x000F661C File Offset: 0x000F481C
	private void OnCopySettings(object data)
	{
		Valve component = ((GameObject)data).GetComponent<Valve>();
		if (component != null)
		{
			this.ChangeFlow(component.desiredFlow);
		}
	}

	// Token: 0x06002EA4 RID: 11940 RVA: 0x000F664C File Offset: 0x000F484C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.synchronizeAnims = false;
		this.valveBase.CurrentFlow = this.valveBase.MaxFlow;
		this.desiredFlow = this.valveBase.MaxFlow;
		base.Subscribe<Valve>(-905833192, Valve.OnCopySettingsDelegate);
	}

	// Token: 0x06002EA5 RID: 11941 RVA: 0x000F66A9 File Offset: 0x000F48A9
	protected override void OnSpawn()
	{
		this.ChangeFlow(this.desiredFlow);
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x000F66C8 File Offset: 0x000F48C8
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06002EA7 RID: 11943 RVA: 0x000F66DC File Offset: 0x000F48DC
	public void ChangeFlow(float amount)
	{
		this.desiredFlow = Mathf.Clamp(amount, 0f, this.valveBase.MaxFlow);
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.PumpingLiquidOrGas, this.desiredFlow >= 0f, this.valveBase.AccumulatorHandle);
		if (DebugHandler.InstantBuildMode)
		{
			this.UpdateFlow();
			return;
		}
		if (this.desiredFlow == this.valveBase.CurrentFlow)
		{
			if (this.chore != null)
			{
				this.chore.Cancel("desiredFlow == currentFlow");
				this.chore = null;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
			return;
		}
		if (this.chore == null)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.ValveRequest, this);
			component.AddStatusItem(Db.Get().BuildingStatusItems.PendingWork, this);
			this.chore = new WorkChore<Valve>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			return;
		}
	}

	// Token: 0x06002EA8 RID: 11944 RVA: 0x000F6817 File Offset: 0x000F4A17
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		this.UpdateFlow();
	}

	// Token: 0x06002EA9 RID: 11945 RVA: 0x000F6828 File Offset: 0x000F4A28
	public void UpdateFlow()
	{
		this.valveBase.CurrentFlow = this.desiredFlow;
		this.valveBase.UpdateAnim();
		if (this.chore != null)
		{
			this.chore.Cancel("forced complete");
		}
		this.chore = null;
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
	}

	// Token: 0x04001B99 RID: 7065
	[MyCmpReq]
	private ValveBase valveBase;

	// Token: 0x04001B9A RID: 7066
	[Serialize]
	private float desiredFlow = 0.5f;

	// Token: 0x04001B9B RID: 7067
	private Chore chore;

	// Token: 0x04001B9C RID: 7068
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001B9D RID: 7069
	private static readonly EventSystem.IntraObjectHandler<Valve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Valve>(delegate(Valve component, object data)
	{
		component.OnCopySettings(data);
	});
}
