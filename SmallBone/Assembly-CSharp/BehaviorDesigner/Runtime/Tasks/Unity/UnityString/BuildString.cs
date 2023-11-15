using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x02001648 RID: 5704
	[TaskDescription("Creates a string from multiple other strings.")]
	[TaskCategory("Unity/String")]
	public class BuildString : Action
	{
		// Token: 0x06006CD0 RID: 27856 RVA: 0x0013722C File Offset: 0x0013542C
		public override TaskStatus OnUpdate()
		{
			for (int i = 0; i < this.source.Length; i++)
			{
				SharedString sharedString = this.storeResult;
				string value = sharedString.Value;
				SharedString sharedString2 = this.source[i];
				sharedString.Value = value + ((sharedString2 != null) ? sharedString2.ToString() : null);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006CD1 RID: 27857 RVA: 0x00137277 File Offset: 0x00135477
		public override void OnReset()
		{
			this.source = null;
			this.storeResult = null;
		}

		// Token: 0x04005899 RID: 22681
		[Tooltip("The array of strings")]
		public SharedString[] source;

		// Token: 0x0400589A RID: 22682
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedString storeResult;
	}
}
