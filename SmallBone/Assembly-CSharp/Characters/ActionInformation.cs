using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200069E RID: 1694
	[Serializable]
	public class ActionInformation
	{
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060021D3 RID: 8659 RVA: 0x000659EE File Offset: 0x00063BEE
		public AnimationClip characterClip
		{
			get
			{
				return this._characterClip;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060021D4 RID: 8660 RVA: 0x000659F6 File Offset: 0x00063BF6
		public AnimationClip weaponClip
		{
			get
			{
				return this._weaponClip;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060021D5 RID: 8661 RVA: 0x000659FE File Offset: 0x00063BFE
		public bool force
		{
			get
			{
				return this._force;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060021D6 RID: 8662 RVA: 0x00065A06 File Offset: 0x00063C06
		public bool blockMovement
		{
			get
			{
				return this._blockMovement;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060021D7 RID: 8663 RVA: 0x00065A0E File Offset: 0x00063C0E
		public bool blockLook
		{
			get
			{
				return this._blockLook;
			}
		}

		// Token: 0x04001CCE RID: 7374
		[SerializeField]
		private AnimationClip _characterClip;

		// Token: 0x04001CCF RID: 7375
		[SerializeField]
		private AnimationClip _weaponClip;

		// Token: 0x04001CD0 RID: 7376
		[SerializeField]
		private bool _force;

		// Token: 0x04001CD1 RID: 7377
		[SerializeField]
		private bool _blockMovement;

		// Token: 0x04001CD2 RID: 7378
		[SerializeField]
		private bool _blockLook;
	}
}
