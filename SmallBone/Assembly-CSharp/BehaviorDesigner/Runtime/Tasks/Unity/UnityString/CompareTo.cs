using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x02001649 RID: 5705
	[TaskCategory("Unity/String")]
	[TaskDescription("Compares the first string to the second string. Returns an int which indicates whether the first string precedes, matches, or follows the second string.")]
	public class CompareTo : Action
	{
		// Token: 0x06006CD3 RID: 27859 RVA: 0x00137287 File Offset: 0x00135487
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.firstString.Value.CompareTo(this.secondString.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006CD4 RID: 27860 RVA: 0x001372B0 File Offset: 0x001354B0
		public override void OnReset()
		{
			this.firstString = "";
			this.secondString = "";
			this.storeResult = 0;
		}

		// Token: 0x0400589B RID: 22683
		[Tooltip("The string to compare")]
		public SharedString firstString;

		// Token: 0x0400589C RID: 22684
		[Tooltip("The string to compare to")]
		public SharedString secondString;

		// Token: 0x0400589D RID: 22685
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedInt storeResult;
	}
}
