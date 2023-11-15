using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x0200159B RID: 5531
	[TaskCategory("Unity/Math")]
	[TaskDescription("Performs a comparison between two bools.")]
	[TaskIcon("Assets/Behavior Designer/Icon/BoolComparison.png")]
	public class BoolComparison : Conditional
	{
		// Token: 0x06006A64 RID: 27236 RVA: 0x00131B69 File Offset: 0x0012FD69
		public override TaskStatus OnUpdate()
		{
			if (this.bool1.Value != this.bool2.Value)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A65 RID: 27237 RVA: 0x00131B86 File Offset: 0x0012FD86
		public override void OnReset()
		{
			this.bool1 = false;
			this.bool2 = false;
		}

		// Token: 0x04005625 RID: 22053
		[Tooltip("The first bool")]
		public SharedBool bool1;

		// Token: 0x04005626 RID: 22054
		[Tooltip("The second bool")]
		public SharedBool bool2;
	}
}
