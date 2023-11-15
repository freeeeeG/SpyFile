using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FAB RID: 4011
	public sealed class RotateLaser : CharacterOperation
	{
		// Token: 0x06004DDC RID: 19932 RVA: 0x000E8E90 File Offset: 0x000E7090
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x000E8EA0 File Offset: 0x000E70A0
		private IEnumerator CRun(Character owner)
		{
			Vector2 direction = this._laser.direction;
			float startDirection = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
			float directionDistance = this._targetDirection - startDirection;
			float time = 0f;
			while (time < this._curve.duration)
			{
				this._laser.Activate(owner, startDirection + directionDistance * this._curve.Evaluate(time));
				time += owner.chronometer.master.deltaTime;
				yield return null;
			}
			this._laser.Activate(owner, this._targetDirection);
			yield break;
		}

		// Token: 0x04003DC8 RID: 15816
		[SerializeField]
		private Laser _laser;

		// Token: 0x04003DC9 RID: 15817
		[SerializeField]
		private float _targetDirection;

		// Token: 0x04003DCA RID: 15818
		[SerializeField]
		private Curve _curve;
	}
}
