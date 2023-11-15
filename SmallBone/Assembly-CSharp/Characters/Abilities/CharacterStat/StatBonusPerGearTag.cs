using System;
using Characters.Gear;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C73 RID: 3187
	[Serializable]
	public class StatBonusPerGearTag : Ability
	{
		// Token: 0x0600411A RID: 16666 RVA: 0x00089C49 File Offset: 0x00087E49
		public StatBonusPerGearTag()
		{
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x000BD166 File Offset: 0x000BB366
		public StatBonusPerGearTag(Stat.Values stat)
		{
			this._statPerGearTag = stat;
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x000BD175 File Offset: 0x000BB375
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusPerGearTag.Instance(owner, this);
		}

		// Token: 0x040031EB RID: 12779
		[SerializeField]
		private Gear.Tag _tag;

		// Token: 0x040031EC RID: 12780
		[SerializeField]
		private Stat.Values _statPerGearTag;

		// Token: 0x02000C74 RID: 3188
		public class Instance : AbilityInstance<StatBonusPerGearTag>
		{
			// Token: 0x17000DB5 RID: 3509
			// (get) Token: 0x0600411D RID: 16669 RVA: 0x000BD17E File Offset: 0x000BB37E
			public override int iconStacks
			{
				get
				{
					return this.owner.playerComponents.inventory.item.GetItemCountByTag(this.ability._tag);
				}
			}

			// Token: 0x0600411E RID: 16670 RVA: 0x000BD1A5 File Offset: 0x000BB3A5
			public Instance(Character owner, StatBonusPerGearTag ability) : base(owner, ability)
			{
			}

			// Token: 0x0600411F RID: 16671 RVA: 0x000BD1B0 File Offset: 0x000BB3B0
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerGearTag.Clone();
				this.owner.stat.AttachValues(this._stat);
				this.UpdateStatBonus();
				this.owner.playerComponents.inventory.item.onChanged += this.UpdateStatBonus;
			}

			// Token: 0x06004120 RID: 16672 RVA: 0x000BD215 File Offset: 0x000BB415
			protected override void OnDetach()
			{
				this.owner.playerComponents.inventory.item.onChanged -= this.UpdateStatBonus;
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004121 RID: 16673 RVA: 0x000BD254 File Offset: 0x000BB454
			private void UpdateStatBonus()
			{
				int itemCountByTag = this.owner.playerComponents.inventory.item.GetItemCountByTag(this.ability._tag);
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = (double)itemCountByTag * this.ability._statPerGearTag.values[i].value;
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040031ED RID: 12781
			private Stat.Values _stat;
		}
	}
}
