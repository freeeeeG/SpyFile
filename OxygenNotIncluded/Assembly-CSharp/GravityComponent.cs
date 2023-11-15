using System;
using UnityEngine;

// Token: 0x020007E4 RID: 2020
public struct GravityComponent
{
	// Token: 0x0600392D RID: 14637 RVA: 0x0013F9C4 File Offset: 0x0013DBC4
	public GravityComponent(Transform transform, System.Action on_landed, Vector2 initial_velocity, bool land_on_fake_floors, bool mayLeaveWorld)
	{
		this.transform = transform;
		this.elapsedTime = 0f;
		this.velocity = initial_velocity;
		this.onLanded = on_landed;
		this.landOnFakeFloors = land_on_fake_floors;
		this.mayLeaveWorld = mayLeaveWorld;
		KCollider2D component = transform.GetComponent<KCollider2D>();
		this.extents = GravityComponent.GetExtents(component);
		this.bottomYOffset = GravityComponent.GetGroundOffset(component);
	}

	// Token: 0x0600392E RID: 14638 RVA: 0x0013FA20 File Offset: 0x0013DC20
	public static float GetGroundOffset(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.bounds.extents.y - collider.offset.y;
		}
		return 0f;
	}

	// Token: 0x0600392F RID: 14639 RVA: 0x0013FA5C File Offset: 0x0013DC5C
	public static Vector2 GetExtents(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.bounds.extents;
		}
		return Vector2.zero;
	}

	// Token: 0x06003930 RID: 14640 RVA: 0x0013FA8B File Offset: 0x0013DC8B
	public static Vector2 GetOffset(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.offset;
		}
		return Vector2.zero;
	}

	// Token: 0x040025DB RID: 9691
	public Transform transform;

	// Token: 0x040025DC RID: 9692
	public Vector2 velocity;

	// Token: 0x040025DD RID: 9693
	public float elapsedTime;

	// Token: 0x040025DE RID: 9694
	public System.Action onLanded;

	// Token: 0x040025DF RID: 9695
	public bool landOnFakeFloors;

	// Token: 0x040025E0 RID: 9696
	public bool mayLeaveWorld;

	// Token: 0x040025E1 RID: 9697
	public Vector2 extents;

	// Token: 0x040025E2 RID: 9698
	public float bottomYOffset;
}
