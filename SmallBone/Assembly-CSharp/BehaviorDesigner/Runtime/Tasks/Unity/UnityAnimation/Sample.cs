using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x02001641 RID: 5697
	[TaskCategory("Unity/Animation")]
	[TaskDescription("Samples animations at the current state. Returns Success.")]
	public class Sample : Action
	{
		// Token: 0x06006CB8 RID: 27832 RVA: 0x00136EF8 File Offset: 0x001350F8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006CB9 RID: 27833 RVA: 0x00136F38 File Offset: 0x00135138
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			this.animation.Sample();
			return TaskStatus.Success;
		}

		// Token: 0x06006CBA RID: 27834 RVA: 0x00136F60 File Offset: 0x00135160
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x04005883 RID: 22659
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005884 RID: 22660
		private Animation animation;

		// Token: 0x04005885 RID: 22661
		private GameObject prevGameObject;
	}
}
