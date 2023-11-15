using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class TriggerRecorder : MonoBehaviour
{
	// Token: 0x060006E4 RID: 1764 RVA: 0x0002DDD1 File Offset: 0x0002C1D1
	private void OnTriggerStay(Collider collider)
	{
		this.m_triggers.Add(collider);
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0002DDDF File Offset: 0x0002C1DF
	private void FixedUpdate()
	{
		this.m_triggers.Clear();
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0002DDEC File Offset: 0x0002C1EC
	public List<Collider> GetRecentCollisions()
	{
		return this.m_triggers;
	}

	// Token: 0x040005B9 RID: 1465
	private List<Collider> m_triggers = new List<Collider>();
}
