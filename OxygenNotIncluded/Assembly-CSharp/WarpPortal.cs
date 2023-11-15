using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006B8 RID: 1720
public class WarpPortal : Workable
{
	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06002ECA RID: 11978 RVA: 0x000F73E0 File Offset: 0x000F55E0
	public bool ReadyToWarp
	{
		get
		{
			return this.warpPortalSMI.IsInsideState(this.warpPortalSMI.sm.occupied.waiting);
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06002ECB RID: 11979 RVA: 0x000F7402 File Offset: 0x000F5602
	public bool IsWorking
	{
		get
		{
			return this.warpPortalSMI.IsInsideState(this.warpPortalSMI.sm.occupied);
		}
	}

	// Token: 0x06002ECC RID: 11980 RVA: 0x000F741F File Offset: 0x000F561F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.assignable.OnAssign += this.Assign;
	}

	// Token: 0x06002ECD RID: 11981 RVA: 0x000F7440 File Offset: 0x000F5640
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.warpPortalSMI = new WarpPortal.WarpPortalSM.Instance(this);
		this.warpPortalSMI.sm.isCharged.Set(!this.IsConsumed, this.warpPortalSMI, false);
		this.warpPortalSMI.StartSM();
		this.selectEventHandle = Game.Instance.Subscribe(-1503271301, new Action<object>(this.OnObjectSelected));
	}

	// Token: 0x06002ECE RID: 11982 RVA: 0x000F74B1 File Offset: 0x000F56B1
	private void OnObjectSelected(object data)
	{
		if (data != null && (GameObject)data == base.gameObject && Components.LiveMinionIdentities.Count > 0)
		{
			this.Discover();
		}
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x000F74DC File Offset: 0x000F56DC
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(this.selectEventHandle);
		base.OnCleanUp();
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x000F74F4 File Offset: 0x000F56F4
	private void Discover()
	{
		if (this.discovered)
		{
			return;
		}
		ClusterManager.Instance.GetWorld(this.GetTargetWorldID()).SetDiscovered(true);
		SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.WarpWorldReveal, -1, null).smi as SimpleEvent.StatesInstance;
		statesInstance.minions = new GameObject[]
		{
			Components.LiveMinionIdentities[0].gameObject
		};
		statesInstance.callback = delegate()
		{
			ManagementMenu.Instance.OpenClusterMap();
			ClusterMapScreen.Instance.SetTargetFocusPosition(ClusterManager.Instance.GetWorld(this.GetTargetWorldID()).GetMyWorldLocation(), 0.5f);
		};
		statesInstance.ShowEventPopup();
		this.discovered = true;
	}

