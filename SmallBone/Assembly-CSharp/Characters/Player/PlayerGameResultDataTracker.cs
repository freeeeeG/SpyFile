using System;
using Data;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007FA RID: 2042
	public sealed class PlayerGameResultDataTracker : MonoBehaviour
	{
		// Token: 0x0600297F RID: 10623 RVA: 0x0007EAD8 File Offset: 0x0007CCD8
		private void Awake()
		{
			Character character = this._character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			this._character.health.onTookDamage += new TookDamageDelegate(this.HandleOnTookDamage);
			this._character.health.onHealed += this.HandleOnHealed;
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x0007EB44 File Offset: 0x0007CD44
		private void HandleOnHealed(double healed, double overHealed)
		{
			GameData.Progress.totalHeal += (int)healed;
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x0007EB54 File Offset: 0x0007CD54
		private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			Damage damage = tookDamage;
			int num = (int)damage.amount;
			GameData.Progress.totalTakingDamage += num;
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x0007EB80 File Offset: 0x0007CD80
		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (target.character == null)
			{
				return;
			}
			Damage damage = gaveDamage;
			int num = (int)damage.amount;
			GameData.Progress.totalDamage += num;
			GameData.Progress.bestDamage = Mathf.Max(num, GameData.Progress.bestDamage);
		}

		// Token: 0x040023AA RID: 9130
		[SerializeField]
		private Character _character;
	}
}
