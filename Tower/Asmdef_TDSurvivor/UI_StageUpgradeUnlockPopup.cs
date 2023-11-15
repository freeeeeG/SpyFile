using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000189 RID: 393
public class UI_StageUpgradeUnlockPopup : APopupWindow
{
	// Token: 0x06000A69 RID: 2665 RVA: 0x00026D18 File Offset: 0x00024F18
	private void OnEnable()
	{
		this.button_AddTowerSlot.onClick.AddListener(new UnityAction(this.OnClickButton_AddTowerSlot));
		this.button_HandDrawIncrease.onClick.AddListener(new UnityAction(this.OnClickButton_HandDrawIncrease));
		this.button_AddExp.onClick.AddListener(new UnityAction(this.OnClickButton_AddExp));
		this.button_AddGold.onClick.AddListener(new UnityAction(this.OnClickButton_AddGold));
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x00026D98 File Offset: 0x00024F98
	private void OnDisable()
	{
		this.button_AddTowerSlot.onClick.RemoveListener(new UnityAction(this.OnClickButton_AddTowerSlot));
		this.button_HandDrawIncrease.onClick.RemoveListener(new UnityAction(this.OnClickButton_HandDrawIncrease));
		this.button_AddExp.onClick.RemoveListener(new UnityAction(this.OnClickButton_AddExp));
		this.button_AddGold.onClick.RemoveListener(new UnityAction(this.OnClickButton_AddGold));
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x00026E18 File Offset: 0x00025018
	protected override void ShowWindowProc()
	{
		GameplayData gameplayData = GameDataManager.instance.GameplayData;
		bool flag = gameplayData.TowerCardLimit >= gameplayData.MAX_TOWER_CARD_LIMIT;
		this.list_UpgradeItems.Find((UI_Obj_StageUpgradeItem x) => x.UpgradeType == UI_StageUpgradeUnlockPopup.eUpgradeType.AddTowerSlot).gameObject.SetActive(!flag);
		bool flag2 = gameplayData.DrawCardPerRound >= gameplayData.MAX_DRAW_CARD_PER_ROUND;
		this.list_UpgradeItems.Find((UI_Obj_StageUpgradeItem x) => x.UpgradeType == UI_StageUpgradeUnlockPopup.eUpgradeType.HandDrawIncrease).gameObject.SetActive(!flag2);
		this.animator.SetBool("isOn", true);
		base.StartCoroutine(this.CR_ShowWindowProc());
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x00026EE2 File Offset: 0x000250E2
	private IEnumerator CR_ShowWindowProc()
	{
		yield return new WaitForSeconds(0.15f);
		int num;
		for (int i = 0; i < this.list_UpgradeItems.Count; i = num + 1)
		{
			SoundManager.PlaySound("UI", "StageUpgrade_ItemShow", -1f, -1f, -1f);
			if (this.list_UpgradeItems[i].gameObject.activeInHierarchy)
			{
				this.list_UpgradeItems[i].Toggle(true);
			}
			yield return new WaitForSeconds(0.05f);
			num = i;
		}
		yield break;
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x00026EF1 File Offset: 0x000250F1
	protected override void CloseWindowProc()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00026F04 File Offset: 0x00025104
	private void Anim_AfterSelectedItem(UI_StageUpgradeUnlockPopup.eUpgradeType selectedType)
	{
		foreach (UI_Obj_StageUpgradeItem ui_Obj_StageUpgradeItem in this.list_UpgradeItems)
		{
			if (ui_Obj_StageUpgradeItem.UpgradeType == selectedType)
			{
				base.StartCoroutine(this.CR_SelectedAnimProc(ui_Obj_StageUpgradeItem));
			}
			else
			{
				ui_Obj_StageUpgradeItem.Toggle(false);
			}
		}
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x00026F70 File Offset: 0x00025170
	private IEnumerator CR_SelectedAnimProc(UI_Obj_StageUpgradeItem item)
	{
		SoundManager.PlaySound("UI", "StageUpgrade_SelectItem", -1f, -1f, -1f);
		item.transform.DOMove(this.node_Center.position, 0.33f, false).SetEase(Ease.OutCubic);
		yield return new WaitForSeconds(0.33f);
		item.PlaySelectedAnimation();
		item.transform.DOScale(1.66f, 0.5f).SetEase(Ease.OutElastic);
		SoundManager.PlaySound("UI", "StageUpgrade_SelectedEffect", -1f, -1f, -1f);
		yield return new WaitForSeconds(2f);
		item.Toggle(false);
		base.CloseWindow();
		yield break;
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x00026F86 File Offset: 0x00025186
	private void OnClickButton_AddTowerSlot()
	{
		if (this.isUpgradeSelected)
		{
			return;
		}
		this.isUpgradeSelected = true;
		EventMgr.SendEvent<int>(eGameEvents.RequestAddTowerCardLimit, 1);
		this.Anim_AfterSelectedItem(UI_StageUpgradeUnlockPopup.eUpgradeType.AddTowerSlot);
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x00026FAD File Offset: 0x000251AD
	private void OnClickButton_HandDrawIncrease()
	{
		if (this.isUpgradeSelected)
		{
			return;
		}
		this.isUpgradeSelected = true;
		EventMgr.SendEvent<int>(eGameEvents.RequestAddDrawCardCount, 1);
		this.Anim_AfterSelectedItem(UI_StageUpgradeUnlockPopup.eUpgradeType.HandDrawIncrease);
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x00026FD4 File Offset: 0x000251D4
	private void OnClickButton_AddExp()
	{
		if (this.isUpgradeSelected)
		{
			return;
		}
		this.isUpgradeSelected = true;
		EventMgr.SendEvent<int>(eGameEvents.RequestAddExp, 10);
		this.Anim_AfterSelectedItem(UI_StageUpgradeUnlockPopup.eUpgradeType.AddExp);
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x00026FFC File Offset: 0x000251FC
	private void OnClickButton_AddGold()
	{
		if (this.isUpgradeSelected)
		{
			return;
		}
		this.isUpgradeSelected = true;
		EventMgr.SendEvent<int>(eGameEvents.RequestAddGem, 35);
		this.Anim_AfterSelectedItem(UI_StageUpgradeUnlockPopup.eUpgradeType.AddGold);
	}

	// Token: 0x04000801 RID: 2049
	[SerializeField]
	private List<UI_Obj_StageUpgradeItem> list_UpgradeItems;

	// Token: 0x04000802 RID: 2050
	[SerializeField]
	private Transform node_Center;

	// Token: 0x04000803 RID: 2051
	[SerializeField]
	private Button button_AddTowerSlot;

	// Token: 0x04000804 RID: 2052
	[SerializeField]
	private Button button_HandDrawIncrease;

	// Token: 0x04000805 RID: 2053
	[SerializeField]
	private Button button_AddExp;

	// Token: 0x04000806 RID: 2054
	[SerializeField]
	private Button button_AddGold;

	// Token: 0x04000807 RID: 2055
	private bool isUpgradeSelected;

	// Token: 0x020002A6 RID: 678
	public enum eUpgradeType
	{
		// Token: 0x04000C7F RID: 3199
		NONE,
		// Token: 0x04000C80 RID: 3200
		AddTowerSlot,
		// Token: 0x04000C81 RID: 3201
		HandDrawIncrease,
		// Token: 0x04000C82 RID: 3202
		AddExp,
		// Token: 0x04000C83 RID: 3203
		AddGold
	}
}
