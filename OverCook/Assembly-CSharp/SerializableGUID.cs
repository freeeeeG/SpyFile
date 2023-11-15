using System;
using UnityEngine;

// Token: 0x02000221 RID: 545
[Serializable]
public struct SerializableGUID
{
	// Token: 0x06000929 RID: 2345 RVA: 0x00036165 File Offset: 0x00034565
	private SerializableGUID(string _data)
	{
		this.Data = _data;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x0003616E File Offset: 0x0003456E
	public static explicit operator SerializableGUID(Guid _guid)
	{
		return new SerializableGUID(_guid.ToString());
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00036182 File Offset: 0x00034582
	public static explicit operator Guid(SerializableGUID _sGUID)
	{
		return new Guid(_sGUID.Data);
	}

	// Token: 0x040007EB RID: 2027
	[SerializeField]
	private string Data;
}
