using System;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000DF5 RID: 3573
	public class EclipseEvent : GameplayEvent<EclipseEvent.StatesInstance>
	{
		// Token: 0x06006DD2 RID: 28114 RVA: 0x002B5103 File Offset: 0x002B3303
		public EclipseEvent() : base("EclipseEvent", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.ECLIPSE.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.ECLIPSE.DESCRIPTION;
		}

		// Token: 0x06006DD3 RID: 28115 RVA: 0x002B5132 File Offset: 0x002B3332
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new EclipseEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400524F RID: 21071
		public const string ID = "EclipseEvent";

		// Token: 0x04005250 RID: 21072
		public const float duration = 30f;

		// Token: 0x02001F76 RID: 8054
		public class StatesInstance : GameplayEventStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, EclipseEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600A282 RID: 41602 RVA: 0x00364D6F File Offset: 0x00362F6F
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, EclipseEvent eclipseEvent) : base(master, eventInstance, eclipseEvent)
			{
			}
		}

		// Token: 0x02001F77 RID: 8055
		public class States : GameplayEventStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, EclipseEvent>
		{
			// Token: 0x0600A283 RID: 41603 RVA: 0x00364D7C File Offset: 0x00362F7C
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.GoTo(this.eclipse);
				this.eclipse.ToggleNotification((EclipseEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null)).Enter(delegate(EclipseEvent.StatesInstance smi)
				{
					TimeOfDay.Instance.SetEclipse(true);
				}).Exit(delegate(EclipseEvent.StatesInstance smi)
				{
					TimeOfDay.Instance.SetEclipse(false);
				}).ScheduleGoTo(30f, this.finished);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600A284 RID: 41604 RVA: 0x00364E28 File Offset: 0x00363028
			public override EventInfoData GenerateEventPopupData(EclipseEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.SUN,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x04008E32 RID: 36402
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x04008E33 RID: 36403
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State eclipse;

			// Token: 0x04008E34 RID: 36404
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State finished;
		}
	}
}
