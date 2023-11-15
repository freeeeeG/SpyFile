using System;
using System.Collections.Generic;

namespace Klei
{
	// Token: 0x02000DC2 RID: 3522
	public class WorldGenSave
	{
		// Token: 0x06006C76 RID: 27766 RVA: 0x002AD43B File Offset: 0x002AB63B
		public WorldGenSave()
		{
			this.data = new Data();
		}

		// Token: 0x04005169 RID: 20841
		public Vector2I version;

		// Token: 0x0400516A RID: 20842
		public Data data;

		// Token: 0x0400516B RID: 20843
		public string worldID;

		// Token: 0x0400516C RID: 20844
		public List<string> traitIDs;

		// Token: 0x0400516D RID: 20845
		public List<string> storyTraitIDs;
	}
}
