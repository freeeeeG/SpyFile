using System;
using UnityEngine;

// Token: 0x0200010C RID: 268
public abstract class IUserInterface : MonoBehaviour
{
	// Token: 0x060006AB RID: 1707 RVA: 0x000125D3 File Offset: 0x000107D3
	public virtual void Initialize()
	{
		this.m_RootUI = base.transform.Find("Root").gameObject;
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x000125F0 File Offset: 0x000107F0
	public bool IsVisible()
	{
		return this.m_Active;
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x000125F8 File Offset: 0x000107F8
	public virtual void Show()
	{
		this.m_RootUI.SetActive(true);
		this.m_Active = true;
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x0001260D File Offset: 0x0001080D
	public void SetActive()
	{
		this.m_RootUI.SetActive(true);
		this.m_Active = true;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00012622 File Offset: 0x00010822
	public virtual void Hide()
	{
		this.m_RootUI.SetActive(false);
		this.m_Active = false;
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x00012637 File Offset: 0x00010837
	public virtual void ClosePanel()
	{
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00012639 File Offset: 0x00010839
	public virtual void Release()
	{
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x0001263B File Offset: 0x0001083B
	public virtual void Update()
	{
	}

	// Token: 0x04000312 RID: 786
	protected GameObject m_RootUI;

	// Token: 0x04000313 RID: 787
	protected bool m_Active;
}
