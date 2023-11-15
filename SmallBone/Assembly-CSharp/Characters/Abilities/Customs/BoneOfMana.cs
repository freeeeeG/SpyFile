using System;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Operations;
using Characters.Player;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D32 RID: 3378
	[Serializable]
	public class BoneOfMana : Ability
	{
		// Token: 0x0600442A RID: 17450 RVA: 0x000C60F4 File Offset: 0x000C42F4
		public override void Initialize()
		{
			base.Initialize();
			foreach (CharacterOperation[] array2 in this.operationsByCount)
			{
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].Initialize();
				}
			}
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x000C6136 File Offset: 0x000C4336
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new BoneOfMana.Instance(owner, this);
		}

		// Token: 0x040033F7 RID: 13303
		[NonSerialized]
		public CharacterOperation[][] operationsByCount;

		// Token: 0x040033F8 RID: 13304
		[SerializeField]
		private ActionTypeBoolArray _actionTypeFilter;

		// Token: 0x02000D33 RID: 3379
		public class Instance : AbilityInstance<BoneOfMana>
		{
			// Token: 0x17000E27 RID: 3623
			// (get) Token: 0x0600442D RID: 17453 RVA: 0x000C613F File Offset: 0x000C433F
			public override int iconStacks
			{
				get
				{
					return this._balanceHeadCount;
				}
			}

			// Token: 0x0600442E RID: 17454 RVA: 0x000C6147 File Offset: 0x000C4347
			public Instance(Character owner, BoneOfMana ability) : base(owner, ability)
			{
				this._weaponInventory = owner.playerComponents.inventory.weapon;
				this.UpdateBalanaceHeadCount();
			}

			// Token: 0x0600442F RID: 17455 RVA: 0x000C616D File Offset: 0x000C436D
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.OnOwnerStartAction;
				this._weaponInventory.onChanged += this.OnWeaponChanged;
			}

			// Token: 0x06004430 RID: 17456 RVA: 0x000C619D File Offset: 0x000C439D
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.OnOwnerStartAction;
				this._weaponInventory.onChanged -= this.OnWeaponChanged;
			}

			// Token: 0x06004431 RID: 17457 RVA: 0x000C61D0 File Offset: 0x000C43D0
			private void OnOwnerStartAction(Characters.Actions.Action action)
			{
				if (!this.ability._actionTypeFilter.GetOrDefault(action.type))
				{
					return;
				}
				if (action.cooldown.usedByStreak)
				{
					return;
				}
				CharacterOperation[] array = this.ability.operationsByCount[this._balanceHeadCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Run(this.owner);
				}
			}

			// Token: 0x06004432 RID: 17458 RVA: 0x000C6233 File Offset: 0x000C4433
			private void OnWeaponChanged(Weapon old, Weapon @new)
			{
				this.UpdateBalanaceHeadCount();
			}

			// Token: 0x06004433 RID: 17459 RVA: 0x000C623B File Offset: 0x000C443B
			private void UpdateBalanaceHeadCount()
			{
				this._balanceHeadCount = this._weaponInventory.GetCountByCategory(Weapon.Category.Balance);
			}

			// Token: 0x040033F9 RID: 13305
			private WeaponInventory _weaponInventory;

			// Token: 0x040033FA RID: 13306
			private int _balanceHeadCount;
		}
	}
}
