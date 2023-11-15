using System;

namespace flanne
{
	// Token: 0x0200011C RID: 284
	public class StatMod
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060007D2 RID: 2002 RVA: 0x000217CC File Offset: 0x0001F9CC
		// (remove) Token: 0x060007D3 RID: 2003 RVA: 0x00021804 File Offset: 0x0001FA04
		public event EventHandler ChangedEvent;

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x00021839 File Offset: 0x0001FA39
		public int FlatBonus
		{
			get
			{
				return this._flatBonus;
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00021841 File Offset: 0x0001FA41
		public float Modify(float baseValue)
		{
			return (baseValue + (float)this._flatBonus) * (1f + this._multiplierBonus) * this._multiplierReduction;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00021860 File Offset: 0x0001FA60
		public float ModifyInverse(float baseValue)
		{
			return (baseValue + (float)this._flatBonus) / ((1f + this._multiplierBonus) * this._multiplierReduction);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002187F File Offset: 0x0001FA7F
		public void AddFlatBonus(int value)
		{
			this._flatBonus += value;
			EventHandler changedEvent = this.ChangedEvent;
			if (changedEvent == null)
			{
				return;
			}
			changedEvent(this, null);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x000218A1 File Offset: 0x0001FAA1
		public void AddMultiplierBonus(float value)
		{
			this._multiplierBonus += value;
			EventHandler changedEvent = this.ChangedEvent;
			if (changedEvent == null)
			{
				return;
			}
			changedEvent(this, null);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000218C3 File Offset: 0x0001FAC3
		public void AddMultiplierReduction(float value)
		{
			this._multiplierReduction *= value;
			EventHandler changedEvent = this.ChangedEvent;
			if (changedEvent == null)
			{
				return;
			}
			changedEvent(this, null);
		}

		// Token: 0x040005B3 RID: 1459
		private int _flatBonus;

		// Token: 0x040005B4 RID: 1460
		private float _multiplierBonus;

		// Token: 0x040005B5 RID: 1461
		private float _multiplierReduction = 1f;
	}
}
