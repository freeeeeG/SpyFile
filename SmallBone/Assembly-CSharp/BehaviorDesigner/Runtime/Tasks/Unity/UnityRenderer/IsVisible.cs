using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRenderer
{
	// Token: 0x02001564 RID: 5476
	[TaskCategory("Unity/Renderer")]
	[TaskDescription("Returns Success if the Renderer is visible, otherwise Failure.")]
	public class IsVisible : Conditional
	{
		// Token: 0x060069A2 RID: 27042 RVA: 0x00130050 File Offset: 0x0012E250
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.renderer = defaultGameObject.GetComponent<Renderer>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060069A3 RID: 27043 RVA: 0x00130090 File Offset: 0x0012E290
		public override TaskStatus OnUpdate()
		{
			if (this.renderer == null)
			{
				Debug.LogWarning("Renderer is null");
				return TaskStatus.Failure;
			}
			if (!this.renderer.isVisible)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060069A4 RID: 27044 RVA: 0x001300BC File Offset: 0x0012E2BC
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x04005569 RID: 21865
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400556A RID: 21866
		private Renderer renderer;

		// Token: 0x0400556B RID: 21867
		private GameObject prevGameObject;
	}
}
