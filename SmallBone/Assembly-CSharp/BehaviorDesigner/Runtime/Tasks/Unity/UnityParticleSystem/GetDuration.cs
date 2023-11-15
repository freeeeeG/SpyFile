using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x0200157F RID: 5503
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Stores the duration of the Particle System.")]
	public class GetDuration : Action
	{
		// Token: 0x060069F4 RID: 27124 RVA: 0x00130B80 File Offset: 0x0012ED80
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060069F5 RID: 27125 RVA: 0x00130BC0 File Offset: 0x0012EDC0
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.storeResult.Value = this.particleSystem.main.duration;
			return TaskStatus.Success;
		}

		// Token: 0x060069F6 RID: 27126 RVA: 0x00130C06 File Offset: 0x0012EE06
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = 0f;
		}

		// Token: 0x040055BC RID: 21948
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055BD RID: 21949
		[RequiredField]
		[Tooltip("The duration of the ParticleSystem")]
		public SharedFloat storeResult;

		// Token: 0x040055BE RID: 21950
		private ParticleSystem particleSystem;

		// Token: 0x040055BF RID: 21951
		private GameObject prevGameObject;
	}
}
