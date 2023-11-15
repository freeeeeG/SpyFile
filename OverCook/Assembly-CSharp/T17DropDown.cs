using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B69 RID: 2921
[AddComponentMenu("T17_UI/Dropdown", 35)]
[RequireComponent(typeof(RectTransform))]
public class T17DropDown : Dropdown, IT17EventHelper
{
	// Token: 0x06003B6C RID: 15212 RVA: 0x0011ACDF File Offset: 0x001190DF
	public T17EventSystem GetDomain()
	{
		return null;
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x0011ACE2 File Offset: 0x001190E2
	public GameObject GetGameobject()
	{
		return base.gameObject;
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x0011ACEA File Offset: 0x001190EA
	public void SetEventSystem(T17EventSystem gamersEventSystem = null)
	{
	}
}
