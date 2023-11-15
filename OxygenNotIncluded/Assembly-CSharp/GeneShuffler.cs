using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000608 RID: 1544
[AddComponentMenu("KMonoBehaviour/Workable/GeneShuffler")]
public class GeneShuffler : Workable
{
	// Token: 0x1700020D RID: 525
	// (get) Token: 0x060026BF RID: 9919 RVA: 0x000D24C8 File Offset: 0x000D06C8
	public bool WorkComplete
	{
		get
		{
			return this.geneShufflerSMI.IsInsideState(this.geneShufflerSMI.sm.working.complete);
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x060026C0 RID: 9920 RVA: 0x000D24EA File Offset: 0x000D06EA
	public bool IsWorking
	{
		get
		{
			return this.geneShufflerSMI.IsInsideState(this.geneShufflerSMI.sm.working);
		}
	}

	// Token: 0x060026C1 RID: 9921 RVA: 0x000D2507 File Offset: 0x000D0707
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.assignable.OnAssign += this.Assign;
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x060026C2 RID: 9922 RVA: 0x000D2530 File Offset: 0x000D0730
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.showProgressBar = false;
		this.geneShufflerSMI = new GeneShuffler.GeneShufflerSM.Instance(this);
		this.RefreshRechargeChore();
		this.RefreshConsumedState();
		base.Subscribe<GeneShuffler>(-1697596308, GeneShuffler.OnStorageChangeDelegate);
		this.geneShufflerSMI.StartSM();
	}

	// Token: 0x060026C3 RID: 9923 RVA: 0x000D257E File Offset: 0x000D077E
	private void Assign(IAssignableIdentity new_assignee)
	{
		this.CancelChore();
		if (new_assignee != null)
		{
			this.ActivateChore();
		}
	}

	// Token: 0x060026C4 RID: 9924 RVA: 0x000D258F File Offset: 0x000D078F
	private void Recharge()
	{
		this.SetConsumed(false);
		this.RequestRecharge(false);
		this.RefreshRechargeChore();
		this.RefreshSideScreen();
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x000D25AB File Offset: 0x000D07AB
	private void SetConsumed(bool consumed)
	{
		this.IsConsumed = consumed;
		this.RefreshConsumedState();
	}

	// Token: 0x060026C6 RID: 9926 RVA: 0x000D25BA File Offset: 0x000D07BA
	private void RefreshConsumedState()
	{
		this.geneShufflerSMI.sm.isCharged.Set(!this.IsConsumed, this.geneShufflerSMI, false);
	}

	// Token: 0x060026C7 RID: 9927 RVA: 0x000D25E4 File Offset: 0x000D07E4
	private void OnStorageChange(object data)
	{
		if (this.storage_recursion_guard)
		{
			return;
		}
		this.storage_recursion_guard = true;
		if (this.IsConsumed)
		{
			for (int i = this.storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this.storage.items[i];
				if (!(gameObject == null) && gameObject.IsPrefabID(GeneShuffler.RechargeTag))
				{
					this.storage.ConsumeIgnoringDisease(gameObject);
					this.Recharge();
					break;
				}
			}
		}
		this.storage_recursion_guard = false;
	}

	// Token: 0x060026C8 RID: 9928 RVA: 0x000D266C File Offset: 0x000D086C
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.notification = new Notification(MISC.NOTIFICATIONS.GENESHUFFLER.NAME, NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.GENESHUFFLER.TOOLTIP + notificationList.ReduceMessages(false), null, false, 0f, null, null, null, true, false, false);
		this.notifier.Add(this.notification, "");
		this.DeSelectBuilding();
	}

	// Token: 0x060026C9 RID: 9929 RVA: 0x000D26DE File Offset: 0x000D08DE
	private void DeSelectBuilding()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x060026CA RID: 9930 RVA: 0x000D26F9 File Offset: 0x000D08F9
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060026CB RID: 9931 RVA: 0x000D2703 File Offset: 0x000D0903
	protected override void OnAbortWork(Worker worker)
	{
		base.OnAbortWork(worker);
		if (this.chore != null)
		{
			this.chore.Cancel("aborted");
		}
		this.notifier.Remove(this.notification);
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x000D2735 File Offset: 0x000D0935
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		if (this.chore != null)
		{
			this.chore.Cancel("stopped");
		}
		this.notifier.Remove(this.notification);
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x000D2768 File Offset: 0x000D0968
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		CameraController.Instance.CameraGoTo(base.transform.GetPosition(), 1f, false);
		this.ApplyRandomTrait(worker);
		this.assignable.Unassign();
		this.DeSelectBuilding();
		this.notifier.Remove(this.notification);
	}

	// Token: 0x060026CE RID: 9934 RVA: 0x000D27C0 File Offset: 0x000D09C0
	private void ApplyRandomTrait(Worker worker)
	{
		Traits component = worker.GetComponent<Traits>();
		List<string> list = new List<string>();
		foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.GENESHUFFLERTRAITS)
		{
			if (!component.HasTrait(traitVal.id))
			{
				list.Add(traitVal.id);
			}
		}
		if (list.Count > 0)
		{
			string id = list[UnityEngine.Random.Range(0, list.Count)];
			Trait trait = Db.Get().traits.TryGet(id);
			worker.GetComponent<Traits>().Add(trait);
			InfoDialogScreen infoDialogScreen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
			string text = string.Format(UI.GENESHUFFLERMESSAGE.BODY_SUCCESS, worker.GetProperName(), trait.Name, trait.GetTooltip());
			infoDialogScreen.SetHeader(UI.GENESHUFFLERMESSAGE.HEADER).AddPlainText(text).AddDefaultOK(false);
			this.SetConsumed(true);
			return;
		}
		InfoDialogScreen infoDialogScreen2 = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		string text2 = string.Format(UI.GENESHUFFLERMESSAGE.BODY_FAILURE, worker.GetProperName());
		infoDialogScreen2.SetHeader(UI.GENESHUFFLERMESSAGE.HEADER).AddPlainText(text2).AddDefaultOK(false);
	}

