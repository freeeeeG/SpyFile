using System;
using UnityEngine;

// Token: 0x020007BC RID: 1980
public struct FallerComponent
{
	// Token: 0x060036C4 RID: 14020 RVA: 0x001278B8 File Offset: 0x00125AB8
	public FallerComponent(Transform transform, Vector2 initial_velocity)
	{
		this.transform = transform;
		this.transformInstanceId = transform.GetInstanceID();
		this.isFalling = false;
		this.initialVelocity = initial_velocity;
		this.partitionerEntry = default(HandleVector<int>.Handle);
		this.solidChangedCB = null;
		this.cellChangedCB = null;
		KCircleCollider2D component = transform.GetComponent<KCircleCollider2D>();
		if (component != null)
		{
			this.offset = component.radius;
			return;
		}
		KCollider2D component2 = transform.GetComponent<KCollider2D>();
		if (component2 != null)
		{
			this.offset = transform.GetPosition().y - component2.bounds.min.y;
			return;
		}
		this.offset = 0f;
	}

	// Token: 0x040021AA RID: 8618
	public Transform transform;

	// Token: 0x040021AB RID: 8619
	public int transformInstanceId;

	// Token: 0x040021AC RID: 8620
	public bool isFalling;

	// Token: 0x040021AD RID: 8621
	public float offset;

	// Token: 0x040021AE RID: 8622
	public Vector2 initialVelocity;

	// Token: 0x040021AF RID: 8623
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040021B0 RID: 8624
	public Action<object> solidChangedCB;

	// Token: 0x040021B1 RID: 8625
	public System.Action cellChangedCB;
}
