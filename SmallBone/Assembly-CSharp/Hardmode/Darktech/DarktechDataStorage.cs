using System;
using System.Collections.Generic;
using Data;

namespace Hardmode.Darktech
{
	// Token: 0x02000157 RID: 343
	public sealed class DarktechDataStorage
	{
		// Token: 0x060006C1 RID: 1729 RVA: 0x00013800 File Offset: 0x00011A00
		public DarktechDataStorage()
		{
			Array values = Enum.GetValues(typeof(DarktechData.Type));
			this._types = new List<DarktechData.Type>(values.Length);
			foreach (object obj in values)
			{
				DarktechData.Type item = (DarktechData.Type)obj;
				this._types.Add(item);
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00013880 File Offset: 0x00011A80
		public void LockAll()
		{
			Array values = Enum.GetValues(typeof(DarktechData.Type));
			foreach (object obj in values)
			{
				DarktechData.Type key = (DarktechData.Type)obj;
				GameData.HardmodeProgress.unlocked.SetData(key, false);
			}
			foreach (object obj2 in values)
			{
				DarktechData.Type key2 = (DarktechData.Type)obj2;
				GameData.HardmodeProgress.activated.SetData(key2, false);
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00013934 File Offset: 0x00011B34
		public void Lock(DarktechData.Type type)
		{
			GameData.HardmodeProgress.unlocked.SetData(type, false);
			GameData.HardmodeProgress.activated.SetData(type, false);
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00013950 File Offset: 0x00011B50
		public void UnlockAll()
		{
			foreach (DarktechData.Type type in this._types)
			{
				this.Unlock(type);
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x000139A4 File Offset: 0x00011BA4
		public void ActivateAll()
		{
			foreach (DarktechData.Type type in this._types)
			{
				this.Activate(type);
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000139F8 File Offset: 0x00011BF8
		public void Unlock(DarktechData.Type type)
		{
			if (GameData.HardmodeProgress.unlocked.GetData(type))
			{
				return;
			}
			GameData.HardmodeProgress.unlocked.SetData(type, true);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00013A14 File Offset: 0x00011C14
		public void Activate(DarktechData.Type type)
		{
			if (GameData.HardmodeProgress.activated.GetData(type))
			{
				return;
			}
			GameData.HardmodeProgress.activated.SetData(type, true);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00013A30 File Offset: 0x00011C30
		public bool IsActivated(DarktechData.Type type)
		{
			return GameData.HardmodeProgress.activated.GetData(type);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00013A3D File Offset: 0x00011C3D
		public bool IsUnlocked(DarktechData.Type type)
		{
			return GameData.HardmodeProgress.unlocked.GetData(type);
		}

		// Token: 0x04000501 RID: 1281
		private List<DarktechData.Type> _types;
	}
}
