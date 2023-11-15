using System;
using Characters.Actions;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A01 RID: 2561
	[Serializable]
	public class ChangeAction : Ability
	{
		// Token: 0x06003679 RID: 13945 RVA: 0x000A12D0 File Offset: 0x0009F4D0
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ChangeAction.Instance(owner, this);
		}

		// Token: 0x04002BA5 RID: 11173
		[SerializeField]
		[Information("반드시 개수와 순서가 일치해야함, SkillInfo를 가지고 있을 경우 actions 대신 skills쪽에 할당", InformationAttribute.InformationType.Info, true)]
		private Weapon _weapon;

		// Token: 0x04002BA6 RID: 11174
		[Space]
		[SerializeField]
		private SkillInfo[] _skills;

		// Token: 0x04002BA7 RID: 11175
		[SerializeField]
		private SkillInfo[] _skillsToChange;

		// Token: 0x04002BA8 RID: 11176
		[Space]
		[SerializeField]
		private Characters.Actions.Action[] _actions;

		// Token: 0x04002BA9 RID: 11177
		[SerializeField]
		private Characters.Actions.Action[] _actionsToChange;

		// Token: 0x04002BAA RID: 11178
		[SerializeField]
		private bool _copyCooldown;

		// Token: 0x02000A02 RID: 2562
		public class Instance : AbilityInstance<ChangeAction>
		{
			// Token: 0x0600367B RID: 13947 RVA: 0x000A12D9 File Offset: 0x0009F4D9
			public Instance(Character owner, ChangeAction ability) : base(owner, ability)
			{
			}

			// Token: 0x0600367C RID: 13948 RVA: 0x000A12E4 File Offset: 0x0009F4E4
			protected override void OnAttach()
			{
				this.owner.playerComponents.inventory.weapon.onSwap += this.OnSwap;
				for (int i = 0; i < this.ability._actions.Length; i++)
				{
					this.ability._weapon.ChangeAction(this.ability._actions[i], this.ability._actionsToChange[i]);
					if (this.ability._copyCooldown)
					{
						this.ability._actionsToChange[i].cooldown.CopyCooldown(this.ability._actions[i].cooldown);
					}
				}
				this.ability._weapon.AttachSkillChanges(this.ability._skills, this.ability._skillsToChange, this.ability._copyCooldown);
			}

			// Token: 0x0600367D RID: 13949 RVA: 0x000A13C0 File Offset: 0x0009F5C0
			protected override void OnDetach()
			{
				this.owner.playerComponents.inventory.weapon.onSwap -= this.OnSwap;
				for (int i = 0; i < this.ability._actions.Length; i++)
				{
					this.ability._weapon.ChangeAction(this.ability._actionsToChange[i], this.ability._actions[i]);
					if (this.ability._copyCooldown)
					{
						this.ability._actions[i].cooldown.CopyCooldown(this.ability._actionsToChange[i].cooldown);
					}
				}
				this.ability._weapon.DetachSkillChanges(this.ability._skills, this.ability._skillsToChange, this.ability._copyCooldown);
			}

			// Token: 0x0600367E RID: 13950 RVA: 0x000A149C File Offset: 0x0009F69C
			private void OnSwap()
			{
				if (this.owner.playerComponents.inventory.weapon.current == this.ability._weapon)
				{
					SkillInfo[] array = this.ability._skills;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].gameObject.SetActive(true);
					}
					array = this.ability._skillsToChange;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].gameObject.SetActive(true);
					}
					Characters.Actions.Action[] array2 = this.ability._actions;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].gameObject.SetActive(true);
					}
					array2 = this.ability._actionsToChange;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].gameObject.SetActive(true);
					}
				}
			}
		}
	}
}
