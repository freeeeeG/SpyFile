using System;

// Token: 0x020006A1 RID: 1697
public class TeleportalPad : StateMachineComponent<TeleportalPad.StatesInstance>
{
	// Token: 0x06002DDA RID: 11738 RVA: 0x000F2A96 File Offset: 0x000F0C96
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001B0A RID: 6922
	[MyCmpReq]
	private Operational operational;

	// Token: 0x020013CE RID: 5070
	public class StatesInstance : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.GameInstance
	{
		// Token: 0x06008247 RID: 33351 RVA: 0x002F92E6 File Offset: 0x002F74E6
		public StatesInstance(TeleportalPad master) : base(master)
		{
		}
	}

	// Token: 0x020013CF RID: 5071
	public class States : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad>
	{
		// Token: 0x06008248 RID: 33352 RVA: 0x002F92F0 File Offset: 0x002F74F0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inactive;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventTransition(GameHashes.OperationalChanged, this.inactive, (TeleportalPad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.inactive.PlayAnim("idle").EventTransition(GameHashes.OperationalChanged, this.no_target, (TeleportalPad.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.no_target.Enter(delegate(TeleportalPad.StatesInstance smi)
			{
				if (smi.master.GetComponent<Teleporter>().HasTeleporterTarget())
				{
					smi.GoTo(this.portal_on.turn_on);
				}
			}).PlayAnim("idle").EventTransition(GameHashes.TeleporterIDsChanged, this.portal_on.turn_on, (TeleportalPad.StatesInstance smi) => smi.master.GetComponent<Teleporter>().HasTeleporterTarget());
			this.portal_on.EventTransition(GameHashes.TeleporterIDsChanged, this.portal_on.turn_off, (TeleportalPad.StatesInstance smi) => !smi.master.GetComponent<Teleporter>().HasTeleporterTarget());
			this.portal_on.turn_on.PlayAnim("working_pre").OnAnimQueueComplete(this.portal_on.loop);
			this.portal_on.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(TeleportalPad.StatesInstance smi, float dt)
			{
				Teleporter component = smi.master.GetComponent<Teleporter>();
				Teleporter teleporter = component.FindTeleportTarget();
				component.SetTeleportTarget(teleporter);
				if (teleporter != null)
				{
					component.TeleportObjects();
				}
			}, UpdateRate.SIM_200ms, false);
			this.portal_on.turn_off.PlayAnim("working_pst").OnAnimQueueComplete(this.no_target);
		}

		// Token: 0x0400636B RID: 25451
		public StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Signal targetTeleporter;

		// Token: 0x0400636C RID: 25452
		public StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Signal doTeleport;

		// Token: 0x0400636D RID: 25453
		public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State inactive;

		// Token: 0x0400636E RID: 25454
		public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State no_target;

		// Token: 0x0400636F RID: 25455
		public TeleportalPad.States.PortalOnStates portal_on;

		// Token: 0x02002136 RID: 8502
		public class PortalOnStates : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State
		{
			// Token: 0x040094EB RID: 38123
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State turn_on;

			// Token: 0x040094EC RID: 38124
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State loop;

			// Token: 0x040094ED RID: 38125
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State turn_off;
		}
	}
}
