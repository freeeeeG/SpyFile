using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200064D RID: 1613
public class MassageTable : RelaxationPoint, IGameObjectEffectDescriptor, IActivationRangeTarget
{
	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06002A7A RID: 10874 RVA: 0x000E2AFF File Offset: 0x000E0CFF
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.MASSAGETABLE.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06002A7B RID: 10875 RVA: 0x000E2B0B File Offset: 0x000E0D0B
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.MASSAGETABLE.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x06002A7C RID: 10876 RVA: 0x000E2B17 File Offset: 0x000E0D17
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<MassageTable>(-905833192, MassageTable.OnCopySettingsDelegate);
	}

	// Token: 0x06002A7D RID: 10877 RVA: 0x000E2B30 File Offset: 0x000E0D30
	private void OnCopySettings(object data)
	{
		MassageTable component = ((GameObject)data).GetComponent<MassageTable>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x06002A7E RID: 10878 RVA: 0x000E2B6C File Offset: 0x000E0D6C
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < MassageTable.EffectsRemoved.Length; i++)
		{
			string effect_id = MassageTable.EffectsRemoved[i];
			component.Remove(effect_id);
		}
	}

	// Token: 0x06002A7F RID: 10879 RVA: 0x000E2BA8 File Offset: 0x000E0DA8
	public new List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		if (MassageTable.EffectsRemoved.Length != 0)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
			list.Add(item2);
			for (int i = 0; i < MassageTable.EffectsRemoved.Length; i++)
			{
				string text = MassageTable.EffectsRemoved[i];
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string arg2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
				Descriptor item3 = default(Descriptor);
				item3.IncreaseIndent();
				item3.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2), Descriptor.DescriptorType.Effect);
				list.Add(item3);
			}
		}
		return list;
	}

	// Token: 0x06002A80 RID: 10880 RVA: 0x000E2D08 File Offset: 0x000E0F08
	protected override WorkChore<RelaxationPoint> CreateWorkChore()
	{
		WorkChore<RelaxationPoint> workChore = new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.StressHeal, this, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, this);
		workChore.AddPrecondition(MassageTable.IsStressAboveActivationRange, this);
		return workChore;
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06002A81 RID: 10881 RVA: 0x000E2D58 File Offset: 0x000E0F58
	// (set) Token: 0x06002A82 RID: 10882 RVA: 0x000E2D60 File Offset: 0x000E0F60
	public float ActivateValue
	{
		get
		{
			return this.activateValue;
		}
		set
		{
			this.activateValue = value;
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06002A83 RID: 10883 RVA: 0x000E2D69 File Offset: 0x000E0F69
	// (set) Token: 0x06002A84 RID: 10884 RVA: 0x000E2D71 File Offset: 0x000E0F71
	public float DeactivateValue
	{
		get
		{
			return this.stopStressingValue;
		}
		set
		{
			this.stopStressingValue = value;
		}
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06002A85 RID: 10885 RVA: 0x000E2D7A File Offset: 0x000E0F7A
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06002A86 RID: 10886 RVA: 0x000E2D7D File Offset: 0x000E0F7D
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06002A87 RID: 10887 RVA: 0x000E2D84 File Offset: 0x000E0F84
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06002A88 RID: 10888 RVA: 0x000E2D8B File Offset: 0x000E0F8B
	public string ActivationRangeTitleText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06002A89 RID: 10889 RVA: 0x000E2D97 File Offset: 0x000E0F97
	public string ActivateSliderLabelText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.ACTIVATE;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06002A8A RID: 10890 RVA: 0x000E2DA3 File Offset: 0x000E0FA3
	public string DeactivateSliderLabelText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.DEACTIVATE;
		}
	}

	// Token: 0x040018DC RID: 6364
	[Serialize]
	private float activateValue = 50f;

	// Token: 0x040018DD RID: 6365
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoreBack"
	};

	// Token: 0x040018DE RID: 6366
	private static readonly EventSystem.IntraObjectHandler<MassageTable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<MassageTable>(delegate(MassageTable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040018DF RID: 6367
	private static readonly Chore.Precondition IsStressAboveActivationRange = new Chore.Precondition
	{
		id = "IsStressAboveActivationRange",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_STRESS_ABOVE_ACTIVATION_RANGE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			IActivationRangeTarget activationRangeTarget = (IActivationRangeTarget)data;
			return Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject).value >= activationRangeTarget.ActivateValue;
		}
	};
}
