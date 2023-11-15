using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class UI_GameIntro : AUISituational
{
	// Token: 0x06000965 RID: 2405 RVA: 0x0002386D File Offset: 0x00021A6D
	private void OnEnable()
	{
		EventMgr.Register(eMapSceneEvents.TriggerGameIntroUI, new Action(this.OnTriggerGameIntroUI));
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00023887 File Offset: 0x00021A87
	private void OnDisable()
	{
		EventMgr.Remove(eMapSceneEvents.TriggerGameIntroUI, new Action(this.OnTriggerGameIntroUI));
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x000238A1 File Offset: 0x00021AA1
	private void OnTriggerGameIntroUI()
	{
		base.StartCoroutine(this.CR_GameIntroProc());
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x000238B0 File Offset: 0x00021AB0
	private IEnumerator CR_GameIntroProc()
	{
		base.Toggle(true);
		yield return new WaitForSeconds(2f);
		base.Toggle(false);
		yield return new WaitForSeconds(0.5f);
		EventMgr.SendEvent(eMapSceneEvents.OnGameIntroUIFinished);
		yield break;
	}
}
