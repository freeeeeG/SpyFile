using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200088C RID: 2188
public class RadiationMonitor : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance>
{
	// Token: 0x06003FAA RID: 16298 RVA: 0x001637F8 File Offset: 0x001619F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.init;
		this.init.Transition(null, (RadiationMonitor.Instance smi) => !Sim.IsRadiationEnabled(), UpdateRate.SIM_200ms).Transition(this.active, (RadiationMonitor.Instance smi) => Sim.IsRadiationEnabled(), UpdateRate.SIM_200ms);
		this.active.Update(new Action<RadiationMonitor.Instance, float>(RadiationMonitor.CheckRadiationLevel), UpdateRate.SIM_1000ms, false).DefaultState(this.active.idle);
		this.active.idle.DoNothing().ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ParamTransition<float>(this.radiationExposure, this.active.sick.major, RadiationMonitor.COMPARE_GTE_MAJOR).ParamTransition<float>(this.radiationExposure, this.active.sick.minor, RadiationMonitor.COMPARE_GTE_MINOR);
		this.active.sick.ParamTransition<float>(this.radiationExposure, this.active.idle, RadiationMonitor.COMPARE_LT_MINOR).Enter(delegate(RadiationMonitor.Instance smi)
		{
			smi.sm.isSick.Set(true, smi, false);
		}).Exit(delegate(RadiationMonitor.Instance smi)
		{
			smi.sm.isSick.Set(false, smi, false);
		});
		this.active.sick.minor.ToggleEffect(RadiationMonitor.minorSicknessEffect).ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ParamTransition<float>(this.radiationExposure, this.active.sick.major, RadiationMonitor.COMPARE_GTE_MAJOR).ToggleAnims("anim_loco_radiation1_kanim", 4f, "").ToggleAnims("anim_idle_radiation1_kanim", 4f, "").ToggleExpression(Db.Get().Expressions.Radiation1, null).DefaultState(this.active.sick.minor.waiting);
		this.active.sick.minor.reacting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.minor.waiting);
		this.active.sick.major.ToggleEffect(RadiationMonitor.majorSicknessEffect).ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ToggleAnims("anim_loco_radiation2_kanim", 4f, "").ToggleAnims("anim_idle_radiation2_kanim", 4f, "").ToggleExpression(Db.Get().Expressions.Radiation2, null).DefaultState(this.active.sick.major.waiting);
		this.active.sick.major.waiting.ScheduleGoTo(120f, this.active.sick.major.vomiting);
		this.active.sick.major.vomiting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.major.waiting);
		this.active.sick.extreme.ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ToggleEffect(RadiationMonitor.extremeSicknessEffect).ToggleAnims("anim_loco_radiation3_kanim", 4f, "").ToggleAnims("anim_idle_radiation3_kanim", 4f, "").ToggleExpression(Db.Get().Expressions.Radiation3, null).DefaultState(this.active.sick.extreme.waiting);
		this.active.sick.extreme.waiting.ScheduleGoTo(60f, this.active.sick.extreme.vomiting);
		this.active.sick.extreme.vomiting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.extreme.waiting);
		this.active.sick.deadly.ToggleAnims("anim_loco_radiation4_kanim", 4f, "").ToggleAnims("anim_idle_radiation4_kanim", 4f, "").ToggleExpression(Db.Get().Expressions.Radiation4, null).Enter(delegate(RadiationMonitor.Instance smi)
		{
			smi.GetComponent<Health>().Incapacitate(GameTags.RadiationSicknessIncapacitation);
		});
	}

	// Token: 0x06003FAB RID: 16299 RVA: 0x00163D3C File Offset: 0x00161F3C
	private Chore CreateVomitChore(RadiationMonitor.Instance smi)
	{
		Notification notification = new Notification(DUPLICANTS.STATUSITEMS.RADIATIONVOMITING.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.RADIATIONVOMITING.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		return new VomitChore(Db.Get().ChoreTypes.Vomit, smi.master, Db.Get().DuplicantStatusItems.Vomiting, notification, null);
	}

	// Token: 0x06003FAC RID: 16300 RVA: 0x00163DB4 File Offset: 0x00161FB4
	private static void RadiationRecovery(RadiationMonitor.Instance smi, float dt)
	{
		float num = Db.Get().Attributes.RadiationRecovery.Lookup(smi.gameObject).GetTotalValue() * dt;
		smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).ApplyDelta(num);
		smi.master.Trigger(1556680150, num);
	}

	// Token: 0x06003FAD RID: 16301 RVA: 0x00163E24 File Offset: 0x00162024
	private static void CheckRadiationLevel(RadiationMonitor.Instance smi, float dt)
	{
		RadiationMonitor.RadiationRecovery(smi, dt);
		smi.sm.timeUntilNextExposureReact.Delta(-dt, smi);
		smi.sm.timeUntilNextSickReact.Delta(-dt, smi);
		int num = Grid.PosToCell(smi.gameObject);
		if (Grid.IsValidCell(num))
		{
			float num2 = Mathf.Clamp01(1f - Db.Get().Attributes.RadiationResistance.Lookup(smi.gameObject).GetTotalValue());
			float num3 = Grid.Radiation[num] * 1f * num2 / 600f * dt;
			smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).ApplyDelta(num3);
			float num4 = num3 / dt * 600f;
			smi.sm.currentExposurePerCycle.Set(num4, smi, false);
			if (smi.sm.timeUntilNextExposureReact.Get(smi) <= 0f && !smi.HasTag(GameTags.InTransitTube) && RadiationMonitor.COMPARE_REACT(smi, num4))
			{
				smi.sm.timeUntilNextExposureReact.Set(120f, smi, false);
				Emote radiation_Glare = Db.Get().Emotes.Minion.Radiation_Glare;
				smi.master.gameObject.GetSMI<ReactionMonitor.Instance>().AddSelfEmoteReactable(smi.master.gameObject, "RadiationReact", radiation_Glare, true, Db.Get().ChoreTypes.EmoteHighPriority, 0f, 20f, float.NegativeInfinity, 0f, null);
			}
		}
		if (smi.sm.timeUntilNextSickReact.Get(smi) <= 0f && smi.sm.isSick.Get(smi) && !smi.HasTag(GameTags.InTransitTube))
		{
			smi.sm.timeUntilNextSickReact.Set(60f, smi, false);
			Emote radiation_Itch = Db.Get().Emotes.Minion.Radiation_Itch;
			smi.master.gameObject.GetSMI<ReactionMonitor.Instance>().AddSelfEmoteReactable(smi.master.gameObject, "RadiationReact", radiation_Itch, true, Db.Get().ChoreTypes.RadiationPain, 0f, 20f, float.NegativeInfinity, 0f, null);
		}
		smi.sm.radiationExposure.Set(smi.master.gameObject.GetComponent<KSelectable>().GetAmounts().GetValue("RadiationBalance"), smi, false);
	}

	// Token: 0x04002951 RID: 10577
	public const float BASE_ABSORBTION_RATE = 1f;

	// Token: 0x04002952 RID: 10578
	public const float MIN_TIME_BETWEEN_EXPOSURE_REACTS = 120f;

	// Token: 0x04002953 RID: 10579
	public const float MIN_TIME_BETWEEN_SICK_REACTS = 60f;

	// Token: 0x04002954 RID: 10580
	public const int VOMITS_PER_CYCLE_MAJOR = 5;

	// Token: 0x04002955 RID: 10581
	public const int VOMITS_PER_CYCLE_EXTREME = 10;

	// Token: 0x04002956 RID: 10582
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter radiationExposure;

	// Token: 0x04002957 RID: 10583
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter currentExposurePerCycle;

	// Token: 0x04002958 RID: 10584
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.BoolParameter isSick;

	// Token: 0x04002959 RID: 10585
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeUntilNextExposureReact;

	// Token: 0x0400295A RID: 10586
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeUntilNextSickReact;

	// Token: 0x0400295B RID: 10587
	public static string minorSicknessEffect = "RadiationExposureMinor";

	// Token: 0x0400295C RID: 10588
	public static string majorSicknessEffect = "RadiationExposureMajor";

	// Token: 0x0400295D RID: 10589
	public static string extremeSicknessEffect = "RadiationExposureExtreme";

	// Token: 0x0400295E RID: 10590
	public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State init;

	// Token: 0x0400295F RID: 10591
	public RadiationMonitor.ActiveStates active;

	// Token: 0x04002960 RID: 10592
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_RECOVERY_IMMEDIATE = (RadiationMonitor.Instance smi, float p) => p > 100f * smi.difficultySettingMod / 2f;

	// Token: 0x04002961 RID: 10593
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_REACT = (RadiationMonitor.Instance smi, float p) => p >= 133f * smi.difficultySettingMod;

	// Token: 0x04002962 RID: 10594
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_LT_MINOR = (RadiationMonitor.Instance smi, float p) => p < 100f * smi.difficultySettingMod;

	// Token: 0x04002963 RID: 10595
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_MINOR = (RadiationMonitor.Instance smi, float p) => p >= 100f * smi.difficultySettingMod;

	// Token: 0x04002964 RID: 10596
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_MAJOR = (RadiationMonitor.Instance smi, float p) => p >= 300f * smi.difficultySettingMod;

	// Token: 0x04002965 RID: 10597
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_EXTREME = (RadiationMonitor.Instance smi, float p) => p >= 600f * smi.difficultySettingMod;

	// Token: 0x04002966 RID: 10598
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_DEADLY = (RadiationMonitor.Instance smi, float p) => p >= 900f * smi.difficultySettingMod;

	// Token: 0x0200169E RID: 5790
	public class ActiveStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006C44 RID: 27716
		public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04006C45 RID: 27717
		public RadiationMonitor.SickStates sick;
	}

	// Token: 0x0200169F RID: 5791
	public class SickStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006C46 RID: 27718
		public RadiationMonitor.SickStates.MinorStates minor;

		// Token: 0x04006C47 RID: 27719
		public RadiationMonitor.SickStates.MajorStates major;

		// Token: 0x04006C48 RID: 27720
		public RadiationMonitor.SickStates.ExtremeStates extreme;

		// Token: 0x04006C49 RID: 27721
		public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State deadly;

		// Token: 0x020021A8 RID: 8616
		public class MinorStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x040096C9 RID: 38601
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x040096CA RID: 38602
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State reacting;
		}

		// Token: 0x020021A9 RID: 8617
		public class MajorStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x040096CB RID: 38603
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x040096CC RID: 38604
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State vomiting;
		}

		// Token: 0x020021AA RID: 8618
		public class ExtremeStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x040096CD RID: 38605
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x040096CE RID: 38606
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State vomiting;
		}
	}

	// Token: 0x020016A0 RID: 5792
	public new class Instance : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B97 RID: 35735 RVA: 0x00316B74 File Offset: 0x00314D74
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
			if (Sim.IsRadiationEnabled())
			{
				SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Radiation);
				if (currentQualitySetting != null)
				{
					string id = currentQualitySetting.id;
					if (id != null)
					{
						if (id == "Easiest")
						{
							this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.EASIEST;
							return;
						}
						if (id == "Easier")
						{
							this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.EASIER;
							return;
						}
						if (id == "Harder")
						{
							this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.HARDER;
							return;
						}
						if (!(id == "Hardest"))
						{
							return;
						}
						this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.HARDEST;
					}
				}
			}
		}

		// Token: 0x06008B98 RID: 35736 RVA: 0x00316C2A File Offset: 0x00314E2A
		public float SicknessSecondsRemaining()
		{
			return 600f * (Mathf.Max(0f, base.sm.radiationExposure.Get(base.smi) - 100f * this.difficultySettingMod) / 100f);
		}

		// Token: 0x06008B99 RID: 35737 RVA: 0x00316C68 File Offset: 0x00314E68
		public string GetEffectStatusTooltip()
		{
			if (this.effects.HasEffect(RadiationMonitor.minorSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.minorSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.minorSicknessEffect));
			}
			if (this.effects.HasEffect(RadiationMonitor.majorSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.majorSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.majorSicknessEffect));
			}
			if (this.effects.HasEffect(RadiationMonitor.extremeSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.extremeSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.extremeSicknessEffect));
			}
			return DUPLICANTS.MODIFIERS.RADIATIONEXPOSUREDEADLY.TOOLTIP;
		}

		// Token: 0x04006C4A RID: 27722
		public Effects effects;

		// Token: 0x04006C4B RID: 27723
		public float difficultySettingMod = 1f;
	}
}
