using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020003E8 RID: 1000
public class HappySinger : GameStateMachine<HappySinger, HappySinger.Instance>
{
	// Token: 0x0600151D RID: 5405 RVA: 0x0006F534 File Offset: 0x0006D734
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsJoySinger").ToggleLoopingSound(this.soundPath, null, true, true, true).ToggleAnims("anim_loco_singer_kanim", 0f, "").ToggleAnims("anim_idle_singer_kanim", 0f, "").EventHandler(GameHashes.TagsChanged, delegate(HappySinger.Instance smi, object obj)
		{
			smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
		}).Enter(delegate(HappySinger.Instance smi)
		{
			smi.musicParticleFX = Util.KInstantiate(EffectPrefabs.Instance.HappySingerFX, smi.master.transform.GetPosition() + this.offset);
			smi.musicParticleFX.transform.SetParent(smi.master.transform);
			smi.CreatePasserbyReactable();
			smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
		}).Update(delegate(HappySinger.Instance smi, float dt)
		{
			if (!smi.GetSpeechMonitor().IsPlayingSpeech() && SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject))
			{
				smi.GetSpeechMonitor().PlaySpeech(Db.Get().Thoughts.CatchyTune.speechPrefix, Db.Get().Thoughts.CatchyTune.sound);
			}
		}, UpdateRate.SIM_1000ms, false).Exit(delegate(HappySinger.Instance smi)
		{
			Util.KDestroyGameObject(smi.musicParticleFX);
			smi.ClearPasserbyReactable();
			smi.musicParticleFX.SetActive(false);
		});
	}

	// Token: 0x04000B6A RID: 2922
	private Vector3 offset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04000B6B RID: 2923
	public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000B6C RID: 2924
	public HappySinger.OverjoyedStates overjoyed;

	// Token: 0x04000B6D RID: 2925
	public string soundPath = GlobalAssets.GetSound("DupeSinging_NotesFX_LP", false);

	// Token: 0x0200106D RID: 4205
	public class OverjoyedStates : GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005905 RID: 22789
		public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04005906 RID: 22790
		public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State moving;
	}

	// Token: 0x0200106E RID: 4206
	public new class Instance : GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060075A0 RID: 30112 RVA: 0x002CD480 File Offset: 0x002CB680
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060075A1 RID: 30113 RVA: 0x002CD48C File Offset: 0x002CB68C
		public void CreatePasserbyReactable()
		{
			if (this.passerbyReactable == null)
			{
				EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 0f, 600f, float.PositiveInfinity, 0f);
				Emote sing = Db.Get().Emotes.Minion.Sing;
				emoteReactable.SetEmote(sing).SetThought(Db.Get().Thoughts.CatchyTune).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
				emoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.AddReactionEffect), null);
				this.passerbyReactable = emoteReactable;
			}
		}

		// Token: 0x060075A2 RID: 30114 RVA: 0x002CD546 File Offset: 0x002CB746
		public SpeechMonitor.Instance GetSpeechMonitor()
		{
			if (this.speechMonitor == null)
			{
				this.speechMonitor = base.master.gameObject.GetSMI<SpeechMonitor.Instance>();
			}
			return this.speechMonitor;
		}

		// Token: 0x060075A3 RID: 30115 RVA: 0x002CD56C File Offset: 0x002CB76C
		private void AddReactionEffect(GameObject reactor)
		{
			reactor.Trigger(-1278274506, null);
		}

		// Token: 0x060075A4 RID: 30116 RVA: 0x002CD57A File Offset: 0x002CB77A
		private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
		{
			return transition.end == NavType.Floor;
		}

		// Token: 0x060075A5 RID: 30117 RVA: 0x002CD585 File Offset: 0x002CB785
		public void ClearPasserbyReactable()
		{
			if (this.passerbyReactable != null)
			{
				this.passerbyReactable.Cleanup();
				this.passerbyReactable = null;
			}
		}

		// Token: 0x04005907 RID: 22791
		private Reactable passerbyReactable;

		// Token: 0x04005908 RID: 22792
		public GameObject musicParticleFX;

		// Token: 0x04005909 RID: 22793
		public SpeechMonitor.Instance speechMonitor;
	}
}
