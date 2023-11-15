using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001630 RID: 5680
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Sets the look at position. Returns Success.")]
	public class SetLookAtPosition : Action
	{
		// Token: 0x06006C71 RID: 27761 RVA: 0x001362DB File Offset: 0x001344DB
		public override void OnStart()
		{
			this.animator = base.GetComponent<Animator>();
			this.positionSet = false;
		}

		// Token: 0x06006C72 RID: 27762 RVA: 0x001362F0 File Offset: 0x001344F0
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			if (!this.positionSet)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006C73 RID: 27763 RVA: 0x00136317 File Offset: 0x00134517
		public override void OnAnimatorIK()
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.SetLookAtPosition(this.position.Value);
			this.positionSet = true;
		}

		// Token: 0x06006C74 RID: 27764 RVA: 0x00136345 File Offset: 0x00134545
		public override void OnReset()
		{
			this.position = Vector3.zero;
		}

		// Token: 0x04005833 RID: 22579
		[Tooltip("The position to lookAt")]
		public SharedVector3 position;

		// Token: 0x04005834 RID: 22580
		private Animator animator;

		// Token: 0x04005835 RID: 22581
		private bool positionSet;
	}
}
