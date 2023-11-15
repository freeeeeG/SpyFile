using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200012B RID: 299
public abstract class AUICard : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x0001CDE2 File Offset: 0x0001AFE2
	public CardData CardData
	{
		get
		{
			return this.cardData;
		}
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0001CDEA File Offset: 0x0001AFEA
	private void OnEnable()
	{
		EventMgr.Register<eGameState>(eGameEvents.GameStateChanged, new Action<eGameState>(this.OnGameStateChanged));
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0001CE03 File Offset: 0x0001B003
	private void OnDisable()
	{
		EventMgr.Remove<eGameState>(eGameEvents.GameStateChanged, new Action<eGameState>(this.OnGameStateChanged));
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x0001CE1C File Offset: 0x0001B01C
	private void OnGameStateChanged(eGameState state)
	{
		if (this.cardState != eCardState.BUILDING)
		{
			return;
		}
		if (state != eGameState.EDIT_MODE && state != eGameState.BUFF_MODE)
		{
			Debug.Log(string.Format("清除dragPointerEventData (OnGameStateChanged), dragPointerEventData: {0}", this.dragPointerEventData.pointerDrag));
			this.EndDrag();
		}
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0001CE50 File Offset: 0x0001B050
	private void Update()
	{
		switch (this.cardState)
		{
		case eCardState.HIDE:
		case eCardState.ANIMATION:
		case eCardState.DRAGGING:
		case eCardState.BUILDING:
		case eCardState.DOCKED:
		case eCardState.REMOVED:
			break;
		case eCardState.NORMAL:
			this.UpdateCardPosition(false);
			return;
		case eCardState.RECOVERING:
			this.UpdateRecovering();
			break;
		default:
			return;
		}
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0001CE99 File Offset: 0x0001B099
	private void OnDestroy()
	{
		if (this.cardController != null)
		{
			this.cardController.UnregisterCard(this);
		}
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0001CEB8 File Offset: 0x0001B0B8
	public void SetupContent(CardData cardData, bool isDraggable)
	{
		if (cardData == null)
		{
			Debug.LogError("不可以設定空的CardData!!!");
		}
		if (cardData.GetDataSource() == null)
		{
			Debug.LogWarning(string.Format("CardData ({0})的data source是空的! 試著從ResourceMgr抓", cardData.ItemType));
			cardData.data = Object.Instantiate<AItemSettingData>(Singleton<ResourceManager>.Instance.GetItemDataByType(cardData.ItemType));
			if (cardData.GetDataSource() == null)
			{
				Debug.LogError("CardData的data source還是空的!!!");
			}
		}
		this.cardData = cardData;
		this.isDraggable = isDraggable;
		this.Initialize();
		this.SetupContentProc(cardData);
	}

	// Token: 0x060007A6 RID: 1958
	protected abstract void SetupContentProc(CardData cardData);

	// Token: 0x060007A7 RID: 1959 RVA: 0x0001CF3C File Offset: 0x0001B13C
	public void PlayDrawCardAnimation(Vector3 fromPos, float duration)
	{
		base.StartCoroutine(this.CR_DrawCardAnimation(fromPos, duration));
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0001CF4D File Offset: 0x0001B14D
	private IEnumerator CR_DrawCardAnimation(Vector3 fromPos, float duration)
	{
		this.SwitchCardState(eCardState.ANIMATION);
		Transform animTransform = this.animator.transform;
		animTransform.position = fromPos;
		animTransform.localScale = Vector3.one * 0.5f;
		animTransform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(-720f, 720f));
		float time = 0f;
		float flyHeight = Random.Range(50f, 100f);
		while (time < duration)
		{
			time += Time.deltaTime;
			float num = Mathf.Clamp01(time / duration);
			num = Easing.GetEasingFunction(Easing.Type.EaseOutCubic, num);
			animTransform.position = Vector3.Lerp(fromPos, base.transform.position, num);
			animTransform.localPosition += Vector3.up * Mathf.Sin(num * 3.1415927f) * flyHeight;
			animTransform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, num);
			animTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(base.transform.localRotation.eulerAngles.z, -8f, num));
			yield return null;
		}
		animTransform.position = base.transform.position;
		animTransform.localScale = Vector3.one;
		animTransform.localRotation = Quaternion.Euler(0f, 0f, -8f);
		this.ShowCard();
		yield break;
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x0001CF6C File Offset: 0x0001B16C
	private void UpdateRecovering()
	{
		Vector3 cardLocalPositionBySiblingIndex = this.cardController.GetCardLocalPositionBySiblingIndex(this);
		this.UpdateCardPosition(false);
		if (Vector3.Distance(base.transform.localPosition, cardLocalPositionBySiblingIndex) < 15f)
		{
			base.transform.localPosition = cardLocalPositionBySiblingIndex;
			this.SwitchCardState(eCardState.NORMAL);
		}
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0001CFB8 File Offset: 0x0001B1B8
	public void UpdateCardPosition(bool isImmediate = false)
	{
		Vector3 cardLocalPositionBySiblingIndex = this.cardController.GetCardLocalPositionBySiblingIndex(this);
		if (isImmediate)
		{
			base.transform.localPosition = cardLocalPositionBySiblingIndex;
			return;
		}
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, cardLocalPositionBySiblingIndex, Time.deltaTime * this.dragRecoverSpeed);
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x0001D00C File Offset: 0x0001B20C
	public Vector3 GetTargetCardWorldPos()
	{
		Vector3 cardLocalPositionBySiblingIndex = this.cardController.GetCardLocalPositionBySiblingIndex(this);
		return base.transform.TransformPoint(cardLocalPositionBySiblingIndex);
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x0001D032 File Offset: 0x0001B232
	public void ShowCard()
	{
		if (this.cardState != eCardState.HIDE && this.cardState != eCardState.ANIMATION)
		{
			return;
		}
		this.animator.SetBool("isOn", true);
		this.SwitchCardState(eCardState.NORMAL);
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x0001D060 File Offset: 0x0001B260
	public void RemoveCard()
	{
		DebugManager.Log(eDebugKey.UI, "移除卡片: " + base.gameObject.name, null);
		this.SwitchCardState(eCardState.REMOVED);
		this.animator.SetBool("isOn", false);
		EventMgr.SendEvent<CardData>(eGameEvents.RequestRemoveCardFromHand, this.cardData);
		Singleton<PrefabManager>.Instance.DespawnPrefab(base.gameObject, 0f);
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x0001D0CC File Offset: 0x0001B2CC
	protected void Initialize()
	{
		if (base.transform.parent.parent.gameObject.TryGetComponent<CardController>(out this.cardController))
		{
			this.cardController.RegisterCard(this);
		}
		else
		{
			Debug.LogError("找不到CardController", base.gameObject);
		}
		this.dragStartLocalPosition = base.transform.localPosition;
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x0001D12A File Offset: 0x0001B32A
	protected void SwitchCardState(eCardState targetState)
	{
		if (this.cardState == targetState)
		{
			return;
		}
		this.cardState = targetState;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x0001D13D File Offset: 0x0001B33D
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0001D13F File Offset: 0x0001B33F
	public void OnPointerUp(PointerEventData eventData)
	{
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x0001D144 File Offset: 0x0001B344
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!this.isDraggable)
		{
			return;
		}
		if (this.cardState != eCardState.NORMAL && this.cardState != eCardState.DOCKED)
		{
			return;
		}
		this.dragPointerEventData = eventData;
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			this.animator.SetBool("Selected", true);
			this.canvasGroup.blocksRaycasts = false;
		}
		if (this.cardState == eCardState.DOCKED)
		{
			this.SetDocked(false, null);
		}
		this.SwitchCardState(eCardState.DRAGGING);
		this.cardController.SetState(CardController.eCardControllerState.DRAGGING);
		this.dragStartLocalPosition = base.transform.localPosition;
		this.mousePosition = Singleton<CameraManager>.Instance.MousePosToWorldPos(Input.mousePosition).WithZ(0f);
		this.startPosition.x = base.transform.position.x;
		this.startPosition.y = base.transform.position.y;
		this.differencePoint = this.mousePosition - this.startPosition;
		SoundManager.PlaySound("UI", "ClickCard", -1f, -1f, -1f);
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x0001D260 File Offset: 0x0001B460
	public void OnDrag(PointerEventData eventData)
	{
		if (this.cardState != eCardState.DRAGGING)
		{
			return;
		}
		this.mousePosition = Singleton<CameraManager>.Instance.MousePosToWorldPos(Input.mousePosition).WithZ(0f);
		base.transform.position = this.mousePosition - this.differencePoint;
		base.transform.localPosition = base.transform.localPosition.WithZ(0f);
		if (Input.GetMouseButton(1))
		{
			this.EndDrag();
			this.animator.SetBool("Selected", false);
		}
		this.DraggingOntoFieldProc();
	}

	// Token: 0x060007B4 RID: 1972
	protected abstract void DraggingOntoFieldProc();

	// Token: 0x060007B5 RID: 1973 RVA: 0x0001D301 File Offset: 0x0001B501
	public void OnEndDrag(PointerEventData eventData)
	{
		this.EndDrag();
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x0001D30C File Offset: 0x0001B50C
	protected void EndDrag()
	{
		this.animator.SetBool("Selected", false);
		this.canvasGroup.blocksRaycasts = true;
		if (this.dragPointerEventData != null)
		{
			this.dragPointerEventData.pointerDrag = null;
			this.dragPointerEventData.dragging = false;
			this.dragPointerEventData = null;
		}
		if (this.cardState != eCardState.DOCKED)
		{
			this.cardController.SetState(CardController.eCardControllerState.NORMAL);
			this.SwitchCardState(eCardState.RECOVERING);
			this.EndDragProc();
		}
	}

	// Token: 0x060007B7 RID: 1975
	protected abstract void EndDragProc();

	// Token: 0x060007B8 RID: 1976 RVA: 0x0001D380 File Offset: 0x0001B580
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.cardController.IsState(CardController.eCardControllerState.NORMAL) && this.cardState == eCardState.NORMAL)
		{
			this.animator.SetBool("Hover", true);
		}
		if (this.cardData.CardType == eCardType.TOWER_CARD || this.cardData.CardType == eCardType.BUFF_CARD)
		{
			string locNameString = this.cardData.data.GetLocNameString(false);
			string arg = this.cardData.data.GetLocStatsString() + "\n" + this.cardData.data.GetLocFlavorTextString();
			EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
			EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, locNameString, arg);
			EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._2D, base.transform, Vector3.up * 100f);
		}
		SoundManager.PlaySound("UI", "MouseOverCard", -1f, -1f, -1f);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0001D479 File Offset: 0x0001B679
	public void OnPointerExit(PointerEventData eventData)
	{
		this.animator.SetBool("Hover", false);
		if (this.cardData.CardType == eCardType.TOWER_CARD || this.cardData.CardType == eCardType.BUFF_CARD)
		{
			EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0001D4BC File Offset: 0x0001B6BC
	public void SetDocked(bool isDocked, Transform dockTranform = null)
	{
		if (isDocked)
		{
			this.tranformParent_CardList = base.transform.parent;
			this.siblingIndexInCardList = base.transform.GetSiblingIndex();
			Debug.Log(string.Format("sibling index: {0}", this.siblingIndexInCardList));
			base.transform.SetParent(dockTranform);
			this.SwitchCardState(eCardState.DOCKED);
			return;
		}
		base.transform.SetParent(this.tranformParent_CardList);
		base.transform.SetSiblingIndex(this.siblingIndexInCardList);
		this.SwitchCardState(eCardState.RECOVERING);
		EventMgr.SendEvent<AUICard>(eGameEvents.UI_OnCardRemoveFromCraftTowerUI, this);
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x0001D556 File Offset: 0x0001B756
	public void SkipFlipAnimation()
	{
		this.animator.CrossFade("On", 0f, 0);
	}

	// Token: 0x0400062C RID: 1580
	[SerializeField]
	[Header("拖動後復原的速度")]
	protected float dragRecoverSpeed = 3f;

	// Token: 0x0400062D RID: 1581
	[SerializeField]
	protected Animator animator;

	// Token: 0x0400062E RID: 1582
	[SerializeField]
	protected CanvasGroup canvasGroup;

	// Token: 0x0400062F RID: 1583
	protected CardController cardController;

	// Token: 0x04000630 RID: 1584
	protected eCardState cardState;

	// Token: 0x04000631 RID: 1585
	protected CardData cardData;

	// Token: 0x04000632 RID: 1586
	protected bool isDraggable = true;

	// Token: 0x04000633 RID: 1587
	protected Vector2 mousePosition;

	// Token: 0x04000634 RID: 1588
	protected Vector2 startPosition;

	// Token: 0x04000635 RID: 1589
	protected Vector2 differencePoint;

	// Token: 0x04000636 RID: 1590
	protected Vector3 dragStartLocalPosition;

	// Token: 0x04000637 RID: 1591
	private PointerEventData dragPointerEventData;

	// Token: 0x04000638 RID: 1592
	protected Transform tranformParent_CardList;

	// Token: 0x04000639 RID: 1593
	protected int siblingIndexInCardList;
}
