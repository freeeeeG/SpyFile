using System;
using UnityEngine;

// Token: 0x02000808 RID: 2056
public class LimitedQuantityItem : MonoBehaviour
{
	// Token: 0x0600274B RID: 10059 RVA: 0x000B933F File Offset: 0x000B773F
	public void NotifyOfImpendingDestruction()
	{
		this.m_Callback(base.gameObject);
	}

	// Token: 0x0600274C RID: 10060 RVA: 0x000B9352 File Offset: 0x000B7752
	public void RegisterImpendingDestructionNotification(ImpendingDestructionCallback func)
	{
		this.m_Callback = (ImpendingDestructionCallback)Delegate.Combine(this.m_Callback, func);
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x000B936B File Offset: 0x000B776B
	public void UnregisterImpendingDestructionNotification(ImpendingDestructionCallback func)
	{
		this.m_Callback = (ImpendingDestructionCallback)Delegate.Remove(this.m_Callback, func);
	}

	// Token: 0x04001EE2 RID: 7906
	private ImpendingDestructionCallback m_Callback = delegate(GameObject A_0)
	{
	};
}
