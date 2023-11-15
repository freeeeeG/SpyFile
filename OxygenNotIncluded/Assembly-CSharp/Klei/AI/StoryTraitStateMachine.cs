using System;
using Database;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000E0A RID: 3594
	public abstract class StoryTraitStateMachine<TStateMachine, TInstance, TDef> : GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef> where TStateMachine : StoryTraitStateMachine<TStateMachine, TInstance, TDef> where TInstance : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitInstance where TDef : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitDef
	{
		// Token: 0x02001F8B RID: 8075
		public class TraitDef : StateMachine.BaseDef
		{
			// Token: 0x04008E6E RID: 36462
			public string InitalLoreId;

			// Token: 0x04008E6F RID: 36463
			public string CompleteLoreId;

			// Token: 0x04008E70 RID: 36464
			public Story Story;

			// Token: 0x04008E71 RID: 36465
			public StoryCompleteData CompletionData;

			// Token: 0x04008E72 RID: 36466
			public StoryManager.PopupInfo EventIntroInfo = new StoryManager.PopupInfo
			{
				PopupType = EventInfoDataHelper.PopupType.NONE
			};

			// Token: 0x04008E73 RID: 36467
			public StoryManager.PopupInfo EventCompleteInfo = new StoryManager.PopupInfo
			{
				PopupType = EventInfoDataHelper.PopupType.NONE
			};
		}

		// Token: 0x02001F8C RID: 8076
		public class TraitInstance : GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef>.GameInstance
		{
			// Token: 0x0600A2E0 RID: 41696 RVA: 0x00367098 File Offset: 0x00365298
			public TraitInstance(StateMachineController master) : base(master)
			{
				StoryManager.Instance.ForceCreateStory(base.def.Story, base.gameObject.GetMyWorldId());
				this.buildingActivatedHandle = master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
			}

			// Token: 0x0600A2E1 RID: 41697 RVA: 0x003670F8 File Offset: 0x003652F8
			public TraitInstance(StateMachineController master, TDef def) : base(master, def)
			{
				StoryManager.Instance.ForceCreateStory(def.Story, base.gameObject.GetMyWorldId());
				this.buildingActivatedHandle = master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
			}

			// Token: 0x0600A2E2 RID: 41698 RVA: 0x00367154 File Offset: 0x00365354
			public override void StartSM()
			{
				this.selectable = base.GetComponent<KSelectable>();
				this.notifier = base.gameObject.AddOrGet<Notifier>();
				base.StartSM();
				base.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				if (this.buildingActivatedHandle == -1)
				{
					this.buildingActivatedHandle = base.master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
				}
				this.TriggerStoryEvent(StoryInstance.State.DISCOVERED);
			}

			// Token: 0x0600A2E3 RID: 41699 RVA: 0x003671CF File Offset: 0x003653CF
			public override void StopSM(string reason)
			{
				base.StopSM(reason);
				base.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				base.Unsubscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
				this.buildingActivatedHandle = -1;
			}

			// Token: 0x0600A2E4 RID: 41700 RVA: 0x00367210 File Offset: 0x00365410
			public void TriggerStoryEvent(StoryInstance.State storyEvent)
			{
				switch (storyEvent)
				{
				case StoryInstance.State.RETROFITTED:
				case StoryInstance.State.NOT_STARTED:
					break;
				case StoryInstance.State.DISCOVERED:
					StoryManager.Instance.DiscoverStoryEvent(base.def.Story);
					return;
				case StoryInstance.State.IN_PROGRESS:
					StoryManager.Instance.BeginStoryEvent(base.def.Story);
					return;
				case StoryInstance.State.COMPLETE:
				{
					Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), base.def.CompletionData.KeepSakeSpawnOffset), Grid.SceneLayer.Ore);
					StoryManager.Instance.CompleteStoryEvent(base.def.Story, keepsakeSpawnPosition);
					break;
				}
				default:
					return;
				}
			}

			// Token: 0x0600A2E5 RID: 41701 RVA: 0x003672B8 File Offset: 0x003654B8
			protected virtual void OnBuildingActivated(object activated)
			{
				if (!(bool)activated)
				{
					return;
				}
				this.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
			}

			// Token: 0x0600A2E6 RID: 41702 RVA: 0x003672CC File Offset: 0x003654CC
			protected virtual void OnObjectSelect(object clicked)
			{
				if (!(bool)clicked)
				{
					return;
				}
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE)
				{
					this.OnNotificationClicked(null);
					return;
				}
				if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
				{
					this.DisplayPopup(base.def.EventIntroInfo);
				}
			}

			// Token: 0x0600A2E7 RID: 41703 RVA: 0x0036734C File Offset: 0x0036554C
			public virtual void CompleteEvent()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
				{
					return;
				}
				this.DisplayPopup(base.def.EventCompleteInfo);
			}

			// Token: 0x0600A2E8 RID: 41704 RVA: 0x0036739C File Offset: 0x0036559C
			public virtual void OnCompleteStorySequence()
			{
				this.TriggerStoryEvent(StoryInstance.State.COMPLETE);
			}

			// Token: 0x0600A2E9 RID: 41705 RVA: 0x003673A8 File Offset: 0x003655A8
			protected void DisplayPopup(StoryManager.PopupInfo info)
			{
				if (info.PopupType == EventInfoDataHelper.PopupType.NONE)
				{
					return;
				}
				StoryInstance storyInstance = StoryManager.Instance.DisplayPopup(base.def.Story, info, new System.Action(this.OnPopupClosed), new Notification.ClickCallback(this.OnNotificationClicked));
				if (storyInstance != null && !info.DisplayImmediate)
				{
					this.selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
					this.notifier.Add(storyInstance.Notification, "");
				}
			}

			// Token: 0x0600A2EA RID: 41706 RVA: 0x0036743C File Offset: 0x0036563C
			public void OnNotificationClicked(object data = null)
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				this.selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
				this.notifier.Remove(storyInstance.Notification);
				if (storyInstance.PendingType == EventInfoDataHelper.PopupType.COMPLETE)
				{
					this.ShowEventCompleteUI();
					return;
				}
				this.ShowEventBeginUI();
			}

			// Token: 0x0600A2EB RID: 41707 RVA: 0x003674B0 File Offset: 0x003656B0
			public virtual void OnPopupClosed()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
				{
					Game.Instance.unlocks.Unlock(base.def.CompleteLoreId, true);
					return;
				}
				Game.Instance.unlocks.Unlock(base.def.InitalLoreId, true);
			}

			// Token: 0x0600A2EC RID: 41708 RVA: 0x0036752B File Offset: 0x0036572B
			protected virtual void ShowEventBeginUI()
			{
			}

			// Token: 0x0600A2ED RID: 41709 RVA: 0x00367530 File Offset: 0x00365730
			protected virtual void ShowEventCompleteUI()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), base.def.CompletionData.CameraTargetOffset), Grid.SceneLayer.Ore);
				StoryManager.Instance.CompleteStoryEvent(base.def.Story, base.master, new FocusTargetSequence.Data
				{
					WorldId = base.master.GetMyWorldId(),
					OrthographicSize = 6f,
					TargetSize = 6f,
					Target = target,
					PopupData = storyInstance.EventInfo,
					CompleteCB = new System.Action(this.OnCompleteStorySequence),
					CanCompleteCB = null
				});
			}

			// Token: 0x04008E74 RID: 36468
			protected int buildingActivatedHandle = -1;

			// Token: 0x04008E75 RID: 36469
			protected Notifier notifier;

			// Token: 0x04008E76 RID: 36470
			protected KSelectable selectable;
		}
	}
}
