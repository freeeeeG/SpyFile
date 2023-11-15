using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001596 RID: 5526
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Sets the start size of the Particle System.")]
	public class SetStartSize : Action
	{
		// Token: 0x06006A50 RID: 27216 RVA: 0x00131898 File Offset: 0x0012FA98
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A51 RID: 27217 RVA: 0x001318D8 File Offset: 0x0012FAD8
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.main.startSize = this.startSize.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A52 RID: 27218 RVA: 0x00131923 File Offset: 0x0012FB23
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.startSize = 0f;
		}

		// Token: 0x04005612 RID: 22034
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005613 RID: 22035
		[Tooltip("The start size of the ParticleSystem")]
		public SharedFloat startSize;

		// Token: 0x04005614 RID: 22036
		private ParticleSystem particleSystem;

		// Token: 0x04005615 RID: 22037
		private GameObject prevGameObject;
	}
}
