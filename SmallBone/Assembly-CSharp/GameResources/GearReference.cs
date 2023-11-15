using System;
using Characters.Gear;
using Data;
using Scenes;
using UnityEngine;

namespace GameResources
{
	// Token: 0x0200017D RID: 381
	public abstract class GearReference
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000814 RID: 2068
		public abstract Gear.Type type { get; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x00017F00 File Offset: 0x00016100
		public bool unlocked
		{
			get
			{
				return !this.needUnlock || GameData.Gear.IsUnlocked(this.type.ToString(), this.name);
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00017F38 File Offset: 0x00016138
		public void Unlock()
		{
			if (GameData.Gear.IsUnlocked(this.type.ToString(), this.name))
			{
				return;
			}
			GameData.Gear.SetUnlocked(this.type.ToString(), this.name, true);
			Scene<GameBase>.instance.uiManager.unlockNotice.Show(this.unlockIcon, Localization.GetLocalizedString(this.displayNameKey));
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00017FAC File Offset: 0x000161AC
		public GearRequest LoadAsync()
		{
			return new GearRequest(this.path);
		}

		// Token: 0x04000658 RID: 1624
		public string name;

		// Token: 0x04000659 RID: 1625
		public string guid;

		// Token: 0x0400065A RID: 1626
		public string path;

		// Token: 0x0400065B RID: 1627
		public bool obtainable;

		// Token: 0x0400065C RID: 1628
		public bool needUnlock;

		// Token: 0x0400065D RID: 1629
		public Sprite unlockIcon;

		// Token: 0x0400065E RID: 1630
		public Rarity rarity;

		// Token: 0x0400065F RID: 1631
		public Sprite icon;

		// Token: 0x04000660 RID: 1632
		public Sprite thumbnail;

		// Token: 0x04000661 RID: 1633
		public string displayNameKey;

		// Token: 0x04000662 RID: 1634
		public Gear.Tag gearTag;
	}
}
