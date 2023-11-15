using System;
using Platforms;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008B2 RID: 2226
	public sealed class Sin : InscriptionInstance
	{
		// Token: 0x06002F63 RID: 12131 RVA: 0x00002191 File Offset: 0x00000391
		protected override void Initialize()
		{
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x0008E041 File Offset: 0x0008C241
		public override void Attach()
		{
			Achievement.Type.HateTheSinnerButLoveTheSin.Set();
			base.character.stat.adaptiveAttribute = true;
			base.character.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x0008E07C File Offset: 0x0008C27C
		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			Character character = target.character;
			if (character.type == Character.Type.Player || character.type == Character.Type.PlayerMinion)
			{
				return false;
			}
			damage.multiplier -= 0.30000001192092896;
			return false;
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x0008E0B8 File Offset: 0x0008C2B8
		public override void Detach()
		{
			base.character.stat.adaptiveAttribute = false;
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
		}

		// Token: 0x04002722 RID: 10018
		[SerializeField]
		[Header("1세트 효과")]
		private float[] _damageReducePercentPoints;
	}
}
