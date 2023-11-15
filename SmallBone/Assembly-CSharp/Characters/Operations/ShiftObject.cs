using System;
using Characters.AI;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E03 RID: 3587
	public class ShiftObject : CharacterOperation
	{
		// Token: 0x060047B6 RID: 18358 RVA: 0x000D08C0 File Offset: 0x000CEAC0
		public override void Run(Character owner)
		{
			Character target = this._controller.target;
			if (target == null)
			{
				return;
			}
			Collider2D platform = this.GetPlatform();
			Bounds bounds = (platform != null) ? platform.bounds : target.movement.controller.collisionState.lastStandingCollider.bounds;
			float x = target.transform.position.x + this._offsetX;
			float num;
			if (this._underTheCeiling)
			{
				num = (this._fromPlatform ? this.GetClosestCeiling(this._offsetY, new Vector2(target.transform.position.x, bounds.max.y)) : this.GetClosestCeiling(this._offsetY, target.transform.position));
			}
			else
			{
				num = (this._fromPlatform ? bounds.max.y : target.transform.position.y);
				num += this._offsetY;
			}
			this._object.position = new Vector2(x, num);
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x000D09D8 File Offset: 0x000CEBD8
		private float GetClosestCeiling(float distance, Vector3 from)
		{
			RaycastHit2D hit = Physics2D.Raycast(from, Vector2.up, distance, Layers.groundMask);
			if (hit)
			{
				return hit.point.y;
			}
			return from.y + distance;
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x000D0A20 File Offset: 0x000CEC20
		private Collider2D GetPlatform()
		{
			if (this._lastStandingPlatform)
			{
				return null;
			}
			ShiftObject.caster.contactFilter.SetLayerMask(Layers.groundMask);
			NonAllocCaster nonAllocCaster = ShiftObject.caster.BoxCast(this._controller.target.transform.position, this._controller.target.collider.bounds.size, 0f, Vector2.down, 100f);
			if (nonAllocCaster.results.Count == 0)
			{
				return null;
			}
			return nonAllocCaster.results[0].collider;
		}

		// Token: 0x040036C2 RID: 14018
		[SerializeField]
		private AIController _controller;

		// Token: 0x040036C3 RID: 14019
		[SerializeField]
		private Transform _object;

		// Token: 0x040036C4 RID: 14020
		[SerializeField]
		private float _offsetY;

		// Token: 0x040036C5 RID: 14021
		[SerializeField]
		private float _offsetX;

		// Token: 0x040036C6 RID: 14022
		[SerializeField]
		private bool _lastStandingPlatform = true;

		// Token: 0x040036C7 RID: 14023
		[SerializeField]
		private bool _fromPlatform;

		// Token: 0x040036C8 RID: 14024
		[SerializeField]
		private bool _underTheCeiling;

		// Token: 0x040036C9 RID: 14025
		private static NonAllocCaster caster = new NonAllocCaster(1);
	}
}
