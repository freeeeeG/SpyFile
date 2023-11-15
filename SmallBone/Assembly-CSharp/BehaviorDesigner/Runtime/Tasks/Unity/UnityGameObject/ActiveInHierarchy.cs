using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015D6 RID: 5590
	[TaskDescription("Returns Success if the GameObject is active in the hierarchy, otherwise Failure.")]
	[TaskCategory("Unity/GameObject")]
	public class ActiveInHierarchy : Conditional
	{
		// Token: 0x06006B17 RID: 27415 RVA: 0x00133315 File Offset: 0x00131515
		public override TaskStatus OnUpdate()
		{
			if (!base.GetDefaultGameObject(this.targetGameObject.Value).activeInHierarchy)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B18 RID: 27416 RVA: 0x00133332 File Offset: 0x00131532
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040056DE RID: 22238
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;
	}
}
