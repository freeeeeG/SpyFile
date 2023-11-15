using System;
using UnityEngine;

// Token: 0x02000C6A RID: 3178
[AddComponentMenu("KMonoBehaviour/scripts/SpawnScreen")]
public class SpawnScreen : KMonoBehaviour
{
	// Token: 0x0600652F RID: 25903 RVA: 0x00259AE0 File Offset: 0x00257CE0
	protected override void OnPrefabInit()
	{
		Util.KInstantiateUI(this.Screen, base.gameObject, false);
	}

	// Token: 0x04004570 RID: 17776
	public GameObject Screen;
}
