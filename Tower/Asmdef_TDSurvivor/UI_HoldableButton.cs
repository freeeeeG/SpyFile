using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200013A RID: 314
public class UI_HoldableButton : Button
{
	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06000818 RID: 2072 RVA: 0x0001EDA2 File Offset: 0x0001CFA2
	public float PressedTime
	{
		get
		{
			return this.pressedTime;
		}
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0001EDAA File Offset: 0x0001CFAA
	protected override void Awake()
	{
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0001EDAC File Offset: 0x0001CFAC
	public void Update()
	{
		if (base.IsPressed() && this.isPointerOnButton)
		{
			this.pressedTime += Time.deltaTime;
			if (this.pressedTime > this.triggerDelay)
			{
				if (this.doContinuousTriggerOnHold)
				{
					this.eventTimer += Time.deltaTime;
					if (this.eventTimer >= this.triggerEventInterval)
					{
						this.eventTimer = 0f;
						this.OnHoldButton();
					}
				}
				else if (!this.isFirstHoldEventCalled)
				{
					this.OnHoldButton();
					this.isFirstHoldEventCalled = true;
				}
			}
			this.isLastFramePressed = true;
			return;
		}
		if (this.isLastFramePressed && this.isPointerOnButton)
		{
			this.OnButtonUp();
		}
		this.isLastFramePressed = false;
		this.pressedTime = 0f;
		this.eventTimer = 0f;
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0001EE89 File Offset: 0x0001D089
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!base.interactable)
		{
			return;
		}
		base.OnPointerDown(eventData);
		this.isFirstHoldEventCalled = false;
		this.OnButtonDown();
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x0001EEAD File Offset: 0x0001D0AD
	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (!base.interactable)
		{
			return;
		}
		this.isPointerOnButton = true;
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x0001EEBF File Offset: 0x0001D0BF
	public override void OnPointerExit(PointerEventData eventData)
	{
		if (!base.interactable)
		{
			return;
		}
		this.isPointerOnButton = false;
	}

	// Token: 0x04000693 RID: 1683
	[Header("第一次觸發Hold需要多久")]
	[SerializeField]
	protected float triggerDelay = 0.5f;

	// Token: 0x04000694 RID: 1684
	[Header("是否會連續觸發")]
	[SerializeField]
	protected bool doContinuousTriggerOnHold;

	// Token: 0x04000695 RID: 1685
	[Header("按著多久會連續觸發一次")]
	[SerializeField]
	protected float triggerEventInterval = 0.1f;

	// Token: 0x04000696 RID: 1686
	protected float eventTimer;

	// Token: 0x04000697 RID: 1687
	protected float pressedTime;

	// Token: 0x04000698 RID: 1688
	public Action OnButtonDown = delegate()
	{
	};

	// Token: 0x04000699 RID: 1689
	public Action OnHoldButton = delegate()
	{
	};

	// Token: 0x0400069A RID: 1690
	public Action OnButtonUp = delegate()
	{
	};

	// Token: 0x0400069B RID: 1691
	private bool isLastFramePressed;

	// Token: 0x0400069C RID: 1692
	private bool isPointerOnButton;

	// Token: 0x0400069D RID: 1693
	private bool isFirstHoldEventCalled;
}
