using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009FC RID: 2556
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SuitTank")]
public class SuitTank : KMonoBehaviour, IGameObjectEffectDescriptor, OxygenBreather.IGasProvider
{
	// Token: 0x06004C6E RID: 19566 RVA: 0x001ACA28 File Offset: 0x001AAC28
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SuitTank>(-1617557748, SuitTank.OnEquippedDelegate);
		base.Subscribe<SuitTank>(-170173755, SuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06004C6F RID: 19567 RVA: 0x001ACA54 File Offset: 0x001AAC54
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.amount != 0f)
		{
			this.storage.AddGasChunk(SimHashes.Oxygen, this.amount, base.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false, true);
			this.amount = 0f;
		}
	}

	// Token: 0x06004C70 RID: 19568 RVA: 0x001ACAA9 File Offset: 0x001AACA9
	public float GetTankAmount()
	{
		if (this.storage == null)
		{
			this.storage = base.GetComponent<Storage>();
		}
		return this.storage.GetMassAvailable(this.elementTag);
	}

	// Token: 0x06004C71 RID: 19569 RVA: 0x001ACAD6 File Offset: 0x001AACD6
	public float PercentFull()
	{
		return this.GetTankAmount() / this.capacity;
	}

	// Token: 0x06004C72 RID: 19570 RVA: 0x001ACAE5 File Offset: 0x001AACE5
	public bool IsEmpty()
	{
		return this.GetTankAmount() <= 0f;
	}

	// Token: 0x06004C73 RID: 19571 RVA: 0x001ACAF7 File Offset: 0x001AACF7
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06004C74 RID: 19572 RVA: 0x001ACB09 File Offset: 0x001AAD09
	public bool NeedsRecharging()
	{
		return this.PercentFull() < 0.25f;
	}

	// Token: 0x06004C75 RID: 19573 RVA: 0x001ACB18 File Offset: 0x001AAD18
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.elementTag == GameTags.Breathable)
		{
			string text = this.underwaterSupport ? string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK_UNDERWATER, GameUtil.GetFormattedMass(this.GetTankAmount(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")) : string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK, GameUtil.GetFormattedMass(this.GetTankAmount(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x06004C76 RID: 19574 RVA: 0x001ACB9C File Offset: 0x001AAD9C
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		OxygenBreather component = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<OxygenBreather>();
		if (component != null)
		{
			component.SetGasProvider(this);
			component.AddTag(GameTags.HasSuitTank);
		}
	}

	// Token: 0x06004C77 RID: 19575 RVA: 0x001ACC00 File Offset: 0x001AAE00
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), false);
			OxygenBreather component = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<OxygenBreather>();
			if (component != null)
			{
				component.SetGasProvider(new GasBreatherFromWorldProvider());
				component.RemoveTag(GameTags.HasSuitTank);
			}
		}
	}

	// Token: 0x06004C78 RID: 19576 RVA: 0x001ACC6E File Offset: 0x001AAE6E
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suitSuffocationMonitor = new SuitSuffocationMonitor.Instance(oxygen_breather, this);
		this.suitSuffocationMonitor.StartSM();
	}

	// Token: 0x06004C79 RID: 19577 RVA: 0x001ACC88 File Offset: 0x001AAE88
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suitSuffocationMonitor.StopSM("Removed suit tank");
		this.suitSuffocationMonitor = null;
	}

	// Token: 0x06004C7A RID: 19578 RVA: 0x001ACCA4 File Offset: 0x001AAEA4
	public bool ConsumeGas(OxygenBreather oxygen_breather, float gas_consumed)
	{
		if (this.IsEmpty())
		{
			return false;
		}
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		this.storage.ConsumeAndGetDisease(this.elementTag, gas_consumed, out num, out diseaseInfo, out num2);
		Game.Instance.accumulators.Accumulate(oxygen_breather.O2Accumulator, num);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, -num, oxygen_breather.GetProperName(), null);
		base.Trigger(608245985, base.gameObject);
		return true;
	}

	// Token: 0x06004C7B RID: 19579 RVA: 0x001ACD10 File Offset: 0x001AAF10
	public bool ShouldEmitCO2()
	{
		return !base.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
	}

	// Token: 0x06004C7C RID: 19580 RVA: 0x001ACD25 File Offset: 0x001AAF25
	public bool ShouldStoreCO2()
	{
		return base.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
	}

	// Token: 0x06004C7D RID: 19581 RVA: 0x001ACD37 File Offset: 0x001AAF37
	public bool IsLowOxygen()
	{
		return this.IsEmpty();
	}

	// Token: 0x06004C7E RID: 19582 RVA: 0x001ACD40 File Offset: 0x001AAF40
	[ContextMenu("SetToRefillAmount")]
	public void SetToRefillAmount()
	{
		float tankAmount = this.GetTankAmount();
		float num = 0.25f * this.capacity;
		if (tankAmount > num)
		{
			this.storage.ConsumeIgnoringDisease(this.elementTag, tankAmount - num);
		}
	}

	// Token: 0x06004C7F RID: 19583 RVA: 0x001ACD79 File Offset: 0x001AAF79
	[ContextMenu("Empty")]
	public void Empty()
	{
		this.storage.ConsumeIgnoringDisease(this.elementTag, this.GetTankAmount());
	}

	// Token: 0x06004C80 RID: 19584 RVA: 0x001ACD92 File Offset: 0x001AAF92
	[ContextMenu("Fill Tank")]
	public void FillTank()
	{
		this.Empty();
		this.storage.AddGasChunk(SimHashes.Oxygen, this.capacity, 15f, 0, 0, false, false);
	}

	// Token: 0x040031D9 RID: 12761
	[Serialize]
	public string element;

	// Token: 0x040031DA RID: 12762
	[Serialize]
	public float amount;

	// Token: 0x040031DB RID: 12763
	public Tag elementTag;

	// Token: 0x040031DC RID: 12764
	[MyCmpReq]
	public Storage storage;

	// Token: 0x040031DD RID: 12765
	public float capacity;

	// Token: 0x040031DE RID: 12766
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x040031DF RID: 12767
	public bool underwaterSupport;

	// Token: 0x040031E0 RID: 12768
	private SuitSuffocationMonitor.Instance suitSuffocationMonitor;

	// Token: 0x040031E1 RID: 12769
	private static readonly EventSystem.IntraObjectHandler<SuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>(delegate(SuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x040031E2 RID: 12770
	private static readonly EventSystem.IntraObjectHandler<SuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>(delegate(SuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
