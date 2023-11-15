using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015E0 RID: 5600
	[TaskCategory("Unity/GameObject")]
	[TaskDescription("Stores the GameObject tag. Returns Success.")]
	public class GetTag : Action
	{
		// Token: 0x06006B35 RID: 27445 RVA: 0x001335DF File Offset: 0x001317DF
		public override TaskStatus OnUpdate()
		{
			this.storeValue.Value = base.GetDefaultGameObject(this.targetGameObject.Value).tag;
			return TaskStatus.Success;
		}

		// Token: 0x06006B36 RID: 27446 RVA: 0x00133603 File Offset: 0x00131803
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = "";
		}

		// Token: 0x040056F1 RID: 22257
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056F2 RID: 22258
		[RequiredField]
		[Tooltip("Active state of the GameObject")]
		public SharedString storeValue;
	}
}
