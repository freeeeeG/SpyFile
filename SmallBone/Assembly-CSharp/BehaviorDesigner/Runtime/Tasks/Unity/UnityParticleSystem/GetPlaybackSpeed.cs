using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001585 RID: 5509
	[TaskDescription("Stores the playback speed of the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class GetPlaybackSpeed : Action
	{
		// Token: 0x06006A0C RID: 27148 RVA: 0x00130F08 File Offset: 0x0012F108
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A0D RID: 27149 RVA: 0x00130F48 File Offset: 0x0012F148
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			ParticleSystem.MainModule main = this.particleSystem.main;
			this.storeResult.Value = main.simulationSpeed;
			return TaskStatus.Success;
		}

		// Token: 0x06006A0E RID: 27150 RVA: 0x00130F8E File Offset: 0x0012F18E
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = 0f;
		}

		// Token: 0x040055D4 RID: 21972
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055D5 RID: 21973
		[RequiredField]
		[Tooltip("The playback speed of the ParticleSystem")]
		public SharedFloat storeResult;

		// Token: 0x040055D6 RID: 21974
		private ParticleSystem particleSystem;

		// Token: 0x040055D7 RID: 21975
		private GameObject prevGameObject;
	}
}
