using System;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class VisibilityFromOnlineStatus : MonoBehaviour
{
	// Token: 0x0600071D RID: 1821 RVA: 0x0002E609 File Offset: 0x0002CA09
	private void Start()
	{
		base.gameObject.SetActive(this.ShouldBeVisible());
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x0002E61C File Offset: 0x0002CA1C
	private bool ShouldBeVisible()
	{
		if (!ConnectionStatus.IsInSession())
		{
			return this.m_visibleOffline;
		}
		return this.m_visibleOnline && ((!ConnectionStatus.IsHost()) ? this.m_visibleToClient : this.m_visibleToHost);
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x0002E658 File Offset: 0x0002CA58
	private void Update()
	{
		bool flag = this.ShouldBeVisible();
		if (flag != base.enabled)
		{
			base.gameObject.SetActive(flag);
		}
	}

	// Token: 0x040005E6 RID: 1510
	public bool m_visibleOffline;

	// Token: 0x040005E7 RID: 1511
	public bool m_visibleOnline;

	// Token: 0x040005E8 RID: 1512
	public bool m_visibleToHost;

	// Token: 0x040005E9 RID: 1513
	public bool m_visibleToClient;
}
