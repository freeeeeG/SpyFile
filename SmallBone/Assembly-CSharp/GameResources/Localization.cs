using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data;
using UnityEngine;

namespace GameResources
{
	// Token: 0x02000184 RID: 388
	public static class Localization
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000863 RID: 2147 RVA: 0x000185CC File Offset: 0x000167CC
		// (remove) Token: 0x06000864 RID: 2148 RVA: 0x00018600 File Offset: 0x00016800
		public static event Action OnChange;

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x00018633 File Offset: 0x00016833
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x0001863A File Offset: 0x0001683A
		public static ReadOnlyCollection<string> nativeNames { get; private set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x00018642 File Offset: 0x00016842
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x00018649 File Offset: 0x00016849
		private static int _current
		{
			get
			{
				return GameData.Settings.language;
			}
			set
			{
				GameData.Settings.language = value;
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00018654 File Offset: 0x00016854
		public static void Initialize()
		{
			LocalizationStringResource instance = LocalizationStringResource.instance;
			Localization.nativeNames = Array.AsReadOnly<string>(instance.GetStrings(Localization.Key.languageNative.hashcode));
			string[] strings = instance.GetStrings(Localization.Key.languageNumber.hashcode);
			for (int i = 0; i < strings.Length; i++)
			{
				int num;
				if (int.TryParse(strings[i], out num))
				{
					Localization.languangeNumberToIndex.Array[num] = i;
				}
			}
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x000186B8 File Offset: 0x000168B8
		public static void ValidateLanguage()
		{
			string text = Application.systemLanguage.ToString();
			if (Localization._current < 0 || Localization._current >= Localization.systemLanguages.Length)
			{
				Localization._current = Convert.ToInt32(Localization.Language.English);
				for (int i = 0; i < Localization.systemLanguages.Length; i++)
				{
					if (Localization.systemLanguages[i].Equals(text, StringComparison.OrdinalIgnoreCase))
					{
						Debug.Log("System language is automatically detected : " + text);
						Localization._current = i;
						return;
					}
				}
			}
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00018738 File Offset: 0x00016938
		public static bool TryGetLocalizedString(Localization.Key key, out string @string)
		{
			return Localization.TryGetLocalizedStringAt(key.hashcode, Localization._current, out @string);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0001874B File Offset: 0x0001694B
		public static bool TryGetLocalizedString(string key, out string @string)
		{
			return Localization.TryGetLocalizedStringAt(StringComparer.OrdinalIgnoreCase.GetHashCode(key), Localization._current, out @string);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00018763 File Offset: 0x00016963
		public static string GetLocalizedString(Localization.Key key)
		{
			return Localization.GetLocalizedStringAt(key.hashcode, Localization._current);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00018778 File Offset: 0x00016978
		public static bool TryGetLocalizedStringArray(string key, out string[] strings)
		{
			List<string> list = new List<string>();
			string text;
			if (Localization.TryGetLocalizedString(key, out text))
			{
				strings = new string[]
				{
					text
				};
				return true;
			}
			int num = 0;
			while (Localization.TryGetLocalizedString(string.Format("{0}/{1}", key, num), out text))
			{
				list.Add(text);
				num++;
			}
			if (num == 0)
			{
				strings = null;
				return false;
			}
			strings = list.ToArray();
			return true;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x000187E0 File Offset: 0x000169E0
		public static string[] GetLocalizedStringArray(string key)
		{
			List<string> list = new List<string>();
			string text;
			if (Localization.TryGetLocalizedString(key, out text))
			{
				return new string[]
				{
					text
				};
			}
			int num = 0;
			while (Localization.TryGetLocalizedString(string.Format("{0}/{1}", key, num), out text))
			{
				list.Add(text);
				num++;
			}
			return list.ToArray();
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00018838 File Offset: 0x00016A38
		public static string[][] GetLocalizedStringArrays(string key)
		{
			List<string[]> list = new List<string[]>();
			int num = 0;
			string[] item;
			while (Localization.TryGetLocalizedStringArray(string.Format("{0}/{1}", key, num), out item))
			{
				list.Add(item);
				num++;
			}
			return list.ToArray();
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0001887C File Offset: 0x00016A7C
		public static string[] GetLocalizedStrings(params Localization.Key[] keys)
		{
			string[] array = new string[keys.Length];
			for (int i = 0; i < keys.Length; i++)
			{
				array[i] = Localization.GetLocalizedStringAt(keys[i].hashcode, Localization._current);
			}
			return array;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x000188B8 File Offset: 0x00016AB8
		public static string[] GetLocalizedStrings(params string[] keys)
		{
			string[] array = new string[keys.Length];
			for (int i = 0; i < keys.Length; i++)
			{
				array[i] = Localization.GetLocalizedStringAt(StringComparer.OrdinalIgnoreCase.GetHashCode(keys[i]), Localization._current);
			}
			return array;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000188F7 File Offset: 0x00016AF7
		public static string GetLocalizedString(string key)
		{
			return Localization.GetLocalizedStringAt(StringComparer.OrdinalIgnoreCase.GetHashCode(key), Localization._current);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00018910 File Offset: 0x00016B10
		private static string GetLocalizedStringAt(int key, int index)
		{
			string result;
			Localization.TryGetLocalizedStringAt(key, index, out result);
			return result;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00018928 File Offset: 0x00016B28
		private static bool TryGetLocalizedStringAt(int key, int number, out string @string)
		{
			if (LocalizationStringResource.instance == null)
			{
				@string = string.Empty;
				return false;
			}
			string[] array;
			if (LocalizationStringResource.instance.TryGetStrings(key, out array))
			{
				int num = Localization.languangeNumberToIndex.Array[number];
				@string = array[num];
				if (string.IsNullOrWhiteSpace(@string))
				{
					@string = array[0];
				}
				return true;
			}
			@string = string.Empty;
			return false;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00018984 File Offset: 0x00016B84
		public static void Change(int number)
		{
			Localization._current = Localization.languangeNumberToIndex.Array[number];
			Action onChange = Localization.OnChange;
			if (onChange == null)
			{
				return;
			}
			onChange();
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x000189A6 File Offset: 0x00016BA6
		public static void Change(Localization.Language language)
		{
			Localization.Change(Convert.ToInt32(language));
		}

		// Token: 0x040006A7 RID: 1703
		private static string[] systemLanguages = new string[]
		{
			"Korean",
			"English",
			"Japanese",
			"ChineseSimplified",
			"ChineseTraditional",
			"German",
			"Spanish",
			"Portuguese",
			"Russian",
			"Polish",
			"French"
		};

		// Token: 0x040006A8 RID: 1704
		public const string language = "language";

		// Token: 0x040006A9 RID: 1705
		public const string label = "label";

		// Token: 0x040006AA RID: 1706
		public const string name = "name";

		// Token: 0x040006AB RID: 1707
		public const string desc = "desc";

		// Token: 0x040006AC RID: 1708
		public const string flavor = "flavor";

		// Token: 0x040006AD RID: 1709
		public const string active = "active";

		// Token: 0x040006AE RID: 1710
		public const string skill = "skill";

		// Token: 0x040006B1 RID: 1713
		public static readonly EnumArray<Localization.Language, int> languangeNumberToIndex = new EnumArray<Localization.Language, int>();

		// Token: 0x040006B2 RID: 1714
		public static readonly EnumArray<Localization.Language, int> displayOrderToIndex = new EnumArray<Localization.Language, int>();

		// Token: 0x02000185 RID: 389
		public enum Language
		{
			// Token: 0x040006B4 RID: 1716
			Korean,
			// Token: 0x040006B5 RID: 1717
			English,
			// Token: 0x040006B6 RID: 1718
			Japanese,
			// Token: 0x040006B7 RID: 1719
			Chinese_Simplifed,
			// Token: 0x040006B8 RID: 1720
			Chinese_Traditional,
			// Token: 0x040006B9 RID: 1721
			German,
			// Token: 0x040006BA RID: 1722
			Spanish,
			// Token: 0x040006BB RID: 1723
			Portguese,
			// Token: 0x040006BC RID: 1724
			Russian,
			// Token: 0x040006BD RID: 1725
			Polish,
			// Token: 0x040006BE RID: 1726
			French
		}

		// Token: 0x02000186 RID: 390
		public sealed class Key : IEquatable<Localization.Key>
		{
			// Token: 0x06000879 RID: 2169 RVA: 0x00018A3F File Offset: 0x00016C3F
			private Key(string key)
			{
				this.key = key;
				this.hashcode = StringComparer.OrdinalIgnoreCase.GetHashCode(key);
			}

			// Token: 0x0600087A RID: 2170 RVA: 0x00018A5F File Offset: 0x00016C5F
			public override int GetHashCode()
			{
				return this.hashcode;
			}

			// Token: 0x0600087B RID: 2171 RVA: 0x00018A68 File Offset: 0x00016C68
			public bool Equals(Localization.Key other)
			{
				return this.hashcode.Equals(other.hashcode);
			}

			// Token: 0x040006BF RID: 1727
			public static readonly Localization.Key languageCode = new Localization.Key("language/code");

			// Token: 0x040006C0 RID: 1728
			public static readonly Localization.Key languageSystem = new Localization.Key("language/system");

			// Token: 0x040006C1 RID: 1729
			public static readonly Localization.Key languageNative = new Localization.Key("language/native");

			// Token: 0x040006C2 RID: 1730
			public static readonly Localization.Key languageNumber = new Localization.Key("language/number");

			// Token: 0x040006C3 RID: 1731
			public static readonly Localization.Key languageDisplayOrder = new Localization.Key("language/displayOrder");

			// Token: 0x040006C4 RID: 1732
			public static readonly Localization.Key titleFont = new Localization.Key("font/title");

			// Token: 0x040006C5 RID: 1733
			public static readonly Localization.Key bodyFont = new Localization.Key("font/body");

			// Token: 0x040006C6 RID: 1734
			public static readonly Localization.Key colorClose = new Localization.Key("cc");

			// Token: 0x040006C7 RID: 1735
			public static readonly Localization.Key colorOpenGold = new Localization.Key("cogold");

			// Token: 0x040006C8 RID: 1736
			public static readonly Localization.Key colorOpenDarkQuartz = new Localization.Key("codark");

			// Token: 0x040006C9 RID: 1737
			public static readonly Localization.Key colorOpenBone = new Localization.Key("cobone");

			// Token: 0x040006CA RID: 1738
			public readonly string key;

			// Token: 0x040006CB RID: 1739
			public readonly int hashcode;
		}
	}
}
