using System;
using UnityEngine;

// Token: 0x02000271 RID: 625
public class ArrayIndexAttribute : PropertyAttribute
{
	// Token: 0x06000BBE RID: 3006 RVA: 0x0003DA82 File Offset: 0x0003BE82
	public ArrayIndexAttribute(string _arrayPath)
	{
		this.ArrayPath = _arrayPath;
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x0003DAA7 File Offset: 0x0003BEA7
	public ArrayIndexAttribute(string _arrayPath, string _scriptableObjectPath) : this(_arrayPath)
	{
		this.ScriptableObjectPath = _scriptableObjectPath;
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0003DAB7 File Offset: 0x0003BEB7
	public ArrayIndexAttribute(string _arrayPath, string _scriptableObjectPath, SerializationUtils.RootType _rootType) : this(_arrayPath, _scriptableObjectPath)
	{
		this.SearchRoot = _rootType;
	}

	// Token: 0x040008E9 RID: 2281
	public SerializationUtils.RootType SearchRoot;

	// Token: 0x040008EA RID: 2282
	public string ArrayPath = string.Empty;

	// Token: 0x040008EB RID: 2283
	public string ScriptableObjectPath = string.Empty;
}
