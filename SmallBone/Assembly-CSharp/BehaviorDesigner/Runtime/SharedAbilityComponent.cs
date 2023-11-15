using System;
using Characters.Abilities;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001441 RID: 5185
	[Serializable]
	public class SharedAbilityComponent : SharedVariable<AbilityComponent>
	{
		// Token: 0x060065A8 RID: 26024 RVA: 0x00126225 File Offset: 0x00124425
		public static implicit operator SharedAbilityComponent(AbilityComponent value)
		{
			return new SharedAbilityComponent
			{
				Value = value
			};
		}
	}
}
