using System;
using UnityEngine;

// Token: 0x0200068D RID: 1677
[Serializable]
public class GameDebugConfig : ScriptableObject
{
	// Token: 0x04001886 RID: 6278
	public float m_timeScale = 1f;

	// Token: 0x04001887 RID: 6279
	public bool m_immortal;

	// Token: 0x04001888 RID: 6280
	public bool m_skipTutorial;

	// Token: 0x04001889 RID: 6281
	public bool m_skipPlayerJoining;

	// Token: 0x0400188A RID: 6282
	public GameDebugConfig.SkipPlayerJoiningConfig m_skipJoiningConfig;

	// Token: 0x0400188B RID: 6283
	public bool m_infiniteChopping;

	// Token: 0x0400188C RID: 6284
	public bool m_skipCinematics;

	// Token: 0x0400188D RID: 6285
	public bool m_supressCompetitveMode;

	// Token: 0x0400188E RID: 6286
	public GameDebugConfig.KeyboardType m_keyboardType;

	// Token: 0x0400188F RID: 6287
	public GameDebugConfig.NXDemoSplitting NXDemoSplittingMode;

	// Token: 0x0200068E RID: 1678
	public enum NXDemoSplitting
	{
		// Token: 0x04001891 RID: 6289
		Default,
		// Token: 0x04001892 RID: 6290
		LockedIntoJoycons
	}

	// Token: 0x0200068F RID: 1679
	public enum KeyboardType
	{
		// Token: 0x04001894 RID: 6292
		Actual,
		// Token: 0x04001895 RID: 6293
		DebugForPads
	}

	// Token: 0x02000690 RID: 1680
	public enum SkipPlayerJoiningConfig
	{
		// Token: 0x04001897 RID: 6295
		ControllerEach,
		// Token: 0x04001898 RID: 6296
		TwoSidedPads
	}
}
