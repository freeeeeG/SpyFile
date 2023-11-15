using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x0200159C RID: 5532
	[TaskCategory("Unity/Math")]
	[TaskDescription("Flips the value of the bool.")]
	public class BoolFlip : Action
	{
		// Token: 0x06006A67 RID: 27239 RVA: 0x00131BA0 File Offset: 0x0012FDA0
		public override TaskStatus OnUpdate()
		{
			this.boolVariable.Value = !this.boolVariable.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A68 RID: 27240 RVA: 0x00131BBC File Offset: 0x0012FDBC
		public override void OnReset()
		{
			this.boolVariable.Value = false;
		}

		// Token: 0x04005627 RID: 22055
		[Tooltip("The bool to flip the value of")]
		public SharedBool boolVariable;
	}
}
