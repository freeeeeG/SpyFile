using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001587 RID: 5511
	[TaskDescription("Is the Particle System alive?")]
	[TaskCategory("Unity/ParticleSystem")]
	public class IsAlive : Conditional
	{
		// Token: 0x06006A14 RID: 27156 RVA: 0x00131034 File Offset: 0x0012F234
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A15 RID: 27157 RVA: 0x00131074 File Offset: 0x0012F274
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			if (!this.particleSystem.IsAlive())
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A16 RID: 27158 RVA: 0x001310A0 File Offset: 0x0012F2A0
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040055DC RID: 21980
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055DD RID: 21981
		private ParticleSystem particleSystem;

		// Token: 0x040055DE RID: 21982
		private GameObject prevGameObject;
	}
}
