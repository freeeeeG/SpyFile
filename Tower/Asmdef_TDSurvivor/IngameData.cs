using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005D RID: 93
[Serializable]
public class IngameData
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x0600022F RID: 559 RVA: 0x00009780 File Offset: 0x00007980
	// (set) Token: 0x06000230 RID: 560 RVA: 0x00009788 File Offset: 0x00007988
	public int Coin { get; private set; }

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000231 RID: 561 RVA: 0x00009791 File Offset: 0x00007991
	// (set) Token: 0x06000232 RID: 562 RVA: 0x00009799 File Offset: 0x00007999
	public int Energy { get; private set; }

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000233 RID: 563 RVA: 0x000097A2 File Offset: 0x000079A2
	// (set) Token: 0x06000234 RID: 564 RVA: 0x000097AA File Offset: 0x000079AA
	public int HP { get; private set; }

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000235 RID: 565 RVA: 0x000097B3 File Offset: 0x000079B3
	// (set) Token: 0x06000236 RID: 566 RVA: 0x000097BB File Offset: 0x000079BB
	public int DrawCardCost { get; private set; }

	// Token: 0x06000237 RID: 567 RVA: 0x000097C4 File Offset: 0x000079C4
	public IngameData(int coin, int hp)
	{
		this.Initialize(coin, hp);
	}

	// Token: 0x06000238 RID: 568 RVA: 0x000097E4 File Offset: 0x000079E4
	public void Initialize(int coin, int hp)
	{
		EventMgr.Register<int>(eGameEvents.RequestAddCoin, new Action<int>(this.OnRequestAddCoin));
		EventMgr.Register<int>(eGameEvents.RequestSetCoin, new Action<int>(this.OnRequestSetCoin));
		EventMgr.Register<int>(eGameEvents.RequestAddHP, new Action<int>(this.OnRequestAddHP));
		EventMgr.Register<int>(eGameEvents.RequestSetHP, new Action<int>(this.OnRequestSetHP));
		EventMgr.Register<eItemType>(eGameEvents.RequestAddCardToDeck, new Action<eItemType>(this.OnRequestAddCardToDeck));
		EventMgr.Register<CardData>(eGameEvents.RequestRemoveCardFromDeck, new Action<CardData>(this.OnRequestRemoveCardFromDeck));
		EventMgr.Register<eItemType>(eGameEvents.RequestAddCardToHand, new Action<eItemType>(this.OnRequestAddCardToHand));
		EventMgr.Register<eItemType, Vector3>(eGameEvents.RequestAddCardToHandFromPosition, new Action<eItemType, Vector3>(this.OnRequestAddCardToHandFromPosition));
		EventMgr.Register<CardData>(eGameEvents.RequestRemoveCardFromHand, new Action<CardData>(this.OnRequestRemoveCardFromHand));
		EventMgr.Register<int>(eGameEvents.RequestRemoveExcessHandCard, new Action<int>(this.OnRequestRemoveExcessHandCard));
		EventMgr.Register<CardData, CardData, int>(eGameEvents.RequestCombineCard, new Action<CardData, CardData, int>(this.OnRequestCombineCard));
		EventMgr.Register(eGameEvents.RequestRedrawCards, new Action(this.OnRequestRedrawCards));
		EventMgr.Register(eGameEvents.RequestDiscardAllCardsFromHand, new Action(this.OnRequestDiscardAllCardsFromHand));
		EventMgr.Register(eGameEvents.RequestResetDrawCardCost, new Action(this.OnRequestResetDrawCardCost));
		EventMgr.Register<int, bool>(eGameEvents.RequestDrawCard, new Action<int, bool>(this.OnRequestDrawCard));
		EventMgr.Register(eGameEvents.RequestShuffleDeck, new Action(this.OnRequestShuffleDeck));
		EventMgr.Register<ABaseTower>(eGameEvents.RequestSellTower, new Action<ABaseTower>(this.OnRequestSellTower));
		EventMgr.Register<ABaseTower>(eGameEvents.RequestUpgradeTower, new Action<ABaseTower>(this.OnRequestUpgradeTower));
		this.list_DeckData = new List<CardData>();
		this.list_DiscardData = new List<CardData>();
		this.list_HandCardData = new List<CardData>();
		foreach (CardData item in GameDataManager.instance.GameplayData.list_ItemStorage)
		{
			this.list_DeckData.Add(item);
		}
		this.SetCoin(coin);
		this.SetHP(hp);
		Debug.Log("IngameData Created");
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00009A30 File Offset: 0x00007C30
	public void ClearEvents()
	{
		EventMgr.Remove<int>(eGameEvents.RequestAddCoin, new Action<int>(this.OnRequestAddCoin));
		EventMgr.Remove<int>(eGameEvents.RequestSetCoin, new Action<int>(this.OnRequestSetCoin));
		EventMgr.Remove<int>(eGameEvents.RequestAddHP, new Action<int>(this.OnRequestAddHP));
		EventMgr.Remove<int>(eGameEvents.RequestSetHP, new Action<int>(this.OnRequestSetHP));
		EventMgr.Remove<eItemType>(eGameEvents.RequestAddCardToDeck, new Action<eItemType>(this.OnRequestAddCardToDeck));
		EventMgr.Remove<CardData>(eGameEvents.RequestRemoveCardFromDeck, new Action<CardData>(this.OnRequestRemoveCardFromDeck));
		EventMgr.Remove<eItemType>(eGameEvents.RequestAddCardToHand, new Action<eItemType>(this.OnRequestAddCardToHand));
		EventMgr.Remove<eItemType, Vector3>(eGameEvents.RequestAddCardToHandFromPosition, new Action<eItemType, Vector3>(this.OnRequestAddCardToHandFromPosition));
		EventMgr.Remove<CardData>(eGameEvents.RequestRemoveCardFromHand, new Action<CardData>(this.OnRequestRemoveCardFromHand));
		EventMgr.Remove<int>(eGameEvents.RequestRemoveExcessHandCard, new Action<int>(this.OnRequestRemoveExcessHandCard));
		EventMgr.Remove<CardData, CardData, int>(eGameEvents.RequestCombineCard, new Action<CardData, CardData, int>(this.OnRequestCombineCard));
		EventMgr.Remove(eGameEvents.RequestRedrawCards, new Action(this.OnRequestRedrawCards));
		EventMgr.Remove(eGameEvents.RequestDiscardAllCardsFromHand, new Action(this.OnRequestDiscardAllCardsFromHand));
		EventMgr.Remove(eGameEvents.RequestResetDrawCardCost, new Action(this.OnRequestResetDrawCardCost));
		EventMgr.Remove<int, bool>(eGameEvents.RequestDrawCard, new Action<int, bool>(this.OnRequestDrawCard));
		EventMgr.Remove(eGameEvents.RequestShuffleDeck, new Action(this.OnRequestShuffleDeck));
		EventMgr.Remove<ABaseTower>(eGameEvents.RequestSellTower, new Action<ABaseTower>(this.OnRequestSellTower));
		EventMgr.Remove<ABaseTower>(eGameEvents.RequestUpgradeTower, new Action<ABaseTower>(this.OnRequestUpgradeTower));
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00009BF0 File Offset: 0x00007DF0
	~IngameData()
	{
	}

	// Token: 0x0600023B RID: 571 RVA: 0x00009C18 File Offset: 0x00007E18
	private void OnRequestAddCoin(int value)
	{
		this.AddCoin(value);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00009C21 File Offset: 0x00007E21
	private void OnRequestSetCoin(int value)
	{
		this.SetCoin(value);
	}

	// Token: 0x0600023D RID: 573 RVA: 0x00009C2A File Offset: 0x00007E2A
	private void AddCoin(int value)
	{
		this.Coin = Mathf.Max(0, this.Coin + value);
		EventMgr.SendEvent<int>(eGameEvents.OnCoinChanged, this.Coin);
	}

	// Token: 0x0600023E RID: 574 RVA: 0x00009C53 File Offset: 0x00007E53
	private void SetCoin(int value)
	{
		this.Coin = value;
		EventMgr.SendEvent<int>(eGameEvents.OnCoinChanged, this.Coin);
	}

	// Token: 0x0600023F RID: 575 RVA: 0x00009C6F File Offset: 0x00007E6F
	public bool IsCoinEnough(int value)
	{
		return this.Coin >= value;
	}

	// Token: 0x06000240 RID: 576 RVA: 0x00009C7D File Offset: 0x00007E7D
	private void OnRequestAddHP(int value)
	{
		this.AddHP(value);
	}

	// Token: 0x06000241 RID: 577 RVA: 0x00009C86 File Offset: 0x00007E86
	private void OnRequestSetHP(int value)
	{
		this.SetHP(value);
	}

	// Token: 0x06000242 RID: 578 RVA: 0x00009C8F File Offset: 0x00007E8F
	private void AddHP(int value)
	{
		this.HP = Mathf.Max(0, this.HP + value);
		EventMgr.SendEvent<int>(eGameEvents.OnHpChanged, this.HP);
	}

	// Token: 0x06000243 RID: 579 RVA: 0x00009CB8 File Offset: 0x00007EB8
	private void SetHP(int value)
	{
		this.HP = value;
		EventMgr.SendEvent<int>(eGameEvents.OnHpChanged, this.HP);
	}

	// Token: 0x06000244 RID: 580 RVA: 0x00009CD4 File Offset: 0x00007ED4
	public bool IsPlayerAlive()
	{
		return this.HP > 0;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x00009CDF File Offset: 0x00007EDF
	public int GetHandCardCount()
	{
		return this.list_HandCardData.Count;
	}

	// Token: 0x06000246 RID: 582 RVA: 0x00009CEC File Offset: 0x00007EEC
	public int GetHandCardSpace()
	{
		if (this.IsHandCardFull())
		{
			return 0;
		}
		return GameDataManager.instance.GameplayData.ItemCardLimit - this.list_HandCardData.Count;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x00009D13 File Offset: 0x00007F13
	public bool IsHandCardFull()
	{
		return this.list_HandCardData.Count >= GameDataManager.instance.GameplayData.ItemCardLimit;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00009D34 File Offset: 0x00007F34
	private void OnRequestAddCardToDeck(eItemType towerType)
	{
		this.AddCardToDeck(towerType);
	}

	// Token: 0x06000249 RID: 585 RVA: 0x00009D3D File Offset: 0x00007F3D
	private void OnRequestRemoveCardFromDeck(CardData data)
	{
		if (!this.list_DeckData.Contains(data))
		{
			return;
		}
		this.list_DeckData.Remove(data);
		EventMgr.SendEvent<List<CardData>>(eGameEvents.OnDeckChanged, this.list_DeckData);
	}

	// Token: 0x0600024A RID: 586 RVA: 0x00009D6E File Offset: 0x00007F6E
	private void OnRequestAddCardToHand(eItemType towerType)
	{
		if (this.IsHandCardFull())
		{
			return;
		}
		this.AddCardToPlayerHand(towerType);
	}

	// Token: 0x0600024B RID: 587 RVA: 0x00009D80 File Offset: 0x00007F80
	private void OnRequestAddCardToHandFromPosition(eItemType towerType, Vector3 position)
	{
		if (this.IsHandCardFull())
		{
			return;
		}
		this.AddCardToPlayerHandFromPosition(towerType, position);
	}

	// Token: 0x0600024C RID: 588 RVA: 0x00009D93 File Offset: 0x00007F93
	private void OnRequestRemoveCardFromHand(CardData data)
	{
		if (!this.list_HandCardData.Contains(data))
		{
			return;
		}
		this.list_HandCardData.Remove(data);
		this.list_DiscardData.Add(data);
		EventMgr.SendEvent<List<CardData>>(eGameEvents.OnHandCardChanged, this.list_HandCardData);
	}

	// Token: 0x0600024D RID: 589 RVA: 0x00009DD0 File Offset: 0x00007FD0
	private void OnRequestRemoveExcessHandCard(int limit)
	{
		if (this.list_HandCardData.Count <= limit)
		{
			return;
		}
		for (int i = this.list_HandCardData.Count - 1; i >= limit; i--)
		{
			CardData item = this.list_HandCardData[i];
			this.list_DiscardData.Add(item);
			this.list_HandCardData.RemoveAt(i);
		}
		EventMgr.SendEvent<List<CardData>>(eGameEvents.OnHandCardChanged, this.list_HandCardData);
	}

	// Token: 0x0600024E RID: 590 RVA: 0x00009E3C File Offset: 0x0000803C
	private void OnRequestCombineCard(CardData cannonData, CardData panelData, int overrideSiblingIndex)
	{
		Debug.LogError("需要重寫合併功能");
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00009E48 File Offset: 0x00008048
	private void OnRequestDiscardAllCardsFromHand()
	{
		foreach (CardData item in this.list_HandCardData)
		{
			this.list_DiscardData.Add(item);
		}
		this.list_HandCardData.Clear();
		EventMgr.SendEvent<List<CardData>>(eGameEvents.OnHandCardChanged, this.list_HandCardData);
	}

	// Token: 0x06000250 RID: 592 RVA: 0x00009EC0 File Offset: 0x000080C0
	private void OnRequestRedrawCards()
	{
		this.PutCardBackToDeck(this.list_HandCardData.Count);
	}

	// Token: 0x06000251 RID: 593 RVA: 0x00009ED3 File Offset: 0x000080D3
	private void OnRequestResetDrawCardCost()
	{
		this.DrawCardCost = this.DRAW_CARD_COST_INITIAL;
		EventMgr.SendEvent<int>(eGameEvents.OnDrawCardCostChanged, this.DrawCardCost);
	}

	// Token: 0x06000252 RID: 594 RVA: 0x00009EF4 File Offset: 0x000080F4
	private void PutDiscardBackToDeck()
	{
		this.list_DiscardData.Shuffle<CardData>();
		foreach (CardData cardData in this.list_DiscardData)
		{
			if (cardData.IsFromPlayerStorage)
			{
				this.list_DeckData.Add(cardData);
			}
		}
		this.list_DiscardData.Clear();
		EventMgr.SendEvent<List<CardData>>(eGameEvents.OnDeckChanged, this.list_DeckData);
	}

	// Token: 0x06000253 RID: 595 RVA: 0x00009F80 File Offset: 0x00008180
	private void OnRequestDrawCard(int count, bool doIncreaseDrawCost)
	{
		if (this.list_DeckData.Count < count)
		{
			this.PutDiscardBackToDeck();
		}
		this.DrawCardFromDeck(count);
		if (doIncreaseDrawCost)
		{
			this.DrawCardCost += this.DRAW_CARD_COST_INCREASE;
			EventMgr.SendEvent<int>(eGameEvents.OnDrawCardCostChanged, this.DrawCardCost);
		}
	}

	// Token: 0x06000254 RID: 596 RVA: 0x00009FD1 File Offset: 0x000081D1
	private void OnRequestShuffleDeck()
	{
		this.ShuffleDeck();
	}

	// Token: 0x06000255 RID: 597 RVA: 0x00009FDC File Offset: 0x000081DC
	public void AddCardToDeck(eItemType itemType)
	{
		DebugManager.Log(eDebugKey.CARDS, string.Format("增加卡片到牌堆：{0}", itemType), null);
		CardData item = Singleton<CardManager>.Instance.CreateCardData(itemType, false, eItemType.NONE);
		this.list_DeckData.Add(item);
		EventMgr.SendEvent<List<CardData>>(eGameEvents.OnDeckChanged, this.list_DeckData);
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000A030 File Offset: 0x00008230
	public void AddCardToPlayerHand(eItemType itemType)
	{
		DebugManager.Log(eDebugKey.CARDS, string.Format("增加卡片到手牌：{0}", itemType), null);
		CardData item = Singleton<CardManager>.Instance.CreateCardData(itemType, false, eItemType.NONE);
		this.list_HandCardData.Add(item);
		EventMgr.SendEvent<List<CardData>>(eGameEvents.OnHandCardChanged, this.list_HandCardData);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000A084 File Offset: 0x00008284
	public void AddCardToPlayerHandFromPosition(eItemType itemType, Vector3 position)
	{
		DebugManager.Log(eDebugKey.CARDS, string.Format("增加卡片到手牌：{0}", itemType), null);
		CardData cardData = Singleton<CardManager>.Instance.CreateCardData(itemType, false, eItemType.NONE);
		this.list_HandCardData.Add(cardData);
		EventMgr.SendEvent<CardData, Vector3>(eGameEvents.AddCardToHand, cardData, position);
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000A0D2 File Offset: 0x000082D2
	public void ShuffleDeck()
	{
		if (this.list_DeckData.Count == 0)
		{
			return;
		}
		DebugManager.Log(eDebugKey.CARDS, "牌堆洗牌", null);
		this.list_DeckData.Shuffle<CardData>();
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000A0FC File Offset: 0x000082FC
	public void PutCardBackToDeck(int count)
	{
		for (int i = 0; i < count; i++)
		{
			if (this.list_HandCardData.Count == 0)
			{
				return;
			}
			CardData item = this.list_HandCardData[0];
			this.list_HandCardData.RemoveAt(0);
			this.list_DeckData.Add(item);
			EventMgr.SendEvent<List<CardData>>(eGameEvents.OnDeckChanged, this.list_DeckData);
			EventMgr.SendEvent<List<CardData>>(eGameEvents.OnHandCardChanged, this.list_HandCardData);
		}
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000A170 File Offset: 0x00008370
	public void DrawCardFromDeck(int count)
	{
		DebugManager.Log(eDebugKey.CARDS, string.Format("抽卡{0}張", count), null);
		for (int i = 0; i < count; i++)
		{
			if (this.IsHandCardFull())
			{
				Debug.Log("手牌已滿, 略過一張抽卡");
			}
			else
			{
				if (this.list_DeckData.Count == 0)
				{
					Debug.Log("牌堆沒有牌可以抽了");
					return;
				}
				CardData item = this.list_DeckData[0];
				this.list_DeckData.RemoveAt(0);
				this.list_HandCardData.Add(item);
				EventMgr.SendEvent<List<CardData>>(eGameEvents.OnDeckChanged, this.list_DeckData);
				EventMgr.SendEvent<List<CardData>>(eGameEvents.OnHandCardChanged, this.list_HandCardData);
			}
		}
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000A218 File Offset: 0x00008418
	private void OnRequestSellTower(ABaseTower tower)
	{
		int sellValue = tower.GetSellValue();
		this.AddCoin(sellValue);
		SoundManager.PlaySound("UI", "SellTower_Poof", -1f, -1f, -1f);
		SoundManager.PlaySound("UI", "SellTower_Coin", -1f, -1f, -1f);
		Object.Instantiate(Resources.Load("VFX/Particle_CoinBlast"), tower.Renderer_Tower.transform.position, Quaternion.identity);
		tower.Despawn();
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0000A29C File Offset: 0x0000849C
	private void OnRequestUpgradeTower(ABaseTower tower)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04000198 RID: 408
	public List<CardData> list_DeckData;

	// Token: 0x04000199 RID: 409
	public List<CardData> list_DiscardData;

	// Token: 0x0400019A RID: 410
	public List<CardData> list_HandCardData;

	// Token: 0x0400019B RID: 411
	private readonly int DRAW_CARD_COST_INITIAL = 5;

	// Token: 0x0400019C RID: 412
	private readonly int DRAW_CARD_COST_INCREASE = 5;
}
