using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class AssetManager : MonoBehaviour
{
	// Token: 0x06000177 RID: 375 RVA: 0x0000A25B File Offset: 0x0000845B
	private void Awake()
	{
		AssetManager.inst = this;
		Object.DontDestroyOnLoad(base.gameObject);
		this.soundEffects.InitCanPlay();
	}

	// Token: 0x06000178 RID: 376 RVA: 0x000051D0 File Offset: 0x000033D0
	private void OnEnable()
	{
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000A279 File Offset: 0x00008479
	private void OnDestroy()
	{
		this.soundEffects.InitCanPlay();
	}

	// Token: 0x04000189 RID: 393
	public static AssetManager inst;

	// Token: 0x0400018A RID: 394
	public DataBase dataBase;

	// Token: 0x0400018B RID: 395
	public GameParameters gameParameters;

	// Token: 0x0400018C RID: 396
	public ResourceLibrary resourceLibrary;

	// Token: 0x0400018D RID: 397
	public SoundEffects soundEffects;

	// Token: 0x0400018E RID: 398
	public UI_Setting UI_Setting;

	// Token: 0x0400018F RID: 399
	public LanguageText[] languageTexts;
}
