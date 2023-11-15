using System;
using System.Runtime.CompilerServices;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007C4 RID: 1988
	public class Ground : Movement
	{
		// Token: 0x06002863 RID: 10339 RVA: 0x0007A528 File Offset: 0x00078728
		public override void Initialize(IProjectile projectile, float direction)
		{
			base.Initialize(projectile, direction);
			this._ySpeed = 0f;
			this._state = Ground.Action.Cotinue;
			this._grounded = false;
			this._caster = new Ground.TerrainCheckCaster();
			this._owner = projectile.owner;
			if (this._followOwner)
			{
				base.direction = (float)((this._owner.transform.position.x > projectile.transform.position.x) ? 0 : 180);
				base.directionVector = ((this._owner.transform.position.x > projectile.transform.position.x) ? Vector2.right : Vector2.left);
			}
			this.SetScaleByFacingDirection();
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x0007A5EC File Offset: 0x000787EC
		private void SetScaleByFacingDirection()
		{
			if (!this._flipXByFacingDirection)
			{
				return;
			}
			Vector3 localScale = base.projectile.transform.localScale;
			localScale.x = Mathf.Abs(localScale.x) * (float)((base.directionVector.x > 0f) ? 1 : -1);
			base.projectile.transform.localScale = localScale;
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x0007A650 File Offset: 0x00078850
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			Vector2 a = (this._startSpeed + (this._targetSpeed - this._startSpeed) * this._curve.Evaluate(time / this._easingTime)) * base.directionVector * base.projectile.speedMultiplier;
			this._ySpeed -= this._gravity * deltaTime;
			a.y += this._ySpeed;
			base.projectile.collider.enabled = true;
			Bounds bounds = base.projectile.collider.bounds;
			base.projectile.collider.enabled = false;
			Vector3 vector = a * deltaTime;
			bounds.center += vector;
			ValueTuple<RaycastHit2D, RaycastHit2D, RaycastHit2D, RaycastHit2D> valueTuple = this._caster.Cast(bounds, Mathf.Abs(vector.y) + 0.0625f, Mathf.Abs(vector.x) + 0.0625f, this._terrainLayer);
			if (valueTuple.Item1 && valueTuple.Item2)
			{
				float num = Mathf.Min(valueTuple.Item1.distance, valueTuple.Item2.distance);
				a.y = -Mathf.Max(-0.03125f, num - 0.03125f);
				this._ySpeed = 0f;
				this._grounded = true;
			}
			else if (valueTuple.Item1)
			{
				a.y = -Mathf.Max(-0.03125f, valueTuple.Item1.distance - 0.03125f);
				this._ySpeed = 0f;
				this._grounded = true;
			}
			else if (valueTuple.Item2)
			{
				a.y = -Mathf.Max(-0.03125f, valueTuple.Item2.distance - 0.03125f);
				this._ySpeed = 0f;
				this._grounded = true;
			}
			if (valueTuple.Item3 || valueTuple.Item4)
			{
				switch (this._onFaceWall)
				{
				case Ground.Action.Hold:
					this._state = Ground.Action.Hold;
					break;
				case Ground.Action.Despawn:
					base.projectile.Despawn();
					break;
				case Ground.Action.Return:
					base.direction -= 180f;
					base.directionVector = new Vector2(-base.directionVector.x, base.directionVector.y);
					a.x *= -1f;
					this.SetScaleByFacingDirection();
					break;
				}
			}
			else if (this._grounded && (!valueTuple.Item1 || !valueTuple.Item2))
			{
				switch (this._onFaceClif)
				{
				case Ground.Action.Hold:
					this._state = Ground.Action.Hold;
					break;
				case Ground.Action.Despawn:
					base.projectile.Despawn();
					break;
				case Ground.Action.Return:
					base.direction -= 180f;
					base.directionVector = new Vector2(-base.directionVector.x, base.directionVector.y);
					a.x *= -1f;
					this.SetScaleByFacingDirection();
					break;
				}
			}
			else if (this._followOwner)
			{
				base.direction = (float)((this._owner.transform.position.x > base.projectile.transform.position.x) ? 0 : 180);
				base.directionVector = ((this._owner.transform.position.x > base.projectile.transform.position.x) ? Vector2.right : Vector2.left);
				if (Mathf.Abs(this._owner.transform.position.x - base.projectile.transform.position.x) < 0.5f)
				{
					this._state = Ground.Action.Hold;
				}
				else
				{
					this._state = Ground.Action.Cotinue;
				}
				this.SetScaleByFacingDirection();
			}
			this._grounded = false;
			if (this._state == Ground.Action.Hold)
			{
				return new ValueTuple<Vector2, float>(base.directionVector, 0f);
			}
			return new ValueTuple<Vector2, float>(a.normalized, a.magnitude);
		}

		// Token: 0x040022B8 RID: 8888
		private const float offset = 0.03125f;

		// Token: 0x040022B9 RID: 8889
		private Ground.TerrainCheckCaster _caster;

		// Token: 0x040022BA RID: 8890
		[SerializeField]
		private Ground.Action _onFaceClif;

		// Token: 0x040022BB RID: 8891
		[SerializeField]
		private Ground.Action _onFaceWall = Ground.Action.Return;

		// Token: 0x040022BC RID: 8892
		private Ground.Action _state;

		// Token: 0x040022BD RID: 8893
		[SerializeField]
		private float _startSpeed;

		// Token: 0x040022BE RID: 8894
		[SerializeField]
		private float _targetSpeed;

		// Token: 0x040022BF RID: 8895
		[SerializeField]
		private AnimationCurve _curve;

		// Token: 0x040022C0 RID: 8896
		[SerializeField]
		private float _easingTime;

		// Token: 0x040022C1 RID: 8897
		[SerializeField]
		private float _gravity;

		// Token: 0x040022C2 RID: 8898
		[SerializeField]
		private bool _flipXByFacingDirection = true;

		// Token: 0x040022C3 RID: 8899
		[SerializeField]
		private bool _followOwner;

		// Token: 0x040022C4 RID: 8900
		[SerializeField]
		private LayerMask _terrainLayer = Layers.terrainMask;

		// Token: 0x040022C5 RID: 8901
		private float _ySpeed;

		// Token: 0x040022C6 RID: 8902
		private bool _grounded;

		// Token: 0x040022C7 RID: 8903
		private Character _owner;

		// Token: 0x020007C5 RID: 1989
		private class TerrainCheckCaster
		{
			// Token: 0x06002867 RID: 10343 RVA: 0x0007AAC4 File Offset: 0x00078CC4
			internal TerrainCheckCaster()
			{
				this._groundLeft = new RayCaster
				{
					direction = Vector2.down
				};
				this._groundRight = new RayCaster
				{
					direction = Vector2.down
				};
				this._wallLeft = new RayCaster
				{
					direction = Vector2.left
				};
				this._wallRight = new RayCaster
				{
					direction = Vector2.right
				};
			}

			// Token: 0x06002868 RID: 10344 RVA: 0x0007AB30 File Offset: 0x00078D30
			[return: TupleElementNames(new string[]
			{
				"groundLeft",
				"groundRight",
				"wallLeft",
				"wallRight"
			})]
			internal ValueTuple<RaycastHit2D, RaycastHit2D, RaycastHit2D, RaycastHit2D> Cast(Bounds bounds, float groundDistance, float wallDistance, LayerMask layerMask)
			{
				this._groundLeft.contactFilter.SetLayerMask(Layers.footholdMask);
				this._groundRight.contactFilter.SetLayerMask(Layers.footholdMask);
				Vector2 origin = new Vector2(bounds.min.x, bounds.min.y);
				Vector2 origin2 = new Vector2(bounds.max.x, bounds.min.y);
				this._groundLeft.origin = origin;
				this._groundLeft.distance = groundDistance;
				this._groundRight.origin = origin2;
				this._groundRight.distance = groundDistance;
				this._wallLeft.contactFilter.SetLayerMask(layerMask);
				this._wallRight.contactFilter.SetLayerMask(layerMask);
				this._wallLeft.origin = origin;
				this._wallLeft.distance = wallDistance;
				this._wallRight.origin = origin2;
				this._wallRight.distance = wallDistance;
				return new ValueTuple<RaycastHit2D, RaycastHit2D, RaycastHit2D, RaycastHit2D>(this._groundLeft.SingleCast(), this._groundRight.SingleCast(), this._wallLeft.SingleCast(), this._wallRight.SingleCast());
			}

			// Token: 0x040022C8 RID: 8904
			private RayCaster _groundLeft;

			// Token: 0x040022C9 RID: 8905
			private RayCaster _groundRight;

			// Token: 0x040022CA RID: 8906
			private RayCaster _wallLeft;

			// Token: 0x040022CB RID: 8907
			private RayCaster _wallRight;
		}

		// Token: 0x020007C6 RID: 1990
		public enum Action
		{
			// Token: 0x040022CD RID: 8909
			Hold,
			// Token: 0x040022CE RID: 8910
			Despawn,
			// Token: 0x040022CF RID: 8911
			Return,
			// Token: 0x040022D0 RID: 8912
			Cotinue
		}
	}
}
