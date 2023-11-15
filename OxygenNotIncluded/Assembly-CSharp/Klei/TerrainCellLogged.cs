using System;
using System.Collections.Generic;
using ProcGen.Map;
using ProcGenGame;
using VoronoiTree;

namespace Klei
{
	// Token: 0x02000DC7 RID: 3527
	public class TerrainCellLogged : TerrainCell
	{
		// Token: 0x06006C89 RID: 27785 RVA: 0x002AD872 File Offset: 0x002ABA72
		public TerrainCellLogged()
		{
		}

		// Token: 0x06006C8A RID: 27786 RVA: 0x002AD87A File Offset: 0x002ABA7A
		public TerrainCellLogged(Cell node, Diagram.Site site, Dictionary<Tag, int> distancesToTags) : base(node, site, distancesToTags)
		{
		}

		// Token: 0x06006C8B RID: 27787 RVA: 0x002AD885 File Offset: 0x002ABA85
		public override void LogInfo(string evt, string param, float value)
		{
		}
	}
}
