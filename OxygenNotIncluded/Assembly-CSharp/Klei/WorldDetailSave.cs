using System;
using System.Collections.Generic;
using Delaunay.Geo;
using KSerialization;
using ProcGen;
using ProcGenGame;

namespace Klei
{
	// Token: 0x02000DC3 RID: 3523
	public class WorldDetailSave
	{
		// Token: 0x06006C77 RID: 27767 RVA: 0x002AD44E File Offset: 0x002AB64E
		public WorldDetailSave()
		{
			this.overworldCells = new List<WorldDetailSave.OverworldCell>();
		}

		// Token: 0x0400516E RID: 20846
		public List<WorldDetailSave.OverworldCell> overworldCells;

		// Token: 0x0400516F RID: 20847
		public int globalWorldSeed;

		// Token: 0x04005170 RID: 20848
		public int globalWorldLayoutSeed;

		// Token: 0x04005171 RID: 20849
		public int globalTerrainSeed;

		// Token: 0x04005172 RID: 20850
		public int globalNoiseSeed;

		// Token: 0x02001F56 RID: 8022
		[SerializationConfig(MemberSerialization.OptOut)]
		public class OverworldCell
		{
			// Token: 0x0600A23B RID: 41531 RVA: 0x00363972 File Offset: 0x00361B72
			public OverworldCell()
			{
			}

			// Token: 0x0600A23C RID: 41532 RVA: 0x0036397A File Offset: 0x00361B7A
			public OverworldCell(SubWorld.ZoneType zoneType, TerrainCell tc)
			{
				this.poly = tc.poly;
				this.tags = tc.node.tags;
				this.zoneType = zoneType;
			}

			// Token: 0x04008DD2 RID: 36306
			public Polygon poly;

			// Token: 0x04008DD3 RID: 36307
			public TagSet tags;

			// Token: 0x04008DD4 RID: 36308
			public SubWorld.ZoneType zoneType;
		}
	}
}
