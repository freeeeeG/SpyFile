using System;
using Characters.Actions;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DDE RID: 3550
	public class AbyssLaser : CharacterOperation, IAttackDamage
	{
		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06004730 RID: 18224 RVA: 0x000CEB92 File Offset: 0x000CCD92
		public float amount
		{
			get
			{
				return this._damageMultiplier * this._attackDamage.amount;
			}
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x000CEBA6 File Offset: 0x000CCDA6
		private void Awake()
		{
			if (this._spawnPosition == null)
			{
				this._spawnPosition = base.transform;
			}
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x000CEBC4 File Offset: 0x000CCDC4
		public override void Run(Character owner)
		{
			this._damageMultiplier = (this._damageMultiplierMax - this._damageMultiplierMin) * this._chargeAction.chargedPercent + this._damageMultiplierMin;
			EffectPoolInstance effectPoolInstance = this._info.Spawn(this._spawnPosition.position, owner, this._spawnPosition.rotation.eulerAngles.z, 1f);
			if (this._attachToSpawnPosition)
			{
				effectPoolInstance.transform.parent = this._spawnPosition;
			}
			float y = (this._yScaleMax - this._yScaleMin) * this._chargeAction.chargedPercent + this._yScaleMin;
			effectPoolInstance.transform.localScale = new Vector3(1f, y, 1f);
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x000CEC81 File Offset: 0x000CCE81
		public override void Stop()
		{
			this._info.DespawnChildren();
		}

		// Token: 0x04003613 RID: 13843
		[SerializeField]
		private ChargeAction _chargeAction;

		// Token: 0x04003614 RID: 13844
		[SerializeField]
		[Header("Effect")]
		private float _yScaleMin;

		// Token: 0x04003615 RID: 13845
		[SerializeField]
		private float _yScaleMax;

		// Token: 0x04003616 RID: 13846
		[Space]
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04003617 RID: 13847
		[SerializeField]
		private bool _attachToSpawnPosition;

		// Token: 0x04003618 RID: 13848
		[SerializeField]
		private EffectInfo _info;

		// Token: 0x04003619 RID: 13849
		[SerializeField]
		[Header("Attack")]
		private AttackDamage _attackDamage;

		// Token: 0x0400361A RID: 13850
		private float _damageMultiplier;

		// Token: 0x0400361B RID: 13851
		[SerializeField]
		[Space]
		private float _damageMultiplierMin;

		// Token: 0x0400361C RID: 13852
		[SerializeField]
		private float _damageMultiplierMax;
	}
}
