using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001549 RID: 5449
	[TaskDescription("Gets the GameObject from the Transform component. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SharedTransformToGameObject : Action
	{
		// Token: 0x06006937 RID: 26935 RVA: 0x0012F21D File Offset: 0x0012D41D
		public override TaskStatus OnUpdate()
		{
			if (this.sharedTransform.Value == null)
			{
				return TaskStatus.Failure;
			}
			this.sharedGameObject.Value = this.sharedTransform.Value.gameObject;
			return TaskStatus.Success;
		}

		// Token: 0x06006938 RID: 26936 RVA: 0x0012F250 File Offset: 0x0012D450
		public override void OnReset()
		{
			this.sharedTransform = null;
			this.sharedGameObject = null;
		}

		// Token: 0x04005504 RID: 21764
		[Tooltip("The Transform component")]
		public SharedTransform sharedTransform;

		// Token: 0x04005505 RID: 21765
		[RequiredField]
		[Tooltip("The GameObject to set")]
		public SharedGameObject sharedGameObject;
	}
}
