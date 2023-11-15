using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001581 RID: 5505
	[TaskDescription("Stores if the Particle System is emitting particles.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class GetEnableEmission : Action
	{
		// Token: 0x060069FC RID: 27132 RVA: 0x00130CA0 File Offset: 0x0012EEA0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060069FD RID: 27133 RVA: 0x00130CE0 File Offset: 0x0012EEE0
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.storeResult.Value = this.particleSystem.emission.enabled;
			return TaskStatus.Success;
		}

		// Token: 0x060069FE RID: 27134 RVA: 0x00130D26 File Offset: 0x0012EF26
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = false;
		}

		// Token: 0x040055C4 RID: 21956
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055C5 RID: 21957
		[RequiredField]
		[Tooltip("Is the Particle System emitting particles?")]
		public SharedBool storeResult;

		// Token: 0x040055C6 RID: 21958
		private ParticleSystem particleSystem;

		// Token: 0x040055C7 RID: 21959
		private GameObject prevGameObject;
	}
}
