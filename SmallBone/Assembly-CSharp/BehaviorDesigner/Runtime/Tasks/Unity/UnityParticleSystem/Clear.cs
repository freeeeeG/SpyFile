using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x0200157E RID: 5502
	[TaskDescription("Clear the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class Clear : Action
	{
		// Token: 0x060069F0 RID: 27120 RVA: 0x00130B0C File Offset: 0x0012ED0C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060069F1 RID: 27121 RVA: 0x00130B4C File Offset: 0x0012ED4C
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.Clear();
			return TaskStatus.Success;
		}

		// Token: 0x060069F2 RID: 27122 RVA: 0x00130B74 File Offset: 0x0012ED74
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040055B9 RID: 21945
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055BA RID: 21946
		private ParticleSystem particleSystem;

		// Token: 0x040055BB RID: 21947
		private GameObject prevGameObject;
	}
}
