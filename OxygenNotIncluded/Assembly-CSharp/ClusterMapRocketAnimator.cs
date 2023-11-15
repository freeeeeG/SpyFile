using System;
using UnityEngine;

// Token: 0x020009C8 RID: 2504
public class ClusterMapRocketAnimator : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x06004ADF RID: 19167 RVA: 0x001A5674 File Offset: 0x001A3874
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.idle;
		this.root.Transition(null, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.entityTarget.IsNull), UpdateRate.SIM_200ms).Target(this.entityTarget).EventHandlerTransition(GameHashes.RocketSelfDestructRequested, this.exploding, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true).EventHandlerTransition(GameHashes.StartMining, this.utility.mining, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true).EventHandlerTransition(GameHashes.RocketLaunched, this.moving.takeoff, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true);
		this.idle.Target(this.masterTarget).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("idle_loop", KAnim.PlayMode.Loop);
		}).Target(this.entityTarget).Transition(this.moving.traveling, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling), UpdateRate.SIM_200ms).Transition(this.grounded, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsGrounded), UpdateRate.SIM_200ms).Transition(this.moving.landing, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsLanding), UpdateRate.SIM_200ms).Transition(this.utility.mining, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsMining), UpdateRate.SIM_200ms);
		this.grounded.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(false, smi);
			smi.ToggleVisAnim(false);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
			smi.ToggleVisAnim(true);
		}).Target(this.entityTarget).EventTransition(GameHashes.RocketLaunched, this.moving.takeoff, null);
		this.moving.takeoff.Transition(this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("launching", KAnim.PlayMode.Loop);
			this.ToggleSelectable(false, smi);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
		});
		this.moving.landing.Transition(this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("landing", KAnim.PlayMode.Loop);
			this.ToggleSelectable(false, smi);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
		});
		this.moving.traveling.DefaultState(this.moving.traveling.regular).Target(this.entityTarget).EventTransition(GameHashes.ClusterLocationChanged, this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling))).EventTransition(GameHashes.ClusterDestinationChanged, this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)));
		this.moving.traveling.regular.Target(this.entityTarget).Transition(this.moving.traveling.boosted, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsBoosted), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop);
		});
		this.moving.traveling.boosted.Target(this.entityTarget).Transition(this.moving.traveling.regular, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsBoosted)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("boosted", KAnim.PlayMode.Loop);
		});
		this.utility.Target(this.masterTarget).EventTransition(GameHashes.ClusterDestinationChanged, this.idle, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling));
		this.utility.mining.DefaultState(this.utility.mining.pre).Target(this.entityTarget).EventTransition(GameHashes.StopMining, this.utility.mining.pst, null);
		this.utility.mining.pre.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_pre", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.utility.mining.loop);
			});
		});
		this.utility.mining.loop.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_loop", KAnim.PlayMode.Loop);
		});
		this.utility.mining.pst.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_pst", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.idle);
			});
		});
		this.exploding.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("rocket_self_destruct_kanim")
			});
			smi.PlayVisAnim("explode", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.exploding_pst);
			});
		});
		this.exploding_pst.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().Stop();
			smi.entity.gameObject.Trigger(-1311384361, null);
		});
	}

	// Token: 0x06004AE0 RID: 19168 RVA: 0x001A5B44 File Offset: 0x001A3D44
	private bool ClusterChangedAtMyLocation(ClusterMapRocketAnimator.StatesInstance smi, object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		return clusterLocationChangedEvent.oldLocation == smi.entity.Location || clusterLocationChangedEvent.newLocation == smi.entity.Location;
	}

	// Token: 0x06004AE1 RID: 19169 RVA: 0x001A5B88 File Offset: 0x001A3D88
	private bool IsTraveling(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return smi.entity.GetComponent<ClusterTraveler>().IsTraveling() && ((Clustercraft)smi.entity).HasResourcesToMove(1, Clustercraft.CombustionResource.All);
	}

	// Token: 0x06004AE2 RID: 19170 RVA: 0x001A5BB0 File Offset: 0x001A3DB0
	private bool IsBoosted(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).controlStationBuffTimeRemaining > 0f;
	}

	// Token: 0x06004AE3 RID: 19171 RVA: 0x001A5BC9 File Offset: 0x001A3DC9
	private bool IsGrounded(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).Status == Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x06004AE4 RID: 19172 RVA: 0x001A5BDE File Offset: 0x001A3DDE
	private bool IsLanding(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).Status == Clustercraft.CraftStatus.Landing;
	}

	// Token: 0x06004AE5 RID: 19173 RVA: 0x001A5BF3 File Offset: 0x001A3DF3
	private bool IsMining(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).HasTag(GameTags.POIHarvesting);
	}

	// Token: 0x06004AE6 RID: 19174 RVA: 0x001A5C0C File Offset: 0x001A3E0C
	private bool IsSurfaceTransitioning(ClusterMapRocketAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Landing || clustercraft.Status == Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x06004AE7 RID: 19175 RVA: 0x001A5C44 File Offset: 0x001A3E44
	private void ToggleSelectable(bool isSelectable, ClusterMapRocketAnimator.StatesInstance smi)
	{
		if (smi.entity.IsNullOrDestroyed())
		{
			return;
		}
		KSelectable component = smi.entity.GetComponent<KSelectable>();
		component.IsSelectable = isSelectable;
		if (!isSelectable && component.IsSelected && ClusterMapScreen.Instance.GetMode() != ClusterMapScreen.Mode.SelectDestination)
		{
			ClusterMapSelectTool.Instance.Select(null, true);
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x0400311C RID: 12572
	public StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x0400311D RID: 12573
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;

	// Token: 0x0400311E RID: 12574
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State grounded;

	// Token: 0x0400311F RID: 12575
	public ClusterMapRocketAnimator.MovingStates moving;

	// Token: 0x04003120 RID: 12576
	public ClusterMapRocketAnimator.UtilityStates utility;

	// Token: 0x04003121 RID: 12577
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State exploding;

	// Token: 0x04003122 RID: 12578
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State exploding_pst;

	// Token: 0x02001857 RID: 6231
	public class TravelingStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x040071B7 RID: 29111
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State regular;

		// Token: 0x040071B8 RID: 29112
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State boosted;
	}

	// Token: 0x02001858 RID: 6232
	public class MovingStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x040071B9 RID: 29113
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State takeoff;

		// Token: 0x040071BA RID: 29114
		public ClusterMapRocketAnimator.TravelingStates traveling;

		// Token: 0x040071BB RID: 29115
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State landing;
	}

	// Token: 0x02001859 RID: 6233
	public class UtilityStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x040071BC RID: 29116
		public ClusterMapRocketAnimator.UtilityStates.MiningStates mining;

		// Token: 0x020021F9 RID: 8697
		public class MiningStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
		{
			// Token: 0x04009812 RID: 38930
			public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State pre;

			// Token: 0x04009813 RID: 38931
			public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State loop;

			// Token: 0x04009814 RID: 38932
			public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State pst;
		}
	}

	// Token: 0x0200185A RID: 6234
	public class StatesInstance : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600918A RID: 37258 RVA: 0x00329C24 File Offset: 0x00327E24
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x0600918B RID: 37259 RVA: 0x00329C4D File Offset: 0x00327E4D
		public void PlayVisAnim(string animName, KAnim.PlayMode playMode)
		{
			base.GetComponent<ClusterMapVisualizer>().PlayAnim(animName, playMode);
		}

		// Token: 0x0600918C RID: 37260 RVA: 0x00329C5C File Offset: 0x00327E5C
		public void ToggleVisAnim(bool on)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			if (!on)
			{
				component.GetFirstAnimController().Play("grounded", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x0600918D RID: 37261 RVA: 0x00329C94 File Offset: 0x00327E94
		public void SubscribeOnVisAnimComplete(Action<object> action)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			this.UnsubscribeOnVisAnimComplete();
			this.animCompleteSubscriber = component.GetFirstAnimController().gameObject;
			this.animCompleteHandle = this.animCompleteSubscriber.Subscribe(-1061186183, action);
		}

		// Token: 0x0600918E RID: 37262 RVA: 0x00329CD6 File Offset: 0x00327ED6
		public void UnsubscribeOnVisAnimComplete()
		{
			if (this.animCompleteHandle != -1)
			{
				DebugUtil.DevAssert(this.animCompleteSubscriber != null, "ClusterMapRocketAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly", null);
				this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
				this.animCompleteHandle = -1;
			}
		}

		// Token: 0x0600918F RID: 37263 RVA: 0x00329D10 File Offset: 0x00327F10
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			this.UnsubscribeOnVisAnimComplete();
		}

		// Token: 0x040071BD RID: 29117
		public ClusterGridEntity entity;

		// Token: 0x040071BE RID: 29118
		private int animCompleteHandle = -1;

		// Token: 0x040071BF RID: 29119
		private GameObject animCompleteSubscriber;
	}
}
