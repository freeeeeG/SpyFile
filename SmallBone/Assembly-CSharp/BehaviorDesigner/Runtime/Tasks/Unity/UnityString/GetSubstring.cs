using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x0200164D RID: 5709
	[TaskDescription("Stores a substring of the target string")]
	[TaskCategory("Unity/String")]
	public class GetSubstring : Action
	{
		// Token: 0x06006CE0 RID: 27872 RVA: 0x0013740C File Offset: 0x0013560C
		public override TaskStatus OnUpdate()
		{
			if (this.length.Value != -1)
			{
				this.storeResult.Value = this.targetString.Value.Substring(this.startIndex.Value, this.length.Value);
			}
			else
			{
				this.storeResult.Value = this.targetString.Value.Substring(this.startIndex.Value);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006CE1 RID: 27873 RVA: 0x00137481 File Offset: 0x00135681
		public override void OnReset()
		{
			this.targetString = "";
			this.startIndex = 0;
			this.length = -1;
			this.storeResult = "";
		}

		// Token: 0x040058A6 RID: 22694
		[Tooltip("The target string")]
		public SharedString targetString;

		// Token: 0x040058A7 RID: 22695
		[Tooltip("The start substring index")]
		public SharedInt startIndex = 0;

		// Token: 0x040058A8 RID: 22696
		[Tooltip("The length of the substring. Don't use if -1")]
		public SharedInt length = -1;

		// Token: 0x040058A9 RID: 22697
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedString storeResult;
	}
}
