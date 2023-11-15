using System;

namespace Characters.Abilities.Weapons
{
	// Token: 0x02000BEE RID: 3054
	public class PrisonerLancePassiveComponent : AbilityComponent<PrisonerLancePassive>
	{
		// Token: 0x06003EB4 RID: 16052 RVA: 0x000B6579 File Offset: 0x000B4779
		public void StartDetect()
		{
			base.baseAbility.StartDetect();
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x000B6586 File Offset: 0x000B4786
		public void StopDetect()
		{
			base.baseAbility.StopDetect();
		}
	}
}
