using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001599 RID: 5529
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Simulate the Particle System.")]
	public class Simulate : Action
	{
		// Token: 0x06006A5C RID: 27228 RVA: 0x00131A6C File Offset: 0x0012FC6C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A5D RID: 27229 RVA: 0x00131AAC File Offset: 0x0012FCAC
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.Simulate(this.time.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006A5E RID: 27230 RVA: 0x00131ADF File Offset: 0x0012FCDF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.time = 0f;
		}

		// Token: 0x0400561E RID: 22046
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400561F RID: 22047
		[Tooltip("Time to fastfoward the Particle System to")]
		public SharedFloat time;

		// Token: 0x04005620 RID: 22048
		private ParticleSystem particleSystem;

		// Token: 0x04005621 RID: 22049
		private GameObject prevGameObject;
	}
}
