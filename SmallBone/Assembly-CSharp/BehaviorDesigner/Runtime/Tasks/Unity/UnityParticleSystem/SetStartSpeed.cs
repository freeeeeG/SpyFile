using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001597 RID: 5527
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Sets the start speed of the Particle System.")]
	public class SetStartSpeed : Action
	{
		// Token: 0x06006A54 RID: 27220 RVA: 0x0013193C File Offset: 0x0012FB3C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A55 RID: 27221 RVA: 0x0013197C File Offset: 0x0012FB7C
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.main.startSpeed = this.startSpeed.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A56 RID: 27222 RVA: 0x001319C7 File Offset: 0x0012FBC7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.startSpeed = 0f;
		}

		// Token: 0x04005616 RID: 22038
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005617 RID: 22039
		[Tooltip("The start speed of the ParticleSystem")]
		public SharedFloat startSpeed;

		// Token: 0x04005618 RID: 22040
		private ParticleSystem particleSystem;

		// Token: 0x04005619 RID: 22041
		private GameObject prevGameObject;
	}
}
