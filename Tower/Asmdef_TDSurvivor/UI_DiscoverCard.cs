using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000160 RID: 352
public class UI_DiscoverCard : AUISituational
{
	// Token: 0x06000939 RID: 2361 RVA: 0x00022F58 File Offset: 0x00021158
	private void OnEnable()
	{
		EventMgr.Register<List<DiscoverRewardData>>(eGameEvents.UI_ShowDiscoverReward, new Action<List<DiscoverRewardData>>(this.OnShowDiscoverReward));
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00022F72 File Offset: 0x00021172
	private void OnDisable()
	{
		EventMgr.Remove<List<DiscoverRewardData>>(eGameEvents.UI_ShowDiscoverReward, new Action<List<DiscoverRewardData>>(this.OnShowDiscoverReward));
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00022F8C File Offset: 0x0002118C
	private void OnShowDiscoverReward(List<DiscoverRewardData> list_Reward)
	{
		this.SetupContent(list_Reward);
		base.StartCoroutine(this.CR_ShowDiscoverProc());
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00022FA4 File Offset: 0x000211A4
	public void SetupContent(List<DiscoverRewardData> list_Data)
	{
		if (this.list_DiscoverCards == null)
		{
			this.list_DiscoverCards = new List<UI_Obj_DiscoverCard>();
		}
		this.ClearContent();
		this.text_Title.text = LocalizationManager.Instance.GetString("UI", "DISCOVER_CARD_TITLE", Array.Empty<object>());
		for (int i = 0; i < list_Data.Count; i++)
		{
			DiscoverRewardData data = list_Data[i];
			UI_Obj_DiscoverCard component = Singleton<PrefabManager>.Instance.InstantiatePrefab("Obj_UI_DiscoverCard", Vector3.zero, Quaternion.identity, this.node_Cards).GetComponent<UI_Obj_DiscoverCard>();
			component.SetupContent(data);
			UI_Obj_DiscoverCard ui_Obj_DiscoverCard = component;
			ui_Obj_DiscoverCard.OnCardClicked = (Action<UI_Obj_DiscoverCard>)Delegate.Combine(ui_Obj_DiscoverCard.OnCardClicked, new Action<UI_Obj_DiscoverCard>(this.OnCardClickedCallback));
			component.transform.localPosition = component.transform.localPosition.WithZ(0f);
			this.list_DiscoverCards.Add(component);
		}
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00023087 File Offset: 0x00021287
	private IEnumerator CR_ShowDiscoverProc()
	{
		base.Toggle(true);
		yield return new WaitForSeconds(0.1f);
		int num;
		for (int i = 0; i < this.list_DiscoverCards.Count; i = num + 1)
		{
			this.list_DiscoverCards[i].ToggleCard(true);
			SoundManager.PlaySound("UI", "DiscoverCard_ShowCard", -1f, -1f, -1f);
			yield return new WaitForSeconds(this.showCardInterval);
			num = i;
		}
		yield break;
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x00023096 File Offset: 0x00021296
	private void OnCardClickedCallback(UI_Obj_DiscoverCard card)
	{
		base.StartCoroutine(this.CR_GiveCardAnim(card));
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x000230A6 File Offset: 0x000212A6
	private IEnumerator CR_GiveCardAnim(UI_Obj_DiscoverCard card)
	{
		for (int i = 0; i < this.list_DiscoverCards.Count; i++)
		{
			if (card != this.list_DiscoverCards[i])
			{
				this.list_DiscoverCards[i].ToggleCard(false);
			}
		}
		EventMgr.SendEvent<DiscoverRewardData, Vector3>(eGameEvents.OnDiscoverRewardSelected, card.Data, card.transform.position);
		yield return null;
		this.CloseUI(0f);
		yield break;
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x000230BC File Offset: 0x000212BC
	private void ClearContent()
	{
		for (int i = this.list_DiscoverCards.Count - 1; i >= 0; i--)
		{
			UI_Obj_DiscoverCard ui_Obj_DiscoverCard = this.list_DiscoverCards[i];
			ui_Obj_DiscoverCard.OnCardClicked = (Action<UI_Obj_DiscoverCard>)Delegate.Remove(ui_Obj_DiscoverCard.OnCardClicked, new Action<UI_Obj_DiscoverCard>(this.OnCardClickedCallback));
			Singleton<PrefabManager>.Instance.DespawnPrefab(this.list_DiscoverCards[i].gameObject, 0f);
		}
		this.list_DiscoverCards.Clear();
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00023139 File Offset: 0x00021339
	private void CloseUI(float delay)
	{
		base.StartCoroutine(this.CR_CloseUI(delay));
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x00023149 File Offset: 0x00021349
	private IEnumerator CR_CloseUI(float delay)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		EventMgr.SendEvent(eGameEvents.OnDiscoverRewardComplete);
		base.Toggle(false);
		yield break;
	}

	// Token: 0x0400074F RID: 1871
	[SerializeField]
	[Header("ItemSlot的連結, 飛卡片效果用")]
	private UI_ItemSlot ui_ItemSlot;

	// Token: 0x04000750 RID: 1872
	[SerializeField]
	[Header("標題文字")]
	private TMP_Text text_Title;

	// Token: 0x04000751 RID: 1873
	[SerializeField]
	[Header("卡片產生的Parent node")]
	private Transform node_Cards;

	// Token: 0x04000752 RID: 1874
	[SerializeField]
	[Header("卡片顯示動畫的interval")]
	private float showCardInterval = 0.15f;

	// Token: 0x04000753 RID: 1875
	[SerializeField]
	[Header("已產生的卡片")]
	private List<UI_Obj_DiscoverCard> list_DiscoverCards;
}
