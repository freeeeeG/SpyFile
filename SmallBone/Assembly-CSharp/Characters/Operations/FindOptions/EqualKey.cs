using System;
using UnityEngine;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000E9C RID: 3740
	[Serializable]
	public class EqualKey : ICondition
	{
		// Token: 0x060049D4 RID: 18900 RVA: 0x000D79AF File Offset: 0x000D5BAF
		public bool Satisfied(Character character)
		{
			return this._key == character.key;
		}

		// Token: 0x04003919 RID: 14617
		[SerializeField]
		private Key _key;
	}
}