	// Token: 0x06002ED1 RID: 11985 RVA: 0x000F7584 File Offset: 0x000F5784
	public void StartWarpSequence()
	{
		this.warpPortalSMI.GoTo(this.warpPortalSMI.sm.occupied.warping);
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x000F75A6 File Offset: 0x000F57A6
	public void CancelAssignment()
	{
		this.CancelChore();
		this.assignable.Unassign();
		this.warpPortalSMI.GoTo(this.warpPortalSMI.sm.idle);
	}

	// Token: 0x06002ED3 RID: 11987 RVA: 0x000F75D4 File Offset: 0x000F57D4
	private int GetTargetWorldID()
	{
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
		foreach (WarpReceiver component in UnityEngine.Object.FindObjectsOfType<WarpReceiver>())
		{
			if (component.GetMyWorldId() != this.GetMyWorldId())
			{
				return component.GetMyWorldId();
			}
		}
		global::Debug.LogError("No receiver world found for warp portal sender");
		return -1;
	}

	// Token: 0x06002ED4 RID: 11988 RVA: 0x000F7630 File Offset: 0x000F5830
	private void Warp()
	{
		if (base.worker == null || base.worker.HasTag(GameTags.Dying) || base.worker.HasTag(GameTags.Dead))
		{
			return;
		}
		WarpReceiver warpReceiver = null;
		foreach (WarpReceiver warpReceiver2 in UnityEngine.Object.FindObjectsOfType<WarpReceiver>())
		{
			if (warpReceiver2.GetMyWorldId() != this.GetMyWorldId())
			{
				warpReceiver = warpReceiver2;
				break;
			}
		}
		if (warpReceiver == null)
		{
			SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
			warpReceiver = UnityEngine.Object.FindObjectOfType<WarpReceiver>();
		}
		if (warpReceiver != null)
		{
			this.delayWarpRoutine = base.StartCoroutine(this.DelayedWarp(warpReceiver));
		}
		else
		{
			global::Debug.LogWarning("No warp receiver found - maybe POI stomping or failure to spawn?");
		}
		if (SelectTool.Instance.selected == base.GetComponent<KSelectable>())
		{
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x06002ED5 RID: 11989 RVA: 0x000F770A File Offset: 0x000F590A
	public IEnumerator DelayedWarp(WarpReceiver receiver)
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		int myWorldId = base.worker.GetMyWorldId();
		int myWorldId2 = receiver.GetMyWorldId();
		CameraController.Instance.ActiveWorldStarWipe(myWorldId2, Grid.CellToPos(Grid.PosToCell(receiver)), 10f, null);
		Worker worker = base.worker;
		worker.StopWork();
		receiver.ReceiveWarpedDuplicant(worker);
		ClusterManager.Instance.MigrateMinion(worker.GetComponent<MinionIdentity>(), myWorldId2, myWorldId);
		this.delayWarpRoutine = null;
		yield break;
	}

	// Token: 0x06002ED6 RID: 11990 RVA: 0x000F7720 File Offset: 0x000F5920
	public void SetAssignable(bool set_it)
	{
		this.assignable.SetCanBeAssigned(set_it);
		this.RefreshSideScreen();
	}

	// Token: 0x06002ED7 RID: 11991 RVA: 0x000F7734 File Offset: 0x000F5934
	private void Assign(IAssignableIdentity new_assignee)
	{
		this.CancelChore();
		if (new_assignee != null)
		{
			this.ActivateChore();
		}
	}

	// Token: 0x06002ED8 RID: 11992 RVA: 0x000F7748 File Offset: 0x000F5948
	private void ActivateChore()
	{
		global::Debug.Assert(this.chore == null);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.Migrate, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim("anim_interacts_warp_portal_sender_kanim"), false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		this.workLayer = Grid.SceneLayer.Building;
		this.workAnims = new HashedString[]
		{
			"sending_pre",
			"sending_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"sending_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"idle_loop"
		};
		this.showProgressBar = false;
	}

	// Token: 0x06002ED9 RID: 11993 RVA: 0x000F782A File Offset: 0x000F5A2A
	private void CancelChore()
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
		if (this.delayWarpRoutine != null)
		{
			base.StopCoroutine(this.delayWarpRoutine);
			this.delayWarpRoutine = null;
		}
	}

	// Token: 0x06002EDA RID: 11994 RVA: 0x000F7867 File Offset: 0x000F5A67
	private void CompleteChore()
	{
		this.IsConsumed = true;
		this.chore.Cleanup();
		this.chore = null;
	}

	// Token: 0x06002EDB RID: 11995 RVA: 0x000F7882 File Offset: 0x000F5A82
	public void RefreshSideScreen()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x04001BBC RID: 7100
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x04001BBD RID: 7101
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001BBE RID: 7102
	private Chore chore;

	// Token: 0x04001BBF RID: 7103
	private WarpPortal.WarpPortalSM.Instance warpPortalSMI;

	// Token: 0x04001BC0 RID: 7104
	private Notification notification;

	// Token: 0x04001BC1 RID: 7105
	public const float RECHARGE_TIME = 3000f;

	// Token: 0x04001BC2 RID: 7106
	[Serialize]
	public bool IsConsumed;

	// Token: 0x04001BC3 RID: 7107
	[Serialize]
	public float rechargeProgress;

	// Token: 0x04001BC4 RID: 7108
	[Serialize]
	private bool discovered;

	// Token: 0x04001BC5 RID: 7109
	private int selectEventHandle = -1;

	// Token: 0x04001BC6 RID: 7110
	private Coroutine delayWarpRoutine;

	// Token: 0x04001BC7 RID: 7111
	private static readonly HashedString[] printing_anim = new HashedString[]
	{
		"printing_pre",
		"printing_loop",
		"printing_pst"
	};

	// Token: 0x020013F8 RID: 5112
	public class WarpPortalSM : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal>
	{
		// Token: 0x060082F6 RID: 33526 RVA: 0x002FBDEC File Offset: 0x002F9FEC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				if (smi.master.rechargeProgress != 0f)
				{
					smi.GoTo(this.recharging);
				}
			}).DefaultState(this.idle);
			this.idle.PlayAnim("idle", KAnim.PlayMode.Loop).Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.IsConsumed = false;
				smi.sm.isCharged.Set(true, smi, false);
				smi.master.SetAssignable(true);
			}).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.SetAssignable(false);
			}).WorkableStartTransition((WarpPortal.WarpPortalSM.Instance smi) => smi.master, this.become_occupied).ParamTransition<bool>(this.isCharged, this.recharging, GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.IsFalse);
			this.become_occupied.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				this.worker.Set(smi.master.worker, smi);
				smi.GoTo(this.occupied.get_on);
			});
			this.occupied.OnTargetLost(this.worker, this.idle).Target(this.worker).TagTransition(GameTags.Dying, this.idle, false).Target(this.masterTarget).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				this.worker.Set(null, smi);
			});
			this.occupied.get_on.PlayAnim("sending_pre").OnAnimQueueComplete(this.occupied.waiting);
			this.occupied.waiting.PlayAnim("sending_loop", KAnim.PlayMode.Loop).ToggleNotification((WarpPortal.WarpPortalSM.Instance smi) => smi.CreateDupeWaitingNotification()).Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			}).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			});
			this.occupied.warping.PlayAnim("sending_pst").OnAnimQueueComplete(this.do_warp);
			this.do_warp.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.Warp();
			}).GoTo(this.recharging);
			this.recharging.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.SetAssignable(false);
				smi.master.IsConsumed = true;
				this.isCharged.Set(false, smi, false);
			}).PlayAnim("recharge", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.WarpPortalCharging, (WarpPortal.WarpPortalSM.Instance smi) => smi.master).Update(delegate(WarpPortal.WarpPortalSM.Instance smi, float dt)
			{
				smi.master.rechargeProgress += dt;
				if (smi.master.rechargeProgress > 3000f)
				{
					this.isCharged.Set(true, smi, false);
					smi.master.rechargeProgress = 0f;
					smi.GoTo(this.idle);
				}
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x040063F2 RID: 25586
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State idle;

		// Token: 0x040063F3 RID: 25587
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State become_occupied;

		// Token: 0x040063F4 RID: 25588
		public WarpPortal.WarpPortalSM.OccupiedStates occupied;

		// Token: 0x040063F5 RID: 25589
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State do_warp;

		// Token: 0x040063F6 RID: 25590
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State recharging;

		// Token: 0x040063F7 RID: 25591
		public StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.BoolParameter isCharged;

		// Token: 0x040063F8 RID: 25592
		private StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.TargetParameter worker;

		// Token: 0x02002146 RID: 8518
		public class OccupiedStates : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State
		{
			// Token: 0x0400953A RID: 38202
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State get_on;

			// Token: 0x0400953B RID: 38203
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State waiting;

			// Token: 0x0400953C RID: 38204
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State warping;
		}

		// Token: 0x02002147 RID: 8519
		public new class Instance : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.GameInstance
		{
			// Token: 0x0600A9AD RID: 43437 RVA: 0x003723B2 File Offset: 0x003705B2
			public Instance(WarpPortal master) : base(master)
			{
			}

			// Token: 0x0600A9AE RID: 43438 RVA: 0x003723BC File Offset: 0x003705BC
			public Notification CreateDupeWaitingNotification()
			{
				if (base.master.worker != null)
				{
					return new Notification(MISC.NOTIFICATIONS.WARP_PORTAL_DUPE_READY.NAME.Replace("{dupe}", base.master.worker.name), NotificationType.Neutral, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.WARP_PORTAL_DUPE_READY.TOOLTIP.Replace("{dupe}", base.master.worker.name), null, false, 0f, null, null, base.master.transform, true, false, false);
				}
				return null;
			}
		}
	}
}
