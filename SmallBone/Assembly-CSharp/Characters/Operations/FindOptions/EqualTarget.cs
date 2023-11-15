using System;
using UnityEngine;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000E9F RID: 3743
	[Serializable]
	public class EqualTarget : ICondition
	{
		// Token: 0x060049DA RID: 18906 RVA: 0x000D7A63 File Offset: 0x000D5C63
		public bool Satisfied(Character character)
		{
			return this._target == character;
		}

		// Token: 0x0400391D RID: 14621
		[SerializeField]
		private Character _target;
	}
}
