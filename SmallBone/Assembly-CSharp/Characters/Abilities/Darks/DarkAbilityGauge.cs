using System;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BCE RID: 3022
	public sealed class DarkAbilityGauge : MonoBehaviour
	{
		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06003E35 RID: 15925 RVA: 0x000B4EC4 File Offset: 0x000B30C4
		// (remove) Token: 0x06003E36 RID: 15926 RVA: 0x000B4EFC File Offset: 0x000B30FC
		public event DarkAbilityGauge.onChangedDelegate onChanged;

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06003E37 RID: 15927 RVA: 0x000B4F31 File Offset: 0x000B3131
		public float gaugePercent
		{
			get
			{
				return this._currentValue / this._maxValue;
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06003E38 RID: 15928 RVA: 0x000B4F40 File Offset: 0x000B3140
		public float currentValue
		{
			get
			{
				return this._currentValue;
			}
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x000B4F48 File Offset: 0x000B3148
		private void Awake()
		{
			this._currentValue = this._initialValue;
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x000B4F56 File Offset: 0x000B3156
		public void Set(float maxValue, float initialValue)
		{
			this._maxValue = maxValue;
			this._initialValue = initialValue;
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x000B4F66 File Offset: 0x000B3166
		public bool Has(float amount)
		{
			return this._currentValue >= amount;
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x000B4F74 File Offset: 0x000B3174
		public void Clear()
		{
			this.Set(0f);
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x000B4F81 File Offset: 0x000B3181
		public void Add(float amount)
		{
			this.Set(this._currentValue + amount);
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x000B4F91 File Offset: 0x000B3191
		public void FillUp()
		{
			this.Set(this._maxValue);
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x000B4FA0 File Offset: 0x000B31A0
		public void Set(float value)
		{
			value = Mathf.Clamp(value, 0f, this._maxValue);
			float currentValue = this._currentValue;
			this._currentValue = value;
			DarkAbilityGauge.onChangedDelegate onChangedDelegate = this.onChanged;
			if (onChangedDelegate == null)
			{
				return;
			}
			onChangedDelegate(currentValue, this._currentValue);
		}

		// Token: 0x04003014 RID: 12308
		[SerializeField]
		private float _initialValue;

		// Token: 0x04003015 RID: 12309
		[SerializeField]
		private float _maxValue;

		// Token: 0x04003016 RID: 12310
		private float _currentValue;

		// Token: 0x02000BCF RID: 3023
		// (Invoke) Token: 0x06003E42 RID: 15938
		public delegate void onChangedDelegate(float oldValue, float newValue);
	}
}
