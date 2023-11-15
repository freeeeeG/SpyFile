using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001464 RID: 5220
	public class FlyToDestination : Action
	{
		// Token: 0x060065F4 RID: 26100 RVA: 0x00126975 File Offset: 0x00124B75
		public override void OnStart()
		{
			this._ownerValue = this._owner.Value;
			this._destinationValue = this._destination.Value;
		}

		// Token: 0x060065F5 RID: 26101 RVA: 0x0012699C File Offset: 0x00124B9C
		public override TaskStatus OnUpdate()
		{
			if (this._ownerValue == null || this._destinationValue == null)
			{
				return TaskStatus.Failure;
			}
			if (Vector2.Distance(this._destinationValue.position, this._ownerValue.transform.position) <= this.minimumDistanceValue)
			{
				return TaskStatus.Success;
			}
			Vector3 normalized = (this._destinationValue.position - this._ownerValue.transform.position).normalized;
			this._ownerValue.movement.Move(normalized);
			return TaskStatus.Running;
		}

		// Token: 0x040051E9 RID: 20969
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040051EA RID: 20970
		[SerializeField]
		private SharedTransform _destination;

		// Token: 0x040051EB RID: 20971
		[SerializeField]
		private float minimumDistanceValue = 0.1f;

		// Token: 0x040051EC RID: 20972
		private Character _ownerValue;

		// Token: 0x040051ED RID: 20973
		private Transform _destinationValue;
	}
}
