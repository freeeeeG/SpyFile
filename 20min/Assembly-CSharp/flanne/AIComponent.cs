using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200008F RID: 143
	public class AIComponent : MonoBehaviour
	{
		// Token: 0x06000538 RID: 1336 RVA: 0x0001960A File Offset: 0x0001780A
		private void Awake()
		{
			this.maxMoveSpeed = this.baseMaxMoveSpeed;
			this.acceleration = this.baseAcceleration;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00019624 File Offset: 0x00017824
		private void Start()
		{
			AIController.SharedInstance.Register(this);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00019631 File Offset: 0x00017831
		private void OnEnable()
		{
			this.specialTimer = 0f;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001963E File Offset: 0x0001783E
		private void OnDestroy()
		{
			AIController.SharedInstance.UnRegister(this);
		}

		// Token: 0x04000320 RID: 800
		public MoveComponent2D moveComponent;

		// Token: 0x04000321 RID: 801
		public float baseMaxMoveSpeed;

		// Token: 0x04000322 RID: 802
		public float baseAcceleration;

		// Token: 0x04000323 RID: 803
		public bool ignoreFlock;

		// Token: 0x04000324 RID: 804
		public bool rotateTowardsPlayer;

		// Token: 0x04000325 RID: 805
		public bool dontLookAtPlayer;

		// Token: 0x04000326 RID: 806
		public Animator animator;

		// Token: 0x04000327 RID: 807
		public AISpecial specialSO;

		// Token: 0x04000328 RID: 808
		public float specialRangeSqr = -1f;

		// Token: 0x04000329 RID: 809
		public float specialCooldown;

		// Token: 0x0400032A RID: 810
		[NonSerialized]
		public float specialTimer;

		// Token: 0x0400032B RID: 811
		public Transform specialPoint;

		// Token: 0x0400032C RID: 812
		public bool dontFaceDuringSpecial;

		// Token: 0x0400032D RID: 813
		[NonSerialized]
		public bool frozen;

		// Token: 0x0400032E RID: 814
		[NonSerialized]
		public int damageToPlayer = 1;

		// Token: 0x0400032F RID: 815
		[NonSerialized]
		public float maxMoveSpeed;

		// Token: 0x04000330 RID: 816
		[NonSerialized]
		public float acceleration;
	}
}
