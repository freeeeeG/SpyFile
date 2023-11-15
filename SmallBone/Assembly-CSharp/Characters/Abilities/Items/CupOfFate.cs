using System;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CB2 RID: 3250
	[Serializable]
	public sealed class CupOfFate : Ability
	{
		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06004201 RID: 16897 RVA: 0x000C03D9 File Offset: 0x000BE5D9
		// (set) Token: 0x06004202 RID: 16898 RVA: 0x000C03E1 File Offset: 0x000BE5E1
		public int stack
		{
			get
			{
				return this._stack;
			}
			set
			{
				this._stack = math.min(value, this._maxStack);
				if (this._instance == null)
				{
					return;
				}
				this._instance.UpdateStack();
			}
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x000C0409 File Offset: 0x000BE609
		public void Load(Character owner, int stack)
		{
			owner.ability.Add(this);
			this.stack = stack;
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x000C041F File Offset: 0x000BE61F
		public override void Initialize()
		{
			base.Initialize();
			if (this._maxStack == 0)
			{
				this._maxStack = int.MaxValue;
			}
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x000C043A File Offset: 0x000BE63A
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this._instance = new CupOfFate.Instance(owner, this);
			return this._instance;
		}

		// Token: 0x04003298 RID: 12952
		[SerializeField]
		private Inscription.Key[] _keys;

		// Token: 0x04003299 RID: 12953
		[Tooltip("다시 획득할 경우 스택을 초기화할지")]
		[SerializeField]
		private bool _resetOnAttach = true;

		// Token: 0x0400329A RID: 12954
		[SerializeField]
		private int _maxStack;

		// Token: 0x0400329B RID: 12955
		[SerializeField]
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		private float _iconStacksPerStack = 1f;

		// Token: 0x0400329C RID: 12956
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x0400329D RID: 12957
		private CupOfFate.Instance _instance;

		// Token: 0x0400329E RID: 12958
		private int _stack;

		// Token: 0x02000CB3 RID: 3251
		public class Instance : AbilityInstance<CupOfFate>
		{
			// Token: 0x17000DD3 RID: 3539
			// (get) Token: 0x06004207 RID: 16903 RVA: 0x000C0469 File Offset: 0x000BE669
			public override int iconStacks
			{
				get
				{
					return (int)((float)this.ability.stack * this.ability._iconStacksPerStack);
				}
			}

			// Token: 0x06004208 RID: 16904 RVA: 0x000C0484 File Offset: 0x000BE684
			public Instance(Character owner, CupOfFate ability) : base(owner, ability)
			{
			}

			// Token: 0x06004209 RID: 16905 RVA: 0x000C0490 File Offset: 0x000BE690
			protected override void OnAttach()
			{
				Singleton<Service>.Instance.gearManager.onItemInstanceChanged += this.HandleItemInstanceChanged;
				this.HandleItemInstanceChanged();
				this._stat = this.ability._statPerStack.Clone();
				this.owner.stat.AttachValues(this._stat);
				if (this.ability._resetOnAttach)
				{
					this.ability.stack = 1;
					return;
				}
				this.UpdateStack();
			}

			// Token: 0x0600420A RID: 16906 RVA: 0x000C050C File Offset: 0x000BE70C
			protected override void OnDetach()
			{
				Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.HandleItemInstanceChanged;
				foreach (Inscription.Key key in this.ability._keys)
				{
					foreach (Item item in Singleton<Service>.Instance.gearManager.GetItemInstanceByKeyword(key))
					{
						item.onDiscard -= this.TryStackUp;
					}
				}
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x0600420B RID: 16907 RVA: 0x000C05BC File Offset: 0x000BE7BC
			private void HandleItemInstanceChanged()
			{
				foreach (Inscription.Key key in this.ability._keys)
				{
					foreach (Item item in Singleton<Service>.Instance.gearManager.GetItemInstanceByKeyword(key))
					{
						item.onDiscard -= this.TryStackUp;
						item.onDiscard += this.TryStackUp;
					}
				}
			}

			// Token: 0x0600420C RID: 16908 RVA: 0x000C064C File Offset: 0x000BE84C
			public void TryStackUp(Gear gear)
			{
				if (gear.destructible)
				{
					CupOfFate ability = this.ability;
					int stack = ability.stack;
					ability.stack = stack + 1;
					this.UpdateStack();
				}
			}

			// Token: 0x0600420D RID: 16909 RVA: 0x000C067C File Offset: 0x000BE87C
			public void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this.ability.stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x0400329F RID: 12959
			private Stat.Values _stat;
		}
	}
}
