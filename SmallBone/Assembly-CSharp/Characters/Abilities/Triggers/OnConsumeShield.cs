using System;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B2B RID: 2859
	[Serializable]
	public class OnConsumeShield : Trigger
	{
		// Token: 0x060039DB RID: 14811 RVA: 0x000AAE00 File Offset: 0x000A9000
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.health.onConsumeShield += this.HandleOnConsumeShield;
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x000AAE25 File Offset: 0x000A9025
		public override void Detach()
		{
			this._character.health.onConsumeShield -= this.HandleOnConsumeShield;
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x000AAE43 File Offset: 0x000A9043
		private void HandleOnConsumeShield()
		{
			base.Invoke();
		}

		// Token: 0x04002DE7 RID: 11751
		private Character _character;
	}
}
