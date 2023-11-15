using System;
using UnityEngine;

// Token: 0x02000B1C RID: 2844
[AddComponentMenu("KMonoBehaviour/scripts/HasSortOrder")]
public class HasSortOrder : KMonoBehaviour, IHasSortOrder
{
	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x0600578F RID: 22415 RVA: 0x00200DB5 File Offset: 0x001FEFB5
	// (set) Token: 0x06005790 RID: 22416 RVA: 0x00200DBD File Offset: 0x001FEFBD
	public int sortOrder { get; set; }
}
