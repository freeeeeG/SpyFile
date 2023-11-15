using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001586 RID: 5510
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Stores the time of the Particle System.")]
	public class GetTime : Action
	{
		// Token: 0x06006A10 RID: 27152 RVA: 0x00130FA8 File Offset: 0x0012F1A8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A11 RID: 27153 RVA: 0x00130FE8 File Offset: 0x0012F1E8
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.storeResult.Value = this.particleSystem.time;
			return TaskStatus.Success;
		}

		// Token: 0x06006A12 RID: 27154 RVA: 0x0013101B File Offset: 0x0012F21B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = 0f;
		}

		// Token: 0x040055D8 RID: 21976
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055D9 RID: 21977
		[RequiredField]
		[Tooltip("The time of the ParticleSystem")]
		public SharedFloat storeResult;

		// Token: 0x040055DA RID: 21978
		private ParticleSystem particleSystem;

		// Token: 0x040055DB RID: 21979
		private GameObject prevGameObject;
	}
}
