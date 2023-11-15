using System;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class Panel_ShanguoDemo : MonoBehaviour
{
	// Token: 0x0600067F RID: 1663 RVA: 0x000252A3 File Offset: 0x000234A3
	private void Start()
	{
		if (!GameParameters.Inst.ifDemo)
		{
			this.Close();
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}
}
