using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
	// Token: 0x02001652 RID: 5714
	public sealed class GetTargetVelocity : Action
	{
		// Token: 0x06006CEF RID: 27887 RVA: 0x00137635 File Offset: 0x00135835
		public override TaskStatus OnUpdate()
		{
			this._storedVelocity.SetValue(this._target.Value.movement.velocity);
			return TaskStatus.Success;
		}

		// Token: 0x040058B3 RID: 22707
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x040058B4 RID: 22708
		[SerializeField]
		private SharedVector2 _storedVelocity;
	}
}
