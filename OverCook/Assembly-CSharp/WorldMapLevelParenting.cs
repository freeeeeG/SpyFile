using System;
using UnityEngine;

// Token: 0x02000BE9 RID: 3049
public class WorldMapLevelParenting : MonoBehaviour
{
	// Token: 0x06003E48 RID: 15944 RVA: 0x0012A40C File Offset: 0x0012880C
	protected void Awake()
	{
		if (base.gameObject.RequestComponent<GridAutoParenting>() != null)
		{
			Transform parent = base.gameObject.transform.parent;
			GameObject gameObject = parent.gameObject.RequestChild("Tile/FlagBase");
			if (gameObject != null)
			{
				base.transform.SetParent(gameObject.transform, false);
				base.transform.localRotation = Quaternion.identity;
			}
		}
	}
}
