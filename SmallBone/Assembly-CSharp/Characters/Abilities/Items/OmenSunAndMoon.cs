using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CF0 RID: 3312
	[Serializable]
	public sealed class OmenSunAndMoon : Ability
	{
		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x060042EF RID: 17135 RVA: 0x000C32C6 File Offset: 0x000C14C6
		// (set) Token: 0x060042F0 RID: 17136 RVA: 0x000C32CE File Offset: 0x000C14CE
		public int stack { get; set; }

		// Token: 0x060042F1 RID: 17137 RVA: 0x000C32D7 File Offset: 0x000C14D7
		public override void Initialize()
		{
			base.Initialize();
			this._abilityComponent.Initialize();
			this._abilityOnDied.Initialize();
			this._onRechargeRevive.Initialize();
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x000C3300 File Offset: 0x000C1500
		public void LoadStack()
		{
			if (this._instance == null)
			{
				return;
			}
			this._instance.LoadStack();
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x000C3316 File Offset: 0x000C1516
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this._instance = new OmenSunAndMoon.Instance(owner, this);
			return this._instance;
		}

		// Token: 0x04003344 RID: 13124
		[SerializeField]
		private int _killCount;

		// Token: 0x04003345 RID: 13125
		[SerializeField]
		private CharacterTypeBoolArray _killTargetFilter;

		// Token: 0x04003346 RID: 13126
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x04003347 RID: 13127
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityOnDied;

		// Token: 0x04003348 RID: 13128
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onRechargeRevive;

		// Token: 0x0400334A RID: 13130
		private OmenSunAndMoon.Instance _instance;

		// Token: 0x02000CF1 RID: 3313
		public sealed class Instance : AbilityInstance<OmenSunAndMoon>
		{
			// Token: 0x17000DEB RID: 3563
			// (get) Token: 0x060042F5 RID: 17141 RVA: 0x000C332B File Offset: 0x000C152B
			public override Sprite icon
			{
				get
				{
					return base.icon;
				}
			}

			// Token: 0x17000DEC RID: 3564
			// (get) Token: 0x060042F6 RID: 17142 RVA: 0x000C3333 File Offset: 0x000C1533
			public override int iconStacks
			{
				get
				{
					if (!this._detached)
					{
						return 0;
					}
					return this.ability._killCount - this.ability.stack + 1;
				}
			}

			// Token: 0x17000DED RID: 3565
			// (get) Token: 0x060042F7 RID: 17143 RVA: 0x000C3358 File Offset: 0x000C1558
			public override float iconFillAmount
			{
				get
				{
					return (float)(this._detached ? 1 : 0);
				}
			}

			// Token: 0x060042F8 RID: 17144 RVA: 0x000C3367 File Offset: 0x000C1567
			public Instance(Character owner, OmenSunAndMoon ability) : base(owner, ability)
			{
			}

			// Token: 0x060042F9 RID: 17145 RVA: 0x000C3374 File Offset: 0x000C1574
			protected override void OnAttach()
			{
				if (this.ability.stack == 0 || this.ability.stack > this.ability._killCount)
				{
					this.owner.ability.Add(this.ability._abilityComponent.ability);
					return;
				}
				this._detached = true;
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
				this.owner.ability.Add(this.ability._abilityOnDied.ability);
			}

			// Token: 0x060042FA RID: 17146 RVA: 0x000C3418 File Offset: 0x000C1618
			protected override void OnDetach()
			{
				this.owner.ability.Remove(this.ability._abilityOnDied.ability);
				this.owner.ability.Remove(this.ability._abilityComponent.ability);
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
			}

			// Token: 0x060042FB RID: 17147 RVA: 0x000C3490 File Offset: 0x000C1690
			public override void UpdateTime(float deltaTime)
			{
				if (this.owner.ability.Contains(this.ability._abilityComponent.ability))
				{
					return;
				}
				if (!this._detached)
				{
					this._detached = true;
					this.ability.stack = 1;
					Character owner = this.owner;
					owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
					this.owner.ability.Add(this.ability._abilityOnDied.ability);
				}
			}

			// Token: 0x060042FC RID: 17148 RVA: 0x000C3524 File Offset: 0x000C1724
			private void HandleOnKilled(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (!this.ability._killTargetFilter[character.type])
				{
					return;
				}
				OmenSunAndMoon ability = this.ability;
				int stack = ability.stack;
				ability.stack = stack + 1;
				if (this.ability.stack > this.ability._killCount)
				{
					this._detached = false;
					Character owner = this.owner;
					owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
					this.owner.ability.Add(this.ability._abilityComponent.ability);
					this.ability._onRechargeRevive.Run(this.owner);
				}
			}

			// Token: 0x060042FD RID: 17149 RVA: 0x000C35F0 File Offset: 0x000C17F0
			internal void LoadStack()
			{
				if (this.ability.stack != 0 && this.ability.stack <= this.ability._killCount)
				{
					this._detached = true;
					this.owner.ability.Remove(this.ability._abilityComponent.ability);
					Character owner = this.owner;
					owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
					this.owner.ability.Add(this.ability._abilityOnDied.ability);
				}
			}

			// Token: 0x0400334B RID: 13131
			private bool _detached;
		}
	}
}
