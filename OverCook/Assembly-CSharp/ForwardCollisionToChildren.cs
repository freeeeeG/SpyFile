using System;
using UnityEngine;

// Token: 0x02000134 RID: 308
[AddComponentMenu("Scripts/Core/Components/ForwardCollisionToChildren")]
public class ForwardCollisionToChildren : MonoBehaviour
{
	// Token: 0x060005A3 RID: 1443 RVA: 0x0002AA34 File Offset: 0x00028E34
	private void OnCollisionEnter(Collision other)
	{
		for (int i = 0; i < base.gameObject.transform.childCount; i++)
		{
			Transform child = base.gameObject.transform.GetChild(i);
			child.gameObject.SendMessage("OnCollisionEnter", other, SendMessageOptions.DontRequireReceiver);
		}
	}
}
