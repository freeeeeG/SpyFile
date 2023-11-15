using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x0200164E RID: 5710
	[TaskCategory("Unity/String")]
	[TaskDescription("Returns success if the string is null or empty")]
	public class IsNullOrEmpty : Conditional
	{
		// Token: 0x06006CE3 RID: 27875 RVA: 0x001374DB File Offset: 0x001356DB
		public override TaskStatus OnUpdate()
		{
			if (!string.IsNullOrEmpty(this.targetString.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006CE4 RID: 27876 RVA: 0x001374F2 File Offset: 0x001356F2
		public override void OnReset()
		{
			this.targetString = "";
		}

		// Token: 0x040058AA RID: 22698
		[Tooltip("The target string")]
		public SharedString targetString;
	}
}
