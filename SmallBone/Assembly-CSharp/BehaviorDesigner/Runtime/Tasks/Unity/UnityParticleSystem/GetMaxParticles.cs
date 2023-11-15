using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001583 RID: 5507
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Stores the max particles of the Particle System.")]
	public class GetMaxParticles : Action
	{
		// Token: 0x06006A04 RID: 27140 RVA: 0x00130DD8 File Offset: 0x0012EFD8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A05 RID: 27141 RVA: 0x00130E18 File Offset: 0x0012F018
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.storeResult.Value = (float)this.particleSystem.main.maxParticles;
			return TaskStatus.Success;
		}

		// Token: 0x06006A06 RID: 27142 RVA: 0x00130E5F File Offset: 0x0012F05F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = 0f;
		}

		// Token: 0x040055CC RID: 21964
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055CD RID: 21965
		[RequiredField]
		[Tooltip("The max particles of the ParticleSystem")]
		public SharedFloat storeResult;

		// Token: 0x040055CE RID: 21966
		private ParticleSystem particleSystem;

		// Token: 0x040055CF RID: 21967
		private GameObject prevGameObject;
	}
}
