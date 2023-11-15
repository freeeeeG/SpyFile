using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A72 RID: 2674
[Serializable]
public class BaseMenuBehaviour : MonoBehaviour, IMenuEventDelegate
{
	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x060034DB RID: 13531 RVA: 0x0006AEB4 File Offset: 0x000692B4
	public T17EventSystem CachedEventSystem
	{
		get
		{
			return this.m_CachedEventSystem;
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x060034DC RID: 13532 RVA: 0x0006AEBC File Offset: 0x000692BC
	public GamepadUser CurrentGamepadUser
	{
		get
		{
			return this.m_CurrentGamepadUser;
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x060034DD RID: 13533 RVA: 0x0006AEC4 File Offset: 0x000692C4
	public NavigateOnUICancel NavigateOnUICancel
	{
		get
		{
			return this.m_NavigateOnUICancel;
		}
	}

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x060034DE RID: 13534 RVA: 0x0006AECC File Offset: 0x000692CC
	// (remove) Token: 0x060034DF RID: 13535 RVA: 0x0006AF04 File Offset: 0x00069304
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MenuChangedHandler MenuChangedEvent;

	// Token: 0x060034E0 RID: 13536 RVA: 0x0006AF3A File Offset: 0x0006933A
	protected virtual void Awake()
	{
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x0006AF3C File Offset: 0x0006933C
	protected virtual void OnDestroy()
	{
		if (BaseMenuBehaviour.LastMenuThatCalledShow == this)
		{
			BaseMenuBehaviour.LastMenuThatCalledShow = null;
		}
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x0006AF54 File Offset: 0x00069354
	protected virtual void Start()
	{
		if (!this.m_bDidSingleTimeInitialize)
		{
			this.SingleTimeInitialize();
		}
	}

	// Token: 0x060034E3 RID: 13539 RVA: 0x0006AF68 File Offset: 0x00069368
	protected virtual void Update()
	{
		if (this.m_bLinkingUpdateRequested)
		{
			if (this.m_Waitframe <= 0)
			{
				this.m_bLinkingUpdateRequested = false;
				this.UpdateAndConstrainSelectableNavigation();
				this.m_Waitframe = 1;
			}
			else
			{
				this.m_Waitframe--;
			}
		}
		this.m_bShownThisFrame = false;
	}

	// Token: 0x060034E4 RID: 13540 RVA: 0x0006AFBA File Offset: 0x000693BA
	public void DoSingleTimeInitialize()
	{
		if (!this.m_bDidSingleTimeInitialize)
		{
			this.SingleTimeInitialize();
		}
	}

	// Token: 0x060034E5 RID: 13541 RVA: 0x0006AFCD File Offset: 0x000693CD
	protected virtual void SingleTimeInitialize()
	{
		this.m_bDidSingleTimeInitialize = true;
		this.m_NavigateOnUICancel = base.GetComponent<NavigateOnUICancel>();
		if (this.m_TransitionAnimator == null)
		{
			this.m_TransitionAnimator = base.GetComponent<Animator>();
		}
	}

	// Token: 0x060034E6 RID: 13542 RVA: 0x0006B000 File Offset: 0x00069400
	public virtual bool Show(GamepadUser gamepadUser, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		this.m_CurrentGamepadUser = gamepadUser;
		this.m_Parent = parent;
		if (this.m_CurrentGamepadUser == null)
		{
			this.m_CachedEventSystem = null;
		}
		else
		{
			this.m_CachedEventSystem = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(this.m_CurrentGamepadUser);
		}
		this.m_ObjectThatInvokedShow = invoker;
		if (!this.m_bDidSingleTimeInitialize)
		{
			this.SingleTimeInitialize();
		}
		if (this.m_bShouldBlockParentCancelHandler && this.m_NavigateOnUICancel != null)
		{
			this.m_bAllowCancelHandeling = true;
			BaseMenuBehaviour.AllowedCancelHandlers++;
		}
		if (this.m_ObjectThatInvokedShow != null)
		{
			this.m_bWasInvokerActive = this.m_ObjectThatInvokedShow.activeSelf;
		}
		if (parent != null)
		{
			parent.AddChildMenu(this);
			if (this.m_bShouldBlockParentCancelHandler && parent.m_NavigateOnUICancel != null)
			{
				parent.m_bAllowCancelHandeling = false;
				BaseMenuBehaviour.AllowedCancelHandlers--;
			}
		}
		if (!base.gameObject.activeInHierarchy)
		{
			if (hideInvoker && this.m_ObjectThatInvokedShow != null)
			{
				this.m_ObjectThatInvokedShow.SetActive(false);
			}
			base.gameObject.SetActive(true);
			if (this.MenuChangedEvent != null)
			{
				this.MenuChangedEvent();
			}
			if (this.m_Parent != null)
			{
				this.m_Parent.ChildMenuChanged(this, this);
			}
			BaseMenuBehaviour.LastMenuThatCalledShow = this;
			if (this.OnShow != null)
			{
				this.OnShow(this);
			}
			this.m_bShownThisFrame = true;
			return true;
		}
		return false;
	}

	// Token: 0x060034E7 RID: 13543 RVA: 0x0006B194 File Offset: 0x00069594
	public virtual bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (base.gameObject.activeSelf || isTabSwitch)
		{
			if (this.m_NavigateOnUICancel != null)
			{
				this.m_bAllowCancelHandeling = false;
				BaseMenuBehaviour.AllowedCancelHandlers--;
			}
			if (this.m_ObjectThatInvokedShow != null && restoreInvokerState)
			{
				this.m_ObjectThatInvokedShow.SetActive(this.m_bWasInvokerActive);
			}
			base.gameObject.SetActive(false);
			if (this.m_ChildMenus != null)
			{
				for (int i = this.m_ChildMenus.Count - 1; i >= 0; i--)
				{
					this.m_ChildMenus[i].Hide(restoreInvokerState, isTabSwitch);
				}
			}
			if (this.OnHide != null)
			{
				this.OnHide(this);
			}
			this.m_CurrentGamepadUser = null;
			this.m_CachedEventSystem = null;
			if (this.MenuChangedEvent != null)
			{
				this.MenuChangedEvent();
			}
			if (this.m_Parent != null)
			{
				this.m_Parent.ChildMenuChanged(this, this);
			}
			return true;
		}
		return false;
	}

	// Token: 0x060034E8 RID: 13544 RVA: 0x0006B2A8 File Offset: 0x000696A8
	protected virtual void AddChildMenu(BaseMenuBehaviour childMenu)
	{
		if (this.m_ChildMenus == null)
		{
			this.m_ChildMenus = new List<BaseMenuBehaviour>();
		}
		if (!this.m_ChildMenus.Contains(childMenu))
		{
			childMenu.OnHide = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Combine(childMenu.OnHide, new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnChildHide));
			this.m_ChildMenus.Add(childMenu);
		}
	}

	// Token: 0x060034E9 RID: 13545 RVA: 0x0006B310 File Offset: 0x00069710
	private void OnChildHide(BaseMenuBehaviour childMenu)
	{
		if (this.m_ChildMenus.Contains(childMenu))
		{
			childMenu.OnHide = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Remove(childMenu.OnHide, new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnChildHide));
			this.m_ChildMenus.Remove(childMenu);
			if (childMenu.m_bShouldBlockParentCancelHandler && this.m_NavigateOnUICancel != null)
			{
				this.m_bAllowCancelHandeling = true;
				BaseMenuBehaviour.AllowedCancelHandlers++;
			}
			BaseMenuBehaviour.LastMenuThatCalledShow = this;
		}
	}

	// Token: 0x060034EA RID: 13546 RVA: 0x0006B397 File Offset: 0x00069797
	public void GetChildMenus(out List<BaseMenuBehaviour> childList)
	{
		childList = this.m_ChildMenus;
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x0006B3A1 File Offset: 0x000697A1
	protected void RaiseMenuChangedEvent()
	{
		if (this.MenuChangedEvent != null)
		{
			this.MenuChangedEvent();
		}
		if (this.m_Parent != null)
		{
			this.m_Parent.ChildMenuChanged(this, null);
		}
	}

	// Token: 0x060034EC RID: 13548 RVA: 0x0006B3D7 File Offset: 0x000697D7
	public void ChildMenuChanged(IMenuEventDelegate sender = null, IMenuEventDelegate changedItem = null)
	{
		this.RaiseMenuChangedEvent();
		if (this.m_Parent != null)
		{
			this.m_Parent.ChildMenuChanged(this, changedItem);
		}
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x0006B3FD File Offset: 0x000697FD
	public void PlayForwardTransition()
	{
		this.PlayTransition(this.m_ForwardTransition);
	}

	// Token: 0x060034EE RID: 13550 RVA: 0x0006B40B File Offset: 0x0006980B
	public void PlayBackTransition()
	{
		this.PlayTransition(this.m_BackTransition);
	}

	// Token: 0x060034EF RID: 13551 RVA: 0x0006B419 File Offset: 0x00069819
	private void PlayTransition(string trigger)
	{
		if (this.m_TransitionAnimator != null)
		{
			this.m_TransitionAnimator.SetTrigger(trigger);
		}
	}

	// Token: 0x060034F0 RID: 13552 RVA: 0x0006B43D File Offset: 0x0006983D
	public virtual void ConfirmChangeFocus(BaseMenuBehaviour.ConfirmFocusCallback confirmCallback)
	{
		if (confirmCallback != null)
		{
			confirmCallback(true);
		}
	}

	// Token: 0x060034F1 RID: 13553 RVA: 0x0006B44C File Offset: 0x0006984C
	public void UpdateAndConstrainSelectableNavigation()
	{
		Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>(true);
		this.SetAllowedSelectables(new HashSet<Selectable>(componentsInChildren));
		foreach (Selectable selectable in componentsInChildren)
		{
			if (selectable.navigation.mode == Navigation.Mode.Explicit)
			{
				Navigation navigation = default(Navigation);
				navigation.mode = Navigation.Mode.Explicit;
				Selectable selectable2 = selectable.FindSelectable(selectable.transform.rotation * Vector3.down);
				if (selectable2 != null && this.m_AllowedSelectables.Contains(selectable2))
				{
					navigation.selectOnDown = selectable2;
				}
				Selectable selectable3 = selectable.FindSelectable(selectable.transform.rotation * Vector3.up);
				if (selectable3 != null && this.m_AllowedSelectables.Contains(selectable3))
				{
					navigation.selectOnUp = selectable3;
				}
				Selectable selectable4 = selectable.FindSelectable(selectable.transform.rotation * Vector3.left);
				if (selectable4 != null && this.m_AllowedSelectables.Contains(selectable4))
				{
					navigation.selectOnLeft = selectable4;
				}
				Selectable selectable5 = selectable.FindSelectable(selectable.transform.rotation * Vector3.right);
				if (selectable5 != null && this.m_AllowedSelectables.Contains(selectable5))
				{
					navigation.selectOnRight = selectable5;
				}
				selectable.navigation = navigation;
			}
		}
	}

	// Token: 0x060034F2 RID: 13554 RVA: 0x0006B5C5 File Offset: 0x000699C5
	public void SetAllowedSelectables(HashSet<Selectable> selectables)
	{
		this.m_AllowedSelectables = selectables;
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x0006B5CE File Offset: 0x000699CE
	public void AddAllowedSelectables(HashSet<Selectable> selectables)
	{
		this.m_AllowedSelectables.UnionWith(selectables);
	}

	// Token: 0x060034F4 RID: 13556 RVA: 0x0006B5DC File Offset: 0x000699DC
	public void AddAllowedSelectables(Selectable selectable)
	{
		this.m_AllowedSelectables.Add(selectable);
	}

	// Token: 0x060034F5 RID: 13557 RVA: 0x0006B5EB File Offset: 0x000699EB
	public string GetLegendText()
	{
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost() && !string.IsNullOrEmpty(this.m_legendTextClient))
		{
			return this.m_legendTextClient;
		}
		return this.m_legendText;
	}

	// Token: 0x060034F6 RID: 13558 RVA: 0x0006B620 File Offset: 0x00069A20
	public bool InvokeNavigateOnUICancel()
	{
		if (!this.m_bShownThisFrame && this.m_bAllowCancelHandeling && this.m_NavigateOnUICancel != null && this.m_CurrentGamepadUser != null && (this.m_CachedEventSystem == null || (this.m_CachedEventSystem != null && !this.m_CachedEventSystem.IsDisabled())) && !T17DialogBoxManager.HasDialogsForGamer(this.m_CurrentGamepadUser))
		{
			this.m_NavigateOnUICancel.m_DoThisOnUICancel.Invoke();
			return true;
		}
		return false;
	}

	// Token: 0x04002A56 RID: 10838
	protected T17EventSystem m_CachedEventSystem;

	// Token: 0x04002A57 RID: 10839
	protected GamepadUser m_CurrentGamepadUser;

	// Token: 0x04002A58 RID: 10840
	[HideInInspector]
	public Navigation m_BorderSelectables;

	// Token: 0x04002A59 RID: 10841
	[HideInInspector]
	public Sprite m_TabIcon;

	// Token: 0x04002A5A RID: 10842
	[HideInInspector]
	public Sprite m_TabIconDisabled;

	// Token: 0x04002A5B RID: 10843
	public string m_legendText;

	// Token: 0x04002A5C RID: 10844
	public string m_legendTextClient;

	// Token: 0x04002A5D RID: 10845
	public string m_legendTextInSession;

	// Token: 0x04002A5E RID: 10846
	public Animator m_TransitionAnimator;

	// Token: 0x04002A5F RID: 10847
	public string m_BackTransition = "TransitionBack";

	// Token: 0x04002A60 RID: 10848
	public string m_ForwardTransition = "TransitionForward";

	// Token: 0x04002A61 RID: 10849
	public bool m_bMenuIsEnabled = true;

	// Token: 0x04002A63 RID: 10851
	public BaseMenuBehaviour.BaseMenuBehaviourEvent OnHide;

	// Token: 0x04002A64 RID: 10852
	public BaseMenuBehaviour.BaseMenuBehaviourEvent OnShow;

	// Token: 0x04002A65 RID: 10853
	protected HashSet<Selectable> m_AllowedSelectables = new HashSet<Selectable>();

	// Token: 0x04002A66 RID: 10854
	protected List<BaseMenuBehaviour> m_ChildMenus;

	// Token: 0x04002A67 RID: 10855
	protected NavigateOnUICancel m_NavigateOnUICancel;

	// Token: 0x04002A68 RID: 10856
	protected BaseMenuBehaviour m_Parent;

	// Token: 0x04002A69 RID: 10857
	protected GameObject m_ObjectThatInvokedShow;

	// Token: 0x04002A6A RID: 10858
	protected bool m_bWasInvokerActive = true;

	// Token: 0x04002A6B RID: 10859
	protected bool m_bLinkingUpdateRequested;

	// Token: 0x04002A6C RID: 10860
	protected int m_Waitframe = 1;

	// Token: 0x04002A6D RID: 10861
	[HideInInspector]
	public bool m_bShouldBlockParentCancelHandler = true;

	// Token: 0x04002A6E RID: 10862
	private bool m_bAllowCancelHandeling;

	// Token: 0x04002A6F RID: 10863
	private bool m_bDidSingleTimeInitialize;

	// Token: 0x04002A70 RID: 10864
	private bool m_bShownThisFrame;

	// Token: 0x04002A71 RID: 10865
	public static int AllowedCancelHandlers;

	// Token: 0x04002A72 RID: 10866
	public static BaseMenuBehaviour LastMenuThatCalledShow;

	// Token: 0x02000A73 RID: 2675
	// (Invoke) Token: 0x060034F9 RID: 13561
	public delegate void BaseMenuBehaviourEvent(BaseMenuBehaviour menu);

	// Token: 0x02000A74 RID: 2676
	// (Invoke) Token: 0x060034FD RID: 13565
	public delegate void ConfirmFocusCallback(bool canChangeFocus);
}
