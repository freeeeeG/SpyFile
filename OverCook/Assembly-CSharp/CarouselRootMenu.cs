using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000AA7 RID: 2727
[RequireComponent(typeof(T17GridLayoutGroup))]
public class CarouselRootMenu : RootMenu, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x060035F0 RID: 13808 RVA: 0x000FC210 File Offset: 0x000FA610
	public CarouselButton[] Buttons
	{
		get
		{
			return this.m_carouselButtons;
		}
	}

	// Token: 0x060035F1 RID: 13809 RVA: 0x000FC218 File Offset: 0x000FA618
	public CarouselButton GetCurrentButton()
	{
		return this.m_currentButton;
	}

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x060035F2 RID: 13810 RVA: 0x000FC220 File Offset: 0x000FA620
	// (remove) Token: 0x060035F3 RID: 13811 RVA: 0x000FC258 File Offset: 0x000FA658
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CarouselRootMenu.CarouselButtonClickedEvent CarouselButtonClicked;

	// Token: 0x060035F4 RID: 13812 RVA: 0x000FC290 File Offset: 0x000FA690
	protected override void Awake()
	{
		base.Awake();
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		this.m_gridLayout = base.gameObject.RequireComponent<T17GridLayoutGroup>();
		this.m_rectTransform = base.gameObject.RequireComponent<RectTransform>();
		CarouselButton carouselButton = null;
		this.m_carouselButtons = base.GetComponentsInChildren<CarouselButton>();
		for (int i = 0; i < this.m_carouselButtons.Length; i++)
		{
			CarouselButton curr = this.m_carouselButtons[i];
			curr.m_rootMenu = this;
			T17Button button = curr.Button as T17Button;
			if (button != null)
			{
				button.onClick.AddListener(delegate()
				{
					this.OnClick(curr);
				});
				T17Button button6 = button;
				button6.OnButtonDeselect = (T17Button.T17ButtonDelegate)Delegate.Combine(button6.OnButtonDeselect, new T17Button.T17ButtonDelegate(this.OnDeselected));
				if (this.m_canInteractWithMouse)
				{
					T17Button button2 = button;
					button2.OnButtonPointerEnter = (T17Button.T17ButtonDelegate)Delegate.Combine(button2.OnButtonPointerEnter, new T17Button.T17ButtonDelegate(delegate(T17Button A_1)
					{
						button.interactable = true;
					}));
					T17Button button3 = button;
					button3.OnButtonPointerExit = (T17Button.T17ButtonDelegate)Delegate.Combine(button3.OnButtonPointerExit, new T17Button.T17ButtonDelegate(delegate(T17Button A_1)
					{
						button.interactable = this.IsButtonInteractable(curr);
					}));
				}
				else
				{
					T17Button button4 = button;
					button4.OnButtonPointerEnter = (T17Button.T17ButtonDelegate)Delegate.Combine(button4.OnButtonPointerEnter, new T17Button.T17ButtonDelegate(delegate(T17Button A_1)
					{
						button.interactable = this.IsButtonInteractable(curr);
					}));
					T17Button button5 = button;
					button5.OnButtonPointerExit = (T17Button.T17ButtonDelegate)Delegate.Combine(button5.OnButtonPointerExit, new T17Button.T17ButtonDelegate(delegate(T17Button A_1)
					{
						button.interactable = this.IsButtonInteractable(curr);
					}));
				}
			}
			if (carouselButton != null)
			{
				this.ConnectHorizontal(carouselButton.Button, curr.Button);
			}
			carouselButton = curr;
		}
	}

	// Token: 0x060035F5 RID: 13813 RVA: 0x000FC45F File Offset: 0x000FA85F
	protected override void Start()
	{
		base.Start();
		if (base.CachedEventSystem != null)
		{
			this.SelectInitialButton();
		}
		else
		{
			this.m_playerManager.EngagementChangeCallback += this.OnEngagementChanged;
		}
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x000FC49A File Offset: 0x000FA89A
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.m_playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x000FC4BC File Offset: 0x000FA8BC
	private void OnEngagementChanged(EngagementSlot _s, GamepadUser _p, GamepadUser _n)
	{
		if (_n == null)
		{
			return;
		}
		this.m_CachedEventSystem = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(_n);
		if (this.m_CachedEventSystem == null)
		{
			return;
		}
		this.m_playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		this.SelectInitialButton();
	}

	// Token: 0x060035F8 RID: 13816 RVA: 0x000FC516 File Offset: 0x000FA916
	private void SetInteractible(CarouselButton button)
	{
		if (this.m_canInteractWithMouse)
		{
			button.Button.interactable = true;
		}
		else
		{
			button.Button.interactable = this.IsButtonInteractable(button);
		}
	}

	// Token: 0x060035F9 RID: 13817 RVA: 0x000FC548 File Offset: 0x000FA948
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (base.CachedEventSystem != null)
		{
			CarouselButton[] componentsInChildren = base.GetComponentsInChildren<CarouselButton>();
			base.CachedEventSystem.SetSelectedGameObject(componentsInChildren[componentsInChildren.Length / 2].Button.gameObject);
		}
		return base.Show(currentGamer, parent, invoker, hideInvoker);
	}

	// Token: 0x060035FA RID: 13818 RVA: 0x000FC594 File Offset: 0x000FA994
	private void SelectInitialButton()
	{
		CarouselButton initialButton = this.GetInitialButton();
		if (initialButton == null)
		{
			return;
		}
		this.m_buttonLerp = base.StartCoroutine(this.LerpToButton(initialButton, true));
		base.CachedEventSystem.SetSelectedGameObject(initialButton.Button.gameObject);
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x000FC5DF File Offset: 0x000FA9DF
	protected virtual CarouselButton GetInitialButton()
	{
		return (this.m_carouselButtons.Length <= 0) ? null : this.m_carouselButtons[this.m_carouselButtons.Length / 2];
	}

	// Token: 0x060035FC RID: 13820 RVA: 0x000FC608 File Offset: 0x000FAA08
	protected virtual bool IsButtonInteractable(CarouselButton _button)
	{
		if (_button == this.m_currentButton)
		{
			return true;
		}
		if (base.CachedEventSystem != null)
		{
			T17StandaloneInputModule t17StandaloneInputModule = (T17StandaloneInputModule)base.CachedEventSystem.currentInputModule;
			if (t17StandaloneInputModule != null && t17StandaloneInputModule.WasUsingMouse)
			{
				return true;
			}
		}
		return base.CachedEventSystem != null && base.CachedEventSystem.currentSelectedGameObject != null && base.CachedEventSystem.currentSelectedGameObject.IsInHierarchyOf(base.gameObject);
	}

	// Token: 0x060035FD RID: 13821 RVA: 0x000FC6A8 File Offset: 0x000FAAA8
	public void OnBeginDrag(PointerEventData _eventData)
	{
		this.m_isDragging = true;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_rectTransform, _eventData.position, null, out this.m_dragPos);
	}

	// Token: 0x060035FE RID: 13822 RVA: 0x000FC6CC File Offset: 0x000FAACC
	public void OnDrag(PointerEventData _eventData)
	{
		if (this.m_buttonLerp != null)
		{
			base.StopCoroutine(this.m_buttonLerp);
			this.m_buttonLerp = null;
		}
		Vector2 dragPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_rectTransform, _eventData.position, null, out dragPos);
		RectOffset ourPadding = new RectOffset(Mathf.RoundToInt((float)this.m_gridLayout.padding.left + (dragPos.x - this.m_dragPos.x)), this.m_gridLayout.padding.right, this.m_gridLayout.padding.top, this.m_gridLayout.padding.bottom);
		this.m_gridLayout.ourPadding = ourPadding;
		this.m_gridLayout.ForceRefresh();
		this.m_dragPos = dragPos;
		this.RecenterMenu();
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x000FC790 File Offset: 0x000FAB90
	public void OnEndDrag(PointerEventData _eventData)
	{
		if (this.m_dragCentering != null)
		{
			base.StopCoroutine(this.m_dragCentering);
		}
		this.m_dragCentering = base.StartCoroutine(this.SelectAfterDrag(false));
		this.m_isDragging = false;
	}

	// Token: 0x06003600 RID: 13824 RVA: 0x000FC7C4 File Offset: 0x000FABC4
	private IEnumerator SelectAfterDrag(bool _doInstant = false)
	{
		if (!_doInstant)
		{
			yield return new WaitForSeconds(this.m_selectAfterDragDelay);
		}
		if (this.m_buttonLerp != null)
		{
			base.StopCoroutine(this.m_buttonLerp);
			this.m_buttonLerp = null;
		}
		this.OnButtonSelected(this.GetButtonInFocusArea());
		yield break;
	}

	// Token: 0x06003601 RID: 13825 RVA: 0x000FC7E8 File Offset: 0x000FABE8
	protected void OnClick(CarouselButton _carouselButton)
	{
		if (this.m_isDragging)
		{
			return;
		}
		if (_carouselButton == this.GetButtonInFocusArea())
		{
			if (this.CarouselButtonClicked != null)
			{
				this.CarouselButtonClicked(_carouselButton);
			}
		}
		else
		{
			this.HandleButtonSelection(_carouselButton);
		}
	}

	// Token: 0x06003602 RID: 13826 RVA: 0x000FC838 File Offset: 0x000FAC38
	public void OnButtonSelected(CarouselButton _carouselButton)
	{
		if (this.m_isDragging)
		{
			return;
		}
		if (base.CachedEventSystem != null)
		{
			T17StandaloneInputModule t17StandaloneInputModule = (T17StandaloneInputModule)base.CachedEventSystem.currentInputModule;
			if (t17StandaloneInputModule != null)
			{
				if (!t17StandaloneInputModule.WasUsingMouse || !Input.GetMouseButtonDown(0))
				{
					this.HandleButtonSelection(_carouselButton);
				}
				t17StandaloneInputModule.SetLastSelected(_carouselButton.gameObject);
			}
		}
	}

	// Token: 0x06003603 RID: 13827 RVA: 0x000FC8A8 File Offset: 0x000FACA8
	private void HandleButtonSelection(CarouselButton _carouselButton)
	{
		if (_carouselButton.gameObject.IsInHierarchyOf(this.m_gridLayout.gameObject))
		{
			if (this.m_deselectMenu != null)
			{
				base.StopCoroutine(this.m_deselectMenu);
				this.m_deselectMenu = null;
			}
			if (this.m_buttonLerp != null)
			{
				base.StopCoroutine(this.m_buttonLerp);
				this.m_buttonLerp = null;
			}
			if (this.m_dragCentering != null)
			{
				base.StopCoroutine(this.m_dragCentering);
				this.m_dragCentering = null;
			}
			CarouselButton[] componentsInChildren = base.GetComponentsInChildren<CarouselButton>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.SetInteractible(componentsInChildren[i]);
			}
			this.RecenterMenu();
			if (componentsInChildren.Length > 0)
			{
				this.ConnectHorizontal(componentsInChildren[componentsInChildren.Length - 1].Button, componentsInChildren[0].Button);
			}
			this.m_buttonLerp = base.StartCoroutine(this.LerpToButton(_carouselButton, false));
		}
	}

	// Token: 0x06003604 RID: 13828 RVA: 0x000FC98B File Offset: 0x000FAD8B
	protected virtual void OnDeselected(T17Button _button)
	{
		if (this.m_deselectMenu == null)
		{
			this.m_deselectMenu = base.StartCoroutine(this.DeselectMenu());
		}
	}

	// Token: 0x06003605 RID: 13829 RVA: 0x000FC9AC File Offset: 0x000FADAC
	public void ShiftFocus(int _offset)
	{
		if (base.CachedEventSystem == null)
		{
			return;
		}
		CarouselButton buttonInFocusArea = this.GetButtonInFocusArea();
		CarouselButton[] componentsInChildren = base.GetComponentsInChildren<CarouselButton>();
		int i;
		for (i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] == buttonInFocusArea)
			{
				break;
			}
		}
		UnityEngine.Debug.LogError(i + ", " + (i + _offset) % componentsInChildren.Length);
		if (i != componentsInChildren.Length)
		{
			i = (i + _offset) % componentsInChildren.Length;
			base.CachedEventSystem.SetSelectedGameObject(componentsInChildren[i].Button.gameObject);
		}
	}

	// Token: 0x06003606 RID: 13830 RVA: 0x000FCA4A File Offset: 0x000FAE4A
	public void ShiftLeft()
	{
		this.ShiftFocus(-1);
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x000FCA53 File Offset: 0x000FAE53
	public void ShiftRight()
	{
		this.ShiftFocus(1);
	}

	// Token: 0x06003608 RID: 13832 RVA: 0x000FCA5C File Offset: 0x000FAE5C
	protected void MoveFrontToEnd()
	{
		CarouselButton[] componentsInChildren = base.GetComponentsInChildren<CarouselButton>();
		componentsInChildren[0].transform.SetAsLastSibling();
		RectOffset rectOffset = new RectOffset(this.m_gridLayout.padding.left, this.m_gridLayout.padding.right, this.m_gridLayout.padding.top, this.m_gridLayout.padding.bottom);
		rectOffset.left += Mathf.RoundToInt(this.m_gridLayout.cellSize.x + this.m_gridLayout.spacing.x);
		this.m_gridLayout.ourPadding = rectOffset;
		this.m_gridLayout.ForceRefresh();
	}

	// Token: 0x06003609 RID: 13833 RVA: 0x000FCB14 File Offset: 0x000FAF14
	protected void MoveEndToFront()
	{
		CarouselButton[] componentsInChildren = base.GetComponentsInChildren<CarouselButton>();
		componentsInChildren[componentsInChildren.Length - 1].transform.SetAsFirstSibling();
		RectOffset rectOffset = new RectOffset(this.m_gridLayout.padding.left, this.m_gridLayout.padding.right, this.m_gridLayout.padding.top, this.m_gridLayout.padding.bottom);
		rectOffset.left -= Mathf.RoundToInt(this.m_gridLayout.cellSize.x + this.m_gridLayout.spacing.x);
		this.m_gridLayout.ourPadding = rectOffset;
		this.m_gridLayout.ForceRefresh();
	}

	// Token: 0x0600360A RID: 13834 RVA: 0x000FCBD0 File Offset: 0x000FAFD0
	protected void ConnectHorizontal(Selectable _left, Selectable _right)
	{
		Navigation navigation = _left.navigation;
		Navigation navigation2 = _right.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		navigation2.mode = Navigation.Mode.Explicit;
		navigation.selectOnRight = _right;
		navigation2.selectOnLeft = _left;
		_left.navigation = navigation;
		_right.navigation = navigation2;
	}

	// Token: 0x0600360B RID: 13835 RVA: 0x000FCC1C File Offset: 0x000FB01C
	protected void RecenterMenu()
	{
		float cellWidthWithSpacing = this.GetCellWidthWithSpacing();
		float num = (float)this.m_gridLayout.padding.left;
		while (num > cellWidthWithSpacing || num < -cellWidthWithSpacing)
		{
			if (this.m_gridLayout.padding.left < 0)
			{
				this.MoveFrontToEnd();
				num += cellWidthWithSpacing;
			}
			else
			{
				this.MoveEndToFront();
				num -= cellWidthWithSpacing;
			}
		}
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x000FCC88 File Offset: 0x000FB088
	protected CarouselButton GetButtonInFocusArea()
	{
		CarouselButton[] componentsInChildren = base.GetComponentsInChildren<CarouselButton>();
		float num = float.MaxValue;
		int num2 = -1;
		Camera main = Camera.main;
		Vector3 b = main.WorldToScreenPoint(base.transform.position);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Vector3 a = main.WorldToScreenPoint(componentsInChildren[i].transform.position);
			float sqrMagnitude = (a - b).sqrMagnitude;
			if (num2 == -1 || num > sqrMagnitude)
			{
				num2 = i;
				num = sqrMagnitude;
			}
		}
		if (num2 < 0)
		{
			num2 = componentsInChildren.Length / 2;
		}
		return componentsInChildren[num2];
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x000FCD24 File Offset: 0x000FB124
	protected IEnumerator LerpToButton(CarouselButton _carouselButton, bool _doInstant = false)
	{
		if (_carouselButton == null)
		{
			yield break;
		}
		if (!_doInstant)
		{
			yield return new WaitForEndOfFrame();
		}
		this.m_currentButton = _carouselButton;
		int idx = 0;
		CarouselButton[] buttons = base.GetComponentsInChildren<CarouselButton>();
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i] == _carouselButton)
			{
				idx = i;
			}
			this.SetInteractible(buttons[i]);
		}
		this.m_gridLayout.childAlignment = TextAnchor.MiddleLeft;
		float initLeftPadding = (float)this.m_gridLayout.padding.left;
		RectOffset newPadding = new RectOffset(this.m_gridLayout.padding.left, this.m_gridLayout.padding.right, this.m_gridLayout.padding.top, this.m_gridLayout.padding.bottom);
		float cellWidth = this.GetCellWidthWithSpacing();
		float finalLeftPadding = this.m_rectTransform.rect.width * 0.5f - this.m_gridLayout.cellSize.x * 0.5f - (float)idx * cellWidth;
		float deltaLeft = 0f;
		float m_progress = 0f;
		if (this.m_lerpTime > 0f && !_doInstant)
		{
			while (m_progress < 1f)
			{
				m_progress = Mathf.Min(m_progress + Time.deltaTime / this.m_lerpTime, 1f);
				deltaLeft += Mathf.Lerp(initLeftPadding, finalLeftPadding, m_progress) - (float)newPadding.left;
				if (initLeftPadding < finalLeftPadding)
				{
					while (deltaLeft > cellWidth * this.m_adjustmentDistance)
					{
						this.MoveEndToFront();
						deltaLeft -= cellWidth;
						initLeftPadding -= cellWidth;
						finalLeftPadding -= cellWidth;
					}
				}
				else
				{
					while (deltaLeft < -cellWidth * this.m_adjustmentDistance)
					{
						this.MoveFrontToEnd();
						deltaLeft += cellWidth;
						initLeftPadding += cellWidth;
						finalLeftPadding += cellWidth;
					}
				}
				newPadding.left = Mathf.RoundToInt(Mathf.Lerp(initLeftPadding, finalLeftPadding, m_progress));
				this.m_gridLayout.ourPadding = newPadding;
				this.m_gridLayout.ForceRefresh();
				yield return null;
			}
		}
		else
		{
			newPadding.left = Mathf.RoundToInt(finalLeftPadding);
			this.m_gridLayout.ourPadding = newPadding;
			this.m_gridLayout.ForceRefresh();
		}
		this.OnButtonFocusChanged(_carouselButton);
		this.m_buttonLerp = null;
		this.RecenterMenu();
		yield break;
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x000FCD4D File Offset: 0x000FB14D
	protected virtual void OnButtonFocusChanged(CarouselButton _button)
	{
	}

	// Token: 0x0600360F RID: 13839 RVA: 0x000FCD50 File Offset: 0x000FB150
	protected IEnumerator DeselectMenu()
	{
		yield return new WaitForEndOfFrame();
		foreach (CarouselButton carouselButton in base.GetComponentsInChildren<CarouselButton>())
		{
			if (carouselButton == this.m_currentButton)
			{
				carouselButton.Button.interactable = true;
			}
			else
			{
				carouselButton.Button.interactable = this.IsButtonInteractable(carouselButton);
			}
		}
		this.m_deselectMenu = null;
		yield break;
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x000FCD6C File Offset: 0x000FB16C
	protected float GetCellWidthWithSpacing()
	{
		return this.m_gridLayout.cellSize.x + this.m_gridLayout.spacing.x;
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x000FCDA0 File Offset: 0x000FB1A0
	protected void DisallowButton(CarouselButton _button)
	{
		this.m_carouselButtons = this.m_carouselButtons.AllRemoved_Predicate((CarouselButton x) => x == _button);
		if (_button != null)
		{
			_button.gameObject.SetActive(false);
		}
		CarouselButton carouselButton = null;
		for (int i = 0; i < this.m_carouselButtons.Length; i++)
		{
			CarouselButton carouselButton2 = this.m_carouselButtons[i];
			if (carouselButton != null)
			{
				this.ConnectHorizontal(carouselButton.Button, carouselButton2.Button);
			}
			carouselButton = carouselButton2;
		}
	}

	// Token: 0x04002B6F RID: 11119
	private CarouselButton[] m_carouselButtons;

	// Token: 0x04002B70 RID: 11120
	private T17GridLayoutGroup m_gridLayout;

	// Token: 0x04002B71 RID: 11121
	private RectTransform m_rectTransform;

	// Token: 0x04002B72 RID: 11122
	[SerializeField]
	[Range(0.01f, 0.6f)]
	protected float m_lerpTime = 1f;

	// Token: 0x04002B73 RID: 11123
	[SerializeField]
	[Range(0f, 1f)]
	private float m_adjustmentDistance = 0.8f;

	// Token: 0x04002B74 RID: 11124
	protected Coroutine m_buttonLerp;

	// Token: 0x04002B75 RID: 11125
	protected Coroutine m_deselectMenu;

	// Token: 0x04002B76 RID: 11126
	protected Coroutine m_dragCentering;

	// Token: 0x04002B77 RID: 11127
	protected CarouselButton m_currentButton;

	// Token: 0x04002B78 RID: 11128
	[SerializeField]
	private bool m_canInteractWithMouse = true;

	// Token: 0x04002B79 RID: 11129
	[SerializeField]
	protected float m_selectAfterDragDelay = 0.25f;

	// Token: 0x04002B7A RID: 11130
	private Vector2 m_dragPos;

	// Token: 0x04002B7B RID: 11131
	private bool m_isDragging;

	// Token: 0x04002B7D RID: 11133
	private PlayerManager m_playerManager;

	// Token: 0x02000AA8 RID: 2728
	// (Invoke) Token: 0x06003613 RID: 13843
	public delegate void CarouselButtonClickedEvent(CarouselButton button);
}
