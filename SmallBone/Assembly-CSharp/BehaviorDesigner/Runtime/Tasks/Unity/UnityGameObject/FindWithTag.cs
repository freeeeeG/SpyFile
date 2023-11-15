using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015DE RID: 5598
	[TaskCategory("Unity/GameObject")]
	[TaskDescription("Finds a GameObject by tag. Returns success if an object is found.")]
	public class FindWithTag : Action
	{
		// Token: 0x06006B2F RID: 27439 RVA: 0x001334F4 File Offset: 0x001316F4
		public override TaskStatus OnUpdate()
		{
			if (this.random.Value)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag(this.tag.Value);
				if (array == null || array.Length == 0)
				{
					return TaskStatus.Failure;
				}
				this.storeValue.Value = array[UnityEngine.Random.Range(0, array.Length)];
			}
			else
			{
				this.storeValue.Value = GameObject.FindWithTag(this.tag.Value);
			}
			if (!(this.storeValue.Value != null))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B30 RID: 27440 RVA: 0x00133571 File Offset: 0x00131771
		public override void OnReset()
		{
			this.tag.Value = null;
			this.storeValue.Value = null;
		}

		// Token: 0x040056EB RID: 22251
		[Tooltip("The tag of the GameObject to find")]
		public SharedString tag;

		// Token: 0x040056EC RID: 22252
		[Tooltip("Should a random GameObject be found?")]
		public SharedBool random;

		// Token: 0x040056ED RID: 22253
		[RequiredField]
		[Tooltip("The object found by name")]
		public SharedGameObject storeValue;
	}
}
