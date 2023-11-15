using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001487 RID: 5255
	[TaskDescription("Pause or disable a behavior tree and return success after it has been stopped.")]
	[TaskIcon("{SkinColor}StopBehaviorTreeIcon.png")]
	public class StopBehaviorTree : Action
	{
		// Token: 0x06006667 RID: 26215 RVA: 0x00128574 File Offset: 0x00126774
		public override void OnStart()
		{
			Behavior[] components = base.GetDefaultGameObject(this.behaviorGameObject.Value).GetComponents<Behavior>();
			if (components.Length == 1)
			{
				this.behavior = components[0];
				return;
			}
			if (components.Length > 1)
			{
				for (int i = 0; i < components.Length; i++)
				{
					if (components[i].Group == this.group.Value)
					{
						this.behavior = components[i];
						break;
					}
				}
				if (this.behavior == null)
				{
					this.behavior = components[0];
				}
			}
		}

		// Token: 0x06006668 RID: 26216 RVA: 0x001285F3 File Offset: 0x001267F3
		public override TaskStatus OnUpdate()
		{
			if (this.behavior == null)
			{
				return TaskStatus.Failure;
			}
			this.behavior.DisableBehavior(this.pauseBehavior.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006669 RID: 26217 RVA: 0x0012861C File Offset: 0x0012681C
		public override void OnReset()
		{
			this.behaviorGameObject = null;
			this.group = 0;
			this.pauseBehavior = false;
		}

		// Token: 0x04005274 RID: 21108
		[Tooltip("The GameObject of the behavior tree that should be stopped. If null use the current behavior")]
		public SharedGameObject behaviorGameObject;

		// Token: 0x04005275 RID: 21109
		[Tooltip("The group of the behavior tree that should be stopped")]
		public SharedInt group;

		// Token: 0x04005276 RID: 21110
		[Tooltip("Should the behavior be paused or completely disabled")]
		public SharedBool pauseBehavior = false;

		// Token: 0x04005277 RID: 21111
		private Behavior behavior;
	}
}
