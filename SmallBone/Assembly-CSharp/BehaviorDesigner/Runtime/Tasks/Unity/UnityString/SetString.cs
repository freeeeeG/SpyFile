using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x02001650 RID: 5712
	[TaskDescription("Sets the variable string to the value string.")]
	[TaskCategory("Unity/String")]
	public class SetString : Action
	{
		// Token: 0x06006CE9 RID: 27881 RVA: 0x00137585 File Offset: 0x00135785
		public override TaskStatus OnUpdate()
		{
			this.variable.Value = this.value.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006CEA RID: 27882 RVA: 0x0013759E File Offset: 0x0013579E
		public override void OnReset()
		{
			this.variable = "";
			this.value = "";
		}

		// Token: 0x040058AF RID: 22703
		[Tooltip("The target string")]
		[RequiredField]
		public SharedString variable;

		// Token: 0x040058B0 RID: 22704
		[Tooltip("The value string")]
		public SharedString value;
	}
}
