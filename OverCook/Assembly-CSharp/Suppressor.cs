using System;
using UnityEngine;

// Token: 0x02000B9D RID: 2973
public class Suppressor
{
	// Token: 0x06003CEF RID: 15599 RVA: 0x0012359F File Offset: 0x0012199F
	public Suppressor(UnityEngine.Object _owner)
	{
		this.m_owner = _owner;
	}

	// Token: 0x06003CF0 RID: 15600 RVA: 0x001235AE File Offset: 0x001219AE
	public void Release()
	{
		this.m_owner = null;
	}

	// Token: 0x06003CF1 RID: 15601 RVA: 0x001235B7 File Offset: 0x001219B7
	public bool IsReleased()
	{
		return this.m_owner == null;
	}

	// Token: 0x06003CF2 RID: 15602 RVA: 0x001235C5 File Offset: 0x001219C5
	public bool IsSuppressedBy(UnityEngine.Object _other)
	{
		return this.m_owner == _other;
	}

	// Token: 0x04003108 RID: 12552
	private UnityEngine.Object m_owner;
}
