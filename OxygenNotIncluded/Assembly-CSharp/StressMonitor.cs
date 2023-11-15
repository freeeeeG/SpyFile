using System;
using Klei.AI;
using Klei.CustomSettings;

// Token: 0x02000899 RID: 2201
public class StressMonitor : GameStateMachine<StressMonitor, StressMonitor.Instance>
{
	// Token: 0x06003FEE RID: 16366 RVA: 0x00165C18 File Offset: 0x00163E18
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		default_state = this.satisfied;
		this.root.Update("StressMonitor", delegate(StressMonitor.Instance smi, float dt)
		{
			smi.ReportStress(dt);
		}, UpdateRate.SIM_200ms, false);
		this.satisfied.TriggerOnEnter(GameHashes.NotStressed, null).Transition(this.stressed.tier1, (StressMonitor.Instance smi) => smi.stress.value >= 60f, UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Neutral, null);
		this.stressed.ToggleStatusItem(Db.Get().DuplicantStatusItems.Stressed, null).Transition(this.satisfied, (StressMonitor.Instance smi) => smi.stress.value < 60f, UpdateRate.SIM_200ms).ToggleReactable((StressMonitor.Instance smi) => smi.CreateConcernReactable()).TriggerOnEnter(GameHashes.Stressed, null);
		this.stressed.tier1.Transition(this.stressed.tier2, (StressMonitor.Instance smi) => smi.HasHadEnough(), UpdateRate.SIM_200ms);
		this.stressed.tier2.TriggerOnEnter(GameHashes.StressedHadEnough, null).Transition(this.stressed.tier1, (StressMonitor.Instance smi) => !smi.HasHadEnough(), UpdateRate.SIM_200ms);
	}

	// Token: 0x04002999 RID: 10649
	public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x0400299A RID: 10650
	public StressMonitor.Stressed stressed;

	// Token: 0x0400299B RID: 10651
	private const float StressThreshold_One = 60f;

	// Token: 0x0400299C RID: 10652
	private const float StressThreshold_Two = 100f;

	// Token: 0x020016BF RID: 5823
	public class Stressed : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006CB4 RID: 27828
		public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier1;

		// Token: 0x04006CB5 RID: 27829
		public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier2;
	}

	// Token: 0x020016C0 RID: 5824
	public new class Instance : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C28 RID: 35880 RVA: 0x00317DDC File Offset: 0x00315FDC
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.stress = Db.Get().Amounts.Stress.Lookup(base.gameObject);
			SettingConfig settingConfig = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.StressBreaks.id];
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.StressBreaks);
			this.allowStressBreak = settingConfig.IsDefaultLevel(currentQualitySetting.id);
		}

		// Token: 0x06008C29 RID: 35881 RVA: 0x00317E53 File Offset: 0x00316053
		public bool IsStressed()
		{
			return base.IsInsideState(base.sm.stressed);
		}

		// Token: 0x06008C2A RID: 35882 RVA: 0x00317E66 File Offset: 0x00316066
		public bool HasHadEnough()
		{
			return this.allowStressBreak && this.stress.value >= 100f;
		}

		// Token: 0x06008C2B RID: 35883 RVA: 0x00317E88 File Offset: 0x00316088
		public void ReportStress(float dt)
		{
			for (int num = 0; num != this.stress.deltaAttribute.Modifiers.Count; num++)
			{
				AttributeModifier attributeModifier = this.stress.deltaAttribute.Modifiers[num];
				DebugUtil.DevAssert(!attributeModifier.IsMultiplier, "Reporting stress for multipliers not supported yet.", null);
				ReportManager.Instance.ReportValue(ReportManager.ReportType.StressDelta, attributeModifier.Value * dt, attributeModifier.GetDescription(), base.gameObject.GetProperName());
			}
		}

		// Token: 0x06008C2C RID: 35884 RVA: 0x00317F04 File Offset: 0x00316104
		public Reactable CreateConcernReactable()
		{
			return new EmoteReactable(base.master.gameObject, "StressConcern", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 30f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Concern);
		}

		// Token: 0x04006CB6 RID: 27830
		public AmountInstance stress;

		// Token: 0x04006CB7 RID: 27831
		private bool allowStressBreak = true;
	}
}
