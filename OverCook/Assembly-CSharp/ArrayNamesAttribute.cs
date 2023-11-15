using System;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class ArrayNamesAttribute : PropertyAttribute
{
	// Token: 0x06000BC1 RID: 3009 RVA: 0x0003DAC8 File Offset: 0x0003BEC8
	public ArrayNamesAttribute(string _nameCalculatorMethod)
	{
		this.NameCalculatorMethod = _nameCalculatorMethod;
	}

	// Token: 0x040008EC RID: 2284
	public string NameCalculatorMethod;
}
