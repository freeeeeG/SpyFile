using System;

namespace AStar
{
	// Token: 0x020000FC RID: 252
	public class SearchParameters
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x0002844D File Offset: 0x0002684D
		public SearchParameters(Point2 startLocation, Point2 endLocation, bool[,] map)
		{
			this.StartLocation = startLocation;
			this.EndLocation = endLocation;
			this.Map = map;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x0002846A File Offset: 0x0002686A
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x00028472 File Offset: 0x00026872
		public Point2 StartLocation { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0002847B File Offset: 0x0002687B
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x00028483 File Offset: 0x00026883
		public Point2 EndLocation { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0002848C File Offset: 0x0002688C
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x00028494 File Offset: 0x00026894
		public bool[,] Map { get; set; }
	}
}
