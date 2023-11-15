using System;

namespace Klei
{
	// Token: 0x02000DC4 RID: 3524
	public class SimSaveFileStructure
	{
		// Token: 0x06006C78 RID: 27768 RVA: 0x002AD461 File Offset: 0x002AB661
		public SimSaveFileStructure()
		{
			this.worldDetail = new WorldDetailSave();
		}

		// Token: 0x04005173 RID: 20851
		public int WidthInCells;

		// Token: 0x04005174 RID: 20852
		public int HeightInCells;

		// Token: 0x04005175 RID: 20853
		public int x;

		// Token: 0x04005176 RID: 20854
		public int y;

		// Token: 0x04005177 RID: 20855
		public byte[] Sim;

		// Token: 0x04005178 RID: 20856
		public WorldDetailSave worldDetail;
	}
}
