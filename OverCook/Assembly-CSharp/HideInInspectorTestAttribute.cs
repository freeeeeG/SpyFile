using System;
using UnityEngine;

// Token: 0x0200027C RID: 636
public class HideInInspectorTestAttribute : PropertyAttribute
{
	// Token: 0x06000BCB RID: 3019 RVA: 0x0003DB9F File Offset: 0x0003BF9F
	public HideInInspectorTestAttribute(string _variableName, object _value, SerializationUtils.RootType _rootType) : this(_variableName, _value)
	{
		this.SearchRoot = _rootType;
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x0003DBB0 File Offset: 0x0003BFB0
	public HideInInspectorTestAttribute(string _variableName, object _value)
	{
		this.Pairs = new HideInInspectorTestAttribute.HideTestPair[]
		{
			new HideInInspectorTestAttribute.HideTestPair(_variableName, _value)
		};
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x0003DBD7 File Offset: 0x0003BFD7
	public HideInInspectorTestAttribute(string _variableName, object _value, string _variableName2, object _value2)
	{
		this.Pairs = new HideInInspectorTestAttribute.HideTestPair[]
		{
			new HideInInspectorTestAttribute.HideTestPair(_variableName, _value),
			new HideInInspectorTestAttribute.HideTestPair(_variableName2, _value2)
		};
	}

	// Token: 0x040008FE RID: 2302
	public SerializationUtils.RootType SearchRoot;

	// Token: 0x040008FF RID: 2303
	public HideInInspectorTestAttribute.HideTestPair[] Pairs;

	// Token: 0x0200027D RID: 637
	public struct HideTestPair
	{
		// Token: 0x06000BCE RID: 3022 RVA: 0x0003DC12 File Offset: 0x0003C012
		public HideTestPair(string _variableName, object _value)
		{
			this.m_variableName = _variableName;
			this.m_value = _value;
		}

		// Token: 0x04000900 RID: 2304
		public string m_variableName;

		// Token: 0x04000901 RID: 2305
		public object m_value;
	}
}
