using System;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008A4 RID: 2212
	public class Mutation : SimpleStatBonusKeyword
	{
		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06002F02 RID: 12034 RVA: 0x0008D1C2 File Offset: 0x0008B3C2
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06002F03 RID: 12035 RVA: 0x00088DBC File Offset: 0x00086FBC
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.PercentPoint;
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06002F04 RID: 12036 RVA: 0x0008D1CA File Offset: 0x0008B3CA
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.SwapCooldownSpeed;
			}
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x0008D1D4 File Offset: 0x0008B3D4
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.step == this._statBonusByStep.Length - 1 && !base.character.onGiveDamage.Contains(new GiveDamageDelegate(this.OnGiveDamage)))
			{
				base.character.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.OnGiveDamage));
				return;
			}
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x0008D25C File Offset: 0x0008B45C
		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if (damage.motionType != Damage.MotionType.Swap)
			{
				return false;
			}
			if (base.character == null)
			{
				return false;
			}
			Weapon current = base.character.playerComponents.inventory.weapon.current;
			int num = this._statBonusByRarity[current.rarity];
			damage.percentMultiplier *= (double)(1f + (float)num * 0.01f);
			return false;
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x0008D2CB File Offset: 0x0008B4CB
		public override void Detach()
		{
			base.Detach();
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
		}

		// Token: 0x040026EC RID: 9964
		[SerializeField]
		[Header("2세트 효과")]
		private double[] _statBonusByStep;

		// Token: 0x040026ED RID: 9965
		[Header("4세트 효과")]
		[SerializeField]
		private RarityPossibilities _statBonusByRarity;
	}
}
