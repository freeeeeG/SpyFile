using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012A RID: 298
public class CollisionRecorder : MonoBehaviour
{
	// Token: 0x06000574 RID: 1396 RVA: 0x0002A413 File Offset: 0x00028813
	private void OnCollisionStay(Collision _collision)
	{
		if (this.m_collisionFilter == null || this.m_collisionFilter(_collision))
		{
			this.m_collision.Add(_collision);
		}
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0002A43D File Offset: 0x0002883D
	public List<Collision> GetRecentCollisions()
	{
		return this.m_collision;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0002A445 File Offset: 0x00028845
	private void LateUpdate()
	{
		this.m_collision.Clear();
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0002A452 File Offset: 0x00028852
	public void SetFilter(Generic<bool, Collision> _filterFunction)
	{
		this.m_collisionFilter = _filterFunction;
	}

	// Token: 0x040004A7 RID: 1191
	private Generic<bool, Collision> m_collisionFilter;

	// Token: 0x040004A8 RID: 1192
	private List<Collision> m_collision = new List<Collision>();
}
