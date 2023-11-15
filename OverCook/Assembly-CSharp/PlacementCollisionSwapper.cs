using System;
using UnityEngine;

// Token: 0x02000535 RID: 1333
[RequireComponent(typeof(Collider))]
public class PlacementCollisionSwapper : MonoBehaviour, ICarryNotified
{
	// Token: 0x060018F4 RID: 6388 RVA: 0x0007F299 File Offset: 0x0007D699
	private void Awake()
	{
		this.m_collider = this.FindCollider();
		this.m_defaultMaterial = this.m_collider.sharedMaterial;
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x0007F2B8 File Offset: 0x0007D6B8
	private Collider FindCollider()
	{
		Collider[] array = base.gameObject.RequestComponents<Collider>();
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].isTrigger)
			{
				return array[i];
			}
		}
		return null;
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x0007F2F7 File Offset: 0x0007D6F7
	private void UpdateLayer()
	{
		if (this.m_carried)
		{
			this.m_collider.sharedMaterial = this.m_materialWhenPlaced;
		}
		else
		{
			this.m_collider.sharedMaterial = this.m_defaultMaterial;
		}
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x0007F32B File Offset: 0x0007D72B
	public void OnCarryBegun(ICarrier _carrier)
	{
		this.m_carried = true;
		this.UpdateLayer();
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x0007F33A File Offset: 0x0007D73A
	public void OnCarryEnded(ICarrier _carrier)
	{
		this.m_carried = false;
		this.UpdateLayer();
	}

	// Token: 0x0400140C RID: 5132
	[SerializeField]
	public PhysicMaterial m_materialWhenPlaced;

	// Token: 0x0400140D RID: 5133
	private Collider m_collider;

	// Token: 0x0400140E RID: 5134
	private PhysicMaterial m_defaultMaterial;

	// Token: 0x0400140F RID: 5135
	private bool m_carried;

	// Token: 0x04001410 RID: 5136
	private bool m_onSurface;
}
