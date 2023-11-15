using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A8 RID: 168
public class UI_Panel_Battle_BattleButtons : MonoBehaviour
{
	// Token: 0x060005D5 RID: 1493 RVA: 0x000218AF File Offset: 0x0001FAAF
	private void Awake()
	{
		UI_Panel_Battle_BattleButtons.inst = this;
		this.SetTargePosX(this.showPosX);
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x000218C4 File Offset: 0x0001FAC4
	private void Update()
	{
		float x = Mathf.SmoothDamp(base.transform.localPosition.x, this.targetPosX, ref this.refSmooth, this.moveTime);
		float y = base.transform.localPosition.y;
		base.transform.localPosition = new Vector2(x, y);
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x00021921 File Offset: 0x0001FB21
	private void SetTargePosX(float pos)
	{
		this.targetPosX = pos;
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0002192C File Offset: 0x0001FB2C
	public void Show()
	{
		this.UpdateLanguage();
		if (TempData.inst.modeType != EnumModeType.WANDER)
		{
			this.SetTargePosX(this.showPosX);
		}
		Button[] componentsInChildren = base.GetComponentsInChildren<Button>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rect);
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x00021984 File Offset: 0x0001FB84
	public void Hide()
	{
		this.UpdateLanguage();
		if (TempData.inst.modeType == EnumModeType.WANDER)
		{
			return;
		}
		this.SetTargePosX(this.hidePosX);
		Button[] componentsInChildren = base.GetComponentsInChildren<Button>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rect);
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x000219DC File Offset: 0x0001FBDC
	private void UpdateLanguage()
	{
		LanguageText languageText = LanguageText.Inst;
		this.textShop.text = languageText.shop + " [Q]";
		this.textChallenge.text = languageText.challenge + " [E]";
		this.textFight.text = languageText.fight + " [G]";
		if (TempData.inst.modeType == EnumModeType.WANDER)
		{
			this.textFight.gameObject.SetActive(!BattleManager.inst.wander_On);
		}
	}

	// Token: 0x040004CB RID: 1227
	public static UI_Panel_Battle_BattleButtons inst;

	// Token: 0x040004CC RID: 1228
	[SerializeField]
	private RectTransform rect;

	// Token: 0x040004CD RID: 1229
	[SerializeField]
	private float hidePosX;

	// Token: 0x040004CE RID: 1230
	[SerializeField]
	private float showPosX;

	// Token: 0x040004CF RID: 1231
	[SerializeField]
	private Text textShop;

	// Token: 0x040004D0 RID: 1232
	[SerializeField]
	private Text textChallenge;

	// Token: 0x040004D1 RID: 1233
	[SerializeField]
	private Text textFight;

	// Token: 0x040004D2 RID: 1234
	[Header("OnMoving")]
	[SerializeField]
	private float targetPosX;

	// Token: 0x040004D3 RID: 1235
	[SerializeField]
	private float moveTime = 0.5f;

	// Token: 0x040004D4 RID: 1236
	[SerializeField]
	private float refSmooth;
}
