using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001527 RID: 5415
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedBool : Conditional
	{
		// Token: 0x060068D0 RID: 26832 RVA: 0x0012E688 File Offset: 0x0012C888
		public override TaskStatus OnUpdate()
		{
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068D1 RID: 26833 RVA: 0x0012E6B8 File Offset: 0x0012C8B8
		public override void OnReset()
		{
			this.variable = false;
			this.compareTo = false;
		}

		// Token: 0x040054BF RID: 21695
		[Tooltip("The first variable to compare")]
		public SharedBool variable;

		// Token: 0x040054C0 RID: 21696
		[Tooltip("The variable to compare to")]
		public SharedBool compareTo;
	}
}
