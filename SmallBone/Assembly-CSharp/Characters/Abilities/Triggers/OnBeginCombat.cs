using System;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B26 RID: 2854
	[Serializable]
	public class OnBeginCombat : Trigger
	{
		// Token: 0x060039D1 RID: 14801 RVA: 0x000AAC46 File Offset: 0x000A8E46
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.playerComponents.combatDetector.onBeginCombat += base.Invoke;
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x000AAC70 File Offset: 0x000A8E70
		public override void Detach()
		{
			this._character.playerComponents.combatDetector.onBeginCombat -= base.Invoke;
		}

		// Token: 0x04002DE0 RID: 11744
		private Character _character;
	}
}
