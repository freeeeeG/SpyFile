using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B73 RID: 2931
public class T17NavigableGrid : FrontendMenuBehaviour
{
	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06003B9F RID: 15263 RVA: 0x0011BA69 File Offset: 0x00119E69
	protected bool isAltLayout
	{
		get
		{
			return this.m_bAltElementLayout;
		}
	}

	// Token: 0x06003BA0 RID: 15264 RVA: 0x0011BA74 File Offset: 0x00119E74
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		if (this.m_ContentParent != null)
		{
			T17GridLayoutGroup component = this.m_ContentParent.GetComponent<T17GridLayoutGroup>();
			if (component != null)
			{
				this.m_AltLayoutSize.x = component.m_CellCountX;
				this.m_AltLayoutSize.y = component.m_CellCountY;
				this.m_AltLayoutIsVertical = (component.startAxis == T17GridLayoutGroup.Axis.Vertical);
				this.m_bAltElementLayout = true;
			}
			if (!this.m_ContentIsDynamic)
			{
				for (int i = 0; i < this.m_ContentParent.transform.childCount; i++)
				{
					this.AddNewObject(this.m_ContentParent.transform.GetChild(i).gameObject);
				}
			}
			if (component != null)
			{
				component.ForceRefresh();
			}
		}
		if (this.m_BorderSelectables.selectOnUp != null)
		{
			T17Image component2 = this.m_BorderSelectables.selectOnUp.GetComponent<T17Image>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
		}
		if (this.m_BorderSelectables.selectOnLeft != null)
		{
			T17Image component3 = this.m_BorderSelectables.selectOnLeft.GetComponent<T17Image>();
			if (component3 != null)
			{
				component3.enabled = false;
			}
		}
		if (this.m_BorderSelectables.selectOnRight != null)
		{
			T17Image component4 = this.m_BorderSelectables.selectOnRight.GetComponent<T17Image>();
			if (component4 != null)
			{
				component4.enabled = false;
			}
		}
		if (this.m_BorderSelectables.selectOnDown != null)
		{
			T17Image component5 = this.m_BorderSelectables.selectOnDown.GetComponent<T17Image>();
			if (component5 != null)
			{
				component5.enabled = false;
			}
		}
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x0011BC30 File Offset: 0x0011A030
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		bool result = base.Show(currentGamer, parent, invoker, hideInvoker);
		if (this.m_ContentIsDynamic)
		{
			if (this.m_ContentParent != null)
			{
				T17GridLayoutGroup component = this.m_ContentParent.GetComponent<T17GridLayoutGroup>();
				if (component != null)
				{
					this.m_AltLayoutSize.x = component.m_CellCountX;
					this.m_AltLayoutSize.y = component.m_CellCountY;
				}
			}
			this.m_ContentSelectables.Clear();
			for (int i = 0; i < this.m_ContentParent.transform.childCount; i++)
			{
				GameObject gameObject = this.m_ContentParent.transform.GetChild(i).gameObject;
				if (gameObject.activeSelf)
				{
					this.AddNewObject(gameObject);
				}
			}
			if (this.m_bAltElementLayout)
			{
				T17GridLayoutGroup component2 = this.m_ContentParent.GetComponent<T17GridLayoutGroup>();
				if (component2 != null)
				{
					component2.ForceRefresh();
				}
			}
		}
		return result;
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x0011BD24 File Offset: 0x0011A124
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		bool result = base.Hide(restoreInvokerState, isTabSwitch);
		if (this.m_ContentIsDynamic)
		{
			this.m_ContentSelectables.Clear();
		}
		return result;
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x0011BD54 File Offset: 0x0011A154
	protected override void Update()
	{
		base.Update();
		if (this.m_DataCache != null)
		{
			if (this.m_CachedEventSystem == null)
			{
				this.m_DataCache = null;
				return;
			}
			GameObject lastRequestedSelectedGameobject = this.m_CachedEventSystem.GetLastRequestedSelectedGameobject();
			this.m_CachedEventSystem.SetSelectedGameObject(null);
			if (this.m_DataCache == this.m_BorderSelectables.selectOnUp)
			{
				if (this.m_bContentElementSelected && this.m_BorderSelectables.selectOnUp.navigation.selectOnUp != null)
				{
					this.m_bContentElementSelected = false;
					this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnUp.navigation.selectOnUp.gameObject);
				}
				else if (!this.ReselectCurrent(ref this.m_CachedEventSystem))
				{
					GameObject gameObject = (!(this.m_BorderSelectables.selectOnUp.navigation.selectOnUp == null) && this.m_BorderSelectables.selectOnUp.navigation.selectOnUp.IsInteractable()) ? this.m_BorderSelectables.selectOnUp.navigation.selectOnUp.gameObject : null;
					if (gameObject == null && lastRequestedSelectedGameobject != null)
					{
						this.m_CachedEventSystem.SetSelectedGameObject(lastRequestedSelectedGameobject);
					}
					else
					{
						this.m_CachedEventSystem.SetSelectedGameObject(gameObject);
					}
				}
			}
			else if (this.m_DataCache == this.m_BorderSelectables.selectOnDown)
			{
				if (this.m_bContentElementSelected && this.m_BorderSelectables.selectOnDown.navigation.selectOnDown != null)
				{
					this.m_bContentElementSelected = false;
					this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnDown.navigation.selectOnDown.gameObject);
				}
				else if (!this.ReselectCurrent(ref this.m_CachedEventSystem))
				{
					GameObject selectedGameObject = (!(this.m_BorderSelectables.selectOnDown.navigation.selectOnDown == null) && this.m_BorderSelectables.selectOnDown.navigation.selectOnDown.IsInteractable()) ? this.m_BorderSelectables.selectOnDown.navigation.selectOnDown.gameObject : null;
					this.m_CachedEventSystem.SetSelectedGameObject(selectedGameObject);
				}
			}
			else if (this.m_DataCache == this.m_BorderSelectables.selectOnLeft)
			{
				if (this.m_bContentElementSelected && this.m_BorderSelectables.selectOnLeft.navigation.selectOnLeft != null)
				{
					this.m_bContentElementSelected = false;
					this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnLeft.navigation.selectOnLeft.gameObject);
				}
				else if (!this.ReselectCurrent(ref this.m_CachedEventSystem))
				{
					GameObject selectedGameObject2 = (!(this.m_BorderSelectables.selectOnLeft.navigation.selectOnLeft == null) && this.m_BorderSelectables.selectOnLeft.navigation.selectOnLeft.IsInteractable()) ? this.m_BorderSelectables.selectOnLeft.navigation.selectOnLeft.gameObject : null;
					this.m_CachedEventSystem.SetSelectedGameObject(selectedGameObject2);
				}
			}
			else if (this.m_DataCache == this.m_BorderSelectables.selectOnRight)
			{
				if (this.m_bContentElementSelected && this.m_BorderSelectables.selectOnRight.navigation.selectOnRight != null)
				{
					this.m_bContentElementSelected = false;
					this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnRight.navigation.selectOnRight.gameObject);
				}
				else if (!this.ReselectCurrent(ref this.m_CachedEventSystem))
				{
					GameObject selectedGameObject3 = (!(this.m_BorderSelectables.selectOnRight.navigation.selectOnRight == null) && this.m_BorderSelectables.selectOnRight.navigation.selectOnRight.IsInteractable()) ? this.m_BorderSelectables.selectOnRight.navigation.selectOnRight.gameObject : null;
					this.m_CachedEventSystem.SetSelectedGameObject(selectedGameObject3);
				}
			}
		}
		this.m_DataCache = null;
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x0011C214 File Offset: 0x0011A614
	public virtual void AddNewObject(GameObject newObject)
	{
		if (newObject == null)
		{
			return;
		}
		RectTransform component = newObject.GetComponent<RectTransform>();
		if (this.m_ContentSelectables.Contains(component))
		{
			return;
		}
		this.m_ContentSelectables.Add(component);
		newObject.transform.SetParent(this.m_ContentParent.transform);
		newObject.transform.localScale = Vector3.one;
		newObject.transform.localPosition = Vector3.zero;
		int currentIndex = this.m_ContentSelectables.Count - 1;
		Selectable sel = newObject.GetComponentInChildren<Selectable>(true);
		T17_UISelectDeselectEvents t17_UISelectDeselectEvents = newObject.GetComponentInChildren<T17_UISelectDeselectEvents>(true);
		if (sel == null)
		{
			sel = newObject.AddComponent<Selectable>();
		}
		if (t17_UISelectDeselectEvents == null)
		{
			t17_UISelectDeselectEvents = newObject.AddComponent<T17_UISelectDeselectEvents>();
		}
		if (t17_UISelectDeselectEvents != null)
		{
			t17_UISelectDeselectEvents.m_OnSelectEvent.AddListener(delegate(BaseEventData eventData)
			{
				this.OnElementSelected(sel, currentIndex);
			});
		}
		Navigation navigation = sel.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		if (!this.m_bAltElementLayout)
		{
			if (currentIndex == 0)
			{
				navigation.selectOnUp = this.m_BorderSelectables.selectOnUp;
			}
			else
			{
				Selectable component2 = this.m_ContentSelectables[currentIndex - 1].GetComponent<Selectable>();
				Navigation navigation2 = component2.navigation;
				navigation.selectOnUp = component2;
				navigation2.selectOnDown = sel;
				component2.navigation = navigation2;
			}
			navigation.selectOnLeft = this.m_BorderSelectables.selectOnLeft;
			navigation.selectOnRight = this.m_BorderSelectables.selectOnRight;
			navigation.selectOnDown = this.m_BorderSelectables.selectOnDown;
		}
		else
		{
			if (currentIndex == 0)
			{
				navigation.selectOnUp = this.m_BorderSelectables.selectOnUp;
				navigation.selectOnLeft = this.m_BorderSelectables.selectOnLeft;
			}
			else
			{
				RectTransform rectTransform = null;
				if (this.m_AltLayoutIsVertical)
				{
					if (currentIndex % this.m_AltLayoutSize.y > 0)
					{
						rectTransform = this.m_ContentSelectables[currentIndex - 1];
					}
				}
				else
				{
					int num = currentIndex - this.m_AltLayoutSize.x;
					if (num >= 0)
					{
						rectTransform = this.m_ContentSelectables[num];
					}
				}
				RectTransform rectTransform2 = null;
				if (this.m_AltLayoutIsVertical)
				{
					int num2 = currentIndex - this.m_AltLayoutSize.y;
					if (num2 >= 0)
					{
						rectTransform2 = this.m_ContentSelectables[num2];
					}
				}
				else if (this.m_AltLayoutSize.x != 0 && currentIndex % this.m_AltLayoutSize.x > 0)
				{
					rectTransform2 = this.m_ContentSelectables[currentIndex - 1];
				}
				if (rectTransform != null)
				{
					Selectable component3 = rectTransform.GetComponent<Selectable>();
					navigation.selectOnUp = component3;
					Navigation navigation3 = component3.navigation;
					navigation3.selectOnDown = sel;
					component3.navigation = navigation3;
				}
				else
				{
					navigation.selectOnUp = this.m_BorderSelectables.selectOnUp;
				}
				if (rectTransform2 != null)
				{
					Selectable component4 = rectTransform2.GetComponent<Selectable>();
					navigation.selectOnLeft = component4;
					Navigation navigation4 = component4.navigation;
					navigation4.selectOnRight = sel;
					component4.navigation = navigation4;
				}
				else
				{
					navigation.selectOnLeft = this.m_BorderSelectables.selectOnLeft;
				}
			}
			navigation.selectOnRight = this.m_BorderSelectables.selectOnRight;
			navigation.selectOnDown = this.m_BorderSelectables.selectOnDown;
		}
		sel.navigation = navigation;
		Navigation navigation5 = this.m_BorderSelectables.selectOnDown.navigation;
		navigation5.selectOnUp = sel;
		this.m_BorderSelectables.selectOnDown.navigation = navigation5;
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x0011C5F4 File Offset: 0x0011A9F4
	public virtual void SelectFirstElement()
	{
		if (this.m_ContentSelectables != null && this.m_ContentSelectables.Count > 0 && this.m_CachedEventSystem != null)
		{
			this.m_CachedEventSystem.SetSelectedGameObject(null);
			this.m_CachedEventSystem.SetSelectedGameObject(this.m_ContentSelectables[0].gameObject);
		}
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x0011C658 File Offset: 0x0011AA58
	protected virtual bool ReselectCurrent(ref T17EventSystem m_CachedEventSystem)
	{
		if (this.m_ContentSelectables != null && this.m_ContentSelectables.Count > 0)
		{
			if (this.m_CurrentSelected >= 0 && this.m_CurrentSelected < this.m_ContentSelectables.Count)
			{
				m_CachedEventSystem.SetSelectedGameObject(this.m_ContentSelectables[this.m_CurrentSelected].gameObject);
				return true;
			}
			if (this.m_ContentSelectables[0] != null)
			{
				m_CachedEventSystem.SetSelectedGameObject(this.m_ContentSelectables[0].gameObject);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003BA7 RID: 15271 RVA: 0x0011C6F4 File Offset: 0x0011AAF4
	protected virtual void OnElementSelected(Selectable sel, int index)
	{
		this.m_bContentElementSelected = true;
		this.m_PreviousSelected = this.m_CurrentSelected;
		this.m_CurrentSelected = index;
	}

	// Token: 0x06003BA8 RID: 15272 RVA: 0x0011C710 File Offset: 0x0011AB10
	public void RedirectEdgeLink(Selectable data)
	{
		this.m_DataCache = data;
	}

	// Token: 0x06003BA9 RID: 15273 RVA: 0x0011C71C File Offset: 0x0011AB1C
	public void ClearContents()
	{
		for (int i = this.m_ContentSelectables.Count - 1; i >= 0; i--)
		{
			RectTransform rectTransform = this.m_ContentSelectables[i];
			if (rectTransform != null && rectTransform.gameObject != null)
			{
				rectTransform.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(rectTransform.gameObject);
			}
		}
		this.m_ContentSelectables.Clear();
	}

	// Token: 0x04003074 RID: 12404
	public RectTransform m_ContentParent;

	// Token: 0x04003075 RID: 12405
	public bool m_ContentIsDynamic;

	// Token: 0x04003076 RID: 12406
	protected List<RectTransform> m_ContentSelectables = new List<RectTransform>();

	// Token: 0x04003077 RID: 12407
	protected int m_PreviousSelected;

	// Token: 0x04003078 RID: 12408
	protected int m_CurrentSelected;

	// Token: 0x04003079 RID: 12409
	private bool m_bAltElementLayout;

	// Token: 0x0400307A RID: 12410
	private bool m_AltLayoutIsVertical;

	// Token: 0x0400307B RID: 12411
	private T17NavigableGrid.LayoutSize m_AltLayoutSize = default(T17NavigableGrid.LayoutSize);

	// Token: 0x0400307C RID: 12412
	private bool m_bContentElementSelected;

	// Token: 0x0400307D RID: 12413
	private Selectable m_DataCache;

	// Token: 0x02000B74 RID: 2932
	private struct LayoutSize
	{
		// Token: 0x0400307E RID: 12414
		public int x;

		// Token: 0x0400307F RID: 12415
		public int y;
	}
}
