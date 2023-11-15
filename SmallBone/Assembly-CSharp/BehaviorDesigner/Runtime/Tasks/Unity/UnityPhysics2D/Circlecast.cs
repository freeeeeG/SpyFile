using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics2D
{
	// Token: 0x0200157B RID: 5499
	[TaskCategory("Unity/Physics2D")]
	[TaskDescription("Casts a circle against all colliders in the scene. Returns success if a collider was hit.")]
	public class Circlecast : Action
	{
		// Token: 0x060069E7 RID: 27111 RVA: 0x00130734 File Offset: 0x0012E934
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
			RaycastHit2D raycastHit2D = Physics2D.CircleCast(origin, this.radius.Value, vector, (this.distance.Value == -1f) ? float.PositiveInfinity : this.distance.Value, this.layerMask);
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

		// Token: 0x060069E8 RID: 27112 RVA: 0x00130860 File Offset: 0x0012EA60
		public override void OnReset()
		{
			this.originGameObject = null;
			this.originPosition = Vector2.zero;
			this.direction = Vector2.zero;
			this.radius = 0f;
			this.distance = -1f;
			this.layerMask = -1;
			this.space = Space.Self;
		}

		// Token: 0x040055A1 RID: 21921
		[Tooltip("Starts the circlecast at the GameObject's position. If null the originPosition will be used.")]
		public SharedGameObject originGameObject;

		// Token: 0x040055A2 RID: 21922
		[Tooltip("Starts the circlecast at the position. Only used if originGameObject is null.")]
		public SharedVector2 originPosition;

		// Token: 0x040055A3 RID: 21923
		[Tooltip("The radius of the circlecast")]
		public SharedFloat radius;

		// Token: 0x040055A4 RID: 21924
		[Tooltip("The direction of the circlecast")]
		public SharedVector2 direction;

		// Token: 0x040055A5 RID: 21925
		[Tooltip("The length of the ray. Set to -1 for infinity.")]
		public SharedFloat distance = -1f;

		// Token: 0x040055A6 RID: 21926
		[Tooltip("Selectively ignore colliders.")]
		public LayerMask layerMask = -1;

		// Token: 0x040055A7 RID: 21927
		[Tooltip("Use world or local space. The direction is in world space if no GameObject is specified.")]
		public Space space = Space.Self;

		// Token: 0x040055A8 RID: 21928
		[SharedRequired]
		[Tooltip("Stores the hit object of the circlecast.")]
		public SharedGameObject storeHitObject;

		// Token: 0x040055A9 RID: 21929
		[Tooltip("Stores the hit point of the circlecast.")]
		[SharedRequired]
		public SharedVector2 storeHitPoint;

		// Token: 0x040055AA RID: 21930
		[SharedRequired]
		[Tooltip("Stores the hit normal of the circlecast.")]
		public SharedVector2 storeHitNormal;

		// Token: 0x040055AB RID: 21931
		[SharedRequired]
		[Tooltip("Stores the hit distance of the circlecast.")]
		public SharedFloat storeHitDistance;
	}
}
