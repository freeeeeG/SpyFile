using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E2D RID: 3629
	public class Polymorph : CharacterOperation
	{
		// Token: 0x0600486B RID: 18539 RVA: 0x000D287B File Offset: 0x000D0A7B
		public override void Run(Character target)
		{
			this._polymorph.character = target;
			this._polymorph.StartPolymorph(this._duration);
		}

		// Token: 0x04003773 RID: 14195
		[SerializeField]
		private PolymorphBody _polymorph;

		// Token: 0x04003774 RID: 14196
		[SerializeField]
		private float _duration;
	}
}
