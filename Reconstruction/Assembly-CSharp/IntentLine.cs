using System;
using UnityEngine;

// Token: 0x02000175 RID: 373
public class IntentLine : MonoBehaviour
{
	// Token: 0x0600097F RID: 2431 RVA: 0x00019208 File Offset: 0x00017408
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Bullet component = collision.GetComponent<Bullet>();
		if (component != null)
		{
			component.BulletDamageIntensify += this.IntensifyValue;
		}
	}

	// Token: 0x040004DB RID: 1243
	public float IntensifyValue;
}
