using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000148 RID: 328
public class UI_BuildTowerButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000893 RID: 2195 RVA: 0x00020D79 File Offset: 0x0001EF79
	private void Awake()
	{
		this.index = base.transform.GetSiblingIndex();
		this.keycode = KeyCode.Alpha1 + this.index;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00020D9C File Offset: 0x0001EF9C
	private void OnEnable()
	{
		EventMgr.Register<List<TowerIngameData>, int>(eGameEvents.OnTowerCardChanged, new Action<List<TowerIngameData>, int>(this.OnTowerChanged));
		EventMgr.Register<List<TowerIngameData>, int>(eGameEvents.UI_ForceUpdateAllTowerCard, new Action<List<TowerIngameData>, int>(this.OnForceUpdateAllTowerCard));
		EventMgr.Register(eGameEvents.ConfirmPlacement, new Action(this.OnConfirmPlacement));
		EventMgr.Register(eGameEvents.CancelPlacement, new Action(this.OnCancelPlacement));
		EventMgr.Register<int>(eGameEvents.OnCoinChanged, new Action<int>(this.OnCoinChanged));
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnButtonDown = (Action)Delegate.Combine(ui_HoldableButton.OnButtonDown, new Action(this.OnButtonDown));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnHoldButton = (Action)Delegate.Combine(ui_HoldableButton2.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonUp = (Action)Delegate.Combine(ui_HoldableButton3.OnButtonUp, new Action(this.OnButtonUp));
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00020E98 File Offset: 0x0001F098
	private void OnDisable()
	{
		EventMgr.Remove<List<TowerIngameData>, int>(eGameEvents.OnTowerCardChanged, new Action<List<TowerIngameData>, int>(this.OnTowerChanged));
		EventMgr.Remove<List<TowerIngameData>, int>(eGameEvents.UI_ForceUpdateAllTowerCard, new Action<List<TowerIngameData>, int>(this.OnForceUpdateAllTowerCard));
		EventMgr.Remove(eGameEvents.ConfirmPlacement, new Action(this.OnConfirmPlacement));
		EventMgr.Remove(eGameEvents.CancelPlacement, new Action(this.OnCancelPlacement));
		EventMgr.Remove<int>(eGameEvents.OnCoinChanged, new Action<int>(this.OnCoinChanged));
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnButtonDown = (Action)Delegate.Remove(ui_HoldableButton.OnButtonDown, new Action(this.OnButtonDown));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnHoldButton = (Action)Delegate.Remove(ui_HoldableButton2.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonUp = (Action)Delegate.Remove(ui_HoldableButton3.OnButtonUp, new Action(this.OnButtonUp));
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00020F92 File Offset: 0x0001F192
	private void Update()
	{
		if (!this.isActive)
		{
			return;
		}
		if (this.canBuild && Input.GetKeyDown(this.keycode))
		{
			EventMgr.SendEvent(eGameEvents.CancelPlacement);
			this.InitiateTowerPlacement();
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x00020FC5 File Offset: 0x0001F1C5
	public void ToggleButton(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00020FD8 File Offset: 0x0001F1D8
	private void OnConfirmPlacement()
	{
		this.animator.SetBool("isSelected", false);
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x00020FEB File Offset: 0x0001F1EB
	private void OnCancelPlacement()
	{
		this.animator.SetBool("isSelected", false);
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00021000 File Offset: 0x0001F200
	private void OnTowerChanged(List<TowerIngameData> list, int index)
	{
		if (this.index != index)
		{
			return;
		}
		if (index >= list.Count)
		{
			this.node_CardFace.gameObject.SetActive(false);
			this.node_Empty.gameObject.SetActive(true);
			this.currentData = null;
			this.currentSettingData = null;
			this.isActive = false;
			return;
		}
		TowerIngameData data = list[index];
		this.UpdateCardContent(data);
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x00021068 File Offset: 0x0001F268
	private void OnForceUpdateAllTowerCard(List<TowerIngameData> list, int coin)
	{
		int towerCardLimit = GameDataManager.instance.GameplayData.TowerCardLimit;
		if (this.index >= towerCardLimit)
		{
			this.node_CardFace.gameObject.SetActive(false);
			this.node_Empty.gameObject.SetActive(false);
			this.node_Locked.gameObject.SetActive(true);
			this.currentData = null;
			this.currentSettingData = null;
			this.isActive = false;
			return;
		}
		if (this.index >= list.Count)
		{
			this.node_CardFace.gameObject.SetActive(false);
			this.node_Empty.gameObject.SetActive(true);
			this.node_Locked.gameObject.SetActive(false);
			this.currentData = null;
			this.currentSettingData = null;
			this.isActive = false;
			return;
		}
		this.node_CardFace.gameObject.SetActive(true);
		this.node_Empty.gameObject.SetActive(false);
		this.node_Locked.gameObject.SetActive(false);
		this.isActive = true;
		Debug.Log(string.Format("更新#{0}卡片內容: {1}", this.index, list[this.index].ItemType));
		this.UpdateCardContent(list[this.index]);
		this.UpdateCoinUI(coin);
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x000211B4 File Offset: 0x0001F3B4
	private void UpdateCardContent(TowerIngameData data)
	{
		this.currentData = data;
		this.currentSettingData = (Singleton<ResourceManager>.Instance.GetItemDataByType(data.ItemType) as TowerSettingData);
		this.image_Icon.sprite = this.currentSettingData.GetCardIcon();
		this.image_Icon.AdjustSizeToSprite();
		this.text_Cost.text = this.currentSettingData.GetBuildCost(1f).ToString();
		if (this.currentSettingData.TowerSizeType != eTowerSizeType._1x1)
		{
			this.node_TowerSize.gameObject.SetActive(true);
			this.text_TowerSize.text = this.currentSettingData.TowerSizeType.GetString();
		}
		else
		{
			this.node_TowerSize.gameObject.SetActive(false);
		}
		this.isActive = true;
		this.button.interactable = this.isActive;
		this.canBuild = this.isActive;
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x00021298 File Offset: 0x0001F498
	private void OnButtonDown()
	{
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x0002129A File Offset: 0x0001F49A
	private void OnHoldButton()
	{
		this.InitiateTowerPlacement();
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x000212A2 File Offset: 0x0001F4A2
	private void OnButtonUp()
	{
		this.InitiateTowerPlacement();
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x000212AA File Offset: 0x0001F4AA
	private void OnCoinChanged(int coin)
	{
		if (!this.isActive)
		{
			return;
		}
		this.UpdateCoinUI(coin);
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x000212BC File Offset: 0x0001F4BC
	private void UpdateCoinUI(int coin)
	{
		if (coin < this.currentSettingData.GetBuildCost(1f))
		{
			this.text_Cost.color = new Color(1f, 0.15f, 0.3f);
			this.button.interactable = false;
			return;
		}
		this.text_Cost.color = Color.white;
		this.button.interactable = true;
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x00021324 File Offset: 0x0001F524
	private void InitiateTowerPlacement()
	{
		EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.EDIT_MODE);
		TowerSettingData data = Singleton<ResourceManager>.Instance.GetItemDataByType(this.currentData.ItemType) as TowerSettingData;
		ABaseTower abaseTower = Singleton<ResourceManager>.Instance.CreateTower(data);
		EventMgr.SendEvent<GameObject, Action>(eGameEvents.RequestStartPlacement, abaseTower.gameObject, new Action(this.OnPlacementComplete));
		this.animator.SetBool("isSelected", true);
		SoundManager.PlaySound("UI", "ClickCard", -1f, -1f, -1f);
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x000213B4 File Offset: 0x0001F5B4
	private void OnPlacementComplete()
	{
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x000213B8 File Offset: 0x0001F5B8
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.currentSettingData == null)
		{
			return;
		}
		string locNameString = this.currentSettingData.GetLocNameString(true);
		string arg = this.currentSettingData.GetLocStatsString() + "\n" + this.currentSettingData.GetLocFlavorTextString();
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
		EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, locNameString, arg);
		EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._2D, base.transform, Vector3.up * 50f);
		if (this.cardMouseOverTweener != null && this.cardMouseOverTweener.IsActive())
		{
			this.cardMouseOverTweener.Complete();
		}
		this.cardMouseOverTweener = base.transform.DOScale(Vector3.one * 1.05f, 0.15f);
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00021490 File Offset: 0x0001F690
	public void OnPointerExit(PointerEventData eventData)
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
		if (this.cardMouseOverTweener != null && this.cardMouseOverTweener.IsActive())
		{
			this.cardMouseOverTweener.Complete();
		}
		this.cardMouseOverTweener = base.transform.DOScale(Vector3.one * 0.9f, 0.15f);
	}

	// Token: 0x040006F1 RID: 1777
	[SerializeField]
	private KeyCode keycode;

	// Token: 0x040006F2 RID: 1778
	[SerializeField]
	private Animator animator;

	// Token: 0x040006F3 RID: 1779
	[SerializeField]
	private int index;

	// Token: 0x040006F4 RID: 1780
	[SerializeField]
	private Image image_Icon;

	// Token: 0x040006F5 RID: 1781
	[SerializeField]
	private TMP_Text text_Cost;

	// Token: 0x040006F6 RID: 1782
	[SerializeField]
	private Transform node_Content;

	// Token: 0x040006F7 RID: 1783
	[SerializeField]
	private Transform node_CardFace;

	// Token: 0x040006F8 RID: 1784
	[SerializeField]
	[Header("沒有鎖住, 但沒有卡片的狀態")]
	private Transform node_Empty;

	// Token: 0x040006F9 RID: 1785
	[SerializeField]
	[Header("鎖住狀態")]
	private Transform node_Locked;

	// Token: 0x040006FA RID: 1786
	[SerializeField]
	private Transform node_TowerSize;

	// Token: 0x040006FB RID: 1787
	[SerializeField]
	private TMP_Text text_TowerSize;

	// Token: 0x040006FC RID: 1788
	[SerializeField]
	private UI_HoldableButton button;

	// Token: 0x040006FD RID: 1789
	private TowerIngameData currentData;

	// Token: 0x040006FE RID: 1790
	private TowerSettingData currentSettingData;

	// Token: 0x040006FF RID: 1791
	private Tweener cardMouseOverTweener;

	// Token: 0x04000700 RID: 1792
	private bool isActive;

	// Token: 0x04000701 RID: 1793
	private bool canBuild;
}
