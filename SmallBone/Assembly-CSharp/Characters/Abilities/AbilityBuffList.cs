using System;
using System.Linq;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000996 RID: 2454
	public class AbilityBuffList : ScriptableObject
	{
		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x060034CA RID: 13514 RVA: 0x0009B833 File Offset: 0x00099A33
		public AbilityBuff[] abilityBuff
		{
			get
			{
				return this._abilityBuff;
			}
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x0009B83C File Offset: 0x00099A3C
		public AbilityBuff Take(System.Random random, Rarity rarity)
		{
			return (from food in this._abilityBuff
			where food.rarity == rarity
			select food).Random(random);
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x0009B874 File Offset: 0x00099A74
		public AbilityBuff Get(string name)
		{
			foreach (AbilityBuff abilityBuff2 in this._abilityBuff)
			{
				if (abilityBuff2.name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return abilityBuff2;
				}
			}
			return null;
		}

		// Token: 0x04002A85 RID: 10885
		[SerializeField]
		private AbilityBuff[] _abilityBuff;
	}
}
