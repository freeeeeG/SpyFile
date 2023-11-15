using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x0200164B RID: 5707
	[TaskCategory("Unity/String")]
	[TaskDescription("Stores the length of the string")]
	public class GetLength : Action
	{
		// Token: 0x06006CDA RID: 27866 RVA: 0x00137398 File Offset: 0x00135598
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.targetString.Value.Length;
			return TaskStatus.Success;
		}

		// Token: 0x06006CDB RID: 27867 RVA: 0x001373B6 File Offset: 0x001355B6
		public override void OnReset()
		{
			this.targetString = "";
			this.storeResult = 0;
		}

		// Token: 0x040058A2 RID: 22690
		[Tooltip("The target string")]
		public SharedString targetString;

		// Token: 0x040058A3 RID: 22691
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedInt storeResult;
	}
}
