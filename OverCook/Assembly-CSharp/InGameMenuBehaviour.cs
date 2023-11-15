using System;
using UnityEngine;

// Token: 0x02000ACA RID: 2762
public class InGameMenuBehaviour : BaseMenuBehaviour
{
	// Token: 0x060037A9 RID: 14249 RVA: 0x000FA7CC File Offset: 0x000F8BCC
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		if (this.m_CachedEventSystem != null)
		{
			this.m_ObjectSelectedBeforeShow = this.m_CachedEventSystem.currentSelectedGameObject;
			if (this.m_BorderSelectables.selectOnUp != null && this.m_bSelectTopElementOnShow)
			{
				this.m_CachedEventSystem.SetSelectedGameObject(null);
				this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnUp.gameObject);
				T17StandaloneInputModule t17StandaloneInputModule = (T17StandaloneInputModule)this.m_CachedEventSystem.currentInputModule;
				if (t17StandaloneInputModule != null)
				{
					t17StandaloneInputModule.SetLastSelected(this.m_BorderSelectables.selectOnUp.gameObject);
				}
			}
		}
		this.m_IPlayerManager = GameUtils.RequireManager<PlayerManager>();
		return true;
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x000FA894 File Offset: 0x000F8C94
	protected override void Update()
	{
		base.Update();
	}

	// Token: 0x060037AB RID: 14251 RVA: 0x000FA89C File Offset: 0x000F8C9C
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		T17EventSystem cachedEventSystem = this.m_CachedEventSystem;
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		if (cachedEventSystem != null)
		{
			cachedEventSystem.ForceDeselectSelectionObject();
			if (this.m_ObjectSelectedBeforeShow != null)
			{
				cachedEventSystem.SetSelectedGameObject(this.m_ObjectSelectedBeforeShow);
				this.m_ObjectSelectedBeforeShow = null;
			}
		}
		return true;
	}

	// Token: 0x060037AC RID: 14252 RVA: 0x000FA8F6 File Offset: 0x000F8CF6
	public virtual void Close()
	{
		this.Hide(true, false);
	}

	// Token: 0x060037AD RID: 14253 RVA: 0x000FA901 File Offset: 0x000F8D01
	public new void InvokeNavigateOnUICancel()
	{
		if (this.m_NavigateOnUICancel != null && this.m_NavigateOnUICancel.m_DoThisOnUICancel != null)
		{
			this.m_NavigateOnUICancel.m_DoThisOnUICancel.Invoke();
		}
	}

	// Token: 0x04002C9B RID: 11419
	public bool m_bSelectTopElementOnShow = true;

	// Token: 0x04002C9C RID: 11420
	public string MenuName = "GIVE ME A TITLE!";

	// Token: 0x04002C9D RID: 11421
	protected GameObject m_ObjectSelectedBeforeShow;

	// Token: 0x04002C9E RID: 11422
	protected PlayerManager m_IPlayerManager;
}
