using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020008FD RID: 2301
[SkipSaveFileSerialization]
public class RadiationEater : StateMachineComponent<RadiationEater.StatesInstance>
{
	// Token: 0x060042BD RID: 17085 RVA: 0x00175854 File Offset: 0x00173A54
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001751 RID: 5969
	public class StatesInstance : GameStateMachine<RadiationEater.States, RadiationEater.StatesInstance, RadiationEater, object>.GameInstance
	{
		// Token: 0x06008E15 RID: 36373 RVA: 0x0031E77E File Offset: 0x0031C97E
		public StatesInstance(RadiationEater master) : base(master)
		{
			this.radiationEating = new AttributeModifier(Db.Get().Attributes.RadiationRecovery.Id, TRAITS.RADIATION_EATER_RECOVERY, DUPLICANTS.TRAITS.RADIATIONEATER.NAME, false, false, true);
		}

		// Token: 0x06008E16 RID: 36374 RVA: 0x0031E7B8 File Offset: 0x0031C9B8
		public void OnEatRads(float radsEaten)
		{
			float delta = Mathf.Abs(radsEaten) * TRAITS.RADS_TO_CALS;
			base.smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.Calories).ApplyDelta(delta);
		}

		// Token: 0x04006E62 RID: 28258
		public AttributeModifier radiationEating;
	}

	// Token: 0x02001752 RID: 5970
	public class States : GameStateMachine<RadiationEater.States, RadiationEater.StatesInstance, RadiationEater>
	{
		// Token: 0x06008E17 RID: 36375 RVA: 0x0031E804 File Offset: 0x0031CA04
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier("Radiation Eating", (RadiationEater.StatesInstance smi) => smi.radiationEating, null).EventHandler(GameHashes.RadiationRecovery, delegate(RadiationEater.StatesInstance smi, object data)
			{
				float radsEaten = (float)data;
				smi.OnEatRads(radsEaten);
			});
		}
	}
}
