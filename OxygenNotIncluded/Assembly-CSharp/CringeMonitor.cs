using System;

// Token: 0x02000870 RID: 2160
public class CringeMonitor : GameStateMachine<CringeMonitor, CringeMonitor.Instance>
{
	// Token: 0x06003F27 RID: 16167 RVA: 0x001605E0 File Offset: 0x0015E7E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.Cringe, new GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.TriggerCringe));
		this.cringe.ToggleReactable((CringeMonitor.Instance smi) => smi.GetReactable()).ToggleStatusItem((CringeMonitor.Instance smi) => smi.GetStatusItem(), null).ScheduleGoTo(3f, this.idle);
	}

	// Token: 0x06003F28 RID: 16168 RVA: 0x00160672 File Offset: 0x0015E872
	private void TriggerCringe(CringeMonitor.Instance smi, object data)
	{
		if (smi.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
		{
			return;
		}
		smi.SetCringeSourceData(data);
		smi.GoTo(this.cringe);
	}

	// Token: 0x040028E7 RID: 10471
	public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040028E8 RID: 10472
	public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State cringe;

	// Token: 0x02001659 RID: 5721
	public new class Instance : GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008A6F RID: 35439 RVA: 0x00313C75 File Offset: 0x00311E75
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008A70 RID: 35440 RVA: 0x00313C80 File Offset: 0x00311E80
		public void SetCringeSourceData(object data)
		{
			string name = (string)data;
			this.statusItem = new StatusItem("CringeSource", name, null, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
		}

		// Token: 0x06008A71 RID: 35441 RVA: 0x00313CBC File Offset: 0x00311EBC
		public Reactable GetReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "Cringe", Db.Get().ChoreTypes.EmoteHighPriority, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(Db.Get().Emotes.Minion.Cringe);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x06008A72 RID: 35442 RVA: 0x00313D28 File Offset: 0x00311F28
		public StatusItem GetStatusItem()
		{
			return this.statusItem;
		}

		// Token: 0x04006B59 RID: 27481
		private StatusItem statusItem;
	}
}
