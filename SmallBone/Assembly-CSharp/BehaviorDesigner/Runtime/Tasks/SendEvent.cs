using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200147E RID: 5246
	[HelpURL("https://www.opsive.com/support/documentation/behavior-designer/events/")]
	[TaskIcon("{SkinColor}SendEventIcon.png")]
	[TaskDescription("Sends an event to the behavior tree, returns success after sending the event.")]
	public class SendEvent : Action
	{
		// Token: 0x06006646 RID: 26182 RVA: 0x00127C30 File Offset: 0x00125E30
		public override void OnStart()
		{
			BehaviorTree[] components = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponents<BehaviorTree>();
			if (components.Length == 1)
			{
				this.behaviorTree = components[0];
				return;
			}
			if (components.Length > 1)
			{
				for (int i = 0; i < components.Length; i++)
				{
					if (components[i].Group == this.group.Value)
					{
						this.behaviorTree = components[i];
						break;
					}
				}
				if (this.behaviorTree == null)
				{
					this.behaviorTree = components[0];
				}
			}
		}

		// Token: 0x06006647 RID: 26183 RVA: 0x00127CB0 File Offset: 0x00125EB0
		public override TaskStatus OnUpdate()
		{
			if (this.argument1 == null || this.argument1.IsNone)
			{
				this.behaviorTree.SendEvent(this.eventName.Value);
			}
			else if (this.argument2 == null || this.argument2.IsNone)
			{
				this.behaviorTree.SendEvent<object>(this.eventName.Value, this.argument1.GetValue());
			}
			else if (this.argument3 == null || this.argument3.IsNone)
			{
				this.behaviorTree.SendEvent<object, object>(this.eventName.Value, this.argument1.GetValue(), this.argument2.GetValue());
			}
			else
			{
				this.behaviorTree.SendEvent<object, object, object>(this.eventName.Value, this.argument1.GetValue(), this.argument2.GetValue(), this.argument3.GetValue());
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006648 RID: 26184 RVA: 0x00127DA0 File Offset: 0x00125FA0
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.eventName = "";
		}

		// Token: 0x0400524F RID: 21071
		[Tooltip("The GameObject of the behavior tree that should have the event sent to it. If null use the current behavior")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005250 RID: 21072
		[Tooltip("The event to send")]
		public SharedString eventName;

		// Token: 0x04005251 RID: 21073
		[Tooltip("The group of the behavior tree that the event should be sent to")]
		public SharedInt group;

		// Token: 0x04005252 RID: 21074
		[SharedRequired]
		[Tooltip("Optionally specify a first argument to send")]
		public SharedVariable argument1;

		// Token: 0x04005253 RID: 21075
		[SharedRequired]
		[Tooltip("Optionally specify a second argument to send")]
		public SharedVariable argument2;

		// Token: 0x04005254 RID: 21076
		[Tooltip("Optionally specify a third argument to send")]
		[SharedRequired]
		public SharedVariable argument3;

		// Token: 0x04005255 RID: 21077
		private BehaviorTree behaviorTree;
	}
}
