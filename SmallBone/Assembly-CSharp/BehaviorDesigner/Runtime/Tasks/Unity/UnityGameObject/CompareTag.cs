using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015D9 RID: 5593
	[TaskCategory("Unity/GameObject")]
	[TaskDescription("Returns Success if tags match, otherwise Failure.")]
	public class CompareTag : Conditional
	{
		// Token: 0x06006B20 RID: 27424 RVA: 0x00133398 File Offset: 0x00131598
		public override TaskStatus OnUpdate()
		{
			if (!base.GetDefaultGameObject(this.targetGameObject.Value).CompareTag(this.tag.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B21 RID: 27425 RVA: 0x001333C0 File Offset: 0x001315C0
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.tag = "";
		}

		// Token: 0x040056E2 RID: 22242
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056E3 RID: 22243
		[Tooltip("The tag to compare against")]
		public SharedString tag;
	}
}
