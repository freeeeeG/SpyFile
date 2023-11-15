using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000410 RID: 1040
public class ReactionMonitor : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>
{
	// Token: 0x060015F9 RID: 5625 RVA: 0x00073BE0 File Offset: 0x00071DE0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.EventHandler(GameHashes.DestinationReached, delegate(ReactionMonitor.Instance smi)
		{
			smi.ClearLastReaction();
		}).EventHandler(GameHashes.NavigationFailed, delegate(ReactionMonitor.Instance smi)
		{
			smi.ClearLastReaction();
		});
		this.idle.Enter("ClearReactable", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Set(null, smi, false);
		}).TagTransition(GameTags.Dead, this.dead, false);
		this.reacting.Enter("Reactable.Begin", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Get(smi).Begin(smi.gameObject);
		}).Enter(delegate(ReactionMonitor.Instance smi)
		{
			smi.master.Trigger(-909573545, null);
		}).Enter("Reactable.AddChorePreventionTag", delegate(ReactionMonitor.Instance smi)
		{
			if (this.reactable.Get(smi).preventChoreInterruption)
			{
				smi.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption, false);
			}
		}).Update("Reactable.Update", delegate(ReactionMonitor.Instance smi, float dt)
		{
			this.reactable.Get(smi).Update(dt);
		}, UpdateRate.SIM_200ms, false).Exit(delegate(ReactionMonitor.Instance smi)
		{
			smi.master.Trigger(824899998, null);
		}).Exit("Reactable.End", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Get(smi).End();
		}).Exit("Reactable.RemoveChorePreventionTag", delegate(ReactionMonitor.Instance smi)
		{
			if (this.reactable.Get(smi).preventChoreInterruption)
			{
				smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
			}
		}).EventTransition(GameHashes.NavigationFailed, this.idle, null).TagTransition(GameTags.Dying, this.dead, false).TagTransition(GameTags.Dead, this.dead, false);
		this.dead.DoNothing();
	}

	// Token: 0x060015FA RID: 5626 RVA: 0x00073D81 File Offset: 0x00071F81
	private static bool ShouldReact(ReactionMonitor.Instance smi)
	{
		return smi.ImmediateReactable != null;
	}

	// Token: 0x04000C4C RID: 3148
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State idle;

	// Token: 0x04000C4D RID: 3149
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State reacting;

	// Token: 0x04000C4E RID: 3150
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State dead;

	// Token: 0x04000C4F RID: 3151
	public StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.ObjectParameter<Reactable> reactable;

	// Token: 0x02001090 RID: 4240
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400598D RID: 22925
		public ObjectLayer ReactionLayer;
	}

	// Token: 0x02001091 RID: 4241
	public new class Instance : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.GameInstance
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x0600760C RID: 30220 RVA: 0x002CE565 File Offset: 0x002CC765
		// (set) Token: 0x0600760D RID: 30221 RVA: 0x002CE56D File Offset: 0x002CC76D
		public Reactable ImmediateReactable { get; private set; }

		// Token: 0x0600760E RID: 30222 RVA: 0x002CE576 File Offset: 0x002CC776
		public Instance(IStateMachineTarget master, ReactionMonitor.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			this.lastReactTimes = new Dictionary<HashedString, float>();
			this.oneshotReactables = new List<Reactable>();
		}

		// Token: 0x0600760F RID: 30223 RVA: 0x002CE5AD File Offset: 0x002CC7AD
		public bool CanReact(Emote e)
		{
			return this.animController != null && e.IsValidForController(this.animController);
		}

		// Token: 0x06007610 RID: 30224 RVA: 0x002CE5CC File Offset: 0x002CC7CC
		public bool TryReact(Reactable reactable, float clockTime, Navigator.ActiveTransition transition = null)
		{
			if (reactable == null)
			{
				return false;
			}
			float num;
			if ((this.lastReactTimes.TryGetValue(reactable.id, out num) && num == this.lastReaction) || clockTime - num < reactable.localCooldown)
			{
				return false;
			}
			if (!reactable.CanBegin(base.gameObject, transition))
			{
				return false;
			}
			this.lastReactTimes[reactable.id] = clockTime;
			base.sm.reactable.Set(reactable, base.smi, false);
			base.smi.GoTo(base.sm.reacting);
			return true;
		}

		// Token: 0x06007611 RID: 30225 RVA: 0x002CE65C File Offset: 0x002CC85C
		public void PollForReactables(Navigator.ActiveTransition transition)
		{
			if (this.IsReacting())
			{
				return;
			}
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				Reactable reactable = this.oneshotReactables[i];
				if (reactable.IsExpired())
				{
					reactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
				}
			}
			Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(base.smi.gameObject));
			ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(int)base.def.ReactionLayer];
			ListPool<ScenePartitionerEntry, ReactionMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ReactionMonitor>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, layer, pooledList);
			float num = float.NaN;
			float time = GameClock.Instance.GetTime();
			for (int j = 0; j < pooledList.Count; j++)
			{
				Reactable reactable2 = pooledList[j].obj as Reactable;
				if (this.TryReact(reactable2, time, transition))
				{
					num = time;
					break;
				}
			}
			this.lastReaction = num;
			pooledList.Recycle();
		}

		// Token: 0x06007612 RID: 30226 RVA: 0x002CE761 File Offset: 0x002CC961
		public void ClearLastReaction()
		{
			this.lastReaction = float.NaN;
		}

		// Token: 0x06007613 RID: 30227 RVA: 0x002CE770 File Offset: 0x002CC970
		public void StopReaction()
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				if (base.sm.reactable.Get(base.smi) == this.oneshotReactables[i])
				{
					this.oneshotReactables[i].Cleanup();
					this.oneshotReactables.RemoveAt(i);
					break;
				}
			}
			base.smi.GoTo(base.sm.idle);
		}

		// Token: 0x06007614 RID: 30228 RVA: 0x002CE7EE File Offset: 0x002CC9EE
		public bool IsReacting()
		{
			return base.smi.IsInsideState(base.sm.reacting);
		}

		// Token: 0x06007615 RID: 30229 RVA: 0x002CE808 File Offset: 0x002CCA08
		public SelfEmoteReactable AddSelfEmoteReactable(GameObject target, HashedString reactionId, Emote emote, bool isOneShot, ChoreType choreType, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.NegativeInfinity, float maxInitialDelay = 0f, List<Reactable.ReactablePrecondition> emotePreconditions = null)
		{
			if (!this.CanReact(emote))
			{
				return null;
			}
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(target, reactionId, choreType, globalCooldown, localCooldown, lifeSpan, maxInitialDelay);
			selfEmoteReactable.SetEmote(emote);
			int num = 0;
			while (emotePreconditions != null && num < emotePreconditions.Count)
			{
				selfEmoteReactable.AddPrecondition(emotePreconditions[num]);
				num++;
			}
			if (isOneShot)
			{
				this.AddOneshotReactable(selfEmoteReactable);
			}
			return selfEmoteReactable;
		}

		// Token: 0x06007616 RID: 30230 RVA: 0x002CE86C File Offset: 0x002CCA6C
		public SelfEmoteReactable AddSelfEmoteReactable(GameObject target, string reactionId, string emoteAnim, bool isOneShot, ChoreType choreType, float globalCooldown = 0f, float localCooldown = 20f, float maxTriggerTime = float.NegativeInfinity, float maxInitialDelay = 0f, List<Reactable.ReactablePrecondition> emotePreconditions = null)
		{
			Emote emote = new Emote(null, reactionId, new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "react"
				}
			}, emoteAnim);
			return this.AddSelfEmoteReactable(target, reactionId, emote, isOneShot, choreType, globalCooldown, localCooldown, maxTriggerTime, maxInitialDelay, emotePreconditions);
		}

		// Token: 0x06007617 RID: 30231 RVA: 0x002CE8BC File Offset: 0x002CCABC
		public void AddOneshotReactable(SelfEmoteReactable reactable)
		{
			if (reactable == null)
			{
				return;
			}
			this.oneshotReactables.Add(reactable);
		}

		// Token: 0x06007618 RID: 30232 RVA: 0x002CE8D0 File Offset: 0x002CCAD0
		public void CancelOneShotReactable(SelfEmoteReactable cancel_target)
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				Reactable reactable = this.oneshotReactables[i];
				if (cancel_target == reactable)
				{
					reactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06007619 RID: 30233 RVA: 0x002CE91C File Offset: 0x002CCB1C
		public void CancelOneShotReactables(Emote reactionEmote)
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				EmoteReactable emoteReactable = this.oneshotReactables[i] as EmoteReactable;
				if (emoteReactable != null && emoteReactable.emote == reactionEmote)
				{
					emoteReactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
				}
			}
		}

		// Token: 0x0400598F RID: 22927
		private KBatchedAnimController animController;

		// Token: 0x04005990 RID: 22928
		private float lastReaction = float.NaN;

		// Token: 0x04005991 RID: 22929
		private Dictionary<HashedString, float> lastReactTimes;

		// Token: 0x04005992 RID: 22930
		private List<Reactable> oneshotReactables;
	}
}
