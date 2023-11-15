using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001588 RID: 5512
	[TaskDescription("Is the Particle System paused?")]
	[TaskCategory("Unity/ParticleSystem")]
	public class IsPaused : Conditional
	{
		// Token: 0x06006A18 RID: 27160 RVA: 0x001310AC File Offset: 0x0012F2AC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A19 RID: 27161 RVA: 0x001310EC File Offset: 0x0012F2EC
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			if (!this.particleSystem.isPaused)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A1A RID: 27162 RVA: 0x00131118 File Offset: 0x0012F318
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040055DF RID: 21983
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055E0 RID: 21984
		private ParticleSystem particleSystem;

		// Token: 0x040055E1 RID: 21985
		private GameObject prevGameObject;
	}
}
