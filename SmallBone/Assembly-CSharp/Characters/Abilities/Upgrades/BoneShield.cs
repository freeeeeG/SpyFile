using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AD0 RID: 2768
	[Serializable]
	public sealed class BoneShield : Ability
	{
		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x060038D0 RID: 14544 RVA: 0x000A76B9 File Offset: 0x000A58B9
		// (set) Token: 0x060038D1 RID: 14545 RVA: 0x000A76C7 File Offset: 0x000A58C7
		public int stack
		{
			get
			{
				return (int)this._stackableComponent.stack;
			}
			set
			{
				this._stackableComponent.stack = (float)value;
			}
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x000A76D6 File Offset: 0x000A58D6
		public override void Initialize()
		{
			base.Initialize();
			this._evasion.Initialize();
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x000A76E9 File Offset: 0x000A58E9
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new BoneShield.Instance(owner, this);
		}

		// Token: 0x04002D2B RID: 11563
		[SerializeField]
		private int _maxStack;

		// Token: 0x04002D2C RID: 11564
		[SerializeField]
		private int _stackPerKilledDarkEnemy;

		// Token: 0x04002D2D RID: 11565
		[SerializeField]
		private StackableComponent _stackableComponent;

		// Token: 0x04002D2E RID: 11566
		[SerializeField]
		private GetEvasion _evasion;

		// Token: 0x04002D2F RID: 11567
		[SerializeField]
		private SoundInfo _attachAudioClipInfo;

		// Token: 0x02000AD1 RID: 2769
		public sealed class Instance : AbilityInstance<BoneShield>
		{
			// Token: 0x17000BD1 RID: 3025
			// (get) Token: 0x060038D5 RID: 14549 RVA: 0x000A76F2 File Offset: 0x000A58F2
			public override Sprite icon
			{
				get
				{
					if (this.ability.stack <= 0)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000BD2 RID: 3026
			// (get) Token: 0x060038D6 RID: 14550 RVA: 0x000A770A File Offset: 0x000A590A
			public override int iconStacks
			{
				get
				{
					return this.ability.stack;
				}
			}

			// Token: 0x060038D7 RID: 14551 RVA: 0x000A7717 File Offset: 0x000A5917
			public Instance(Character owner, BoneShield ability) : base(owner, ability)
			{
			}

			// Token: 0x060038D8 RID: 14552 RVA: 0x000A7724 File Offset: 0x000A5924
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(100, new TakeDamageDelegate(this.HandleOnTakeDamage));
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
			}

			// Token: 0x060038D9 RID: 14553 RVA: 0x000A777C File Offset: 0x000A597C
			private void HandleOnKilled(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (character.type == Character.Type.Named)
				{
					this.ability.stack = Mathf.Min(this.ability._maxStack, this.ability.stack + this.ability._stackPerKilledDarkEnemy);
				}
			}

			// Token: 0x060038DA RID: 14554 RVA: 0x000A77D8 File Offset: 0x000A59D8
			private bool HandleOnTakeDamage(ref Damage damage)
			{
				if (this.owner.invulnerable.value || this.owner.evasion.value)
				{
					return false;
				}
				if (damage.attackType.Equals(Damage.AttackType.None))
				{
					return false;
				}
				if (damage.@null)
				{
					return false;
				}
				if (this.ability.stack <= 0)
				{
					return false;
				}
				this.owner.ability.Add(this.ability._evasion);
				BoneShield ability = this.ability;
				int stack = ability.stack;
				ability.stack = stack - 1;
				damage.@null = true;
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attachAudioClipInfo, this.owner.transform.position);
				return false;
			}

			// Token: 0x060038DB RID: 14555 RVA: 0x000A78A0 File Offset: 0x000A5AA0
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.HandleOnTakeDamage));
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
			}
		}
	}
}
