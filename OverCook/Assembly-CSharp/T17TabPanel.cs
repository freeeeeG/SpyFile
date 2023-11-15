using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B7A RID: 2938
public class T17TabPanel : BaseMenuBehaviour
{
	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06003BDF RID: 15327 RVA: 0x0011E109 File Offset: 0x0011C509
	public int CurrentTabIndex
	{
		get
		{
			return this.m_CurrentTabIndex;
		}
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x0011E111 File Offset: 0x0011C511
	public int PreviousTabIndex
	{
		get
		{
			return this.m_OldTabIndex;
		}
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x0011E119 File Offset: 0x0011C519
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x0011E124 File Offset: 0x0011C524
	private void GetButtonsInDirectChildren()
	{
		int childCount = base.transform.childCount;
		List<T17Button> list = new List<T17Button>();
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			T17Button component = child.GetComponent<T17Button>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		this.m_Buttons = list.ToArray();
	}

	// Token: 0x06003BE3 RID: 15331 RVA: 0x0011E18C File Offset: 0x0011C58C
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		this.GetButtonsInDirectChildren();
		this.m_ButtonChildImages = new T17Image[this.m_Buttons.Length];
		for (int i = 0; i < this.m_Buttons.Length; i++)
		{
			T17Button t17Button = this.m_Buttons[i];
			t17Button.OnButtonSelect = new T17Button.T17ButtonDelegate(this.OnTabSelected);
			t17Button.OnButtonPointerEnter = new T17Button.T17ButtonDelegate(this.OnTabSelected);
			if (t17Button.transform.childCount > 0)
			{
				this.m_ButtonChildImages[i] = t17Button.transform.GetChild(0).GetComponent<T17Image>();
			}
		}
		if (this.m_bTabEntriesSetExternally)
		{
			this.m_MenuBodies = new BaseMenuBehaviour[this.m_Buttons.Length];
		}
		else
		{
			this.m_MaxTabIndex = this.m_Buttons.Length - 1;
		}
		this.m_CurrentButtonNormalColors = this.m_Buttons[this.m_CurrentTabIndex].colors;
		this.m_CurrentButtonHighlightColors = this.m_Buttons[this.m_CurrentTabIndex].colors;
		this.m_CurrentButtonSpriteState = this.m_Buttons[this.m_CurrentTabIndex].spriteState;
		this.m_CurrentButtonNormalSprite = ((T17Image)this.m_Buttons[this.m_CurrentTabIndex].targetGraphic).sprite;
		for (int j = 0; j < this.m_MenuBodies.Length; j++)
		{
			if (this.m_MenuBodies[j] != null)
			{
				this.m_MenuBodies[j].Hide(true, false);
			}
		}
		this.SetTabIndex(this.m_CurrentTabIndex, null);
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x0011E30E File Offset: 0x0011C70E
	protected override void Update()
	{
		base.Update();
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x0011E318 File Offset: 0x0011C718
	public void SetMenuBodies(List<BaseMenuBehaviour> menus)
	{
		if (menus != null && menus.Count > 0)
		{
			for (int i = 0; i < this.m_Buttons.Length; i++)
			{
				if (this.m_Buttons[i] != null)
				{
					this.m_Buttons[i].gameObject.SetActive(false);
					Animator component = this.m_Buttons[i].GetComponent<Animator>();
					if (component != null)
					{
						component.SetBool(T17TabPanel.m_ButtonAnimatorHoldBool, false);
						component.SetTrigger(this.m_Buttons[this.m_CurrentTabIndex].animationTriggers.normalTrigger);
					}
				}
			}
			for (int j = 0; j < this.m_MenuBodies.Length; j++)
			{
				if (this.m_MenuBodies[j] != null)
				{
					this.m_MenuBodies[j].Hide(true, true);
				}
			}
			for (int k = 0; k < menus.Count; k++)
			{
				if (k < this.m_Buttons.Length)
				{
					this.m_MenuBodies[k] = menus[k];
					if (this.m_Buttons[k] != null)
					{
						this.m_Buttons[k].gameObject.SetActive(true);
						this.SetButtonInteractableState(k);
					}
				}
			}
			this.m_MaxTabIndex = this.m_Buttons.Length - 1;
		}
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x0011E468 File Offset: 0x0011C868
	private void SetButtonInteractableState(int index)
	{
		if (this.m_Buttons == null || this.m_MenuBodies == null)
		{
			return;
		}
		if (index < 0 || index >= this.m_Buttons.Length || index >= this.m_MenuBodies.Length)
		{
			return;
		}
		if (this.m_Buttons[index] == null || this.m_MenuBodies[index] == null)
		{
			return;
		}
		if (this.m_Buttons[index].m_lockImage != null)
		{
			this.m_Buttons[index].m_lockImage.gameObject.SetActive(!this.m_MenuBodies[index].m_bMenuIsEnabled);
		}
		this.m_Buttons[index].interactable = this.m_MenuBodies[index].m_bMenuIsEnabled;
		if (this.m_ButtonChildImages != null && index <= this.m_ButtonChildImages.Length && this.m_ButtonChildImages[index] != null)
		{
			if (this.m_MenuBodies[index].m_bMenuIsEnabled)
			{
				this.m_ButtonChildImages[index].sprite = this.m_MenuBodies[index].m_TabIcon;
			}
			else
			{
				this.m_ButtonChildImages[index].sprite = this.m_MenuBodies[index].m_TabIconDisabled;
			}
		}
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x0011E5A7 File Offset: 0x0011C9A7
	public void SetTabIndex(int index, Action<bool> onCompleted = null)
	{
		if (index >= 0 && index < this.m_Buttons.Length)
		{
			this.OnTabButtonClicked(this.m_Buttons[index], onCompleted);
		}
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x0011E5D0 File Offset: 0x0011C9D0
	public void SetMenuEnabled(int index, bool enabled)
	{
		if (this.m_MenuBodies != null && index >= 0 && index < this.m_MenuBodies.Length && this.m_MenuBodies[index] != null)
		{
			this.m_MenuBodies[index].m_bMenuIsEnabled = enabled;
			this.SetButtonInteractableState(index);
		}
	}

	// Token: 0x06003BE9 RID: 15337 RVA: 0x0011E625 File Offset: 0x0011CA25
	public bool CheckMenuEnabled(int index)
	{
		return this.m_MenuBodies != null && index >= 0 && index < this.m_MenuBodies.Length && this.m_MenuBodies[index].m_bMenuIsEnabled;
	}

	// Token: 0x06003BEA RID: 15338 RVA: 0x0011E658 File Offset: 0x0011CA58
	public void OnTabSelected(T17Button button)
	{
		if (button.interactable)
		{
			if (this.m_MenuBodies == null || this.m_CurrentTabIndex >= this.m_MenuBodies.Length || this.m_MenuBodies[this.m_CurrentTabIndex] == null)
			{
				this._OnTabButtonSelected(button);
			}
			else
			{
				this.m_MenuBodies[this.m_CurrentTabIndex].ConfirmChangeFocus(delegate(bool canChangeFocus)
				{
					if (canChangeFocus)
					{
						this._OnTabButtonSelected(button);
					}
				});
			}
		}
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x0011E6F0 File Offset: 0x0011CAF0
	public void OnTabButtonClicked(T17Button button, Action<bool> onCompleted = null)
	{
		if (this.m_MenuBodies == null || this.m_CurrentTabIndex >= this.m_MenuBodies.Length || this.m_MenuBodies[this.m_CurrentTabIndex] == null)
		{
			this._OnTabButtonSelected(button);
			if (onCompleted != null)
			{
				onCompleted(true);
			}
		}
		else
		{
			this.m_MenuBodies[this.m_CurrentTabIndex].ConfirmChangeFocus(delegate(bool canChangeFocus)
			{
				if (canChangeFocus)
				{
					this._OnTabButtonSelected(button);
					if (onCompleted != null)
					{
						onCompleted(true);
					}
				}
				else if (onCompleted != null)
				{
					onCompleted(false);
				}
			});
		}
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x0011E794 File Offset: 0x0011CB94
	private void _OnTabButtonSelected(T17Button button)
	{
		T17FrontendFlow instance = T17FrontendFlow.Instance;
		if (instance != null)
		{
			instance.FocusOnMainMenu();
		}
		int i;
		for (i = 0; i < this.m_Buttons.Length; i++)
		{
			if (this.m_Buttons[i] == button)
			{
				break;
			}
		}
		T17Button t17Button = this.m_Buttons[this.m_CurrentTabIndex];
		Animator animator = t17Button.animator;
		if (animator != null)
		{
			animator.SetBool(T17TabPanel.m_ButtonAnimatorHoldBool, false);
			animator.ResetTrigger(t17Button.animationTriggers.highlightedTrigger);
		}
		if (this.m_CachedEventSystem != null && this.m_CachedEventSystem.currentSelectedGameObject != button.gameObject)
		{
			this.m_CachedEventSystem.SetSelectedGameObject(null);
			this.m_CachedEventSystem.SetSelectedGameObject(button.gameObject);
		}
		if (this.m_bHasEventsEnabled && i < this.m_MenuDelegates.Length && this.m_MenuDelegates[i] != null)
		{
			this.m_MenuDelegates[i].Invoke();
		}
		this.SwitchToTab(i);
		if (this.m_bKeepSelectedTabHighlighted)
		{
			if (this.m_Buttons[this.m_OldTabIndex].transition == Selectable.Transition.ColorTint)
			{
				this.m_Buttons[this.m_OldTabIndex].colors = this.m_CurrentButtonNormalColors;
			}
			else if (this.m_Buttons[this.m_OldTabIndex].transition == Selectable.Transition.SpriteSwap)
			{
				((T17Image)this.m_Buttons[this.m_OldTabIndex].targetGraphic).sprite = this.m_CurrentButtonNormalSprite;
			}
			if (button.transition == Selectable.Transition.ColorTint)
			{
				this.m_CurrentButtonNormalColors = button.colors;
				this.m_CurrentButtonHighlightColors = button.colors;
				this.m_CurrentButtonHighlightColors.normalColor = this.m_CurrentButtonHighlightColors.highlightedColor;
				this.m_CurrentButtonHighlightColors.highlightedColor = Color.cyan;
				this.m_Buttons[this.m_CurrentTabIndex].colors = this.m_CurrentButtonHighlightColors;
			}
			else if (button.transition == Selectable.Transition.SpriteSwap)
			{
				this.m_CurrentButtonSpriteState = button.spriteState;
				this.m_CurrentButtonNormalSprite = ((T17Image)button.targetGraphic).sprite;
				((T17Image)button.targetGraphic).sprite = this.m_CurrentButtonSpriteState.pressedSprite;
			}
			else if (button.transition == Selectable.Transition.Animation)
			{
				button.animator.SetBool(T17TabPanel.m_ButtonAnimatorHoldBool, true);
				this.m_Buttons[this.m_CurrentTabIndex].animator.SetTrigger(this.m_Buttons[this.m_CurrentTabIndex].animationTriggers.highlightedTrigger);
				this.m_Buttons[this.m_CurrentTabIndex].animator.ResetTrigger(this.m_Buttons[this.m_CurrentTabIndex].animationTriggers.normalTrigger);
			}
		}
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x0011EA54 File Offset: 0x0011CE54
	private void SwitchToTab(int tabIndex)
	{
		if (this.m_MenuBodies[this.m_CurrentTabIndex] != null && this.m_CurrentTabIndex != tabIndex)
		{
			this.m_MenuBodies[this.m_CurrentTabIndex].Hide(true, true);
		}
		if (tabIndex < this.m_MenuBodies.Length && this.m_MenuBodies[tabIndex] != null)
		{
			this.m_OldTabIndex = this.m_CurrentTabIndex;
			this.m_CurrentTabIndex = tabIndex;
			if (this.m_MenuBodies[this.m_CurrentTabIndex] != null)
			{
				this.m_MenuBodies[this.m_CurrentTabIndex].Show(this.m_CurrentGamepadUser, this, null, true);
				this.SetNavigationLinking(false);
			}
		}
		else
		{
			this.m_OldTabIndex = this.m_CurrentTabIndex;
			this.m_CurrentTabIndex = tabIndex;
		}
		if (this.m_TabSplitterSprites != null && this.m_TabSplitter != null && this.m_CurrentTabIndex < this.m_TabSplitterSprites.Length)
		{
			this.m_TabSplitter.sprite = this.m_TabSplitterSprites[this.m_CurrentTabIndex];
		}
		if (this.m_TabTitleTags != null && this.m_TabTitle != null && this.m_CurrentTabIndex < this.m_TabTitleTags.Length)
		{
			this.m_TabTitle.m_PlaceholderText = this.m_TabTitleTags[this.m_CurrentTabIndex];
			this.m_TabTitle.SetNewLocalizationTag(this.m_TabTitleTags[this.m_CurrentTabIndex]);
		}
		IMenuEventDelegate componentInParent = base.GetComponentInParent<IMenuEventDelegate>();
		if (componentInParent != null)
		{
			componentInParent.ChildMenuChanged(null, null);
		}
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x0011EBDC File Offset: 0x0011CFDC
	private void SetNavigationLinking(bool selectFirstItemOnBody)
	{
		if (this.m_MenuBodies == null || this.m_CurrentTabIndex >= this.m_MenuBodies.Length || this.m_MenuBodies[this.m_CurrentTabIndex] == null)
		{
			return;
		}
		GameObject selectedGameObject = null;
		switch (this.m_RelativePositionToBodies)
		{
		case T17TabPanel.RelativePosition.Left:
			if (this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnLeft != null)
			{
				if (this.m_bAllowDirectNavigation)
				{
					Navigation navigation = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnLeft.navigation;
					navigation.selectOnLeft = this.m_Buttons[this.m_CurrentTabIndex];
					this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnLeft.navigation = navigation;
					navigation = this.m_Buttons[this.m_CurrentTabIndex].navigation;
					navigation.selectOnRight = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnLeft;
					this.m_Buttons[this.m_CurrentTabIndex].navigation = navigation;
				}
				selectedGameObject = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnLeft.gameObject;
			}
			goto IL_3DF;
		case T17TabPanel.RelativePosition.Right:
			if (this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnRight != null)
			{
				if (this.m_bAllowDirectNavigation)
				{
					Navigation navigation = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnRight.navigation;
					navigation.selectOnRight = this.m_Buttons[this.m_CurrentTabIndex];
					this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnRight.navigation = navigation;
					navigation = this.m_Buttons[this.m_CurrentTabIndex].navigation;
					navigation.selectOnLeft = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnRight;
					this.m_Buttons[this.m_CurrentTabIndex].navigation = navigation;
				}
				selectedGameObject = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnRight.gameObject;
			}
			goto IL_3DF;
		case T17TabPanel.RelativePosition.Bottom:
			if (this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnDown != null)
			{
				if (this.m_bAllowDirectNavigation)
				{
					Navigation navigation = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnDown.navigation;
					navigation.selectOnDown = this.m_Buttons[this.m_CurrentTabIndex];
					this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnDown.navigation = navigation;
					navigation = this.m_Buttons[this.m_CurrentTabIndex].navigation;
					navigation.selectOnUp = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnDown;
					this.m_Buttons[this.m_CurrentTabIndex].navigation = navigation;
				}
				selectedGameObject = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnDown.gameObject;
			}
			goto IL_3DF;
		}
		if (this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnUp != null)
		{
			if (this.m_bAllowDirectNavigation)
			{
				Navigation navigation = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnUp.navigation;
				navigation.selectOnUp = this.m_Buttons[this.m_CurrentTabIndex];
				this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnUp.navigation = navigation;
				navigation = this.m_Buttons[this.m_CurrentTabIndex].navigation;
				navigation.selectOnDown = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnUp;
				this.m_Buttons[this.m_CurrentTabIndex].navigation = navigation;
			}
			selectedGameObject = this.m_MenuBodies[this.m_CurrentTabIndex].m_BorderSelectables.selectOnUp.gameObject;
		}
		IL_3DF:
		if ((selectFirstItemOnBody || !this.m_bAllowDirectNavigation) && this.m_CachedEventSystem != null && this.m_Buttons[this.m_CurrentTabIndex] != null)
		{
			this.m_CachedEventSystem.SetSelectedGameObject(null);
			this.m_CachedEventSystem.SetSelectedGameObject(selectedGameObject);
		}
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x0011F01C File Offset: 0x0011D41C
	private void OnEnable()
	{
		if (EventSystem.current != null && this.m_Buttons[this.m_CurrentTabIndex] != null && this.m_Buttons[this.m_CurrentTabIndex].transition == Selectable.Transition.Animation)
		{
			if (this.m_bKeepSelectedTabHighlighted)
			{
				this.m_Buttons[this.m_CurrentTabIndex].animator.SetBool(T17TabPanel.m_ButtonAnimatorHoldBool, true);
			}
			this.m_Buttons[this.m_CurrentTabIndex].animator.SetTrigger(this.m_Buttons[this.m_CurrentTabIndex].animationTriggers.highlightedTrigger);
		}
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x0011F0C0 File Offset: 0x0011D4C0
	public BaseMenuBehaviour GetCurrentPage()
	{
		if (this.m_MenuBodies == null || this.m_CurrentTabIndex >= this.m_MenuBodies.Length || this.m_MenuBodies[this.m_CurrentTabIndex] == null)
		{
			return null;
		}
		return this.m_MenuBodies[this.m_CurrentTabIndex];
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x0011F114 File Offset: 0x0011D514
	public void AttemptToSetTabIndex(int index, Action<bool> onCompleted = null)
	{
		int num = index;
		if (!this.CheckMenuEnabled(num))
		{
			int num2 = this.m_MenuBodies.Length;
			bool flag = true;
			do
			{
				if (num <= 0)
				{
					flag = false;
					num = index;
				}
				num += ((!flag) ? 1 : -1);
				num2--;
			}
			while (!this.CheckMenuEnabled(num) && num2 > 0);
		}
		if (this.CheckMenuEnabled(num))
		{
			this.SetTabIndex(num, onCompleted);
		}
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x0011F180 File Offset: 0x0011D580
	public void SelectPreviousTab()
	{
		int num = this.m_CurrentTabIndex - 1;
		if (num < 0)
		{
			num = 0;
		}
		if (num != this.m_CurrentTabIndex)
		{
			this.SetTabIndex(num, null);
		}
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x0011F1B4 File Offset: 0x0011D5B4
	public void SelectNextTab()
	{
		int num = this.m_CurrentTabIndex + 1;
		if (num > this.m_MaxTabIndex)
		{
			num = this.m_MaxTabIndex;
		}
		if (num != this.m_CurrentTabIndex)
		{
			this.SetTabIndex(num, null);
		}
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x0011F1F4 File Offset: 0x0011D5F4
	public void CollapseCurrentTab()
	{
		if (this.m_CurrentTabIndex < this.m_MenuBodies.Length && this.m_Buttons[this.m_CurrentTabIndex] != null)
		{
			this.m_MenuBodies[this.m_CurrentTabIndex].Hide(false, false);
			if (this.m_Buttons[this.m_CurrentTabIndex].animator != null)
			{
				this.m_Buttons[this.m_CurrentTabIndex].animator.SetBool(T17TabPanel.m_ButtonAnimatorHoldBool, false);
				this.m_Buttons[this.m_CurrentTabIndex].animator.SetTrigger(this.m_Buttons[this.m_CurrentTabIndex].animationTriggers.normalTrigger);
			}
		}
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x0011F2A9 File Offset: 0x0011D6A9
	public void ExpandCurrentTab()
	{
		this.AttemptToSetTabIndex(this.m_CurrentTabIndex, null);
	}

	// Token: 0x0400309B RID: 12443
	[HideInInspector]
	public T17Button[] m_Buttons;

	// Token: 0x0400309C RID: 12444
	[HideInInspector]
	public BaseMenuBehaviour[] m_MenuBodies;

	// Token: 0x0400309D RID: 12445
	[HideInInspector]
	public UnityEvent[] m_MenuDelegates;

	// Token: 0x0400309E RID: 12446
	[HideInInspector]
	public bool m_bHasEventsEnabled;

	// Token: 0x0400309F RID: 12447
	[HideInInspector]
	public bool m_bKeepSelectedTabHighlighted = true;

	// Token: 0x040030A0 RID: 12448
	[HideInInspector]
	public bool m_bTabEntriesSetExternally;

	// Token: 0x040030A1 RID: 12449
	[HideInInspector]
	public Sprite[] m_TabSplitterSprites;

	// Token: 0x040030A2 RID: 12450
	[HideInInspector]
	public string[] m_TabTitleTags;

	// Token: 0x040030A3 RID: 12451
	public T17TabPanel.RelativePosition m_RelativePositionToBodies;

	// Token: 0x040030A4 RID: 12452
	[HideInInspector]
	public bool m_bAllowDirectNavigation;

	// Token: 0x040030A5 RID: 12453
	[HideInInspector]
	public bool m_bAllowIndirectNavigation = true;

	// Token: 0x040030A6 RID: 12454
	[HideInInspector]
	public bool m_bAutoInvokeOnTabSelection;

	// Token: 0x040030A7 RID: 12455
	[HideInInspector]
	public string m_PreviousTabInputAction = "CycleLeft";

	// Token: 0x040030A8 RID: 12456
	[HideInInspector]
	public string m_NextTabInputAction = "CycleRight";

	// Token: 0x040030A9 RID: 12457
	public static readonly int m_ButtonAnimatorHoldBool = Animator.StringToHash("HoldSelected");

	// Token: 0x040030AA RID: 12458
	public Color m_TabDisabledColour = Color.black;

	// Token: 0x040030AB RID: 12459
	public T17Text m_TabTitle;

	// Token: 0x040030AC RID: 12460
	public T17Image m_TabSplitter;

	// Token: 0x040030AD RID: 12461
	private int m_OldTabIndex;

	// Token: 0x040030AE RID: 12462
	private int m_CurrentTabIndex;

	// Token: 0x040030AF RID: 12463
	private int m_MaxTabIndex;

	// Token: 0x040030B0 RID: 12464
	private ColorBlock m_CurrentButtonNormalColors;

	// Token: 0x040030B1 RID: 12465
	private ColorBlock m_CurrentButtonHighlightColors;

	// Token: 0x040030B2 RID: 12466
	private SpriteState m_CurrentButtonSpriteState;

	// Token: 0x040030B3 RID: 12467
	private Sprite m_CurrentButtonNormalSprite;

	// Token: 0x040030B4 RID: 12468
	private T17Image[] m_ButtonChildImages;

	// Token: 0x02000B7B RID: 2939
	public enum RelativePosition
	{
		// Token: 0x040030B6 RID: 12470
		Top,
		// Token: 0x040030B7 RID: 12471
		Left,
		// Token: 0x040030B8 RID: 12472
		Right,
		// Token: 0x040030B9 RID: 12473
		Bottom
	}
}
