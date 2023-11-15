using System;
using System.Collections.Generic;

namespace flanne
{
	// Token: 0x02000072 RID: 114
	public class LocalizationSystem
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x00018434 File Offset: 0x00016634
		public static void Init()
		{
			LocalizationSystem.csvLoader = new CSVLoader();
			LocalizationSystem.csvLoader.LoadCSV();
			LocalizationSystem.UpdateDictionaries();
			LocalizationSystem.isInit = true;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00018458 File Offset: 0x00016658
		public static void UpdateDictionaries()
		{
			LocalizationSystem.localizedEN = LocalizationSystem.csvLoader.GetDictionaryValues("en");
			LocalizationSystem.localizedJP = LocalizationSystem.csvLoader.GetDictionaryValues("jp");
			LocalizationSystem.localizedCH = LocalizationSystem.csvLoader.GetDictionaryValues("ch");
			LocalizationSystem.localizedBR = LocalizationSystem.csvLoader.GetDictionaryValues("br");
			LocalizationSystem.localizedTC = LocalizationSystem.csvLoader.GetDictionaryValues("tc");
			LocalizationSystem.localizedRU = LocalizationSystem.csvLoader.GetDictionaryValues("ru");
			LocalizationSystem.localizedSP = LocalizationSystem.csvLoader.GetDictionaryValues("sp");
			LocalizationSystem.localizedGR = LocalizationSystem.csvLoader.GetDictionaryValues("gr");
			LocalizationSystem.localizedPL = LocalizationSystem.csvLoader.GetDictionaryValues("pl");
			LocalizationSystem.localizedIT = LocalizationSystem.csvLoader.GetDictionaryValues("it");
			LocalizationSystem.localizedTR = LocalizationSystem.csvLoader.GetDictionaryValues("tr");
			LocalizationSystem.localizedFR = LocalizationSystem.csvLoader.GetDictionaryValues("fr");
			LocalizationSystem.localizedKR = LocalizationSystem.csvLoader.GetDictionaryValues("kr");
			LocalizationSystem.localizedHU = LocalizationSystem.csvLoader.GetDictionaryValues("hu");
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001857D File Offset: 0x0001677D
		public static Dictionary<string, string> GetDictionaryForEditor()
		{
			if (!LocalizationSystem.isInit)
			{
				LocalizationSystem.Init();
			}
			return LocalizationSystem.localizedEN;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00018590 File Offset: 0x00016790
		public static string GetLocalizedValue(string key)
		{
			if (!LocalizationSystem.isInit)
			{
				LocalizationSystem.Init();
			}
			string result = key;
			switch (LocalizationSystem.language)
			{
			case LocalizationSystem.Language.English:
				LocalizationSystem.localizedEN.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.Japanese:
				LocalizationSystem.localizedJP.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.Chinese:
				LocalizationSystem.localizedCH.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.BrazilPortuguese:
				LocalizationSystem.localizedBR.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.TChinese:
				LocalizationSystem.localizedTC.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.Russian:
				LocalizationSystem.localizedRU.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.Spanish:
				LocalizationSystem.localizedSP.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.German:
				LocalizationSystem.localizedGR.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.Polish:
				LocalizationSystem.localizedPL.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.Italian:
				LocalizationSystem.localizedIT.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.Turkish:
				LocalizationSystem.localizedTR.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.French:
				LocalizationSystem.localizedFR.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.Korean:
				LocalizationSystem.localizedKR.TryGetValue(key, out result);
				break;
			case LocalizationSystem.Language.Hungarian:
				LocalizationSystem.localizedHU.TryGetValue(key, out result);
				break;
			}
			return result;
		}

		// Token: 0x040002BC RID: 700
		public static LocalizationSystem.Language language;

		// Token: 0x040002BD RID: 701
		private static Dictionary<string, string> localizedEN;

		// Token: 0x040002BE RID: 702
		private static Dictionary<string, string> localizedJP;

		// Token: 0x040002BF RID: 703
		private static Dictionary<string, string> localizedCH;

		// Token: 0x040002C0 RID: 704
		private static Dictionary<string, string> localizedBR;

		// Token: 0x040002C1 RID: 705
		private static Dictionary<string, string> localizedTC;

		// Token: 0x040002C2 RID: 706
		private static Dictionary<string, string> localizedRU;

		// Token: 0x040002C3 RID: 707
		private static Dictionary<string, string> localizedSP;

		// Token: 0x040002C4 RID: 708
		private static Dictionary<string, string> localizedGR;

		// Token: 0x040002C5 RID: 709
		private static Dictionary<string, string> localizedPL;

		// Token: 0x040002C6 RID: 710
		private static Dictionary<string, string> localizedIT;

		// Token: 0x040002C7 RID: 711
		private static Dictionary<string, string> localizedTR;

		// Token: 0x040002C8 RID: 712
		private static Dictionary<string, string> localizedFR;

		// Token: 0x040002C9 RID: 713
		private static Dictionary<string, string> localizedKR;

		// Token: 0x040002CA RID: 714
		private static Dictionary<string, string> localizedHU;

		// Token: 0x040002CB RID: 715
		public static bool isInit;

		// Token: 0x040002CC RID: 716
		public static CSVLoader csvLoader;

		// Token: 0x020002A0 RID: 672
		public enum Language
		{
			// Token: 0x04000A63 RID: 2659
			English,
			// Token: 0x04000A64 RID: 2660
			Japanese,
			// Token: 0x04000A65 RID: 2661
			Chinese,
			// Token: 0x04000A66 RID: 2662
			BrazilPortuguese,
			// Token: 0x04000A67 RID: 2663
			TChinese,
			// Token: 0x04000A68 RID: 2664
			Russian,
			// Token: 0x04000A69 RID: 2665
			Spanish,
			// Token: 0x04000A6A RID: 2666
			German,
			// Token: 0x04000A6B RID: 2667
			Polish,
			// Token: 0x04000A6C RID: 2668
			Italian,
			// Token: 0x04000A6D RID: 2669
			Turkish,
			// Token: 0x04000A6E RID: 2670
			French,
			// Token: 0x04000A6F RID: 2671
			Korean,
			// Token: 0x04000A70 RID: 2672
			Hungarian
		}
	}
}
