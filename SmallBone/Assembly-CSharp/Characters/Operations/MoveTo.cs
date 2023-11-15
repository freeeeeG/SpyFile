using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DF1 RID: 3569
	public class MoveTo : TargetedCharacterOperation
	{
		// Token: 0x06004770 RID: 18288 RVA: 0x000CF8CD File Offset: 0x000CDACD
		public override void Run(Character owner, Character target)
		{
			target.transform.position = this._position.position;
		}

		// Token: 0x04003677 RID: 13943
		[SerializeField]
		private Transform _position;
	}
}
