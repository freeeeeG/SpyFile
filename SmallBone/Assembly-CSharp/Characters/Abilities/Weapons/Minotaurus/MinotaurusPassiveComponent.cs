using System;

namespace Characters.Abilities.Weapons.Minotaurus
{
	// Token: 0x02000C09 RID: 3081
	public sealed class MinotaurusPassiveComponent : AbilityComponent<MinotaurusPassive>
	{
		// Token: 0x06003F3B RID: 16187 RVA: 0x000B7793 File Offset: 0x000B5993
		public void StartRecordingAttacks()
		{
			this._ability.StartRecordingAttacks();
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x000B77A0 File Offset: 0x000B59A0
		public void StopRecodingAttacks()
		{
			this._ability.StopRecodingAttacks();
		}
	}
}
