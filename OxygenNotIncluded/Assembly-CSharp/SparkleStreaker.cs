using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
public class SparkleStreaker : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance>
{
	// Token: 0x06001520 RID: 5408 RVA: 0x0006F70C File Offset: 0x0006D90C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsSparkleStreaker").ToggleLoopingSound(this.soundPath, null, true, true, true).Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.sparkleStreakFX = Util.KInstantiate(EffectPrefabs.Instance.SparkleStreakFX, smi.master.transform.GetPosition() + this.offset);
			smi.sparkleStreakFX.transform.SetParent(smi.master.transform);
			smi.sparkleStreakFX.SetActive(true);
			smi.CreatePasserbyReactable();
		}).Exit(delegate(SparkleStreaker.Instance smi)
		{
			Util.KDestroyGameObject(smi.sparkleStreakFX);
			smi.ClearPasserbyReactable();
		});
		this.overjoyed.idle.Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.SetSparkleSoundParam(0f);
		}).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.moving, (SparkleStreaker.Instance smi) => smi.IsMoving());
		this.overjoyed.moving.Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.SetSparkleSoundParam(1f);
		}).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.idle, (SparkleStreaker.Instance smi) => !smi.IsMoving());
	}

	// Token: 0x04000B6E RID: 2926
	private Vector3 offset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04000B6F RID: 2927
	public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000B70 RID: 2928
	public SparkleStreaker.OverjoyedStates overjoyed;

	// Token: 0x04000B71 RID: 2929
	public string soundPath = GlobalAssets.GetSound("SparkleStreaker_lp", false);

	// Token: 0x04000B72 RID: 2930
	public HashedString SPARKLE_STREAKER_MOVING_PARAMETER = "sparkleStreaker_moving";

	// Token: 0x02001070 RID: 4208
	public class OverjoyedStates : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400590E RID: 22798
		public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x0400590F RID: 22799
		public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State moving;
	}

	// Token: 0x02001071 RID: 4209
	public new class Instance : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060075AC RID: 30124 RVA: 0x002CD651 File Offset: 0x002CB851
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060075AD RID: 30125 RVA: 0x002CD65C File Offset: 0x002CB85C
		public void CreatePasserbyReactable()
		{
			if (this.passerbyReactable == null)
			{
				EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 0f, 600f, float.PositiveInfinity, 0f);
				Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
				emoteReactable.SetEmote(clapCheer).SetThought(Db.Get().Thoughts.Happy).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
				emoteReactable.RegisterEmoteStepCallbacks("clapcheer_pre", new Action<GameObject>(this.AddReactionEffect), null);
				this.passerbyReactable = emoteReactable;
			}
		}

		// Token: 0x060075AE RID: 30126 RVA: 0x002CD716 File Offset: 0x002CB916
		private void AddReactionEffect(GameObject reactor)
		{
			reactor.GetComponent<Effects>().Add("SawSparkleStreaker", true);
		}

		// Token: 0x060075AF RID: 30127 RVA: 0x002CD72A File Offset: 0x002CB92A
		private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
		{
			return transition.end == NavType.Floor;
		}

		// Token: 0x060075B0 RID: 30128 RVA: 0x002CD735 File Offset: 0x002CB935
		public void ClearPasserbyReactable()
		{
			if (this.passerbyReactable != null)
			{
				this.passerbyReactable.Cleanup();
				this.passerbyReactable = null;
			}
		}

		// Token: 0x060075B1 RID: 30129 RVA: 0x002CD751 File Offset: 0x002CB951
		public bool IsMoving()
		{
			return base.smi.master.GetComponent<Navigator>().IsMoving();
		}

		// Token: 0x060075B2 RID: 30130 RVA: 0x002CD768 File Offset: 0x002CB968
		public void SetSparkleSoundParam(float val)
		{
			base.GetComponent<LoopingSounds>().SetParameter(GlobalAssets.GetSound("SparkleStreaker_lp", false), "sparkleStreaker_moving", val);
		}

		// Token: 0x04005910 RID: 22800
		private Reactable passerbyReactable;

		// Token: 0x04005911 RID: 22801
		public GameObject sparkleStreakFX;
	}
}
