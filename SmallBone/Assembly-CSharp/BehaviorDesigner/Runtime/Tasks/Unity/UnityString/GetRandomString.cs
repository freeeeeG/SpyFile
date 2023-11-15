using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x0200164C RID: 5708
	[TaskCategory("Unity/String")]
	[TaskDescription("Randomly selects a string from the array of strings.")]
	public class GetRandomString : Action
	{
		// Token: 0x06006CDD RID: 27869 RVA: 0x001373D4 File Offset: 0x001355D4
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.source[UnityEngine.Random.Range(0, this.source.Length)].Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006CDE RID: 27870 RVA: 0x001373FC File Offset: 0x001355FC
		public override void OnReset()
		{
			this.source = null;
			this.storeResult = null;
		}

		// Token: 0x040058A4 RID: 22692
		[Tooltip("The array of strings")]
		public SharedString[] source;

		// Token: 0x040058A5 RID: 22693
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedString storeResult;
	}
}
