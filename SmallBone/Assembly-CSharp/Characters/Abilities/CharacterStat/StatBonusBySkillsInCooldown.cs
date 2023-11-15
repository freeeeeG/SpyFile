using System;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Player;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C65 RID: 3173
	[Serializable]
	public class StatBonusBySkillsInCooldown : Ability
	{
		// Token: 0x060040E7 RID: 16615 RVA: 0x000BC9B5 File Offset: 0x000BABB5
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusBySkillsInCooldown.Instance(owner, this);
		}

		// Token: 0x040031D5 RID: 12757
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x040031D6 RID: 12758
		[SerializeField]
		private int _maxStack;

		// Token: 0x040031D7 RID: 12759
		[SerializeField]
		private int _skillInCooldownPerStack;

		// Token: 0x02000C66 RID: 3174
		public class Instance : AbilityInstance<StatBonusBySkillsInCooldown>
		{
			// Token: 0x17000DAD RID: 3501
			// (get) Token: 0x060040E9 RID: 16617 RVA: 0x000BC9BE File Offset: 0x000BABBE
			public override Sprite icon
			{
				get
				{
					if (this._stacks > 0)
					{
						return base.icon;
					}
					return null;
				}
			}

			// Token: 0x17000DAE RID: 3502
			// (get) Token: 0x060040EA RID: 16618 RVA: 0x000BC9D1 File Offset: 0x000BABD1
			public override int iconStacks
			{
				get
				{
					return this._stacks;
				}
			}

			// Token: 0x060040EB RID: 16619 RVA: 0x000BC9D9 File Offset: 0x000BABD9
			public Instance(Character owner, StatBonusBySkillsInCooldown ability) : base(owner, ability)
			{
			}

			// Token: 0x060040EC RID: 16620 RVA: 0x000BC9E3 File Offset: 0x000BABE3
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				this.owner.stat.AttachValues(this._stat);
			}

			// Token: 0x060040ED RID: 16621 RVA: 0x000BCA11 File Offset: 0x000BAC11
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x060040EE RID: 16622 RVA: 0x000BCA2C File Offset: 0x000BAC2C
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				int num = 0;
				WeaponInventory weapon = this.owner.playerComponents.inventory.weapon;
				Weapon current = weapon.current;
				Weapon next = weapon.next;
				SkillInfo[] skills = current.skills;
				for (int i = 0; i < skills.Length; i++)
				{
					Characters.Actions.Action action = skills[i].action;
					if (action.cooldown.time != null && !action.cooldown.canUse)
					{
						num++;
					}
				}
				if (next != null)
				{
					skills = next.skills;
					for (int i = 0; i < skills.Length; i++)
					{
						Characters.Actions.Action action2 = skills[i].action;
						if (action2.cooldown.time != null && !action2.cooldown.canUse)
						{
							num++;
						}
					}
				}
				if (num >= this.ability._maxStack)
				{
					num = this.ability._maxStack;
				}
				this._stacks = num;
				this.UpdateStat();
			}

			// Token: 0x060040EF RID: 16623 RVA: 0x000BCB1C File Offset: 0x000BAD1C
			private void UpdateStat()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this._stacks);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040031D8 RID: 12760
			private int _stacks;

			// Token: 0x040031D9 RID: 12761
			private Stat.Values _stat;
		}
	}
}
