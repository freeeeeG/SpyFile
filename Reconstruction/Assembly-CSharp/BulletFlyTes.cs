using System;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class BulletFlyTes : MonoBehaviour
{
	// Token: 0x06000B9A RID: 2970 RVA: 0x0001E3C0 File Offset: 0x0001C5C0
	private void Start()
	{
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x0001E3C2 File Offset: 0x0001C5C2
	private void Update()
	{
		base.transform.position = Vector2.MoveTowards(base.transform.position, this.pos, 10f * Time.deltaTime);
	}

	// Token: 0x040005C7 RID: 1479
	private Vector2 pos = new Vector2(100f, 100f);
}
