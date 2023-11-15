using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D8A RID: 3466
	public class RockstarPassiveComponent : AbilityComponent<RockstarPassive>
	{
		// Token: 0x060045C8 RID: 17864 RVA: 0x000CA579 File Offset: 0x000C8779
		public void AddStack(int amount)
		{
			this._ability.AddStack(amount);
		}
	}
}
