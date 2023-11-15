using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001479 RID: 5241
	[TaskDescription("Restarts a behavior tree, returns success after it has been restarted.")]
	[TaskIcon("{SkinColor}RestartBehaviorTreeIcon.png")]
	public class RestartBehaviorTree : Action
	{
		// Token: 0x0600662C RID: 26156 RVA: 0x00127760 File Offset: 0x00125960
		public override void OnAwake()
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

		// Token: 0x0600662D RID: 26157 RVA: 0x001277DF File Offset: 0x001259DF
		public override TaskStatus OnUpdate()
		{
			if (this.behavior == null)
			{
				return TaskStatus.Failure;
			}
			this.behavior.DisableBehavior();
			this.behavior.EnableBehavior();
			return TaskStatus.Success;
		}

		// Token: 0x0600662E RID: 26158 RVA: 0x00127808 File Offset: 0x00125A08
		public override void OnReset()
		{
			this.behavior = null;
		}

		// Token: 0x04005231 RID: 21041
		[Tooltip("The GameObject of the behavior tree that should be restarted. If null use the current behavior")]
		public SharedGameObject behaviorGameObject;

		// Token: 0x04005232 RID: 21042
		[Tooltip("The group of the behavior tree that should be restarted")]
		public SharedInt group;

		// Token: 0x04005233 RID: 21043
		private Behavior behavior;
	}
}
