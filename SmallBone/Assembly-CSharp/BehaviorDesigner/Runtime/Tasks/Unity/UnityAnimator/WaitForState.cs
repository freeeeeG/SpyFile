using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001638 RID: 5688
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Waits for the Animator to reach the specified state.")]
	public class WaitForState : Action
	{
		// Token: 0x06006C93 RID: 27795 RVA: 0x00136735 File Offset: 0x00134935
		public override void OnAwake()
		{
			this.stateHash = Animator.StringToHash(this.stateName.Value);
		}

		// Token: 0x06006C94 RID: 27796 RVA: 0x00136750 File Offset: 0x00134950
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
				if (!this.animator.HasState(this.layer.Value, this.stateHash))
				{
					Debug.LogError("Error: The Animator does not have the state " + this.stateName.Value + " on layer " + this.layer.Value.ToString());
				}
			}
		}

		// Token: 0x06006C95 RID: 27797 RVA: 0x001367E0 File Offset: 0x001349E0
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			if (this.animator.GetCurrentAnimatorStateInfo(this.layer.Value).shortNameHash == this.stateHash)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		// Token: 0x06006C96 RID: 27798 RVA: 0x00136830 File Offset: 0x00134A30
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.stateName = "";
			this.layer = -1;
		}

		// Token: 0x04005852 RID: 22610
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005853 RID: 22611
		[Tooltip("The name of the state")]
		public SharedString stateName;

		// Token: 0x04005854 RID: 22612
		[Tooltip("The layer where the state is")]
		public SharedInt layer = -1;

		// Token: 0x04005855 RID: 22613
		private Animator animator;

		// Token: 0x04005856 RID: 22614
		private GameObject prevGameObject;

		// Token: 0x04005857 RID: 22615
		private int stateHash;
	}
}
