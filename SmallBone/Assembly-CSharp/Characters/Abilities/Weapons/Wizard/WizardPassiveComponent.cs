using System;

namespace Characters.Abilities.Weapons.Wizard
{
	// Token: 0x02000C04 RID: 3076
	public sealed class WizardPassiveComponent : AbilityComponent<WizardPassive>
	{
		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06003F19 RID: 16153 RVA: 0x000B7523 File Offset: 0x000B5723
		// (set) Token: 0x06003F1A RID: 16154 RVA: 0x000B7530 File Offset: 0x000B5730
		public bool transcendence
		{
			get
			{
				return base.baseAbility.transcendence;
			}
			set
			{
				base.baseAbility.transcendence = value;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06003F1B RID: 16155 RVA: 0x000B753E File Offset: 0x000B573E
		// (set) Token: 0x06003F1C RID: 16156 RVA: 0x000B754B File Offset: 0x000B574B
		public float manaChargingSpeedMultiplier
		{
			get
			{
				return base.baseAbility.manaChargingSpeedMultiplier;
			}
			set
			{
				base.baseAbility.manaChargingSpeedMultiplier = value;
			}
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x000B7559 File Offset: 0x000B5759
		public bool IsMaxGauge()
		{
			return base.baseAbility.IsMaxGauge();
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x000B7566 File Offset: 0x000B5766
		public bool TryReduceMana(float value)
		{
			return base.baseAbility.TryReduceMana(value);
		}
	}
}
