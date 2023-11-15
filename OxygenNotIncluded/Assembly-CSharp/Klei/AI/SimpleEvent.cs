using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DFE RID: 3582
	public class SimpleEvent : GameplayEvent<SimpleEvent.StatesInstance>
	{
		// Token: 0x06006DF1 RID: 28145 RVA: 0x002B5843 File Offset: 0x002B3A43
		public SimpleEvent(string id, string title, string description, string animFileName, string buttonText = null, string buttonTooltip = null) : base(id, 0, 0)
		{
			this.title = title;
			this.description = description;
			this.buttonText = buttonText;
			this.buttonTooltip = buttonTooltip;
			this.animFileName = animFileName;
		}

		// Token: 0x06006DF2 RID: 28146 RVA: 0x002B5879 File Offset: 0x002B3A79
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SimpleEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400527A RID: 21114
		private string buttonText;

		// Token: 0x0400527B RID: 21115
		private string buttonTooltip;

		// Token: 0x02001F85 RID: 8069
		public class States : GameplayEventStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, SimpleEvent>
		{
			// Token: 0x0600A2D1 RID: 41681 RVA: 0x00366E2A File Offset: 0x0036502A
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.root;
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600A2D2 RID: 41682 RVA: 0x00366E40 File Offset: 0x00365040
			public override EventInfoData GenerateEventPopupData(SimpleEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.minions = smi.minions;
				eventInfoData.artifact = smi.artifact;
				EventInfoData.Option option = eventInfoData.AddOption(smi.gameplayEvent.buttonText, null);
				option.callback = delegate()
				{
					if (smi.callback != null)
					{
						smi.callback();
					}
					smi.StopSM("SimpleEvent Finished");
				};
				option.tooltip = smi.gameplayEvent.buttonTooltip;
				if (smi.textParameters != null)
				{
					foreach (global::Tuple<string, string> tuple in smi.textParameters)
					{
						eventInfoData.SetTextParameter(tuple.first, tuple.second);
					}
				}
				return eventInfoData;
			}

			// Token: 0x04008E64 RID: 36452
			public GameStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, object>.State ending;
		}

		// Token: 0x02001F86 RID: 8070
		public class StatesInstance : GameplayEventStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, SimpleEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600A2D4 RID: 41684 RVA: 0x00366F5C File Offset: 0x0036515C
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SimpleEvent simpleEvent) : base(master, eventInstance, simpleEvent)
			{
			}

			// Token: 0x0600A2D5 RID: 41685 RVA: 0x00366F67 File Offset: 0x00365167
			public void SetTextParameter(string key, string value)
			{
				if (this.textParameters == null)
				{
					this.textParameters = new List<global::Tuple<string, string>>();
				}
				this.textParameters.Add(new global::Tuple<string, string>(key, value));
			}

			// Token: 0x0600A2D6 RID: 41686 RVA: 0x00366F8E File Offset: 0x0036518E
			public void ShowEventPopup()
			{
				EventInfoScreen.ShowPopup(base.smi.sm.GenerateEventPopupData(base.smi));
			}

			// Token: 0x04008E65 RID: 36453
			public GameObject[] minions;

			// Token: 0x04008E66 RID: 36454
			public GameObject artifact;

			// Token: 0x04008E67 RID: 36455
			public List<global::Tuple<string, string>> textParameters;

			// Token: 0x04008E68 RID: 36456
			public System.Action callback;
		}
	}
}
