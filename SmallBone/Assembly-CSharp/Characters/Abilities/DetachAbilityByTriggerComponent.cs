using System;

namespace Characters.Abilities
{
	// Token: 0x02000A1C RID: 2588
	public sealed class DetachAbilityByTriggerComponent : AbilityComponent<DetachAbilityByTrigger>
	{
		// Token: 0x060036D7 RID: 14039 RVA: 0x000A2644 File Offset: 0x000A0844
		private void OnDestroy()
		{
			base.baseAbility.Destroy();
		}
	}
}
