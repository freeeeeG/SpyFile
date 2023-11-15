using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Usables
{
	// Token: 0x02000755 RID: 1877
	public sealed class LiquidMaster : MonoBehaviour
	{
		// Token: 0x1400005D RID: 93
		// (add) Token: 0x0600263E RID: 9790 RVA: 0x00073A90 File Offset: 0x00071C90
		// (remove) Token: 0x0600263F RID: 9791 RVA: 0x00073AC8 File Offset: 0x00071CC8
		public event Action onChanged;

		// Token: 0x06002640 RID: 9792 RVA: 0x00073AFD File Offset: 0x00071CFD
		private void Awake()
		{
			this._liquidList = new List<Liquid>();
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x00073B0A File Offset: 0x00071D0A
		public int Count()
		{
			return this._liquidList.Count;
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x00073B17 File Offset: 0x00071D17
		public void Add(Liquid liquid)
		{
			this._liquidList.Add(liquid);
			Action action = this.onChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x00073B35 File Offset: 0x00071D35
		public void Remove(Liquid liquid)
		{
			this._liquidList.Remove(liquid);
			Action action = this.onChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x00073B54 File Offset: 0x00071D54
		public int GetStack()
		{
			int num = 0;
			foreach (Liquid liquid in this._liquidList)
			{
				num += liquid.stack;
			}
			return num;
		}

		// Token: 0x040020E2 RID: 8418
		private List<Liquid> _liquidList;
	}
}
