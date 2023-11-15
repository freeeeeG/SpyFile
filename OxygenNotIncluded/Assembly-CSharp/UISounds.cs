using System;
using UnityEngine;

// Token: 0x02000A0C RID: 2572
[AddComponentMenu("KMonoBehaviour/scripts/UISounds")]
public class UISounds : KMonoBehaviour
{
	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x06004CF2 RID: 19698 RVA: 0x001AF52E File Offset: 0x001AD72E
	// (set) Token: 0x06004CF3 RID: 19699 RVA: 0x001AF535 File Offset: 0x001AD735
	public static UISounds Instance { get; private set; }

	// Token: 0x06004CF4 RID: 19700 RVA: 0x001AF53D File Offset: 0x001AD73D
	public static void DestroyInstance()
	{
		UISounds.Instance = null;
	}

	// Token: 0x06004CF5 RID: 19701 RVA: 0x001AF545 File Offset: 0x001AD745
	protected override void OnPrefabInit()
	{
		UISounds.Instance = this;
	}

	// Token: 0x06004CF6 RID: 19702 RVA: 0x001AF54D File Offset: 0x001AD74D
	public static void PlaySound(UISounds.Sound sound)
	{
		UISounds.Instance.PlaySoundInternal(sound);
	}

	// Token: 0x06004CF7 RID: 19703 RVA: 0x001AF55C File Offset: 0x001AD75C
	private void PlaySoundInternal(UISounds.Sound sound)
	{
		for (int i = 0; i < this.soundData.Length; i++)
		{
			if (this.soundData[i].sound == sound)
			{
				if (this.logSounds)
				{
					DebugUtil.LogArgs(new object[]
					{
						"Play sound",
						this.soundData[i].name
					});
				}
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.soundData[i].name, false));
			}
		}
	}

	// Token: 0x04003230 RID: 12848
	[SerializeField]
	private bool logSounds;

	// Token: 0x04003231 RID: 12849
	[SerializeField]
	private UISounds.SoundData[] soundData;

	// Token: 0x02001896 RID: 6294
	public enum Sound
	{
		// Token: 0x04007262 RID: 29282
		NegativeNotification,
		// Token: 0x04007263 RID: 29283
		PositiveNotification,
		// Token: 0x04007264 RID: 29284
		Select,
		// Token: 0x04007265 RID: 29285
		Negative,
		// Token: 0x04007266 RID: 29286
		Back,
		// Token: 0x04007267 RID: 29287
		ClickObject,
		// Token: 0x04007268 RID: 29288
		HUD_Mouseover,
		// Token: 0x04007269 RID: 29289
		Object_Mouseover,
		// Token: 0x0400726A RID: 29290
		ClickHUD
	}

	// Token: 0x02001897 RID: 6295
	[Serializable]
	private struct SoundData
	{
		// Token: 0x0400726B RID: 29291
		public string name;

		// Token: 0x0400726C RID: 29292
		public UISounds.Sound sound;
	}
}
