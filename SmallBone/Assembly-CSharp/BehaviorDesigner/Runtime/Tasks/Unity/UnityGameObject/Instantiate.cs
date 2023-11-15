using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015E1 RID: 5601
	[TaskCategory("Unity/GameObject")]
	[TaskDescription("Instantiates a new GameObject. Returns Success.")]
	public class Instantiate : Action
	{
		// Token: 0x06006B38 RID: 27448 RVA: 0x0013361C File Offset: 0x0013181C
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = UnityEngine.Object.Instantiate<GameObject>(this.targetGameObject.Value, this.position.Value, this.rotation.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006B39 RID: 27449 RVA: 0x00133650 File Offset: 0x00131850
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.position = Vector3.zero;
			this.rotation = Quaternion.identity;
		}

		// Token: 0x040056F3 RID: 22259
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056F4 RID: 22260
		[Tooltip("The position of the new GameObject")]
		public SharedVector3 position;

		// Token: 0x040056F5 RID: 22261
		[Tooltip("The rotation of the new GameObject")]
		public SharedQuaternion rotation = Quaternion.identity;

		// Token: 0x040056F6 RID: 22262
		[SharedRequired]
		[Tooltip("The instantiated GameObject")]
		public SharedGameObject storeResult;
	}
}
