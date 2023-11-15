using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200016C RID: 364
public class UI_MapScene_PlayerExp : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000993 RID: 2451 RVA: 0x000240BB File Offset: 0x000222BB
	private void OnEnable()
	{
		EventMgr.Register<int>(eGameEvents.OnExpChanged, new Action<int>(this.OnExpChanged));
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x000240D5 File Offset: 0x000222D5
	private void OnDisable()
	{
		EventMgr.Remove<int>(eGameEvents.OnExpChanged, new Action<int>(this.OnExpChanged));
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x000240F0 File Offset: 0x000222F0
	private void Start()
	{
		int exp = GameDataManager.instance.Playerdata.Exp;
		this.text_Exp.text = exp.ToString();
		this.curExpVaue = exp;
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x00024128 File Offset: 0x00022328
	private void OnExpChanged(int value)
	{
		if (value == this.curExpVaue)
		{
			return;
		}
		if (this.coroutine_ExpChange != null)
		{
			base.StopCoroutine(this.coroutine_ExpChange);
		}
		if (this.curExpVaue == -1)
		{
			this.text_Exp.text = value.ToString();
			this.curExpVaue = value;
			return;
		}
		this.coroutine_ExpChange = base.StartCoroutine(this.CR_ExpLerpValue(value));
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00024189 File Offset: 0x00022389
	private IEnumerator CR_ExpLerpValue(int value)
	{
		float duration = 0.33f;
		float timer = 0f;
		int startValue = this.curExpVaue;
		while (timer < duration)
		{
			this.curExpVaue = (int)Mathf.Lerp((float)startValue, (float)value, Mathf.Clamp01(timer / duration));
			this.text_Exp.text = this.curExpVaue.ToString();
			yield return new WaitForSecondsRealtime(0.05f);
			timer += 0.05f;
		}
		this.curExpVaue = value;
		this.text_Exp.text = this.curExpVaue.ToString();
		this.coroutine_ExpChange = null;
		yield break;
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x000241A0 File Offset: 0x000223A0
	public void OnPointerEnter(PointerEventData eventData)
	{
		string @string = LocalizationManager.Instance.GetString("UI", "PLAYER_STAT_EXP_INFO", Array.Empty<object>());
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
		EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, "", @string);
		EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._2D, base.transform, Vector3.right * 200f);
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00024214 File Offset: 0x00022414
	public void OnPointerExit(PointerEventData eventData)
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
	}

	// Token: 0x04000782 RID: 1922
	[SerializeField]
	private Animator animator;

	// Token: 0x04000783 RID: 1923
	[SerializeField]
	private TMP_Text text_Exp;

	// Token: 0x04000784 RID: 1924
	private int curExpVaue = -1;

	// Token: 0x04000785 RID: 1925
	private Coroutine coroutine_ExpChange;
}
