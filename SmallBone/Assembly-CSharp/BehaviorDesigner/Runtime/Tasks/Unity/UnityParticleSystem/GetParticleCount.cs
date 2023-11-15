using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001584 RID: 5508
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Stores the particle count of the Particle System.")]
	public class GetParticleCount : Action
	{
		// Token: 0x06006A08 RID: 27144 RVA: 0x00130E78 File Offset: 0x0012F078
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A09 RID: 27145 RVA: 0x00130EB8 File Offset: 0x0012F0B8
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.storeResult.Value = (float)this.particleSystem.particleCount;
			return TaskStatus.Success;
		}

		// Token: 0x06006A0A RID: 27146 RVA: 0x00130EEC File Offset: 0x0012F0EC
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = 0f;
		}

		// Token: 0x040055D0 RID: 21968
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055D1 RID: 21969
		[RequiredField]
		[Tooltip("The particle count of the ParticleSystem")]
		public SharedFloat storeResult;

		// Token: 0x040055D2 RID: 21970
		private ParticleSystem particleSystem;

		// Token: 0x040055D3 RID: 21971
		private GameObject prevGameObject;
	}
}
