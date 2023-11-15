using System;
using Refic.Emberward.Minigame;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200018C RID: 396
public class UI_TowerControl : AUISituational
{
	// Token: 0x06000A8F RID: 2703 RVA: 0x000277A0 File Offset: 0x000259A0
	private void OnEnable()
	{
		EventMgr.Register<ABaseTower>(eGameEvents.OnClickTowerOnField, new Action<ABaseTower>(this.OnClickTowerOnField));
		this.button_Upgrade.onClick.AddListener(new UnityAction(this.OnClickButtonUpgrade));
		this.button_Sell.onClick.AddListener(new UnityAction(this.OnClickButtonSell));
		this.button_TargetPriority.onClick.AddListener(new UnityAction(this.OnClickTargetPriority));
		this.button_OverChargeTower.onClick.AddListener(new UnityAction(this.OnClickOverchargeTower));
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00027838 File Offset: 0x00025A38
	private void OnDisable()
	{
		EventMgr.Remove<ABaseTower>(eGameEvents.OnClickTowerOnField, new Action<ABaseTower>(this.OnClickTowerOnField));
		this.button_Upgrade.onClick.RemoveListener(new UnityAction(this.OnClickButtonUpgrade));
		this.button_Sell.onClick.RemoveListener(new UnityAction(this.OnClickButtonSell));
		this.button_TargetPriority.onClick.RemoveListener(new UnityAction(this.OnClickTargetPriority));
		this.button_OverChargeTower.onClick.RemoveListener(new UnityAction(this.OnClickOverchargeTower));
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x000278D0 File Offset: 0x00025AD0
	private void OnClickTowerOnField(ABaseTower tower)
	{
		if (this.curTower != null && this.curTower != tower)
		{
			ABaseTower abaseTower = this.curTower;
			abaseTower.OnTowerDespawn = (Action<ABaseTower>)Delegate.Remove(abaseTower.OnTowerDespawn, new Action<ABaseTower>(this.OnTowerDespawn));
		}
		this.curTower = tower;
		tower.OnTowerDespawn = (Action<ABaseTower>)Delegate.Combine(tower.OnTowerDespawn, new Action<ABaseTower>(this.OnTowerDespawn));
		this.SetPositionToTower();
		this.UpdateUIContent(tower);
		this.button_OverChargeTower.gameObject.SetActive(false);
		base.Toggle(true);
		EventMgr.SendEvent<bool>(eGameEvents.LockRangeIndicator, false);
		EventMgr.SendEvent<ABaseTower, float>(eGameEvents.SetupRangeIndicator, tower, tower.SettingData.GetAttackRange(1f));
		EventMgr.SendEvent<bool>(eGameEvents.ToggleRangeIndicator, true);
		EventMgr.SendEvent<bool>(eGameEvents.LockRangeIndicator, true);
		SoundManager.PlaySound("UI", "TowerControl_Show", -1f, -1f, -1f);
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x000279D5 File Offset: 0x00025BD5
	private void OnTowerDespawn(ABaseTower tower)
	{
		if (this.curTower != tower)
		{
			return;
		}
		EventMgr.SendEvent<bool>(eGameEvents.LockRangeIndicator, false);
		EventMgr.SendEvent<bool>(eGameEvents.ToggleRangeIndicator, false);
		this.Close();
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x00027A08 File Offset: 0x00025C08
	private void UpdateUIContent(ABaseTower tower)
	{
		this.text_UpgradeCostPrefix.text = LocalizationManager.Instance.GetString("UI", "TOWER_CONTROL_UPGRADE", Array.Empty<object>());
		this.text_UpgradeCost.text = string.Format("${0}", tower.SettingData.GetBuildCost(1f));
		this.text_SellPricePrefix.text = LocalizationManager.Instance.GetString("UI", "TOWER_CONTROL_SELL", Array.Empty<object>());
		this.text_SellPrice.text = string.Format("${0}", tower.GetSellValue());
		this.text_TowerOvercharge.text = LocalizationManager.Instance.GetString("UI", "TOWER_CONTROL_OVERCHARGE", Array.Empty<object>());
		string @string = LocalizationManager.Instance.GetString("UI", "TOWER_TARGET_PRIORITY_TEXT", Array.Empty<object>());
		string string2 = LocalizationManager.Instance.GetString("UI", tower.TargetPriority.GetLocKey(), Array.Empty<object>());
		this.text_TargetPriorityType.text = @string + "<color=#cc2222>" + string2 + "</color>";
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x00027B21 File Offset: 0x00025D21
	private void OnClickButtonUpgrade()
	{
		SoundManager.PlaySound("UI", "TowerControl_ButtonClick", -1f, -1f, -1f);
		EventMgr.SendEvent<ABaseTower>(eGameEvents.RequestUpgradeTower, this.curTower);
		this.Close();
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00027B5B File Offset: 0x00025D5B
	private void OnClickButtonSell()
	{
		SoundManager.PlaySound("UI", "TowerControl_ButtonClick", -1f, -1f, -1f);
		EventMgr.SendEvent<ABaseTower>(eGameEvents.RequestSellTower, this.curTower);
		this.Close();
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x00027B95 File Offset: 0x00025D95
	private void OnClickTargetPriority()
	{
		SoundManager.PlaySound("UI", "TowerControl_ButtonClick", -1f, -1f, -1f);
		this.curTower.SwitchToNextTargetPriority();
		this.UpdateUIContent(this.curTower);
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x00027BCD File Offset: 0x00025DCD
	private void OnClickOverchargeTower()
	{
		SoundManager.PlaySound("UI", "TowerControl_ButtonClick", -1f, -1f, -1f);
		APopupWindow.CreateWindow<UI_TowerOvercharge_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false).StartMinigame(eOverchargeType.PRESS_ALL_BUTTONS_FROM_1_TO_9, this.curTower);
		this.Close();
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x00027C08 File Offset: 0x00025E08
	private void Close()
	{
		this.curTower = null;
		base.Toggle(false);
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00027C18 File Offset: 0x00025E18
	private void Update()
	{
		if (this.curTower != null && this.curTower.IsInitialized)
		{
			this.SetPositionToTower();
		}
		if (base.IsUIActivated && Input.GetMouseButtonDown(1))
		{
			EventMgr.SendEvent<bool>(eGameEvents.LockRangeIndicator, false);
			EventMgr.SendEvent<bool>(eGameEvents.ToggleRangeIndicator, false);
			this.Close();
		}
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00027C78 File Offset: 0x00025E78
	private void SetPositionToTower()
	{
		Vector3 position = Singleton<CameraManager>.Instance.Calculate2DPosFrom3DPos(this.curTower.transform.position);
		base.transform.position = position;
	}

	// Token: 0x04000813 RID: 2067
	[SerializeField]
	[Header("按鈕:升級")]
	private UI_HoldableButton button_Upgrade;

	// Token: 0x04000814 RID: 2068
	[SerializeField]
	[Header("按鈕:賣塔")]
	private UI_HoldableButton button_Sell;

	// Token: 0x04000815 RID: 2069
	[SerializeField]
	[Header("按鈕:目標優先權")]
	private Button button_TargetPriority;

	// Token: 0x04000816 RID: 2070
	[SerializeField]
	[Header("按鈕:超載")]
	private Button button_OverChargeTower;

	// Token: 0x04000817 RID: 2071
	[SerializeField]
	[Header("文字:升級費用")]
	private TMP_Text text_UpgradeCostPrefix;

	// Token: 0x04000818 RID: 2072
	[SerializeField]
	[Header("文字:升級費用數值")]
	private TMP_Text text_UpgradeCost;

	// Token: 0x04000819 RID: 2073
	[SerializeField]
	[Header("文字:賣塔價格")]
	private TMP_Text text_SellPricePrefix;

	// Token: 0x0400081A RID: 2074
	[SerializeField]
	[Header("文字:賣塔價格數值")]
	private TMP_Text text_SellPrice;

	// Token: 0x0400081B RID: 2075
	[SerializeField]
	[Header("文字:目標優先權")]
	private TMP_Text text_TargetPriorityType;

	// Token: 0x0400081C RID: 2076
	[SerializeField]
	[Header("文字:超載")]
	private TMP_Text text_TowerOvercharge;

	// Token: 0x0400081D RID: 2077
	private ABaseTower curTower;
}
