using System;
using Characters.Abilities.Weapons.Wizard;
using UnityEngine;

namespace Characters.Operations.Customs.Skeleton_Mage
{
	// Token: 0x02001004 RID: 4100
	public sealed class AddManaChargingSpeedMultiplier : CharacterOperation
	{
		// Token: 0x06004F3C RID: 20284 RVA: 0x000EE568 File Offset: 0x000EC768
		public override void Run(Character owner)
		{
			this._passive.manaChargingSpeedMultiplier += this._value;
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x000EE582 File Offset: 0x000EC782
		public override void Stop()
		{
			base.Stop();
			this._passive.manaChargingSpeedMultiplier -= this._value;
		}

		// Token: 0x04003F61 RID: 16225
		[SerializeField]
		private WizardPassiveComponent _passive;

		// Token: 0x04003F62 RID: 16226
		[SerializeField]
		private float _value;
	}
}
