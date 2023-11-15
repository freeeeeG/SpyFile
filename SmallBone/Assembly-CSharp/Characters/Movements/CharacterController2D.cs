using System;
using Level;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Movements
{
	// Token: 0x02000803 RID: 2051
	public class CharacterController2D : MonoBehaviour
	{
		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x060029EC RID: 10732 RVA: 0x0008046F File Offset: 0x0007E66F
		public bool isGrounded
		{
			get
			{
				return this.collisionState.below && this.velocity.y <= 0.001f;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x060029ED RID: 10733 RVA: 0x00080495 File Offset: 0x0007E695
		public bool onTerrain
		{
			get
			{
				return this.collisionState.below && this.terrainMask.Contains(this.collisionState.lastStandingCollider.gameObject.layer);
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x060029EE RID: 10734 RVA: 0x000804C6 File Offset: 0x0007E6C6
		public bool onPlatform
		{
			get
			{
				return this.collisionState.below && this.oneWayPlatformMask.Contains(this.collisionState.lastStandingCollider.gameObject.layer);
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060029EF RID: 10735 RVA: 0x000804F7 File Offset: 0x0007E6F7
		// (set) Token: 0x060029F0 RID: 10736 RVA: 0x000804FF File Offset: 0x0007E6FF
		public LayerMask lastStandingMask { get; set; } = 0;

		// Token: 0x060029F1 RID: 10737 RVA: 0x00080508 File Offset: 0x0007E708
		private void Awake()
		{
			Bounds bounds = this._boxCollider.bounds;
			bounds.center -= base.transform.position;
			this._boxCaster = new BoxSequenceNonAllocCaster(1, this.horizontalRays, this.verticalRays);
			this._boxCaster.SetOriginsFromBounds(bounds);
			this.lastStandingMask = (this.terrainMask | this.oneWayPlatformMask);
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x00080585 File Offset: 0x0007E785
		public void ResetBounds()
		{
			this._bounds.size = Vector2.zero;
			this._boxCaster.SetOriginsFromBounds(this._bounds);
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x000805B0 File Offset: 0x0007E7B0
		public void UpdateBounds()
		{
			Bounds bounds = this._boxCollider.bounds;
			bounds.center -= base.transform.position;
			if (this._bounds == bounds)
			{
				return;
			}
			if (this._boxCaster == null)
			{
				return;
			}
			this._boxCaster.origin = base.transform.position;
			this.UpdateTopCasterPosition(bounds);
			this.UpdateBottomCasterPosition(bounds);
			this.UpdateLeftCasterPosition(bounds);
			this.UpdateRightCasterPosition(bounds);
			this._boxCaster.SetOriginsFromBounds(this._bounds);
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x00080644 File Offset: 0x0007E844
		private void UpdateTopCasterPosition(Bounds bounds)
		{
			Vector2 mostLeftTop = bounds.GetMostLeftTop();
			Vector2 mostRightTop = bounds.GetMostRightTop();
			LineSequenceNonAllocCaster topRaycaster = this._boxCaster.topRaycaster;
			float num = mostLeftTop.y;
			if (topRaycaster.start.y < mostLeftTop.y - this._skinWidth)
			{
				topRaycaster.caster.contactFilter.SetLayerMask(this.terrainMask);
				topRaycaster.CastToLine(mostLeftTop, mostRightTop);
				for (int i = 0; i < topRaycaster.nonAllocCasters.Count; i++)
				{
					ReadonlyBoundedList<RaycastHit2D> results = topRaycaster.nonAllocCasters[i].results;
					if (results.Count != 0)
					{
						num = math.min(num, results[0].point.y - this._boxCaster.origin.y);
					}
				}
				num -= this._skinWidth;
				if (num < this._bounds.max.y)
				{
					return;
				}
			}
			Vector3 max = this._bounds.max;
			max.y = num;
			this._bounds.max = max;
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x00080754 File Offset: 0x0007E954
		private void UpdateBottomCasterPosition(Bounds bounds)
		{
			Vector2 mostLeftBottom = bounds.GetMostLeftBottom();
			Vector2 mostRightBottom = bounds.GetMostRightBottom();
			LineSequenceNonAllocCaster bottomRaycaster = this._boxCaster.bottomRaycaster;
			float num = mostLeftBottom.y;
			if (bottomRaycaster.start.y > mostLeftBottom.y + this._skinWidth)
			{
				bottomRaycaster.caster.contactFilter.SetLayerMask(this.terrainMask);
				bottomRaycaster.CastToLine(mostLeftBottom, mostRightBottom);
				for (int i = 0; i < bottomRaycaster.nonAllocCasters.Count; i++)
				{
					ReadonlyBoundedList<RaycastHit2D> results = bottomRaycaster.nonAllocCasters[i].results;
					if (results.Count != 0)
					{
						num = math.max(num, results[0].point.y - this._boxCaster.origin.y);
					}
				}
				num += this._skinWidth;
				if (num > this._bounds.min.y)
				{
					return;
				}
			}
			Vector3 min = this._bounds.min;
			min.y = num;
			this._bounds.min = min;
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x00080864 File Offset: 0x0007EA64
		private void UpdateLeftCasterPosition(Bounds bounds)
		{
			Vector2 mostLeftTop = bounds.GetMostLeftTop();
			Vector2 mostLeftBottom = bounds.GetMostLeftBottom();
			LineSequenceNonAllocCaster leftRaycaster = this._boxCaster.leftRaycaster;
			float num = mostLeftTop.x;
			if (leftRaycaster.start.x > mostLeftTop.x + this._skinWidth)
			{
				leftRaycaster.caster.contactFilter.SetLayerMask(this.terrainMask);
				leftRaycaster.CastToLine(mostLeftTop, mostLeftBottom);
				for (int i = 0; i < leftRaycaster.nonAllocCasters.Count; i++)
				{
					ReadonlyBoundedList<RaycastHit2D> results = leftRaycaster.nonAllocCasters[i].results;
					if (results.Count != 0)
					{
						num = math.max(num, results[0].point.x - this._boxCaster.origin.x);
					}
				}
				num += this._skinWidth;
				if (num > this._bounds.min.x)
				{
					return;
				}
			}
			Vector3 min = this._bounds.min;
			min.x = num;
			this._bounds.min = min;
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x00080974 File Offset: 0x0007EB74
		private void UpdateRightCasterPosition(Bounds bounds)
		{
			Vector2 mostRightTop = bounds.GetMostRightTop();
			Vector2 mostRightBottom = bounds.GetMostRightBottom();
			LineSequenceNonAllocCaster rightRaycaster = this._boxCaster.rightRaycaster;
			float num = mostRightTop.x;
			if (rightRaycaster.start.x < mostRightTop.x - this._skinWidth)
			{
				rightRaycaster.caster.contactFilter.SetLayerMask(this.terrainMask);
				rightRaycaster.CastToLine(mostRightTop, mostRightBottom);
				for (int i = 0; i < rightRaycaster.nonAllocCasters.Count; i++)
				{
					ReadonlyBoundedList<RaycastHit2D> results = rightRaycaster.nonAllocCasters[i].results;
					if (results.Count != 0)
					{
						num = math.min(num, results[0].point.x - this._boxCaster.origin.x);
					}
				}
				num -= this._skinWidth;
				if (num < this._bounds.max.x)
				{
					return;
				}
			}
			Vector3 max = this._bounds.max;
			max.x = num;
			this._bounds.max = max;
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x00080A84 File Offset: 0x0007EC84
		public Vector2 Move(Vector2 deltaMovement)
		{
			Vector3 position = base.transform.position;
			this.Move(ref position, ref deltaMovement);
			position.x += deltaMovement.x;
			position.y += deltaMovement.y;
			Map instance = Map.Instance;
			if (instance != null && !instance.IsInMap(position))
			{
				Debug.LogWarning("The new position of character " + base.name + " is out of the map. The move was ignored.");
				return Vector2.zero;
			}
			base.transform.position = position;
			return deltaMovement;
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x00080B10 File Offset: 0x0007ED10
		private bool TeleportUponGround(Vector2 direction, float distance, bool recursive)
		{
			if (recursive)
			{
				Vector2 a = base.transform.position;
				while (distance > 0f)
				{
					if (this.TeleportUponGround(a + direction * distance, 4f))
					{
						return true;
					}
					distance -= 1f;
				}
			}
			else
			{
				this.TeleportUponGround(base.transform.position + direction * distance, 4f);
			}
			return false;
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x00080B8C File Offset: 0x0007ED8C
		public bool TeleportUponGround(Vector2 destination, float distance = 4f)
		{
			RaycastHit2D hit = Physics2D.Raycast(destination, Vector2.down, distance, this.terrainMask | this.oneWayPlatformMask);
			if (hit)
			{
				destination = hit.point;
				destination.y += this._skinWidth * 2f;
				return this.Teleport(destination);
			}
			return false;
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x00080BF0 File Offset: 0x0007EDF0
		public bool Teleport(Vector2 destination, float maxRetryDistance)
		{
			return this.Teleport(destination, (MMMaths.Vector3ToVector2(base.transform.position) - destination).normalized, maxRetryDistance);
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x00080C24 File Offset: 0x0007EE24
		public bool Teleport(Vector2 destination, Vector2 direction, float maxRetryDistance)
		{
			int num = 0;
			while ((float)num <= maxRetryDistance)
			{
				if (this.Teleport(destination + direction * (float)num))
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x00080C58 File Offset: 0x0007EE58
		public bool Teleport(Vector2 destination)
		{
			Bounds bounds = this._boxCollider.bounds;
			bounds.center = new Vector2(destination.x, destination.y + (bounds.center.y - bounds.min.y));
			NonAllocOverlapper.shared.contactFilter.SetLayerMask(this.terrainMask);
			if (NonAllocOverlapper.shared.OverlapBox(bounds.center, bounds.size, 0f).results.Count == 0)
			{
				base.transform.position = destination;
				return true;
			}
			return false;
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x00080D04 File Offset: 0x0007EF04
		public bool IsInTerrain()
		{
			NonAllocOverlapper.shared.contactFilter.SetLayerMask(this.terrainMask);
			return NonAllocOverlapper.shared.OverlapBox(base.transform.position + this._bounds.center, this._bounds.size, 0f).results.Count > 0;
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x00080D74 File Offset: 0x0007EF74
		private void Move(ref Vector3 origin, ref Vector2 deltaMovement)
		{
			int num = 0;
			bool flag;
			do
			{
				flag = false;
				num++;
				if (!this.CastLeft(ref origin, ref deltaMovement))
				{
					origin.x += 0.1f * (float)num;
					flag = true;
				}
				if (!this.CastRight(ref origin, ref deltaMovement))
				{
					origin.x -= 0.1f * (float)num;
					flag = true;
				}
				if (!this.CastUp(ref origin, ref deltaMovement))
				{
					origin.y -= 0.1f * (float)num;
					flag = true;
				}
				if (!this.CastDown(ref origin, ref deltaMovement))
				{
					origin.y += 0.1f * (float)num;
					flag = true;
				}
			}
			while (flag && num < 30);
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x00080E0C File Offset: 0x0007F00C
		private bool CastRight(ref Vector3 origin, ref Vector2 deltaMovement)
		{
			float num = this._skinWidth + 0.001f;
			if (deltaMovement.x > 0f)
			{
				num += deltaMovement.x;
			}
			this._boxCaster.rightRaycaster.caster.contactFilter.SetLayerMask(this.terrainMask);
			this._boxCaster.rightRaycaster.caster.origin = origin;
			this._boxCaster.rightRaycaster.caster.distance = num;
			this._boxCaster.rightRaycaster.Cast();
			using (this.collisionState.rightCollisionDetector.scope)
			{
				for (int i = 0; i < this._boxCaster.rightRaycaster.nonAllocCasters.Count; i++)
				{
					NonAllocCaster nonAllocCaster = this._boxCaster.rightRaycaster.nonAllocCasters[i];
					if (nonAllocCaster.results.Count != 0)
					{
						RaycastHit2D hit = nonAllocCaster.results[0];
						if (hit)
						{
							this.collisionState.rightCollisionDetector.Add(hit);
							if (hit.distance == 0f)
							{
								return false;
							}
							deltaMovement.x = math.min(deltaMovement.x, hit.distance - this._skinWidth);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x00080F78 File Offset: 0x0007F178
		private bool CastLeft(ref Vector3 origin, ref Vector2 deltaMovement)
		{
			float num = this._skinWidth + 0.001f;
			if (deltaMovement.x < 0f)
			{
				num += -deltaMovement.x;
			}
			this._boxCaster.leftRaycaster.caster.contactFilter.SetLayerMask(this.terrainMask);
			this._boxCaster.leftRaycaster.caster.origin = origin;
			this._boxCaster.leftRaycaster.caster.distance = num;
			this._boxCaster.leftRaycaster.Cast();
			using (this.collisionState.leftCollisionDetector.scope)
			{
				for (int i = 0; i < this._boxCaster.leftRaycaster.nonAllocCasters.Count; i++)
				{
					NonAllocCaster nonAllocCaster = this._boxCaster.leftRaycaster.nonAllocCasters[i];
					if (nonAllocCaster.results.Count != 0)
					{
						RaycastHit2D hit = nonAllocCaster.results[0];
						if (hit)
						{
							this.collisionState.leftCollisionDetector.Add(hit);
							if (hit.distance == 0f)
							{
								return false;
							}
							deltaMovement.x = math.max(deltaMovement.x, -hit.distance + this._skinWidth);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000810E8 File Offset: 0x0007F2E8
		private bool CastUp(ref Vector3 origin, ref Vector2 deltaMovement)
		{
			float num = this._skinWidth + 0.001f;
			if (deltaMovement.y > 0f)
			{
				num += deltaMovement.y;
			}
			if (this.ignoreAbovePlatform)
			{
				this._boxCaster.topRaycaster.caster.contactFilter.SetLayerMask(this.terrainMask);
			}
			else
			{
				this._boxCaster.topRaycaster.caster.contactFilter.SetLayerMask(this.terrainMask | this.oneWayPlatformMask);
			}
			this._boxCaster.topRaycaster.caster.origin = origin;
			this._boxCaster.topRaycaster.caster.distance = num;
			this._boxCaster.topRaycaster.Cast();
			using (this.collisionState.aboveCollisionDetector.scope)
			{
				for (int i = 0; i < this._boxCaster.topRaycaster.nonAllocCasters.Count; i++)
				{
					NonAllocCaster nonAllocCaster = this._boxCaster.topRaycaster.nonAllocCasters[i];
					if (nonAllocCaster.results.Count != 0)
					{
						RaycastHit2D hit = nonAllocCaster.results[0];
						if (hit)
						{
							this.collisionState.aboveCollisionDetector.Add(hit);
							if (hit.distance == 0f)
							{
								return false;
							}
							deltaMovement.y = math.min(deltaMovement.y, hit.distance - this._skinWidth);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x00081294 File Offset: 0x0007F494
		private bool CastDown(ref Vector3 origin, ref Vector2 deltaMovement)
		{
			float num = this._skinWidth + 0.001f;
			if (deltaMovement.y < 0f)
			{
				num += -deltaMovement.y;
			}
			if (this.ignorePlatform)
			{
				this._boxCaster.bottomRaycaster.caster.contactFilter.SetLayerMask(this.terrainMask);
			}
			else
			{
				this._boxCaster.bottomRaycaster.caster.contactFilter.SetLayerMask(this.terrainMask | this.oneWayPlatformMask);
			}
			this._boxCaster.bottomRaycaster.caster.origin = origin;
			this._boxCaster.bottomRaycaster.caster.distance = num;
			this._boxCaster.bottomRaycaster.Cast();
			using (this.collisionState.belowCollisionDetector.scope)
			{
				for (int i = 0; i < this._boxCaster.bottomRaycaster.nonAllocCasters.Count; i++)
				{
					NonAllocCaster nonAllocCaster = this._boxCaster.bottomRaycaster.nonAllocCasters[i];
					if (nonAllocCaster.results.Count != 0)
					{
						RaycastHit2D hit = nonAllocCaster.results[0];
						if (hit)
						{
							if (this.lastStandingMask.Contains(hit.collider.gameObject.layer))
							{
								this.collisionState.lastStandingCollider = hit.collider;
							}
							this.collisionState.belowCollisionDetector.Add(hit);
							if (hit.distance == 0f)
							{
								return false;
							}
							if (deltaMovement.y < 0f)
							{
								deltaMovement.y = math.max(deltaMovement.y, -hit.distance + this._skinWidth);
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x040023CF RID: 9167
		[HideInInspector]
		public bool ignorePlatform;

		// Token: 0x040023D0 RID: 9168
		[HideInInspector]
		public bool ignoreAbovePlatform = true;

		// Token: 0x040023D1 RID: 9169
		private const float _extraRayLength = 0.001f;

		// Token: 0x040023D2 RID: 9170
		[Range(0.001f, 0.3f)]
		private float _skinWidth = 0.03f;

		// Token: 0x040023D3 RID: 9171
		[FormerlySerializedAs("platformMask")]
		public LayerMask terrainMask = 0;

		// Token: 0x040023D4 RID: 9172
		public LayerMask triggerMask = 0;

		// Token: 0x040023D5 RID: 9173
		public LayerMask oneWayPlatformMask = 0;

		// Token: 0x040023D6 RID: 9174
		public float jumpingThreshold = 0.07f;

		// Token: 0x040023D7 RID: 9175
		[Range(2f, 20f)]
		public int horizontalRays = 8;

		// Token: 0x040023D8 RID: 9176
		[Range(2f, 20f)]
		public int verticalRays = 4;

		// Token: 0x040023D9 RID: 9177
		[SerializeField]
		protected BoxCollider2D _boxCollider;

		// Token: 0x040023DA RID: 9178
		[SerializeField]
		protected Rigidbody2D _rigidBody;

		// Token: 0x040023DB RID: 9179
		public readonly CharacterController2D.CollisionState collisionState = new CharacterController2D.CollisionState();

		// Token: 0x040023DC RID: 9180
		[HideInInspector]
		public Vector3 velocity;

		// Token: 0x040023DD RID: 9181
		private BoxSequenceNonAllocCaster _boxCaster;

		// Token: 0x040023DE RID: 9182
		private Bounds _bounds;

		// Token: 0x02000804 RID: 2052
		public class CollisionState
		{
			// Token: 0x17000884 RID: 2180
			// (get) Token: 0x06002A05 RID: 10757 RVA: 0x00081501 File Offset: 0x0007F701
			public bool above
			{
				get
				{
					return this.aboveCollisionDetector.colliding;
				}
			}

			// Token: 0x17000885 RID: 2181
			// (get) Token: 0x06002A06 RID: 10758 RVA: 0x0008150E File Offset: 0x0007F70E
			public bool below
			{
				get
				{
					return this.belowCollisionDetector.colliding;
				}
			}

			// Token: 0x17000886 RID: 2182
			// (get) Token: 0x06002A07 RID: 10759 RVA: 0x0008151B File Offset: 0x0007F71B
			public bool right
			{
				get
				{
					return this.rightCollisionDetector.colliding;
				}
			}

			// Token: 0x17000887 RID: 2183
			// (get) Token: 0x06002A08 RID: 10760 RVA: 0x00081528 File Offset: 0x0007F728
			public bool left
			{
				get
				{
					return this.leftCollisionDetector.colliding;
				}
			}

			// Token: 0x17000888 RID: 2184
			// (get) Token: 0x06002A09 RID: 10761 RVA: 0x00081535 File Offset: 0x0007F735
			public bool horizontal
			{
				get
				{
					return this.right || this.left;
				}
			}

			// Token: 0x17000889 RID: 2185
			// (get) Token: 0x06002A0A RID: 10762 RVA: 0x00081535 File Offset: 0x0007F735
			public bool vertical
			{
				get
				{
					return this.right || this.left;
				}
			}

			// Token: 0x1700088A RID: 2186
			// (get) Token: 0x06002A0B RID: 10763 RVA: 0x00081547 File Offset: 0x0007F747
			public bool any
			{
				get
				{
					return this.below || this.right || this.left || this.above;
				}
			}

			// Token: 0x1700088B RID: 2187
			// (get) Token: 0x06002A0C RID: 10764 RVA: 0x00081569 File Offset: 0x0007F769
			// (set) Token: 0x06002A0D RID: 10765 RVA: 0x00081571 File Offset: 0x0007F771
			public Collider2D lastStandingCollider { get; internal set; }

			// Token: 0x06002A0E RID: 10766 RVA: 0x0008157A File Offset: 0x0007F77A
			internal CollisionState()
			{
			}

			// Token: 0x040023E0 RID: 9184
			internal readonly ManualCollisionDetector aboveCollisionDetector = new ManualCollisionDetector();

			// Token: 0x040023E1 RID: 9185
			internal readonly ManualCollisionDetector belowCollisionDetector = new ManualCollisionDetector();

			// Token: 0x040023E2 RID: 9186
			internal readonly ManualCollisionDetector leftCollisionDetector = new ManualCollisionDetector();

			// Token: 0x040023E3 RID: 9187
			internal readonly ManualCollisionDetector rightCollisionDetector = new ManualCollisionDetector();
		}
	}
}
