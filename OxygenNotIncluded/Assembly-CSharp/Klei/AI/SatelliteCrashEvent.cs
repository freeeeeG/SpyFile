using System;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DFD RID: 3581
	public class SatelliteCrashEvent : GameplayEvent<SatelliteCrashEvent.StatesInstance>
	{
		// Token: 0x06006DEF RID: 28143 RVA: 0x002B580A File Offset: 0x002B3A0A
		public SatelliteCrashEvent() : base("SatelliteCrash", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.SATELLITE_CRASH.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.SATELLITE_CRASH.DESCRIPTION;
		}

		// Token: 0x06006DF0 RID: 28144 RVA: 0x002B5839 File Offset: 0x002B3A39
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SatelliteCrashEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x02001F83 RID: 8067
		public class StatesInstance : GameplayEventStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, SatelliteCrashEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600A2CC RID: 41676 RVA: 0x00366C71 File Offset: 0x00364E71
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SatelliteCrashEvent crashEvent) : base(master, eventInstance, crashEvent)
			{
			}

			// Token: 0x0600A2CD RID: 41677 RVA: 0x00366C7C File Offset: 0x00364E7C
			public Notification Plan()
			{
				Vector3 position = new Vector3((float)(Grid.WidthInCells / 2 + UnityEngine.Random.Range(-Grid.WidthInCells / 3, Grid.WidthInCells / 3)), (float)(Grid.HeightInCells - 1), Grid.GetLayerZ(Grid.SceneLayer.FXFront));
				GameObject spawn = Util.KInstantiate(Assets.GetPrefab(SatelliteCometConfig.ID), position);
				spawn.SetActive(true);
				Notification notification = EventInfoScreen.CreateNotification(base.smi.sm.GenerateEventPopupData(base.smi), null);
				notification.clickFocus = spawn.transform;
				Comet component = spawn.GetComponent<Comet>();
				component.OnImpact = (System.Action)Delegate.Combine(component.OnImpact, new System.Action(delegate()
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = spawn.transform.position;
					notification.clickFocus = gameObject.transform;
					GridVisibility.Reveal(Grid.PosToXY(gameObject.transform.position).x, Grid.PosToXY(gameObject.transform.position).y, 6, 4f);
				}));
				return notification;
			}
		}

		// Token: 0x02001F84 RID: 8068
		public class States : GameplayEventStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, SatelliteCrashEvent>
		{
			// Token: 0x0600A2CE RID: 41678 RVA: 0x00366D54 File Offset: 0x00364F54
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.notify;
				this.notify.ToggleNotification((SatelliteCrashEvent.StatesInstance smi) => smi.Plan());
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600A2CF RID: 41679 RVA: 0x00366DA0 File Offset: 0x00364FA0
			public override EventInfoData GenerateEventPopupData(SatelliteCrashEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.location = GAMEPLAY_EVENTS.LOCATIONS.SURFACE;
				eventInfoData.whenDescription = GAMEPLAY_EVENTS.TIMES.NOW;
				eventInfoData.AddDefaultOption(delegate
				{
					smi.GoTo(smi.sm.ending);
				});
				return eventInfoData;
			}

			// Token: 0x04008E62 RID: 36450
			public GameStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, object>.State notify;

			// Token: 0x04008E63 RID: 36451
			public GameStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, object>.State ending;
		}
	}
}
