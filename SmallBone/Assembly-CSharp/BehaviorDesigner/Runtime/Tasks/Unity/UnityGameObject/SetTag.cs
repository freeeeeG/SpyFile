using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015E4 RID: 5604
	[TaskCategory("Unity/GameObject")]
	[TaskDescription("Sets the GameObject tag. Returns Success.")]
	public class SetTag : Action
	{
		// Token: 0x06006B41 RID: 27457 RVA: 0x0013375A File Offset: 0x0013195A
		public override TaskStatus OnUpdate()
		{
			base.GetDefaultGameObject(this.targetGameObject.Value).tag = this.tag.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006B42 RID: 27458 RVA: 0x0013377E File Offset: 0x0013197E
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.tag = "";
		}

		// Token: 0x040056FC RID: 22268
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056FD RID: 22269
		[Tooltip("The GameObject tag")]
		public SharedString tag;
	}
}
