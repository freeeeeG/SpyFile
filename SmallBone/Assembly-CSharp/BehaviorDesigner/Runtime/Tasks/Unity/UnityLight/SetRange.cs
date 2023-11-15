using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015C1 RID: 5569
	[TaskDescription("Sets the range of the light.")]
	[TaskCategory("Unity/Light")]
	public class SetRange : Action
	{
		// Token: 0x06006AD2 RID: 27346 RVA: 0x00132C9C File Offset: 0x00130E9C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AD3 RID: 27347 RVA: 0x00132CDC File Offset: 0x00130EDC
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.range = this.range.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006AD4 RID: 27348 RVA: 0x00132D0F File Offset: 0x00130F0F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.range = 0f;
		}

		// Token: 0x040056AE RID: 22190
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056AF RID: 22191
		[Tooltip("The range to set")]
		public SharedFloat range;

		// Token: 0x040056B0 RID: 22192
		private Light light;

		// Token: 0x040056B1 RID: 22193
		private GameObject prevGameObject;
	}
}
