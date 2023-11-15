using System;

namespace Pathfinding
{
	// Token: 0x0200004B RID: 75
	public struct Connection
	{
		// Token: 0x06000377 RID: 887 RVA: 0x0001348C File Offset: 0x0001168C
		public Connection(GraphNode node, uint cost, byte shapeEdge = 255)
		{
			this.node = node;
			this.cost = cost;
			this.shapeEdge = shapeEdge;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000134A3 File Offset: 0x000116A3
		public override int GetHashCode()
		{
			return this.node.GetHashCode() ^ (int)this.cost;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000134B8 File Offset: 0x000116B8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Connection connection = (Connection)obj;
			return connection.node == this.node && connection.cost == this.cost && connection.shapeEdge == this.shapeEdge;
		}

		// Token: 0x0400022E RID: 558
		public GraphNode node;

		// Token: 0x0400022F RID: 559
		public uint cost;

		// Token: 0x04000230 RID: 560
		public byte shapeEdge;

		// Token: 0x04000231 RID: 561
		public const byte NoSharedEdge = 255;
	}
}
