using System;
using STRINGS;

// Token: 0x020003D6 RID: 982
public class ChoreDriver : StateMachineComponent<ChoreDriver.StatesInstance>
{
	// Token: 0x06001497 RID: 5271 RVA: 0x0006D085 File Offset: 0x0006B285
	public Chore GetCurrentChore()
	{
		return base.smi.GetCurrentChore();
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x0006D092 File Offset: 0x0006B292
	public bool HasChore()
	{
		return base.smi.GetCurrentChore() != null;
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x0006D0A2 File Offset: 0x0006B2A2
	public void StopChore()
	{
		base.smi.sm.stop.Trigger(base.smi);
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x0006D0C0 File Offset: 0x0006B2C0
	public void SetChore(Chore.Precondition.Context context)
	{
		Chore currentChore = base.smi.GetCurrentChore();
		if (currentChore != context.chore)
		{
			this.StopChore();
			if (context.chore.IsValid())
			{
				context.chore.PrepareChore(ref context);
				this.context = context;
				base.smi.sm.nextChore.Set(context.chore, base.smi, false);
				return;
			}
			string text = "Null";
			string text2 = "Null";
			if (currentChore != null)
			{
				text = currentChore.GetType().Name;
			}
			if (context.chore != null)
			{
				text2 = context.chore.GetType().Name;
			}
			Debug.LogWarning(string.Concat(new string[]
			{
				"Stopping chore ",
				text,
				" to start ",
				text2,
				" but stopping the first chore cancelled the second one."
			}));
		}
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x0006D194 File Offset: 0x0006B394
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04000B29 RID: 2857
	[MyCmpAdd]
	private User user;

	// Token: 0x04000B2A RID: 2858
	private Chore.Precondition.Context context;

	// Token: 0x02001045 RID: 4165
	public class StatesInstance : GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.GameInstance
	{
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06007529 RID: 29993 RVA: 0x002CC366 File Offset: 0x002CA566
		// (set) Token: 0x0600752A RID: 29994 RVA: 0x002CC36E File Offset: 0x002CA56E
		public string masterProperName { get; private set; }

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x0600752B RID: 29995 RVA: 0x002CC377 File Offset: 0x002CA577
		// (set) Token: 0x0600752C RID: 29996 RVA: 0x002CC37F File Offset: 0x002CA57F
		public KPrefabID masterPrefabId { get; private set; }

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x0600752D RID: 29997 RVA: 0x002CC388 File Offset: 0x002CA588
		// (set) Token: 0x0600752E RID: 29998 RVA: 0x002CC390 File Offset: 0x002CA590
		public Navigator navigator { get; private set; }

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x0600752F RID: 29999 RVA: 0x002CC399 File Offset: 0x002CA599
		// (set) Token: 0x06007530 RID: 30000 RVA: 0x002CC3A1 File Offset: 0x002CA5A1
		public Worker worker { get; private set; }

		// Token: 0x06007531 RID: 30001 RVA: 0x002CC3AC File Offset: 0x002CA5AC
		public StatesInstance(ChoreDriver master) : base(master)
		{
			this.masterProperName = base.master.GetProperName();
			this.masterPrefabId = base.master.GetComponent<KPrefabID>();
			this.navigator = base.master.GetComponent<Navigator>();
			this.worker = base.master.GetComponent<Worker>();
			this.choreConsumer = base.GetComponent<ChoreConsumer>();
			ChoreConsumer choreConsumer = this.choreConsumer;
			choreConsumer.choreRulesChanged = (System.Action)Delegate.Combine(choreConsumer.choreRulesChanged, new System.Action(this.OnChoreRulesChanged));
		}

		// Token: 0x06007532 RID: 30002 RVA: 0x002CC438 File Offset: 0x002CA638
		public void BeginChore()
		{
			Chore nextChore = this.GetNextChore();
			Chore chore = base.smi.sm.currentChore.Set(nextChore, base.smi, false);
			if (chore != null && chore.IsPreemptable && chore.driver != null)
			{
				chore.Fail("Preemption!");
			}
			base.smi.sm.nextChore.Set(null, base.smi, false);
			Chore chore2 = chore;
			chore2.onExit = (Action<Chore>)Delegate.Combine(chore2.onExit, new Action<Chore>(this.OnChoreExit));
			chore.Begin(base.master.context);
			base.Trigger(-1988963660, chore);
		}

		// Token: 0x06007533 RID: 30003 RVA: 0x002CC4EC File Offset: 0x002CA6EC
		public void EndChore(string reason)
		{
			if (this.GetCurrentChore() != null)
			{
				Chore currentChore = this.GetCurrentChore();
				base.smi.sm.currentChore.Set(null, base.smi, false);
				Chore chore = currentChore;
				chore.onExit = (Action<Chore>)Delegate.Remove(chore.onExit, new Action<Chore>(this.OnChoreExit));
				currentChore.Fail(reason);
				base.Trigger(1745615042, currentChore);
			}
		}

		// Token: 0x06007534 RID: 30004 RVA: 0x002CC55B File Offset: 0x002CA75B
		private void OnChoreExit(Chore chore)
		{
			base.smi.sm.stop.Trigger(base.smi);
		}

		// Token: 0x06007535 RID: 30005 RVA: 0x002CC578 File Offset: 0x002CA778
		public Chore GetNextChore()
		{
			return base.smi.sm.nextChore.Get(base.smi);
		}

		// Token: 0x06007536 RID: 30006 RVA: 0x002CC595 File Offset: 0x002CA795
		public Chore GetCurrentChore()
		{
			return base.smi.sm.currentChore.Get(base.smi);
		}

		// Token: 0x06007537 RID: 30007 RVA: 0x002CC5B4 File Offset: 0x002CA7B4
		private void OnChoreRulesChanged()
		{
			Chore currentChore = this.GetCurrentChore();
			if (currentChore != null && !this.choreConsumer.IsPermittedOrEnabled(currentChore.choreType, currentChore))
			{
				this.EndChore("Permissions changed");
			}
		}

		// Token: 0x040058A7 RID: 22695
		private ChoreConsumer choreConsumer;
	}

	// Token: 0x02001046 RID: 4166
	public class States : GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver>
	{
		// Token: 0x06007538 RID: 30008 RVA: 0x002CC5EC File Offset: 0x002CA7EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.nochore;
			this.saveHistory = true;
			this.nochore.Update(delegate(ChoreDriver.StatesInstance smi, float dt)
			{
				if (smi.masterPrefabId.IsPrefabID(GameTags.Minion) && !smi.masterPrefabId.HasTag(GameTags.Dead))
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.WorkTime, dt, string.Format(UI.ENDOFDAYREPORT.NOTES.TIME_SPENT, DUPLICANTS.CHORES.THINKING.NAME), smi.master.GetProperName());
				}
			}, UpdateRate.SIM_200ms, false).ParamTransition<Chore>(this.nextChore, this.haschore, (ChoreDriver.StatesInstance smi, Chore next_chore) => next_chore != null);
			this.haschore.Enter("BeginChore", delegate(ChoreDriver.StatesInstance smi)
			{
				smi.BeginChore();
			}).Update(delegate(ChoreDriver.StatesInstance smi, float dt)
			{
				if (smi.masterPrefabId.IsPrefabID(GameTags.Minion) && !smi.masterPrefabId.HasTag(GameTags.Dead))
				{
					Chore chore = this.currentChore.Get(smi);
					if (chore == null)
					{
						return;
					}
					if (smi.navigator.IsMoving())
					{
						ReportManager.Instance.ReportValue(ReportManager.ReportType.TravelTime, dt, GameUtil.GetChoreName(chore, null), smi.master.GetProperName());
						return;
					}
					ReportManager.ReportType reportType = chore.GetReportType();
					Workable workable = smi.worker.workable;
					if (workable != null)
					{
						ReportManager.ReportType reportType2 = workable.GetReportType();
						if (reportType != reportType2)
						{
							reportType = reportType2;
						}
					}
					ReportManager.Instance.ReportValue(reportType, dt, string.Format(UI.ENDOFDAYREPORT.NOTES.WORK_TIME, GameUtil.GetChoreName(chore, null)), smi.master.GetProperName());
				}
			}, UpdateRate.SIM_200ms, false).Exit("EndChore", delegate(ChoreDriver.StatesInstance smi)
			{
				smi.EndChore("ChoreDriver.SignalStop");
			}).OnSignal(this.stop, this.nochore);
		}

		// Token: 0x040058A8 RID: 22696
		public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.ObjectParameter<Chore> currentChore;

		// Token: 0x040058A9 RID: 22697
		public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.ObjectParameter<Chore> nextChore;

		// Token: 0x040058AA RID: 22698
		public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.Signal stop;

		// Token: 0x040058AB RID: 22699
		public GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State nochore;

		// Token: 0x040058AC RID: 22700
		public GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State haschore;
	}
}
