using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020003BF RID: 959
public class PeeChore : Chore<PeeChore.StatesInstance>
{
	// Token: 0x060013EC RID: 5100 RVA: 0x0006A2C0 File Offset: 0x000684C0
	public PeeChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Pee, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new PeeChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001015 RID: 4117
	public class StatesInstance : GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.GameInstance
	{
		// Token: 0x0600749E RID: 29854 RVA: 0x002C9124 File Offset: 0x002C7324
		public StatesInstance(PeeChore master, GameObject worker) : base(master)
		{
			this.bladder = Db.Get().Amounts.Bladder.Lookup(worker);
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(worker);
			base.sm.worker.Set(worker, base.smi, false);
		}

		// Token: 0x0600749F RID: 29855 RVA: 0x002C91C9 File Offset: 0x002C73C9
		public bool IsDonePeeing()
		{
			return this.bladder.value <= 0f;
		}

		// Token: 0x060074A0 RID: 29856 RVA: 0x002C91E0 File Offset: 0x002C73E0
		public void SpawnDirtyWater(float dt)
		{
			int gameCell = Grid.PosToCell(base.sm.worker.Get<KMonoBehaviour>(base.smi));
			byte index = Db.Get().Diseases.GetIndex("FoodPoisoning");
			float num = dt * -this.bladder.GetDelta() / this.bladder.GetMax();
			if (num > 0f)
			{
				float mass = 2f * num;
				Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
				if (equippable != null)
				{
					equippable.GetComponent<Storage>().AddLiquid(SimHashes.DirtyWater, mass, this.bodyTemperature.value, index, Mathf.CeilToInt(100000f * num), false, true);
					return;
				}
				SimMessages.AddRemoveSubstance(gameCell, SimHashes.DirtyWater, CellEventLogger.Instance.Vomit, mass, this.bodyTemperature.value, index, Mathf.CeilToInt(100000f * num), true, -1);
			}
		}

		// Token: 0x0400581C RID: 22556
		public Notification stressfullyEmptyingBladder = new Notification(DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);

		// Token: 0x0400581D RID: 22557
		public AmountInstance bladder;

		// Token: 0x0400581E RID: 22558
		private AmountInstance bodyTemperature;
	}

	// Token: 0x02001016 RID: 4118
	public class States : GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore>
	{
		// Token: 0x060074A1 RID: 29857 RVA: 0x002C92C4 File Offset: 0x002C74C4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.running;
			base.Target(this.worker);
			this.running.ToggleAnims("anim_expel_kanim", 0f, "").ToggleEffect("StressfulyEmptyingBladder").DoNotification((PeeChore.StatesInstance smi) => smi.stressfullyEmptyingBladder).DoReport(ReportManager.ReportType.ToiletIncident, (PeeChore.StatesInstance smi) => 1f, (PeeChore.StatesInstance smi) => this.masterTarget.Get(smi).GetProperName()).DoTutorial(Tutorial.TutorialMessages.TM_Mopping).Transition(null, (PeeChore.StatesInstance smi) => smi.IsDonePeeing(), UpdateRate.SIM_200ms).Update("SpawnDirtyWater", delegate(PeeChore.StatesInstance smi, float dt)
			{
				smi.SpawnDirtyWater(dt);
			}, UpdateRate.SIM_200ms, false).PlayAnim("working_loop", KAnim.PlayMode.Loop).ToggleTag(GameTags.MakingMess).Enter(delegate(PeeChore.StatesInstance smi)
			{
				if (Sim.IsRadiationEnabled() && smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
				{
					smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
				}
			}).Exit(delegate(PeeChore.StatesInstance smi)
			{
				if (Sim.IsRadiationEnabled())
				{
					smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
					AmountInstance amountInstance = smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id);
					RadiationMonitor.Instance smi2 = smi.master.gameObject.GetSMI<RadiationMonitor.Instance>();
					if (smi2 != null)
					{
						float num = Math.Min(amountInstance.value, 100f * smi2.difficultySettingMod);
						smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).ApplyDelta(-num);
						if (num >= 1f)
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Mathf.FloorToInt(num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, smi.master.transform, 1.5f, false);
						}
					}
				}
			});
		}

		// Token: 0x0400581F RID: 22559
		public StateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.TargetParameter worker;

		// Token: 0x04005820 RID: 22560
		public GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.State running;
	}
}
