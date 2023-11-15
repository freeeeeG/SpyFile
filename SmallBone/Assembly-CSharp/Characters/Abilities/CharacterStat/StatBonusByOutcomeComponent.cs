using System;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C61 RID: 3169
	public sealed class StatBonusByOutcomeComponent : AbilityComponent<StatBonusByOutcome>, IStackable
	{
		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x060040D9 RID: 16601 RVA: 0x000BC6CB File Offset: 0x000BA8CB
		// (set) Token: 0x060040DA RID: 16602 RVA: 0x000BC6DC File Offset: 0x000BA8DC
		public float stack
		{
			get
			{
				return (float)base.baseAbility.stack;
			}
			set
			{
				Character componentInParent = base.GetComponentInParent<Character>();
				if (componentInParent == null)
				{
					base.baseAbility.stack = (int)value;
					return;
				}
				base.baseAbility.Load(componentInParent, (int)value);
			}
		}
	}
}
