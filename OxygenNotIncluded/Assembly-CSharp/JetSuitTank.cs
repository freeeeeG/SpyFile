using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000833 RID: 2099
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/JetSuitTank")]
public class JetSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06003D04 RID: 15620 RVA: 0x00152F02 File Offset: 0x00151102
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.amount = 25f;
		base.Subscribe<JetSuitTank>(-1617557748, JetSuitTank.OnEquippedDelegate);
		base.Subscribe<JetSuitTank>(-170173755, JetSuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06003D05 RID: 15621 RVA: 0x00152F37 File Offset: 0x00151137
	public float PercentFull()
	{
		return this.amount / 25f;
	}

	// Token: 0x06003D06 RID: 15622 RVA: 0x00152F45 File Offset: 0x00151145
	public bool IsEmpty()
	{
		return this.amount <= 0f;
	}

	// Token: 0x06003D07 RID: 15623 RVA: 0x00152F57 File Offset: 0x00151157
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06003D08 RID: 15624 RVA: 0x00152F69 File Offset: 0x00151169
	public bool NeedsRecharging()
	{
		return this.PercentFull() < 0.25f;
	}

	// Token: 0x06003D09 RID: 15625 RVA: 0x00152F78 File Offset: 0x00151178
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.JETSUIT_TANK, GameUtil.GetFormattedMass(this.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x06003D0A RID: 15626 RVA: 0x00152FBC File Offset: 0x001511BC
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		this.jetSuitMonitor = new JetSuitMonitor.Instance(this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
		this.jetSuitMonitor.StartSM();
		if (this.IsEmpty())
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.JetSuitOutOfFuel);
		}
	}

	// Token: 0x06003D0B RID: 15627 RVA: 0x00153034 File Offset: 0x00151234
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.JetSuitOutOfFuel);
			NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), null, false);
			Navigator component = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<Navigator>();
			if (component && component.CurrentNavType == NavType.Hover)
			{
				component.SetCurrentNavType(NavType.Floor);
			}
		}
		if (this.jetSuitMonitor != null)
		{
			this.jetSuitMonitor.StopSM("Removed jetsuit tank");
			this.jetSuitMonitor = null;
		}
	}

	// Token: 0x040027D2 RID: 10194
	[MyCmpGet]
	private ElementEmitter elementConverter;

	// Token: 0x040027D3 RID: 10195
	[Serialize]
	public float amount;

	// Token: 0x040027D4 RID: 10196
	public const float FUEL_CAPACITY = 25f;

	// Token: 0x040027D5 RID: 10197
	public const float FUEL_BURN_RATE = 0.1f;

	// Token: 0x040027D6 RID: 10198
	public const float CO2_EMITTED_PER_FUEL_BURNED = 3f;

	// Token: 0x040027D7 RID: 10199
	public const float EMIT_TEMPERATURE = 473.15f;

	// Token: 0x040027D8 RID: 10200
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x040027D9 RID: 10201
	private JetSuitMonitor.Instance jetSuitMonitor;

	// Token: 0x040027DA RID: 10202
	private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>(delegate(JetSuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x040027DB RID: 10203
	private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>(delegate(JetSuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
