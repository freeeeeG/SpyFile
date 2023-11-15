using System;
using Characters;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200034E RID: 846
	public class CharacterDied : Trigger
	{
		// Token: 0x06000FE4 RID: 4068 RVA: 0x0002FAAA File Offset: 0x0002DCAA
		protected override bool Check()
		{
			return !(this._character == null) && this._character.health.dead;
		}

		// Token: 0x04000D06 RID: 3334
		[SerializeField]
		private Character _character;
	}
}
