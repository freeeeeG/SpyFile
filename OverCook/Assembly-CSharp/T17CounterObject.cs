using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B63 RID: 2915
public class T17CounterObject : MonoBehaviour
{
	// Token: 0x06003B42 RID: 15170 RVA: 0x0011A05D File Offset: 0x0011845D
	private void Start()
	{
		this.m_StartValue = Mathf.Clamp(this.m_StartValue, this.m_MinimumValue, this.m_MaxValue);
		this.m_CounterValue = this.m_StartValue;
		this.UpdateText();
	}

	// Token: 0x06003B43 RID: 15171 RVA: 0x0011A090 File Offset: 0x00118490
	public void CounterIncrease()
	{
		this.m_CounterValue++;
		if (this.m_CounterValue > this.m_MaxValue)
		{
			this.m_CounterValue = this.m_MaxValue;
		}
		this.UpdateText();
		if (this.OnCounterIncreased != null)
		{
			this.OnCounterIncreased();
		}
		if (this.m_CounterValue == this.m_MaxValue && this.OnCounterMaxValueReached != null)
		{
			this.OnCounterMaxValueReached();
		}
	}

	// Token: 0x06003B44 RID: 15172 RVA: 0x0011A10C File Offset: 0x0011850C
	public void CounterDescrease()
	{
		this.m_CounterValue--;
		if (this.m_CounterValue < this.m_MinimumValue)
		{
			this.m_CounterValue = this.m_MinimumValue;
		}
		if (this.OnCounterDescreased != null)
		{
			this.OnCounterDescreased();
		}
		if (this.m_CounterValue == this.m_MinimumValue && this.OnCounterMinumumValueReached != null)
		{
			this.OnCounterMinumumValueReached();
		}
		this.UpdateText();
	}

	// Token: 0x06003B45 RID: 15173 RVA: 0x0011A188 File Offset: 0x00118588
	private void UpdateText()
	{
		string text = this.m_CounterValue.ToString().PadLeft(this.m_Digits, '0');
		if (this.m_CounterDisplay != null)
		{
			this.m_CounterDisplay.text = text;
		}
	}

	// Token: 0x06003B46 RID: 15174 RVA: 0x0011A1D1 File Offset: 0x001185D1
	public void SetMaxValue(int maxValue)
	{
		this.m_MaxValue = Mathf.Max(this.m_MinimumValue, maxValue);
		this.m_CounterValue = Mathf.Clamp(this.m_CounterValue, this.m_MinimumValue, this.m_MaxValue);
		this.UpdateText();
	}

	// Token: 0x06003B47 RID: 15175 RVA: 0x0011A208 File Offset: 0x00118608
	public void Reset()
	{
		this.m_StartValue = Mathf.Clamp(this.m_StartValue, this.m_MinimumValue, this.m_MaxValue);
		this.m_CounterValue = this.m_StartValue;
		this.UpdateText();
	}

	// Token: 0x06003B48 RID: 15176 RVA: 0x0011A239 File Offset: 0x00118639
	public int GetCounterValue()
	{
		return this.m_CounterValue;
	}

	// Token: 0x0400302E RID: 12334
	public Text m_CounterDisplay;

	// Token: 0x0400302F RID: 12335
	public int m_Digits = 3;

	// Token: 0x04003030 RID: 12336
	public int m_MinimumValue;

	// Token: 0x04003031 RID: 12337
	public int m_MaxValue = 999;

	// Token: 0x04003032 RID: 12338
	public int m_StartValue = 1;

	// Token: 0x04003033 RID: 12339
	public T17CounterObject.T17CounterObjectDelegate OnCounterIncreased;

	// Token: 0x04003034 RID: 12340
	public T17CounterObject.T17CounterObjectDelegate OnCounterDescreased;

	// Token: 0x04003035 RID: 12341
	public T17CounterObject.T17CounterObjectDelegate OnCounterMinumumValueReached;

	// Token: 0x04003036 RID: 12342
	public T17CounterObject.T17CounterObjectDelegate OnCounterMaxValueReached;

	// Token: 0x04003037 RID: 12343
	private int m_CounterValue;

	// Token: 0x02000B64 RID: 2916
	// (Invoke) Token: 0x06003B4A RID: 15178
	public delegate void T17CounterObjectDelegate();
}
