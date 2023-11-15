using System;
using System.Collections;
using System.Collections.Generic;
using Refic.Emberward.Minigame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200018D RID: 397
public class UI_TowerOvercharge_Popup : APopupWindow
{
	// Token: 0x06000A9C RID: 2716 RVA: 0x00027CB4 File Offset: 0x00025EB4
	private void OnEnable()
	{
		foreach (Obj_UI_OverchargeButton obj_UI_OverchargeButton in this.list_OverchargeButton)
		{
			obj_UI_OverchargeButton.ClickButtonCallback = (Action<int, OverchargeItemData>)Delegate.Combine(obj_UI_OverchargeButton.ClickButtonCallback, new Action<int, OverchargeItemData>(this.OnClickButtonCallback));
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00027D20 File Offset: 0x00025F20
	private void OnDisable()
	{
		foreach (Obj_UI_OverchargeButton obj_UI_OverchargeButton in this.list_OverchargeButton)
		{
			obj_UI_OverchargeButton.ClickButtonCallback = (Action<int, OverchargeItemData>)Delegate.Remove(obj_UI_OverchargeButton.ClickButtonCallback, new Action<int, OverchargeItemData>(this.OnClickButtonCallback));
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00027D8C File Offset: 0x00025F8C
	private void Update()
	{
		if (this.isMinigameStarted)
		{
			this.timer_TickSound += Time.deltaTime;
			if (this.timer_TickSound > 0.25f)
			{
				SoundManager.PlaySound("UI", "Overcharge_Tick", -1f, -1f, -1f);
				this.timer_TickSound -= 0.25f;
			}
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x00027DF1 File Offset: 0x00025FF1
	protected override void ShowWindowProc()
	{
		SoundManager.PlaySound("UI", "InfoPopupWindow", -1f, -1f, -1f);
		this.animator.SetBool("isOn", true);
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00027E23 File Offset: 0x00026023
	protected override void CloseWindowProc()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00027E36 File Offset: 0x00026036
	public void StartMinigame(eOverchargeType type, ABaseTower targetTower)
	{
		this.targetTower = targetTower;
		this.cr_MinigameProc = base.StartCoroutine(this.CR_MinigameProc(type));
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00027E54 File Offset: 0x00026054
	private void OnClickButtonCallback(int index, OverchargeItemData data)
	{
		if (!this.isMinigameStarted)
		{
			return;
		}
		bool flag = this.minigame.ValidateButtonPress(index);
		this.list_OverchargeButton[index].SetButtonState(Obj_UI_OverchargeButton.eButtonAnimState.CLICK);
		this.animator.SetTrigger("shake");
		if (flag)
		{
			this.correctCount++;
			SoundManager.PlaySound("UI", "Overcharge_Correct", 0.85f + (float)this.correctCount * 0.2f, -1f, -1f);
			this.list_OverchargeButton[index].SetButtonState(Obj_UI_OverchargeButton.eButtonAnimState.CORRECT);
			return;
		}
		SoundManager.PlaySound("UI", "Overcharge_Wrong", -1f, -1f, -1f);
		this.list_OverchargeButton[index].SetButtonState(Obj_UI_OverchargeButton.eButtonAnimState.WRONG);
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00027F19 File Offset: 0x00026119
	private IEnumerator CR_MinigameProc(eOverchargeType type)
	{
		this.SetupMinigameByType(type);
		this.minigame.Initialize();
		for (int i = 0; i < this.list_OverchargeButton.Count; i++)
		{
			Obj_UI_OverchargeButton obj_UI_OverchargeButton = this.list_OverchargeButton[i];
			OverchargeItemData itemData = this.minigame.GetItemData(i);
			Color textColor = this.list_TextColors.RandomItem<Color>();
			obj_UI_OverchargeButton.SetContent(i, itemData, textColor);
		}
		for (int j = 0; j < this.list_OverchargeButton.Count; j++)
		{
			int siblingIndex = Random.Range(0, this.list_OverchargeButton.Count);
			this.list_OverchargeButton[j].transform.SetSiblingIndex(siblingIndex);
		}
		this.text_Title.text = LocalizationManager.Instance.GetString("UI", "TOWER_OVERCHARGE_TITLE", Array.Empty<object>());
		this.text_Description.text = LocalizationManager.Instance.GetString("UI", string.Format("TOWER_OVERCHARGE_DESC_{0}", type), Array.Empty<object>());
		this.isMinigameStarted = true;
		SoundManager.PlaySound("UI", "Overcharge_Show", -1f, -1f, -1f);
		while (!this.minigame.IsCompleted())
		{
			yield return null;
		}
		this.isMinigameStarted = false;
		SoundManager.PlaySound("UI", "Overcharge_Completed", -1f, -1f, -1f);
		this.text_Description.color = new Color(1f, 0.88f, 0.43f);
		this.text_Description.text = LocalizationManager.Instance.GetString("UI", "TOWER_OVERCHARGE_COMPLETED", Array.Empty<object>());
		foreach (Obj_UI_OverchargeButton obj_UI_OverchargeButton2 in this.list_OverchargeButton)
		{
			obj_UI_OverchargeButton2.SetButtonState(Obj_UI_OverchargeButton.eButtonAnimState.COMPLETED);
		}
		yield return new WaitForSeconds(0.6f);
		this.animator.SetBool("isOn", false);
		yield return new WaitForSeconds(0.2f);
		if (this.targetTower != null)
		{
			EventMgr.SendEvent<ABaseTower, eItemType>(eGameEvents.RequestGiveBuffToTower, this.targetTower, eItemType._3008_OVERCHARGE);
			this.targetTower.ResetShootCooldown();
			GameObject gameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("VFX/VFX_TowerOverload"));
			gameObject.transform.position = this.targetTower.transform.position + Vector3.up * 0.5f;
			gameObject.transform.parent = this.targetTower.transform;
		}
		base.CloseWindow();
		yield break;
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00027F30 File Offset: 0x00026130
	private void SetupMinigameByType(eOverchargeType type)
	{
		switch (type)
		{
		case eOverchargeType.None:
			break;
		case eOverchargeType.PRESS_ALL_BUTTONS_FROM_1_TO_9:
			this.minigame = new TowerOverchargeMinigame_1To9();
			return;
		case eOverchargeType.PRESS_THE_DIFFERENT_BUTTON:
			this.minigame = new TowerOverchargeMinigame_PressDifferent();
			return;
		case eOverchargeType.PRESS_THE_SMALLEST_NUMBER:
			this.minigame = new TowerOverchargeMinigame_PressSmallest();
			return;
		case eOverchargeType.PRESS_ALL_BUTTONS_FROM_9_TO_1:
			this.minigame = new TowerOverchargeMinigame_9To1();
			return;
		case eOverchargeType.PRESS_THE_LARGEST_NUMBER:
			this.minigame = new TowerOverchargeMinigame_PressLargest();
			break;
		case eOverchargeType.PRESS_ALL_BUTTONS:
			this.minigame = new TowerOverchargeMinigame_PressAll();
			return;
		default:
			return;
		}
	}

	// Token: 0x0400081E RID: 2078
	[SerializeField]
	private Button button_Close;

	// Token: 0x0400081F RID: 2079
	[SerializeField]
	private List<Obj_UI_OverchargeButton> list_OverchargeButton;

	// Token: 0x04000820 RID: 2080
	[SerializeField]
	private List<Color> list_TextColors;

	// Token: 0x04000821 RID: 2081
	[SerializeField]
	private TMP_Text text_Title;

	// Token: 0x04000822 RID: 2082
	[SerializeField]
	private TMP_Text text_Description;

	// Token: 0x04000823 RID: 2083
	private ATowerOverchargeMinigame minigame;

	// Token: 0x04000824 RID: 2084
	private bool isMinigameStarted;

	// Token: 0x04000825 RID: 2085
	private Coroutine cr_MinigameProc;

	// Token: 0x04000826 RID: 2086
	private ABaseTower targetTower;

	// Token: 0x04000827 RID: 2087
	private float timer_TickSound;

	// Token: 0x04000828 RID: 2088
	private int correctCount;
}
