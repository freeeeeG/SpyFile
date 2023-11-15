using System;
using UnityEngine;

// Token: 0x02000A63 RID: 2659
public class StoryLevelConfig : LevelConfigBase
{
	// Token: 0x04002A14 RID: 10772
	[Header("Story Text")]
	public bool AutoStartDialogue;

	// Token: 0x04002A15 RID: 10773
	public string[] OnionScript = new string[]
	{
		"<Placeholder Dialog>"
	};

	// Token: 0x04002A16 RID: 10774
	public string[] OnionReturnScript = new string[]
	{
		"<Placeholder Dialog>"
	};
}
