using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200070E RID: 1806
	public class OnDieDeactivation : MonoBehaviour
	{
		// Token: 0x06002496 RID: 9366 RVA: 0x0006E06C File Offset: 0x0006C26C
		private void Start()
		{
			if (this._character == null)
			{
				this._character = base.GetComponentInParent<Character>();
			}
			this._character.health.onDiedTryCatch += this.OnDie;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x0006E0A4 File Offset: 0x0006C2A4
		private void Update()
		{
			if (this._character.health.dead)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x0006E0C4 File Offset: 0x0006C2C4
		private void OnDestroy()
		{
			if (this._character != null)
			{
				this._character.health.onDiedTryCatch -= this.OnDie;
			}
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x0006E0F0 File Offset: 0x0006C2F0
		private void OnDie()
		{
			this._character.health.onDiedTryCatch -= this.OnDie;
			base.gameObject.SetActive(false);
		}

		// Token: 0x04001F11 RID: 7953
		[SerializeField]
		private Character _character;
	}
}
