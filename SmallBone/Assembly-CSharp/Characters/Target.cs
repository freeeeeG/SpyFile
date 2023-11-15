using System;
using Level;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200072D RID: 1837
	public class Target : MonoBehaviour, ITarget
	{
		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002556 RID: 9558 RVA: 0x00070968 File Offset: 0x0006EB68
		public Collider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06002557 RID: 9559 RVA: 0x00070970 File Offset: 0x0006EB70
		// (set) Token: 0x06002558 RID: 9560 RVA: 0x00070978 File Offset: 0x0006EB78
		public Character character
		{
			get
			{
				return this._character;
			}
			set
			{
				this._character = value;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06002559 RID: 9561 RVA: 0x00070981 File Offset: 0x0006EB81
		// (set) Token: 0x0600255A RID: 9562 RVA: 0x00070989 File Offset: 0x0006EB89
		public DestructibleObject damageable
		{
			get
			{
				return this._damageable;
			}
			set
			{
				this._damageable = value;
			}
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x00070992 File Offset: 0x0006EB92
		Transform ITarget.get_transform()
		{
			return base.transform;
		}

		// Token: 0x04001FB8 RID: 8120
		[GetComponent]
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04001FB9 RID: 8121
		[SerializeField]
		private Character _character;

		// Token: 0x04001FBA RID: 8122
		[SerializeField]
		private DestructibleObject _damageable;
	}
}
