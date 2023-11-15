using System;
using UnityEngine;

namespace Housing
{
	// Token: 0x0200014A RID: 330
	public class BuildLevel : MonoBehaviour
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00013242 File Offset: 0x00011442
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x0001324A File Offset: 0x0001144A
		public BuildLevel next
		{
			get
			{
				return this._next;
			}
			set
			{
				this._next = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x00013253 File Offset: 0x00011453
		// (set) Token: 0x06000695 RID: 1685 RVA: 0x0001325B File Offset: 0x0001145B
		public int order
		{
			get
			{
				return this._order;
			}
			set
			{
				this._order = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x00013264 File Offset: 0x00011464
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x0001326C File Offset: 0x0001146C
		public int cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				this._cost = value;
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000698 RID: 1688 RVA: 0x00013278 File Offset: 0x00011478
		// (remove) Token: 0x06000699 RID: 1689 RVA: 0x000132B0 File Offset: 0x000114B0
		public event Action onBuild;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600069A RID: 1690 RVA: 0x000132E8 File Offset: 0x000114E8
		// (remove) Token: 0x0600069B RID: 1691 RVA: 0x00013320 File Offset: 0x00011520
		public event Action onNew;

		// Token: 0x0600069C RID: 1692 RVA: 0x00013355 File Offset: 0x00011555
		public void Build(int buildedOrder, int seen)
		{
			this.Build(buildedOrder);
			BuildLevel next = this._next;
			if (next != null)
			{
				next.Build(buildedOrder, seen);
			}
			this.New(seen);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00013378 File Offset: 0x00011578
		public BuildLevel GetLevelAfterPoint(int point)
		{
			if (this._order > point)
			{
				return this;
			}
			if (!(this._next == null))
			{
				return this._next.GetLevelAfterPoint(point);
			}
			return null;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000133A1 File Offset: 0x000115A1
		private void Build(int buildedOrder)
		{
			if (this._order > buildedOrder)
			{
				return;
			}
			Action action = this.onBuild;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x000133BD File Offset: 0x000115BD
		private void New(int seen)
		{
			if (this._order <= seen)
			{
				return;
			}
			Action action = this.onNew;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x040004D5 RID: 1237
		[SerializeField]
		private BuildLevel _next;

		// Token: 0x040004D6 RID: 1238
		[SerializeField]
		private int _order;

		// Token: 0x040004D7 RID: 1239
		[SerializeField]
		private int _cost;
	}
}
