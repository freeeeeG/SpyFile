using System;
using Characters.Actions;
using Characters.Player;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AC4 RID: 2756
	[Serializable]
	public sealed class ReduceCooldownAnotherSkill : Ability
	{
		// Token: 0x060038AC RID: 14508 RVA: 0x000A70E4 File Offset: 0x000A52E4
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ReduceCooldownAnotherSkill.Instance(owner, this);
		}

		// Token: 0x04002D11 RID: 11537
		[SerializeField]
		private bool _containsNextWeapon;

		// Token: 0x04002D12 RID: 11538
		[SerializeField]
		private ReduceCooldownAnotherSkill.Type _type;

		// Token: 0x04002D13 RID: 11539
		[SerializeField]
		private ReduceCooldownAnotherSkill.ValueType _valueType;

		// Token: 0x04002D14 RID: 11540
		[SerializeField]
		private float _amount;

		// Token: 0x04002D15 RID: 11541
		[Range(0f, 100f)]
		[SerializeField]
		private int _remainPercent;

		// Token: 0x02000AC5 RID: 2757
		public class Instance : AbilityInstance<ReduceCooldownAnotherSkill>
		{
			// Token: 0x060038AE RID: 14510 RVA: 0x000A70ED File Offset: 0x000A52ED
			public Instance(Character owner, ReduceCooldownAnotherSkill ability) : base(owner, ability)
			{
			}

			// Token: 0x060038AF RID: 14511 RVA: 0x000A70F7 File Offset: 0x000A52F7
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.HandleOnStartAction;
			}

			// Token: 0x060038B0 RID: 14512 RVA: 0x000A7110 File Offset: 0x000A5310
			private void HandleOnStartAction(Characters.Actions.Action skill)
			{
				if (skill.type != Characters.Actions.Action.Type.Skill)
				{
					return;
				}
				WeaponInventory weapon = this.owner.playerComponents.inventory.weapon;
				this.Reduce(weapon.current.actionsByType[Characters.Actions.Action.Type.Skill], skill);
				if (this.ability._containsNextWeapon && weapon.next != null)
				{
					this.Reduce(weapon.next.actionsByType[Characters.Actions.Action.Type.Skill], skill);
				}
			}

			// Token: 0x060038B1 RID: 14513 RVA: 0x000A7188 File Offset: 0x000A5388
			private void Reduce(Characters.Actions.Action[] actions, Characters.Actions.Action except)
			{
				foreach (Characters.Actions.Action action in actions)
				{
					if (action.type == Characters.Actions.Action.Type.Skill && !(action == except))
					{
						ReduceCooldownAnotherSkill.Type type = this.ability._type;
						if (type != ReduceCooldownAnotherSkill.Type.Set)
						{
							if (type == ReduceCooldownAnotherSkill.Type.Reduce)
							{
								if (action.cooldown.time == null)
								{
									return;
								}
								ReduceCooldownAnotherSkill.ValueType valueType = this.ability._valueType;
								if (valueType != ReduceCooldownAnotherSkill.ValueType.Constant)
								{
									if (valueType == ReduceCooldownAnotherSkill.ValueType.RemainPercent)
									{
										float time = action.cooldown.time.remainTime * (float)this.ability._remainPercent * 0.01f;
										action.cooldown.time.ReduceCooldown(time);
									}
								}
								else
								{
									action.cooldown.time.ReduceCooldown(this.ability._amount);
								}
							}
						}
						else
						{
							ReduceCooldownAnotherSkill.ValueType valueType = this.ability._valueType;
							if (valueType != ReduceCooldownAnotherSkill.ValueType.Constant)
							{
								if (valueType == ReduceCooldownAnotherSkill.ValueType.RemainPercent)
								{
									float remainTime = action.cooldown.time.remainTime * (float)this.ability._remainPercent * 0.01f;
									action.cooldown.time.remainTime = remainTime;
								}
							}
							else
							{
								action.cooldown.time.remainTime = this.ability._amount;
								if (action.cooldown.time.remainTime < 0f)
								{
									action.cooldown.time.remainTime = 0f;
								}
							}
						}
					}
				}
			}

			// Token: 0x060038B2 RID: 14514 RVA: 0x000A72FF File Offset: 0x000A54FF
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.HandleOnStartAction;
			}
		}

		// Token: 0x02000AC6 RID: 2758
		private enum Type
		{
			// Token: 0x04002D17 RID: 11543
			Set,
			// Token: 0x04002D18 RID: 11544
			Reduce
		}

		// Token: 0x02000AC7 RID: 2759
		private enum ValueType
		{
			// Token: 0x04002D1A RID: 11546
			Constant,
			// Token: 0x04002D1B RID: 11547
			RemainPercent
		}
	}
}
