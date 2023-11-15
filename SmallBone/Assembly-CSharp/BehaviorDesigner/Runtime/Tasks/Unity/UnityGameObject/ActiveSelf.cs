using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015D7 RID: 5591
	[TaskDescription("Returns Success if the GameObject is active in the hierarchy, otherwise Failure.")]
	[TaskCategory("Unity/GameObject")]
	public class ActiveSelf : Conditional
	{
		// Token: 0x06006B1A RID: 27418 RVA: 0x0013333B File Offset: 0x0013153B
		public override TaskStatus OnUpdate()
		{
			if (!base.GetDefaultGameObject(this.targetGameObject.Value).activeSelf)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B1B RID: 27419 RVA: 0x00133358 File Offset: 0x00131558
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040056DF RID: 22239
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;
	}
}
