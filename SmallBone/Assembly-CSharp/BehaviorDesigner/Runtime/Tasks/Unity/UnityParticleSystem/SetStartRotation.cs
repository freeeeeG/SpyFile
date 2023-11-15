using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001595 RID: 5525
	[TaskDescription("Sets the start rotation of the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class SetStartRotation : Action
	{
		// Token: 0x06006A4C RID: 27212 RVA: 0x001317F4 File Offset: 0x0012F9F4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A4D RID: 27213 RVA: 0x00131834 File Offset: 0x0012FA34
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.main.startRotation = this.startRotation.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A4E RID: 27214 RVA: 0x0013187F File Offset: 0x0012FA7F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.startRotation = 0f;
		}

		// Token: 0x0400560E RID: 22030
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400560F RID: 22031
		[Tooltip("The start rotation of the ParticleSystem")]
		public SharedFloat startRotation;

		// Token: 0x04005610 RID: 22032
		private ParticleSystem particleSystem;

		// Token: 0x04005611 RID: 22033
		private GameObject prevGameObject;
	}
}
