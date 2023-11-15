using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015B8 RID: 5560
	[TaskCategory("Unity/Light")]
	[TaskDescription("Stores the range of the light.")]
	public class GetRange : Action
	{
		// Token: 0x06006AAE RID: 27310 RVA: 0x001327C0 File Offset: 0x001309C0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AAF RID: 27311 RVA: 0x00132800 File Offset: 0x00130A00
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.storeValue = this.light.range;
			return TaskStatus.Success;
		}

		// Token: 0x06006AB0 RID: 27312 RVA: 0x00132833 File Offset: 0x00130A33
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x0400568A RID: 22154
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400568B RID: 22155
		[RequiredField]
		[Tooltip("The range to store")]
		public SharedFloat storeValue;

		// Token: 0x0400568C RID: 22156
		private Light light;

		// Token: 0x0400568D RID: 22157
		private GameObject prevGameObject;
	}
}
