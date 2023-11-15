using System;
using Refic.Emberward.Minigame;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000142 RID: 322
public class Obj_UI_OverchargeButton : MonoBehaviour
{
	// Token: 0x06000851 RID: 2129 RVA: 0x0001F888 File Offset: 0x0001DA88
	private void OnEnable()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0001F8A6 File Offset: 0x0001DAA6
	private void OnDisable()
	{
		this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x0001F8C4 File Offset: 0x0001DAC4
	public void OnClickButton()
	{
		Action<int, OverchargeItemData> clickButtonCallback = this.ClickButtonCallback;
		if (clickButtonCallback == null)
		{
			return;
		}
		clickButtonCallback(this.index, this.data);
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0001F8E2 File Offset: 0x0001DAE2
	public void SetContent(int index, OverchargeItemData data, Color textColor)
	{
		this.index = index;
		this.data = data;
		this.text_Number.text = data.showText;
		this.text_Number.color = textColor;
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x0001F910 File Offset: 0x0001DB10
	public void SetButtonState(Obj_UI_OverchargeButton.eButtonAnimState state)
	{
		switch (state)
		{
		case Obj_UI_OverchargeButton.eButtonAnimState.IDLE:
			this.animator.CrossFade("Idle", 0f, 0);
			return;
		case Obj_UI_OverchargeButton.eButtonAnimState.CORRECT:
			this.animator.SetTrigger("Correct");
			return;
		case Obj_UI_OverchargeButton.eButtonAnimState.WRONG:
			this.animator.SetTrigger("Wrong");
			return;
		case Obj_UI_OverchargeButton.eButtonAnimState.CLICK:
			this.animator.SetTrigger("Click");
			return;
		case Obj_UI_OverchargeButton.eButtonAnimState.COMPLETED:
			this.animator.SetTrigger("Completed");
			return;
		default:
			return;
		}
	}

	// Token: 0x040006B7 RID: 1719
	[SerializeField]
	private Animator animator;

	// Token: 0x040006B8 RID: 1720
	[SerializeField]
	private TMP_Text text_Number;

	// Token: 0x040006B9 RID: 1721
	[SerializeField]
	private Button button;

	// Token: 0x040006BA RID: 1722
	private int index = -1;

	// Token: 0x040006BB RID: 1723
	private OverchargeItemData data;

	// Token: 0x040006BC RID: 1724
	public Action<int, OverchargeItemData> ClickButtonCallback;

	// Token: 0x0200027F RID: 639
	public enum eButtonAnimState
	{
		// Token: 0x04000BE1 RID: 3041
		IDLE,
		// Token: 0x04000BE2 RID: 3042
		CORRECT,
		// Token: 0x04000BE3 RID: 3043
		WRONG,
		// Token: 0x04000BE4 RID: 3044
		CLICK,
		// Token: 0x04000BE5 RID: 3045
		COMPLETED
	}
}
