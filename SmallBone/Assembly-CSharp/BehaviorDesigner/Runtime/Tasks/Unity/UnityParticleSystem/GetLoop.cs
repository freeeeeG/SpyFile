using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001582 RID: 5506
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Stores if the Particle System should loop.")]
	public class GetLoop : Action
	{
		// Token: 0x06006A00 RID: 27136 RVA: 0x00130D3C File Offset: 0x0012EF3C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A01 RID: 27137 RVA: 0x00130D7C File Offset: 0x0012EF7C
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.storeResult.Value = this.particleSystem.main.loop;
			return TaskStatus.Success;
		}

		// Token: 0x06006A02 RID: 27138 RVA: 0x00130DC2 File Offset: 0x0012EFC2
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = false;
		}

		// Token: 0x040055C8 RID: 21960
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055C9 RID: 21961
		[RequiredField]
		[Tooltip("Should the ParticleSystem loop?")]
		public SharedBool storeResult;

		// Token: 0x040055CA RID: 21962
		private ParticleSystem particleSystem;

		// Token: 0x040055CB RID: 21963
		private GameObject prevGameObject;
	}
}
