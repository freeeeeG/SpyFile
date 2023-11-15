using System;
using UnityEngine;

namespace Level.Specials
{
	// Token: 0x0200062B RID: 1579
	public class TimeCostEvent : CostEvent
	{
		// Token: 0x06001FA4 RID: 8100 RVA: 0x00060324 File Offset: 0x0005E524
		private void Awake()
		{
			this._currentCost = (double)this._initialCost;
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x00060334 File Offset: 0x0005E534
		public void UpdateCost()
		{
			double num = this._speed * (double)this.updateInterval;
			if (this._currentCost + num >= (double)this._maxCost)
			{
				this._currentCost = (double)this._maxCost;
				return;
			}
			this._currentCost += num;
			this._display.UpdateDisplay();
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x00060388 File Offset: 0x0005E588
		public void AddSpeed(double extraIncrease)
		{
			this._speed += extraIncrease;
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x00060398 File Offset: 0x0005E598
		public void SetCostSpeed(float chestPrice)
		{
			this._speed = (double)(chestPrice * this._costMultipler / this._targetTime);
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x000603B0 File Offset: 0x0005E5B0
		public override double GetValue()
		{
			return this._currentCost;
		}

		// Token: 0x04001AD2 RID: 6866
		[SerializeField]
		private TimeCostEventDisplay _display;

		// Token: 0x04001AD3 RID: 6867
		[NonSerialized]
		public float updateInterval = 0.2f;

		// Token: 0x04001AD4 RID: 6868
		[SerializeField]
		private int _initialCost;

		// Token: 0x04001AD5 RID: 6869
		[SerializeField]
		private int _maxCost;

		// Token: 0x04001AD6 RID: 6870
		[SerializeField]
		private float _costMultipler;

		// Token: 0x04001AD7 RID: 6871
		[SerializeField]
		private float _targetTime;

		// Token: 0x04001AD8 RID: 6872
		private double _currentCost;

		// Token: 0x04001AD9 RID: 6873
		private double _speed;
	}
}
