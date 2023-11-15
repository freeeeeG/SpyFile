using System;
using Level;
using UnityEngine;

// Token: 0x02000085 RID: 133
public class SetPositionToMapLeft : MonoBehaviour
{
	// Token: 0x0600028E RID: 654 RVA: 0x0000A620 File Offset: 0x00008820
	private void Start()
	{
		base.transform.position = new Vector2(Map.Instance.bounds.min.x, Map.Instance.bounds.max.y);
	}
}
