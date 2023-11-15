using System;
using Steamworks;
using UnityEngine;

// Token: 0x02000AE8 RID: 2792
public class HideInBigPicture : MonoBehaviour
{
	// Token: 0x06003894 RID: 14484 RVA: 0x0010AFD5 File Offset: 0x001093D5
	protected void Awake()
	{
		if (SteamUtils.IsSteamInBigPictureMode())
		{
			base.gameObject.SetActive(false);
		}
	}
}
