using System;
using Characters.Gear.Weapons;
using Services;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D2C RID: 3372
	public class Berserker2PassiveComponent : AbilityComponent<Berserker2Passive>
	{
		// Token: 0x060043FA RID: 17402 RVA: 0x000C59BD File Offset: 0x000C3BBD
		private void Awake()
		{
			this._polymorphWeapon = UnityEngine.Object.Instantiate<Weapon>(this._polymorphWeapon);
			this._polymorphWeapon.gameObject.SetActive(false);
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x000C59E1 File Offset: 0x000C3BE1
		public override void Initialize()
		{
			base.Initialize();
			this._ability._polymorphWeapon = this._polymorphWeapon;
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x000C59FA File Offset: 0x000C3BFA
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			UnityEngine.Object.Destroy(this._polymorphWeapon.gameObject);
		}

		// Token: 0x040033D3 RID: 13267
		[SerializeField]
		private Weapon _polymorphWeapon;
	}
}
