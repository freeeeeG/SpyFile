using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000090 RID: 144
	public class MoveComponent2D : MonoBehaviour
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00019665 File Offset: 0x00017865
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x0001966D File Offset: 0x0001786D
		public Vector2 vector
		{
			get
			{
				return this._vector;
			}
			set
			{
				this.vectorLastFrame = this._vector;
				this._vector = value;
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00019682 File Offset: 0x00017882
		private void Start()
		{
			MoveSystem2D.Register(this);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001968A File Offset: 0x0001788A
		private void OnDisable()
		{
			this.vector = Vector2.zero;
			this.Rb.velocity = Vector2.zero;
			this.Rb.angularVelocity = 0f;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000196B7 File Offset: 0x000178B7
		private void OnDestroy()
		{
			MoveSystem2D.UnRegister(this);
		}

		// Token: 0x04000331 RID: 817
		private Vector2 _vector;

		// Token: 0x04000332 RID: 818
		public Vector2 vectorLastFrame;

		// Token: 0x04000333 RID: 819
		public float drag;

		// Token: 0x04000334 RID: 820
		public Rigidbody2D Rb;

		// Token: 0x04000335 RID: 821
		public bool knockbackImmune;

		// Token: 0x04000336 RID: 822
		public bool rotateTowardsMove;
	}
}
