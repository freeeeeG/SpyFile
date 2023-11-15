using System;

namespace Klei
{
	// Token: 0x02000DBE RID: 3518
	public struct SolidInfo
	{
		// Token: 0x06006C71 RID: 27761 RVA: 0x002AD349 File Offset: 0x002AB549
		public SolidInfo(int cellIdx, bool isSolid)
		{
			this.cellIdx = cellIdx;
			this.isSolid = isSolid;
		}

		// Token: 0x04005153 RID: 20819
		public int cellIdx;

		// Token: 0x04005154 RID: 20820
		public bool isSolid;
	}
}
