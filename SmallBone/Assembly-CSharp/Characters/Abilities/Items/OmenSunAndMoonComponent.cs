using System;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CF2 RID: 3314
	public sealed class OmenSunAndMoonComponent : AbilityComponent<OmenSunAndMoon>, IStackable
	{
		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x060042FE RID: 17150 RVA: 0x000C3695 File Offset: 0x000C1895
		// (set) Token: 0x060042FF RID: 17151 RVA: 0x000C36A3 File Offset: 0x000C18A3
		public float stack
		{
			get
			{
				return (float)base.baseAbility.stack;
			}
			set
			{
				Debug.Log(string.Format("Load {0}", value));
				base.baseAbility.stack = (int)value;
				base.baseAbility.LoadStack();
			}
		}
	}
}
