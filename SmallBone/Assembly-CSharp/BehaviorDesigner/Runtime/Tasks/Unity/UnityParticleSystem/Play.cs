using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x0200158C RID: 5516
	[TaskDescription("Play the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class Play : Action
	{
		// Token: 0x06006A28 RID: 27176 RVA: 0x00131288 File Offset: 0x0012F488
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A29 RID: 27177 RVA: 0x001312C8 File Offset: 0x0012F4C8
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.Play();
			return TaskStatus.Success;
		}

		// Token: 0x06006A2A RID: 27178 RVA: 0x001312F0 File Offset: 0x0012F4F0
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040055EB RID: 21995
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055EC RID: 21996
		private ParticleSystem particleSystem;

		// Token: 0x040055ED RID: 21997
		private GameObject prevGameObject;
	}
}
