using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200087B RID: 2171
public class GameplayEventMonitor : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>
{
	// Token: 0x06003F4D RID: 16205 RVA: 0x00161870 File Offset: 0x0015FA70
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
		default_state = this.idle;
		this.root.EventHandler(GameHashes.GameplayEventMonitorStart, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnMonitorStart(data);
		}).EventHandler(GameHashes.GameplayEventMonitorEnd, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnMonitorEnd(data);
		}).EventHandler(GameHashes.GameplayEventMonitorChanged, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			this.UpdateFX(smi);
		});
		this.idle.EventTransition(GameHashes.GameplayEventMonitorStart, this.activeState, new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.HasEvents)).Enter(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State.Callback(this.UpdateEventDisplay));
		this.activeState.DefaultState(this.activeState.unseenEvents);
		this.activeState.unseenEvents.ToggleFX(new Func<GameplayEventMonitor.Instance, StateMachine.Instance>(this.CreateFX)).EventHandler(GameHashes.SelectObject, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnSelect(data);
		}).EventTransition(GameHashes.GameplayEventMonitorChanged, this.activeState.seenAllEvents, new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.SeenAll));
		this.activeState.seenAllEvents.EventTransition(GameHashes.GameplayEventMonitorStart, this.activeState.unseenEvents, GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Not(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.SeenAll))).Enter(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State.Callback(this.UpdateEventDisplay));
	}

	// Token: 0x06003F4E RID: 16206 RVA: 0x001619F0 File Offset: 0x0015FBF0
	private bool HasEvents(GameplayEventMonitor.Instance smi)
	{
		return smi.events.Count > 0;
	}

	// Token: 0x06003F4F RID: 16207 RVA: 0x00161A00 File Offset: 0x0015FC00
	private bool SeenAll(GameplayEventMonitor.Instance smi)
	{
		return smi.UnseenCount() == 0;
	}

	// Token: 0x06003F50 RID: 16208 RVA: 0x00161A0B File Offset: 0x0015FC0B
	private void UpdateFX(GameplayEventMonitor.Instance smi)
	{
		if (smi.fx != null)
		{
			smi.fx.sm.notificationCount.Set(smi.UnseenCount(), smi.fx, false);
		}
	}

	// Token: 0x06003F51 RID: 16209 RVA: 0x00161A38 File Offset: 0x0015FC38
	private GameplayEventFX.Instance CreateFX(GameplayEventMonitor.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			smi.fx = new GameplayEventFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f));
			return smi.fx;
		}
		return null;
	}

	// Token: 0x06003F52 RID: 16210 RVA: 0x00161A70 File Offset: 0x0015FC70
	public void UpdateEventDisplay(GameplayEventMonitor.Instance smi)
	{
		if (smi.events.Count == 0 || smi.UnseenCount() > 0)
		{
			NameDisplayScreen.Instance.SetGameplayEventDisplay(smi.master.gameObject, false, null, null);
			return;
		}
		int num = -1;
		GameplayEvent gameplayEvent = null;
		foreach (GameplayEventInstance gameplayEventInstance in smi.events)
		{
			Sprite displaySprite = gameplayEventInstance.gameplayEvent.GetDisplaySprite();
			if (gameplayEventInstance.gameplayEvent.importance > num && displaySprite != null)
			{
				num = gameplayEventInstance.gameplayEvent.importance;
				gameplayEvent = gameplayEventInstance.gameplayEvent;
			}
		}
		if (gameplayEvent != null)
		{
			NameDisplayScreen.Instance.SetGameplayEventDisplay(smi.master.gameObject, true, gameplayEvent.GetDisplayString(), gameplayEvent.GetDisplaySprite());
		}
	}

	// Token: 0x0400290F RID: 10511
	public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State idle;

	// Token: 0x04002910 RID: 10512
	public GameplayEventMonitor.ActiveState activeState;

	// Token: 0x02001675 RID: 5749
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001676 RID: 5750
	public class ActiveState : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State
	{
		// Token: 0x04006BC2 RID: 27586
		public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State unseenEvents;

		// Token: 0x04006BC3 RID: 27587
		public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State seenAllEvents;
	}

	// Token: 0x02001677 RID: 5751
	public new class Instance : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.GameInstance
	{
		// Token: 0x06008AF1 RID: 35569 RVA: 0x00314E87 File Offset: 0x00313087
		public Instance(IStateMachineTarget master, GameplayEventMonitor.Def def) : base(master, def)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x06008AF2 RID: 35570 RVA: 0x00314EB0 File Offset: 0x003130B0
		public void OnMonitorStart(object data)
		{
			GameplayEventInstance gameplayEventInstance = data as GameplayEventInstance;
			if (!this.events.Contains(gameplayEventInstance))
			{
				this.events.Add(gameplayEventInstance);
				gameplayEventInstance.RegisterMonitorCallback(base.gameObject);
			}
			base.smi.sm.UpdateFX(base.smi);
			base.smi.sm.UpdateEventDisplay(base.smi);
		}

		// Token: 0x06008AF3 RID: 35571 RVA: 0x00314F18 File Offset: 0x00313118
		public void OnMonitorEnd(object data)
		{
			GameplayEventInstance gameplayEventInstance = data as GameplayEventInstance;
			if (this.events.Contains(gameplayEventInstance))
			{
				this.events.Remove(gameplayEventInstance);
				gameplayEventInstance.UnregisterMonitorCallback(base.gameObject);
			}
			base.smi.sm.UpdateFX(base.smi);
			base.smi.sm.UpdateEventDisplay(base.smi);
			if (this.events.Count == 0)
			{
				base.smi.GoTo(base.sm.idle);
			}
		}

		// Token: 0x06008AF4 RID: 35572 RVA: 0x00314FA4 File Offset: 0x003131A4
		public void OnSelect(object data)
		{
			if (!(bool)data)
			{
				return;
			}
			foreach (GameplayEventInstance gameplayEventInstance in this.events)
			{
				if (!gameplayEventInstance.seenNotification && gameplayEventInstance.GetEventPopupData != null)
				{
					gameplayEventInstance.seenNotification = true;
					EventInfoScreen.ShowPopup(gameplayEventInstance.GetEventPopupData());
					break;
				}
			}
			if (this.UnseenCount() == 0)
			{
				base.smi.GoTo(base.sm.activeState.seenAllEvents);
			}
		}

		// Token: 0x06008AF5 RID: 35573 RVA: 0x00315048 File Offset: 0x00313248
		public int UnseenCount()
		{
			return this.events.Count((GameplayEventInstance evt) => !evt.seenNotification);
		}

		// Token: 0x04006BC4 RID: 27588
		public List<GameplayEventInstance> events = new List<GameplayEventInstance>();

		// Token: 0x04006BC5 RID: 27589
		public GameplayEventFX.Instance fx;
	}
}
