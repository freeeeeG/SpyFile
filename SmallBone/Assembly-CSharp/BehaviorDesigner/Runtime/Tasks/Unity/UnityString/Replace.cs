using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x0200164F RID: 5711
	[TaskDescription("Replaces a string with the new string")]
	[TaskCategory("Unity/String")]
	public class Replace : Action
	{
		// Token: 0x06006CE6 RID: 27878 RVA: 0x00137504 File Offset: 0x00135704
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.targetString.Value.Replace(this.oldString.Value, this.newString.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006CE7 RID: 27879 RVA: 0x00137538 File Offset: 0x00135738
		public override void OnReset()
		{
			this.targetString = "";
			this.oldString = "";
			this.newString = "";
			this.storeResult = "";
		}

		// Token: 0x040058AB RID: 22699
		[Tooltip("The target string")]
		public SharedString targetString;

		// Token: 0x040058AC RID: 22700
		[Tooltip("The old replace")]
		public SharedString oldString;

		// Token: 0x040058AD RID: 22701
		[Tooltip("The new string")]
		public SharedString newString;

		// Token: 0x040058AE RID: 22702
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedString storeResult;
	}
}
