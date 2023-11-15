using System;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011E4 RID: 4580
	public class Point : MonoBehaviour
	{
		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x060059EA RID: 23018 RVA: 0x0010B577 File Offset: 0x00109777
		public Point.Tag tag
		{
			get
			{
				return this._tag;
			}
		}

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x060059EB RID: 23019 RVA: 0x0010B57F File Offset: 0x0010977F
		public int floor
		{
			get
			{
				return this._floor;
			}
		}

		// Token: 0x0400489E RID: 18590
		[SerializeField]
		private Point.Tag _tag;

		// Token: 0x0400489F RID: 18591
		[SerializeField]
		[Range(1f, 5f)]
		private int _floor;

		// Token: 0x020011E5 RID: 4581
		public enum Tag
		{
			// Token: 0x040048A1 RID: 18593
			None,
			// Token: 0x040048A2 RID: 18594
			Top,
			// Token: 0x040048A3 RID: 18595
			Center,
			// Token: 0x040048A4 RID: 18596
			Opposition,
			// Token: 0x040048A5 RID: 18597
			Inner
		}
	}
}
