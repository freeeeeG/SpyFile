using System;
using Level;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class SetParentToMap : MonoBehaviour
{
	// Token: 0x0600028C RID: 652 RVA: 0x0000A607 File Offset: 0x00008807
	private void Start()
	{
		base.transform.SetParent(Map.Instance.transform);
	}
}
