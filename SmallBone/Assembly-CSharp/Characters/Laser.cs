using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200072F RID: 1839
	public abstract class Laser : MonoBehaviour
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06002563 RID: 9571 RVA: 0x00070A0A File Offset: 0x0006EC0A
		public Vector2 direction
		{
			get
			{
				return this._direction;
			}
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x00070A12 File Offset: 0x0006EC12
		public virtual void Activate(Character owner, Vector2 direction)
		{
			this._owner = owner;
			this.Activate(direction);
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x00070A22 File Offset: 0x0006EC22
		public virtual void Activate(Character owner, float direction)
		{
			this._owner = owner;
			this.Activate(direction);
		}

		// Token: 0x06002566 RID: 9574
		public abstract void Activate(Vector2 direction);

		// Token: 0x06002567 RID: 9575
		public abstract void Activate(float direction);

		// Token: 0x06002568 RID: 9576
		public abstract void Deactivate();

		// Token: 0x04001FBF RID: 8127
		[SerializeField]
		protected Transform _originTransform;

		// Token: 0x04001FC0 RID: 8128
		[SerializeField]
		protected LayerMask _terrainLayer;

		// Token: 0x04001FC1 RID: 8129
		[SerializeField]
		protected float _maxLength;

		// Token: 0x04001FC2 RID: 8130
		protected Vector2 _direction;

		// Token: 0x04001FC3 RID: 8131
		protected Character _owner;
	}
}
