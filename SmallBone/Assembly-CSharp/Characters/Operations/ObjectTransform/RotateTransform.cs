using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FAE RID: 4014
	public class RotateTransform : CharacterOperation
	{
		// Token: 0x06004DE9 RID: 19945 RVA: 0x000E9167 File Offset: 0x000E7367
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CRotate(owner));
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x000E9178 File Offset: 0x000E7378
		private int GetDirectionSign()
		{
			switch (this._directipnType)
			{
			case RotateTransform.Direction.Left:
				return -1;
			case RotateTransform.Direction.Right:
				return 1;
			case RotateTransform.Direction.Random:
				if (!MMMaths.RandomBool())
				{
					return -1;
				}
				return 1;
			default:
				return 1;
			}
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x000E91B0 File Offset: 0x000E73B0
		private IEnumerator CRotate(Character owner)
		{
			float elpased = 0f;
			int direction = this.GetDirectionSign();
			do
			{
				yield return null;
				this._transform.Rotate(Vector3.forward * this._speed * (float)direction);
				elpased += owner.chronometer.master.deltaTime;
			}
			while (elpased < this._duration);
			yield break;
		}

		// Token: 0x04003DD7 RID: 15831
		[SerializeField]
		private RotateTransform.Direction _directipnType = RotateTransform.Direction.Right;

		// Token: 0x04003DD8 RID: 15832
		[SerializeField]
		private Transform _transform;

		// Token: 0x04003DD9 RID: 15833
		[SerializeField]
		private float _duration;

		// Token: 0x04003DDA RID: 15834
		[SerializeField]
		private float _speed;

		// Token: 0x02000FAF RID: 4015
		private enum Direction
		{
			// Token: 0x04003DDC RID: 15836
			Left,
			// Token: 0x04003DDD RID: 15837
			Right,
			// Token: 0x04003DDE RID: 15838
			Random
		}
	}
}
