using System;
using System.Collections;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D16 RID: 3350
	public class LivingArmor2PassiveAttacher : AbilityAttacher
	{
		// Token: 0x0600438C RID: 17292 RVA: 0x000C4C4D File Offset: 0x000C2E4D
		public override void OnIntialize()
		{
			this._targetValue = this._gauge.maxValue * this._enhancedGaugePercent;
			this._livingArmorPassive.Initialize();
			this._livingArmorPassive2.Initialize();
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x000C4C7D File Offset: 0x000C2E7D
		public override void StartAttach()
		{
			base.owner.StartCoroutine(this.CStartAttach());
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x000C4C91 File Offset: 0x000C2E91
		private IEnumerator CStartAttach()
		{
			yield return null;
			this._gauge.onChanged += this.OnGaugeValueChanged;
			if (!this.alreadyAttached)
			{
				this.OnGaugeValueChanged(0f, this._gauge.currentValue);
				this.alreadyAttached = true;
			}
			yield break;
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x000C4CA0 File Offset: 0x000C2EA0
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			this._gauge.onChanged -= this.OnGaugeValueChanged;
			base.owner.ability.Remove(this._livingArmorPassive.ability);
			base.owner.ability.Remove(this._livingArmorPassive2.ability);
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x000C4D0C File Offset: 0x000C2F0C
		private void OnGaugeValueChanged(float oldValue, float newValue)
		{
			if (oldValue > newValue)
			{
				return;
			}
			if (newValue == this._gauge.maxValue && !base.owner.ability.Contains(this._livingArmorPassive2.ability))
			{
				base.owner.ability.Remove(this._livingArmorPassive.ability);
				base.owner.ability.Add(this._livingArmorPassive2.ability);
				return;
			}
			if (this._targetValue <= newValue && !base.owner.ability.Contains(this._livingArmorPassive.ability) && !base.owner.ability.Contains(this._livingArmorPassive2.ability))
			{
				base.owner.ability.Add(this._livingArmorPassive.ability);
			}
		}

		// Token: 0x0400339A RID: 13210
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x0400339B RID: 13211
		[SerializeField]
		[Header("Passive Components")]
		private LivingArmorPassiveComponent _livingArmorPassive;

		// Token: 0x0400339C RID: 13212
		[SerializeField]
		private LivingArmorPassiveComponent _livingArmorPassive2;

		// Token: 0x0400339D RID: 13213
		[Range(0f, 1f)]
		[SerializeField]
		private float _enhancedGaugePercent;

		// Token: 0x0400339E RID: 13214
		private float _targetValue;

		// Token: 0x0400339F RID: 13215
		private bool alreadyAttached;
	}
}
