using System;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class DetectRange : MonoBehaviour
{
	// Token: 0x06000973 RID: 2419 RVA: 0x00019144 File Offset: 0x00017344
	private void Awake()
	{
		this.detector = base.transform.parent.GetComponent<RangeHolder>();
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0001915C File Offset: 0x0001735C
	private void OnTriggerEnter2D(Collider2D collision)
	{
		TargetPoint component = collision.GetComponent<TargetPoint>();
		if (component)
		{
			this.detector.AddTarget(component);
		}
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00019184 File Offset: 0x00017384
	private void OnTriggerExit2D(Collider2D collision)
	{
		TargetPoint component = collision.GetComponent<TargetPoint>();
		this.detector.RemoveTarget(component);
	}

	// Token: 0x040004D9 RID: 1241
	private RangeHolder detector;
}
