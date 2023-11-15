using System;
using Characters.AI;
using Level;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FD2 RID: 4050
	public class SetTripleDancePosition : CharacterOperation
	{
		// Token: 0x06004E5D RID: 20061 RVA: 0x000EAB6C File Offset: 0x000E8D6C
		public override void Run(Character owner)
		{
			Character target = this._controller.target;
			if (target == null)
			{
				return;
			}
			Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			float num = target.transform.position.x;
			num += this._offsetX;
			this.Evaluate(ref num);
			float num2 = bounds.max.y;
			num2 += this._offsetY;
			this._object.position = new Vector2(num, num2);
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x000EABFC File Offset: 0x000E8DFC
		private void Evaluate(ref float x)
		{
			Bounds bounds = Map.Instance.bounds;
			float num = bounds.max.x - this._minDistanceFromSide;
			float num2 = bounds.min.x + this._minDistanceFromSide;
			if (num < x)
			{
				x = num;
			}
			if (num2 > x)
			{
				x = num2;
			}
		}

		// Token: 0x04003E69 RID: 15977
		[SerializeField]
		private AIController _controller;

		// Token: 0x04003E6A RID: 15978
		[SerializeField]
		private Transform _object;

		// Token: 0x04003E6B RID: 15979
		[SerializeField]
		private float _minDistanceFromSide;

		// Token: 0x04003E6C RID: 15980
		[SerializeField]
		private float _offsetY;

		// Token: 0x04003E6D RID: 15981
		[SerializeField]
		private float _offsetX;
	}
}
