using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200083E RID: 2110
[SerializationConfig(MemberSerialization.OptIn)]
public class LeadSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06003D68 RID: 15720 RVA: 0x00154D34 File Offset: 0x00152F34
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LeadSuitTank>(-1617557748, LeadSuitTank.OnEquippedDelegate);
		base.Subscribe<LeadSuitTank>(-170173755, LeadSuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06003D69 RID: 15721 RVA: 0x00154D5E File Offset: 0x00152F5E
	public float PercentFull()
	{
		return this.batteryCharge;
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x00154D66 File Offset: 0x00152F66
	public bool IsEmpty()
	{
		return this.batteryCharge <= 0f;
	}

	// Token: 0x06003D6B RID: 15723 RVA: 0x00154D78 File Offset: 0x00152F78
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06003D6C RID: 15724 RVA: 0x00154D8A File Offset: 0x00152F8A
	public bool NeedsRecharging()
	{
		return this.PercentFull() <= 0.25f;
	}

	// Token: 0x06003D6D RID: 15725 RVA: 0x00154D9C File Offset: 0x00152F9C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.LEADSUIT_BATTERY, GameUtil.GetFormattedPercent(this.PercentFull() * 100f, GameUtil.TimeSlice.None));
		list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x06003D6E RID: 15726 RVA: 0x00154DE0 File Offset: 0x00152FE0
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitBatteryDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		this.leadSuitMonitor = new LeadSuitMonitor.Instance(this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
		this.leadSuitMonitor.StartSM();
		if (this.NeedsRecharging())
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.SuitBatteryLow);
		}
	}

	// Token: 0x06003D6F RID: 15727 RVA: 0x00154E58 File Offset: 0x00153058
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.SuitBatteryLow);
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.SuitBatteryOut);
			NameDisplayScreen.Instance.SetSuitBatteryDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), null, false);
		}
		if (this.leadSuitMonitor != null)
		{
			this.leadSuitMonitor.StopSM("Removed leadsuit tank");
			this.leadSuitMonitor = null;
		}
	}

	// Token: 0x0400280A RID: 10250
	[Serialize]
	public float batteryCharge = 1f;

	// Token: 0x0400280B RID: 10251
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x0400280C RID: 10252
	public float batteryDuration = 200f;

	// Token: 0x0400280D RID: 10253
	public float coolingOperationalTemperature = 333.15f;

	// Token: 0x0400280E RID: 10254
	public Tag coolantTag;

	// Token: 0x0400280F RID: 10255
	private LeadSuitMonitor.Instance leadSuitMonitor;

	// Token: 0x04002810 RID: 10256
	private static readonly EventSystem.IntraObjectHandler<LeadSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<LeadSuitTank>(delegate(LeadSuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04002811 RID: 10257
	private static readonly EventSystem.IntraObjectHandler<LeadSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<LeadSuitTank>(delegate(LeadSuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
