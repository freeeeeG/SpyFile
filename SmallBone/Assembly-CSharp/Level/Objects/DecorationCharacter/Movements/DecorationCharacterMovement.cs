using System;
using Characters;
using Characters.Movements;
using UnityEngine;

namespace Level.Objects.DecorationCharacter.Movements
{
	// Token: 0x0200057A RID: 1402
	public class DecorationCharacterMovement : MonoBehaviour
	{
		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001B88 RID: 7048 RVA: 0x000557E9 File Offset: 0x000539E9
		public CharacterController2D controller
		{
			get
			{
				return this._controller;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x000557F1 File Offset: 0x000539F1
		public Movement.Config config
		{
			get
			{
				return this.configs[0];
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001B8A RID: 7050 RVA: 0x000557FF File Offset: 0x000539FF
		// (set) Token: 0x06001B8B RID: 7051 RVA: 0x00055807 File Offset: 0x00053A07
		public bool isGrounded { get; private set; }

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001B8C RID: 7052 RVA: 0x00055810 File Offset: 0x00053A10
		// (set) Token: 0x06001B8D RID: 7053 RVA: 0x00055818 File Offset: 0x00053A18
		public Vector2 lastDirection { get; private set; }

		// Token: 0x06001B8E RID: 7054 RVA: 0x00055824 File Offset: 0x00053A24
		private void Awake()
		{
			this._controller = base.GetComponent<CharacterController2D>();
			this._decorationCharacter = base.GetComponent<DecorationCharacter>();
			this.configs.Add(int.MinValue, this._config);
			if (this.config.type == Movement.Config.Type.Static)
			{
				this._decorationCharacter.animationController.parameter.grounded = true;
				this._decorationCharacter.animationController.parameter.walk = false;
				this._decorationCharacter.animationController.parameter.ySpeed = 0f;
			}
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000558B4 File Offset: 0x00053AB4
		private Vector2 HandleMove(float deltaTime)
		{
			Vector2 vector = Vector2.zero;
			float speed = this._decorationCharacter.speed;
			vector = this.move * speed;
			this._decorationCharacter.animationController.parameter.walk = (vector.x != 0f);
			this._decorationCharacter.animationController.parameter.movementSpeed = (this.moveBackward ? (-speed) : speed) * 0.25f;
			if (!this.config.lockLookingDirection)
			{
				if (this.moveBackward)
				{
					if (this.move.x > 0f)
					{
						this._decorationCharacter.lookingDirection = Character.LookingDirection.Left;
					}
					else if (this.move.x < 0f)
					{
						this._decorationCharacter.lookingDirection = Character.LookingDirection.Right;
					}
				}
				else if (this.move.x > 0f)
				{
					this._decorationCharacter.lookingDirection = Character.LookingDirection.Right;
				}
				else if (this.move.x < 0f)
				{
					this._decorationCharacter.lookingDirection = Character.LookingDirection.Left;
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
				if (Mathf.Abs(this._velocity.x) > speed)
				{
					this._velocity.x = speed * Mathf.Sign(this._velocity.x);
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
			this._decorationCharacter.animationController.parameter.ySpeed = this._velocity.y;
			if (deltaTime > 0f)
			{
				this._velocity /= deltaTime;
				if ((this.config.type == Movement.Config.Type.AcceleratingFlying || this.config.type == Movement.Config.Type.Bidimensional) && this._velocity.sqrMagnitude > speed * speed)
				{
					this._velocity = this._velocity.normalized * speed;
				}
				this._controller.velocity = this._velocity;
			}
			return vector;
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x00055CA8 File Offset: 0x00053EA8
		private void AddGravity(float deltaTime)
		{
			if (!this.config.ignoreGravity)
			{
				this._velocity.y = this._velocity.y + this.config.gravity * deltaTime;
			}
			if (this._velocity.y < -this.config.maxFallSpeed)
			{
				this._velocity.y = -this.config.maxFallSpeed;
			}
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x00055D10 File Offset: 0x00053F10
		private void LateUpdate()
		{
			Movement.Config config = this.config;
			if (config.type == Movement.Config.Type.Static)
			{
				return;
			}
			float num = this._decorationCharacter.deltaTime;
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
				this._decorationCharacter.animationController.parameter.grounded = true;
			}
			else
			{
				this._decorationCharacter.animationController.parameter.grounded = this._controller.isGrounded;
			}
			this.force = Vector2.zero;
			if (!config.keepMove)
			{
				this.move = Vector2.zero;
			}
			if (ptr.y <= 0f && this._controller.collisionState.below)
			{
				this.isGrounded = true;
				return;
			}
			this.isGrounded = false;
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x00055E14 File Offset: 0x00054014
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

		// Token: 0x06001B93 RID: 7059 RVA: 0x00055E81 File Offset: 0x00054081
		public void Move(float angle)
		{
			this.move.x = Mathf.Cos(angle);
			this.move.y = Mathf.Sin(angle);
			this.lastDirection = this.move;
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x00055EB4 File Offset: 0x000540B4
		public void MoveTo(Vector2 position)
		{
			this.MoveHorizontal(new Vector2(position.x - base.transform.position.x, position.y - base.transform.position.y).normalized);
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x00055F02 File Offset: 0x00054102
		public void Jump(float jumpHeight)
		{
			if (jumpHeight > 1E-45f)
			{
				this._velocity.y = Mathf.Sqrt(2f * jumpHeight * -this.config.gravity);
			}
		}

		// Token: 0x040017AD RID: 6061
		public readonly PriorityList<Movement.Config> configs = new PriorityList<Movement.Config>();

		// Token: 0x040017AE RID: 6062
		[NonSerialized]
		public Vector2 move;

		// Token: 0x040017AF RID: 6063
		[NonSerialized]
		public bool moveBackward;

		// Token: 0x040017B0 RID: 6064
		[NonSerialized]
		public Vector2 force;

		// Token: 0x040017B1 RID: 6065
		[GetComponent]
		[SerializeField]
		private DecorationCharacter _decorationCharacter;

		// Token: 0x040017B2 RID: 6066
		[GetComponent]
		[SerializeField]
		private CharacterController2D _controller;

		// Token: 0x040017B3 RID: 6067
		[SerializeField]
		private Movement.Config _config;

		// Token: 0x040017B4 RID: 6068
		private Vector2 _moved;

		// Token: 0x040017B5 RID: 6069
		private Vector2 _velocity;
	}
}
