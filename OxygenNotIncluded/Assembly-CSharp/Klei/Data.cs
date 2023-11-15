using System;
using System.Collections.Generic;
using ProcGen;
using ProcGenGame;
using VoronoiTree;

namespace Klei
{
	// Token: 0x02000DC1 RID: 3521
	public class Data
	{
		// Token: 0x06006C75 RID: 27765 RVA: 0x002AD3D0 File Offset: 0x002AB5D0
		public Data()
		{
			this.worldLayout = new WorldLayout(null, 0);
			this.terrainCells = new List<TerrainCell>();
			this.overworldCells = new List<TerrainCell>();
			this.rivers = new List<ProcGen.River>();
			this.gameSpawnData = new GameSpawnData();
			this.world = new Chunk();
			this.voronoiTree = new Tree(0);
		}

		// Token: 0x0400515C RID: 20828
		public int globalWorldSeed;

		// Token: 0x0400515D RID: 20829
		public int globalWorldLayoutSeed;

		// Token: 0x0400515E RID: 20830
		public int globalTerrainSeed;

		// Token: 0x0400515F RID: 20831
		public int globalNoiseSeed;

		// Token: 0x04005160 RID: 20832
		public int chunkEdgeSize = 32;

		// Token: 0x04005161 RID: 20833
		public WorldLayout worldLayout;

		// Token: 0x04005162 RID: 20834
		public List<TerrainCell> terrainCells;

		// Token: 0x04005163 RID: 20835
		public List<TerrainCell> overworldCells;

		// Token: 0x04005164 RID: 20836
		public List<ProcGen.River> rivers;

		// Token: 0x04005165 RID: 20837
		public GameSpawnData gameSpawnData;

		// Token: 0x04005166 RID: 20838
		public Chunk world;

		// Token: 0x04005167 RID: 20839
		public Tree voronoiTree;

		// Token: 0x04005168 RID: 20840
		public AxialI clusterLocation;
	}
}
