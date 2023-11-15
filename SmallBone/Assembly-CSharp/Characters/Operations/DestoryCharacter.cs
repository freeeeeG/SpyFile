using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E16 RID: 3606
	public class DestoryCharacter : CharacterOperation
	{
		// Token: 0x060047FD RID: 18429 RVA: 0x000D1B6C File Offset: 0x000CFD6C
		public override void Run(Character owner)
		{
			this._character.gameObject.SetActive(false);
		}

		// Token: 0x04003721 RID: 14113
		[SerializeField]
		private Character _character;
	}
}
