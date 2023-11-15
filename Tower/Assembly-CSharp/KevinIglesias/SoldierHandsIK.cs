using System;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000CF RID: 207
	public class SoldierHandsIK : MonoBehaviour
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x0000D288 File Offset: 0x0000B488
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
			this.weight = 0f;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000D2A1 File Offset: 0x0000B4A1
		private void Update()
		{
			this.weight = Mathf.Lerp(0f, 1f, 1f - Mathf.Cos(this.retargeter.localPosition.y * 3.1415927f * 0.5f));
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000D2E0 File Offset: 0x0000B4E0
		private void OnAnimatorIK(int layerIndex)
		{
			if (this.hand == SoldierIKGoal.LeftHand)
			{
				this.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, this.weight);
				this.animator.SetIKPosition(AvatarIKGoal.LeftHand, this.handEffector.position);
				this.animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, this.weight);
				this.animator.SetIKRotation(AvatarIKGoal.LeftHand, this.handEffector.rotation);
				return;
			}
			this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, this.weight);
			this.animator.SetIKPosition(AvatarIKGoal.RightHand, this.handEffector.position);
			this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, this.weight);
			this.animator.SetIKRotation(AvatarIKGoal.RightHand, this.handEffector.rotation);
		}

		// Token: 0x0400028C RID: 652
		public Transform retargeter;

		// Token: 0x0400028D RID: 653
		public Transform handEffector;

		// Token: 0x0400028E RID: 654
		public SoldierIKGoal hand;

		// Token: 0x0400028F RID: 655
		private Animator animator;

		// Token: 0x04000290 RID: 656
		private float weight;
	}
}
