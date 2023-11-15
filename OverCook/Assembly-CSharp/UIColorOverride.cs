using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A4 RID: 420
[ExecuteInEditMode]
public class UIColorOverride : MonoBehaviour
{
	// Token: 0x0600071B RID: 1819 RVA: 0x0002E5C4 File Offset: 0x0002C9C4
	private void LateUpdate()
	{
		foreach (Image image in base.gameObject.RequestComponentsRecursive<Image>())
		{
			image.color = this.m_color;
		}
	}

	// Token: 0x040005E5 RID: 1509
	[SerializeField]
	private Color m_color = Color.white;
}
