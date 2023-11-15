using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class NotificationOnDisable : MonoBehaviour
{
	// Token: 0x060003F4 RID: 1012 RVA: 0x000151C6 File Offset: 0x000133C6
	private void Start()
	{
		this._isInit = true;
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x000151CF File Offset: 0x000133CF
	private void OnDisable()
	{
		if (this._isInit)
		{
			this.PostNotification(this.notification, base.gameObject);
		}
	}

	// Token: 0x040001E6 RID: 486
	[SerializeField]
	private string notification;

	// Token: 0x040001E7 RID: 487
	private bool _isInit;
}
