using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000715 RID: 1813
public class CropSleepingMonitor : GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>
{
	// Token: 0x060031D8 RID: 12760 RVA: 0x00108BD8 File Offset: 0x00106DD8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.awake;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.Update("CropSleepingMonitor.root", delegate(CropSleepingMonitor.Instance smi, float dt)
		{
			int cell = Grid.PosToCell(smi.master.gameObject);
			GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State state = smi.IsCellSafe(cell) ? this.awake : this.sleeping;
			smi.GoTo(state);
		}, UpdateRate.SIM_1000ms, false);
		this.sleeping.TriggerOnEnter(GameHashes.CropSleep, null).ToggleStatusItem(Db.Get().CreatureStatusItems.CropSleeping, (CropSleepingMonitor.Instance smi) => smi);
		this.awake.TriggerOnEnter(GameHashes.CropWakeUp, null);
	}

	// Token: 0x04001DD4 RID: 7636
	public GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State sleeping;

	// Token: 0x04001DD5 RID: 7637
	public GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State awake;

	// Token: 0x02001479 RID: 5241
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060084BA RID: 33978 RVA: 0x00303130 File Offset: 0x00301330
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			if (this.prefersDarkness)
			{
				return new List<Descriptor>
				{
					new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_DARKNESS, Descriptor.DescriptorType.Requirement, false)
				};
			}
			Klei.AI.Attribute minLightLux = Db.Get().PlantAttributes.MinLightLux;
			AttributeInstance attributeInstance = minLightLux.Lookup(obj);
			int lux = Mathf.RoundToInt((attributeInstance != null) ? attributeInstance.GetTotalValue() : obj.GetComponent<Modifiers>().GetPreModifiedAttributeValue(minLightLux));
			return new List<Descriptor>
			{
				new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(lux)), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(lux)), Descriptor.DescriptorType.Requirement, false)
			};
		}

		// Token: 0x04006583 RID: 25987
		public bool prefersDarkness;
	}

	// Token: 0x0200147A RID: 5242
	public new class Instance : GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.GameInstance
	{
		// Token: 0x060084BC RID: 33980 RVA: 0x003031E5 File Offset: 0x003013E5
		public Instance(IStateMachineTarget master, CropSleepingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060084BD RID: 33981 RVA: 0x003031EF File Offset: 0x003013EF
		public bool IsSleeping()
		{
			return this.GetCurrentState() == base.smi.sm.sleeping;
		}

		// Token: 0x060084BE RID: 33982 RVA: 0x0030320C File Offset: 0x0030140C
		public bool IsCellSafe(int cell)
		{
			AttributeInstance attributeInstance = Db.Get().PlantAttributes.MinLightLux.Lookup(base.gameObject);
			int num = Grid.LightIntensity[cell];
			if (!base.def.prefersDarkness)
			{
				return (float)num >= attributeInstance.GetTotalValue();
			}
			return num == 0;
		}
	}
}
