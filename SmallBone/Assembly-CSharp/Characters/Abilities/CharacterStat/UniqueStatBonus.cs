using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C8F RID: 3215
	[Serializable]
	public class UniqueStatBonus : Ability
	{
		// Token: 0x0600417B RID: 16763 RVA: 0x00089C49 File Offset: 0x00087E49
		public UniqueStatBonus()
		{
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x000BE7C0 File Offset: 0x000BC9C0
		public UniqueStatBonus(Stat.Values stat)
		{
			this._stat = stat;
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x000BE7CF File Offset: 0x000BC9CF
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new UniqueStatBonus.Instance(owner, this);
		}

		// Token: 0x04003237 RID: 12855
		[SerializeField]
		private string _key;

		// Token: 0x04003238 RID: 12856
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x04003239 RID: 12857
		private static readonly IDictionary<string, List<UniqueStatBonus.Instance>> keys = new Dictionary<string, List<UniqueStatBonus.Instance>>();

		// Token: 0x02000C90 RID: 3216
		public class Instance : AbilityInstance<UniqueStatBonus>
		{
			// Token: 0x0600417F RID: 16767 RVA: 0x000BE7E4 File Offset: 0x000BC9E4
			public Instance(Character owner, UniqueStatBonus ability) : base(owner, ability)
			{
			}

			// Token: 0x06004180 RID: 16768 RVA: 0x000BE7F0 File Offset: 0x000BC9F0
			protected override void OnAttach()
			{
				if (UniqueStatBonus.keys.ContainsKey(this.ability._key))
				{
					if (UniqueStatBonus.keys[this.ability._key].Count == 0)
					{
						this.AttachStatBonus();
					}
					UniqueStatBonus.keys[this.ability._key].Add(this);
					return;
				}
				UniqueStatBonus.keys.Add(this.ability._key, new List<UniqueStatBonus.Instance>
				{
					this
				});
				this.AttachStatBonus();
			}

			// Token: 0x06004181 RID: 16769 RVA: 0x000BE87C File Offset: 0x000BCA7C
			protected override void OnDetach()
			{
				if (!UniqueStatBonus.keys.ContainsKey(this.ability._key))
				{
					this.owner.stat.DetachValues(this.ability._stat);
					return;
				}
				List<UniqueStatBonus.Instance> list = UniqueStatBonus.keys[this.ability._key];
				list.Remove(this);
				this.owner.stat.DetachValues(this.ability._stat);
				this.attachedStatBonus = false;
				if (list.Count((UniqueStatBonus.Instance key) => key.attachedStatBonus) > 0)
				{
					return;
				}
				if (list.Count > 0)
				{
					list[0].AttachStatBonus();
					return;
				}
				UniqueStatBonus.keys.Remove(this.ability._key);
			}

			// Token: 0x06004182 RID: 16770 RVA: 0x000BE951 File Offset: 0x000BCB51
			public void AttachStatBonus()
			{
				this.owner.stat.AttachOrUpdateValues(this.ability._stat);
				this.attachedStatBonus = true;
			}

			// Token: 0x0400323A RID: 12858
			public bool attachedStatBonus;
		}
	}
}
