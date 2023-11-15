using System;

// Token: 0x0200089F RID: 2207
public class ToiletMonitor : GameStateMachine<ToiletMonitor, ToiletMonitor.Instance>
{
	// Token: 0x06004002 RID: 16386 RVA: 0x00166564 File Offset: 0x00164764
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventHandler(GameHashes.ToiletSensorChanged, delegate(ToiletMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		}).Exit("ClearStatusItem", delegate(ToiletMonitor.Instance smi)
		{
			smi.ClearStatusItem();
		});
	}

	// Token: 0x040029B0 RID: 10672
	public GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040029B1 RID: 10673
	public GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.State unsatisfied;

	// Token: 0x020016D0 RID: 5840
	public new class Instance : GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C88 RID: 35976 RVA: 0x00319114 File Offset: 0x00317314
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.toiletSensor = base.GetComponent<Sensors>().GetSensor<ToiletSensor>();
		}

		// Token: 0x06008C89 RID: 35977 RVA: 0x00319130 File Offset: 0x00317330
		public void RefreshStatusItem()
		{
			StatusItem status_item = null;
			if (!this.toiletSensor.AreThereAnyToilets())
			{
				status_item = Db.Get().DuplicantStatusItems.NoToilets;
			}
			else if (!this.toiletSensor.AreThereAnyUsableToilets())
			{
				status_item = Db.Get().DuplicantStatusItems.NoUsableToilets;
			}
			else if (this.toiletSensor.GetNearestUsableToilet() == null)
			{
				status_item = Db.Get().DuplicantStatusItems.ToiletUnreachable;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Toilet, status_item, null);
		}

		// Token: 0x06008C8A RID: 35978 RVA: 0x003191B7 File Offset: 0x003173B7
		public void ClearStatusItem()
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Toilet, null, null);
		}

		// Token: 0x04006D00 RID: 27904
		private ToiletSensor toiletSensor;
	}
}
