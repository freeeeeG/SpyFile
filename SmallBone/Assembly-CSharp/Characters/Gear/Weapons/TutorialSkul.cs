using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Gear.Weapons
{
	// Token: 0x02000836 RID: 2102
	public class TutorialSkul : MonoBehaviour
	{
		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002BA2 RID: 11170 RVA: 0x00086529 File Offset: 0x00084729
		public Characters.Actions.Action idle
		{
			get
			{
				return this._idle;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06002BA3 RID: 11171 RVA: 0x00086531 File Offset: 0x00084731
		public Characters.Actions.Action openEyes
		{
			get
			{
				return this._openEyes;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06002BA4 RID: 11172 RVA: 0x00086539 File Offset: 0x00084739
		public Characters.Actions.Action equipHead
		{
			get
			{
				return this._equipHead;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002BA5 RID: 11173 RVA: 0x00086541 File Offset: 0x00084741
		public Characters.Actions.Action scratchHead
		{
			get
			{
				return this._scratchHead;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002BA6 RID: 11174 RVA: 0x00086549 File Offset: 0x00084749
		public Characters.Actions.Action blink
		{
			get
			{
				return this._blink;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002BA7 RID: 11175 RVA: 0x00086551 File Offset: 0x00084751
		public Characters.Actions.Action getBone
		{
			get
			{
				return this._getBone;
			}
		}

		// Token: 0x040024FD RID: 9469
		[SerializeField]
		private Characters.Actions.Action _idle;

		// Token: 0x040024FE RID: 9470
		[SerializeField]
		private Characters.Actions.Action _openEyes;

		// Token: 0x040024FF RID: 9471
		[SerializeField]
		private Characters.Actions.Action _equipHead;

		// Token: 0x04002500 RID: 9472
		[SerializeField]
		private Characters.Actions.Action _scratchHead;

		// Token: 0x04002501 RID: 9473
		[SerializeField]
		private Characters.Actions.Action _blink;

		// Token: 0x04002502 RID: 9474
		[SerializeField]
		private Characters.Actions.Action _getBone;
	}
}
