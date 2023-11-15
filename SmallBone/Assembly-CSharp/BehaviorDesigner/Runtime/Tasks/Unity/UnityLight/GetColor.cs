using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015B5 RID: 5557
	[TaskDescription("Stores the color of the light.")]
	[TaskCategory("Unity/Light")]
	public class GetColor : Action
	{
		// Token: 0x06006AA2 RID: 27298 RVA: 0x0013261C File Offset: 0x0013081C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006AA3 RID: 27299 RVA: 0x0013265C File Offset: 0x0013085C
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.storeValue = this.light.color;
			return TaskStatus.Success;
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x0013268F File Offset: 0x0013088F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Color.white;
		}

		// Token: 0x0400567E RID: 22142
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400567F RID: 22143
		[Tooltip("The color to store")]
		[RequiredField]
		public SharedColor storeValue;

		// Token: 0x04005680 RID: 22144
		private Light light;

		// Token: 0x04005681 RID: 22145
		private GameObject prevGameObject;
	}
}
