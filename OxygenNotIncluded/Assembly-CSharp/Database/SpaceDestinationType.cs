using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Database
{
	// Token: 0x02000D1D RID: 3357
	[DebuggerDisplay("{Id}")]
	public class SpaceDestinationType : Resource
	{
		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x060069FC RID: 27132 RVA: 0x002938A4 File Offset: 0x00291AA4
		// (set) Token: 0x060069FD RID: 27133 RVA: 0x002938AC File Offset: 0x00291AAC
		public int maxiumMass { get; private set; }

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060069FE RID: 27134 RVA: 0x002938B5 File Offset: 0x00291AB5
		// (set) Token: 0x060069FF RID: 27135 RVA: 0x002938BD File Offset: 0x00291ABD
		public int minimumMass { get; private set; }

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06006A00 RID: 27136 RVA: 0x002938C6 File Offset: 0x00291AC6
		public float replishmentPerCycle
		{
			get
			{
				return 1000f / (float)this.cyclesToRecover;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06006A01 RID: 27137 RVA: 0x002938D5 File Offset: 0x00291AD5
		public float replishmentPerSim1000ms
		{
			get
			{
				return 1000f / ((float)this.cyclesToRecover * 600f);
			}
		}

		// Token: 0x06006A02 RID: 27138 RVA: 0x002938EC File Offset: 0x00291AEC
		public SpaceDestinationType(string id, ResourceSet parent, string name, string description, int iconSize, string spriteName, Dictionary<SimHashes, MathUtil.MinMax> elementTable, Dictionary<string, int> recoverableEntities = null, ArtifactDropRate artifactDropRate = null, int max = 64000000, int min = 63994000, int cycles = 6, bool visitable = true) : base(id, parent, name)
		{
			this.typeName = name;
			this.description = description;
			this.iconSize = iconSize;
			this.spriteName = spriteName;
			this.elementTable = elementTable;
			this.recoverableEntities = recoverableEntities;
			this.artifactDropTable = artifactDropRate;
			this.maxiumMass = max;
			this.minimumMass = min;
			this.cyclesToRecover = cycles;
			this.visitable = visitable;
		}

		// Token: 0x04004CEE RID: 19694
		public const float MASS_TO_RECOVER = 1000f;

		// Token: 0x04004CEF RID: 19695
		public string typeName;

		// Token: 0x04004CF0 RID: 19696
		public string description;

		// Token: 0x04004CF1 RID: 19697
		public int iconSize = 128;

		// Token: 0x04004CF2 RID: 19698
		public string spriteName;

		// Token: 0x04004CF3 RID: 19699
		public Dictionary<SimHashes, MathUtil.MinMax> elementTable;

		// Token: 0x04004CF4 RID: 19700
		public Dictionary<string, int> recoverableEntities;

		// Token: 0x04004CF5 RID: 19701
		public ArtifactDropRate artifactDropTable;

		// Token: 0x04004CF6 RID: 19702
		public bool visitable;

		// Token: 0x04004CF9 RID: 19705
		public int cyclesToRecover;
	}
}
