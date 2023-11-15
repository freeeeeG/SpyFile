using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class KillCollider : MonoBehaviour
{
	// Token: 0x060005B8 RID: 1464 RVA: 0x0002ABA3 File Offset: 0x00028FA3
	private void OnCollisionEnter(Collision collision)
	{
		UnityEngine.Object.Destroy(collision.gameObject);
	}
}
