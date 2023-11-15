using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class FloatComponent : MonoBehaviour
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000238 RID: 568 RVA: 0x000098E6 File Offset: 0x00007AE6
	// (set) Token: 0x06000239 RID: 569 RVA: 0x000098EE File Offset: 0x00007AEE
	public float value
	{
		get
		{
			return this._value;
		}
		set
		{
			this._value = value;
		}
	}

	// Token: 0x0600023A RID: 570 RVA: 0x000098F7 File Offset: 0x00007AF7
	public void Increase(float amount)
	{
		this._value += amount;
	}

	// Token: 0x0600023B RID: 571 RVA: 0x00009907 File Offset: 0x00007B07
	public void Increase(FloatComponent amount)
	{
		this._value += amount.value;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000991C File Offset: 0x00007B1C
	public void Decrease(float amount)
	{
		this._value -= amount;
	}

	// Token: 0x0600023D RID: 573 RVA: 0x00009907 File Offset: 0x00007B07
	public void Decrease(FloatComponent amount)
	{
		this._value += amount.value;
	}

	// Token: 0x040001F1 RID: 497
	[SerializeField]
	private string _label;

	// Token: 0x040001F2 RID: 498
	[SerializeField]
	private float _value;
}
