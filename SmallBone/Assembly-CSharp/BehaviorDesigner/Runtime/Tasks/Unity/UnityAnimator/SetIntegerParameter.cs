using System;
using System.Collections;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x0200162D RID: 5677
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Sets the int parameter on an animator. Returns Success.")]
	public class SetIntegerParameter : Action
	{
		// Token: 0x06006C62 RID: 27746 RVA: 0x001360C8 File Offset: 0x001342C8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C63 RID: 27747 RVA: 0x00136108 File Offset: 0x00134308
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.hashID = Animator.StringToHash(this.paramaterName.Value);
			int integer = this.animator.GetInteger(this.hashID);
			this.animator.SetInteger(this.hashID, this.intValue.Value);
			if (this.setOnce)
			{
				base.StartCoroutine(this.ResetValue(integer));
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006C64 RID: 27748 RVA: 0x0013618A File Offset: 0x0013438A
		public IEnumerator ResetValue(int origVale)
		{
			yield return null;
			this.animator.SetInteger(this.hashID, origVale);
			yield break;
		}

		// Token: 0x06006C65 RID: 27749 RVA: 0x001361A0 File Offset: 0x001343A0
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.paramaterName = "";
			this.intValue = 0;
		}

		// Token: 0x04005823 RID: 22563
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005824 RID: 22564
		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

		// Token: 0x04005825 RID: 22565
		[Tooltip("The value of the int parameter")]
		public SharedInt intValue;

		// Token: 0x04005826 RID: 22566
		[Tooltip("Should the value be reverted back to its original value after it has been set?")]
		public bool setOnce;

		// Token: 0x04005827 RID: 22567
		private int hashID;

		// Token: 0x04005828 RID: 22568
		private Animator animator;

		// Token: 0x04005829 RID: 22569
		private GameObject prevGameObject;
	}
}
