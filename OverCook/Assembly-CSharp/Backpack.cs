using System;
using UnityEngine;

// Token: 0x020005ED RID: 1517
public class Backpack : CarryableItem
{
	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06001CDC RID: 7388 RVA: 0x0008DC71 File Offset: 0x0008C071
	// (set) Token: 0x06001CDD RID: 7389 RVA: 0x0008DC79 File Offset: 0x0008C079
	public bool m_usesSeparateColliders { get; private set; }

	// Token: 0x06001CDE RID: 7390 RVA: 0x0008DC84 File Offset: 0x0008C084
	public bool CanHandleDispenserPickup(ICarrier _carrier)
	{
		Vector3 vector = _carrier.AccessGameObject().transform.position - base.transform.position;
		vector.y = 0f;
		Vector3 normalized = vector.normalized;
		float f = Vector3.Dot(-base.transform.forward, normalized);
		float num = Mathf.Acos(f) * 57.29578f;
		return num <= this.m_pickupAngle * 0.5f;
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x0008DD00 File Offset: 0x0008C100
	private void Awake()
	{
		BoxCollider boxCollider = base.gameObject.RequestComponentInImmediateChildren<BoxCollider>();
		this.m_usesSeparateColliders = (boxCollider != null);
		if (this.m_usesSeparateColliders)
		{
			this.m_restingColliderCenter = boxCollider.center;
			this.m_restingColliderSize = boxCollider.size;
			UnityEngine.Object.Destroy(boxCollider.gameObject);
		}
		BoxCollider boxCollider2 = this.m_collider as BoxCollider;
		this.m_usesSeparateColliders &= (boxCollider2 != null);
		if (this.m_usesSeparateColliders)
		{
			this.m_carriedColliderCenter = boxCollider2.center;
			this.m_carriedColliderSize = boxCollider2.size;
			boxCollider2.center = this.m_restingColliderCenter;
			boxCollider2.size = this.m_restingColliderSize;
		}
	}

	// Token: 0x04001675 RID: 5749
	[SerializeField]
	public float m_pickupAngle = 90f;

	// Token: 0x04001676 RID: 5750
	[SerializeField]
	public Collider m_collider;

	// Token: 0x04001677 RID: 5751
	[HideInInspector]
	public Vector3 m_restingColliderCenter;

	// Token: 0x04001678 RID: 5752
	[HideInInspector]
	public Vector3 m_restingColliderSize;

	// Token: 0x04001679 RID: 5753
	[HideInInspector]
	public Vector3 m_carriedColliderCenter;

	// Token: 0x0400167A RID: 5754
	[HideInInspector]
	public Vector3 m_carriedColliderSize;
}
