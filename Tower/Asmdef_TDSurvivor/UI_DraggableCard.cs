using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000161 RID: 353
public class UI_DraggableCard : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x06000944 RID: 2372 RVA: 0x00023172 File Offset: 0x00021372
	private void Awake()
	{
		this.func_FollowUITarget.ToggleFollowing(false);
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00023180 File Offset: 0x00021380
	public void SetupContent(TowerIngameData data)
	{
		this.towerIngameData = data;
		this.cache_TowerSettingData = (Singleton<ResourceManager>.Instance.GetItemDataByType(data.ItemType) as TowerSettingData);
		this.cardFace.SetupContent(data.ItemType, data.ItemType.ToCardType(), this.cache_TowerSettingData.GetCardIcon(), true);
		this.cardFace.SetTowerDetail(data.ItemType, true, true);
		base.gameObject.name = base.gameObject.name + string.Format("_{0}", data.ItemType);
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0002321A File Offset: 0x0002141A
	public void SetupReference(UI_TowerArrange_Popup towerArrangeUI, Transform node_CardPool, Transform node_DraggingCardParent)
	{
		this.ref_TowerArrangeUI = towerArrangeUI;
		this.node_CardPool = node_CardPool;
		this.node_DraggingCardParent = node_DraggingCardParent;
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00023231 File Offset: 0x00021431
	public void StartCardFollowing()
	{
		this.func_FollowUITarget.ToggleFollowing(true);
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x00023240 File Offset: 0x00021440
	public void ReturnToCardPool()
	{
		if (this.IsInCardSlot())
		{
			this.currentCardSlot.RemoveCardFromSlot(this);
			this.currentCardSlot = null;
		}
		base.transform.SetParent(this.node_CardPool, true);
		if (this.siblingIndexInCardPool != -1)
		{
			base.transform.SetSiblingIndex(this.siblingIndexInCardPool);
		}
		this.layoutElement.ignoreLayout = false;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.node_CardPool as RectTransform);
		this.func_FollowUITarget.PreservePosition();
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x000232BC File Offset: 0x000214BC
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right && this.IsInCardSlot())
		{
			SoundManager.PlaySound("UI", "CardRemove", -1f, -1f, -1f);
			this.ReturnToCardPool();
		}
		if (eventData.button == PointerEventData.InputButton.Left && !this.IsInCardSlot())
		{
			UI_Obj_CardSlot emptyCardSlot = this.ref_TowerArrangeUI.GetEmptyCardSlot();
			if (emptyCardSlot != null)
			{
				base.transform.SetParent(this.node_DraggingCardParent, true);
				emptyCardSlot.PutCardOnSlot(this);
				SoundManager.PlaySound("UI", "CardMoveDirectlyToSlot", -1f, -1f, -1f);
			}
		}
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0002335C File Offset: 0x0002155C
	public void OnBeginDrag(PointerEventData eventData)
	{
		this.isDragging = true;
		base.transform.SetParent(this.node_DraggingCardParent, true);
		this.layoutElement.ignoreLayout = true;
		base.transform.SetAsLastSibling();
		Action<UI_DraggableCard> onCardStartDragCallback = this.OnCardStartDragCallback;
		if (onCardStartDragCallback != null)
		{
			onCardStartDragCallback(this);
		}
		SoundManager.PlaySound("UI", "CardPickup", -1f, -1f, -1f);
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x000233CC File Offset: 0x000215CC
	public void OnDrag(PointerEventData eventData)
	{
		this.rectTransform.position = Singleton<CameraManager>.Instance.ScreenPosToUIPos(eventData.position);
		Debug.Log("OnDrag: 底下是 " + eventData.pointerCurrentRaycast.gameObject.name);
		if (this.IsInCardSlot())
		{
			UI_Obj_CardSlot component = eventData.pointerCurrentRaycast.gameObject.GetComponent<UI_Obj_CardSlot>();
			Debug.Log(string.Format("cardSlot: {0}", component != null));
			if (component != null && component != this.currentCardSlot && component.HasCardInSlot())
			{
				Debug.Log("拖曳中交換: " + base.name + " <-> " + component.GetCurrentCard().name);
				component.MoveCurrentCardToSlot(this.GetCardSlot());
				component.PutCardOnSlot(this);
			}
		}
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x000234AC File Offset: 0x000216AC
	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("OnEndDrag: 底下是 " + eventData.pointerCurrentRaycast.gameObject.name);
		if (this.IsInCardSlot())
		{
			this.SetCardToSlotPosition();
		}
		else
		{
			this.ReturnToCardPool();
		}
		this.isDragging = false;
		this.layoutElement.ignoreLayout = false;
		Action<UI_DraggableCard> onCardEndDragCallback = this.OnCardEndDragCallback;
		if (onCardEndDragCallback != null)
		{
			onCardEndDragCallback(this);
		}
		SoundManager.PlaySound("UI", "CardDrop", -1f, -1f, -1f);
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x00023535 File Offset: 0x00021735
	public TowerIngameData GetTowerIngameData()
	{
		return this.towerIngameData;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0002353D File Offset: 0x0002173D
	public bool IsInCardSlot()
	{
		return this.currentCardSlot != null;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0002354B File Offset: 0x0002174B
	public void SetCardToSlotPosition()
	{
		if (this.currentCardSlot == null)
		{
			return;
		}
		base.transform.position = this.currentCardSlot.transform.position;
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00023577 File Offset: 0x00021777
	public UI_Obj_CardSlot GetCardSlot()
	{
		return this.currentCardSlot;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0002357F File Offset: 0x0002177F
	public void RegisterToCardSlot(UI_Obj_CardSlot slot)
	{
		this.currentCardSlot = slot;
		this.layoutElement.ignoreLayout = true;
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x00023594 File Offset: 0x00021794
	public void ToggleRaycast(bool isOn)
	{
		this.canvasGroup.blocksRaycasts = isOn;
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x000235A2 File Offset: 0x000217A2
	public void OnPointerDown(PointerEventData eventData)
	{
		this.animator.SetBool("isOn", true);
		if (!this.IsInCardSlot())
		{
			this.siblingIndexInCardPool = base.transform.GetSiblingIndex();
			return;
		}
		this.siblingIndexInCardPool = -1;
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x000235D6 File Offset: 0x000217D6
	public void OnPointerUp(PointerEventData eventData)
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x000235E9 File Offset: 0x000217E9
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isMouseOver = true;
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x000235F2 File Offset: 0x000217F2
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isTooltipOn = false;
		this.isMouseOver = false;
		this.mouseStayTime = 0f;
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x00023620 File Offset: 0x00021820
	private void Update()
	{
		if (this.isMouseOver && !this.isTooltipOn)
		{
			this.mouseStayTime += Time.deltaTime;
			if (this.mouseStayTime > 0.5f)
			{
				this.isTooltipOn = true;
				string locNameString = this.cache_TowerSettingData.GetLocNameString(true);
				string arg = this.cache_TowerSettingData.GetLocStatsString() + "\n" + this.cache_TowerSettingData.GetLocFlavorTextString();
				EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
				EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, locNameString, arg);
				EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._2D, base.transform, Vector3.up * 50f + Vector3.right * 50f);
			}
		}
	}

	// Token: 0x04000754 RID: 1876
	[SerializeField]
	private Animator animator;

	// Token: 0x04000755 RID: 1877
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x04000756 RID: 1878
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04000757 RID: 1879
	[SerializeField]
	private UI_CardFace cardFace;

	// Token: 0x04000758 RID: 1880
	private bool isDragging;

	// Token: 0x04000759 RID: 1881
	private Vector3 startDragPosition;

	// Token: 0x0400075A RID: 1882
	private Vector3 startDragMousePos;

	// Token: 0x0400075B RID: 1883
	private Transform node_CardPool;

	// Token: 0x0400075C RID: 1884
	private Transform node_DraggingCardParent;

	// Token: 0x0400075D RID: 1885
	private int siblingIndexInCardPool = -1;

	// Token: 0x0400075E RID: 1886
	[SerializeField]
	private LayoutElement layoutElement;

	// Token: 0x0400075F RID: 1887
	[SerializeField]
	private UI_Func_FollowUITarget func_FollowUITarget;

	// Token: 0x04000760 RID: 1888
	[SerializeField]
	private UI_Obj_CardSlot currentCardSlot;

	// Token: 0x04000761 RID: 1889
	private TowerIngameData towerIngameData;

	// Token: 0x04000762 RID: 1890
	private UI_TowerArrange_Popup ref_TowerArrangeUI;

	// Token: 0x04000763 RID: 1891
	public Action<UI_DraggableCard> OnCardStartDragCallback;

	// Token: 0x04000764 RID: 1892
	public Action<UI_DraggableCard> OnCardEndDragCallback;

	// Token: 0x04000765 RID: 1893
	public Action<UI_DraggableCard> OnCardClickCallback;

	// Token: 0x04000766 RID: 1894
	private TowerSettingData cache_TowerSettingData;

	// Token: 0x04000767 RID: 1895
	private bool isMouseOver;

	// Token: 0x04000768 RID: 1896
	private float mouseStayTime;

	// Token: 0x04000769 RID: 1897
	private bool isTooltipOn;
}
