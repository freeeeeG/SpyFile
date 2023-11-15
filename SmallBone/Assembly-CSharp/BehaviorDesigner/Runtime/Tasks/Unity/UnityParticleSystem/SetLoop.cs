using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x0200158F RID: 5519
	[TaskDescription("Sets if the Particle System should loop.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class SetLoop : Action
	{
		// Token: 0x06006A34 RID: 27188 RVA: 0x00131418 File Offset: 0x0012F618
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A35 RID: 27189 RVA: 0x00131458 File Offset: 0x0012F658
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.main.loop = this.loop.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A36 RID: 27190 RVA: 0x0013149E File Offset: 0x0012F69E
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.loop = false;
		}

		// Token: 0x040055F6 RID: 22006
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055F7 RID: 22007
		[Tooltip("Should the ParticleSystem loop?")]
		public SharedBool loop;

		// Token: 0x040055F8 RID: 22008
		private ParticleSystem particleSystem;

		// Token: 0x040055F9 RID: 22009
		private GameObject prevGameObject;
	}
}
