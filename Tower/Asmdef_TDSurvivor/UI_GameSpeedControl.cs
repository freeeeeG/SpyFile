using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000166 RID: 358
public class UI_GameSpeedControl : AUISituational
{
	// Token: 0x0600096A RID: 2410 RVA: 0x000238C8 File Offset: 0x00021AC8
	private void OnEnable()
	{
		this.button_GameSpeed.onClick.AddListener(new UnityAction(this.OnClickButton_GameSpeed));
		EventMgr.Register(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
		EventMgr.Register(eGameEvents.OnBattleEnd, new Action(this.OnBattleEnd));
		this.UpdateButton();
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x00023928 File Offset: 0x00021B28
	private void OnDisable()
	{
		this.button_GameSpeed.onClick.RemoveListener(new UnityAction(this.OnClickButton_GameSpeed));
		EventMgr.Remove(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
		EventMgr.Remove(eGameEvents.OnBattleEnd, new Action(this.OnBattleEnd));
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x00023981 File Offset: 0x00021B81
	private void OnBattleStart()
	{
		base.Toggle(true);
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0002398A File Offset: 0x00021B8A
	private void OnBattleEnd()
	{
		base.Toggle(false);
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00023994 File Offset: 0x00021B94
	private void OnClickButton_GameSpeed()
	{
		this.currentGameSpeedType = (this.currentGameSpeedType + 1) % UI_GameSpeedControl.eGameSpeedType._2x;
		switch (this.currentGameSpeedType)
		{
		case UI_GameSpeedControl.eGameSpeedType._1x:
			EventMgr.SendEvent<float>(eGameEvents.RequestModifyBattleGameSpeed, 1f);
			break;
		case UI_GameSpeedControl.eGameSpeedType._1p5x:
			EventMgr.SendEvent<float>(eGameEvents.RequestModifyBattleGameSpeed, 1.5f);
			break;
		case UI_GameSpeedControl.eGameSpeedType._2x:
			EventMgr.SendEvent<float>(eGameEvents.RequestModifyBattleGameSpeed, 2f);
			break;
		}
		this.UpdateButton();
		SoundManager.PlaySound("UI", "CommonButton_Click", -1f, -1f, -1f);
		this.animator.SetTrigger("click");
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00023A3C File Offset: 0x00021C3C
	private void UpdateButton()
	{
		for (int i = 0; i < this.list_Node_SpeedIcons.Count; i++)
		{
			this.list_Node_SpeedIcons[i].SetActive(i == (int)this.currentGameSpeedType);
		}
	}

	// Token: 0x04000772 RID: 1906
	[SerializeField]
	private Button button_GameSpeed;

	// Token: 0x04000773 RID: 1907
	[SerializeField]
	private List<GameObject> list_Node_SpeedIcons;

	// Token: 0x04000774 RID: 1908
	[SerializeField]
	private UI_GameSpeedControl.eGameSpeedType currentGameSpeedType;

	// Token: 0x02000295 RID: 661
	private enum eGameSpeedType
	{
		// Token: 0x04000C37 RID: 3127
		_1x,
		// Token: 0x04000C38 RID: 3128
		_1p5x,
		// Token: 0x04000C39 RID: 3129
		_2x
	}
}
