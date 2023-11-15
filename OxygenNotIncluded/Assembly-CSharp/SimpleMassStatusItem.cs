using System;
using UnityEngine;

// Token: 0x02000502 RID: 1282
[AddComponentMenu("KMonoBehaviour/scripts/SimpleMassStatusItem")]
public class SimpleMassStatusItem : KMonoBehaviour
{
	// Token: 0x06001E2D RID: 7725 RVA: 0x000A1119 File Offset: 0x0009F319
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OreMass, base.gameObject);
	}

	// Token: 0x040010E5 RID: 4325
	public string symbolPrefix = "";
}
