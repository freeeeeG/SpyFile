using System;
using Characters.Gear;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D69 RID: 3433
	[Serializable]
	public class MedalOfCarleon : Ability
	{
		// Token: 0x06004535 RID: 17717 RVA: 0x000C8E6D File Offset: 0x000C706D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new MedalOfCarleon.Instance(owner, this);
		}

		// Token: 0x040034AC RID: 13484
		[SerializeField]
		private int _goldPerItem;

		// Token: 0x02000D6A RID: 3434
		public class Instance : AbilityInstance<MedalOfCarleon>
		{
			// Token: 0x06004536 RID: 17718 RVA: 0x000C8E76 File Offset: 0x000C7076
			public Instance(Character owner, MedalOfCarleon ability) : base(owner, ability)
			{
				this._levelManager = Singleton<Service>.Instance.levelManager;
			}

			// Token: 0x06004537 RID: 17719 RVA: 0x000C8E90 File Offset: 0x000C7090
			protected override void OnAttach()
			{
				this._levelManager.onMapLoadedAndFadedIn += this.DrpoGold;
			}

			// Token: 0x06004538 RID: 17720 RVA: 0x000C8EA9 File Offset: 0x000C70A9
			protected override void OnDetach()
			{
				this._levelManager.onMapLoadedAndFadedIn -= this.DrpoGold;
			}

			// Token: 0x06004539 RID: 17721 RVA: 0x000C8EC4 File Offset: 0x000C70C4
			private void DrpoGold()
			{
				int itemCountByTag = this.owner.playerComponents.inventory.item.GetItemCountByTag(Gear.Tag.Carleon);
				if (itemCountByTag == 0)
				{
					return;
				}
				int num = this.ability._goldPerItem * itemCountByTag;
				if (num > 0)
				{
					this._levelManager.DropGold(num, itemCountByTag * 4, this._levelManager.player.transform.position);
				}
			}

			// Token: 0x040034AD RID: 13485
			private LevelManager _levelManager;
		}
	}
}
