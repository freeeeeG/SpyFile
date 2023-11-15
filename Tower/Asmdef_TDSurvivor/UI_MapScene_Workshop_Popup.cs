using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class UI_MapScene_Workshop_Popup : APopupWindow
{
	// Token: 0x060009B6 RID: 2486 RVA: 0x00024927 File Offset: 0x00022B27
	private void OnEnable()
	{
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00024929 File Offset: 0x00022B29
	private void OnDisable()
	{
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x0002492B File Offset: 0x00022B2B
	protected override void Start()
	{
		base.Start();
		this.isCardSelected = false;
		this.text_CardName.text = "";
		this.text_CardDescription.text = "";
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x0002495C File Offset: 0x00022B5C
	public void SetupContent(List<DiscoverRewardData> list_Data)
	{
		if (this.list_Cards == null)
		{
			this.list_Cards = new List<UI_Obj_ShopCard>();
		}
		this.ClearContent();
		Debug.Log("======================= list_Data ==========");
		for (int i = 0; i < list_Data.Count; i++)
		{
			Debug.Log(string.Format("{0} = {1}, {2}", i, list_Data[i].DiscoverRewardType, list_Data[i].List_RewardContentType[0]));
		}
		for (int j = 0; j < list_Data.Count; j++)
		{
			DiscoverRewardData data = list_Data[j];
			UI_Obj_ShopCard component = Singleton<PrefabManager>.Instance.InstantiatePrefab("Obj_UI_ShopCard", Vector3.zero, Quaternion.identity, this.node_Cards).GetComponent<UI_Obj_ShopCard>();
			component.SetupContent(data, UI_Obj_ShopCard.eCardSelectType.SELECTABLE, 0);
			UI_Obj_ShopCard ui_Obj_ShopCard = component;
			ui_Obj_ShopCard.OnCardClicked = (Action<UI_Obj_ShopCard>)Delegate.Combine(ui_Obj_ShopCard.OnCardClicked, new Action<UI_Obj_ShopCard>(this.OnCardClickedCallback));
			component.transform.SetParent(this.list_CardAnchors[j]);
			component.transform.ResetLocalTransform();
			component.transform.localPosition = component.transform.localPosition.WithZ(0f);
			component.transform.localScale = Vector3.one * 1f;
			UI_Obj_ShopCard ui_Obj_ShopCard2 = component;
			ui_Obj_ShopCard2.OnCardMouseEnter = (Action<UI_Obj_ShopCard>)Delegate.Combine(ui_Obj_ShopCard2.OnCardMouseEnter, new Action<UI_Obj_ShopCard>(this.OnCardMouseEnterCallback));
			UI_Obj_ShopCard ui_Obj_ShopCard3 = component;
			ui_Obj_ShopCard3.OnCardMouseExit = (Action<UI_Obj_ShopCard>)Delegate.Combine(ui_Obj_ShopCard3.OnCardMouseExit, new Action<UI_Obj_ShopCard>(this.OnCardMouseExitCallback));
			this.list_Cards.Add(component);
		}
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00024AF8 File Offset: 0x00022CF8
	private void OnCardMouseEnterCallback(UI_Obj_ShopCard card)
	{
		if (this.isCardSelected)
		{
			return;
		}
		this.isCardSelected = true;
		string locNameString = card.GetLocNameString();
		string locTooltipString = card.GetLocTooltipString();
		this.text_CardName.text = locNameString;
		this.text_CardDescription.text = locTooltipString;
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x00024B3B File Offset: 0x00022D3B
	private void OnCardMouseExitCallback(UI_Obj_ShopCard card)
	{
		this.text_CardName.text = "";
		this.text_CardDescription.text = "";
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00024B5D File Offset: 0x00022D5D
	private IEnumerator CR_ShowDiscoverProc()
	{
		Debug.Log("CR_ShowDiscoverProc");
		this.Toggle(true);
		yield return new WaitForSeconds(0.5f);
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

	// Token: 0x060009BD RID: 2493 RVA: 0x00024B6C File Offset: 0x00022D6C
	private void OnCardClickedCallback(UI_Obj_ShopCard card)
	{
		for (int i = 0; i < this.list_Cards.Count; i++)
		{
			if (card == this.list_Cards[i])
			{
				EventMgr.SendEvent<eItemType>(eGameEvents.RequestAddCardToStorage, card.ItemType);
			}
			else
			{
				this.list_Cards[i].ToggleCard(false);
			}
		}
		GameDataManager.instance.SaveData();
		base.CloseWindow();
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x00024BDC File Offset: 0x00022DDC
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

	// Token: 0x060009BF RID: 2495 RVA: 0x00024C5C File Offset: 0x00022E5C
	private void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
		if (isOn)
		{
			SoundManager.PlaySound("MapScene", "MapNodePage_Open", -1f, -1f, -1f);
			SoundManager.PlaySound("MapScene", "MapNodePage_Workshop", -1f, -1f, 0.5f);
			return;
		}
		SoundManager.PlaySound("MapScene", "MapNodePage_Close", -1f, -1f, -1f);
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00024CDB File Offset: 0x00022EDB
	protected override void ShowWindowProc()
	{
		base.StartCoroutine(this.CR_ShowDiscoverProc());
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x00024CEA File Offset: 0x00022EEA
	protected override void CloseWindowProc()
	{
		EventMgr.SendEvent(eMapSceneEvents.WorkshopUICompleted);
		this.Toggle(false);
	}

	// Token: 0x04000793 RID: 1939
	[SerializeField]
	[Header("標題文字")]
	private TMP_Text text_Title;

	// Token: 0x04000794 RID: 1940
	[SerializeField]
	[Header("卡片產生的Parent node")]
	private Transform node_Cards;

	// Token: 0x04000795 RID: 1941
	[SerializeField]
	[Header("卡片顯示動畫的interval")]
	private float showCardInterval = 0.15f;

	// Token: 0x04000796 RID: 1942
	[SerializeField]
	private List<Transform> list_CardAnchors;

	// Token: 0x04000797 RID: 1943
	[SerializeField]
	[Header("已產生的卡片")]
	private List<UI_Obj_ShopCard> list_Cards;

	// Token: 0x04000798 RID: 1944
	[SerializeField]
	private TMP_Text text_CardName;

	// Token: 0x04000799 RID: 1945
	[SerializeField]
	private TMP_Text text_CardDescription;

	// Token: 0x0400079A RID: 1946
	private bool isCardSelected;
}
