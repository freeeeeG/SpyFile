using System;
using UnityEngine;

// Token: 0x02000270 RID: 624
[Serializable]
public abstract class Optional<T>
{
	// Token: 0x06000BB9 RID: 3001 RVA: 0x0003D9AC File Offset: 0x0003BDAC
	public Optional(T _value)
	{
		this.m_value = _value;
		this.m_hasValue = true;
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x0003D9DC File Offset: 0x0003BDDC
	public Optional()
	{
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0003D9FE File Offset: 0x0003BDFE
	// (set) Token: 0x06000BBC RID: 3004 RVA: 0x0003DA06 File Offset: 0x0003BE06
	public T Value
	{
		get
		{
			return this.m_value;
		}
		set
		{
			this.m_value = value;
			this.m_hasValue = true;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0003DA16 File Offset: 0x0003BE16
	public bool HasValue
	{
		get
		{
			return this.m_hasValue;
		}
	}

	// Token: 0x040008E7 RID: 2279
	[SerializeField]
	[HideInInspectorTest("m_hasValue", true)]
	public T m_value = default(T);

	// Token: 0x040008E8 RID: 2280
	[SerializeField]
	protected bool m_hasValue;
}
