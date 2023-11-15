using System;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CB4 RID: 3252
	public sealed class CupOfFateComponent : AbilityComponent<CupOfFate>, IStackable
	{
		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x000C06E6 File Offset: 0x000BE8E6
		// (set) Token: 0x0600420F RID: 16911 RVA: 0x000C06F4 File Offset: 0x000BE8F4
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