	// Token: 0x060026CF RID: 9935 RVA: 0x000D2950 File Offset: 0x000D0B50
	private void ActivateChore()
	{
		global::Debug.Assert(this.chore == null);
		base.GetComponent<Workable>().SetWorkTime(float.PositiveInfinity);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.GeneShuffle, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim("anim_interacts_neuralvacillator_kanim"), false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x060026D0 RID: 9936 RVA: 0x000D29C0 File Offset: 0x000D0BC0
	private void CancelChore()
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x060026D1 RID: 9937 RVA: 0x000D29E2 File Offset: 0x000D0BE2
	private void CompleteChore()
	{
		this.chore.Cleanup();
		this.chore = null;
	}

	// Token: 0x060026D2 RID: 9938 RVA: 0x000D29F6 File Offset: 0x000D0BF6
	public void RequestRecharge(bool request)
	{
		this.RechargeRequested = request;
		this.RefreshRechargeChore();
	}

	// Token: 0x060026D3 RID: 9939 RVA: 0x000D2A05 File Offset: 0x000D0C05
	private void RefreshRechargeChore()
	{
		this.delivery.Pause(!this.RechargeRequested, "No recharge requested");
	}

	// Token: 0x060026D4 RID: 9940 RVA: 0x000D2A20 File Offset: 0x000D0C20
	public void RefreshSideScreen()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x060026D5 RID: 9941 RVA: 0x000D2A3F File Offset: 0x000D0C3F
	public void SetAssignable(bool set_it)
	{
		this.assignable.SetCanBeAssigned(set_it);
		this.RefreshSideScreen();
	}

	// Token: 0x04001637 RID: 5687
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x04001638 RID: 5688
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001639 RID: 5689
	[MyCmpReq]
	public ManualDeliveryKG delivery;

	// Token: 0x0400163A RID: 5690
	[MyCmpReq]
	public Storage storage;

	// Token: 0x0400163B RID: 5691
	[Serialize]
	public bool IsConsumed;

	// Token: 0x0400163C RID: 5692
	[Serialize]
	public bool RechargeRequested;

	// Token: 0x0400163D RID: 5693
	private Chore chore;

	// Token: 0x0400163E RID: 5694
	private GeneShuffler.GeneShufflerSM.Instance geneShufflerSMI;

	// Token: 0x0400163F RID: 5695
	private Notification notification;

	// Token: 0x04001640 RID: 5696
	private static Tag RechargeTag = new Tag("GeneShufflerRecharge");

	// Token: 0x04001641 RID: 5697
	private static readonly EventSystem.IntraObjectHandler<GeneShuffler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<GeneShuffler>(delegate(GeneShuffler component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04001642 RID: 5698
	private bool storage_recursion_guard;

	// Token: 0x020012B2 RID: 4786
	public class GeneShufflerSM : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler>
	{
		// Token: 0x06007E39 RID: 32313 RVA: 0x002E7A74 File Offset: 0x002E5C74
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("on").Enter(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.SetAssignable(true);
			}).Exit(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.SetAssignable(false);
			}).WorkableStartTransition((GeneShuffler.GeneShufflerSM.Instance smi) => smi.master, this.working.pre).ParamTransition<bool>(this.isCharged, this.consumed, GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.IsFalse);
			this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
			this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, this.working.complete);
			this.working.complete.ToggleStatusItem(Db.Get().BuildingStatusItems.GeneShuffleCompleted, null).Enter(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			}).WorkableStopTransition((GeneShuffler.GeneShufflerSM.Instance smi) => smi.master, this.working.pst);
			this.working.pst.OnAnimQueueComplete(this.consumed);
			this.consumed.PlayAnim("off", KAnim.PlayMode.Once).ParamTransition<bool>(this.isCharged, this.recharging, GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.IsTrue);
			this.recharging.PlayAnim("recharging", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		}

		// Token: 0x04006067 RID: 24679
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State idle;

		// Token: 0x04006068 RID: 24680
		public GeneShuffler.GeneShufflerSM.WorkingStates working;

		// Token: 0x04006069 RID: 24681
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State consumed;

		// Token: 0x0400606A RID: 24682
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State recharging;

		// Token: 0x0400606B RID: 24683
		public StateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.BoolParameter isCharged;

		// Token: 0x020020D5 RID: 8405
		public class WorkingStates : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State
		{
			// Token: 0x040092F5 RID: 37621
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State pre;

			// Token: 0x040092F6 RID: 37622
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State loop;

			// Token: 0x040092F7 RID: 37623
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State complete;

			// Token: 0x040092F8 RID: 37624
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State pst;
		}

		// Token: 0x020020D6 RID: 8406
		public new class Instance : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.GameInstance
		{
			// Token: 0x0600A77A RID: 42874 RVA: 0x0036F6D0 File Offset: 0x0036D8D0
			public Instance(GeneShuffler master) : base(master)
			{
			}
		}
	}
}
