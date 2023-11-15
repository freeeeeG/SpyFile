using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015DD RID: 5597
	[TaskCategory("Unity/GameObject")]
	[TaskDescription("Finds a GameObject by tag. Returns Success.")]
	public class FindGameObjectsWithTag : Action
	{
		// Token: 0x06006B2C RID: 27436 RVA: 0x00133498 File Offset: 0x00131698
		public override TaskStatus OnUpdate()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(this.tag.Value);
			for (int i = 0; i < array.Length; i++)
			{
				this.storeValue.Value.Add(array[i]);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B2D RID: 27437 RVA: 0x001334D8 File Offset: 0x001316D8
		public override void OnReset()
		{
			this.tag.Value = null;
			this.storeValue.Value = null;
		}

		// Token: 0x040056E9 RID: 22249
		[Tooltip("The tag of the GameObject to find")]
		public SharedString tag;

		// Token: 0x040056EA RID: 22250
		[RequiredField]
		[Tooltip("The objects found by name")]
		public SharedGameObjectList storeValue;
	}
}
