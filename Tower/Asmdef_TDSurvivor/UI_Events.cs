using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000163 RID: 355
public class UI_Events : AUISituational
{
	// Token: 0x0600095D RID: 2397 RVA: 0x00023767 File Offset: 0x00021967
	private void OnEnable()
	{
		EventMgr.Register<EventStageData>(eMapSceneEvents.StartEventBlockProcess, new Action<EventStageData>(this.OnStartMapEvent));
		this.button.onClick.AddListener(new UnityAction(this.OnPressButton));
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0002379C File Offset: 0x0002199C
	private void OnDisable()
	{
		EventMgr.Remove<EventStageData>(eMapSceneEvents.StartEventBlockProcess, new Action<EventStageData>(this.OnStartMapEvent));
		this.button.onClick.RemoveListener(new UnityAction(this.OnPressButton));
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x000237D1 File Offset: 0x000219D1
	private void OnStartMapEvent(EventStageData data)
	{
		base.Toggle(true);
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x000237DA File Offset: 0x000219DA
	private void OnPressButton()
	{
		base.Toggle(false);
		EventMgr.SendEvent(eMapSceneEvents.MapBlockCompleted);
	}

	// Token: 0x0400076B RID: 1899
	[SerializeField]
	private Button button;
}
