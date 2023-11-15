using System;
using Characters.Abilities.CharacterStat;
using Characters.Movements;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008B3 RID: 2227
	public sealed class Soar : InscriptionInstance
	{
		// Token: 0x06002F69 RID: 12137 RVA: 0x0008E0E8 File Offset: 0x0008C2E8
		protected override void Initialize()
		{
			this._statBonus.Initialize();
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x0008E0F8 File Offset: 0x0008C2F8
		public override void Attach()
		{
			base.character.movement.onJump += this.OnJump;
			base.character.movement.onGrounded += this.OnGrounded;
			base.character.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.OnGiveDamage));
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x0008E160 File Offset: 0x0008C360
		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if (this.keyword.step < this.keyword.steps.Count - 1)
			{
				return false;
			}
			if (base.character.movement.controller.isGrounded)
			{
				return false;
			}
			damage.percentMultiplier *= (double)this._damageMultiplier;
			return false;
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x0008E1B9 File Offset: 0x0008C3B9
		private void OnGrounded()
		{
			if (this.keyword.step < 1)
			{
				return;
			}
			base.character.ability.Remove(this._statBonus);
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x0008E1E1 File Offset: 0x0008C3E1
		private void OnJump(Movement.JumpType jumpType, float jumpHeight)
		{
			if (this.keyword.step < 1)
			{
				return;
			}
			base.character.ability.Add(this._statBonus);
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x0008E20C File Offset: 0x0008C40C
		public override void Detach()
		{
			base.character.movement.onJump -= this.OnJump;
			base.character.movement.onGrounded -= this.OnGrounded;
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			base.character.ability.Remove(this._statBonus);
		}

		// Token: 0x04002723 RID: 10019
		[SerializeField]
		[Header("2세트 효과")]
		private StatBonus _statBonus;

		// Token: 0x04002724 RID: 10020
		[Header("4세트 효과")]
		[SerializeField]
		private float _damageMultiplier;
	}
}
