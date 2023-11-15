using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x0200159A RID: 5530
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Stop the Particle System.")]
	public class Stop : Action
	{
		// Token: 0x06006A60 RID: 27232 RVA: 0x00131AF8 File Offset: 0x0012FCF8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A61 RID: 27233 RVA: 0x00131B38 File Offset: 0x0012FD38
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.Stop();
			return TaskStatus.Success;
		}

		// Token: 0x06006A62 RID: 27234 RVA: 0x00131B60 File Offset: 0x0012FD60
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x04005622 RID: 22050
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005623 RID: 22051
		private ParticleSystem particleSystem;

		// Token: 0x04005624 RID: 22052
		private GameObject prevGameObject;
	}
}
