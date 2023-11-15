using System;
using System.Collections.Generic;

namespace Pathfinding.Serialization
{
	// Token: 0x020000B6 RID: 182
	public class GraphMeta
	{
		// Token: 0x06000818 RID: 2072 RVA: 0x00035BE8 File Offset: 0x00033DE8
		public Type GetGraphType(int index, Type[] availableGraphTypes)
		{
			if (string.IsNullOrEmpty(this.typeNames[index]))
			{
				return null;
			}
			for (int i = 0; i < availableGraphTypes.Length; i++)
			{
				if (availableGraphTypes[i].FullName == this.typeNames[index])
				{
					return availableGraphTypes[i];
				}
			}
			throw new Exception("No graph of type '" + this.typeNames[index] + "' could be created, type does not exist");
		}

		// Token: 0x040004C4 RID: 1220
		public Version version;

		// Token: 0x040004C5 RID: 1221
		public int graphs;

		// Token: 0x040004C6 RID: 1222
		public List<string> guids;

		// Token: 0x040004C7 RID: 1223
		public List<string> typeNames;
	}
}
