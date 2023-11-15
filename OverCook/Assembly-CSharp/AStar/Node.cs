using System;

namespace AStar
{
	// Token: 0x020000F9 RID: 249
	public class Node
	{
		// Token: 0x060004AC RID: 1196 RVA: 0x00027E98 File Offset: 0x00026298
		public Node(int x, int y, bool isWalkable, Point2 endLocation)
		{
			this.Location = new Point2(x, y);
			this.State = NodeState.Untested;
			this.IsWalkable = isWalkable;
			this.H = Node.GetTraversalCost(this.Location, endLocation);
			this.G = 0f;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00027EE4 File Offset: 0x000262E4
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x00027EEC File Offset: 0x000262EC
		public Point2 Location { get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00027EF5 File Offset: 0x000262F5
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x00027EFD File Offset: 0x000262FD
		public bool IsWalkable { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00027F06 File Offset: 0x00026306
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x00027F0E File Offset: 0x0002630E
		public float G { get; private set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00027F17 File Offset: 0x00026317
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x00027F1F File Offset: 0x0002631F
		public float H { get; private set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00027F28 File Offset: 0x00026328
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00027F30 File Offset: 0x00026330
		public NodeState State { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00027F39 File Offset: 0x00026339
		public float F
		{
			get
			{
				return this.G + this.H;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00027F48 File Offset: 0x00026348
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x00027F50 File Offset: 0x00026350
		public Node ParentNode
		{
			get
			{
				return this.parentNode;
			}
			set
			{
				this.parentNode = value;
				this.G = this.parentNode.G + Node.GetTraversalCost(this.Location, this.parentNode.Location);
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00027F81 File Offset: 0x00026381
		public override string ToString()
		{
			return string.Format("{0}, {1}: {2}", this.Location.X, this.Location.Y, this.State);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00027FB8 File Offset: 0x000263B8
		internal static float GetTraversalCost(Point2 location, Point2 otherLocation)
		{
			float num = (float)(otherLocation.X - location.X);
			float num2 = (float)(otherLocation.Y - location.Y);
			return (float)Math.Sqrt((double)(num * num + num2 * num2));
		}

		// Token: 0x04000419 RID: 1049
		private Node parentNode;
	}
}
