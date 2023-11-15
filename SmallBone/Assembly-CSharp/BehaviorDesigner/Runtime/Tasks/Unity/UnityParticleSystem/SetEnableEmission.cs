using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x0200158E RID: 5518
	[TaskDescription("Enables or disables the Particle System emission.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class SetEnableEmission : Action
	{
		// Token: 0x06006A30 RID: 27184 RVA: 0x0013137C File Offset: 0x0012F57C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x001313BC File Offset: 0x0012F5BC
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.emission.enabled = this.enable.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A32 RID: 27186 RVA: 0x00131402 File Offset: 0x0012F602
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.enable = false;
		}

		// Token: 0x040055F2 RID: 22002
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055F3 RID: 22003
		[Tooltip("Enable the ParticleSystem emissions?")]
		public SharedBool enable;

		// Token: 0x040055F4 RID: 22004
		private ParticleSystem particleSystem;

		// Token: 0x040055F5 RID: 22005
		private GameObject prevGameObject;
	}
}
