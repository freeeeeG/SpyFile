using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public class BansheeChore : Chore<BansheeChore.StatesInstance>
{
	// Token: 0x0600138B RID: 5003 RVA: 0x00067050 File Offset: 0x00065250
	public BansheeChore(ChoreType chore_type, IStateMachineTarget target, Notification notification, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.BansheeWail, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BansheeChore.StatesInstance(this, target.gameObject, notification);
	}

	// Token: 0x04000A73 RID: 2675
	private const string audienceEffectName = "WailedAt";

	// Token: 0x02000FE2 RID: 4066
	public class StatesInstance : GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.GameInstance
	{
		// Token: 0x060073CE RID: 29646 RVA: 0x002C3694 File Offset: 0x002C1894
		public StatesInstance(BansheeChore master, GameObject wailer, Notification notification) : base(master)
		{
			base.sm.wailer.Set(wailer, base.smi, false);
			this.notification = notification;
		}

		// Token: 0x060073CF RID: 29647 RVA: 0x002C36C0 File Offset: 0x002C18C0
		public void FindAudience()
		{
			Navigator component = base.GetComponent<Navigator>();
			int worldId = (int)Grid.WorldIdx[Grid.PosToCell(base.gameObject)];
			int num = int.MaxValue;
			int num2 = Grid.InvalidCell;
			List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(worldId, false);
			for (int i = 0; i < worldItems.Count; i++)
			{
				if (!worldItems[i].IsNullOrDestroyed() && !(worldItems[i].gameObject == base.gameObject))
				{
					int num3 = Grid.PosToCell(worldItems[i]);
					if (component.CanReach(num3) && !worldItems[i].GetComponent<Effects>().HasEffect("WailedAt"))
					{
						int navigationCost = component.GetNavigationCost(num3);
						if (navigationCost < num)
						{
							num = navigationCost;
							num2 = num3;
						}
					}
				}
			}
			if (num2 == Grid.InvalidCell)
			{
				num2 = this.FindIdleCell();
			}
			base.sm.targetWailLocation.Set(num2, base.smi, false);
			this.GoTo(base.sm.moveToAudience);
		}

		// Token: 0x060073D0 RID: 29648 RVA: 0x002C37C8 File Offset: 0x002C19C8
		public int FindIdleCell()
		{
			Navigator component = base.smi.master.GetComponent<Navigator>();
			MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)component.GetCurrentAbilities();
			minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
			IdleCellQuery idleCellQuery = PathFinderQueries.idleCellQuery.Reset(base.GetComponent<MinionBrain>(), UnityEngine.Random.Range(30, 90));
			component.RunQuery(idleCellQuery);
			minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
			return idleCellQuery.GetResultCell();
		}

		// Token: 0x060073D1 RID: 29649 RVA: 0x002C3828 File Offset: 0x002C1A28
		public void BotherAudience(float dt)
		{
			if (dt <= 0f)
			{
				return;
			}
			int num = Grid.PosToCell(base.smi.master.gameObject);
			int worldId = (int)Grid.WorldIdx[num];
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(worldId, false))
			{
				if (!minionIdentity.IsNullOrDestroyed() && !(minionIdentity.gameObject == base.smi.master.gameObject))
				{
					int endCell = Grid.PosToCell(minionIdentity);
					if (Grid.GetCellDistance(num, Grid.PosToCell(minionIdentity)) <= STRESS.BANSHEE_WAIL_RADIUS)
					{
						HashSet<int> hashSet = new HashSet<int>();
						Grid.CollectCellsInLine(num, endCell, hashSet);
						bool flag = false;
						foreach (int i in hashSet)
						{
							if (Grid.Solid[i])
							{
								flag = true;
								break;
							}
						}
						if (!flag && !minionIdentity.GetComponent<Effects>().HasEffect("WailedAt"))
						{
							minionIdentity.GetComponent<Effects>().Add("WailedAt", true);
							minionIdentity.GetSMI<ThreatMonitor.Instance>().ClearMainThreat();
							new FleeChore(minionIdentity.GetComponent<IStateMachineTarget>(), base.smi.master.gameObject);
						}
					}
				}
			}
		}

		// Token: 0x04005730 RID: 22320
		public Notification notification;
	}

	// Token: 0x02000FE3 RID: 4067
	public class States : GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore>
	{
		// Token: 0x060073D2 RID: 29650 RVA: 0x002C39B8 File Offset: 0x002C1BB8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.findAudience;
			base.Target(this.wailer);
			this.wailPreEffect = new Effect("BansheeWailing", DUPLICANTS.MODIFIERS.BANSHEE_WAILING.NAME, DUPLICANTS.MODIFIERS.BANSHEE_WAILING.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.wailPreEffect.Add(new AttributeModifier("AirConsumptionRate", 7.5f, null, false, false, true));
			Db.Get().effects.Add(this.wailPreEffect);
			this.wailRecoverEffect = new Effect("BansheeWailingRecovery", DUPLICANTS.MODIFIERS.BANSHEE_WAILING_RECOVERY.NAME, DUPLICANTS.MODIFIERS.BANSHEE_WAILING_RECOVERY.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.wailRecoverEffect.Add(new AttributeModifier("AirConsumptionRate", 1f, null, false, false, true));
			Db.Get().effects.Add(this.wailRecoverEffect);
			this.findAudience.Enter("FindAudience", delegate(BansheeChore.StatesInstance smi)
			{
				smi.FindAudience();
			}).ToggleAnims("anim_loco_banshee_kanim", 0f, "");
			this.moveToAudience.MoveTo((BansheeChore.StatesInstance smi) => smi.sm.targetWailLocation.Get(smi), this.wail, this.delay, false).ToggleAnims("anim_loco_banshee_kanim", 0f, "");
			this.wail.defaultState = this.wail.pre.DoNotification((BansheeChore.StatesInstance smi) => smi.notification);
			this.wail.pre.ToggleAnims("anim_banshee_kanim", 0f, "").PlayAnim("working_pre").ToggleEffect((BansheeChore.StatesInstance smi) => this.wailPreEffect).OnAnimQueueComplete(this.wail.loop);
			this.wail.loop.ToggleAnims("anim_banshee_kanim", 0f, "").Enter(delegate(BansheeChore.StatesInstance smi)
			{
				smi.Play("working_loop", KAnim.PlayMode.Loop);
				AcousticDisturbance.Emit(smi.master.gameObject, STRESS.BANSHEE_WAIL_RADIUS);
			}).ScheduleGoTo(5f, this.wail.pst).Update(delegate(BansheeChore.StatesInstance smi, float dt)
			{
				smi.BotherAudience(dt);
			}, UpdateRate.SIM_200ms, false);
			this.wail.pst.ToggleAnims("anim_banshee_kanim", 0f, "").QueueAnim("working_pst", false, null).EventHandlerTransition(GameHashes.AnimQueueComplete, this.recover, (BansheeChore.StatesInstance smi, object data) => true).ScheduleGoTo(3f, this.recover);
			this.recover.ToggleEffect((BansheeChore.StatesInstance smi) => this.wailRecoverEffect).ToggleAnims("anim_emotes_default_kanim", 0f, "").QueueAnim("breathe_pre", false, null).QueueAnim("breathe_loop", false, null).QueueAnim("breathe_loop", false, null).QueueAnim("breathe_loop", false, null).QueueAnim("breathe_pst", false, null).OnAnimQueueComplete(this.complete);
			this.delay.ScheduleGoTo(1f, this.wander);
			this.wander.MoveTo((BansheeChore.StatesInstance smi) => smi.FindIdleCell(), this.findAudience, this.findAudience, false).ToggleAnims("anim_loco_banshee_kanim", 0f, "");
			this.complete.Enter(delegate(BansheeChore.StatesInstance smi)
			{
				smi.StopSM("complete");
			});
		}

		// Token: 0x04005731 RID: 22321
		public StateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.TargetParameter wailer;

		// Token: 0x04005732 RID: 22322
		public StateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.IntParameter targetWailLocation;

		// Token: 0x04005733 RID: 22323
		public GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.State findAudience;

		// Token: 0x04005734 RID: 22324
		public GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.State moveToAudience;

		// Token: 0x04005735 RID: 22325
		public BansheeChore.States.Wail wail;

		// Token: 0x04005736 RID: 22326
		public GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.State recover;

		// Token: 0x04005737 RID: 22327
		public GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.State delay;

		// Token: 0x04005738 RID: 22328
		public GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.State wander;

		// Token: 0x04005739 RID: 22329
		public GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.State complete;

		// Token: 0x0400573A RID: 22330
		private Effect wailPreEffect;

		// Token: 0x0400573B RID: 22331
		private Effect wailRecoverEffect;

		// Token: 0x02001FC4 RID: 8132
		public class Wail : GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.State
		{
			// Token: 0x04008F04 RID: 36612
			public GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.State pre;

			// Token: 0x04008F05 RID: 36613
			public GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.State loop;

			// Token: 0x04008F06 RID: 36614
			public GameStateMachine<BansheeChore.States, BansheeChore.StatesInstance, BansheeChore, object>.State pst;
		}
	}
}
