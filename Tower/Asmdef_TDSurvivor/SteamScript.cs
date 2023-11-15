using System;
using Steamworks;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class SteamScript : MonoBehaviour
{
	// Token: 0x06000539 RID: 1337 RVA: 0x00015006 File Offset: 0x00013206
	private void Start()
	{
		if (SteamManager.Initialized)
		{
			Debug.Log(SteamFriends.GetPersonaName());
		}
	}
}
