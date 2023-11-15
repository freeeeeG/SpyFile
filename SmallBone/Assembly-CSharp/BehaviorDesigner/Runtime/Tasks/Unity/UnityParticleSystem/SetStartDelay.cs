using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001593 RID: 5523
	[TaskDescription("Sets the start delay of the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class SetStartDelay : Action
	{
		// Token: 0x06006A44 RID: 27204 RVA: 0x001316AC File Offset: 0x0012F8AC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A45 RID: 27205 RVA: 0x001316EC File Offset: 0x0012F8EC
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.main.startDelay = this.startDelay.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A46 RID: 27206 RVA: 0x00131737 File Offset: 0x0012F937
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.startDelay = 0f;
		}

		// Token: 0x04005606 RID: 22022
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005607 RID: 22023
		[Tooltip("The start delay of the ParticleSystem")]
		public SharedFloat startDelay;

		// Token: 0x04005608 RID: 22024
		private ParticleSystem particleSystem;

		// Token: 0x04005609 RID: 22025
		private GameObject prevGameObject;
	}
}
