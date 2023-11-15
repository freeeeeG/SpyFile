using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class UI_StageAnnounce : AUISituational
{
	// Token: 0x06000A5D RID: 2653 RVA: 0x00026A46 File Offset: 0x00024C46
	private void OnEnable()
	{
		EventMgr.Register<int, float>(eGameEvents.UI_ShowStageAnnounce, new Action<int, float>(this.OnShowStageAnnounce));
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00026A60 File Offset: 0x00024C60
	private void OnDisable()
	{
		EventMgr.Remove<int, float>(eGameEvents.UI_ShowStageAnnounce, new Action<int, float>(this.OnShowStageAnnounce));
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00026A7C File Offset: 0x00024C7C
	private void OnShowStageAnnounce(int index, float duration)
	{
		string @string = LocalizationManager.Instance.GetString("StageInfo", "Stage", Array.Empty<object>());
		string string2 = LocalizationManager.Instance.GetString("StageInfo", string.Format("Stage_{0:000}", index), Array.Empty<object>());
		this.text_StageName.text = string2;
		this.text_StageDescription.text = string.Format("{0} {1}", @string, index);
		base.StartCoroutine(this.CR_Proc(duration));
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00026AFE File Offset: 0x00024CFE
	private IEnumerator CR_Proc(float duration)
	{
		base.Toggle(true);
		SoundManager.PlaySound("UI", "StageAnnounce_Show", -1f, -1f, -1f);
		yield return new WaitForSeconds(duration);
		base.Toggle(false);
		SoundManager.PlaySound("UI", "StageAnnounce_Hide", -1f, -1f, -1f);
		yield break;
	}

	// Token: 0x040007F1 RID: 2033
	[SerializeField]
	private TMP_Text text_StageName;

	// Token: 0x040007F2 RID: 2034
	[SerializeField]
	private TMP_Text text_StageDescription;
}
