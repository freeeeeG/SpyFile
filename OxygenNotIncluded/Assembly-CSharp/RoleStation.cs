using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000942 RID: 2370
[AddComponentMenu("KMonoBehaviour/Workable/RoleStation")]
public class RoleStation : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x060044CB RID: 17611 RVA: 0x001837DC File Offset: 0x001819DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = true;
		this.UpdateStatusItemDelegate = new Action<object>(this.UpdateSkillPointAvailableStatusItem);
	}

	// Token: 0x060044CC RID: 17612 RVA: 0x00183800 File Offset: 0x00181A00
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RoleStations.Add(this);
		this.smi = new RoleStation.RoleStationSM.Instance(this);
		this.smi.StartSM();
		base.SetWorkTime(7.53f);
		this.resetProgressOnStop = true;
		this.subscriptions.Add(Game.Instance.Subscribe(-1523247426, this.UpdateStatusItemDelegate));
		this.subscriptions.Add(Game.Instance.Subscribe(1505456302, this.UpdateStatusItemDelegate));
		this.UpdateSkillPointAvailableStatusItem(null);
	}

	// Token: 0x060044CD RID: 17613 RVA: 0x00183890 File Offset: 0x00181A90
	protected override void OnStopWork(Worker worker)
	{
		Telepad.StatesInstance statesInstance = this.GetSMI<Telepad.StatesInstance>();
		statesInstance.sm.idlePortal.Trigger(statesInstance);
	}

	// Token: 0x060044CE RID: 17614 RVA: 0x001838B8 File Offset: 0x00181AB8
	private void UpdateSkillPointAvailableStatusItem(object data = null)
	{
		foreach (object obj in Components.MinionResumes)
		{
			MinionResume minionResume = (MinionResume)obj;
			if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
			{
				if (this.skillPointAvailableStatusItem == Guid.Empty)
				{
					this.skillPointAvailableStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, null);
				}
				return;
			}
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, false);
		this.skillPointAvailableStatusItem = Guid.Empty;
	}

	// Token: 0x060044CF RID: 17615 RVA: 0x00183984 File Offset: 0x00181B84
	private Chore CreateWorkChore()
	{
		return new WorkChore<RoleStation>(Db.Get().ChoreTypes.LearnSkill, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
	}

	// Token: 0x060044D0 RID: 17616 RVA: 0x001839C5 File Offset: 0x00181BC5
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		worker.GetComponent<MinionResume>().SkillLearned();
	}

	// Token: 0x060044D1 RID: 17617 RVA: 0x001839D9 File Offset: 0x00181BD9
	private void OnSelectRolesClick()
	{
		DetailsScreen.Instance.Show(false);
		ManagementMenu.Instance.ToggleSkills();
	}

	// Token: 0x060044D2 RID: 17618 RVA: 0x001839F0 File Offset: 0x00181BF0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int id in this.subscriptions)
		{
			Game.Instance.Unsubscribe(id);
		}
		Components.RoleStations.Remove(this);
	}

	// Token: 0x060044D3 RID: 17619 RVA: 0x00183A58 File Offset: 0x00181C58
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		return base.GetDescriptors(go);
	}

	// Token: 0x04002D92 RID: 11666
	private Chore chore;

	// Token: 0x04002D93 RID: 11667
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04002D94 RID: 11668
	[MyCmpAdd]
	private Operational operational;

	// Token: 0x04002D95 RID: 11669
	private RoleStation.RoleStationSM.Instance smi;

	// Token: 0x04002D96 RID: 11670
	private Guid skillPointAvailableStatusItem;

	// Token: 0x04002D97 RID: 11671
	private Action<object> UpdateStatusItemDelegate;

	// Token: 0x04002D98 RID: 11672
	private List<int> subscriptions = new List<int>();

	// Token: 0x02001791 RID: 6033
	public class RoleStationSM : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation>
	{
		// Token: 0x06008EA8 RID: 36520 RVA: 0x0031FDF8 File Offset: 0x0031DFF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (RoleStation.RoleStationSM.Instance smi) => smi.GetComponent<Operational>().IsOperational);
			this.operational.ToggleChore((RoleStation.RoleStationSM.Instance smi) => smi.master.CreateWorkChore(), this.unoperational);
		}

		// Token: 0x04006F2A RID: 28458
		public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State unoperational;

		// Token: 0x04006F2B RID: 28459
		public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State operational;

		// Token: 0x020021D6 RID: 8662
		public new class Instance : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.GameInstance
		{
			// Token: 0x0600ABDE RID: 43998 RVA: 0x003762FF File Offset: 0x003744FF
			public Instance(RoleStation master) : base(master)
			{
			}
		}
	}
}
