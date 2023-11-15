using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200016D RID: 365
public class UI_MapScene_PlayerGem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600099B RID: 2459 RVA: 0x00024236 File Offset: 0x00022436
	private void OnEnable()
	{
		EventMgr.Register<int>(eGameEvents.OnGemChanged, new Action<int>(this.OnGemChanged));
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x00024250 File Offset: 0x00022450
	private void OnDisable()
	{
		EventMgr.Remove<int>(eGameEvents.OnGemChanged, new Action<int>(this.OnGemChanged));
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0002426C File Offset: 0x0002246C
	private void Start()
	{
		int gem = GameDataManager.instance.GameplayData.Gem;
		this.text_Gem.text = gem.ToString();
		this.curGemValue = gem;
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x000242A4 File Offset: 0x000224A4
	private void OnGemChanged(int value)
	{
		Debug.Log(string.Format("OnGemChanged: {0}", value));
		if (value == this.curGemValue)
		{
			return;
		}
		if (this.coroutine_GemChange != null)
		{
			base.StopCoroutine(this.coroutine_GemChange);
		}
		if (this.curGemValue == -1)
		{
			this.text_Gem.text = value.ToString();
			this.curGemValue = value;
			return;
		}
		this.coroutine_GemChange = base.StartCoroutine(this.CR_GemLerpValue(value));
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x0002431A File Offset: 0x0002251A
	private IEnumerator CR_GemLerpValue(int value)
	{
		Debug.Log(string.Format("CurValue: {0}, LerpValue: {1}", this.curGemValue, value));
		float duration = 0.33f;
		float timer = 0f;
		int startValue = this.curGemValue;
		while (timer < duration)
		{
			this.curGemValue = (int)Mathf.Lerp((float)startValue, (float)value, timer / duration);
			this.text_Gem.text = this.curGemValue.ToString();
			yield return new WaitForSecondsRealtime(0.05f);
			timer += 0.05f;
		}
		this.curGemValue = value;
		this.text_Gem.text = this.curGemValue.ToString();
		this.coroutine_GemChange = null;
		yield break;
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00024330 File Offset: 0x00022530
	public void OnPointerEnter(PointerEventData eventData)
	{
		string @string = LocalizationManager.Instance.GetString("UI", "PLAYER_STAT_GEM_INFO", Array.Empty<object>());
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
		EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, "", @string);
		EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._2D, base.transform, Vector3.right * 200f);
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x000243A4 File Offset: 0x000225A4
	public void OnPointerExit(PointerEventData eventData)
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
	}

	// Token: 0x04000786 RID: 1926
	[SerializeField]
	private Animator animator;

	// Token: 0x04000787 RID: 1927
	[SerializeField]
	private TMP_Text text_Gem;

	// Token: 0x04000788 RID: 1928
	private int curGemValue = -1;

	// Token: 0x04000789 RID: 1929
	private Coroutine coroutine_GemChange;
}
