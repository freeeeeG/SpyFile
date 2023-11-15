using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
	// Token: 0x020015BC RID: 5564
	[TaskCategory("Unity/Light")]
	[TaskDescription("Sets the color of the light.")]
	public class SetColor : Action
	{
		// Token: 0x06006ABE RID: 27326 RVA: 0x001329F0 File Offset: 0x00130BF0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006ABF RID: 27327 RVA: 0x00132A30 File Offset: 0x00130C30
		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.color = this.color.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006AC0 RID: 27328 RVA: 0x00132A63 File Offset: 0x00130C63
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.color = Color.white;
		}

		// Token: 0x0400569A RID: 22170
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400569B RID: 22171
		[Tooltip("The color to set")]
		public SharedColor color;

		// Token: 0x0400569C RID: 22172
		private Light light;

		// Token: 0x0400569D RID: 22173
		private GameObject prevGameObject;
	}
}
