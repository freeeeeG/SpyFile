using System;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class ForbidRange : MonoBehaviour
{
	// Token: 0x06000977 RID: 2423 RVA: 0x000191AC File Offset: 0x000173AC
	private void Awake()
	{
		this.detector = base.transform.parent.GetComponent<RangeHolder>();
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x000191C4 File Offset: 0x000173C4
	private void OnTriggerEnter2D(Collider2D collision)
	{
		collision.GetComponent<TargetPoint>();
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x000191CD File Offset: 0x000173CD
	private void OnTriggerExit2D(Collider2D collision)
	{
		collision.GetComponent<TargetPoint>();
	}

	// Token: 0x040004DA RID: 1242
	private RangeHolder detector;
}
