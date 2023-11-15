using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007E3 RID: 2019
[SkipSaveFileSerialization]
public class GlowStick : StateMachineComponent<GlowStick.StatesInstance>
{
	// Token: 0x0600392B RID: 14635 RVA: 0x0013F9AD File Offset: 0x0013DBAD
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0200159C RID: 5532
	public class StatesInstance : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick, object>.GameInstance
	{
		// Token: 0x0600882D RID: 34861 RVA: 0x0030DE90 File Offset: 0x0030C090
		public StatesInstance(GlowStick master) : base(master)
		{
			this._light2D.Color = Color.green;
			this._light2D.Range = 2f;
			this._light2D.Angle = 0f;
			this._light2D.Direction = LIGHT2D.DEFAULT_DIRECTION;
			this._light2D.Offset = new Vector2(0.05f, 0.5f);
			this._light2D.shape = global::LightShape.Circle;
			this._light2D.Lux = 500;
			this._radiationEmitter.emitRads = 100f;
			this._radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			this._radiationEmitter.emitRate = 0.5f;
			this._radiationEmitter.emitRadiusX = 3;
			this._radiationEmitter.emitRadiusY = 3;
			this.radiationResistance = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, TRAITS.GLOWSTICK_RADIATION_RESISTANCE, DUPLICANTS.TRAITS.GLOWSTICK.NAME, false, false, true);
		}

		// Token: 0x04006938 RID: 26936
		[MyCmpAdd]
		private RadiationEmitter _radiationEmitter;

		// Token: 0x04006939 RID: 26937
		[MyCmpAdd]
		private Light2D _light2D;

		// Token: 0x0400693A RID: 26938
		public AttributeModifier radiationResistance;
	}

	// Token: 0x0200159D RID: 5533
	public class States : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick>
	{
		// Token: 0x0600882E RID: 34862 RVA: 0x0030DF90 File Offset: 0x0030C190
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleComponent<RadiationEmitter>(false).ToggleComponent<Light2D>(false).ToggleAttributeModifier("Radiation Resistance", (GlowStick.StatesInstance smi) => smi.radiationResistance, null);
		}
	}
}
