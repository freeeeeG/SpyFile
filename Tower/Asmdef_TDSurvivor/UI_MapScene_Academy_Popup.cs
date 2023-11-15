using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class UI_MapScene_Academy_Popup : APopupWindow
{
	// Token: 0x06000984 RID: 2436 RVA: 0x00023D48 File Offset: 0x00021F48
	private void OnEnable()
	{
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x00023D4A File Offset: 0x00021F4A
	private void OnDisable()
	{
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x00023D4C File Offset: 0x00021F4C
	protected override void Start()
	{
		base.Start();
		this.isSelected = false;
		this.isCardSelected = false;
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00023D64 File Offset: 0x00021F64
	public void SetupContent(List<DiscoverRewardData> list_Data_Set1, List<DiscoverRewardData> list_Data_Set2, List<DiscoverRewardData> list_Data_Set3)
	{
		if (this.list_CreatedCards == null)
		{
			this.list_CreatedCards = new List<UI_Obj_ShopCard>();
		}
		this.ClearContent();
		if (list_Data_Set1.Count != 10)
		{
			Debug.LogError("傳進來的資料數量不對! (Set 1)");
		}
		if (list_Data_Set2.Count != 10)
		{
			Debug.LogError("傳進來的資料數量不對! (Set 2)");
		}
		if (list_Data_Set3.Count != 10)
		{
			Debug.LogError("傳進來的資料數量不對! (Set 3)");
		}
		this.SetupDataset(0, list_Data_Set1, this.list_CardSet[0]);
		this.SetupDataset(1, list_Data_Set2, this.list_CardSet[1]);
		this.SetupDataset(2, list_Data_Set3, this.list_CardSet[2]);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00023E04 File Offset: 0x00022004
	private void SetupDataset(int index, List<DiscoverRewardData> dataset, UI_Obj_AcademyCardSet targetCardset)
	{
		List<UI_Obj_ShopCard> list = new List<UI_Obj_ShopCard>();
		for (int i = 0; i < dataset.Count; i++)
		{
			DiscoverRewardData data = dataset[i];
			UI_Obj_ShopCard component = Singleton<PrefabManager>.Instance.InstantiatePrefab("Obj_UI_ShopCard", Vector3.zero, Quaternion.identity, this.node_Cards).GetComponent<UI_Obj_ShopCard>();
			component.SetupContent(data, UI_Obj_ShopCard.eCardSelectType.SELECTABLE, 0);
			component.ToggleClickable(true);
			component.transform.ResetLocalTransform();
			component.transform.localPosition = component.transform.localPosition.WithZ(0f);
			component.transform.localScale = ((i < 3) ? (Vector3.one * 0.9f) : (Vector3.one * 0.6f));
			component.ToggleCard(true);
			UI_Obj_ShopCard ui_Obj_ShopCard = component;
			ui_Obj_ShopCard.OnCardMouseEnter = (Action<UI_Obj_ShopCard>)Delegate.Combine(ui_Obj_ShopCard.OnCardMouseEnter, new Action<UI_Obj_ShopCard>(this.OnCardMouseEnterCallback));
			UI_Obj_ShopCard ui_Obj_ShopCard2 = component;
			ui_Obj_ShopCard2.OnCardMouseExit = (Action<UI_Obj_ShopCard>)Delegate.Combine(ui_Obj_ShopCard2.OnCardMouseExit, new Action<UI_Obj_ShopCard>(this.OnCardMouseExitCallback));
			this.list_CreatedCards.Add(component);
			list.Add(component);
		}
		targetCardset.Setup(index, new Action<int>(this.OnCardClickedCallback));
		for (int j = 0; j < 3; j++)
		{
			targetCardset.SetTowerCardToNode(list[j], j);
		}
		for (int k = 0; k < 7; k++)
		{
			targetCardset.SetTetrisCardToNode(list[k + 3], k);
		}
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00023F78 File Offset: 0x00022178
	private void OnCardClickedCallback(int index)
	{
		Debug.Log(string.Format("選擇Cardset: #{0}", index));
		if (this.isSelected)
		{
			return;
		}
		this.isSelected = true;
		SoundManager.PlaySound("MapScene", "MapNodePage_Academy_SelectCardSet", -1f, -1f, -1f);
		base.StartCoroutine(this.CR_CardClickedProc(index));
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00023FD7 File Offset: 0x000221D7
	private IEnumerator CR_CardClickedProc(int index)
	{
		for (int i = 0; i < this.list_CardSet.Count; i++)
		{
			if (index == i)
			{
				this.list_CardSet[i].PlaySelectedAnim();
			}
			else
			{
				this.list_CardSet[i].Toggle(false);
			}
		}
		for (int j = 0; j < 3; j++)
		{
			int index2 = 10 * index + j;
			EventMgr.SendEvent<TowerIngameData>(eGameEvents.RequestAddTowerCard, new TowerIngameData(this.list_CreatedCards[index2].ItemType, 1));
		}
		for (int k = 0; k < 7; k++)
		{
			int index3 = 10 * index + 3 + k;
			EventMgr.SendEvent<eItemType>(eGameEvents.RequestAddCardToStorage, this.list_CreatedCards[index3].ItemType);
		}
		GameDataManager.instance.SaveData();
		yield return new WaitForSeconds(1f);
		base.CloseWindow();
		yield break;
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00023FED File Offset: 0x000221ED
	private void OnCardMouseEnterCallback(UI_Obj_ShopCard card)
	{
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00023FEF File Offset: 0x000221EF
	private void OnCardMouseExitCallback(UI_Obj_ShopCard card)
	{
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00023FF1 File Offset: 0x000221F1
	private IEnumerator CR_ShowWindowProc()
	{
		this.Toggle(true);
		yield return new WaitForSeconds(0.8f);
		int num;
		for (int i = 0; i < this.list_CardSet.Count; i = num + 1)
		{
			this.list_CardSet[i].Toggle(true);
			yield return new WaitForSeconds(0.15f);
			num = i;
		}
		yield break;
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x00024000 File Offset: 0x00022200
	private void ClearContent()
	{
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x00024004 File Offset: 0x00022204
	private void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
		if (isOn)
		{
			SoundManager.PlaySound("MapScene", "MapNodePage_Open", -1f, -1f, -1f);
			SoundManager.PlaySound("MapScene", "MapNodePage_Academy", -1f, -1f, 0.5f);
			return;
		}
		SoundManager.PlaySound("MapScene", "MapNodePage_Close", -1f, -1f, -1f);
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00024083 File Offset: 0x00022283
	protected override void ShowWindowProc()
	{
		base.StartCoroutine(this.CR_ShowWindowProc());
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x00024092 File Offset: 0x00022292
	protected override void CloseWindowProc()
	{
		EventMgr.SendEvent(eMapSceneEvents.WorkshopUICompleted);
		this.Toggle(false);
	}

	// Token: 0x0400077B RID: 1915
	[SerializeField]
	[Header("標題文字")]
	private TMP_Text text_Title;

	// Token: 0x0400077C RID: 1916
	[SerializeField]
	[Header("卡片產生的Parent node")]
	private Transform node_Cards;

	// Token: 0x0400077D RID: 1917
	[SerializeField]
	[Header("卡片顯示動畫的interval")]
	private float showCardInterval = 0.15f;

	// Token: 0x0400077E RID: 1918
	[SerializeField]
	private List<UI_Obj_AcademyCardSet> list_CardSet;

	// Token: 0x0400077F RID: 1919
	[SerializeField]
	[Header("已產生的卡片")]
	private List<UI_Obj_ShopCard> list_CreatedCards;

	// Token: 0x04000780 RID: 1920
	private bool isSelected;

	// Token: 0x04000781 RID: 1921
	private bool isCardSelected;
}
