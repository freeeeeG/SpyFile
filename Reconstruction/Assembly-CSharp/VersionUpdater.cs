using System;
using TMPro;
using UnityEngine;

// Token: 0x020002A0 RID: 672
public class VersionUpdater : MonoBehaviour
{
	// Token: 0x06001078 RID: 4216 RVA: 0x0002D58C File Offset: 0x0002B78C
	private void Start()
	{
		base.GetComponent<TextMeshProUGUI>().text = "V" + Application.version;
	}
}
