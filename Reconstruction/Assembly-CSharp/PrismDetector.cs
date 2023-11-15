using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x020001EF RID: 495
public class PrismDetector : MonoBehaviour
{
	// Token: 0x06000C90 RID: 3216 RVA: 0x00020B1A File Offset: 0x0001ED1A
	public void SetSize(float size)
	{
		base.transform.DOScaleY(1f + size, 1f);
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x00020B34 File Offset: 0x0001ED34
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Bullet component = collision.GetComponent<Bullet>();
		if (component != null)
		{
			component.BulletDamageIntensify += this.IntensifyValue;
		}
	}

	// Token: 0x04000634 RID: 1588
	public float IntensifyValue;
}
