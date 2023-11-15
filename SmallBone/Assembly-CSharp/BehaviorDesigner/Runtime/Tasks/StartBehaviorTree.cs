using System;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001486 RID: 5254
	[TaskDescription("Start a new behavior tree and return success after it has been started.")]
	[TaskIcon("{SkinColor}StartBehaviorTreeIcon.png")]
	public class StartBehaviorTree : Action
	{
		// Token: 0x06006661 RID: 26209 RVA: 0x001283B8 File Offset: 0x001265B8
		public override void OnStart()
		{
			Behavior[] components = base.GetDefaultGameObject(this.behaviorGameObject.Value).GetComponents<Behavior>();
			if (components.Length == 1)
			{
				this.behavior = components[0];
			}
			else if (components.Length > 1)
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
			if (this.behavior != null)
			{
				List<SharedVariable> allVariables = base.Owner.GetAllVariables();
				if (allVariables != null && this.synchronizeVariables.Value)
				{
					for (int j = 0; j < allVariables.Count; j++)
					{
						this.behavior.SetVariableValue(allVariables[j].Name, allVariables[j]);
					}
				}
				this.behavior.EnableBehavior();
				if (this.waitForCompletion.Value)
				{
					this.behaviorComplete = false;
					this.behavior.OnBehaviorEnd += this.BehaviorEnded;
				}
			}
		}

		// Token: 0x06006662 RID: 26210 RVA: 0x001284CA File Offset: 0x001266CA
		public override TaskStatus OnUpdate()
		{
			if (this.behavior == null)
			{
				return TaskStatus.Failure;
			}
			if (this.waitForCompletion.Value && !this.behaviorComplete)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006663 RID: 26211 RVA: 0x001284F4 File Offset: 0x001266F4
		private void BehaviorEnded(Behavior behavior)
		{
			this.behaviorComplete = true;
		}

		// Token: 0x06006664 RID: 26212 RVA: 0x001284FD File Offset: 0x001266FD
		public override void OnEnd()
		{
			if (this.behavior != null && this.waitForCompletion.Value)
			{
				this.behavior.OnBehaviorEnd -= this.BehaviorEnded;
			}
		}

		// Token: 0x06006665 RID: 26213 RVA: 0x00128531 File Offset: 0x00126731
		public override void OnReset()
		{
			this.behaviorGameObject = null;
			this.group = 0;
			this.waitForCompletion = false;
			this.synchronizeVariables = false;
		}

		// Token: 0x0400526E RID: 21102
		[Tooltip("The GameObject of the behavior tree that should be started. If null use the current behavior")]
		public SharedGameObject behaviorGameObject;

		// Token: 0x0400526F RID: 21103
		[Tooltip("The group of the behavior tree that should be started")]
		public SharedInt group;

		// Token: 0x04005270 RID: 21104
		[Tooltip("Should this task wait for the behavior tree to complete?")]
		public SharedBool waitForCompletion = false;

		// Token: 0x04005271 RID: 21105
		[Tooltip("Should the variables be synchronized?")]
		public SharedBool synchronizeVariables;

		// Token: 0x04005272 RID: 21106
		private bool behaviorComplete;

		// Token: 0x04005273 RID: 21107
		private Behavior behavior;
	}
}
