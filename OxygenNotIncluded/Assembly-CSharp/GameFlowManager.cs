using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007CD RID: 1997
[SerializationConfig(MemberSerialization.OptIn)]
public class GameFlowManager : StateMachineComponent<GameFlowManager.StatesInstance>, ISaveLoadable
{
	// Token: 0x060037CA RID: 14282 RVA: 0x0012F2B7 File Offset: 0x0012D4B7
	public static void DestroyInstance()
	{
		GameFlowManager.Instance = null;
	}

	// Token: 0x060037CB RID: 14283 RVA: 0x0012F2BF File Offset: 0x0012D4BF
	protected override void OnPrefabInit()
	{
		GameFlowManager.Instance = this;
	}

	// Token: 0x060037CC RID: 14284 RVA: 0x0012F2C7 File Offset: 0x0012D4C7
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x060037CD RID: 14285 RVA: 0x0012F2D4 File Offset: 0x0012D4D4
	public bool IsGameOver()
	{
		return base.smi.IsInsideState(base.smi.sm.gameover);
	}

	// Token: 0x040022A4 RID: 8868
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x040022A5 RID: 8869
	public static GameFlowManager Instance;

	// Token: 0x02001557 RID: 5463
	public class StatesInstance : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.GameInstance
	{
		// Token: 0x0600876D RID: 34669 RVA: 0x0030B04F File Offset: 0x0030924F
		public bool IsIncapacitated(GameObject go)
		{
			return false;
		}

		// Token: 0x0600876E RID: 34670 RVA: 0x0030B054 File Offset: 0x00309254
		public void CheckForGameOver()
		{
			if (!Game.Instance.GameStarted())
			{
				return;
			}
			if (GenericGameSettings.instance.disableGameOver)
			{
				return;
			}
			bool flag = false;
			if (Components.LiveMinionIdentities.Count == 0)
			{
				flag = true;
			}
			else
			{
				flag = true;
				foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
				{
					if (!this.IsIncapacitated(minionIdentity.gameObject))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				this.GoTo(base.sm.gameover.pending);
			}
		}

		// Token: 0x0600876F RID: 34671 RVA: 0x0030B100 File Offset: 0x00309300
		public StatesInstance(GameFlowManager smi) : base(smi)
		{
		}

		// Token: 0x04006800 RID: 26624
		public Notification colonyLostNotification = new Notification(MISC.NOTIFICATIONS.COLONYLOST.NAME, NotificationType.Bad, null, null, false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x02001558 RID: 5464
	public class States : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager>
	{
		// Token: 0x06008770 RID: 34672 RVA: 0x0030B138 File Offset: 0x00309338
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.loading;
			this.loading.ScheduleGoTo(4f, this.running);
			this.running.Update("CheckForGameOver", delegate(GameFlowManager.StatesInstance smi, float dt)
			{
				smi.CheckForGameOver();
			}, UpdateRate.SIM_200ms, false);
			this.gameover.TriggerOnEnter(GameHashes.GameOver, null).ToggleNotification((GameFlowManager.StatesInstance smi) => smi.colonyLostNotification);
			this.gameover.pending.Enter("Goto(gameover.active)", delegate(GameFlowManager.StatesInstance smi)
			{
				UIScheduler.Instance.Schedule("Goto(gameover.active)", 4f, delegate(object d)
				{
					smi.GoTo(this.gameover.active);
				}, null, null);
			});
			this.gameover.active.Enter(delegate(GameFlowManager.StatesInstance smi)
			{
				if (GenericGameSettings.instance.demoMode)
				{
					DemoTimer.Instance.EndDemo();
					return;
				}
				GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.GameOverScreen, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<KScreen>().Show(true);
			});
		}

		// Token: 0x04006801 RID: 26625
		public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State loading;

		// Token: 0x04006802 RID: 26626
		public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State running;

		// Token: 0x04006803 RID: 26627
		public GameFlowManager.States.GameOverState gameover;

		// Token: 0x0200217D RID: 8573
		public class GameOverState : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State
		{
			// Token: 0x040095FC RID: 38396
			public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State pending;

			// Token: 0x040095FD RID: 38397
			public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State active;
		}
	}
}
