using System;

// Token: 0x02000473 RID: 1139
public class ElementsAudio
{
	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060018E1 RID: 6369 RVA: 0x00081BCE File Offset: 0x0007FDCE
	public static ElementsAudio Instance
	{
		get
		{
			if (ElementsAudio._instance == null)
			{
				ElementsAudio._instance = new ElementsAudio();
			}
			return ElementsAudio._instance;
		}
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x00081BE6 File Offset: 0x0007FDE6
	public void LoadData(ElementsAudio.ElementAudioConfig[] elements_audio_configs)
	{
		this.elementAudioConfigs = elements_audio_configs;
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x00081BF0 File Offset: 0x0007FDF0
	public ElementsAudio.ElementAudioConfig GetConfigForElement(SimHashes id)
	{
		if (this.elementAudioConfigs != null)
		{
			for (int i = 0; i < this.elementAudioConfigs.Length; i++)
			{
				if (this.elementAudioConfigs[i].elementID == id)
				{
					return this.elementAudioConfigs[i];
				}
			}
		}
		return null;
	}

	// Token: 0x04000DBE RID: 3518
	private static ElementsAudio _instance;

	// Token: 0x04000DBF RID: 3519
	private ElementsAudio.ElementAudioConfig[] elementAudioConfigs;

	// Token: 0x020010ED RID: 4333
	public class ElementAudioConfig : Resource
	{
		// Token: 0x04005AB6 RID: 23222
		public SimHashes elementID;

		// Token: 0x04005AB7 RID: 23223
		public AmbienceType ambienceType = AmbienceType.None;

		// Token: 0x04005AB8 RID: 23224
		public SolidAmbienceType solidAmbienceType = SolidAmbienceType.None;

		// Token: 0x04005AB9 RID: 23225
		public string miningSound = "";

		// Token: 0x04005ABA RID: 23226
		public string miningBreakSound = "";

		// Token: 0x04005ABB RID: 23227
		public string oreBumpSound = "";

		// Token: 0x04005ABC RID: 23228
		public string floorEventAudioCategory = "";

		// Token: 0x04005ABD RID: 23229
		public string creatureChewSound = "";
	}
}
