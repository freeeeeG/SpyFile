using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRenderer
{
	// Token: 0x02001565 RID: 5477
	[TaskCategory("Unity/Renderer")]
	[TaskDescription("Sets the material on the Renderer.")]
	public class SetMaterial : Action
	{
		// Token: 0x060069A6 RID: 27046 RVA: 0x001300C8 File Offset: 0x0012E2C8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.renderer = defaultGameObject.GetComponent<Renderer>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x00130108 File Offset: 0x0012E308
		public override TaskStatus OnUpdate()
		{
			if (this.renderer == null)
			{
				Debug.LogWarning("Renderer is null");
				return TaskStatus.Failure;
			}
			this.renderer.material = this.material.Value;
			return TaskStatus.Success;
		}

		// Token: 0x060069A8 RID: 27048 RVA: 0x0013013B File Offset: 0x0012E33B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.material = null;
		}

		// Token: 0x0400556C RID: 21868
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400556D RID: 21869
		[Tooltip("The material to set")]
		public SharedMaterial material;

		// Token: 0x0400556E RID: 21870
		private Renderer renderer;

		// Token: 0x0400556F RID: 21871
		private GameObject prevGameObject;
	}
}
