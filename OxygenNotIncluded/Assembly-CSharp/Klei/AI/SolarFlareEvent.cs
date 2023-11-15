using System;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000DFF RID: 3583
	public class SolarFlareEvent : GameplayEvent<SolarFlareEvent.StatesInstance>
	{
		// Token: 0x06006DF3 RID: 28147 RVA: 0x002B5883 File Offset: 0x002B3A83
		public SolarFlareEvent() : base("SolarFlareEvent", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.DESCRIPTION;
		}

		// Token: 0x06006DF4 RID: 28148 RVA: 0x002B58B2 File Offset: 0x002B3AB2
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SolarFlareEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400527C RID: 21116
		public const string ID = "SolarFlareEvent";

		// Token: 0x0400527D RID: 21117
		public const float DURATION = 7f;

		// Token: 0x02001F87 RID: 8071
		public class StatesInstance : GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600A2D7 RID: 41687 RVA: 0x00366FAC File Offset: 0x003651AC
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SolarFlareEvent solarFlareEvent) : base(master, eventInstance, solarFlareEvent)
			{
			}
		}

		// Token: 0x02001F88 RID: 8072
		public class States : GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>
		{
			// Token: 0x0600A2D8 RID: 41688 RVA: 0x00366FB7 File Offset: 0x003651B7
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.idle;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.idle.DoNothing();
				this.start.ScheduleGoTo(7f, this.finished);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600A2D9 RID: 41689 RVA: 0x00366FF8 File Offset: 0x003651F8
			public override EventInfoData GenerateEventPopupData(SolarFlareEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.SUN,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x04008E69 RID: 36457
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State idle;

			// Token: 0x04008E6A RID: 36458
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State start;

			// Token: 0x04008E6B RID: 36459
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State finished;
		}
	}
}
