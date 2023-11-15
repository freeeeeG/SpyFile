using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Services;
using Singletons;

namespace Characters.Gear.Upgrades
{
	// Token: 0x02000855 RID: 2133
	public sealed class UpgradeManager : Singleton<UpgradeManager>
	{
		// Token: 0x06002C57 RID: 11351 RVA: 0x00087A13 File Offset: 0x00085C13
		protected override void Awake()
		{
			base.Awake();
			UpgradeResource.instance.Initialize();
			this.Initialize();
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x00087A2C File Offset: 0x00085C2C
		private void Initialize()
		{
			UpgradeResource instance = UpgradeResource.instance;
			this._upgrades = instance.upgradeReferences;
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x00087A4B File Offset: 0x00085C4B
		public List<UpgradeResource.Reference> GetAllList()
		{
			return this._upgrades;
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x00087A53 File Offset: 0x00085C53
		public List<UpgradeResource.Reference> GetAllUnlockedList()
		{
			return (from info in this._upgrades
			where !info.needUnlock || GameData.Gear.IsUnlocked(Gear.Type.Upgrade.ToString(), info.name)
			select info).ToList<UpgradeResource.Reference>();
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x00087A84 File Offset: 0x00085C84
		public List<UpgradeResource.Reference> GetList(UpgradeObject.Type type)
		{
			return (from @object in this._upgrades
			where @object.type == type && !Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(@object)
			select @object).ToList<UpgradeResource.Reference>();
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x00087ABC File Offset: 0x00085CBC
		public List<UpgradeResource.Reference> GetUnlockList(UpgradeObject.Type type)
		{
			return (from @object in this._upgrades
			where @object.type == type && (!@object.needUnlock || GameData.Gear.IsUnlocked(Gear.Type.Upgrade.ToString(), @object.name)) && !Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(@object)
			select @object).ToList<UpgradeResource.Reference>();
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x00087AF4 File Offset: 0x00085CF4
		public UpgradeResource.Reference FindByName(string name)
		{
			foreach (UpgradeResource.Reference reference in this._upgrades)
			{
				if (reference.name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return reference;
				}
			}
			return null;
		}

		// Token: 0x0400256E RID: 9582
		private List<UpgradeResource.Reference> _upgrades;
	}
}
