using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020006A0 RID: 1696
public class Telepad : StateMachineComponent<Telepad.StatesInstance>
{
	// Token: 0x06002DD1 RID: 11729 RVA: 0x000F2730 File Offset: 0x000F0930
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<Deconstructable>().allowDeconstruction = false;
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		if (num == 0)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Headquarters spawned at: (",
				num.ToString(),
				",",
				num2.ToString(),
				")"
			}));
		}
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x000F27A4 File Offset: 0x000F09A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Telepads.Add(this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
		base.smi.StartSM();
	}

	// Token: 0x06002DD3 RID: 11731 RVA: 0x000F2826 File Offset: 0x000F0A26
	protected override void OnCleanUp()
	{
		Components.Telepads.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002DD4 RID: 11732 RVA: 0x000F283C File Offset: 0x000F0A3C
	public void Update()
	{
		if (base.smi.IsColonyLost())
		{
			return;
		}
		if (Immigration.Instance.ImmigrantsAvailable && base.GetComponent<Operational>().IsOperational)
		{
			base.smi.sm.openPortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NewDuplicantsAvailable, this);
		}
		else
		{
			base.smi.sm.closePortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Wattson, this);
		}
		if (this.GetTimeRemaining() < -120f)
		{
			Messenger.Instance.QueueMessage(new DuplicantsLeftMessage());
			Immigration.Instance.EndImmigration();
		}
	}

	// Token: 0x06002DD5 RID: 11733 RVA: 0x000F2925 File Offset: 0x000F0B25
	public void RejectAll()
	{
		Immigration.Instance.EndImmigration();
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	// Token: 0x06002DD6 RID: 11734 RVA: 0x000F2950 File Offset: 0x000F0B50
	public void OnAcceptDelivery(ITelepadDeliverable delivery)
	{
		int cell = Grid.PosToCell(this);
		Immigration.Instance.EndImmigration();
		GameObject gameObject = delivery.Deliver(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component != null)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, GameClock.Instance.GetTimeSinceStartOfReport(), string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, DUPLICANTS.CHORES.NOT_EXISTING_TASK), gameObject.GetProperName());
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.gameObject.GetComponent<KSelectable>().GetMyWorldId(), false))
			{
				minionIdentity.GetComponent<Effects>().Add("NewCrewArrival", true);
			}
			MinionResume component2 = component.GetComponent<MinionResume>();
			int num = 0;
			while ((float)num < this.startingSkillPoints)
			{
				component2.ForceAddSkillPoint();
				num++;
			}
		}
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	// Token: 0x06002DD7 RID: 11735 RVA: 0x000F2A64 File Offset: 0x000F0C64
	public float GetTimeRemaining()
	{
		return Immigration.Instance.GetTimeRemaining();
	}

	// Token: 0x04001B03 RID: 6915
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001B04 RID: 6916
	private MeterController meter;

	// Token: 0x04001B05 RID: 6917
	private const float MAX_IMMIGRATION_TIME = 120f;

	// Token: 0x04001B06 RID: 6918
	private const int NUM_METER_NOTCHES = 8;

	// Token: 0x04001B07 RID: 6919
	private List<MinionStartingStats> minionStats;

	// Token: 0x04001B08 RID: 6920
	public float startingSkillPoints;

	// Token: 0x04001B09 RID: 6921
	public static readonly HashedString[] PortalBirthAnim = new HashedString[]
	{
		"portalbirth"
	};

	// Token: 0x020013CC RID: 5068
	public class StatesInstance : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.GameInstance
	{
		// Token: 0x06008241 RID: 33345 RVA: 0x002F8FB9 File Offset: 0x002F71B9
		public StatesInstance(Telepad master) : base(master)
		{
		}

		// Token: 0x06008242 RID: 33346 RVA: 0x002F8FC2 File Offset: 0x002F71C2
		public bool IsColonyLost()
		{
			return GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver();
		}

		// Token: 0x06008243 RID: 33347 RVA: 0x002F8FE0 File Offset: 0x002F71E0
		public void UpdateMeter()
		{
			float timeRemaining = Immigration.Instance.GetTimeRemaining();
			float totalWaitTime = Immigration.Instance.GetTotalWaitTime();
			float positionPercent = Mathf.Clamp01(1f - timeRemaining / totalWaitTime);
			base.master.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x020013CD RID: 5069
	public class States : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad>
	{
		// Token: 0x06008244 RID: 33348 RVA: 0x002F9024 File Offset: 0x002F7224
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.OnSignal(this.idlePortal, this.resetToIdle);
			this.resetToIdle.GoTo(this.idle);
			this.idle.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.UpdateMeter();
			}).Update("TelepadMeter", delegate(Telepad.StatesInstance smi, float dt)
			{
				smi.UpdateMeter();
			}, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("idle").OnSignal(this.openPortal, this.opening);
			this.unoperational.PlayAnim("idle").Enter("StopImmigration", delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).EventTransition(GameHashes.OperationalChanged, this.idle, (Telepad.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.opening.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.open);
			this.open.OnSignal(this.closePortal, this.close).Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_loop", KAnim.PlayMode.Loop).Transition(this.close, (Telepad.StatesInstance smi) => smi.IsColonyLost(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OperationalChanged, this.close, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.close.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).PlayAnims((Telepad.StatesInstance smi) => Telepad.States.workingAnims, KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		}

		// Token: 0x04006361 RID: 25441
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal openPortal;

		// Token: 0x04006362 RID: 25442
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal closePortal;

		// Token: 0x04006363 RID: 25443
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal idlePortal;

		// Token: 0x04006364 RID: 25444
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State idle;

		// Token: 0x04006365 RID: 25445
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State resetToIdle;

		// Token: 0x04006366 RID: 25446
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State opening;

		// Token: 0x04006367 RID: 25447
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State open;

		// Token: 0x04006368 RID: 25448
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State close;

		// Token: 0x04006369 RID: 25449
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State unoperational;

		// Token: 0x0400636A RID: 25450
		private static readonly HashedString[] workingAnims = new HashedString[]
		{
			"working_loop",
			"working_pst"
		};
	}
}
