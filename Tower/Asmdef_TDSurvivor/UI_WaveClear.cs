using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000190 RID: 400
public class UI_WaveClear : AUISituational
{
	// Token: 0x06000AB6 RID: 2742 RVA: 0x000284AE File Offset: 0x000266AE
	private void OnEnable()
	{
		EventMgr.Register<int>(eGameEvents.UI_WaveClearUI_Show, new Action<int>(this.OnRoundEnd));
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x000284CB File Offset: 0x000266CB
	private void OnDisable()
	{
		EventMgr.Remove<int>(eGameEvents.UI_WaveClearUI_Show, new Action<int>(this.OnRoundEnd));
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x000284E8 File Offset: 0x000266E8
	private void OnRoundEnd(int reward)
	{
		this.UpdateLoc(reward);
		base.StartCoroutine(this.CR_WaveClearAnnounce());
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x000284FE File Offset: 0x000266FE
	private IEnumerator CR_WaveClearAnnounce()
	{
		base.Toggle(true);
		yield return null;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.node_Reward);
		SoundManager.PlaySound("UI", "RoundEnd", -1f, -1f, -1f);
		yield return new WaitForSeconds(this.waitTime);
		base.Toggle(false);
		yield break;
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x00028510 File Offset: 0x00026710
	private void UpdateLoc(int reward)
	{
		this.text_Clear.text = LocalizationManager.Instance.GetString("UI", "WAVECLEAR", Array.Empty<object>());
		this.text_Reward.text = LocalizationManager.Instance.GetString("UI", "WAVECLEAR_REWARD", Array.Empty<object>());
		this.text_RewardValue.text = reward.ToString();
	}

	// Token: 0x04000838 RID: 2104
	[SerializeField]
	private TMP_Text text_Clear;

	// Token: 0x04000839 RID: 2105
	[SerializeField]
	private TMP_Text text_Reward;

	// Token: 0x0400083A RID: 2106
	[SerializeField]
	private TMP_Text text_RewardValue;

	// Token: 0x0400083B RID: 2107
	[SerializeField]
	private RectTransform node_Reward;

	// Token: 0x0400083C RID: 2108
	[SerializeField]
	private float waitTime = 2f;
}
