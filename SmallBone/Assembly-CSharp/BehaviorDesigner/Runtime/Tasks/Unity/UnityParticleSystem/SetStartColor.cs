using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem
{
	// Token: 0x02001592 RID: 5522
	[TaskCategory("Unity/ParticleSystem")]
	[TaskDescription("Sets the start color of the Particle System.")]
	public class SetStartColor : Action
	{
		// Token: 0x06006A40 RID: 27200 RVA: 0x00131608 File Offset: 0x0012F808
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006A41 RID: 27201 RVA: 0x00131648 File Offset: 0x0012F848
		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.particleSystem.main.startColor = this.startColor.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A42 RID: 27202 RVA: 0x00131693 File Offset: 0x0012F893
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.startColor = Color.white;
		}

		// Token: 0x04005602 RID: 22018
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005603 RID: 22019
		[Tooltip("The start color of the ParticleSystem")]
		public SharedColor startColor;

		// Token: 0x04005604 RID: 22020
		private ParticleSystem particleSystem;

		// Token: 0x04005605 RID: 22021
		private GameObject prevGameObject;
	}
}
