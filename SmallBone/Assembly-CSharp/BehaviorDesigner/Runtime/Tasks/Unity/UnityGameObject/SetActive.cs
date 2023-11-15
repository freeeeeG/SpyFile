using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015E3 RID: 5603
	[TaskCategory("Unity/GameObject")]
	[TaskDescription("Activates/Deactivates the GameObject. Returns Success.")]
	public class SetActive : Action
	{
		// Token: 0x06006B3E RID: 27454 RVA: 0x00133721 File Offset: 0x00131921
		public override TaskStatus OnUpdate()
		{
			base.GetDefaultGameObject(this.targetGameObject.Value).SetActive(this.active.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006B3F RID: 27455 RVA: 0x00133745 File Offset: 0x00131945
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.active = false;
		}

		// Token: 0x040056FA RID: 22266
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056FB RID: 22267
		[Tooltip("Active state of the GameObject")]
		public SharedBool active;
	}
}
