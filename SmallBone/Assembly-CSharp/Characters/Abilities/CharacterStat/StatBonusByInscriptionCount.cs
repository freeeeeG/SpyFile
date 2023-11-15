using System;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C50 RID: 3152
	[Serializable]
	public class StatBonusByInscriptionCount : Ability
	{
		// Token: 0x0600408D RID: 16525 RVA: 0x000BB92A File Offset: 0x000B9B2A
		public override void Initialize()
		{
			base.Initialize();
			if (this._maxStack == 0)
			{
				this._maxStack = int.MaxValue;
			}
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x000BB945 File Offset: 0x000B9B45
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByInscriptionCount.Instance(owner, this);
		}

		// Token: 0x0400319B RID: 12699
		[SerializeField]
		private Inscription.Key[] _keys;

		// Token: 0x0400319C RID: 12700
		[SerializeField]
		private int _maxStack;

		// Token: 0x0400319D RID: 12701
		[SerializeField]
		[Tooltip("스택이 쌓일 때마다 남은 시간을 초기화할지")]
		private bool _refreshRemainTime = true;

		// Token: 0x0400319E RID: 12702
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		[SerializeField]
		private float _iconStacksPerStack = 1f;

		// Token: 0x0400319F RID: 12703
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x02000C51 RID: 3153
		public class Instance : AbilityInstance<StatBonusByInscriptionCount>
		{
			// Token: 0x17000D9C RID: 3484
			// (get) Token: 0x06004090 RID: 16528 RVA: 0x000BB968 File Offset: 0x000B9B68
			public override int iconStacks
			{
				get
				{
					return (int)((float)this._stack * this.ability._iconStacksPerStack);
				}
			}

			// Token: 0x06004091 RID: 16529 RVA: 0x000BB97E File Offset: 0x000B9B7E
			public Instance(Character owner, StatBonusByInscriptionCount ability) : base(owner, ability)
			{
			}

			// Token: 0x06004092 RID: 16530 RVA: 0x000BB988 File Offset: 0x000B9B88
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.onUpdatedKeywordCounts += this.UpdateStack;
				this.owner.stat.AttachValues(this._stat);
				this.UpdateStack();
			}

			// Token: 0x06004093 RID: 16531 RVA: 0x000BB9F4 File Offset: 0x000B9BF4
			protected override void OnDetach()
			{
				if (Service.quitting)
				{
					return;
				}
				if (Singleton<Service>.Instance.levelManager == null || Singleton<Service>.Instance.levelManager.player == null || Singleton<Service>.Instance.levelManager.player.playerComponents.inventory == null)
				{
					return;
				}
				Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.onUpdatedKeywordCounts -= this.UpdateStack;
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004094 RID: 16532 RVA: 0x000BBA8E File Offset: 0x000B9C8E
			public override void Refresh()
			{
				if (this.ability._refreshRemainTime)
				{
					base.Refresh();
				}
			}

			// Token: 0x06004095 RID: 16533 RVA: 0x000BBAA4 File Offset: 0x000B9CA4
			public void UpdateStack()
			{
				Synergy synergy = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy;
				int num = 0;
				foreach (Inscription.Key key in this.ability._keys)
				{
					num += synergy.inscriptions[key].count;
				}
				this._stack = num;
				if (this._stack >= this.ability._maxStack)
				{
					return;
				}
				for (int j = 0; j < this._stat.values.Length; j++)
				{
					this._stat.values[j].value = this.ability._statPerStack.values[j].GetStackedValue((double)this._stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040031A0 RID: 12704
			private int _stack;

			// Token: 0x040031A1 RID: 12705
			private Stat.Values _stat;
		}
	}
}
