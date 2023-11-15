using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001598 RID: 5528
	[TaskDescription("Sets the time of the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class SetTime : Action
	{
		// Token: 0x06006A58 RID: 27224 RVA: 0x001319E0 File Offset: 0x0012FBE0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A59 RID: 27225 RVA: 0x00131A20 File Offset: 0x0012FC20
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.time = this.time.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A5A RID: 27226 RVA: 0x00131A53 File Offset: 0x0012FC53
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.time = 0f;
		}

		// Token: 0x0400561A RID: 22042
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400561B RID: 22043
		[Tooltip("The time of the ParticleSystem")]
		public SharedFloat time;

		// Token: 0x0400561C RID: 22044
		private ParticleSystem particleSystem;

		// Token: 0x0400561D RID: 22045
		private GameObject prevGameObject;
	}
}
