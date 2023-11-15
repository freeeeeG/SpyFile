using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FBE RID: 4030
	public sealed class ScaleByPlatformSize : CharacterOperation
	{
		// Token: 0x06004E18 RID: 19992 RVA: 0x000E9858 File Offset: 0x000E7A58
		public override void Run(Character owner)
		{
			Bounds? boundsBelowPlatform = this.GetBoundsBelowPlatform(1f);
			if (boundsBelowPlatform == null)
			{
				return;
			}
			if (boundsBelowPlatform.Value.size.x / this._multiplier > this._maxScale.localScale.x)
			{
				this._target.localScale = new Vector2(this._maxScale.localScale.x, this._target.localScale.y);
				return;
			}
			if (boundsBelowPlatform.Value.size.x / this._multiplier < this._minScale.localScale.x)
			{
				this._target.localScale = new Vector2(this._minScale.localScale.x, this._target.localScale.y);
				return;
			}
			this._target.localScale = new Vector2(boundsBelowPlatform.Value.size.x / this._multiplier, this._target.localScale.y);
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x000E9984 File Offset: 0x000E7B84
		private Bounds? GetBoundsBelowPlatform(float distance = 1f)
		{
			ScaleByPlatformSize._belowCaster.contactFilter.SetLayerMask(this._groundMask);
			ScaleByPlatformSize._belowCaster.BoxCast(this._origin.transform.position, this._targetCollider.size, 0f, Vector2.down, distance);
			ReadonlyBoundedList<RaycastHit2D> results = ScaleByPlatformSize._belowCaster.results;
			if (results.Count <= 0)
			{
				return null;
			}
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < results.Count; i++)
			{
				float distance2 = results[i].distance;
				if (distance2 < num2)
				{
					num2 = distance2;
					num = i;
				}
			}
			if (num == -1)
			{
				return null;
			}
			return new Bounds?(results[num].collider.bounds);
		}

		// Token: 0x04003E13 RID: 15891
		[SerializeField]
		private Transform _origin;

		// Token: 0x04003E14 RID: 15892
		[SerializeField]
		private Transform _target;

		// Token: 0x04003E15 RID: 15893
		[SerializeField]
		private BoxCollider2D _targetCollider;

		// Token: 0x04003E16 RID: 15894
		[SerializeField]
		private Transform _minScale;

		// Token: 0x04003E17 RID: 15895
		[SerializeField]
		private Transform _maxScale;

		// Token: 0x04003E18 RID: 15896
		[SerializeField]
		private float _multiplier;

		// Token: 0x04003E19 RID: 15897
		[SerializeField]
		private LayerMask _groundMask = Layers.groundMask;

		// Token: 0x04003E1A RID: 15898
		private static NonAllocCaster _belowCaster = new NonAllocCaster(1);
	}
}
