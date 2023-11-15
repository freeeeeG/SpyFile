using System;
using UnityEngine;

// Token: 0x02000A75 RID: 2677
public class LogicModeUI : ScriptableObject
{
	// Token: 0x04003533 RID: 13619
	[Header("Base Assets")]
	public Sprite inputSprite;

	// Token: 0x04003534 RID: 13620
	public Sprite outputSprite;

	// Token: 0x04003535 RID: 13621
	public Sprite resetSprite;

	// Token: 0x04003536 RID: 13622
	public GameObject prefab;

	// Token: 0x04003537 RID: 13623
	public GameObject ribbonInputPrefab;

	// Token: 0x04003538 RID: 13624
	public GameObject ribbonOutputPrefab;

	// Token: 0x04003539 RID: 13625
	public GameObject controlInputPrefab;

	// Token: 0x0400353A RID: 13626
	[Header("Colouring")]
	public Color32 colourOn = new Color32(0, byte.MaxValue, 0, 0);

	// Token: 0x0400353B RID: 13627
	public Color32 colourOff = new Color32(byte.MaxValue, 0, 0, 0);

	// Token: 0x0400353C RID: 13628
	public Color32 colourDisconnected = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x0400353D RID: 13629
	public Color32 colourOnProtanopia = new Color32(179, 204, 0, 0);

	// Token: 0x0400353E RID: 13630
	public Color32 colourOffProtanopia = new Color32(166, 51, 102, 0);

	// Token: 0x0400353F RID: 13631
	public Color32 colourOnDeuteranopia = new Color32(128, 0, 128, 0);

	// Token: 0x04003540 RID: 13632
	public Color32 colourOffDeuteranopia = new Color32(byte.MaxValue, 153, 0, 0);

	// Token: 0x04003541 RID: 13633
	public Color32 colourOnTritanopia = new Color32(51, 102, byte.MaxValue, 0);

	// Token: 0x04003542 RID: 13634
	public Color32 colourOffTritanopia = new Color32(byte.MaxValue, 153, 0, 0);
}
