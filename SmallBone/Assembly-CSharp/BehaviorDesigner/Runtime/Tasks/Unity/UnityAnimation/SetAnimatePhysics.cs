using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x02001642 RID: 5698
	[TaskCategory("Unity/Animation")]
	[TaskDescription("Sets animate physics to the specified value. Returns Success.")]
	public class SetAnimatePhysics : Action
	{
		// Token: 0x06006CBC RID: 27836 RVA: 0x00136F6C File Offset: 0x0013516C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006CBD RID: 27837 RVA: 0x00136FAC File Offset: 0x001351AC
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			this.animation.animatePhysics = this.animatePhysics.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006CBE RID: 27838 RVA: 0x00136FDF File Offset: 0x001351DF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animatePhysics.Value = false;
		}

		// Token: 0x04005886 RID: 22662
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005887 RID: 22663
		[Tooltip("Are animations executed in the physics loop?")]
		public SharedBool animatePhysics;

		// Token: 0x04005888 RID: 22664
		private Animation animation;

		// Token: 0x04005889 RID: 22665
		private GameObject prevGameObject;
	}
}
