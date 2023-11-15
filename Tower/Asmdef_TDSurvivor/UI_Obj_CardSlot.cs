using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000178 RID: 376
public class UI_Obj_CardSlot : MonoBehaviour, IDropHandler, IEventSystemHandler
{
	// Token: 0x060009F3 RID: 2547 RVA: 0x000255FD File Offset: 0x000237FD
	private void Start()
	{
		this.image_Debug_SlotStatus.gameObject.SetActive(false);
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00025610 File Offset: 0x00023810
	private void Update()
	{
		if (this.HasCardInSlot())
		{
			Debug.DrawLine(base.transform.position, this.currentCard.transform.position, Color.red);
		}
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x0002563F File Offset: 0x0002383F
	public bool HasCardInSlot()
	{
		return this.currentCard != null;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0002564D File Offset: 0x0002384D
	public UI_DraggableCard GetCurrentCard()
	{
		return this.currentCard;
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x00025655 File Offset: 0x00023855
	public void PutCardOnSlot(UI_DraggableCard card)
	{
		if (this.currentCard == card)
		{
			return;
		}
		this.currentCard = card;
		card.RegisterToCardSlot(this);
		card.transform.position = base.transform.position;
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x0002568C File Offset: 0x0002388C
	public void MoveCurrentCardToSlot(UI_Obj_CardSlot targetSlot)
	{
		if (this.currentCard == null)
		{
			return;
		}
		SoundManager.PlaySound("UI", "CardSwap", -1f, -1f, -1f);
		targetSlot.PutCardOnSlot(this.currentCard);
		this.currentCard = null;
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x000256DA File Offset: 0x000238DA
	public void RemoveCardFromSlot(UI_DraggableCard card)
	{
		if (card != this.currentCard)
		{
			return;
		}
		this.currentCard = null;
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x000256F4 File Offset: 0x000238F4
	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
		{
			return;
		}
		Debug.Log("OnDrop: " + eventData.pointerDrag.name);
		UI_DraggableCard component = eventData.pointerDrag.GetComponent<UI_DraggableCard>();
		if (component != null)
		{
			if (component.IsInCardSlot())
			{
				if (this.HasCardInSlot())
				{
					Debug.Log("交換卡片: " + this.currentCard.name + " <-> " + component.name);
					this.MoveCurrentCardToSlot(component.GetCardSlot());
				}
				else
				{
					component.GetCardSlot().RemoveCardFromSlot(component);
				}
			}
			else if (this.HasCardInSlot())
			{
				this.currentCard.ReturnToCardPool();
			}
			this.PutCardOnSlot(component);
		}
	}

	// Token: 0x040007BC RID: 1980
	[SerializeField]
	private Image image_Debug_SlotStatus;

	// Token: 0x040007BD RID: 1981
	[SerializeField]
	private UI_DraggableCard currentCard;
}
