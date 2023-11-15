using System;
using Characters;
using Characters.AI;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014BB RID: 5307
	public class CanMoveToDirection : Conditional
	{
		// Token: 0x0600673E RID: 26430 RVA: 0x0012AEAE File Offset: 0x001290AE
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
			this._minimumDistanceValue = this._minimumDistance.Value;
		}

		// Token: 0x0600673F RID: 26431 RVA: 0x0012AED4 File Offset: 0x001290D4
		public override TaskStatus OnUpdate()
		{
			Vector2 value = this._direction.Value;
			if (!Precondition.CanMoveToDirection(this._ownerValue, value, this._minimumDistanceValue))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x04005353 RID: 21331
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005354 RID: 21332
		[SerializeField]
		private SharedVector2 _direction;

		// Token: 0x04005355 RID: 21333
		[SerializeField]
		private SharedFloat _minimumDistance = 0.5f;

		// Token: 0x04005356 RID: 21334
		private Character _ownerValue;

		// Token: 0x04005357 RID: 21335
		private float _minimumDistanceValue;
	}
}
