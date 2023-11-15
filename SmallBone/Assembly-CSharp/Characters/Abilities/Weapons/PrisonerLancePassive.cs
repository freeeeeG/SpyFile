using System;
using UnityEngine;

namespace Characters.Abilities.Weapons
{
	// Token: 0x02000BEC RID: 3052
	[Serializable]
	public class PrisonerLancePassive : Ability
	{
		// Token: 0x06003EA8 RID: 16040 RVA: 0x000B63CC File Offset: 0x000B45CC
		public override IAbilityInstance CreateInstance(Character owner)
		{
			if (this._instance == null)
			{
				this._instance = new PrisonerLancePassive.Instance(owner, this);
			}
			return this._instance;
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x000B63E9 File Offset: 0x000B45E9
		public void StartDetect()
		{
			this._instance.StartDetect();
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x000B63F6 File Offset: 0x000B45F6
		public void StopDetect()
		{
			this._instance.StopDetect();
		}

		// Token: 0x04003057 RID: 12375
		[SerializeField]
		private string _attackKey;

		// Token: 0x04003058 RID: 12376
		[SerializeField]
		private int _killCount;

		// Token: 0x04003059 RID: 12377
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter;

		// Token: 0x0400305A RID: 12378
		private PrisonerLancePassive.Instance _instance;

		// Token: 0x02000BED RID: 3053
		public class Instance : AbilityInstance<PrisonerLancePassive>
		{
			// Token: 0x17000D3F RID: 3391
			// (get) Token: 0x06003EAC RID: 16044 RVA: 0x000B6403 File Offset: 0x000B4603
			public override Sprite icon
			{
				get
				{
					if (this._currentKillCount < this.ability._killCount)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x06003EAD RID: 16045 RVA: 0x000B6420 File Offset: 0x000B4620
			public Instance(Character owner, PrisonerLancePassive ability) : base(owner, ability)
			{
			}

			// Token: 0x06003EAE RID: 16046 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x06003EAF RID: 16047 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}

			// Token: 0x06003EB0 RID: 16048 RVA: 0x000B642C File Offset: 0x000B462C
			private void OnKilled(ITarget target, ref Damage damage)
			{
				if (target.character == null)
				{
					return;
				}
				if (!this.ability._characterTypeFilter[target.character.type])
				{
					return;
				}
				if (!damage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				this._currentKillCount++;
			}

			// Token: 0x06003EB1 RID: 16049 RVA: 0x000B648E File Offset: 0x000B468E
			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				if (!damage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				damage.criticalChance = 1.0;
				return false;
			}

			// Token: 0x06003EB2 RID: 16050 RVA: 0x000B64BC File Offset: 0x000B46BC
			public void StartDetect()
			{
				if (this._currentKillCount >= this.ability._killCount)
				{
					this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnGiveDamage));
				}
				this._currentKillCount = 0;
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.OnKilled));
			}

			// Token: 0x06003EB3 RID: 16051 RVA: 0x000B6528 File Offset: 0x000B4728
			public void StopDetect()
			{
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.OnKilled));
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x0400305B RID: 12379
			private int _currentKillCount;
		}
	}
}
