using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DE7 RID: 3559
	public class JustActivate : CharacterOperation
	{
		// Token: 0x0600474F RID: 18255 RVA: 0x000CF492 File Offset: 0x000CD692
		public override void Run(Character owner)
		{
			this._gameObject.SetActive(true);
		}

		// Token: 0x0400365E RID: 13918
		[SerializeField]
		private GameObject _gameObject;
	}
}
