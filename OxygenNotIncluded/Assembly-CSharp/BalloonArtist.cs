using System;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;

// Token: 0x020003E7 RID: 999
public class BalloonArtist : GameStateMachine<BalloonArtist, BalloonArtist.Instance>
{
	// Token: 0x06001519 RID: 5401 RVA: 0x0006F330 File Offset: 0x0006D530
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<int>(this.balloonsGivenOut, this.overjoyed.exitEarly, (BalloonArtist.Instance smi, int p) => p >= TRAITS.JOY_REACTIONS.BALLOON_ARTIST.NUM_BALLOONS_TO_GIVE).Exit(delegate(BalloonArtist.Instance smi)
		{
			smi.numBalloonsGiven = 0;
			this.balloonsGivenOut.Set(0, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(BalloonArtist.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.balloon_stand);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistPlanning, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.balloon_stand, (BalloonArtist.Instance smi) => smi.IsRecTime());
		this.overjoyed.balloon_stand.ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistHandingOut, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.idle, (BalloonArtist.Instance smi) => !smi.IsRecTime()).ToggleChore((BalloonArtist.Instance smi) => new BalloonArtistChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(BalloonArtist.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x04000B67 RID: 2919
	public StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.IntParameter balloonsGivenOut;

	// Token: 0x04000B68 RID: 2920
	public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000B69 RID: 2921
	public BalloonArtist.OverjoyedStates overjoyed;

	// Token: 0x0200106A RID: 4202
	public class OverjoyedStates : GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040058F8 RID: 22776
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040058F9 RID: 22777
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State balloon_stand;

		// Token: 0x040058FA RID: 22778
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x0200106B RID: 4203
	public new class Instance : GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600758F RID: 30095 RVA: 0x002CD1E4 File Offset: 0x002CB3E4
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06007590 RID: 30096 RVA: 0x002CD1ED File Offset: 0x002CB3ED
		[OnDeserialized]
		private void OnDeserialized()
		{
			base.smi.sm.balloonsGivenOut.Set(this.numBalloonsGiven, base.smi, false);
		}

		// Token: 0x06007591 RID: 30097 RVA: 0x002CD214 File Offset: 0x002CB414
		public void Internal_InitBalloons()
		{
			JoyResponseOutfitTarget joyResponseOutfitTarget = JoyResponseOutfitTarget.FromMinion(base.master.gameObject);
			if (!this.balloonSymbolIter.IsNullOrDestroyed())
			{
				if (this.balloonSymbolIter.facade.AndThen<string>((BalloonArtistFacadeResource f) => f.Id) == joyResponseOutfitTarget.ReadFacadeId())
				{
					return;
				}
			}
			this.balloonSymbolIter = joyResponseOutfitTarget.ReadFacadeId().AndThen<BalloonArtistFacadeResource>((string id) => Db.Get().Permits.BalloonArtistFacades.Get(id)).AndThen<BalloonOverrideSymbolIter>((BalloonArtistFacadeResource permit) => permit.GetSymbolIter()).UnwrapOr(new BalloonOverrideSymbolIter(Option.None), null);
			this.SetBalloonSymbolOverride(this.balloonSymbolIter.Current());
		}

		// Token: 0x06007592 RID: 30098 RVA: 0x002CD301 File Offset: 0x002CB501
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06007593 RID: 30099 RVA: 0x002CD324 File Offset: 0x002CB524
		public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverrideSymbol)
		{
			if (balloonOverrideSymbol.animFile.IsNone())
			{
				base.master.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"), 0);
				return;
			}
			base.master.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", balloonOverrideSymbol.symbol.Unwrap(), 0);
		}

		// Token: 0x06007594 RID: 30100 RVA: 0x002CD3AC File Offset: 0x002CB5AC
		public BalloonOverrideSymbol GetCurrentBalloonSymbolOverride()
		{
			return this.balloonSymbolIter.Current();
		}

		// Token: 0x06007595 RID: 30101 RVA: 0x002CD3B9 File Offset: 0x002CB5B9
		public void ApplyNextBalloonSymbolOverride()
		{
			this.SetBalloonSymbolOverride(this.balloonSymbolIter.Next());
		}

		// Token: 0x06007596 RID: 30102 RVA: 0x002CD3CC File Offset: 0x002CB5CC
		public void GiveBalloon()
		{
			this.numBalloonsGiven++;
			base.smi.sm.balloonsGivenOut.Set(this.numBalloonsGiven, base.smi, false);
		}

		// Token: 0x06007597 RID: 30103 RVA: 0x002CD400 File Offset: 0x002CB600
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}

		// Token: 0x040058FB RID: 22779
		[Serialize]
		public int numBalloonsGiven;

		// Token: 0x040058FC RID: 22780
		[NonSerialized]
		private BalloonOverrideSymbolIter balloonSymbolIter;

		// Token: 0x040058FD RID: 22781
		private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

		// Token: 0x040058FE RID: 22782
		private const int TARGET_OVERRIDE_PRIORITY = 0;
	}
}
