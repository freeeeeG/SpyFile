using System;
using Characters.Abilities;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x0200084D RID: 2125
	public sealed class RebornRecovery : UpgradeAbility
	{
		// Token: 0x06002C3A RID: 11322 RVA: 0x00087731 File Offset: 0x00085931
		public override void Attach(Character target)
		{
			this._owner = target;
			this._attached = true;
			this._revive.Initialize();
			this._owner.ability.Add(this._revive.ability);
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x00087768 File Offset: 0x00085968
		private void Update()
		{
			if (!this._attached)
			{
				return;
			}
			if (this._owner.ability.Contains(this._revive.ability))
			{
				return;
			}
			this.DestroyAbility();
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x00087797 File Offset: 0x00085997
		public override void Detach()
		{
			if (Service.quitting)
			{
				return;
			}
			this._owner.ability.Remove(this._revive.ability);
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x000877C0 File Offset: 0x000859C0
		private void DestroyAbility()
		{
			this._owner.ability.Remove(this._revive.ability);
			UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
			int num = upgrade.IndexOf(this._upgrade);
			if (num == -1)
			{
				return;
			}
			upgrade.Remove(num);
		}

		// Token: 0x04002563 RID: 9571
		[SerializeField]
		private UpgradeObject _upgrade;

		// Token: 0x04002564 RID: 9572
		[SerializeField]
		private ReviveComponent _revive;

		// Token: 0x04002565 RID: 9573
		private Character _owner;

		// Token: 0x04002566 RID: 9574
		private bool _attached;
	}
}
