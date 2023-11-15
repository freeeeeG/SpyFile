using System;
using Level;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x02000171 RID: 369
	public sealed class LuckyMeasuringInstrumentSlot : MonoBehaviour
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x00016667 File Offset: 0x00014867
		public Vector3 dropPoint
		{
			get
			{
				return this._dropPoint.position;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x00016674 File Offset: 0x00014874
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x0001667C File Offset: 0x0001487C
		public DroppedGear droppedGear
		{
			get
			{
				return this._droppedGear;
			}
			set
			{
				this._droppedGear = value;
			}
		}

		// Token: 0x040005A3 RID: 1443
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x040005A4 RID: 1444
		private DroppedGear _droppedGear;
	}
}
