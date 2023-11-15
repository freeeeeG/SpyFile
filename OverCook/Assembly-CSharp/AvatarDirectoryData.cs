using System;
using UnityEngine;

// Token: 0x0200063F RID: 1599
[Serializable]
public class AvatarDirectoryData : ScriptableObject
{
	// Token: 0x04001767 RID: 5991
	public int DirectoryID;

	// Token: 0x04001768 RID: 5992
	public ChefAvatarData[] Avatars = new ChefAvatarData[0];

	// Token: 0x04001769 RID: 5993
	public ChefColourData[] Colours = new ChefColourData[0];
}
