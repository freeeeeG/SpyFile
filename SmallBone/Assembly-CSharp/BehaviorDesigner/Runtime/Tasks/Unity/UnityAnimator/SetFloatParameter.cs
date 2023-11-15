using System;
using System.Collections;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x0200162B RID: 5675
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Sets the float parameter on an animator. Returns Success.")]
	public class SetFloatParameter : Action
	{
		// Token: 0x06006C57 RID: 27735 RVA: 0x00135F54 File Offset: 0x00134154
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C58 RID: 27736 RVA: 0x00135F94 File Offset: 0x00134194
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.hashID = Animator.StringToHash(this.paramaterName.Value);
			float @float = this.animator.GetFloat(this.hashID);
			this.animator.SetFloat(this.hashID, this.floatValue.Value);
			if (this.setOnce)
			{
				base.StartCoroutine(this.ResetValue(@float));
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006C59 RID: 27737 RVA: 0x00136016 File Offset: 0x00134216
		public IEnumerator ResetValue(float origVale)
		{
			yield return null;
			this.animator.SetFloat(this.hashID, origVale);
			yield break;
		}

		// Token: 0x06006C5A RID: 27738 RVA: 0x0013602C File Offset: 0x0013422C
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.paramaterName = "";
			this.floatValue = 0f;
		}

		// Token: 0x04005818 RID: 22552
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005819 RID: 22553
		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

		// Token: 0x0400581A RID: 22554
		[Tooltip("The value of the float parameter")]
		public SharedFloat floatValue;

		// Token: 0x0400581B RID: 22555
		[Tooltip("Should the value be reverted back to its original value after it has been set?")]
		public bool setOnce;

		// Token: 0x0400581C RID: 22556
		private int hashID;

		// Token: 0x0400581D RID: 22557
		private Animator animator;

		// Token: 0x0400581E RID: 22558
		private GameObject prevGameObject;
	}
}
