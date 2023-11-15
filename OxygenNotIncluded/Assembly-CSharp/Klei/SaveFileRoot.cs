using System;
using System.Collections.Generic;
using KMod;

namespace Klei
{
	// Token: 0x02000DC0 RID: 3520
	internal class SaveFileRoot
	{
		// Token: 0x06006C74 RID: 27764 RVA: 0x002AD3BD File Offset: 0x002AB5BD
		public SaveFileRoot()
		{
			this.streamed = new Dictionary<string, byte[]>();
		}

		// Token: 0x04005156 RID: 20822
		public int WidthInCells;

		// Token: 0x04005157 RID: 20823
		public int HeightInCells;

		// Token: 0x04005158 RID: 20824
		public Dictionary<string, byte[]> streamed;

		// Token: 0x04005159 RID: 20825
		public string clusterID;

		// Token: 0x0400515A RID: 20826
		public List<ModInfo> requiredMods;

		// Token: 0x0400515B RID: 20827
		public List<Label> active_mods;
	}
}
