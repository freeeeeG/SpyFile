using System;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200005B RID: 91
	[Serializable]
	public class GraphCollision
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x00015630 File Offset: 0x00013830
		public void Initialize(GraphTransform transform, float scale)
		{
			this.up = (transform.Transform(Vector3.up) - transform.Transform(Vector3.zero)).normalized;
			this.upheight = this.up * this.height;
			this.finalRadius = this.diameter * scale * 0.5f;
			this.finalRaycastRadius = this.thickRaycastDiameter * scale * 0.5f;
			this.contactFilter = new ContactFilter2D
			{
				layerMask = this.mask,
				useDepth = false,
				useLayerMask = true,
				useNormalAngle = false,
				useTriggers = false
			};
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000156E4 File Offset: 0x000138E4
		public bool Check(Vector3 position)
		{
			if (!this.collisionCheck)
			{
				return true;
			}
			if (this.use2D)
			{
				ColliderType colliderType = this.type;
				if (colliderType <= ColliderType.Capsule)
				{
					return Physics2D.OverlapCircle(position, this.finalRadius, this.contactFilter, GraphCollision.dummyArray) == 0;
				}
				return Physics2D.OverlapPoint(position, this.contactFilter, GraphCollision.dummyArray) == 0;
			}
			else
			{
				position += this.up * this.collisionOffset;
				ColliderType colliderType = this.type;
				if (colliderType == ColliderType.Sphere)
				{
					return !Physics.CheckSphere(position, this.finalRadius, this.mask, QueryTriggerInteraction.Ignore);
				}
				if (colliderType == ColliderType.Capsule)
				{
					return !Physics.CheckCapsule(position, position + this.upheight, this.finalRadius, this.mask, QueryTriggerInteraction.Ignore);
				}
				RayDirection rayDirection = this.rayDirection;
				if (rayDirection == RayDirection.Up)
				{
					return !Physics.Raycast(position, this.up, this.height, this.mask, QueryTriggerInteraction.Ignore);
				}
				if (rayDirection == RayDirection.Both)
				{
					return !Physics.Raycast(position, this.up, this.height, this.mask, QueryTriggerInteraction.Ignore) && !Physics.Raycast(position + this.upheight, -this.up, this.height, this.mask, QueryTriggerInteraction.Ignore);
				}
				return !Physics.Raycast(position + this.upheight, -this.up, this.height, this.mask, QueryTriggerInteraction.Ignore);
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00015868 File Offset: 0x00013A68
		public Vector3 CheckHeight(Vector3 position)
		{
			RaycastHit raycastHit;
			bool flag;
			return this.CheckHeight(position, out raycastHit, out flag);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00015880 File Offset: 0x00013A80
		public Vector3 CheckHeight(Vector3 position, out RaycastHit hit, out bool walkable)
		{
			walkable = true;
			if (!this.heightCheck || this.use2D)
			{
				hit = default(RaycastHit);
				return position;
			}
			if (this.thickRaycast)
			{
				Ray ray = new Ray(position + this.up * this.fromHeight, -this.up);
				if (Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, this.heightMask, QueryTriggerInteraction.Ignore))
				{
					return VectorMath.ClosestPointOnLine(ray.origin, ray.origin + ray.direction, hit.point);
				}
				walkable &= !this.unwalkableWhenNoGround;
			}
			else
			{
				if (Physics.Raycast(position + this.up * this.fromHeight, -this.up, out hit, this.fromHeight + 0.005f, this.heightMask, QueryTriggerInteraction.Ignore))
				{
					return hit.point;
				}
				walkable &= !this.unwalkableWhenNoGround;
			}
			return position;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00015994 File Offset: 0x00013B94
		public RaycastHit[] CheckHeightAll(Vector3 position, out int numHits)
		{
			if (!this.heightCheck || this.use2D)
			{
				this.hitBuffer[0] = new RaycastHit
				{
					point = position,
					distance = 0f
				};
				numHits = 1;
				return this.hitBuffer;
			}
			numHits = Physics.RaycastNonAlloc(position + this.up * this.fromHeight, -this.up, this.hitBuffer, this.fromHeight + 0.005f, this.heightMask, QueryTriggerInteraction.Ignore);
			if (numHits == this.hitBuffer.Length)
			{
				this.hitBuffer = new RaycastHit[this.hitBuffer.Length * 2];
				return this.CheckHeightAll(position, out numHits);
			}
			return this.hitBuffer;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00015A5C File Offset: 0x00013C5C
		public void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			this.type = (ColliderType)ctx.reader.ReadInt32();
			this.diameter = ctx.reader.ReadSingle();
			this.height = ctx.reader.ReadSingle();
			this.collisionOffset = ctx.reader.ReadSingle();
			this.rayDirection = (RayDirection)ctx.reader.ReadInt32();
			this.mask = ctx.reader.ReadInt32();
			this.heightMask = ctx.reader.ReadInt32();
			this.fromHeight = ctx.reader.ReadSingle();
			this.thickRaycast = ctx.reader.ReadBoolean();
			this.thickRaycastDiameter = ctx.reader.ReadSingle();
			this.unwalkableWhenNoGround = ctx.reader.ReadBoolean();
			this.use2D = ctx.reader.ReadBoolean();
			this.collisionCheck = ctx.reader.ReadBoolean();
			this.heightCheck = ctx.reader.ReadBoolean();
		}

		// Token: 0x04000290 RID: 656
		public ColliderType type = ColliderType.Capsule;

		// Token: 0x04000291 RID: 657
		public float diameter = 1f;

		// Token: 0x04000292 RID: 658
		public float height = 2f;

		// Token: 0x04000293 RID: 659
		public float collisionOffset;

		// Token: 0x04000294 RID: 660
		public RayDirection rayDirection = RayDirection.Both;

		// Token: 0x04000295 RID: 661
		public LayerMask mask;

		// Token: 0x04000296 RID: 662
		public LayerMask heightMask = -1;

		// Token: 0x04000297 RID: 663
		public float fromHeight = 100f;

		// Token: 0x04000298 RID: 664
		public bool thickRaycast;

		// Token: 0x04000299 RID: 665
		public float thickRaycastDiameter = 1f;

		// Token: 0x0400029A RID: 666
		public bool unwalkableWhenNoGround = true;

		// Token: 0x0400029B RID: 667
		public bool use2D;

		// Token: 0x0400029C RID: 668
		public bool collisionCheck = true;

		// Token: 0x0400029D RID: 669
		public bool heightCheck = true;

		// Token: 0x0400029E RID: 670
		public Vector3 up;

		// Token: 0x0400029F RID: 671
		private Vector3 upheight;

		// Token: 0x040002A0 RID: 672
		private ContactFilter2D contactFilter;

		// Token: 0x040002A1 RID: 673
		private static Collider2D[] dummyArray = new Collider2D[1];

		// Token: 0x040002A2 RID: 674
		private float finalRadius;

		// Token: 0x040002A3 RID: 675
		private float finalRaycastRadius;

		// Token: 0x040002A4 RID: 676
		public const float RaycastErrorMargin = 0.005f;

		// Token: 0x040002A5 RID: 677
		private RaycastHit[] hitBuffer = new RaycastHit[8];
	}
}
