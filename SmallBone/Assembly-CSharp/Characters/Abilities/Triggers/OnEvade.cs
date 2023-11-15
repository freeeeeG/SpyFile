using System;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B35 RID: 2869
	[Serializable]
	public class OnEvade : Trigger
	{
		// Token: 0x060039EF RID: 14831 RVA: 0x000AB0EF File Offset: 0x000A92EF
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.onEvade += this.OnCharacterEvade;
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x000AB10F File Offset: 0x000A930F
		public override void Detach()
		{
			this._character.onEvade -= this.OnCharacterEvade;
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x000AAE43 File Offset: 0x000A9043
		private void OnCharacterEvade(ref Damage damage)
		{
			base.Invoke();
		}

		// Token: 0x04002DF5 RID: 11765
		private Character _character;
	}
}
