using System;
using System.Collections;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001629 RID: 5673
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Sets the bool parameter on an animator. Returns Success.")]
	public class SetBoolParameter : Action
	{
		// Token: 0x06006C4C RID: 27724 RVA: 0x00135DE4 File Offset: 0x00133FE4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C4D RID: 27725 RVA: 0x00135E24 File Offset: 0x00134024
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.hashID = Animator.StringToHash(this.paramaterName.Value);
			bool @bool = this.animator.GetBool(this.hashID);
			this.animator.SetBool(this.hashID, this.boolValue.Value);
			if (this.setOnce)
			{
				base.StartCoroutine(this.ResetValue(@bool));
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006C4E RID: 27726 RVA: 0x00135EA6 File Offset: 0x001340A6
		public IEnumerator ResetValue(bool origVale)
		{
			yield return null;
			this.animator.SetBool(this.hashID, origVale);
			yield break;
		}

		// Token: 0x06006C4F RID: 27727 RVA: 0x00135EBC File Offset: 0x001340BC
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.paramaterName = "";
			this.boolValue = false;
		}

		// Token: 0x0400580D RID: 22541
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400580E RID: 22542
		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

		// Token: 0x0400580F RID: 22543
		[Tooltip("The value of the bool parameter")]
		public SharedBool boolValue;

		// Token: 0x04005810 RID: 22544
		[Tooltip("Should the value be reverted back to its original value after it has been set?")]
		public bool setOnce;

		// Token: 0x04005811 RID: 22545
		private int hashID;

		// Token: 0x04005812 RID: 22546
		private Animator animator;

		// Token: 0x04005813 RID: 22547
		private GameObject prevGameObject;
	}
}
