using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014B8 RID: 5304
	public class IsNearByToDestination : Conditional
	{
		// Token: 0x06006735 RID: 26421 RVA: 0x0012ADCC File Offset: 0x00128FCC
		public override void OnStart()
		{
			this._ownerValue = this._owner.Value;
			this._destinationValue = this._destination.Value;
		}

		// Token: 0x06006736 RID: 26422 RVA: 0x0012ADF0 File Offset: 0x00128FF0
		public override TaskStatus OnUpdate()
		{
			if (this._ownerValue == null)
			{
				return TaskStatus.Failure;
			}
			Vector2 destinationValue = this._destinationValue;
			if (Vector2.Distance(this._ownerValue.gameObject.transform.position, this._destinationValue) < this._minimumDistance)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		// Token: 0x04005349 RID: 21321
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x0400534A RID: 21322
		[SerializeField]
		private SharedVector2 _destination;

		// Token: 0x0400534B RID: 21323
		[SerializeField]
		private float _minimumDistance = 0.5f;

		// Token: 0x0400534C RID: 21324
		private Character _ownerValue;

		// Token: 0x0400534D RID: 21325
		private Vector2 _destinationValue;
	}
}
