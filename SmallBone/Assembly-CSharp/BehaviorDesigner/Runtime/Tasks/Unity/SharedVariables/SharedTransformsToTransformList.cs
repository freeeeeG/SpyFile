using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200154A RID: 5450
	[TaskDescription("Sets the SharedTransformList values from the Transforms. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SharedTransformsToTransformList : Action
	{
		// Token: 0x0600693A RID: 26938 RVA: 0x0012F260 File Offset: 0x0012D460
		public override void OnAwake()
		{
			this.storedTransformList.Value = new List<Transform>();
		}

		// Token: 0x0600693B RID: 26939 RVA: 0x0012F274 File Offset: 0x0012D474
		public override TaskStatus OnUpdate()
		{
			if (this.transforms == null || this.transforms.Length == 0)
			{
				return TaskStatus.Failure;
			}
			this.storedTransformList.Value.Clear();
			for (int i = 0; i < this.transforms.Length; i++)
			{
				this.storedTransformList.Value.Add(this.transforms[i].Value);
			}
			return TaskStatus.Success;
		}

		// Token: 0x0600693C RID: 26940 RVA: 0x0012F2D5 File Offset: 0x0012D4D5
		public override void OnReset()
		{
			this.transforms = null;
			this.storedTransformList = null;
		}

		// Token: 0x04005506 RID: 21766
		[Tooltip("The Transforms value")]
		public SharedTransform[] transforms;

		// Token: 0x04005507 RID: 21767
		[RequiredField]
		[Tooltip("The SharedTransformList to set")]
		public SharedTransformList storedTransformList;
	}
}
