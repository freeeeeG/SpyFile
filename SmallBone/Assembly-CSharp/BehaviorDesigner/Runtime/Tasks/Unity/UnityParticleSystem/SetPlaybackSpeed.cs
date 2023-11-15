using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001591 RID: 5521
	[TaskDescription("Sets the playback speed of the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class SetPlaybackSpeed : Action
	{
		// Token: 0x06006A3C RID: 27196 RVA: 0x00131550 File Offset: 0x0012F750
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A3D RID: 27197 RVA: 0x00131590 File Offset: 0x0012F790
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.main.simulationSpeed = this.playbackSpeed.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A3E RID: 27198 RVA: 0x001315D6 File Offset: 0x0012F7D6
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.playbackSpeed = 1f;
		}

		// Token: 0x040055FE RID: 22014
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055FF RID: 22015
		[Tooltip("The playback speed of the ParticleSystem")]
		public SharedFloat playbackSpeed = 1f;

		// Token: 0x04005600 RID: 22016
		private ParticleSystem particleSystem;

		// Token: 0x04005601 RID: 22017
		private GameObject prevGameObject;
	}
}
