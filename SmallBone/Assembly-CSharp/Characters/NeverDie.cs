using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200070D RID: 1805
	public class NeverDie : MonoBehaviour
	{
		// Token: 0x06002493 RID: 9363 RVA: 0x0006E041 File Offset: 0x0006C241
		private void Awake()
		{
			this._character.onDie += this.OnDie;
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x0006E05A File Offset: 0x0006C25A
		private void OnDie()
		{
			this._character.health.ResetToMaximumHealth();
		}

		// Token: 0x04001F10 RID: 7952
		[SerializeField]
		[GetComponent]
		private Character _character;
	}
}
