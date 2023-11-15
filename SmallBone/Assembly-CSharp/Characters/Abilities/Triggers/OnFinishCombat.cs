using System;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B37 RID: 2871
	[Serializable]
	public class OnFinishCombat : Trigger
	{
		// Token: 0x060039F4 RID: 14836 RVA: 0x000AB130 File Offset: 0x000A9330
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.playerComponents.combatDetector.onFinishCombat += base.Invoke;
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x000AB15A File Offset: 0x000A935A
		public override void Detach()
		{
			this._character.playerComponents.combatDetector.onFinishCombat -= base.Invoke;
		}

		// Token: 0x04002DF6 RID: 11766
		private Character _character;
	}
}
