using System;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Movements
{
	// Token: 0x02000806 RID: 2054
	public class Movement : MonoBehaviour
	{
		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06002A14 RID: 10772 RVA: 0x000816D0 File Offset: 0x0007F8D0
		// (remove) Token: 0x06002A15 RID: 10773 RVA: 0x00081708 File Offset: 0x0007F908
		public event Action onGrounded;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06002A16 RID: 10774 RVA: 0x00081740 File Offset: 0x0007F940
		// (remove) Token: 0x06002A17 RID: 10775 RVA: 0x00081778 File Offset: 0x0007F978
		public event Action onFall;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06002A18 RID: 10776 RVA: 0x000817B0 File Offset: 0x0007F9B0
		// (remove) Token: 0x06002A19 RID: 10777 RVA: 0x000817E8 File Offset: 0x0007F9E8
		public event Movement.onJumpDelegate onJump;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06002A1A RID: 10778 RVA: 0x00081820 File Offset: 0x0007FA20
		// (remove) Token: 0x06002A1B RID: 10779 RVA: 0x00081858 File Offset: 0x0007FA58
		public event Action<Vector2> onMoved;

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002A1C RID: 10780 RVA: 0x0008188D File Offset: 0x0007FA8D
		private float speed
		{
			get
			{
				return this._character.stat.GetInterpolatedMovementSpeed();
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002A1D RID: 10781 RVA: 0x0008189F File Offset: 0x0007FA9F
		private float knockbackMultiplier
		{
			get
			{
				return (float)this._character.stat.GetFinal(Stat.Kind.KnockbackResistance);
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002A1E RID: 10782 RVA: 0x000818B7 File Offset: 0x0007FAB7
		public Movement.Config baseConfig
		{
			get
			{
				return this._config;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002A1F RID: 10783 RVA: 0x000818BF File Offset: 0x0007FABF
		public Movement.Config config
		{
			get
			{
				return this.configs[0];
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002A20 RID: 10784 RVA: 0x000818CD File Offset: 0x0007FACD
		// (set) Token: 0x06002A21 RID: 10785 RVA: 0x000818D5 File Offset: 0x0007FAD5
		public Vector2 lastDirection { get; private set; }

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002A22 RID: 10786 RVA: 0x000818DE File Offset: 0x0007FADE
		public CharacterController2D controller
		{
			get
			{
				return this._controller;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x000818E6 File Offset: 0x0007FAE6
		public Character owner
		{
			get
			{
				return this._character;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002A24 RID: 10788 RVA: 0x000818EE File Offset: 0x0007FAEE
		public Vector2 moved
		{
			get
			{
				return this._moved;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x000818F6 File Offset: 0x0007FAF6
		public Vector2 velocity
		{
			get
			{
				return this._velocity;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06002A26 RID: 10790 RVA: 0x000818FE File Offset: 0x0007FAFE
		// (set) Token: 0x06002A27 RID: 10791 RVA: 0x0008190B File Offset: 0x0007FB0B
		public float verticalVelocity
		{
			get
			{
				return this._velocity.y;
			}
			set
			{
				this._velocity.y = value;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06002A28 RID: 10792 RVA: 0x00081919 File Offset: 0x0007FB19
		// (set) Token: 0x06002A29 RID: 10793 RVA: 0x00081921 File Offset: 0x0007FB21
		public bool isGrounded { get; private set; }

		// Token: 0x06002A2B RID: 10795 RVA: 0x00081938 File Offset: 0x0007FB38
		protected virtual void Awake()
		{
			this.push = new Push(this._character);
			this.configs.Add(int.MinValue, this._config);
			this._controller = base.GetComponent<CharacterController2D>();
			this._controller.collisionState.aboveCollisionDetector.OnEnter += delegate(RaycastHit2D hit)
			{
				this.OnControllerCollide(hit, Movement.CollisionDirection.Above);
			};
			this._controller.collisionState.belowCollisionDetector.OnEnter += delegate(RaycastHit2D hit)
			{
				this.OnControllerCollide(hit, Movement.CollisionDirection.Below);
			};
			this._controller.collisionState.leftCollisionDetector.OnEnter += delegate(RaycastHit2D hit)
			{
				this.OnControllerCollide(hit, Movement.CollisionDirection.Left);
			};
			this._controller.collisionState.rightCollisionDetector.OnEnter += delegate(RaycastHit2D hit)
			{
				this.OnControllerCollide(hit, Movement.CollisionDirection.Right);
			};
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += this.FindClosestBelowGround;
			this.currentAirJumpCount = 0;
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x00081A1E File Offset: 0x0007FC1E
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= this.FindClosestBelowGround;
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x00081A44 File Offset: 0x0007FC44
		private void FindClosestBelowGround()
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, Vector2.down, float.PositiveInfinity, Layers.groundMask);
			if (hit)
			{
				this.controller.collisionState.lastStandingCollider = hit.collider;
			}
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x00081A9A File Offset: 0x0007FC9A
		private void Start()
		{
			if (this.config.snapToGround)
			{
				this._controller.Move(new Vector2(0f, -50f));
			}
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x00081AC4 File Offset: 0x0007FCC4
		private void OnControllerCollide(RaycastHit2D raycastHit, Movement.CollisionDirection direction)
		{
			if (this.push.smash && !this.push.expired)
			{
				this.push.CollideWith(raycastHit, direction);
			}
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x00081AF0 File Offset: 0x0007FCF0
		private bool HandlePush(float deltaTime)
		{
			if (this._config.ignorePush)
			{
				return false;
			}
			if (this.push.expired)
			{
				this._controller.ignoreAbovePlatform = true;
				return false;
			}
			Vector2 vector;
			this.push.Update(out vector, deltaTime);
			this._controller.ignoreAbovePlatform = !this.push.smash;
			vector *= this.knockbackMultiplier;
			if (this.push.ignoreOtherForce)
			{
				this._moved = this._controller.Move(vector);
				this._velocity = Vector2.zero;
				return true;
			}
			this.force += vector;
			return false;
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x00081B9C File Offset: 0x0007FD9C
		private Vector2 HandleMove(float deltaTime)
		{
			Vector2 vector = Vector2.zero;
			if (this.HandlePush(deltaTime))
			{
				return vector;
			}
			float num = this.blocked.value ? 0f : this.speed;
			vector = this.move * num;
			this._character.animationController.parameter.walk = (vector.x != 0f);
			this._character.animationController.parameter.movementSpeed = (this.moveBackward ? (-num) : num) * 0.25f;
			if (!this.config.lockLookingDirection)
			{
				if (this.moveBackward)
				{
					if (this.move.x > 0f)
					{
						this._character.lookingDirection = Character.LookingDirection.Left;
					}
					else if (this.move.x < 0f)
					{
						this._character.lookingDirection = Character.LookingDirection.Right;
					}
				}
				else if (this.move.x > 0f)
				{
					this._character.lookingDirection = Character.LookingDirection.Right;
				}
				else if (this.move.x < 0f)
				{
					this._character.lookingDirection = Character.LookingDirection.Left;
				}
			}
			if (this._controller.isGrounded && this._velocity.y < 0f)
			{
				this._velocity.y = 0f;
			}
			switch (this.config.type)
			{
			case Movement.Config.Type.Walking:
				this._velocity.x = vector.x;
				this.AddGravity(deltaTime);
				break;
			case Movement.Config.Type.Flying:
				this._velocity = vector;
				break;
			case Movement.Config.Type.AcceleratingFlying:
				this._velocity *= 1f - this.config.friction * deltaTime;
				this._velocity += vector * this.config.acceleration * deltaTime;
				this.AddGravity(deltaTime);
				break;
			case Movement.Config.Type.AcceleratingWalking:
				this._velocity.x = this._velocity.x * (1f - this.config.friction * deltaTime);
				this._velocity.x = this._velocity.x + vector.x * this.config.acceleration * deltaTime;
				if (Mathf.Abs(this._velocity.x) > num)
				{
					this._velocity.x = num * Mathf.Sign(this._velocity.x);
				}
				this.AddGravity(deltaTime);
				break;
			case Movement.Config.Type.Bidimensional:
				this._velocity *= 1f - this.config.friction * deltaTime;
				this._velocity += vector * this.config.acceleration * deltaTime;
				this.AddGravity(deltaTime);
				break;
			}
			vector = this._velocity * deltaTime + this.force;
			this._moved = this._controller.Move(vector);
			this._velocity = this._moved - this.force;
			if (vector.x > 0f != this._velocity.x > 0f)
			{
				this._velocity.x = 0f;
			}
			if (vector.y > 0f != this._velocity.y > 0f)
			{
				this._velocity.y = 0f;
			}
			this._character.animationController.parameter.ySpeed = this._velocity.y;
			if (deltaTime > 0f)
			{
				this._velocity /= deltaTime;
				if ((this.config.type == Movement.Config.Type.AcceleratingFlying || this.config.type == Movement.Config.Type.Bidimensional) && this._velocity.sqrMagnitude > num * num)
				{
					this._velocity = this._velocity.normalized * num;
				}
				this._controller.velocity = this._velocity;
			}
			Action<Vector2> action = this.onMoved;
			if (action != null)
			{
				action(this._moved);
			}
			return vector;
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x00081FC4 File Offset: 0x000801C4
		private void AddGravity(float deltaTime)
		{
			if (!this.ignoreGravity.value && !this.config.ignoreGravity)
			{
				this._velocity.y = this._velocity.y + this.config.gravity * deltaTime;
			}
			if (this._velocity.y < -this.config.maxFallSpeed)
			{
				this._velocity.y = -this.config.maxFallSpeed;
			}
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x00082038 File Offset: 0x00080238
		protected virtual void LateUpdate()
		{
			Movement.Config config = this.config;
			if (config.type == Movement.Config.Type.Static)
			{
				this._character.animationController.parameter.grounded = true;
				this._character.animationController.parameter.walk = false;
				this._character.animationController.parameter.ySpeed = 0f;
				return;
			}
			float num = this._character.chronometer.animation.DeltaTime();
			if (num == 0f)
			{
				return;
			}
			if (num > 0.1f)
			{
				num = 0.1f;
			}
			this._controller.UpdateBounds();
			bool isGrounded = this.isGrounded;
			this._moved = Vector2.zero;
			ref Vector2 ptr = this.HandleMove(num);
			if (config.type == Movement.Config.Type.Flying || config.type == Movement.Config.Type.AcceleratingFlying || config.type == Movement.Config.Type.Bidimensional)
			{
				this._character.animationController.parameter.grounded = true;
			}
			else
			{
				this._character.animationController.parameter.grounded = this._controller.isGrounded;
			}
			this.force = Vector2.zero;
			if (!config.keepMove)
			{
				this.move = Vector2.zero;
			}
			if (ptr.y <= 0f && this._controller.collisionState.below)
			{
				this.isGrounded = true;
				if (!isGrounded)
				{
					Action action = this.onGrounded;
					if (action != null)
					{
						action();
					}
					this.currentAirJumpCount = 0;
					if (this._velocity.y <= 0f && !this.push.expired && this.push.expireOnGround)
					{
						this.push.Expire();
						return;
					}
				}
			}
			else
			{
				if (this.isGrounded && this.onFall != null)
				{
					this.onFall();
				}
				this.isGrounded = false;
			}
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x000821F8 File Offset: 0x000803F8
		public void MoveHorizontal(Vector2 normalizedDirection)
		{
			if (this.config.keepMove)
			{
				if (normalizedDirection == Vector2.zero)
				{
					return;
				}
				if (normalizedDirection.x > 0f)
				{
					normalizedDirection.x = 1f;
				}
				if (normalizedDirection.x < 0f)
				{
					normalizedDirection.x = -1f;
				}
			}
			this.move = normalizedDirection;
			this.lastDirection = this.move;
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x00082268 File Offset: 0x00080468
		public void MoveVertical(Vector2 normalizedDirection)
		{
			if (this.config.type != Movement.Config.Type.Bidimensional)
			{
				return;
			}
			if (this.config.keepMove)
			{
				if (normalizedDirection == Vector2.zero)
				{
					return;
				}
				if (normalizedDirection.y > 0f)
				{
					normalizedDirection.y = 1f;
				}
				if (normalizedDirection.y < 0f)
				{
					normalizedDirection.y = -1f;
				}
			}
			this.move.y = normalizedDirection.y;
			this.lastDirection = this.move;
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x000822EE File Offset: 0x000804EE
		public void Move(float angle)
		{
			this.move.x = Mathf.Cos(angle);
			this.move.y = Mathf.Sin(angle);
			this.lastDirection = this.move;
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x0008231E File Offset: 0x0008051E
		public void Move(Vector2 direction)
		{
			this.move = direction;
			this.lastDirection = direction;
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x00082330 File Offset: 0x00080530
		public void MoveTo(Vector2 position)
		{
			this.MoveHorizontal(new Vector2(position.x - base.transform.position.x, position.y - base.transform.position.y).normalized);
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x00082380 File Offset: 0x00080580
		public void Jump(float jumpHeight)
		{
			if (jumpHeight > 1E-45f)
			{
				this._velocity.y = Mathf.Sqrt(2f * jumpHeight * -this.config.gravity);
			}
			Movement.onJumpDelegate onJumpDelegate = this.onJump;
			if (onJumpDelegate == null)
			{
				return;
			}
			onJumpDelegate(this.isGrounded ? Movement.JumpType.GroundJump : Movement.JumpType.AirJump, jumpHeight);
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000823D8 File Offset: 0x000805D8
		public void JumpDown()
		{
			bool ignorePlatform = this._controller.ignorePlatform;
			this._controller.ignorePlatform = true;
			this._controller.Move(new Vector3(0f, -0.1f, 0f));
			this._controller.ignorePlatform = ignorePlatform;
			Movement.onJumpDelegate onJumpDelegate = this.onJump;
			if (onJumpDelegate == null)
			{
				return;
			}
			onJumpDelegate(Movement.JumpType.DownJump, 0f);
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x00082444 File Offset: 0x00080644
		public bool TryBelowRayCast(LayerMask mask, out RaycastHit2D point, float distance)
		{
			Movement._belowCaster.contactFilter.SetLayerMask(mask);
			Movement._belowCaster.RayCast(this.owner.transform.position, Vector2.down, distance);
			ReadonlyBoundedList<RaycastHit2D> results = Movement._belowCaster.results;
			point = default(RaycastHit2D);
			if (results.Count < 0)
			{
				return false;
			}
			int index = 0;
			float num = results[0].distance;
			for (int i = 1; i < results.Count; i++)
			{
				float distance2 = results[i].distance;
				if (distance2 < num)
				{
					num = distance2;
					index = i;
				}
			}
			point = results[index];
			return true;
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000824F8 File Offset: 0x000806F8
		public bool TryGetClosestBelowCollider(out Collider2D collider, LayerMask layerMask, float distance = 100f)
		{
			Movement._belowCaster.contactFilter.SetLayerMask(layerMask);
			ReadonlyBoundedList<RaycastHit2D> results = Movement._belowCaster.BoxCast(this.owner.transform.position, this.owner.collider.bounds.size, 0f, Vector2.down, distance).results;
			if (results.Count <= 0)
			{
				collider = null;
				return false;
			}
			int index = 0;
			float num = results[0].distance;
			for (int i = 1; i < results.Count; i++)
			{
				float distance2 = results[i].distance;
				if (distance2 < num)
				{
					num = distance2;
					index = i;
				}
			}
			collider = results[index].collider;
			return true;
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000825CC File Offset: 0x000807CC
		public void TurnOnEdge(ref Vector2 direction)
		{
			Collider2D lastStandingCollider = this.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				return;
			}
			float num = this.velocity.x * this._character.chronometer.master.deltaTime;
			if (this._character.collider.bounds.max.x + num + 0.5f >= lastStandingCollider.bounds.max.x)
			{
				direction = Vector2.left;
				return;
			}
			if (this._character.collider.bounds.min.x + num - 0.5f <= lastStandingCollider.bounds.min.x)
			{
				direction = Vector2.right;
			}
		}

		// Token: 0x040023EA RID: 9194
		public readonly SumInt airJumpCount = new SumInt(1);

		// Token: 0x040023EB RID: 9195
		public readonly TrueOnlyLogicalSumList ignoreGravity = new TrueOnlyLogicalSumList(false);

		// Token: 0x040023EC RID: 9196
		[NonSerialized]
		public int currentAirJumpCount;

		// Token: 0x040023ED RID: 9197
		[NonSerialized]
		public bool binded;

		// Token: 0x040023EE RID: 9198
		[NonSerialized]
		public TrueOnlyLogicalSumList blocked = new TrueOnlyLogicalSumList(false);

		// Token: 0x040023EF RID: 9199
		[NonSerialized]
		public Vector2 move;

		// Token: 0x040023F0 RID: 9200
		[NonSerialized]
		public Vector2 force;

		// Token: 0x040023F1 RID: 9201
		[NonSerialized]
		public bool moveBackward;

		// Token: 0x040023F2 RID: 9202
		[SerializeField]
		private Character _character;

		// Token: 0x040023F3 RID: 9203
		[SerializeField]
		private Movement.Config _config;

		// Token: 0x040023F4 RID: 9204
		[SerializeField]
		[GetComponent]
		private CharacterController2D _controller;

		// Token: 0x040023F5 RID: 9205
		private Vector2 _moved;

		// Token: 0x040023F6 RID: 9206
		private Vector2 _velocity;

		// Token: 0x040023F7 RID: 9207
		private static readonly NonAllocCaster _belowCaster = new NonAllocCaster(15);

		// Token: 0x040023F8 RID: 9208
		public readonly PriorityList<Movement.Config> configs = new PriorityList<Movement.Config>();

		// Token: 0x040023FA RID: 9210
		public Push push;

		// Token: 0x02000807 RID: 2055
		[Serializable]
		public class Config
		{
			// Token: 0x06002A43 RID: 10819 RVA: 0x00082700 File Offset: 0x00080900
			public Config()
			{
			}

			// Token: 0x06002A44 RID: 10820 RVA: 0x0008273C File Offset: 0x0008093C
			public Config(Movement.Config.Type type)
			{
				this.type = type;
			}

			// Token: 0x040023FC RID: 9212
			public static readonly Movement.Config @static = new Movement.Config(Movement.Config.Type.Static);

			// Token: 0x040023FD RID: 9213
			[SerializeField]
			internal Movement.Config.Type type = Movement.Config.Type.Walking;

			// Token: 0x040023FE RID: 9214
			[SerializeField]
			internal bool keepMove;

			// Token: 0x040023FF RID: 9215
			[SerializeField]
			internal bool snapToGround;

			// Token: 0x04002400 RID: 9216
			[SerializeField]
			internal bool lockLookingDirection;

			// Token: 0x04002401 RID: 9217
			[SerializeField]
			internal float gravity = -40f;

			// Token: 0x04002402 RID: 9218
			[SerializeField]
			internal float maxFallSpeed = 25f;

			// Token: 0x04002403 RID: 9219
			[SerializeField]
			internal float acceleration = 2f;

			// Token: 0x04002404 RID: 9220
			[SerializeField]
			internal float friction = 0.95f;

			// Token: 0x04002405 RID: 9221
			[SerializeField]
			internal bool ignoreGravity;

			// Token: 0x04002406 RID: 9222
			[SerializeField]
			internal bool ignorePush;

			// Token: 0x02000808 RID: 2056
			public enum Type
			{
				// Token: 0x04002408 RID: 9224
				Static,
				// Token: 0x04002409 RID: 9225
				Walking,
				// Token: 0x0400240A RID: 9226
				Flying,
				// Token: 0x0400240B RID: 9227
				AcceleratingFlying,
				// Token: 0x0400240C RID: 9228
				AcceleratingWalking,
				// Token: 0x0400240D RID: 9229
				Bidimensional
			}
		}

		// Token: 0x02000809 RID: 2057
		public enum CollisionDirection
		{
			// Token: 0x0400240F RID: 9231
			None,
			// Token: 0x04002410 RID: 9232
			Above,
			// Token: 0x04002411 RID: 9233
			Below,
			// Token: 0x04002412 RID: 9234
			Left,
			// Token: 0x04002413 RID: 9235
			Right
		}

		// Token: 0x0200080A RID: 2058
		public enum JumpType
		{
			// Token: 0x04002415 RID: 9237
			GroundJump,
			// Token: 0x04002416 RID: 9238
			AirJump,
			// Token: 0x04002417 RID: 9239
			DownJump
		}

		// Token: 0x0200080B RID: 2059
		// (Invoke) Token: 0x06002A47 RID: 10823
		public delegate void onJumpDelegate(Movement.JumpType jumpType, float jumpHeight);
	}
}
