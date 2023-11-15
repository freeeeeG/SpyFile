using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000057 RID: 87
public class CardController : MonoBehaviour
{
	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000841B File Offset: 0x0000661B
	public CardController.eCardControllerState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x060001DA RID: 474 RVA: 0x00008423 File Offset: 0x00006623
	private void OnEnable()
	{
		EventMgr.Register<List<CardData>>(eGameEvents.OnHandCardChanged, new Action<List<CardData>>(this.OnCardChanged));
		EventMgr.Register<CardData, Vector3>(eGameEvents.AddCardToHand, new Action<CardData, Vector3>(this.OnAddCardToHand));
	}

	// Token: 0x060001DB RID: 475 RVA: 0x00008455 File Offset: 0x00006655
	private void OnDisable()
	{
		EventMgr.Remove<List<CardData>>(eGameEvents.OnHandCardChanged, new Action<List<CardData>>(this.OnCardChanged));
		EventMgr.Remove<CardData, Vector3>(eGameEvents.AddCardToHand, new Action<CardData, Vector3>(this.OnAddCardToHand));
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00008488 File Offset: 0x00006688
	private void OnAddCardToHand(CardData cardData, Vector3 flyInOriginPosition)
	{
		AUICard auicard = this.CreateCardUI(cardData);
		auicard.PlayDrawCardAnimation(flyInOriginPosition, 0.7f);
		auicard.SkipFlipAnimation();
		auicard.transform.localScale = Vector3.one * 2f;
		auicard.transform.DOScale(Vector3.one, 0.7f);
	}

	// Token: 0x060001DD RID: 477 RVA: 0x000084E0 File Offset: 0x000066E0
	private void OnCardChanged(List<CardData> cardDataList)
	{
		for (int i = 0; i < cardDataList.Count; i++)
		{
			CardData cardData = cardDataList[i];
			bool flag = false;
			for (int j = 0; j < this.list_Cards.Count; j++)
			{
				if (this.list_Cards[j].CardData == cardData)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.CreateCardUI(cardData).PlayDrawCardAnimation(this.ui_PlayerDeck.GetDrawCardInitialPosition(), 0.5f);
			}
		}
		for (int k = this.list_Cards.Count - 1; k >= 0; k--)
		{
			bool flag2 = false;
			for (int l = 0; l < cardDataList.Count; l++)
			{
				if (this.list_Cards[k].CardData == cardDataList[l])
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				this.list_Cards[k].RemoveCard();
			}
		}
		foreach (AUICard auicard in this.list_Cards)
		{
			auicard.UpdateCardPosition(false);
		}
	}

	// Token: 0x060001DE RID: 478 RVA: 0x00008608 File Offset: 0x00006808
	private AUICard CreateCardUI(CardData cardData)
	{
		GameObject gameObject = Singleton<PrefabManager>.Instance.InstantiatePrefab("Obj_UI_Card", base.transform.position, Quaternion.identity, this.node_CardParent);
		AUICard component = gameObject.GetComponent<AUICard>();
		component.SetupContent(cardData, true);
		if (cardData.siblingIndexOnCreate > 0)
		{
			gameObject.transform.SetSiblingIndex(cardData.siblingIndexOnCreate);
		}
		component.UpdateCardPosition(true);
		component.transform.localScale = Vector3.one;
		component.ShowCard();
		SoundManager.PlaySound("UI", "DrawCard", -1f, -1f, -1f);
		return component;
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000869F File Offset: 0x0000689F
	public void RegisterCard(AUICard card)
	{
		if (!this.list_Cards.Contains(card))
		{
			this.list_Cards.Add(card);
			DebugManager.Log(eDebugKey.UI, "註冊卡片: " + card.gameObject.name, card.gameObject);
		}
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x000086DD File Offset: 0x000068DD
	public void UnregisterCard(AUICard card)
	{
		if (this.list_Cards.Contains(card))
		{
			this.list_Cards.Remove(card);
		}
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x000086FA File Offset: 0x000068FA
	public void SetState(CardController.eCardControllerState targetState)
	{
		this.state = targetState;
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x00008703 File Offset: 0x00006903
	public bool IsState(CardController.eCardControllerState state)
	{
		return state == this.state;
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x00008710 File Offset: 0x00006910
	public Vector3 GetCardLocalPositionBySiblingIndex(AUICard card)
	{
		Vector3 a = Vector3.zero;
		int num = card.transform.GetSiblingIndex() % this.cardInARow;
		int num2 = card.transform.GetSiblingIndex() / this.cardInARow;
		switch (this.cardAlignType)
		{
		case CardController.eCardAlignType.CENTER:
		{
			int count = this.list_Cards.Count;
			int num3 = count / 2;
			float num4 = ((count % 2 == 0) ? 0.5f : 0f) * this.GetCardSpacing();
			a = this.node_CardStartPoint.localPosition + Vector3.left * ((float)(num3 - num) * this.GetCardSpacing() - num4);
			break;
		}
		case CardController.eCardAlignType.LEFT:
			a = this.node_CardStartPoint.localPosition - Vector3.right * this.GetCardSpacing() * (float)num;
			break;
		case CardController.eCardAlignType.RIGHT:
			a = this.node_CardStartPoint.localPosition + Vector3.right * this.GetCardSpacing() * (float)num;
			break;
		}
		return a + (Vector3.up * (float)num2 * this.cardSpacing_Vertical + Vector3.right * (float)num2 * 0.5f * this.GetCardSpacing());
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00008858 File Offset: 0x00006A58
	public Vector3 GetCardLocalRotationBySiblingIndex(AUICard card)
	{
		Vector3 zero = Vector3.zero;
		int num = card.transform.GetSiblingIndex() % this.cardInARow;
		return Vector3.forward * 1f * this.cardMaxRotation * ((float)num / (float)this.list_Cards.Count - 0.5f) * 2f;
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x000088BC File Offset: 0x00006ABC
	public float GetCardSpacing()
	{
		if (this.list_Cards.Count < 6)
		{
			return this.cardSpacing_Horizontal;
		}
		return this.cardSpacing_Horizontal * Mathf.Max(0.2f, 1f - 0.08f * (float)(this.list_Cards.Count - 6));
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x0000890C File Offset: 0x00006B0C
	public AUICard GetNearestCardInRange(AUICard card, float range)
	{
		return (from c in this.list_Cards
		where c.CardData.CardType == eCardType.PANEL_CARD && Vector3.Distance(c.transform.position, card.transform.position) <= range
		orderby Vector3.Distance(c.transform.position, card.transform.position)
		select c).FirstOrDefault<AUICard>();
	}

	// Token: 0x0400016E RID: 366
	[SerializeField]
	private List<AUICard> list_Cards;

	// Token: 0x0400016F RID: 367
	[SerializeField]
	private UI_PlayerDeck ui_PlayerDeck;

	// Token: 0x04000170 RID: 368
	[SerializeField]
	private CardController.eCardControllerState state;

	// Token: 0x04000171 RID: 369
	[SerializeField]
	[Header("卡片開始排列的節點")]
	private Transform node_CardStartPoint;

	// Token: 0x04000172 RID: 370
	[SerializeField]
	[Header("卡片放置節點")]
	private Transform node_CardParent;

	// Token: 0x04000173 RID: 371
	[SerializeField]
	[Header("卡片排列方式")]
	private CardController.eCardAlignType cardAlignType;

	// Token: 0x04000174 RID: 372
	[SerializeField]
	[Header("每張卡片的間距")]
	[FormerlySerializedAs("cardSpacing")]
	private float cardSpacing_Horizontal = 50f;

	// Token: 0x04000175 RID: 373
	[SerializeField]
	[Header("每張卡片的間距")]
	[FormerlySerializedAs("cardSpacing")]
	private float cardSpacing_Vertical = 50f;

	// Token: 0x04000176 RID: 374
	[SerializeField]
	[Header("卡片最大旋轉角度")]
	private float cardMaxRotation = 15f;

	// Token: 0x04000177 RID: 375
	[SerializeField]
	private int cardInARow = 8;

	// Token: 0x020001E2 RID: 482
	public enum eCardControllerState
	{
		// Token: 0x040009D4 RID: 2516
		NORMAL,
		// Token: 0x040009D5 RID: 2517
		DRAGGING
	}

	// Token: 0x020001E3 RID: 483
	public enum eCardAlignType
	{
		// Token: 0x040009D7 RID: 2519
		CENTER,
		// Token: 0x040009D8 RID: 2520
		LEFT,
		// Token: 0x040009D9 RID: 2521
		RIGHT
	}
}
