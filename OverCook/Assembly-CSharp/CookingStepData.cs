using System;
using UnityEngine;

// Token: 0x0200041B RID: 1051
[Serializable]
public class CookingStepData : ScriptableObject
{
	// Token: 0x04000EF8 RID: 3832
	public SubTexture2D m_icon;

	// Token: 0x04000EF9 RID: 3833
	public Sprite m_iconSprite;

	// Token: 0x04000EFA RID: 3834
	public GameLoopingAudioTag m_sizzleSound;

	// Token: 0x04000EFB RID: 3835
	public GameOneShotAudioTag m_addToSound;

	// Token: 0x04000EFC RID: 3836
	[SelfAssignID(true)]
	public int m_uID;
}
