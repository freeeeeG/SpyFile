using System;
using KSerialization;

namespace ProcGenGame
{
	// Token: 0x02000CAB RID: 3243
	[SerializationConfig(MemberSerialization.OptOut)]
	public struct Neighbors
	{
		// Token: 0x06006757 RID: 26455 RVA: 0x0026C516 File Offset: 0x0026A716
		public Neighbors(TerrainCell a, TerrainCell b)
		{
			Debug.Assert(a != null && b != null, "NULL Neighbor");
			this.n0 = a;
			this.n1 = b;
		}

		// Token: 0x0400476C RID: 18284
		public TerrainCell n0;

		// Token: 0x0400476D RID: 18285
		public TerrainCell n1;
	}
}
