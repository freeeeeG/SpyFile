using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace KMod
{
	// Token: 0x02000D70 RID: 3440
	public class UserMod2
	{
		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06006B4A RID: 27466 RVA: 0x002A0046 File Offset: 0x0029E246
		// (set) Token: 0x06006B4B RID: 27467 RVA: 0x002A004E File Offset: 0x0029E24E
		public Assembly assembly { get; set; }

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06006B4C RID: 27468 RVA: 0x002A0057 File Offset: 0x0029E257
		// (set) Token: 0x06006B4D RID: 27469 RVA: 0x002A005F File Offset: 0x0029E25F
		public string path { get; set; }

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06006B4E RID: 27470 RVA: 0x002A0068 File Offset: 0x0029E268
		// (set) Token: 0x06006B4F RID: 27471 RVA: 0x002A0070 File Offset: 0x0029E270
		public Mod mod { get; set; }

		// Token: 0x06006B50 RID: 27472 RVA: 0x002A0079 File Offset: 0x0029E279
		public virtual void OnLoad(Harmony harmony)
		{
			harmony.PatchAll(this.assembly);
		}

		// Token: 0x06006B51 RID: 27473 RVA: 0x002A0087 File Offset: 0x0029E287
		public virtual void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
		{
		}
	}
}
