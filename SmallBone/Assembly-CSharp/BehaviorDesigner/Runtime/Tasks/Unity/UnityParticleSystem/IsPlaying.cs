using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001589 RID: 5513
	[TaskDescription("Is the Particle System playing?")]
	[TaskCategory("Unity/ParticleSystem")]
	public class IsPlaying : Conditional
	{
		// Token: 0x06006A1C RID: 27164 RVA: 0x00131124 File Offset: 0x0012F324
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A1D RID: 27165 RVA: 0x00131164 File Offset: 0x0012F364
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			if (!this.particleSystem.isPlaying)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A1E RID: 27166 RVA: 0x00131190 File Offset: 0x0012F390
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040055E2 RID: 21986
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055E3 RID: 21987
		private ParticleSystem particleSystem;

		// Token: 0x040055E4 RID: 21988
		private GameObject prevGameObject;
	}
}
