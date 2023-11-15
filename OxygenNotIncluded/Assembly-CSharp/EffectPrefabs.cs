using System;
using UnityEngine;

// Token: 0x0200077C RID: 1916
public class EffectPrefabs : MonoBehaviour
{
	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x0600350D RID: 13581 RVA: 0x0011FB51 File Offset: 0x0011DD51
	// (set) Token: 0x0600350E RID: 13582 RVA: 0x0011FB58 File Offset: 0x0011DD58
	public static EffectPrefabs Instance { get; private set; }

	// Token: 0x0600350F RID: 13583 RVA: 0x0011FB60 File Offset: 0x0011DD60
	private void Awake()
	{
		EffectPrefabs.Instance = this;
	}

	// Token: 0x0400201E RID: 8222
	public GameObject DreamBubble;

	// Token: 0x0400201F RID: 8223
	public GameObject ThoughtBubble;

	// Token: 0x04002020 RID: 8224
	public GameObject ThoughtBubbleConvo;

	// Token: 0x04002021 RID: 8225
	public GameObject MeteorBackground;

	// Token: 0x04002022 RID: 8226
	public GameObject SparkleStreakFX;

	// Token: 0x04002023 RID: 8227
	public GameObject HappySingerFX;

	// Token: 0x04002024 RID: 8228
	public GameObject HugFrenzyFX;

	// Token: 0x04002025 RID: 8229
	public GameObject GameplayEventDisplay;

	// Token: 0x04002026 RID: 8230
	public GameObject OpenTemporalTearBeam;

	// Token: 0x04002027 RID: 8231
	public GameObject MissileSmokeTrailFX;
}
