using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000074 RID: 116
	public class Multiplier
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060004DD RID: 1245 RVA: 0x000186F4 File Offset: 0x000168F4
		// (remove) Token: 0x060004DE RID: 1246 RVA: 0x0001872C File Offset: 0x0001692C
		public event EventHandler<float> ChangedEvent;

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00018761 File Offset: 0x00016961
		public float value
		{
			get
			{
				return (1f + this.bonus) * this.reduction;
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00018776 File Offset: 0x00016976
		public void Increase(float amount)
		{
			if (amount < 0f)
			{
				this.Decrease(Mathf.Abs(amount));
				return;
			}
			this.bonus += amount;
			EventHandler<float> changedEvent = this.ChangedEvent;
			if (changedEvent == null)
			{
				return;
			}
			changedEvent(this, this.value);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000187B4 File Offset: 0x000169B4
		public void Decrease(float amount)
		{
			if (amount < 0f)
			{
				this.Decrease(Mathf.Abs(amount));
				return;
			}
			this.reduction *= 1f - amount;
			EventHandler<float> changedEvent = this.ChangedEvent;
			if (changedEvent == null)
			{
				return;
			}
			changedEvent(this, this.value);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00018801 File Offset: 0x00016A01
		public void ChangeBonus(float amount)
		{
			this.bonus += amount;
			EventHandler<float> changedEvent = this.ChangedEvent;
			if (changedEvent == null)
			{
				return;
			}
			changedEvent(this, this.value);
		}

		// Token: 0x040002CF RID: 719
		private float bonus;

		// Token: 0x040002D0 RID: 720
		private float reduction = 1f;
	}
}
