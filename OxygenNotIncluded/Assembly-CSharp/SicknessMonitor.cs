using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000892 RID: 2194
public class SicknessMonitor : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance>
{
	// Token: 0x06003FC6 RID: 16326 RVA: 0x00164934 File Offset: 0x00162B34
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		default_state = this.healthy;
		this.healthy.EventTransition(GameHashes.SicknessAdded, this.sick, (SicknessMonitor.Instance smi) => smi.IsSick());
		this.sick.DefaultState(this.sick.minor).EventTransition(GameHashes.SicknessCured, this.post_nocheer, (SicknessMonitor.Instance smi) => !smi.IsSick()).ToggleThought(Db.Get().Thoughts.GotInfected, null);
		this.sick.minor.EventTransition(GameHashes.SicknessAdded, this.sick.major, (SicknessMonitor.Instance smi) => smi.HasMajorDisease());
		this.sick.major.EventTransition(GameHashes.SicknessCured, this.sick.minor, (SicknessMonitor.Instance smi) => !smi.HasMajorDisease()).ToggleUrge(Db.Get().Urges.RestDueToDisease).Update("AutoAssignClinic", delegate(SicknessMonitor.Instance smi, float dt)
		{
			smi.AutoAssignClinic();
		}, UpdateRate.SIM_4000ms, false).Exit(delegate(SicknessMonitor.Instance smi)
		{
			smi.UnassignClinic();
		});
		this.post_nocheer.Enter(delegate(SicknessMonitor.Instance smi)
		{
			new SicknessCuredFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f)).StartSM();
			if (smi.IsSleepingOrSleepSchedule())
			{
				smi.GoTo(this.healthy);
				return;
			}
			smi.GoTo(this.post);
		});
		this.post.ToggleChore((SicknessMonitor.Instance smi) => new EmoteChore(smi.master, Db.Get().ChoreTypes.EmoteHighPriority, SicknessMonitor.SickPostKAnim, SicknessMonitor.SickPostAnims, KAnim.PlayMode.Once, false), this.healthy);
	}

	// Token: 0x04002974 RID: 10612
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x04002975 RID: 10613
	public SicknessMonitor.SickStates sick;

	// Token: 0x04002976 RID: 10614
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State post;

	// Token: 0x04002977 RID: 10615
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State post_nocheer;

	// Token: 0x04002978 RID: 10616
	private static readonly HashedString SickPostKAnim = "anim_cheer_kanim";

	// Token: 0x04002979 RID: 10617
	private static readonly HashedString[] SickPostAnims = new HashedString[]
	{
		"cheer_pre",
		"cheer_loop",
		"cheer_pst"
	};

	// Token: 0x020016AC RID: 5804
	public class SickStates : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006C72 RID: 27762
		public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State minor;

		// Token: 0x04006C73 RID: 27763
		public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State major;
	}

	// Token: 0x020016AD RID: 5805
	public new class Instance : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008BD4 RID: 35796 RVA: 0x00317255 File Offset: 0x00315455
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sicknesses = master.GetComponent<MinionModifiers>().sicknesses;
		}

		// Token: 0x06008BD5 RID: 35797 RVA: 0x0031726F File Offset: 0x0031546F
		private string OnGetToolTip(List<Notification> notifications, object data)
		{
			return DUPLICANTS.STATUSITEMS.HASDISEASE.TOOLTIP;
		}

		// Token: 0x06008BD6 RID: 35798 RVA: 0x0031727B File Offset: 0x0031547B
		public bool IsSick()
		{
			return this.sicknesses.Count > 0;
		}

		// Token: 0x06008BD7 RID: 35799 RVA: 0x0031728C File Offset: 0x0031548C
		public bool HasMajorDisease()
		{
			using (IEnumerator<SicknessInstance> enumerator = this.sicknesses.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.modifier.severity >= Sickness.Severity.Major)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008BD8 RID: 35800 RVA: 0x003172E8 File Offset: 0x003154E8
		public void AutoAssignClinic()
		{
			Ownables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			AssignableSlotInstance slot = soleOwner.GetSlot(clinic);
			if (slot == null)
			{
				return;
			}
			if (slot.assignable != null)
			{
				return;
			}
			soleOwner.AutoAssignSlot(clinic);
		}

		// Token: 0x06008BD9 RID: 35801 RVA: 0x0031734C File Offset: 0x0031554C
		public void UnassignClinic()
		{
			Assignables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			AssignableSlotInstance slot = soleOwner.GetSlot(clinic);
			if (slot != null)
			{
				slot.Unassign(true);
			}
		}

		// Token: 0x06008BDA RID: 35802 RVA: 0x0031739C File Offset: 0x0031559C
		public bool IsSleepingOrSleepSchedule()
		{
			Schedulable component = base.GetComponent<Schedulable>();
			if (component != null && component.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
			{
				return true;
			}
			KPrefabID component2 = base.GetComponent<KPrefabID>();
			return component2 != null && component2.HasTag(GameTags.Asleep);
		}

		// Token: 0x04006C74 RID: 27764
		private Sicknesses sicknesses;
	}
}
