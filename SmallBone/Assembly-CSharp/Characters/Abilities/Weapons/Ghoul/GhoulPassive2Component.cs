using System;

namespace Characters.Abilities.Weapons.Ghoul
{
	// Token: 0x02000C27 RID: 3111
	public class GhoulPassive2Component : AbilityComponent<GhoulPassive2>
	{
		// Token: 0x06004001 RID: 16385 RVA: 0x000B9FBA File Offset: 0x000B81BA
		public void Recover()
		{
			this._ability.Recover();
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x000B9FC7 File Offset: 0x000B81C7
		public void AddStack()
		{
			this._ability.AddStack();
		}
	}
}
