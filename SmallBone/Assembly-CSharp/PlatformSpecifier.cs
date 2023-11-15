using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class PlatformSpecifier : MonoBehaviour
{
	// Token: 0x06000201 RID: 513 RVA: 0x00008E08 File Offset: 0x00007008
	private void Awake()
	{
		if (Application.isConsolePlatform && !this._console)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!Application.isConsolePlatform && !this._standalone)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x040001BA RID: 442
	[SerializeField]
	private bool _standalone = true;

	// Token: 0x040001BB RID: 443
	[SerializeField]
	private bool _console = true;
}
