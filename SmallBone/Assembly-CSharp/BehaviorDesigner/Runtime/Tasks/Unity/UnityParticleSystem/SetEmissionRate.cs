using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x0200158D RID: 5517
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Sets the emission rate of the Particle System.")]
	public class SetEmissionRate : Action
	{
		// Token: 0x06006A2C RID: 27180 RVA: 0x001312FC File Offset: 0x0012F4FC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A2D RID: 27181 RVA: 0x0013133C File Offset: 0x0012F53C
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			Debug.Log("Warning: SetEmissionRate is not used in Unity 5.3 or later.");
			return TaskStatus.Success;
		}

		// Token: 0x06006A2E RID: 27182 RVA: 0x00131363 File Offset: 0x0012F563
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.emissionRate = 0f;
		}

		// Token: 0x040055EE RID: 21998
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055EF RID: 21999
		[Tooltip("The emission rate of the ParticleSystem")]
		public SharedFloat emissionRate;

		// Token: 0x040055F0 RID: 22000
		private ParticleSystem particleSystem;

		// Token: 0x040055F1 RID: 22001
		private GameObject prevGameObject;
	}
}
