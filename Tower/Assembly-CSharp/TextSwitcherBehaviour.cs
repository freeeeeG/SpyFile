using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000011 RID: 17
[Serializable]
public class TextSwitcherBehaviour : PlayableBehaviour
{
	// Token: 0x0400002A RID: 42
	public Color color = Color.white;

	// Token: 0x0400002B RID: 43
	public int fontSize = 14;

	// Token: 0x0400002C RID: 44
	public string text;
}
