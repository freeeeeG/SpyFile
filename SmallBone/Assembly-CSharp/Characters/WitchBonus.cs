using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Characters.Abilities;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Data;
using FX.SpriteEffects;
using GameResources;
using PhysicsUtils;
using Platforms;
using Singletons;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000732 RID: 1842
	public class WitchBonus
	{
		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06002576 RID: 9590 RVA: 0x00070EF1 File Offset: 0x0006F0F1
		// (set) Token: 0x06002577 RID: 9591 RVA: 0x00070EF9 File Offset: 0x0006F0F9
		public WitchBonus.Skull skull { get; protected set; }

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002578 RID: 9592 RVA: 0x00070F02 File Offset: 0x0006F102
		// (set) Token: 0x06002579 RID: 9593 RVA: 0x00070F0A File Offset: 0x0006F10A
		public WitchBonus.Body body { get; protected set; }

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x00070F13 File Offset: 0x0006F113
		// (set) Token: 0x0600257B RID: 9595 RVA: 0x00070F1B File Offset: 0x0006F11B
		public WitchBonus.Soul soul { get; protected set; }

		// Token: 0x0600257C RID: 9596 RVA: 0x00070F24 File Offset: 0x0006F124
		public void Apply(Character character)
		{
			this.skull = new WitchBonus.Skull(character);
			this.body = new WitchBonus.Body(character);
			this.soul = new WitchBonus.Soul(character);
		}

		// Token: 0x04001FCD RID: 8141
		public static WitchBonus instance = new WitchBonus();

		// Token: 0x02000733 RID: 1843
		public abstract class Bonus
		{
			// Token: 0x170007CE RID: 1998
			// (get) Token: 0x0600257F RID: 9599 RVA: 0x00070F56 File Offset: 0x0006F156
			// (set) Token: 0x06002580 RID: 9600 RVA: 0x00070F64 File Offset: 0x0006F164
			public int level
			{
				get
				{
					return this._data.value;
				}
				set
				{
					if (value == 0 && this._data.value > 0)
					{
						this.Detach();
					}
					else if (value > 0 && this._data.value == 0)
					{
						this.Attach();
					}
					this._data.value = value;
					this.Update();
				}
			}

			// Token: 0x170007CF RID: 1999
			// (get) Token: 0x06002581 RID: 9601 RVA: 0x00070FB3 File Offset: 0x0006F1B3
			public int maxLevel
			{
				get
				{
					return this._costs.Length;
				}
			}

			// Token: 0x170007D0 RID: 2000
			// (get) Token: 0x06002582 RID: 9602 RVA: 0x00070FBD File Offset: 0x0006F1BD
			public int levelUpCost
			{
				get
				{
					return this._costs[this.level];
				}
			}

			// Token: 0x170007D1 RID: 2001
			// (get) Token: 0x06002583 RID: 9603 RVA: 0x00070FCC File Offset: 0x0006F1CC
			public bool ready
			{
				get
				{
					return this.indexInTree == 0 || this.tree.list[this.indexInTree - 1].level > 0;
				}
			}

			// Token: 0x170007D2 RID: 2002
			// (get) Token: 0x06002584 RID: 9604 RVA: 0x00070FF8 File Offset: 0x0006F1F8
			public string displayName
			{
				get
				{
					return Localization.GetLocalizedString(this._key);
				}
			}

			// Token: 0x06002585 RID: 9605 RVA: 0x00071005 File Offset: 0x0006F205
			public Bonus(string key, Character owner, IntData data, int[] costs)
			{
				this._key = key;
				this._owner = owner;
				this._data = data;
				this._costs = costs;
			}

			// Token: 0x06002586 RID: 9606 RVA: 0x0007102A File Offset: 0x0006F22A
			public virtual void Initialize()
			{
				if (this.level > 0)
				{
					this.Attach();
				}
				this.Update();
			}

			// Token: 0x06002587 RID: 9607 RVA: 0x00071044 File Offset: 0x0006F244
			public bool LevelUp()
			{
				if (!this.ready || this.level == this.maxLevel || !GameData.Currency.darkQuartz.Consume(this.levelUpCost))
				{
					return false;
				}
				int level = this.level;
				this.level = level + 1;
				GameData.Currency.SaveAll();
				GameData.Progress.SaveAll();
				PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
				return true;
			}

			// Token: 0x06002588 RID: 9608
			public abstract void Attach();

			// Token: 0x06002589 RID: 9609
			public abstract void Detach();

			// Token: 0x0600258A RID: 9610 RVA: 0x00002191 File Offset: 0x00000391
			protected virtual void Update()
			{
			}

			// Token: 0x0600258B RID: 9611 RVA: 0x000710A0 File Offset: 0x0006F2A0
			public override string ToString()
			{
				return this.GetDescription(this.level);
			}

			// Token: 0x0600258C RID: 9612 RVA: 0x000710AE File Offset: 0x0006F2AE
			public virtual string GetDescription(int level)
			{
				return Localization.GetLocalizedString(this._key + "/desc");
			}

			// Token: 0x04001FD1 RID: 8145
			public WitchBonus.Tree tree;

			// Token: 0x04001FD2 RID: 8146
			public int indexInTree;

			// Token: 0x04001FD3 RID: 8147
			protected readonly Character _owner;

			// Token: 0x04001FD4 RID: 8148
			protected readonly int[] _costs;

			// Token: 0x04001FD5 RID: 8149
			protected readonly string _key;

			// Token: 0x04001FD6 RID: 8150
			protected readonly IntData _data;
		}

		// Token: 0x02000734 RID: 1844
		public class StatBonus : WitchBonus.Bonus
		{
			// Token: 0x0600258D RID: 9613 RVA: 0x000710C5 File Offset: 0x0006F2C5
			public StatBonus(string key, Character owner, IntData data, int[] costs, Stat.Value statPerLevel) : base(key, owner, data, costs)
			{
				this.stat = new Stat.Values(new Stat.Value[]
				{
					statPerLevel.Clone()
				});
				this._statPerLevel = statPerLevel;
			}

			// Token: 0x0600258E RID: 9614 RVA: 0x000710F8 File Offset: 0x0006F2F8
			protected override void Update()
			{
				base.Update();
				if (Stat.Kind.values[this._statPerLevel.kindIndex].valueForm == Stat.Kind.ValueForm.Product)
				{
					this.stat.values[0].value = 1.0 - (double)base.level * this._statPerLevel.value;
				}
				else if (this._statPerLevel.categoryIndex == Stat.Category.Percent.index)
				{
					this.stat.values[0].value = 1.0 + (double)base.level * this._statPerLevel.value;
				}
				else
				{
					this.stat.values[0].value = (double)base.level * this._statPerLevel.value;
				}
				this._owner.stat.SetNeedUpdate();
			}

			// Token: 0x0600258F RID: 9615 RVA: 0x000711D6 File Offset: 0x0006F3D6
			public override string GetDescription(int level)
			{
				return string.Format(Localization.GetLocalizedString(this._key + "/desc"), this._statPerLevel.value * (double)level);
			}

			// Token: 0x06002590 RID: 9616 RVA: 0x00071205 File Offset: 0x0006F405
			public override void Attach()
			{
				this._owner.stat.AttachValues(this.stat);
				this.Update();
			}

			// Token: 0x06002591 RID: 9617 RVA: 0x00071223 File Offset: 0x0006F423
			public override void Detach()
			{
				this._owner.stat.DetachValues(this.stat);
			}

			// Token: 0x04001FD7 RID: 8151
			public readonly Stat.Values stat;

			// Token: 0x04001FD8 RID: 8152
			protected readonly Stat.Value _statPerLevel;
		}

		// Token: 0x02000735 RID: 1845
		public class StatBonusByWeaponCategory : WitchBonus.Bonus
		{
			// Token: 0x06002592 RID: 9618 RVA: 0x0007123B File Offset: 0x0006F43B
			public StatBonusByWeaponCategory(string key, Character owner, IntData data, int[] costs, Weapon.Category weaponCategory, Stat.Value statPerLevel) : base(key, owner, data, costs)
			{
				this._statPerLevel = new Stat.Values(new Stat.Value[]
				{
					statPerLevel
				});
				this.stat = this._statPerLevel.Clone();
				this._weaponCategory = weaponCategory;
			}

			// Token: 0x06002593 RID: 9619 RVA: 0x00071277 File Offset: 0x0006F477
			public StatBonusByWeaponCategory(string key, Character owner, IntData data, int[] costs, Weapon.Category weaponCategory, Stat.Values statsPerLevel) : base(key, owner, data, costs)
			{
				this._statPerLevel = statsPerLevel;
				this.stat = statsPerLevel.Clone();
				this._weaponCategory = weaponCategory;
			}

			// Token: 0x06002594 RID: 9620 RVA: 0x000712A4 File Offset: 0x0006F4A4
			protected override void Update()
			{
				base.Update();
				for (int i = 0; i < this.stat.values.Length; i++)
				{
					Stat.Value value = this.stat.values[i];
					if (this._owner.playerComponents.inventory.weapon.current.category == this._weaponCategory)
					{
						Stat.Value value2 = this._statPerLevel.values[i];
						if (Stat.Kind.values[value2.kindIndex].valueForm == Stat.Kind.ValueForm.Product)
						{
							value.value = 1.0 - (double)base.level * value2.value;
						}
						else if (value2.categoryIndex == Stat.Category.Percent.index)
						{
							value.value = 1.0 + (double)base.level * value2.value;
						}
						else
						{
							value.value = (double)base.level * value2.value;
						}
					}
					else if (value.IsCategory(Stat.Category.Percent.index))
					{
						value.value = 1.0;
					}
					else
					{
						value.value = 0.0;
					}
				}
				this._owner.stat.SetNeedUpdate();
			}

			// Token: 0x06002595 RID: 9621 RVA: 0x000713E0 File Offset: 0x0006F5E0
			public override string GetDescription(int level)
			{
				object[] args = (from stat in this._statPerLevel.values
				select stat.value * (double)level).ToArray<object>();
				return string.Format(Localization.GetLocalizedString(this._key + "/desc"), args);
			}

			// Token: 0x06002596 RID: 9622 RVA: 0x00071438 File Offset: 0x0006F638
			public override void Attach()
			{
				this._owner.stat.AttachValues(this.stat);
				this._owner.playerComponents.inventory.weapon.onSwap += this.Update;
				this._owner.playerComponents.inventory.weapon.onChanged += this.OnWeaponChanged;
				this.Update();
			}

			// Token: 0x06002597 RID: 9623 RVA: 0x000714B0 File Offset: 0x0006F6B0
			public override void Detach()
			{
				this._owner.stat.DetachValues(this.stat);
				this._owner.playerComponents.inventory.weapon.onSwap -= this.Update;
				this._owner.playerComponents.inventory.weapon.onChanged -= this.OnWeaponChanged;
			}

			// Token: 0x06002598 RID: 9624 RVA: 0x00071520 File Offset: 0x0006F720
			private void OnWeaponChanged(Weapon old, Weapon @new)
			{
				this.Update();
			}

			// Token: 0x04001FD9 RID: 8153
			public readonly Stat.Values stat;

			// Token: 0x04001FDA RID: 8154
			protected readonly Stat.Values _statPerLevel;

			// Token: 0x04001FDB RID: 8155
			protected readonly Weapon.Category _weaponCategory;
		}

		// Token: 0x02000737 RID: 1847
		public class GenericBonus : WitchBonus.Bonus
		{
			// Token: 0x170007D3 RID: 2003
			// (get) Token: 0x0600259B RID: 9627 RVA: 0x0007153D File Offset: 0x0006F73D
			public float bonus
			{
				get
				{
					return this._bonusPerLevel * (float)base.level;
				}
			}

			// Token: 0x0600259C RID: 9628 RVA: 0x0007154D File Offset: 0x0006F74D
			public GenericBonus(string key, Character owner, IntData data, int[] costs, float bonusPerLevel) : base(key, owner, data, costs)
			{
				this._bonusPerLevel = bonusPerLevel;
			}

			// Token: 0x0600259D RID: 9629 RVA: 0x00071562 File Offset: 0x0006F762
			public override string GetDescription(int level)
			{
				return string.Format(Localization.GetLocalizedString(this._key + "/desc"), this._bonusPerLevel * (float)level);
			}

			// Token: 0x0600259E RID: 9630 RVA: 0x00002191 File Offset: 0x00000391
			public override void Attach()
			{
			}

			// Token: 0x0600259F RID: 9631 RVA: 0x00002191 File Offset: 0x00000391
			public override void Detach()
			{
			}

			// Token: 0x04001FDD RID: 8157
			protected readonly float _bonusPerLevel;
		}

		// Token: 0x02000738 RID: 1848
		public class GetShieldOnSwap : WitchBonus.GenericBonus
		{
			// Token: 0x060025A0 RID: 9632 RVA: 0x0007158C File Offset: 0x0006F78C
			public GetShieldOnSwap(string key, Character owner, IntData data, int[] costs, float bonusPerLevel, float duration, float swapCooldownReduction) : base(key, owner, data, costs, bonusPerLevel)
			{
				WitchBonus.GetShieldOnSwap <>4__this = this;
				this._duration = duration;
				this._swapCooldownReduction = swapCooldownReduction;
				this._shield = new Shield(base.bonus)
				{
					duration = this._duration
				};
				this._shield.onDetach += delegate(Shield.Instance instance)
				{
					if (instance.amount > 0.0)
					{
						owner.playerComponents.inventory.weapon.ReduceSwapCooldown(<>4__this._swapCooldownReduction);
					}
				};
			}

			// Token: 0x060025A1 RID: 9633 RVA: 0x00071603 File Offset: 0x0006F803
			private void AttachShieldAbility()
			{
				this._shield.amount = base.bonus;
				this._owner.ability.Add(this._shield);
			}

			// Token: 0x060025A2 RID: 9634 RVA: 0x0007162D File Offset: 0x0006F82D
			public override void Attach()
			{
				this._owner.playerComponents.inventory.weapon.onSwap += this.AttachShieldAbility;
			}

			// Token: 0x060025A3 RID: 9635 RVA: 0x00071655 File Offset: 0x0006F855
			public override void Detach()
			{
				this._owner.playerComponents.inventory.weapon.onSwap -= this.AttachShieldAbility;
			}

			// Token: 0x060025A4 RID: 9636 RVA: 0x0007167D File Offset: 0x0006F87D
			public override string GetDescription(int level)
			{
				return string.Format(Localization.GetLocalizedString(this._key + "/desc"), this._bonusPerLevel * (float)level, this._duration, this._swapCooldownReduction);
			}

			// Token: 0x04001FDE RID: 8158
			private readonly Shield _shield;

			// Token: 0x04001FDF RID: 8159
			private readonly float _duration;

			// Token: 0x04001FE0 RID: 8160
			private readonly float _swapCooldownReduction;
		}

		// Token: 0x0200073A RID: 1850
		public class ReviveOnce : WitchBonus.Bonus, IAbility, IAbilityInstance
		{
			// Token: 0x170007D4 RID: 2004
			// (get) Token: 0x060025A7 RID: 9639 RVA: 0x000716F5 File Offset: 0x0006F8F5
			Character IAbilityInstance.owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x170007D5 RID: 2005
			// (get) Token: 0x060025A8 RID: 9640 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbility ability
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170007D6 RID: 2006
			// (get) Token: 0x060025A9 RID: 9641 RVA: 0x00071700 File Offset: 0x0006F900
			// (set) Token: 0x060025AA RID: 9642 RVA: 0x00071708 File Offset: 0x0006F908
			public float remainTime { get; set; }

			// Token: 0x170007D7 RID: 2007
			// (get) Token: 0x060025AB RID: 9643 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool attached
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170007D8 RID: 2008
			// (get) Token: 0x060025AC RID: 9644 RVA: 0x00071711 File Offset: 0x0006F911
			public Sprite icon { get; }

			// Token: 0x170007D9 RID: 2009
			// (get) Token: 0x060025AD RID: 9645 RVA: 0x00071719 File Offset: 0x0006F919
			public float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x170007DA RID: 2010
			// (get) Token: 0x060025AE RID: 9646 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillInversed
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170007DB RID: 2011
			// (get) Token: 0x060025AF RID: 9647 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillFlipped
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170007DC RID: 2012
			// (get) Token: 0x060025B0 RID: 9648 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconStacks
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x170007DD RID: 2013
			// (get) Token: 0x060025B1 RID: 9649 RVA: 0x00071720 File Offset: 0x0006F920
			public bool expired
			{
				get
				{
					return GameData.Progress.reassembleUsed;
				}
			}

			// Token: 0x170007DE RID: 2014
			// (get) Token: 0x060025B2 RID: 9650 RVA: 0x00071727 File Offset: 0x0006F927
			// (set) Token: 0x060025B3 RID: 9651 RVA: 0x0007172F File Offset: 0x0006F92F
			public float duration { get; set; }

			// Token: 0x170007DF RID: 2015
			// (get) Token: 0x060025B4 RID: 9652 RVA: 0x00071738 File Offset: 0x0006F938
			public int iconPriority
			{
				get
				{
					return 100;
				}
			}

			// Token: 0x170007E0 RID: 2016
			// (get) Token: 0x060025B5 RID: 9653 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool removeOnSwapWeapon
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060025B6 RID: 9654 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbilityInstance CreateInstance(Character owner)
			{
				return this;
			}

			// Token: 0x060025B7 RID: 9655 RVA: 0x0007173C File Offset: 0x0006F93C
			public ReviveOnce(string key, Character owner, IntData data, int[] costs, float remainHealthPercent) : base(key, owner, data, costs)
			{
				this._remainhealthPercent = remainHealthPercent;
				this.icon = CommonResource.instance.reassembleIcon;
			}

			// Token: 0x060025B8 RID: 9656 RVA: 0x00071761 File Offset: 0x0006F961
			public override void Attach()
			{
				this._owner.ability.Add(this);
			}

			// Token: 0x060025B9 RID: 9657 RVA: 0x00071775 File Offset: 0x0006F975
			public override void Detach()
			{
				this._owner.ability.Remove(this);
			}

			// Token: 0x060025BA RID: 9658 RVA: 0x00002191 File Offset: 0x00000391
			public void UpdateTime(float deltaTime)
			{
			}

			// Token: 0x060025BB RID: 9659 RVA: 0x00002191 File Offset: 0x00000391
			public void Refresh()
			{
			}

			// Token: 0x060025BC RID: 9660 RVA: 0x0007178C File Offset: 0x0006F98C
			private void Revive()
			{
				if (GameData.Progress.reassembleUsed)
				{
					return;
				}
				if (this._owner.ability.GetInstance<Revive>() != null)
				{
					return;
				}
				Chronometer.global.AttachTimeScale(this, 0.2f, 0.5f);
				this._owner.health.FixedAmountHeal(this._owner.health.maximumHealth * (double)this._remainhealthPercent * (double)base.level, true);
				GameData.Progress.reassembleUsed = true;
				CommonResource.instance.reassembleParticle.Emit(this._owner.transform.position, this._owner.collider.bounds, this._owner.movement.push);
				this._owner.CancelAction();
				this._owner.chronometer.master.AttachTimeScale(this, 0.01f, 0.5f);
				this._owner.spriteEffectStack.Add(new ColorBlend(int.MaxValue, Color.clear, 0.5f));
				GetInvulnerable getInvulnerable = new GetInvulnerable();
				getInvulnerable.duration = (float)(2 + base.level);
				this._owner.spriteEffectStack.Add(new Invulnerable(0, 0.2f, getInvulnerable.duration));
				this._owner.ability.Add(getInvulnerable);
				CircleCaster circleCaster = new CircleCaster();
				circleCaster.origin = this._owner.transform.position;
				circleCaster.radius = 8f;
				circleCaster.contactFilter.SetLayerMask(WitchBonus.ReviveOnce._layer.Evaluate(this._owner.gameObject));
				List<Target> components = circleCaster.Cast().GetComponents(true);
				for (int i = 0; i < components.Count; i++)
				{
					Character character = components[i].character;
					if (!(character == null))
					{
						Damage damage = new Damage(this._owner, 50.0, MMMaths.RandomPointWithinBounds(character.collider.bounds), Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Item, (double)base.level, 0.5f, 0.0, 1.0, 1.0, true, false, 0.0, 0.0, 0, null, 1.0);
						character.movement.push.ApplyKnockback(this._owner, new Vector2(0f, 5f), new Curve(AnimationCurve.Linear(0f, 0f, 1f, 1f), 0.3f, 0.5f), false, false);
						this._owner.TryAttackCharacter(new TargetStruct(character), ref damage);
					}
				}
			}

			// Token: 0x060025BD RID: 9661 RVA: 0x00071A40 File Offset: 0x0006FC40
			void IAbilityInstance.Attach()
			{
				this._owner.health.onDie += this.Revive;
			}

			// Token: 0x060025BE RID: 9662 RVA: 0x00071A5E File Offset: 0x0006FC5E
			void IAbilityInstance.Detach()
			{
				this._owner.health.onDie -= this.Revive;
			}

			// Token: 0x060025BF RID: 9663 RVA: 0x00071A7C File Offset: 0x0006FC7C
			public override string GetDescription(int level)
			{
				return string.Format(Localization.GetLocalizedString(this._key + "/desc"), this._remainhealthPercent * (float)level);
			}

			// Token: 0x04001FE3 RID: 8163
			private readonly float _remainhealthPercent;

			// Token: 0x04001FE4 RID: 8164
			private static readonly TargetLayer _layer = new TargetLayer(0, false, true, false, false);

			// Token: 0x04001FE5 RID: 8165
			private NonAllocOverlapper _overlapper;
		}

		// Token: 0x0200073B RID: 1851
		public class OverHealToShield : WitchBonus.GenericBonus
		{
			// Token: 0x060025C1 RID: 9665 RVA: 0x00071ABC File Offset: 0x0006FCBC
			public OverHealToShield(string key, Character owner, IntData data, int[] costs, float bonusPerLevel) : base(key, owner, data, costs, bonusPerLevel)
			{
				this._shield = new Shield();
			}

			// Token: 0x060025C2 RID: 9666 RVA: 0x00071AD8 File Offset: 0x0006FCD8
			private void OnHealed(double healed, double overHealed)
			{
				double num = overHealed / this._owner.health.maximumHealth;
				this._shield.amount = (float)(num * (double)base.bonus);
				this._owner.ability.Add(this._shield);
			}

			// Token: 0x060025C3 RID: 9667 RVA: 0x00071B24 File Offset: 0x0006FD24
			public override void Attach()
			{
				this._owner.health.onHealed += this.OnHealed;
			}

			// Token: 0x060025C4 RID: 9668 RVA: 0x00071B42 File Offset: 0x0006FD42
			public override void Detach()
			{
				this._owner.health.onHealed -= this.OnHealed;
			}

			// Token: 0x04001FE9 RID: 8169
			private readonly Shield _shield;
		}

		// Token: 0x0200073C RID: 1852
		public class Alchemy : WitchBonus.Bonus
		{
			// Token: 0x060025C5 RID: 9669 RVA: 0x00071B60 File Offset: 0x0006FD60
			public Alchemy(string key, Character owner, IntData data, int[] costs, RarityPrices itemRarityGoldsPerLevel, RarityPrices essenceRarityGoldsPerLevel) : base(key, owner, data, costs)
			{
				this._itemRarityGoldsPerLevel = itemRarityGoldsPerLevel;
				this._eseenceRarityGoldsPerLevel = essenceRarityGoldsPerLevel;
			}

			// Token: 0x060025C6 RID: 9670 RVA: 0x00071B7D File Offset: 0x0006FD7D
			public override string GetDescription(int level)
			{
				return string.Format(Localization.GetLocalizedString(this._key + "/desc"), this._itemRarityGoldsPerLevel[Rarity.Legendary] * level);
			}

			// Token: 0x060025C7 RID: 9671 RVA: 0x00002191 File Offset: 0x00000391
			public override void Attach()
			{
			}

			// Token: 0x060025C8 RID: 9672 RVA: 0x00002191 File Offset: 0x00000391
			public override void Detach()
			{
			}

			// Token: 0x060025C9 RID: 9673 RVA: 0x00071BAC File Offset: 0x0006FDAC
			public int GetGoldByDiscard(Item item)
			{
				return this._itemRarityGoldsPerLevel[item.rarity] * base.level;
			}

			// Token: 0x060025CA RID: 9674 RVA: 0x00071BC6 File Offset: 0x0006FDC6
			public int GetGoldByDiscard(Quintessence essence)
			{
				return this._eseenceRarityGoldsPerLevel[essence.rarity] * base.level;
			}

			// Token: 0x04001FEA RID: 8170
			private readonly RarityPrices _itemRarityGoldsPerLevel;

			// Token: 0x04001FEB RID: 8171
			private readonly RarityPrices _eseenceRarityGoldsPerLevel;
		}

		// Token: 0x0200073D RID: 1853
		public class Tree
		{
			// Token: 0x170007E1 RID: 2017
			// (get) Token: 0x060025CB RID: 9675 RVA: 0x00071BE0 File Offset: 0x0006FDE0
			// (set) Token: 0x060025CC RID: 9676 RVA: 0x00071BE8 File Offset: 0x0006FDE8
			public ReadOnlyCollection<WitchBonus.Bonus> list { get; protected set; }

			// Token: 0x060025CD RID: 9677 RVA: 0x00071BF4 File Offset: 0x0006FDF4
			protected void InitializeTreeIndex()
			{
				for (int i = 0; i < this.list.Count; i++)
				{
					this.list[i].tree = this;
					this.list[i].indexInTree = i;
				}
			}
		}

		// Token: 0x0200073E RID: 1854
		public class Skull : WitchBonus.Tree
		{
			// Token: 0x060025CF RID: 9679 RVA: 0x00071C3C File Offset: 0x0006FE3C
			public Skull(Character owner)
			{
				this.marrowImplant = new WitchBonus.StatBonus("witch/skull/0", owner, GameData.Progress.witch.skull[0], WitchSettings.instance.골수이식_비용, new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, (double)((float)WitchSettings.instance.골수이식_마법공격력p * 0.01f)));
				this.marrowImplant.Initialize();
				this.fastDislocation = new WitchBonus.StatBonus("witch/skull/1", owner, GameData.Progress.witch.skull[1], WitchSettings.instance.신속한탈골_비용, new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, (double)(WitchSettings.instance.신속한탈골_교대대기시간가속 * 0.01f)));
				this.fastDislocation.Initialize();
				this.nutritionSupply = new WitchBonus.StatBonusByWeaponCategory("witch/skull/2", owner, GameData.Progress.witch.skull[2], WitchSettings.instance.영양공급_비용, Weapon.Category.Balance, new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, (double)(WitchSettings.instance.영양공급_스킬쿨다운p * 0.01f)));
				this.nutritionSupply.Initialize();
				this.enhanceExoskeleton = new WitchBonus.GetShieldOnSwap("witch/skull/3", owner, GameData.Progress.witch.skull[3], WitchSettings.instance.외골격강화_비용, WitchSettings.instance.외골격강화_보호막, WitchSettings.instance.외골격강화_보호막지속시간, WitchSettings.instance.외골격강화_교대대기시간감소);
				this.enhanceExoskeleton.Initialize();
				base.list = new ReadOnlyCollection<WitchBonus.Bonus>(new WitchBonus.Bonus[]
				{
					this.marrowImplant,
					this.fastDislocation,
					this.nutritionSupply,
					this.enhanceExoskeleton
				});
				base.InitializeTreeIndex();
			}

			// Token: 0x04001FED RID: 8173
			private const string _key = "witch/skull";

			// Token: 0x04001FEE RID: 8174
			public readonly WitchBonus.StatBonus marrowImplant;

			// Token: 0x04001FEF RID: 8175
			public readonly WitchBonus.StatBonus fastDislocation;

			// Token: 0x04001FF0 RID: 8176
			public readonly WitchBonus.StatBonusByWeaponCategory nutritionSupply;

			// Token: 0x04001FF1 RID: 8177
			public readonly WitchBonus.GetShieldOnSwap enhanceExoskeleton;
		}

		// Token: 0x0200073F RID: 1855
		public class Body : WitchBonus.Tree
		{
			// Token: 0x060025D0 RID: 9680 RVA: 0x00071DE4 File Offset: 0x0006FFE4
			public Body(Character owner)
			{
				this.strongBone = new WitchBonus.StatBonus("witch/body/0", owner, GameData.Progress.witch.body[0], WitchSettings.instance.통뼈_비용, new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, (double)((float)WitchSettings.instance.통뼈_물리공격력p * 0.01f)));
				this.strongBone.Initialize();
				this.fractureImmunity = new WitchBonus.StatBonus("witch/body/1", owner, GameData.Progress.witch.body[1], WitchSettings.instance.골절상면역_비용, new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, (double)WitchSettings.instance.골절상면역_체력증가));
				this.fractureImmunity.Initialize();
				this.heavyFrame = new WitchBonus.StatBonusByWeaponCategory("witch/body/2", owner, GameData.Progress.witch.body[2], WitchSettings.instance.육중한뼈대_비용, Weapon.Category.Power, new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, (double)(WitchSettings.instance.육중한뼈대_받는피해 * 0.01f)));
				this.heavyFrame.Initialize();
				this.reassemble = new WitchBonus.ReviveOnce("witch/body/3", owner, GameData.Progress.witch.body[3], WitchSettings.instance.재조립_비용, (float)WitchSettings.instance.재조립_체력회복p * 0.01f);
				this.reassemble.Initialize();
				base.list = new ReadOnlyCollection<WitchBonus.Bonus>(new WitchBonus.Bonus[]
				{
					this.strongBone,
					this.fractureImmunity,
					this.heavyFrame,
					this.reassemble
				});
				base.InitializeTreeIndex();
			}

			// Token: 0x04001FF2 RID: 8178
			private const string _key = "witch/body";

			// Token: 0x04001FF3 RID: 8179
			public readonly WitchBonus.StatBonus strongBone;

			// Token: 0x04001FF4 RID: 8180
			public readonly WitchBonus.StatBonus fractureImmunity;

			// Token: 0x04001FF5 RID: 8181
			public readonly WitchBonus.StatBonusByWeaponCategory heavyFrame;

			// Token: 0x04001FF6 RID: 8182
			public readonly WitchBonus.ReviveOnce reassemble;
		}

		// Token: 0x02000740 RID: 1856
		public class Soul : WitchBonus.Tree
		{
			// Token: 0x060025D1 RID: 9681 RVA: 0x00071F78 File Offset: 0x00070178
			public Soul(Character owner)
			{
				this.soulAcceleration = new WitchBonus.StatBonus("witch/soul/0", owner, GameData.Progress.witch.soul[0], WitchSettings.instance.영혼가속_비용, new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, (double)(WitchSettings.instance.영혼가속_치명타확률p * 0.01f)));
				this.soulAcceleration.Initialize();
				this.willOfAncestor = new WitchBonus.StatBonus("witch/soul/1", owner, GameData.Progress.witch.soul[1], WitchSettings.instance.선조의의지_비용, new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.EssenceCooldownSpeed, (double)((float)WitchSettings.instance.선조의의지_정수쿨다운가속p * 0.01f)));
				this.willOfAncestor.Initialize();
				this.fatalMind = new WitchBonus.StatBonusByWeaponCategory("witch/soul/2", owner, GameData.Progress.witch.soul[2], WitchSettings.instance.날카로운정신_비용, Weapon.Category.Speed, new Stat.Values(new Stat.Value[]
				{
					new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, (double)((float)WitchSettings.instance.날카로운정신_공격속도p * 0.01f)),
					new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, (double)((float)WitchSettings.instance.날카로운정신_공격속도p * 0.01f)),
					new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, (double)((float)WitchSettings.instance.날카로운정신_이동속도p * 0.01f))
				}));
				this.fatalMind.Initialize();
				this.ancientAlchemy = new WitchBonus.Alchemy("witch/soul/3", owner, GameData.Progress.witch.soul[3], WitchSettings.instance.고대연금술_비용, new RarityPrices(new int[]
				{
					WitchSettings.instance.고대연금술_골드량_커먼,
					WitchSettings.instance.고대연금술_골드량_레어,
					WitchSettings.instance.고대연금술_골드량_유니크,
					WitchSettings.instance.고대연금술_골드량_레전더리
				}), new RarityPrices(new int[]
				{
					WitchSettings.instance.고대연금술_골드량_정수_커먼,
					WitchSettings.instance.고대연금술_골드량_정수_레어,
					WitchSettings.instance.고대연금술_골드량_정수_유니크,
					WitchSettings.instance.고대연금술_골드량_정수_레전더리
				}));
				this.ancientAlchemy.Initialize();
				base.list = new ReadOnlyCollection<WitchBonus.Bonus>(new WitchBonus.Bonus[]
				{
					this.soulAcceleration,
					this.willOfAncestor,
					this.fatalMind,
					this.ancientAlchemy
				});
				base.InitializeTreeIndex();
			}

			// Token: 0x04001FF7 RID: 8183
			private const string _key = "witch/soul";

			// Token: 0x04001FF8 RID: 8184
			public readonly WitchBonus.StatBonus soulAcceleration;

			// Token: 0x04001FF9 RID: 8185
			public readonly WitchBonus.StatBonus willOfAncestor;

			// Token: 0x04001FFA RID: 8186
			public readonly WitchBonus.StatBonusByWeaponCategory fatalMind;

			// Token: 0x04001FFB RID: 8187
			public readonly WitchBonus.Alchemy ancientAlchemy;
		}
	}
}
