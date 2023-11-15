using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001590 RID: 5520
	[TaskDescription("Sets the max particles of the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class SetMaxParticles : Action
	{
		// Token: 0x06006A38 RID: 27192 RVA: 0x001314B4 File Offset: 0x0012F6B4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A39 RID: 27193 RVA: 0x001314F4 File Offset: 0x0012F6F4
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.main.maxParticles = this.maxParticles.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A3A RID: 27194 RVA: 0x0013153A File Offset: 0x0012F73A
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.maxParticles = 0;
		}

		// Token: 0x040055FA RID: 22010
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055FB RID: 22011
		[Tooltip("The max particles of the ParticleSystem")]
		public SharedInt maxParticles;

		// Token: 0x040055FC RID: 22012
		private ParticleSystem particleSystem;

		// Token: 0x040055FD RID: 22013
		private GameObject prevGameObject;
	}
}
