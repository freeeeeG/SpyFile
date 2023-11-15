using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200083F RID: 2111
public class LeadSuitMonitor : GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance>
{
	// Token: 0x06003D72 RID: 15730 RVA: 0x00154F34 File Offset: 0x00153134
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.wearingSuit;
		base.Target(this.owner);
		this.wearingSuit.DefaultState(this.wearingSuit.hasBattery);
		this.wearingSuit.hasBattery.Update(new Action<LeadSuitMonitor.Instance, float>(LeadSuitMonitor.CoolSuit), UpdateRate.SIM_200ms, false).TagTransition(GameTags.SuitBatteryOut, this.wearingSuit.noBattery, false);
		this.wearingSuit.noBattery.Enter(delegate(LeadSuitMonitor.Instance smi)
		{
			Attributes attributes = smi.sm.owner.Get(smi).GetAttributes();
			if (attributes != null)
			{
				foreach (AttributeModifier modifier in smi.noBatteryModifiers)
				{
					attributes.Add(modifier);
				}
			}
		}).Exit(delegate(LeadSuitMonitor.Instance smi)
		{
			Attributes attributes = smi.sm.owner.Get(smi).GetAttributes();
			if (attributes != null)
			{
				foreach (AttributeModifier modifier in smi.noBatteryModifiers)
				{
					attributes.Remove(modifier);
				}
			}
		}).TagTransition(GameTags.SuitBatteryOut, this.wearingSuit.hasBattery, true);
	}

	// Token: 0x06003D73 RID: 15731 RVA: 0x0015500C File Offset: 0x0015320C
	public static void CoolSuit(LeadSuitMonitor.Instance smi, float dt)
	{
		if (!smi.navigator)
		{
			return;
		}
		GameObject gameObject = smi.sm.owner.Get(smi);
		if (!gameObject)
		{
			return;
		}
		ExternalTemperatureMonitor.Instance smi2 = gameObject.GetSMI<ExternalTemperatureMonitor.Instance>();
		if (smi2 != null && smi2.AverageExternalTemperature >= smi.lead_suit_tank.coolingOperationalTemperature)
		{
			smi.lead_suit_tank.batteryCharge -= 1f / smi.lead_suit_tank.batteryDuration * dt;
			if (smi.lead_suit_tank.IsEmpty())
			{
				gameObject.AddTag(GameTags.SuitBatteryOut);
			}
		}
	}

	// Token: 0x04002812 RID: 10258
	public LeadSuitMonitor.WearingSuit wearingSuit;

	// Token: 0x04002813 RID: 10259
	public StateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.TargetParameter owner;

	// Token: 0x02001611 RID: 5649
	public class WearingSuit : GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006A67 RID: 27239
		public GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.State hasBattery;

		// Token: 0x04006A68 RID: 27240
		public GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.State noBattery;
	}

	// Token: 0x02001612 RID: 5650
	public new class Instance : GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008948 RID: 35144 RVA: 0x00311850 File Offset: 0x0030FA50
		public Instance(IStateMachineTarget master, GameObject owner) : base(master)
		{
			base.sm.owner.Set(owner, base.smi, false);
			this.navigator = owner.GetComponent<Navigator>();
			this.lead_suit_tank = master.GetComponent<LeadSuitTank>();
			this.noBatteryModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.INSULATION, (float)(-(float)TUNING.EQUIPMENT.SUITS.LEADSUIT_INSULATION), STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.SUIT_OUT_OF_BATTERIES, false, false, true));
			this.noBatteryModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.THERMAL_CONDUCTIVITY_BARRIER, -TUNING.EQUIPMENT.SUITS.LEADSUIT_THERMAL_CONDUCTIVITY_BARRIER, STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.SUIT_OUT_OF_BATTERIES, false, false, true));
		}

		// Token: 0x04006A69 RID: 27241
		public Navigator navigator;

		// Token: 0x04006A6A RID: 27242
		public LeadSuitTank lead_suit_tank;

		// Token: 0x04006A6B RID: 27243
		public List<AttributeModifier> noBatteryModifiers = new List<AttributeModifier>();
	}
}
