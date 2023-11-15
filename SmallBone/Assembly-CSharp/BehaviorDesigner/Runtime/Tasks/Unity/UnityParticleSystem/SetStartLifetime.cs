using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001594 RID: 5524
	[TaskDescription("Sets the start lifetime of the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class SetStartLifetime : Action
	{
		// Token: 0x06006A48 RID: 27208 RVA: 0x00131750 File Offset: 0x0012F950
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A49 RID: 27209 RVA: 0x00131790 File Offset: 0x0012F990
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.main.startLifetime = this.startLifetime.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A4A RID: 27210 RVA: 0x001317DB File Offset: 0x0012F9DB
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.startLifetime = 0f;
		}

		// Token: 0x0400560A RID: 22026
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400560B RID: 22027
		[Tooltip("The start lifetime of the ParticleSystem")]
		public SharedFloat startLifetime;

		// Token: 0x0400560C RID: 22028
		private ParticleSystem particleSystem;

		// Token: 0x0400560D RID: 22029
		private GameObject prevGameObject;
	}
}
