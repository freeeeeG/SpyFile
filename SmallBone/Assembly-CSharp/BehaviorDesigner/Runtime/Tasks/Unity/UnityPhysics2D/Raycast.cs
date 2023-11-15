using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics2D
{
	// Token: 0x0200157D RID: 5501
	[TaskDescription("Casts a ray against all colliders in the scene. Returns success if a collider was hit.")]
	[TaskCategory("Unity/Physics2D")]
	public class Raycast : Action
	{
		// Token: 0x060069ED RID: 27117 RVA: 0x00130968 File Offset: 0x0012EB68
		public override TaskStatus OnUpdate()
		{
			Vector2 vector = this.direction.Value;
			Vector2 origin;
			if (this.originGameObject.Value != null)
			{
				origin = this.originGameObject.Value.transform.position;
				if (this.space == Space.Self)
				{
					vector = this.originGameObject.Value.transform.TransformDirection(this.direction.Value);
				}
			}
			else
			{
				origin = this.originPosition.Value;
			}
			RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, vector, (this.distance.Value == -1f) ? float.PositiveInfinity : this.distance.Value, this.layerMask);
			if (raycastHit2D.collider != null)
			{
				this.storeHitObject.Value = raycastHit2D.collider.gameObject;
				this.storeHitPoint.Value = raycastHit2D.point;
				this.storeHitNormal.Value = raycastHit2D.normal;
				this.storeHitDistance.Value = raycastHit2D.distance;
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		// Token: 0x060069EE RID: 27118 RVA: 0x00130A88 File Offset: 0x0012EC88
		public override void OnReset()
		{
			this.originGameObject = null;
			this.originPosition = Vector2.zero;
			this.direction = Vector2.zero;
			this.distance = -1f;
			this.layerMask = -1;
			this.space = Space.Self;
		}

		// Token: 0x040055AF RID: 21935
		[Tooltip("Starts the ray at the GameObject's position. If null the originPosition will be used.")]
		public SharedGameObject originGameObject;

		// Token: 0x040055B0 RID: 21936
		[Tooltip("Starts the ray at the position. Only used if originGameObject is null.")]
		public SharedVector2 originPosition;

		// Token: 0x040055B1 RID: 21937
		[Tooltip("The direction of the ray")]
		public SharedVector2 direction;

		// Token: 0x040055B2 RID: 21938
		[Tooltip("The length of the ray. Set to -1 for infinity.")]
		public SharedFloat distance = -1f;

		// Token: 0x040055B3 RID: 21939
		[Tooltip("Selectively ignore colliders.")]
		public LayerMask layerMask = -1;

		// Token: 0x040055B4 RID: 21940
		[Tooltip("Cast the ray in world or local space. The direction is in world space if no GameObject is specified.")]
		public Space space = Space.Self;

		// Token: 0x040055B5 RID: 21941
		[SharedRequired]
		[Tooltip("Stores the hit object of the raycast.")]
		public SharedGameObject storeHitObject;

		// Token: 0x040055B6 RID: 21942
		[Tooltip("Stores the hit point of the raycast.")]
		[SharedRequired]
		public SharedVector2 storeHitPoint;

		// Token: 0x040055B7 RID: 21943
		[SharedRequired]
		[Tooltip("Stores the hit normal of the raycast.")]
		public SharedVector2 storeHitNormal;

		// Token: 0x040055B8 RID: 21944
		[Tooltip("Stores the hit distance of the raycast.")]
		[SharedRequired]
		public SharedFloat storeHitDistance;
	}
}
