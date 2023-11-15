using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x02001644 RID: 5700
	[TaskDescription("Stops an animation. Stops all animations if animationName is blank. Returns Success.")]
	[TaskCategory("Unity/Animation")]
	public class Stop : Action
	{
		// Token: 0x06006CC4 RID: 27844 RVA: 0x00137074 File Offset: 0x00135274
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006CC5 RID: 27845 RVA: 0x001370B4 File Offset: 0x001352B4
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			if (string.IsNullOrEmpty(this.animationName.Value))
			{
				this.animation.Stop();
			}
			else
			{
				this.animation.Stop(this.animationName.Value);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006CC6 RID: 27846 RVA: 0x00137111 File Offset: 0x00135311
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animationName = "";
		}

		// Token: 0x0400588E RID: 22670
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400588F RID: 22671
		[Tooltip("The name of the animation")]
		public SharedString animationName;

		// Token: 0x04005890 RID: 22672
		private Animation animation;

		// Token: 0x04005891 RID: 22673
		private GameObject prevGameObject;
	}
}
