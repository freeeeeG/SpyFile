using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x0200158A RID: 5514
	[TaskDescription("Is the Particle System stopped?")]
	[TaskCategory("Unity/ParticleSystem")]
	public class IsStopped : Conditional
	{
		// Token: 0x06006A20 RID: 27168 RVA: 0x0013119C File Offset: 0x0012F39C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A21 RID: 27169 RVA: 0x001311DC File Offset: 0x0012F3DC
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			if (!this.particleSystem.isStopped)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A22 RID: 27170 RVA: 0x00131208 File Offset: 0x0012F408
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040055E5 RID: 21989
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055E6 RID: 21990
		private ParticleSystem particleSystem;

		// Token: 0x040055E7 RID: 21991
		private GameObject prevGameObject;
	}
}
