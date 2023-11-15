using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class TutorialManager : MonoBehaviour
{
	// Token: 0x0600078A RID: 1930 RVA: 0x0001CA30 File Offset: 0x0001AC30
	private void OnEnable()
	{
		EventMgr.Register<eTutorialType, float, Action>(eGameEvents.RequestTutorial, new Action<eTutorialType, float, Action>(this.OnRequestTutorial));
		EventMgr.Register<eTutorialType>(eGameEvents.QueueTutorialForGameStart, new Action<eTutorialType>(this.OnQueueTutorialForGameStart));
		EventMgr.Register<eGameEvents, eTutorialType>(eGameEvents.QueueTutorialForEvent, new Action<eGameEvents, eTutorialType>(this.OnQueueTutorialForEvent));
		EventMgr.Register<Action>(eGameEvents.RequestStartQueuedTutorial, new Action<Action>(this.OnRequestStartQueuedTutorial));
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x0001CAA0 File Offset: 0x0001ACA0
	private void OnDisable()
	{
		EventMgr.Remove<eTutorialType, float, Action>(eGameEvents.RequestTutorial, new Action<eTutorialType, float, Action>(this.OnRequestTutorial));
		EventMgr.Remove<eTutorialType>(eGameEvents.QueueTutorialForGameStart, new Action<eTutorialType>(this.OnQueueTutorialForGameStart));
		EventMgr.Remove<eGameEvents, eTutorialType>(eGameEvents.QueueTutorialForEvent, new Action<eGameEvents, eTutorialType>(this.OnQueueTutorialForEvent));
		EventMgr.Remove<Action>(eGameEvents.RequestStartQueuedTutorial, new Action<Action>(this.OnRequestStartQueuedTutorial));
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x0001CB10 File Offset: 0x0001AD10
	private void OnQueueTutorialForEvent(eGameEvents eventType, eTutorialType tutorialType)
	{
		if (this.dic_EventToTutorial == null)
		{
			this.dic_EventToTutorial = new TutorialManager.EventToTutorialDic();
		}
		if (!this.dic_EventToTutorial.ContainsKey(eventType))
		{
			this.dic_EventToTutorial.Add(eventType, tutorialType);
		}
		new SingleEventCapturer(eventType, delegate()
		{
			this.PlayTutorial(this.dic_EventToTutorial[eventType], 0.5f, null);
		});
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0001CB88 File Offset: 0x0001AD88
	private void OnRequestTutorial(eTutorialType tutorialType, float delay, Action finishCallback)
	{
		Debug.Log(string.Format("收到教學要求: {0}", tutorialType));
		if (GameDataManager.instance.Playerdata.IsFinishedTutorial(tutorialType))
		{
			Debug.Log(string.Format("已經跑過教學 {0}", tutorialType));
			if (finishCallback != null)
			{
				finishCallback();
			}
			return;
		}
		this.PlayTutorial(tutorialType, delay, finishCallback);
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x0001CBE5 File Offset: 0x0001ADE5
	private void OnRequestStartQueuedTutorial(Action finishCallback)
	{
		if (this.list_QueuedTutorialForGameStart.Count != 0)
		{
			base.StartCoroutine(this.CR_PlayQueuedTutorial(finishCallback));
			return;
		}
		if (finishCallback != null)
		{
			finishCallback();
		}
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x0001CC0C File Offset: 0x0001AE0C
	private IEnumerator CR_PlayQueuedTutorial(Action finishCallback)
	{
		yield return new WaitForSeconds(0.5f);
		foreach (eTutorialType eTutorialType in this.list_QueuedTutorialForGameStart)
		{
			TutorialManager.<>c__DisplayClass8_0 CS$<>8__locals1 = new TutorialManager.<>c__DisplayClass8_0();
			CS$<>8__locals1.isFinished = false;
			if (GameDataManager.instance.Playerdata.IsFinishedTutorial(eTutorialType))
			{
				Debug.Log(string.Format("已經跑過教學 {0}", eTutorialType));
				if (finishCallback != null)
				{
					finishCallback();
				}
			}
			else
			{
				this.PlayTutorial(eTutorialType, 0f, delegate
				{
					CS$<>8__locals1.isFinished = true;
				});
				while (!CS$<>8__locals1.isFinished)
				{
					yield return null;
				}
				yield return new WaitForSeconds(0.15f);
				CS$<>8__locals1 = null;
			}
		}
		List<eTutorialType>.Enumerator enumerator = default(List<eTutorialType>.Enumerator);
		this.list_QueuedTutorialForGameStart.Clear();
		if (finishCallback != null)
		{
			finishCallback();
		}
		yield break;
		yield break;
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0001CC22 File Offset: 0x0001AE22
	private void OnQueueTutorialForGameStart(eTutorialType type)
	{
		if (!this.list_QueuedTutorialForGameStart.Contains(type))
		{
			this.list_QueuedTutorialForGameStart.Add(type);
		}
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0001CC3E File Offset: 0x0001AE3E
	private Coroutine PlayTutorial(eTutorialType tutorialType, float delay, Action finishCallback)
	{
		return base.StartCoroutine(this.CR_TutorialProc(tutorialType, delay, finishCallback));
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x0001CC4F File Offset: 0x0001AE4F
	private IEnumerator CR_TutorialProc(eTutorialType tutorialType, float delay, Action finishCallback)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		Debug.Log(string.Format("啟動教學 {0}", tutorialType));
		UI_ItemTutorialPopup ui_ItemTutorialPopup = APopupWindow.CreateWindow<UI_ItemTutorialPopup>(APopupWindow.ePopupWindowLayer.TOP, null, false);
		ui_ItemTutorialPopup.OnWindowFinished = (Action)Delegate.Combine(ui_ItemTutorialPopup.OnWindowFinished, finishCallback);
		ui_ItemTutorialPopup.SetTutorialType(tutorialType);
		EventMgr.SendEvent<eTutorialType>(eGameEvents.RequestSetTutorialFinished, tutorialType);
		yield break;
	}

	// Token: 0x04000624 RID: 1572
	[SerializeField]
	private List<eTutorialType> list_QueuedTutorialForGameStart;

	// Token: 0x04000625 RID: 1573
	[SerializeField]
	private TutorialManager.EventToTutorialDic dic_EventToTutorial;

	// Token: 0x02000272 RID: 626
	public class EventToTutorialDic : SerializableDictionary<eGameEvents, eTutorialType>
	{
	}
}
