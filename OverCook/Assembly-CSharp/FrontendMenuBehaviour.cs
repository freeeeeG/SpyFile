using System;
using UnityEngine;

// Token: 0x02000AB9 RID: 2745
public class FrontendMenuBehaviour : BaseMenuBehaviour
{
	// Token: 0x0600369F RID: 13983 RVA: 0x0006B6D8 File Offset: 0x00069AD8
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		if (this.m_CachedEventSystem != null)
		{
			this.m_ObjectSelectedBeforeShow = this.m_CachedEventSystem.currentSelectedGameObject;
			if (this.m_ObjectSelectedBeforeShow == null)
			{
				this.m_ObjectSelectedBeforeShow = this.m_CachedEventSystem.GetLastRequestedSelectedGameobject();
			}
			if (this.m_BorderSelectables.selectOnUp != null && this.m_bSelectTopElementOnShow)
			{
				this.m_CachedEventSystem.SetSelectedGameObject(null);
				this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnUp.gameObject);
			}
		}
		Animator[] componentsInChildren = base.gameObject.GetComponentsInChildren<Animator>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].speed = 1f;
			}
		}
		return true;
	}

	// Token: 0x060036A0 RID: 13984 RVA: 0x0006B7B8 File Offset: 0x00069BB8
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		T17EventSystem cachedEventSystem = this.m_CachedEventSystem;
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		if (cachedEventSystem != null && this.m_ObjectSelectedBeforeShow != null)
		{
			cachedEventSystem.SetSelectedGameObject(null);
			cachedEventSystem.SetSelectedGameObject(this.m_ObjectSelectedBeforeShow);
			this.m_ObjectSelectedBeforeShow = null;
		}
		return true;
	}

	// Token: 0x060036A1 RID: 13985 RVA: 0x0006B813 File Offset: 0x00069C13
	public virtual void Close()
	{
		this.Hide(true, false);
	}

	// Token: 0x04002BE1 RID: 11233
	public bool m_bSelectTopElementOnShow = true;

	// Token: 0x04002BE2 RID: 11234
	public string MenuName = "GIVE ME A TITLE!";

	// Token: 0x04002BE3 RID: 11235
	protected GameObject m_ObjectSelectedBeforeShow;
}
