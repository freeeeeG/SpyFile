using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000180 RID: 384
public class UI_PlayerDeck : MonoBehaviour
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x00025F90 File Offset: 0x00024190
	private void OnEnable()
	{
		EventMgr.Register<List<CardData>>(eGameEvents.OnDeckChanged, new Action<List<CardData>>(this.OnDeckChanged));
		EventMgr.Register<int>(eGameEvents.OnCoinChanged, new Action<int>(this.OnCoinChanged));
		EventMgr.Register<int>(eGameEvents.OnDrawCardCostChanged, new Action<int>(this.OnDrawCardCostChanged));
		EventMgr.Register<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Register(eGameEvents.OnPlayerVictory, new Action(this.OnPlayerVictory));
		EventMgr.Register<List<CardData>>(eGameEvents.OnHandCardChanged, new Action<List<CardData>>(this.OnHandCardChanged));
		this.button_DrawCard.onClick.AddListener(new UnityAction(this.OnClickButton_DrawCard));
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0002604C File Offset: 0x0002424C
	private void OnDisable()
	{
		EventMgr.Remove<List<CardData>>(eGameEvents.OnDeckChanged, new Action<List<CardData>>(this.OnDeckChanged));
		EventMgr.Remove<int>(eGameEvents.OnCoinChanged, new Action<int>(this.OnCoinChanged));
		EventMgr.Remove<int>(eGameEvents.OnDrawCardCostChanged, new Action<int>(this.OnDrawCardCostChanged));
		EventMgr.Remove<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Remove(eGameEvents.OnPlayerVictory, new Action(this.OnPlayerVictory));
		EventMgr.Remove<List<CardData>>(eGameEvents.OnHandCardChanged, new Action<List<CardData>>(this.OnHandCardChanged));
		this.button_DrawCard.onClick.RemoveListener(new UnityAction(this.OnClickButton_DrawCard));
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x00026108 File Offset: 0x00024308
	private void OnHandCardChanged(List<CardData> list)
	{
		if (MainGame.Instance.IngameData.IsHandCardFull())
		{
			this.button_DrawCard.interactable = false;
			this.node_DrawCardCost.SetActive(false);
			return;
		}
		this.button_DrawCard.interactable = true;
		this.node_DrawCardCost.SetActive(true);
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00026157 File Offset: 0x00024357
	private void OnRoundStart(int index, int totalRound)
	{
		if (index == 1)
		{
			this.animator.SetBool("isOn", true);
		}
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0002616E File Offset: 0x0002436E
	private void OnPlayerVictory()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x00026184 File Offset: 0x00024384
	private void OnDeckChanged(List<CardData> list)
	{
		this.text_DeckCardCount.text = list.Count.ToString();
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x000261AA File Offset: 0x000243AA
	private void OnDrawCardCostChanged(int cost)
	{
		this.curDrawCardCost = cost;
		this.text_DrawCardCost.text = cost.ToString();
		this.UpdateUI();
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x000261CB File Offset: 0x000243CB
	private void UpdateUI()
	{
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x000261D0 File Offset: 0x000243D0
	private void OnCoinChanged(int coin)
	{
		bool flag = coin > this.curDrawCardCost;
		this.text_DrawCardCost.color = (flag ? Color.white : Color.red);
		this.button_DrawCard.interactable = flag;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.node_DrawCardCost.transform as RectTransform);
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x00026224 File Offset: 0x00024424
	private void OnClickButton_DrawCard()
	{
		if (MainGame.Instance.IngameData.Coin < this.curDrawCardCost)
		{
			return;
		}
		EventMgr.SendEvent(eGameEvents.CancelPlacement);
		EventMgr.SendEvent<int, bool>(eGameEvents.RequestDrawCard, 1, true);
		EventMgr.SendEvent<int>(eGameEvents.RequestAddCoin, -1 * this.curDrawCardCost);
		this.animator.SetTrigger("Click");
		SoundManager.PlaySound("UI", "DrawCard", -1f, -1f, -1f);
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x000262A9 File Offset: 0x000244A9
	public Vector3 GetDrawCardInitialPosition()
	{
		return this.node_DrawCardInitialPosition.position;
	}

	// Token: 0x040007DA RID: 2010
	[SerializeField]
	private Animator animator;

	// Token: 0x040007DB RID: 2011
	[SerializeField]
	private TMP_Text text_DeckCardCount;

	// Token: 0x040007DC RID: 2012
	[SerializeField]
	private TMP_Text text_DrawCardCost;

	// Token: 0x040007DD RID: 2013
	[SerializeField]
	private Button button_DrawCard;

	// Token: 0x040007DE RID: 2014
	[SerializeField]
	private GameObject node_DrawCardCost;

	// Token: 0x040007DF RID: 2015
	[SerializeField]
	private Transform node_DrawCardInitialPosition;

	// Token: 0x040007E0 RID: 2016
	private int curDrawCardCost;
}
