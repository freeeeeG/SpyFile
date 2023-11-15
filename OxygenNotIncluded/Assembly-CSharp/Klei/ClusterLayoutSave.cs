using System;
using System.Collections.Generic;

namespace Klei
{
	// Token: 0x02000DC5 RID: 3525
	public class ClusterLayoutSave
	{
		// Token: 0x06006C79 RID: 27769 RVA: 0x002AD474 File Offset: 0x002AB674
		public ClusterLayoutSave()
		{
			this.worlds = new List<ClusterLayoutSave.World>();
		}

		// Token: 0x04005179 RID: 20857
		public string ID;

		// Token: 0x0400517A RID: 20858
		public Vector2I version;

		// Token: 0x0400517B RID: 20859
		public List<ClusterLayoutSave.World> worlds;

		// Token: 0x0400517C RID: 20860
		public Vector2I size;

		// Token: 0x0400517D RID: 20861
		public int currentWorldIdx;

		// Token: 0x0400517E RID: 20862
		public int numRings;

		// Token: 0x0400517F RID: 20863
		public Dictionary<ClusterLayoutSave.POIType, List<AxialI>> poiLocations = new Dictionary<ClusterLayoutSave.POIType, List<AxialI>>();

		// Token: 0x04005180 RID: 20864
		public Dictionary<AxialI, string> poiPlacements = new Dictionary<AxialI, string>();

		// Token: 0x02001F57 RID: 8023
		public class World
		{
			// Token: 0x04008DD5 RID: 36309
			public Data data = new Data();

			// Token: 0x04008DD6 RID: 36310
			public string name = string.Empty;

			// Token: 0x04008DD7 RID: 36311
			public bool isDiscovered;

			// Token: 0x04008DD8 RID: 36312
			public List<string> traits = new List<string>();

			// Token: 0x04008DD9 RID: 36313
			public List<string> storyTraits = new List<string>();
		}

		// Token: 0x02001F58 RID: 8024
		public enum POIType
		{
			// Token: 0x04008DDB RID: 36315
			TemporalTear,
			// Token: 0x04008DDC RID: 36316
			ResearchDestination
		}
	}
}
