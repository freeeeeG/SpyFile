using System;
using Steamworks;
using UnityEngine;

// Token: 0x02000753 RID: 1875
public class SteamSaveManager : PCSaveManager
{
	// Token: 0x0600240C RID: 9228 RVA: 0x000AACF2 File Offset: 0x000A90F2
	protected override string GetSaveDirectory()
	{
		return Application.persistentDataPath + "/" + this.GetUserSaveDirectory() + "/";
	}

	// Token: 0x0600240D RID: 9229 RVA: 0x000AAD10 File Offset: 0x000A9110
	private string GetUserSaveDirectory()
	{
		return SteamUser.GetSteamID().ToString();
	}
}
