using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x0200158B RID: 5515
	[TaskDescription("Pause the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class Pause : Action
	{
		// Token: 0x06006A24 RID: 27172 RVA: 0x00131214 File Offset: 0x0012F414
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A25 RID: 27173 RVA: 0x00131254 File Offset: 0x0012F454
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.Pause();
			return TaskStatus.Success;
		}

		// Token: 0x06006A26 RID: 27174 RVA: 0x0013127C File Offset: 0x0012F47C
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040055E8 RID: 21992
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055E9 RID: 21993
		private ParticleSystem particleSystem;

		// Token: 0x040055EA RID: 21994
		private GameObject prevGameObject;
	}
}
