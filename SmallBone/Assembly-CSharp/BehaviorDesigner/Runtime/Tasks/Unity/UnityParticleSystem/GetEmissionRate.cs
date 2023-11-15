using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001580 RID: 5504
	[TaskDescription("Stores the emission rate of the Particle System.")]
	[TaskCategory("Unity/ParticleSystem")]
	public class GetEmissionRate : Action
	{
		// Token: 0x060069F8 RID: 27128 RVA: 0x00130C20 File Offset: 0x0012EE20
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x060069F9 RID: 27129 RVA: 0x00130C60 File Offset: 0x0012EE60
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			Debug.Log("Warning: GetEmissionRate is not used in Unity 5.3 or later.");
			return TaskStatus.Success;
		}

		// Token: 0x060069FA RID: 27130 RVA: 0x00130C87 File Offset: 0x0012EE87
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = 0f;
		}

		// Token: 0x040055C0 RID: 21952
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040055C1 RID: 21953
		[Tooltip("The emission rate of the ParticleSystem")]
		[RequiredField]
		public SharedFloat storeResult;

		// Token: 0x040055C2 RID: 21954
		private ParticleSystem particleSystem;

		// Token: 0x040055C3 RID: 21955
		private GameObject prevGameObject;
	}
}
