using System;
using UnityEngine;

// Token: 0x020006C7 RID: 1735
public class CellSelectionInstantiator : MonoBehaviour
{
	// Token: 0x06002F32 RID: 12082 RVA: 0x000F8CAC File Offset: 0x000F6EAC
	private void Awake()
	{
		GameObject gameObject = Util.KInstantiate(this.CellSelectionPrefab, null, "WorldSelectionCollider");
		GameObject gameObject2 = Util.KInstantiate(this.CellSelectionPrefab, null, "WorldSelectionCollider");
		CellSelectionObject component = gameObject.GetComponent<CellSelectionObject>();
		CellSelectionObject component2 = gameObject2.GetComponent<CellSelectionObject>();
		component.alternateSelectionObject = component2;
		component2.alternateSelectionObject = component;
	}

	// Token: 0x04001BFD RID: 7165
	public GameObject CellSelectionPrefab;
}
