using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016F RID: 367
public class UI_MapScene_Shop_Popup : APopupWindow
{
	// Token: 0x060009A9 RID: 2473 RVA: 0x000244BE File Offset: 0x000226BE
	private void OnEnable()
	{
		this.button_Leave.onClick.AddListener(new UnityAction(this.OnClickButton_Leave));
		EventMgr.Register<int>(eGameEvents.OnGemChanged, new Action<int>(this.OnGemChanged));
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x000244F4 File Offset: 0x000226F4
	private void OnDisable()
	{
		this.button_Leave.onClick.RemoveListener(new UnityAction(this.OnClickButton_Leave));
		EventMgr.Remove<int>(eGameEvents.OnGemChanged, new Action<int>(this.OnGemChanged));
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0002452C File Offset: 0x0002272C
	private void OnGemChanged(int value)
	{
		for (int i = 0; i < this.list_Cards.Count; i++)
		{
			this.list_Cards[i].UpdatePrize(GameDataManager.instance.GameplayData.Gem);
		}
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0002456F File Offset: 0x0002276F
	private void OnClickButton_Leave()
	{
		base.CloseWindow();
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00024578 File Offset: 0x00022778
	public void SetupContent(List<CardData> list_Data)
	{
		if (this.list_Cards == null)
		{
			this.list_Cards = new List<UI_Obj_ShopCard>();
		}
		this.ClearContent();
		for (int i = 0; i < list_Data.Count; i++)
		{
			CardData cardData = list_Data[i];
			UI_Obj_ShopCard component = Singleton<PrefabManager>.Instance.InstantiatePrefab("Obj_UI_ShopCard", Vector3.zero, Quaternion.identity, this.list_CardNodes[i]).GetComponent<UI_Obj_ShopCard>();
			int cost = this.GetCost(cardData.data);
			component.SetupContent(cardData.ItemType, UI_Obj_ShopCard.eCardSelectType.BUYABLE, cost);
			UI_Obj_ShopCard ui_Obj_ShopCard = component;
			ui_Obj_ShopCard.OnCardClicked = (Action<UI_Obj_ShopCard>)Delegate.Combine(ui_Obj_ShopCard.OnCardClicked, new Action<UI_Obj_ShopCard>(this.OnCardClickedCallback));
			component.transform.localPosition = Vector3.zero;
			component.transform.localScale = Vector3.one * 1f;
			component.UpdatePrize(GameDataManager.instance.GameplayData.Gem);
			this.list_Cards.Add(component);
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00024670 File Offset: 0x00022870
	private int GetCost(AItemSettingData data)
	{
		int num = data.GetStoreCost();
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.STORE_DISCOUNT))
		{
			num = Mathf.RoundToInt((float)num * 0.8f);
		}
		return num;
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x000246A6 File Offset: 0x000228A6
	private IEnumerator CR_ShowShopProc()
	{
		Debug.Log("CR_ShowShopProc");
		this.Toggle(true);
		yield return new WaitForSeconds(0.85f);
		int num;
		for (int i = 0; i < this.list_Cards.Count; i = num + 1)
		{
			this.list_Cards[i].ToggleCard(true);
			SoundManager.PlaySound("UI", "DiscoverCard_ShowCard", -1f, -1f, -1f);
			yield return new WaitForSeconds(this.showCardInterval);
			num = i;
		}
		yield break;
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x000246B8 File Offset: 0x000228B8
	private void OnCardClickedCallback(UI_Obj_ShopCard card)
	{
		Debug.Log(string.Format("點擊卡片: {0}", card.gameObject));
		int cost = this.GetCost(card.Data);
		if (GameDataManager.instance.GameplayData.Gem >= cost)
		{
			EventMgr.SendEvent<int>(eGameEvents.RequestAddGem, -1 * cost);
			Debug.Log(string.Format("購買卡片: {0} (類型{1})", card.Data.GetItemType(), card.Data.GetItemType().ToCardType()));
			card.PlayPurchaseAnimation();
			eCardType eCardType = card.Data.GetItemType().ToCardType();
			if (eCardType - eCardType.PANEL_CARD > 1)
			{
				if (eCardType == eCardType.TOWER_CARD)
				{
					TowerIngameData arg = new TowerIngameData(card.Data.GetItemType(), 1);
					EventMgr.SendEvent<TowerIngameData>(eGameEvents.RequestAddTowerCard, arg);
				}
			}
			else
			{
				EventMgr.SendEvent<eItemType>(eGameEvents.RequestAddCardToStorage, card.Data.GetItemType());
			}
			SoundManager.PlaySound("UI", "Shop_Purchase_Success", -1f, -1f, -1f);
			GameDataManager.instance.SaveData();
			return;
		}
		SoundManager.PlaySound("UI", "Shop_Purchase_Fail", -1f, -1f, -1f);
		Debug.Log("鑽石不足");
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x000247F0 File Offset: 0x000229F0
	private void ClearContent()
	{
		for (int i = this.list_Cards.Count - 1; i >= 0; i--)
		{
			UI_Obj_ShopCard ui_Obj_ShopCard = this.list_Cards[i];
			ui_Obj_ShopCard.OnCardClicked = (Action<UI_Obj_ShopCard>)Delegate.Remove(ui_Obj_ShopCard.OnCardClicked, new Action<UI_Obj_ShopCard>(this.OnCardClickedCallback));
			Singleton<PrefabManager>.Instance.DespawnPrefab(this.list_Cards[i].gameObject, 0f);
		}
		this.list_Cards.Clear();
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x00024870 File Offset: 0x00022A70
	private void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
		if (isOn)
		{
			SoundManager.PlaySound("MapScene", "MapNodePage_Open", -1f, -1f, -1f);
			SoundManager.PlaySound("MapScene", "MapNodePage_Shop", -1f, -1f, 0.5f);
			return;
		}
		SoundManager.PlaySound("MapScene", "MapNodePage_Close", -1f, -1f, -1f);
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x000248EF File Offset: 0x00022AEF
	protected override void ShowWindowProc()
	{
		base.StartCoroutine(this.CR_ShowShopProc());
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x000248FE File Offset: 0x00022AFE
	protected override void CloseWindowProc()
	{
		EventMgr.SendEvent(eMapSceneEvents.ShopUICompleted);
		this.Toggle(false);
	}

	// Token: 0x0400078E RID: 1934
	[SerializeField]
	[Header("卡片產生的Parent node")]
	private Transform node_Cards;

	// Token: 0x0400078F RID: 1935
	[SerializeField]
	[Header("卡片顯示動畫的interval")]
	private float showCardInterval = 0.15f;

	// Token: 0x04000790 RID: 1936
	[SerializeField]
	private List<Transform> list_CardNodes;

	// Token: 0x04000791 RID: 1937
	[SerializeField]
	[Header("已產生的卡片")]
	private List<UI_Obj_ShopCard> list_Cards;

	// Token: 0x04000792 RID: 1938
	[SerializeField]
	private Button button_Leave;
}
