using System;
using System.Collections.Generic;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008B3 RID: 2227
	public class EntityTypeComparer : IEqualityComparer<EntityType>
	{
		// Token: 0x06002B5F RID: 11103 RVA: 0x000CAF5B File Offset: 0x000C935B
		public bool Equals(EntityType x, EntityType y)
		{
			return x == y;
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000CAF61 File Offset: 0x000C9361
		public int GetHashCode(EntityType obj)
		{
			return (int)obj;
		}
	}
}
