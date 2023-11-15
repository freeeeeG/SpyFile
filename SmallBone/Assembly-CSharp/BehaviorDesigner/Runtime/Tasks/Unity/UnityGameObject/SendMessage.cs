using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015E2 RID: 5602
	[TaskDescription("Sends a message to the target GameObject. Returns Success.")]
	[TaskCategory("Unity/GameObject")]
	public class SendMessage : Action
	{
		// Token: 0x06006B3B RID: 27451 RVA: 0x00133694 File Offset: 0x00131894
		public override TaskStatus OnUpdate()
		{
			if (this.value.Value != null)
			{
				base.GetDefaultGameObject(this.targetGameObject.Value).SendMessage(this.message.Value, this.value.Value.value.GetValue());
			}
			else
			{
				base.GetDefaultGameObject(this.targetGameObject.Value).SendMessage(this.message.Value);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B3C RID: 27452 RVA: 0x00133708 File Offset: 0x00131908
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.message = "";
		}

		// Token: 0x040056F7 RID: 22263
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056F8 RID: 22264
		[Tooltip("The message to send")]
		public SharedString message;

		// Token: 0x040056F9 RID: 22265
		[Tooltip("The value to send")]
		public SharedGenericVariable value;
	}
}
