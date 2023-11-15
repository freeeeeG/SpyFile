using System;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000D7 RID: 215
	public class VillagerHandsIK : MonoBehaviour
	{
		// Token: 0x0600031A RID: 794 RVA: 0x0000DF18 File Offset: 0x0000C118
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
			this.weight = 0f;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000DF31 File Offset: 0x0000C131
		private void Update()
		{
			this.weight = Mathf.Lerp(0f, 1f, 1f - Mathf.Cos(this.retargeter.localPosition.y * 3.1415927f * 0.5f));
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000DF70 File Offset: 0x0000C170
		private void OnAnimatorIK(int layerIndex)
		{
			if (this.hand == VillagerIKGoal.LeftHand)
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

		// Token: 0x040002EB RID: 747
		public Transform retargeter;

		// Token: 0x040002EC RID: 748
		public Transform handEffector;

		// Token: 0x040002ED RID: 749
		public VillagerIKGoal hand;

		// Token: 0x040002EE RID: 750
		private Animator animator;

		// Token: 0x040002EF RID: 751
		private float weight;
	}
}
