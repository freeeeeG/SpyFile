using System;
using UnityEngine;

// Token: 0x02000795 RID: 1941
[AddComponentMenu("KMonoBehaviour/scripts/EntityPrefabs")]
public class EntityPrefabs : KMonoBehaviour
{
	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x060035FF RID: 13823 RVA: 0x00123C7C File Offset: 0x00121E7C
	// (set) Token: 0x06003600 RID: 13824 RVA: 0x00123C83 File Offset: 0x00121E83
	public static EntityPrefabs Instance { get; private set; }

	// Token: 0x06003601 RID: 13825 RVA: 0x00123C8B File Offset: 0x00121E8B
	public static void DestroyInstance()
	{
		EntityPrefabs.Instance = null;
	}

	// Token: 0x06003602 RID: 13826 RVA: 0x00123C93 File Offset: 0x00121E93
	protected override void OnPrefabInit()
	{
		EntityPrefabs.Instance = this;
	}

	// Token: 0x040020F1 RID: 8433
	public GameObject SelectMarker;

	// Token: 0x040020F2 RID: 8434
	public GameObject ForegroundLayer;
}
