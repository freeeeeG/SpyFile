using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using CutScenes;
using GameResources;
using Hardmode.Darktech;
using Level;
using Level.Npc;
using Level.Npc.FieldNpcs;
using Platforms;
using Singletons;
using SkulStories;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Data
{
	// Token: 0x0200029B RID: 667
	public static class GameData
	{
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x0002A388 File Offset: 0x00028588
		// (set) Token: 0x06000CFA RID: 3322 RVA: 0x0002A394 File Offset: 0x00028594
		public static int version
		{
			get
			{
				return GameData._version.value;
			}
			set
			{
				GameData._version.value = value;
			}
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0002A3A4 File Offset: 0x000285A4
		public static void Initialize()
		{
			GameData._version = new IntData("version", true);
			int value = GameData._version.value;
			GameData._version.value = 9;
			GameData.Generic.instance.Initialize();
			GameData.Currency.Initialize();
			GameData.Progress.instance.Initialize();
			GameData.Settings.instance.Initialize();
			GameData.Save.instance.Initialize();
			GameData.Buff.Initialize(Enum.GetValues(typeof(SavableAbilityManager.Name)).Length);
			GameData.HardmodeProgress.instance.Initialize();
			if (value > 0 && value <= 2)
			{
				GameData.Progress.witch.RefundFormer();
			}
			if (value > 0 && value <= 3)
			{
				GameData.Settings.particleQuality++;
			}
			if (value > 0 && value <= 4)
			{
				if (GameData.Settings.language == 4)
				{
					GameData.Settings.language = 5;
				}
				else if (GameData.Settings.language == 5)
				{
					GameData.Settings.language = 4;
				}
			}
			if (value == 5 && Application.systemLanguage == SystemLanguage.German)
			{
				GameData.Settings.language = 5;
			}
			if (value < 9)
			{
				GameData.Save.instance.ResetAll();
			}
		}

		// Token: 0x04000B24 RID: 2852
		private const int _currentVersion = 9;

		// Token: 0x04000B25 RID: 2853
		private static IntData _version;

		// Token: 0x0200029C RID: 668
		public class Buff
		{
			// Token: 0x170002B8 RID: 696
			// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0002A496 File Offset: 0x00028696
			// (set) Token: 0x06000CFD RID: 3325 RVA: 0x0002A4A3 File Offset: 0x000286A3
			public bool attached
			{
				get
				{
					return this._attached.value;
				}
				set
				{
					this._attached.value = value;
				}
			}

			// Token: 0x170002B9 RID: 697
			// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0002A4B1 File Offset: 0x000286B1
			// (set) Token: 0x06000CFF RID: 3327 RVA: 0x0002A4BE File Offset: 0x000286BE
			public float remainTime
			{
				get
				{
					return this._remainTime.value;
				}
				set
				{
					this._remainTime.value = value;
				}
			}

			// Token: 0x170002BA RID: 698
			// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0002A4CC File Offset: 0x000286CC
			// (set) Token: 0x06000D01 RID: 3329 RVA: 0x0002A4D9 File Offset: 0x000286D9
			public float stack
			{
				get
				{
					return this._stack.value;
				}
				set
				{
					this._stack.value = value;
				}
			}

			// Token: 0x06000D02 RID: 3330 RVA: 0x0002A4E7 File Offset: 0x000286E7
			public static GameData.Buff Get(int index)
			{
				return GameData.Buff._buffs[index];
			}

			// Token: 0x06000D03 RID: 3331 RVA: 0x0002A4E7 File Offset: 0x000286E7
			public static GameData.Buff Get(SavableAbilityManager.Name name)
			{
				return GameData.Buff._buffs[(int)name];
			}

			// Token: 0x06000D04 RID: 3332 RVA: 0x0002A4F4 File Offset: 0x000286F4
			public static void Initialize(int length)
			{
				GameData.Buff._buffs = new List<GameData.Buff>(length);
				for (int i = 0; i < length; i++)
				{
					GameData.Buff._buffs.Add(new GameData.Buff(i));
				}
			}

			// Token: 0x06000D05 RID: 3333 RVA: 0x0002A528 File Offset: 0x00028728
			public static void ResetAll()
			{
				foreach (GameData.Buff buff in GameData.Buff._buffs)
				{
					buff.Reset();
					buff.Save();
				}
			}

			// Token: 0x06000D06 RID: 3334 RVA: 0x0002A580 File Offset: 0x00028780
			private Buff(int index)
			{
				this._attached = new BoolData(string.Format("{0}/{1}/{2}", "Buff", index, "attached"), false);
				this._remainTime = new FloatData(string.Format("{0}/{1}/{2}", "Buff", index, "remainTime"), false);
				this._stack = new FloatData(string.Format("{0}/{1}/{2}", "Buff", index, "stack"), false);
			}

			// Token: 0x06000D07 RID: 3335 RVA: 0x0002A605 File Offset: 0x00028805
			public void Save()
			{
				this._attached.Save();
				this._remainTime.Save();
				this._stack.Save();
			}

			// Token: 0x06000D08 RID: 3336 RVA: 0x0002A628 File Offset: 0x00028828
			public void Reset()
			{
				this.attached = false;
				this.remainTime = 0f;
				this.stack = 0f;
			}

			// Token: 0x04000B26 RID: 2854
			private const string key = "Buff";

			// Token: 0x04000B27 RID: 2855
			private readonly BoolData _attached;

			// Token: 0x04000B28 RID: 2856
			private readonly FloatData _remainTime;

			// Token: 0x04000B29 RID: 2857
			private readonly FloatData _stack;

			// Token: 0x04000B2A RID: 2858
			private static List<GameData.Buff> _buffs;
		}

		// Token: 0x0200029D RID: 669
		public class Currency : GameData.IEditorDrawer
		{
			// Token: 0x06000D09 RID: 3337 RVA: 0x0002A648 File Offset: 0x00028848
			public static void Initialize()
			{
				GameData.Currency.gold = new GameData.Currency("gold", "FFDE37", "Others/Gold_Icon");
				GameData.Currency.darkQuartz = new GameData.Currency("darkQuartz", "b57dff", "Others/DarkQuartz_Icon");
				GameData.Currency.bone = new GameData.Currency("bone", "EEEEEE", "Others/BoneChip_Icon");
				GameData.Currency.heartQuartz = new GameData.Currency("heartQuartz", "AC00E4", "Others/DarkHeart_Icon");
				GameData.Currency.currencies = new EnumArray<GameData.Currency.Type, GameData.Currency>(new GameData.Currency[]
				{
					GameData.Currency.gold,
					GameData.Currency.darkQuartz,
					GameData.Currency.bone,
					GameData.Currency.heartQuartz
				});
			}

			// Token: 0x06000D0A RID: 3338 RVA: 0x0002A6E9 File Offset: 0x000288E9
			public static void ResetAll()
			{
				GameData.Currency.gold.ResetNonpermaAll();
				GameData.Currency.darkQuartz.ResetNonpermaAll();
				GameData.Currency.bone.ResetNonpermaAll();
				GameData.Currency.heartQuartz.ResetNonpermaAll();
				GameData.Currency.SaveAll();
			}

			// Token: 0x06000D0B RID: 3339 RVA: 0x0002A718 File Offset: 0x00028918
			public static void SaveAll()
			{
				GameData.Currency.gold.Save();
				GameData.Currency.darkQuartz.Save();
				GameData.Currency.bone.Save();
				GameData.Currency.heartQuartz.Save();
			}

			// Token: 0x14000013 RID: 19
			// (add) Token: 0x06000D0C RID: 3340 RVA: 0x0002A744 File Offset: 0x00028944
			// (remove) Token: 0x06000D0D RID: 3341 RVA: 0x0002A77C File Offset: 0x0002897C
			public event GameData.Currency.OnEarnDelegate onEarn;

			// Token: 0x14000014 RID: 20
			// (add) Token: 0x06000D0E RID: 3342 RVA: 0x0002A7B4 File Offset: 0x000289B4
			// (remove) Token: 0x06000D0F RID: 3343 RVA: 0x0002A7EC File Offset: 0x000289EC
			public event GameData.Currency.OnConsumeDelegate onConsume;

			// Token: 0x170002BB RID: 699
			// (get) Token: 0x06000D10 RID: 3344 RVA: 0x0002A821 File Offset: 0x00028A21
			// (set) Token: 0x06000D11 RID: 3345 RVA: 0x0002A82E File Offset: 0x00028A2E
			public int balance
			{
				get
				{
					return this._balance.value;
				}
				set
				{
					this._balance.value = value;
				}
			}

			// Token: 0x170002BC RID: 700
			// (get) Token: 0x06000D12 RID: 3346 RVA: 0x0002A83C File Offset: 0x00028A3C
			// (set) Token: 0x06000D13 RID: 3347 RVA: 0x0002A849 File Offset: 0x00028A49
			public int income
			{
				get
				{
					return this._income.value;
				}
				set
				{
					this._income.value = value;
				}
			}

			// Token: 0x170002BD RID: 701
			// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0002A857 File Offset: 0x00028A57
			// (set) Token: 0x06000D15 RID: 3349 RVA: 0x0002A864 File Offset: 0x00028A64
			public int totalIncome
			{
				get
				{
					return this._totalIncome.value;
				}
				set
				{
					this._totalIncome.value = value;
				}
			}

			// Token: 0x170002BE RID: 702
			// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0002A872 File Offset: 0x00028A72
			public string spriteTMPKey
			{
				get
				{
					return this._spriteTMPKey;
				}
			}

			// Token: 0x06000D17 RID: 3351 RVA: 0x0002A87C File Offset: 0x00028A7C
			private Currency(string key, string colorCode, string spriteCode)
			{
				this._key = key;
				this._balance = new IntData("Currency/" + key + "/balance", false);
				this._income = new IntData("Currency/" + key + "/income", false);
				this._totalIncome = new IntData("Currency/" + key + "/totalIncome", false);
				this._spriteTMPKey = "<sprite name=\"" + spriteCode + "\">";
				this.colorCode = colorCode;
			}

			// Token: 0x06000D18 RID: 3352 RVA: 0x00002191 File Offset: 0x00000391
			public void DrawEditor()
			{
			}

			// Token: 0x06000D19 RID: 3353 RVA: 0x0002A91B File Offset: 0x00028B1B
			public void Save()
			{
				this._balance.Save();
				this._income.Save();
				this._totalIncome.Save();
			}

			// Token: 0x06000D1A RID: 3354 RVA: 0x0002A93E File Offset: 0x00028B3E
			public void ResetNonpermaAll()
			{
				this.balance = 0;
				this.income = 0;
			}

			// Token: 0x06000D1B RID: 3355 RVA: 0x0002A94E File Offset: 0x00028B4E
			public void Reset()
			{
				this.balance = 0;
				this.income = 0;
				this.totalIncome = 0;
			}

			// Token: 0x06000D1C RID: 3356 RVA: 0x0002A968 File Offset: 0x00028B68
			public void Earn(double amount)
			{
				double num = this.multiplier.total * amount;
				int num2 = (int)num;
				this._remainder += num - (double)num2;
				if (this._remainder >= 1.0)
				{
					int num3 = (int)this._remainder;
					this._remainder -= (double)num3;
					num2 += num3;
				}
				this.balance += num2;
				this.income += num2;
				this.totalIncome += num2;
				GameData.Currency.OnEarnDelegate onEarnDelegate = this.onEarn;
				if (onEarnDelegate == null)
				{
					return;
				}
				onEarnDelegate(num2);
			}

			// Token: 0x06000D1D RID: 3357 RVA: 0x0002A9FE File Offset: 0x00028BFE
			public void Earn(int amount)
			{
				this.Earn((double)amount);
			}

			// Token: 0x06000D1E RID: 3358 RVA: 0x0002AA08 File Offset: 0x00028C08
			public bool Has(int amount)
			{
				return this.balance >= amount;
			}

			// Token: 0x06000D1F RID: 3359 RVA: 0x0002AA16 File Offset: 0x00028C16
			public bool Consume(int amount)
			{
				if (!this.Has(amount))
				{
					return false;
				}
				this.balance -= amount;
				GameData.Currency.OnConsumeDelegate onConsumeDelegate = this.onConsume;
				if (onConsumeDelegate != null)
				{
					onConsumeDelegate(amount);
				}
				return true;
			}

			// Token: 0x04000B2B RID: 2859
			public static string hasMoneyColorCode = "FFDE37";

			// Token: 0x04000B2C RID: 2860
			public static string noMoneyColorCode = "EE1111";

			// Token: 0x04000B2D RID: 2861
			public static string returnColorCode;

			// Token: 0x04000B2E RID: 2862
			public static GameData.Currency gold;

			// Token: 0x04000B2F RID: 2863
			public static GameData.Currency darkQuartz;

			// Token: 0x04000B30 RID: 2864
			public static GameData.Currency bone;

			// Token: 0x04000B31 RID: 2865
			public static GameData.Currency heartQuartz;

			// Token: 0x04000B32 RID: 2866
			public static GameData.Currency dimentionQuartz;

			// Token: 0x04000B33 RID: 2867
			public static EnumArray<GameData.Currency.Type, GameData.Currency> currencies;

			// Token: 0x04000B36 RID: 2870
			public readonly SumDouble multiplier = new SumDouble(1.0);

			// Token: 0x04000B37 RID: 2871
			private readonly string _key;

			// Token: 0x04000B38 RID: 2872
			private readonly string _spriteTMPKey;

			// Token: 0x04000B39 RID: 2873
			private readonly IntData _balance;

			// Token: 0x04000B3A RID: 2874
			private readonly IntData _income;

			// Token: 0x04000B3B RID: 2875
			private readonly IntData _totalIncome;

			// Token: 0x04000B3C RID: 2876
			public readonly string colorCode;

			// Token: 0x04000B3D RID: 2877
			private double _remainder;

			// Token: 0x0200029E RID: 670
			public enum Type
			{
				// Token: 0x04000B3F RID: 2879
				Gold,
				// Token: 0x04000B40 RID: 2880
				DarkQuartz,
				// Token: 0x04000B41 RID: 2881
				Bone,
				// Token: 0x04000B42 RID: 2882
				HeartQuartz
			}

			// Token: 0x0200029F RID: 671
			// (Invoke) Token: 0x06000D22 RID: 3362
			public delegate void OnEarnDelegate(int amount);

			// Token: 0x020002A0 RID: 672
			// (Invoke) Token: 0x06000D26 RID: 3366
			public delegate void OnConsumeDelegate(int amount);
		}

		// Token: 0x020002A1 RID: 673
		public interface IEditorDrawer
		{
			// Token: 0x06000D29 RID: 3369
			void DrawEditor();
		}

		// Token: 0x020002A2 RID: 674
		public static class Gear
		{
			// Token: 0x06000D2A RID: 3370 RVA: 0x0002AA5A File Offset: 0x00028C5A
			public static bool IsUnlocked(string typeName, string name)
			{
				return PersistentSingleton<PlatformManager>.Instance.platform.data.GetBool("gear/unlocked/" + typeName + "/" + name, false);
			}

			// Token: 0x06000D2B RID: 3371 RVA: 0x0002AA82 File Offset: 0x00028C82
			public static void SetUnlocked(string typeName, string name, bool value)
			{
				PersistentSingleton<PlatformManager>.Instance.platform.data.SetBool("gear/unlocked/" + typeName + "/" + name, value);
			}

			// Token: 0x06000D2C RID: 3372 RVA: 0x0002AAAA File Offset: 0x00028CAA
			public static bool IsFounded(string typeName, string name)
			{
				return PersistentSingleton<PlatformManager>.Instance.platform.data.GetBool("gear/founded/" + typeName + "/" + name, false);
			}

			// Token: 0x06000D2D RID: 3373 RVA: 0x0002AAD2 File Offset: 0x00028CD2
			public static void SetFounded(string typeName, string name, bool value)
			{
				PersistentSingleton<PlatformManager>.Instance.platform.data.SetBool("gear/founded/" + typeName + "/" + name, value);
			}

			// Token: 0x06000D2E RID: 3374 RVA: 0x0002AAFA File Offset: 0x00028CFA
			public static void ResetAll()
			{
				PersistentSingleton<PlatformManager>.Instance.platform.data.DeleteKey((string key) => key.StartsWith(key));
			}

			// Token: 0x04000B43 RID: 2883
			private const string key = "gear";

			// Token: 0x04000B44 RID: 2884
			private const string unlockedKey = "gear/unlocked";

			// Token: 0x04000B45 RID: 2885
			private const string foundedKey = "gear/founded";
		}

		// Token: 0x020002A4 RID: 676
		public class Generic : GameData.IEditorDrawer
		{
			// Token: 0x170002BF RID: 703
			// (get) Token: 0x06000D32 RID: 3378 RVA: 0x0002AB44 File Offset: 0x00028D44
			public static GameData.Generic.Tutorial tutorial
			{
				get
				{
					return GameData.Generic.instance._tutorial;
				}
			}

			// Token: 0x170002C0 RID: 704
			// (get) Token: 0x06000D33 RID: 3379 RVA: 0x0002AB50 File Offset: 0x00028D50
			// (set) Token: 0x06000D34 RID: 3380 RVA: 0x0002AB61 File Offset: 0x00028D61
			public static string lastPlayedVersion
			{
				get
				{
					return GameData.Generic.instance._lastPlayedVersion.value;
				}
				set
				{
					GameData.Generic.instance._lastPlayedVersion.value = value;
				}
			}

			// Token: 0x170002C1 RID: 705
			// (get) Token: 0x06000D35 RID: 3381 RVA: 0x0002AB73 File Offset: 0x00028D73
			// (set) Token: 0x06000D36 RID: 3382 RVA: 0x0002AB84 File Offset: 0x00028D84
			public static bool playedTutorialDuringEA
			{
				get
				{
					return GameData.Generic.instance._playedTutorialDuringEA.value;
				}
				set
				{
					GameData.Generic.instance._playedTutorialDuringEA.value = value;
				}
			}

			// Token: 0x170002C2 RID: 706
			// (get) Token: 0x06000D37 RID: 3383 RVA: 0x0002AB96 File Offset: 0x00028D96
			// (set) Token: 0x06000D38 RID: 3384 RVA: 0x0002ABA7 File Offset: 0x00028DA7
			public static bool normalEnding
			{
				get
				{
					return GameData.Generic.instance._normalEnding.value;
				}
				set
				{
					GameData.Generic.instance._normalEnding.value = value;
				}
			}

			// Token: 0x170002C3 RID: 707
			// (get) Token: 0x06000D39 RID: 3385 RVA: 0x0002ABB9 File Offset: 0x00028DB9
			// (set) Token: 0x06000D3A RID: 3386 RVA: 0x0002ABCA File Offset: 0x00028DCA
			public static int skinIndex
			{
				get
				{
					return GameData.Generic.instance._skinIndex.value;
				}
				set
				{
					GameData.Generic.instance._skinIndex.value = value;
				}
			}

			// Token: 0x06000D3B RID: 3387 RVA: 0x0002ABDC File Offset: 0x00028DDC
			public void Initialize()
			{
				this._lastPlayedVersion = new StringData("Generic/_lastPlayedVersion", Application.version, true);
				this._tutorial = new GameData.Generic.Tutorial();
				this._normalEnding = new BoolData("Generic/normalEnding", false);
				this._skinIndex = new IntData("Generic/_skinIndex", false);
				this._playedTutorialDuringEA = new BoolData("Generic/tutorialPlayed", true);
			}

			// Token: 0x06000D3C RID: 3388 RVA: 0x00002191 File Offset: 0x00000391
			public void DrawEditor()
			{
			}

			// Token: 0x06000D3D RID: 3389 RVA: 0x0002AC40 File Offset: 0x00028E40
			public static void ResetAll()
			{
				GameData.Generic.instance._playedTutorialDuringEA.Reset();
				GameData.Generic.instance._tutorial.Reset();
				GameData.Generic.instance._skinIndex.Reset();
				GameData.Generic.instance._normalEnding.Reset();
				GameData.Generic.SaveAll();
			}

			// Token: 0x06000D3E RID: 3390 RVA: 0x0002AC8E File Offset: 0x00028E8E
			public static void SaveAll()
			{
				GameData.Generic.instance._playedTutorialDuringEA.Save();
				GameData.Generic.instance._tutorial.Save();
				GameData.Generic.instance._skinIndex.Save();
				GameData.Generic.instance._normalEnding.Save();
			}

			// Token: 0x06000D3F RID: 3391 RVA: 0x0002ACCC File Offset: 0x00028ECC
			public static void SaveSkin()
			{
				GameData.Generic.instance._skinIndex.Save();
			}

			// Token: 0x04000B48 RID: 2888
			private GameData.Generic.Tutorial _tutorial;

			// Token: 0x04000B49 RID: 2889
			private StringData _lastPlayedVersion;

			// Token: 0x04000B4A RID: 2890
			private BoolData _playedTutorialDuringEA;

			// Token: 0x04000B4B RID: 2891
			private IntData _skinIndex;

			// Token: 0x04000B4C RID: 2892
			private BoolData _normalEnding;

			// Token: 0x04000B4D RID: 2893
			public static readonly GameData.Generic instance = new GameData.Generic();

			// Token: 0x04000B4E RID: 2894
			private const string _playedTutorialDuringEA_DataPath = "Generic/tutorialPlayed";

			// Token: 0x020002A5 RID: 677
			public enum Skin
			{
				// Token: 0x04000B50 RID: 2896
				Skul,
				// Token: 0x04000B51 RID: 2897
				HeroSkul
			}

			// Token: 0x020002A6 RID: 678
			public class Tutorial : GameData.IEditorDrawer
			{
				// Token: 0x06000D42 RID: 3394 RVA: 0x0002ACE9 File Offset: 0x00028EE9
				public Tutorial()
				{
					this._played = new BoolData("Tutorial/_played", true);
				}

				// Token: 0x06000D43 RID: 3395 RVA: 0x0002AD02 File Offset: 0x00028F02
				public void Start()
				{
					this._isPlaying = true;
				}

				// Token: 0x06000D44 RID: 3396 RVA: 0x0002AD0B File Offset: 0x00028F0B
				public void Stop()
				{
					this._isPlaying = false;
				}

				// Token: 0x06000D45 RID: 3397 RVA: 0x0002AD14 File Offset: 0x00028F14
				public void End()
				{
					this._isPlaying = false;
					this.SetData(true);
				}

				// Token: 0x06000D46 RID: 3398 RVA: 0x0002AD24 File Offset: 0x00028F24
				public bool isPlayed()
				{
					return this._played.value;
				}

				// Token: 0x06000D47 RID: 3399 RVA: 0x0002AD31 File Offset: 0x00028F31
				public bool isPlaying()
				{
					return this._isPlaying;
				}

				// Token: 0x06000D48 RID: 3400 RVA: 0x0002AD39 File Offset: 0x00028F39
				internal void Save()
				{
					this._played.Save();
				}

				// Token: 0x06000D49 RID: 3401 RVA: 0x0002AD46 File Offset: 0x00028F46
				internal void Reset()
				{
					this._played.Reset();
				}

				// Token: 0x06000D4A RID: 3402 RVA: 0x0002AD53 File Offset: 0x00028F53
				private void SetData(bool value)
				{
					this._played.value = value;
					this.Save();
				}

				// Token: 0x06000D4B RID: 3403 RVA: 0x00002191 File Offset: 0x00000391
				public void DrawEditor()
				{
				}

				// Token: 0x04000B52 RID: 2898
				private BoolData _played;

				// Token: 0x04000B53 RID: 2899
				private bool _isPlaying;
			}
		}

		// Token: 0x020002A7 RID: 679
		public class HardmodeProgress : GameData.IEditorDrawer
		{
			// Token: 0x170002C4 RID: 708
			// (get) Token: 0x06000D4C RID: 3404 RVA: 0x0002AD67 File Offset: 0x00028F67
			public static GameData.HardmodeProgress.LuckyMeasuringInstrument luckyMeasuringInstrument
			{
				get
				{
					return GameData.HardmodeProgress.instance._luckyMeasuringInstrument;
				}
			}

			// Token: 0x170002C5 RID: 709
			// (get) Token: 0x06000D4D RID: 3405 RVA: 0x0002AD73 File Offset: 0x00028F73
			public static GameData.HardmodeProgress.InscriptionSynthesisEquipment inscriptionSynthesisEquipment
			{
				get
				{
					return GameData.HardmodeProgress.instance._inscriptionSynthesisEquipment;
				}
			}

			// Token: 0x170002C6 RID: 710
			// (get) Token: 0x06000D4E RID: 3406 RVA: 0x0002AD7F File Offset: 0x00028F7F
			public static GameData.HardmodeProgress.BoolDataEnumArray<DarktechData.Type> unlocked
			{
				get
				{
					return GameData.HardmodeProgress.instance._unlocked;
				}
			}

			// Token: 0x170002C7 RID: 711
			// (get) Token: 0x06000D4F RID: 3407 RVA: 0x0002AD8B File Offset: 0x00028F8B
			public static GameData.HardmodeProgress.BoolDataEnumArray<DarktechData.Type> activated
			{
				get
				{
					return GameData.HardmodeProgress.instance._activated;
				}
			}

			// Token: 0x170002C8 RID: 712
			// (get) Token: 0x06000D50 RID: 3408 RVA: 0x0002AD97 File Offset: 0x00028F97
			// (set) Token: 0x06000D51 RID: 3409 RVA: 0x0002ADA8 File Offset: 0x00028FA8
			public static int hardmodeLevel
			{
				get
				{
					return GameData.HardmodeProgress.instance._hardmodeLevel.value;
				}
				set
				{
					GameData.HardmodeProgress.instance._hardmodeLevel.value = value;
				}
			}

			// Token: 0x170002C9 RID: 713
			// (get) Token: 0x06000D52 RID: 3410 RVA: 0x0002ADBA File Offset: 0x00028FBA
			// (set) Token: 0x06000D53 RID: 3411 RVA: 0x0002ADCB File Offset: 0x00028FCB
			public static int clearedLevel
			{
				get
				{
					return GameData.HardmodeProgress.instance._clearedLevel.value;
				}
				set
				{
					GameData.HardmodeProgress.instance._clearedLevel.value = value;
				}
			}

			// Token: 0x170002CA RID: 714
			// (get) Token: 0x06000D54 RID: 3412 RVA: 0x0002ADDD File Offset: 0x00028FDD
			// (set) Token: 0x06000D55 RID: 3413 RVA: 0x0002ADEE File Offset: 0x00028FEE
			public static bool hardmode
			{
				get
				{
					return GameData.HardmodeProgress.instance._hardmode.value;
				}
				set
				{
					GameData.HardmodeProgress.instance._hardmode.value = value;
					if (value)
					{
						GameData.Settings.easyMode = false;
					}
				}
			}

			// Token: 0x06000D56 RID: 3414 RVA: 0x0002AE0C File Offset: 0x0002900C
			public static void ResetAll()
			{
				GameData.HardmodeProgress.instance._luckyMeasuringInstrument.Reset();
				GameData.HardmodeProgress.instance._inscriptionSynthesisEquipment.Reset();
				GameData.HardmodeProgress.unlocked.ResetAll();
				GameData.HardmodeProgress.activated.ResetAll();
				GameData.HardmodeProgress.instance._hardmodeLevel.Reset();
				GameData.HardmodeProgress.instance._clearedLevel.Reset();
				GameData.HardmodeProgress.instance._hardmode.Reset();
				GameData.HardmodeProgress.SaveAll();
			}

			// Token: 0x06000D57 RID: 3415 RVA: 0x0002AE7D File Offset: 0x0002907D
			public static void ResetNonpermaAll()
			{
				GameData.HardmodeProgress.instance._luckyMeasuringInstrument.Reset();
				GameData.HardmodeProgress.instance._inscriptionSynthesisEquipment.Reset();
				GameData.HardmodeProgress.SaveAll();
			}

			// Token: 0x06000D58 RID: 3416 RVA: 0x0002AEA4 File Offset: 0x000290A4
			public static void SaveAll()
			{
				GameData.HardmodeProgress.instance._luckyMeasuringInstrument.Save();
				GameData.HardmodeProgress.instance._inscriptionSynthesisEquipment.Save();
				GameData.HardmodeProgress.unlocked.SaveAll();
				GameData.HardmodeProgress.activated.SaveAll();
				GameData.HardmodeProgress.instance._hardmodeLevel.Save();
				GameData.HardmodeProgress.instance._clearedLevel.Save();
				GameData.HardmodeProgress.instance._hardmode.Save();
			}

			// Token: 0x06000D59 RID: 3417 RVA: 0x0002AF10 File Offset: 0x00029110
			public void Initialize()
			{
				this._luckyMeasuringInstrument = new GameData.HardmodeProgress.LuckyMeasuringInstrument();
				this._inscriptionSynthesisEquipment = new GameData.HardmodeProgress.InscriptionSynthesisEquipment();
				this._unlocked = new GameData.HardmodeProgress.BoolDataEnumArray<DarktechData.Type>("_unlocked");
				this._activated = new GameData.HardmodeProgress.BoolDataEnumArray<DarktechData.Type>("_activated");
				this._hardmodeLevel = new IntData("HardmodeProgress/hardmodeLevel", false);
				this._clearedLevel = new IntData("HardmodeProgress/clearedLevel", -1, false);
				this._hardmode = new BoolData("HardmodeProgress/hardmode", false);
			}

			// Token: 0x06000D5A RID: 3418 RVA: 0x00002191 File Offset: 0x00000391
			public void DrawEditor()
			{
			}

			// Token: 0x04000B54 RID: 2900
			public static readonly GameData.HardmodeProgress instance = new GameData.HardmodeProgress();

			// Token: 0x04000B55 RID: 2901
			private IntData _hardmodeLevel;

			// Token: 0x04000B56 RID: 2902
			private IntData _clearedLevel;

			// Token: 0x04000B57 RID: 2903
			private BoolData _hardmode;

			// Token: 0x04000B58 RID: 2904
			private GameData.HardmodeProgress.LuckyMeasuringInstrument _luckyMeasuringInstrument;

			// Token: 0x04000B59 RID: 2905
			private GameData.HardmodeProgress.InscriptionSynthesisEquipment _inscriptionSynthesisEquipment;

			// Token: 0x04000B5A RID: 2906
			private GameData.HardmodeProgress.BoolDataEnumArray<DarktechData.Type> _unlocked;

			// Token: 0x04000B5B RID: 2907
			private GameData.HardmodeProgress.BoolDataEnumArray<DarktechData.Type> _activated;

			// Token: 0x04000B5C RID: 2908
			public static readonly int maxLevel = 10;

			// Token: 0x020002A8 RID: 680
			public class BoolDataEnumArray<T> : GameData.IEditorDrawer, IEnumerable<KeyValuePair<T, BoolData>>, IEnumerable where T : Enum
			{
				// Token: 0x06000D5D RID: 3421 RVA: 0x0002AF9C File Offset: 0x0002919C
				public BoolDataEnumArray(string foldoutLabel)
				{
					this._foldoutLabel = foldoutLabel;
					foreach (T t in EnumValues<T>.Values)
					{
						if (this._dictionary.ContainsKey(t))
						{
							Debug.LogError(string.Format("The key {0} is duplicated.", t));
						}
						else
						{
							this._dictionary.Add(t, new BoolData(string.Format("{0}/{1}/{2}", "HardmodeProgress", "BoolDataEnumArray", t), false));
						}
					}
				}

				// Token: 0x06000D5E RID: 3422 RVA: 0x0002B04C File Offset: 0x0002924C
				public void SaveAll()
				{
					foreach (BoolData boolData in this._dictionary.Values)
					{
						boolData.Save();
					}
				}

				// Token: 0x06000D5F RID: 3423 RVA: 0x0002B0A4 File Offset: 0x000292A4
				public void ResetAll()
				{
					foreach (BoolData boolData in this._dictionary.Values)
					{
						boolData.Reset();
					}
				}

				// Token: 0x06000D60 RID: 3424 RVA: 0x0002B0FC File Offset: 0x000292FC
				public bool GetData(T key)
				{
					return this._dictionary[key].value;
				}

				// Token: 0x06000D61 RID: 3425 RVA: 0x0002B10F File Offset: 0x0002930F
				public void SetData(T key, bool value)
				{
					this._dictionary[key].value = value;
				}

				// Token: 0x06000D62 RID: 3426 RVA: 0x00002191 File Offset: 0x00000391
				public void DrawEditor()
				{
				}

				// Token: 0x06000D63 RID: 3427 RVA: 0x0002B123 File Offset: 0x00029323
				public IEnumerator<KeyValuePair<T, BoolData>> GetEnumerator()
				{
					return this._dictionary.GetEnumerator();
				}

				// Token: 0x06000D64 RID: 3428 RVA: 0x0002B123 File Offset: 0x00029323
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this._dictionary.GetEnumerator();
				}

				// Token: 0x04000B5D RID: 2909
				private readonly Dictionary<T, BoolData> _dictionary = new Dictionary<T, BoolData>();

				// Token: 0x04000B5E RID: 2910
				private readonly string _foldoutLabel;

				// Token: 0x04000B5F RID: 2911
				private bool _foldout;
			}

			// Token: 0x020002A9 RID: 681
			public class LuckyMeasuringInstrument
			{
				// Token: 0x06000D65 RID: 3429 RVA: 0x0002B138 File Offset: 0x00029338
				public LuckyMeasuringInstrument()
				{
					this.refreshCount = new IntData("HardmodeProgress/refreshCount", false);
					this.lootCount = new IntData("HardmodeProgress/lootCount", false);
					this.lastUniqueDropOrder = new IntData("HardmodeProgress/lastUniqueDropOrder", false);
					this.lastLegendaryDropOrder = new IntData("HardmodeProgress/lastLegendaryDropOrder", false);
					this.items = new StringDataArray("HardmodeProgress/items", this.maxRefreshCount, false);
				}

				// Token: 0x06000D66 RID: 3430 RVA: 0x0002B1B5 File Offset: 0x000293B5
				public void Save()
				{
					this.refreshCount.Save();
					this.lootCount.Save();
					this.lastUniqueDropOrder.Save();
					this.lastLegendaryDropOrder.Save();
					this.items.Save();
				}

				// Token: 0x06000D67 RID: 3431 RVA: 0x0002B1EE File Offset: 0x000293EE
				public void Reset()
				{
					this.refreshCount.Reset();
					this.lootCount.Reset();
					this.lastUniqueDropOrder.Reset();
					this.lastLegendaryDropOrder.Reset();
					this.items.Reset();
				}

				// Token: 0x04000B60 RID: 2912
				public readonly int maxRefreshCount = 20;

				// Token: 0x04000B61 RID: 2913
				public IntData refreshCount;

				// Token: 0x04000B62 RID: 2914
				public IntData lootCount;

				// Token: 0x04000B63 RID: 2915
				public IntData lastUniqueDropOrder;

				// Token: 0x04000B64 RID: 2916
				public IntData lastLegendaryDropOrder;

				// Token: 0x04000B65 RID: 2917
				public StringDataArray items;

				// Token: 0x04000B66 RID: 2918
				private bool _foldout = true;
			}

			// Token: 0x020002AA RID: 682
			public class InscriptionSynthesisEquipment
			{
				// Token: 0x170002CB RID: 715
				public IntData this[int index]
				{
					get
					{
						return this._inscriptionIndics[index];
					}
					set
					{
						this._inscriptionIndics[index] = value;
					}
				}

				// Token: 0x06000D6A RID: 3434 RVA: 0x0002B23C File Offset: 0x0002943C
				public InscriptionSynthesisEquipment()
				{
					this._inscriptionIndics = new IntData[GameData.HardmodeProgress.InscriptionSynthesisEquipment.count];
					for (int i = 0; i < GameData.HardmodeProgress.InscriptionSynthesisEquipment.count; i++)
					{
						this._inscriptionIndics[i] = new IntData(string.Format("{0}/{1}/{2}", "HardmodeProgress", "InscriptionSynthesisEquipment", i), -1, false);
					}
				}

				// Token: 0x06000D6B RID: 3435 RVA: 0x0002B298 File Offset: 0x00029498
				public void Save()
				{
					IntData[] inscriptionIndics = this._inscriptionIndics;
					for (int i = 0; i < inscriptionIndics.Length; i++)
					{
						inscriptionIndics[i].Save();
					}
				}

				// Token: 0x06000D6C RID: 3436 RVA: 0x0002B2C4 File Offset: 0x000294C4
				public void Reset()
				{
					IntData[] inscriptionIndics = this._inscriptionIndics;
					for (int i = 0; i < inscriptionIndics.Length; i++)
					{
						inscriptionIndics[i].Reset();
					}
				}

				// Token: 0x04000B67 RID: 2919
				public static int count = 2;

				// Token: 0x04000B68 RID: 2920
				private IntData[] _inscriptionIndics;
			}
		}

		// Token: 0x020002AB RID: 683
		public class Progress : GameData.IEditorDrawer
		{
			// Token: 0x170002CC RID: 716
			// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0002B2F6 File Offset: 0x000294F6
			public static GameData.Progress.WitchMastery witch
			{
				get
				{
					return GameData.Progress.instance._witch;
				}
			}

			// Token: 0x170002CD RID: 717
			// (get) Token: 0x06000D6F RID: 3439 RVA: 0x0002B302 File Offset: 0x00029502
			public static GameData.Progress.BoolDataEnumArray<Level.Npc.FieldNpcs.NpcType> fieldNpcEncountered
			{
				get
				{
					return GameData.Progress.instance._fieldNpcEncountered;
				}
			}

			// Token: 0x170002CE RID: 718
			// (get) Token: 0x06000D70 RID: 3440 RVA: 0x0002B30E File Offset: 0x0002950E
			public static GameData.Progress.BoolDataEnumArray<SpecialMap.Type> specialMapEncountered
			{
				get
				{
					return GameData.Progress.instance._specialMapEncountered;
				}
			}

			// Token: 0x170002CF RID: 719
			// (get) Token: 0x06000D71 RID: 3441 RVA: 0x0002B31A File Offset: 0x0002951A
			public static GameData.Progress.BoolDataEnumArray<CutScenes.Key> cutscene
			{
				get
				{
					return GameData.Progress.instance._cutscene;
				}
			}

			// Token: 0x170002D0 RID: 720
			// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0002B326 File Offset: 0x00029526
			public static GameData.Progress.BoolDataEnumArray<SkulStories.Key> skulstory
			{
				get
				{
					return GameData.Progress.instance._skulstory;
				}
			}

			// Token: 0x170002D1 RID: 721
			// (get) Token: 0x06000D73 RID: 3443 RVA: 0x0002B332 File Offset: 0x00029532
			// (set) Token: 0x06000D74 RID: 3444 RVA: 0x0002B343 File Offset: 0x00029543
			public static int playTime
			{
				get
				{
					return GameData.Progress.instance._playTime.value;
				}
				set
				{
					GameData.Progress.instance._playTime.value = value;
				}
			}

			// Token: 0x170002D2 RID: 722
			// (get) Token: 0x06000D75 RID: 3445 RVA: 0x0002B355 File Offset: 0x00029555
			// (set) Token: 0x06000D76 RID: 3446 RVA: 0x0002B366 File Offset: 0x00029566
			public static int deaths
			{
				get
				{
					return GameData.Progress.instance._deaths.value;
				}
				set
				{
					GameData.Progress.instance._deaths.value = value;
				}
			}

			// Token: 0x170002D3 RID: 723
			// (get) Token: 0x06000D77 RID: 3447 RVA: 0x0002B378 File Offset: 0x00029578
			// (set) Token: 0x06000D78 RID: 3448 RVA: 0x0002B389 File Offset: 0x00029589
			public static int kills
			{
				get
				{
					return GameData.Progress.instance._kills.value;
				}
				set
				{
					GameData.Progress.instance._kills.value = value;
				}
			}

			// Token: 0x170002D4 RID: 724
			// (get) Token: 0x06000D79 RID: 3449 RVA: 0x0002B39B File Offset: 0x0002959B
			// (set) Token: 0x06000D7A RID: 3450 RVA: 0x0002B3AC File Offset: 0x000295AC
			public static int totalAdventurerKills
			{
				get
				{
					return GameData.Progress.instance._totalAdventurerKills.value;
				}
				set
				{
					GameData.Progress.instance._totalAdventurerKills.value = value;
				}
			}

			// Token: 0x170002D5 RID: 725
			// (get) Token: 0x06000D7B RID: 3451 RVA: 0x0002B3BE File Offset: 0x000295BE
			// (set) Token: 0x06000D7C RID: 3452 RVA: 0x0002B3CF File Offset: 0x000295CF
			public static int eliteKills
			{
				get
				{
					return GameData.Progress.instance._eliteKills.value;
				}
				set
				{
					GameData.Progress.instance._eliteKills.value = value;
				}
			}

			// Token: 0x170002D6 RID: 726
			// (get) Token: 0x06000D7D RID: 3453 RVA: 0x0002B3E1 File Offset: 0x000295E1
			// (set) Token: 0x06000D7E RID: 3454 RVA: 0x0002B3F2 File Offset: 0x000295F2
			public static int gainedDarkcite
			{
				get
				{
					return GameData.Progress.instance._gainedDarkcite.value;
				}
				set
				{
					GameData.Progress.instance._gainedDarkcite.value = value;
				}
			}

			// Token: 0x170002D7 RID: 727
			// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0002B404 File Offset: 0x00029604
			// (set) Token: 0x06000D80 RID: 3456 RVA: 0x0002B415 File Offset: 0x00029615
			public static int totalDamage
			{
				get
				{
					return GameData.Progress.instance._totalDamage.value;
				}
				set
				{
					GameData.Progress.instance._totalDamage.value = value;
				}
			}

			// Token: 0x170002D8 RID: 728
			// (get) Token: 0x06000D81 RID: 3457 RVA: 0x0002B427 File Offset: 0x00029627
			// (set) Token: 0x06000D82 RID: 3458 RVA: 0x0002B438 File Offset: 0x00029638
			public static int totalTakingDamage
			{
				get
				{
					return GameData.Progress.instance._totalTakingDamage.value;
				}
				set
				{
					GameData.Progress.instance._totalTakingDamage.value = value;
				}
			}

			// Token: 0x170002D9 RID: 729
			// (get) Token: 0x06000D83 RID: 3459 RVA: 0x0002B44A File Offset: 0x0002964A
			// (set) Token: 0x06000D84 RID: 3460 RVA: 0x0002B45B File Offset: 0x0002965B
			public static int totalHeal
			{
				get
				{
					return GameData.Progress.instance._totalHeal.value;
				}
				set
				{
					GameData.Progress.instance._totalHeal.value = value;
				}
			}

			// Token: 0x170002DA RID: 730
			// (get) Token: 0x06000D85 RID: 3461 RVA: 0x0002B46D File Offset: 0x0002966D
			// (set) Token: 0x06000D86 RID: 3462 RVA: 0x0002B47E File Offset: 0x0002967E
			public static int bestDamage
			{
				get
				{
					return GameData.Progress.instance._bestDamage.value;
				}
				set
				{
					GameData.Progress.instance._bestDamage.value = value;
				}
			}

			// Token: 0x170002DB RID: 731
			// (get) Token: 0x06000D87 RID: 3463 RVA: 0x0002B490 File Offset: 0x00029690
			// (set) Token: 0x06000D88 RID: 3464 RVA: 0x0002B4A1 File Offset: 0x000296A1
			public static int encounterWeaponCount
			{
				get
				{
					return GameData.Progress.instance._encounterWeaponCount.value;
				}
				set
				{
					GameData.Progress.instance._encounterWeaponCount.value = value;
				}
			}

			// Token: 0x170002DC RID: 732
			// (get) Token: 0x06000D89 RID: 3465 RVA: 0x0002B4B3 File Offset: 0x000296B3
			// (set) Token: 0x06000D8A RID: 3466 RVA: 0x0002B4C4 File Offset: 0x000296C4
			public static int encounterItemCount
			{
				get
				{
					return GameData.Progress.instance._encounterItemCount.value;
				}
				set
				{
					GameData.Progress.instance._encounterItemCount.value = value;
				}
			}

			// Token: 0x170002DD RID: 733
			// (get) Token: 0x06000D8B RID: 3467 RVA: 0x0002B4D6 File Offset: 0x000296D6
			// (set) Token: 0x06000D8C RID: 3468 RVA: 0x0002B4E7 File Offset: 0x000296E7
			public static int encounterEssenceCount
			{
				get
				{
					return GameData.Progress.instance._encounterEssenceCount.value;
				}
				set
				{
					GameData.Progress.instance._encounterEssenceCount.value = value;
				}
			}

			// Token: 0x170002DE RID: 734
			// (get) Token: 0x06000D8D RID: 3469 RVA: 0x0002B4F9 File Offset: 0x000296F9
			// (set) Token: 0x06000D8E RID: 3470 RVA: 0x0002B50A File Offset: 0x0002970A
			public static int housingPoint
			{
				get
				{
					return GameData.Progress.instance._housingPoint.value;
				}
				set
				{
					GameData.Progress.instance._housingPoint.value = value;
				}
			}

			// Token: 0x170002DF RID: 735
			// (get) Token: 0x06000D8F RID: 3471 RVA: 0x0002B51C File Offset: 0x0002971C
			// (set) Token: 0x06000D90 RID: 3472 RVA: 0x0002B52D File Offset: 0x0002972D
			public static int housingSeen
			{
				get
				{
					return GameData.Progress.instance._housingSeen.value;
				}
				set
				{
					GameData.Progress.instance._housingSeen.value = value;
				}
			}

			// Token: 0x170002E0 RID: 736
			// (get) Token: 0x06000D91 RID: 3473 RVA: 0x0002B53F File Offset: 0x0002973F
			// (set) Token: 0x06000D92 RID: 3474 RVA: 0x0002B550 File Offset: 0x00029750
			public static bool reassembleUsed
			{
				get
				{
					return GameData.Progress.instance._reassembleUsed.value;
				}
				set
				{
					GameData.Progress.instance._reassembleUsed.value = value;
				}
			}

			// Token: 0x170002E1 RID: 737
			// (get) Token: 0x06000D93 RID: 3475 RVA: 0x0002B562 File Offset: 0x00029762
			// (set) Token: 0x06000D94 RID: 3476 RVA: 0x0002B573 File Offset: 0x00029773
			public static bool arachneTutorial
			{
				get
				{
					return GameData.Progress.instance._arachneTutorial.value;
				}
				set
				{
					GameData.Progress.instance._arachneTutorial.value = value;
				}
			}

			// Token: 0x170002E2 RID: 738
			// (get) Token: 0x06000D95 RID: 3477 RVA: 0x0002B585 File Offset: 0x00029785
			// (set) Token: 0x06000D96 RID: 3478 RVA: 0x0002B596 File Offset: 0x00029796
			public static bool foxRescued
			{
				get
				{
					return GameData.Progress.instance._foxRescued.value;
				}
				set
				{
					GameData.Progress.instance._foxRescued.value = value;
				}
			}

			// Token: 0x170002E3 RID: 739
			// (get) Token: 0x06000D97 RID: 3479 RVA: 0x0002B5A8 File Offset: 0x000297A8
			// (set) Token: 0x06000D98 RID: 3480 RVA: 0x0002B5B9 File Offset: 0x000297B9
			public static bool ogreRescued
			{
				get
				{
					return GameData.Progress.instance._ogreRescued.value;
				}
				set
				{
					GameData.Progress.instance._ogreRescued.value = value;
				}
			}

			// Token: 0x170002E4 RID: 740
			// (get) Token: 0x06000D99 RID: 3481 RVA: 0x0002B5CB File Offset: 0x000297CB
			// (set) Token: 0x06000D9A RID: 3482 RVA: 0x0002B5DC File Offset: 0x000297DC
			public static bool druidRescued
			{
				get
				{
					return GameData.Progress.instance._druidRescued.value;
				}
				set
				{
					GameData.Progress.instance._druidRescued.value = value;
				}
			}

			// Token: 0x170002E5 RID: 741
			// (get) Token: 0x06000D9B RID: 3483 RVA: 0x0002B5EE File Offset: 0x000297EE
			// (set) Token: 0x06000D9C RID: 3484 RVA: 0x0002B5FF File Offset: 0x000297FF
			public static bool deathknightRescued
			{
				get
				{
					return GameData.Progress.instance._deathknightRescued.value;
				}
				set
				{
					GameData.Progress.instance._deathknightRescued.value = value;
				}
			}

			// Token: 0x06000D9D RID: 3485 RVA: 0x0002B611 File Offset: 0x00029811
			public static bool GetRescued(Level.Npc.NpcType npcType)
			{
				return GameData.Progress.instance._rescuedByNpcType[npcType].value;
			}

			// Token: 0x06000D9E RID: 3486 RVA: 0x0002B628 File Offset: 0x00029828
			public static void SetRescued(Level.Npc.NpcType npcType, bool value)
			{
				GameData.Progress.instance._rescuedByNpcType[npcType].value = value;
			}

			// Token: 0x06000D9F RID: 3487 RVA: 0x0002B640 File Offset: 0x00029840
			public static void ResetAll()
			{
				GameData.Progress.instance._witch.Reset();
				GameData.Progress.instance._fieldNpcEncountered.ResetAll();
				GameData.Progress.instance._specialMapEncountered.ResetAll();
				GameData.Progress.instance._cutscene.ResetAll();
				GameData.Progress.instance._skulstory.ResetAll();
				GameData.Progress.instance._playTime.Reset();
				GameData.Progress.instance._deaths.Reset();
				GameData.Progress.instance._kills.Reset();
				GameData.Progress.instance._eliteKills.Reset();
				GameData.Progress.instance._totalAdventurerKills.Reset();
				GameData.Progress.instance._gainedDarkcite.Reset();
				GameData.Progress.instance._totalDamage.Reset();
				GameData.Progress.instance._totalTakingDamage.Reset();
				GameData.Progress.instance._totalHeal.Reset();
				GameData.Progress.instance._bestDamage.Reset();
				GameData.Progress.instance._encounterWeaponCount.Reset();
				GameData.Progress.instance._encounterItemCount.Reset();
				GameData.Progress.instance._encounterEssenceCount.Reset();
				GameData.Progress.instance._housingPoint.Reset();
				GameData.Progress.instance._housingSeen.Reset();
				GameData.Progress.instance._reassembleUsed.Reset();
				GameData.Progress.instance._arachneTutorial.Reset();
				GameData.Progress.instance._foxRescued.Reset();
				GameData.Progress.instance._ogreRescued.Reset();
				GameData.Progress.instance._druidRescued.Reset();
				GameData.Progress.instance._deathknightRescued.Reset();
				GameData.Progress.SaveAll();
			}

			// Token: 0x06000DA0 RID: 3488 RVA: 0x0002B7D8 File Offset: 0x000299D8
			public static void ResetNonpermaAll()
			{
				GameData.Progress.playTime = 0;
				GameData.Progress.kills = 0;
				GameData.Progress.eliteKills = 0;
				GameData.Progress.gainedDarkcite = 0;
				GameData.Progress.totalDamage = 0;
				GameData.Progress.totalTakingDamage = 0;
				GameData.Progress.totalHeal = 0;
				GameData.Progress.bestDamage = 0;
				GameData.Progress.encounterWeaponCount = 0;
				GameData.Progress.encounterItemCount = 0;
				GameData.Progress.encounterEssenceCount = 0;
				GameData.Progress.reassembleUsed = false;
				GameData.Progress.fieldNpcEncountered.ResetAll();
				GameData.Progress.specialMapEncountered.ResetAll();
				GameData.Progress.SaveAll();
			}

			// Token: 0x06000DA1 RID: 3489 RVA: 0x0002B848 File Offset: 0x00029A48
			public static void SaveAll()
			{
				GameData.Progress.instance._playTime.Save();
				GameData.Progress.instance._deaths.Save();
				GameData.Progress.instance._kills.Save();
				GameData.Progress.instance._eliteKills.Save();
				GameData.Progress.instance._totalAdventurerKills.Save();
				GameData.Progress.instance._gainedDarkcite.Save();
				GameData.Progress.instance._totalDamage.Save();
				GameData.Progress.instance._totalTakingDamage.Save();
				GameData.Progress.instance._totalHeal.Save();
				GameData.Progress.instance._bestDamage.Save();
				GameData.Progress.instance._encounterWeaponCount.Save();
				GameData.Progress.instance._encounterItemCount.Save();
				GameData.Progress.instance._encounterEssenceCount.Save();
				GameData.Progress.instance._housingPoint.Save();
				GameData.Progress.instance._housingSeen.Save();
				GameData.Progress.instance._reassembleUsed.Save();
				GameData.Progress.instance._arachneTutorial.Save();
				GameData.Progress.instance._foxRescued.Save();
				GameData.Progress.instance._ogreRescued.Save();
				GameData.Progress.instance._druidRescued.Save();
				GameData.Progress.instance._deathknightRescued.Save();
				GameData.Progress.witch.Save();
				GameData.Progress.fieldNpcEncountered.SaveAll();
				GameData.Progress.specialMapEncountered.SaveAll();
				GameData.Progress.cutscene.SaveAll();
				GameData.Progress.skulstory.SaveAll();
			}

			// Token: 0x06000DA2 RID: 3490 RVA: 0x0002B9C4 File Offset: 0x00029BC4
			public void Initialize()
			{
				this._witch = new GameData.Progress.WitchMastery();
				this._fieldNpcEncountered = new GameData.Progress.BoolDataEnumArray<Level.Npc.FieldNpcs.NpcType>("fieldNpcEncountered");
				this._specialMapEncountered = new GameData.Progress.BoolDataEnumArray<SpecialMap.Type>("specialMapEncountered");
				this._cutscene = new GameData.Progress.BoolDataEnumArray<CutScenes.Key>("CutScenes");
				if (this._cutscene.GetData(CutScenes.Key.ending))
				{
					GameData.Generic.normalEnding = true;
					GameData.Generic.SaveAll();
				}
				this._skulstory = new GameData.Progress.BoolDataEnumArray<SkulStories.Key>("SkulStories");
				this._playTime = new IntData("Progress/playTime", false);
				this._deaths = new IntData("Progress/deaths", false);
				this._kills = new IntData("Progress/kills", false);
				this._eliteKills = new IntData("Progress/eliteKills", false);
				this._totalAdventurerKills = new IntData("Progress/totalAdventurerKills", false);
				this._gainedDarkcite = new IntData("Progress/gainedDarkcite", false);
				this._totalDamage = new IntData("Progress/totalDamage", false);
				this._totalTakingDamage = new IntData("Progress/totalTakingDamage", false);
				this._totalHeal = new IntData("Progress/totalHeal", false);
				this._bestDamage = new IntData("Progress/_bestDamage", false);
				this._encounterWeaponCount = new IntData("Progress/encounterWeaponCount", false);
				this._encounterItemCount = new IntData("Progress/encounterItemCount", false);
				this._encounterEssenceCount = new IntData("Progress/encounterEssenceCount", false);
				this._housingPoint = new IntData("Progress/housingPoint", false);
				this._housingSeen = new IntData("Progress/_housingSeen", false);
				this._reassembleUsed = new BoolData("Progress/reassembleUsed", false);
				this._arachneTutorial = new BoolData("Progress/arachneTutorial", false);
				this._foxRescued = new BoolData("Progress/foxRescued", false);
				this._ogreRescued = new BoolData("Progress/ogreRescued", false);
				this._druidRescued = new BoolData("Progress/druidRescued", false);
				this._deathknightRescued = new BoolData("Progress/deathknightRescued", false);
				this._rescuedByNpcType = new EnumArray<Level.Npc.NpcType, BoolData>(new BoolData[]
				{
					null,
					this._foxRescued,
					this._ogreRescued,
					this._druidRescued,
					this._deathknightRescued
				});
			}

			// Token: 0x06000DA3 RID: 3491 RVA: 0x00002191 File Offset: 0x00000391
			public void DrawEditor()
			{
			}

			// Token: 0x04000B69 RID: 2921
			public static readonly GameData.Progress instance = new GameData.Progress();

			// Token: 0x04000B6A RID: 2922
			private GameData.Progress.BoolDataEnumArray<Level.Npc.FieldNpcs.NpcType> _fieldNpcEncountered;

			// Token: 0x04000B6B RID: 2923
			private GameData.Progress.BoolDataEnumArray<SpecialMap.Type> _specialMapEncountered;

			// Token: 0x04000B6C RID: 2924
			private GameData.Progress.BoolDataEnumArray<CutScenes.Key> _cutscene;

			// Token: 0x04000B6D RID: 2925
			private GameData.Progress.BoolDataEnumArray<SkulStories.Key> _skulstory;

			// Token: 0x04000B6E RID: 2926
			private GameData.Progress.WitchMastery _witch;

			// Token: 0x04000B6F RID: 2927
			private IntData _playTime;

			// Token: 0x04000B70 RID: 2928
			private IntData _deaths;

			// Token: 0x04000B71 RID: 2929
			private IntData _kills;

			// Token: 0x04000B72 RID: 2930
			private IntData _eliteKills;

			// Token: 0x04000B73 RID: 2931
			private IntData _totalAdventurerKills;

			// Token: 0x04000B74 RID: 2932
			private IntData _gainedDarkcite;

			// Token: 0x04000B75 RID: 2933
			private IntData _housingPoint;

			// Token: 0x04000B76 RID: 2934
			private IntData _housingSeen;

			// Token: 0x04000B77 RID: 2935
			private BoolData _reassembleUsed;

			// Token: 0x04000B78 RID: 2936
			private IntData _totalDamage;

			// Token: 0x04000B79 RID: 2937
			private IntData _totalTakingDamage;

			// Token: 0x04000B7A RID: 2938
			private IntData _totalHeal;

			// Token: 0x04000B7B RID: 2939
			private IntData _bestDamage;

			// Token: 0x04000B7C RID: 2940
			private IntData _encounterWeaponCount;

			// Token: 0x04000B7D RID: 2941
			private IntData _encounterItemCount;

			// Token: 0x04000B7E RID: 2942
			private IntData _encounterEssenceCount;

			// Token: 0x04000B7F RID: 2943
			private BoolData _arachneTutorial;

			// Token: 0x04000B80 RID: 2944
			private BoolData _foxRescued;

			// Token: 0x04000B81 RID: 2945
			private BoolData _ogreRescued;

			// Token: 0x04000B82 RID: 2946
			private BoolData _druidRescued;

			// Token: 0x04000B83 RID: 2947
			private BoolData _deathknightRescued;

			// Token: 0x04000B84 RID: 2948
			private EnumArray<Level.Npc.NpcType, BoolData> _rescuedByNpcType;

			// Token: 0x020002AC RID: 684
			public class WitchMastery : GameData.IEditorDrawer
			{
				// Token: 0x06000DA6 RID: 3494 RVA: 0x0002BBDF File Offset: 0x00029DDF
				public void Save()
				{
					this.skull.Save();
					this.body.Save();
					this.soul.Save();
				}

				// Token: 0x06000DA7 RID: 3495 RVA: 0x0002BC02 File Offset: 0x00029E02
				public void Reset()
				{
					this.skull.Reset();
					this.body.Reset();
					this.soul.Reset();
				}

				// Token: 0x06000DA8 RID: 3496 RVA: 0x0002BC28 File Offset: 0x00029E28
				public void RefundFormer()
				{
					int num = 0;
					num += this.skull.GerFormerRefundAmount();
					num += this.body.GerFormerRefundAmount();
					num += this.soul.GerFormerRefundAmount();
					if (num == 0)
					{
						return;
					}
					Debug.Log(string.Format("Old witch mastery is refunded. Refunded dark quartz amount : {0}", num));
					GameData.Currency.darkQuartz.balance += num;
					this.Reset();
				}

				// Token: 0x06000DA9 RID: 3497 RVA: 0x00002191 File Offset: 0x00000391
				public void DrawEditor()
				{
				}

				// Token: 0x04000B85 RID: 2949
				private const int count = 4;

				// Token: 0x04000B86 RID: 2950
				public readonly GameData.Progress.WitchMastery.Bonuses skull = new GameData.Progress.WitchMastery.Bonuses("skull");

				// Token: 0x04000B87 RID: 2951
				public readonly GameData.Progress.WitchMastery.Bonuses body = new GameData.Progress.WitchMastery.Bonuses("body");

				// Token: 0x04000B88 RID: 2952
				public readonly GameData.Progress.WitchMastery.Bonuses soul = new GameData.Progress.WitchMastery.Bonuses("soul");

				// Token: 0x04000B89 RID: 2953
				private bool _foldout;

				// Token: 0x020002AD RID: 685
				public class Bonuses : GameData.IEditorDrawer
				{
					// Token: 0x170002E6 RID: 742
					public IntData this[int index]
					{
						get
						{
							return this._datas[index];
						}
					}

					// Token: 0x06000DAC RID: 3500 RVA: 0x0002BCD4 File Offset: 0x00029ED4
					public Bonuses(string key)
					{
						this.key = key;
						for (int i = 0; i < 4; i++)
						{
							this._datas[i] = new IntData(string.Format("{0}/{1}/{2}/{3}", new object[]
							{
								"Progress",
								"WitchMastery",
								key,
								i
							}), false);
						}
					}

					// Token: 0x06000DAD RID: 3501 RVA: 0x0002BD48 File Offset: 0x00029F48
					public void Save()
					{
						IntData[] datas = this._datas;
						for (int i = 0; i < datas.Length; i++)
						{
							datas[i].Save();
						}
					}

					// Token: 0x06000DAE RID: 3502 RVA: 0x0002BD74 File Offset: 0x00029F74
					public void Reset()
					{
						IntData[] datas = this._datas;
						for (int i = 0; i < datas.Length; i++)
						{
							datas[i].Reset();
						}
					}

					// Token: 0x06000DAF RID: 3503 RVA: 0x0002BDA0 File Offset: 0x00029FA0
					public int GerFormerRefundAmount()
					{
						int num = 0;
						for (int i = 0; i < this._datas.Length; i++)
						{
							num += WitchMasteryFormerPrice.GeRefundAmount(i, this._datas[i].value);
						}
						return num;
					}

					// Token: 0x06000DB0 RID: 3504 RVA: 0x00002191 File Offset: 0x00000391
					public void DrawEditor()
					{
					}

					// Token: 0x04000B8A RID: 2954
					public readonly string key;

					// Token: 0x04000B8B RID: 2955
					private bool _foldout = true;

					// Token: 0x04000B8C RID: 2956
					private IntData[] _datas = new IntData[4];
				}
			}

			// Token: 0x020002AE RID: 686
			public class BoolDataEnumArray<T> : GameData.IEditorDrawer, IEnumerable<KeyValuePair<T, BoolData>>, IEnumerable where T : Enum
			{
				// Token: 0x06000DB1 RID: 3505 RVA: 0x0002BDDC File Offset: 0x00029FDC
				public BoolDataEnumArray(string foldoutLabel)
				{
					this._foldoutLabel = foldoutLabel;
					foreach (T t in EnumValues<T>.Values)
					{
						if (this._dictionary.ContainsKey(t))
						{
							Debug.LogError(string.Format("The key {0} is duplicated.", t));
						}
						else
						{
							this._dictionary.Add(t, new BoolData(string.Format("{0}/{1}", "BoolDataEnumArray", t), false));
						}
					}
				}

				// Token: 0x06000DB2 RID: 3506 RVA: 0x0002BE88 File Offset: 0x0002A088
				public void SaveAll()
				{
					foreach (BoolData boolData in this._dictionary.Values)
					{
						boolData.Save();
					}
				}

				// Token: 0x06000DB3 RID: 3507 RVA: 0x0002BEE0 File Offset: 0x0002A0E0
				public void ResetAll()
				{
					foreach (BoolData boolData in this._dictionary.Values)
					{
						boolData.Reset();
					}
				}

				// Token: 0x06000DB4 RID: 3508 RVA: 0x0002BF38 File Offset: 0x0002A138
				public bool GetData(T key)
				{
					return this._dictionary[key].value;
				}

				// Token: 0x06000DB5 RID: 3509 RVA: 0x0002BF4B File Offset: 0x0002A14B
				public void SetData(T key, bool value)
				{
					this._dictionary[key].value = value;
				}

				// Token: 0x06000DB6 RID: 3510 RVA: 0x0002BF60 File Offset: 0x0002A160
				public void SetDataAll(bool value)
				{
					foreach (BoolData boolData in this._dictionary.Values)
					{
						boolData.value = value;
					}
				}

				// Token: 0x06000DB7 RID: 3511 RVA: 0x00002191 File Offset: 0x00000391
				public void DrawEditor()
				{
				}

				// Token: 0x06000DB8 RID: 3512 RVA: 0x0002BFB8 File Offset: 0x0002A1B8
				public IEnumerator<KeyValuePair<T, BoolData>> GetEnumerator()
				{
					return this._dictionary.GetEnumerator();
				}

				// Token: 0x06000DB9 RID: 3513 RVA: 0x0002BFB8 File Offset: 0x0002A1B8
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this._dictionary.GetEnumerator();
				}

				// Token: 0x04000B8D RID: 2957
				private readonly Dictionary<T, BoolData> _dictionary = new Dictionary<T, BoolData>();

				// Token: 0x04000B8E RID: 2958
				private readonly string _foldoutLabel;

				// Token: 0x04000B8F RID: 2959
				private bool _foldout;
			}
		}

		// Token: 0x020002AF RID: 687
		public class Record
		{
			// Token: 0x06000DBA RID: 3514 RVA: 0x0002BFCA File Offset: 0x0002A1CA
			public static int GetBestTime(string key)
			{
				return PersistentSingleton<PlatformManager>.Instance.platform.data.GetInt("Record/bestTime/" + key, int.MaxValue);
			}

			// Token: 0x06000DBB RID: 3515 RVA: 0x0002BFF0 File Offset: 0x0002A1F0
			public static void SetBestTime(string key, int value)
			{
				PersistentSingleton<PlatformManager>.Instance.platform.data.SetInt("Record/bestTime/" + key, value);
			}

			// Token: 0x06000DBC RID: 3516 RVA: 0x0002C014 File Offset: 0x0002A214
			public static bool UpdateBestTime(string key)
			{
				int bestTime = GameData.Record.GetBestTime(key);
				int playTime = GameData.Progress.playTime;
				if (bestTime < playTime)
				{
					return false;
				}
				GameData.Record.SetBestTime(key, playTime);
				return true;
			}

			// Token: 0x06000DBD RID: 3517 RVA: 0x0002C03A File Offset: 0x0002A23A
			public static void ResetAll()
			{
				PersistentSingleton<PlatformManager>.Instance.platform.data.DeleteKey((string key) => key.StartsWith(key));
			}

			// Token: 0x04000B90 RID: 2960
			private const string _key = "Record";

			// Token: 0x04000B91 RID: 2961
			private const string bestTimeKey = "Record/bestTime";
		}

		// Token: 0x020002B1 RID: 689
		public class Save : GameData.IEditorDrawer
		{
			// Token: 0x170002E7 RID: 743
			// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0002C07B File Offset: 0x0002A27B
			// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x0002C083 File Offset: 0x0002A283
			public bool initilaized { get; private set; }

			// Token: 0x170002E8 RID: 744
			// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0002C08C File Offset: 0x0002A28C
			// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x0002C099 File Offset: 0x0002A299
			public bool hasSave
			{
				get
				{
					return this._hasSave.value;
				}
				set
				{
					this._hasSave.value = value;
				}
			}

			// Token: 0x170002E9 RID: 745
			// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x0002C0A7 File Offset: 0x0002A2A7
			public int randomSeed
			{
				get
				{
					return this._randomSeed.value;
				}
			}

			// Token: 0x170002EA RID: 746
			// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x0002C0B4 File Offset: 0x0002A2B4
			// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x0002C0C1 File Offset: 0x0002A2C1
			public int health
			{
				get
				{
					return this._health.value;
				}
				set
				{
					this._health.value = value;
				}
			}

			// Token: 0x170002EB RID: 747
			// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x0002C0CF File Offset: 0x0002A2CF
			// (set) Token: 0x06000DCA RID: 3530 RVA: 0x0002C0DC File Offset: 0x0002A2DC
			public int chapterIndex
			{
				get
				{
					return this._chapterIndex.value;
				}
				set
				{
					this._chapterIndex.value = value;
				}
			}

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x06000DCB RID: 3531 RVA: 0x0002C0EA File Offset: 0x0002A2EA
			// (set) Token: 0x06000DCC RID: 3532 RVA: 0x0002C0F7 File Offset: 0x0002A2F7
			public int stageIndex
			{
				get
				{
					return this._stageIndex.value;
				}
				set
				{
					this._stageIndex.value = value;
				}
			}

			// Token: 0x170002ED RID: 749
			// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0002C105 File Offset: 0x0002A305
			// (set) Token: 0x06000DCE RID: 3534 RVA: 0x0002C112 File Offset: 0x0002A312
			public int pathIndex
			{
				get
				{
					return this._pathIndex.value;
				}
				set
				{
					this._pathIndex.value = value;
				}
			}

			// Token: 0x170002EE RID: 750
			// (get) Token: 0x06000DCF RID: 3535 RVA: 0x0002C120 File Offset: 0x0002A320
			// (set) Token: 0x06000DD0 RID: 3536 RVA: 0x0002C12D File Offset: 0x0002A32D
			public int nodeIndex
			{
				get
				{
					return this._nodeIndex.value;
				}
				set
				{
					this._nodeIndex.value = value;
				}
			}

			// Token: 0x170002EF RID: 751
			// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x0002C13B File Offset: 0x0002A33B
			// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x0002C148 File Offset: 0x0002A348
			public string currentWeapon
			{
				get
				{
					return this._currentWeapon.value;
				}
				set
				{
					this._currentWeapon.value = value;
				}
			}

			// Token: 0x170002F0 RID: 752
			// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x0002C156 File Offset: 0x0002A356
			// (set) Token: 0x06000DD4 RID: 3540 RVA: 0x0002C163 File Offset: 0x0002A363
			public string nextWeapon
			{
				get
				{
					return this._nextWeapon.value;
				}
				set
				{
					this._nextWeapon.value = value;
				}
			}

			// Token: 0x170002F1 RID: 753
			// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x0002C171 File Offset: 0x0002A371
			// (set) Token: 0x06000DD6 RID: 3542 RVA: 0x0002C17E File Offset: 0x0002A37E
			public string currentWeaponSkill1
			{
				get
				{
					return this._currentWeaponSkill1.value;
				}
				set
				{
					this._currentWeaponSkill1.value = value;
				}
			}

			// Token: 0x170002F2 RID: 754
			// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x0002C18C File Offset: 0x0002A38C
			// (set) Token: 0x06000DD8 RID: 3544 RVA: 0x0002C199 File Offset: 0x0002A399
			public string currentWeaponSkill2
			{
				get
				{
					return this._currentWeaponSkill2.value;
				}
				set
				{
					this._currentWeaponSkill2.value = value;
				}
			}

			// Token: 0x170002F3 RID: 755
			// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0002C1A7 File Offset: 0x0002A3A7
			// (set) Token: 0x06000DDA RID: 3546 RVA: 0x0002C1B4 File Offset: 0x0002A3B4
			public string nextWeaponSkill1
			{
				get
				{
					return this._nextWeaponSkill1.value;
				}
				set
				{
					this._nextWeaponSkill1.value = value;
				}
			}

			// Token: 0x170002F4 RID: 756
			// (get) Token: 0x06000DDB RID: 3547 RVA: 0x0002C1C2 File Offset: 0x0002A3C2
			// (set) Token: 0x06000DDC RID: 3548 RVA: 0x0002C1CF File Offset: 0x0002A3CF
			public string nextWeaponSkill2
			{
				get
				{
					return this._nextWeaponSkill2.value;
				}
				set
				{
					this._nextWeaponSkill2.value = value;
				}
			}

			// Token: 0x170002F5 RID: 757
			// (get) Token: 0x06000DDD RID: 3549 RVA: 0x0002C1DD File Offset: 0x0002A3DD
			// (set) Token: 0x06000DDE RID: 3550 RVA: 0x0002C1EA File Offset: 0x0002A3EA
			public float currentWeaponStack
			{
				get
				{
					return this._currentWeaponStack.value;
				}
				set
				{
					this._currentWeaponStack.value = value;
				}
			}

			// Token: 0x170002F6 RID: 758
			// (get) Token: 0x06000DDF RID: 3551 RVA: 0x0002C1F8 File Offset: 0x0002A3F8
			// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x0002C205 File Offset: 0x0002A405
			public float nextWeaponStack
			{
				get
				{
					return this._nextWeaponStack.value;
				}
				set
				{
					this._nextWeaponStack.value = value;
				}
			}

			// Token: 0x170002F7 RID: 759
			// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x0002C213 File Offset: 0x0002A413
			// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x0002C220 File Offset: 0x0002A420
			public string essence
			{
				get
				{
					return this._essence.value;
				}
				set
				{
					this._essence.value = value;
				}
			}

			// Token: 0x170002F8 RID: 760
			// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x0002C22E File Offset: 0x0002A42E
			// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x0002C23B File Offset: 0x0002A43B
			public float essenceStack
			{
				get
				{
					return this._essenceStack.value;
				}
				set
				{
					this._essenceStack.value = value;
				}
			}

			// Token: 0x170002F9 RID: 761
			// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0002C249 File Offset: 0x0002A449
			// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x0002C251 File Offset: 0x0002A451
			public StringDataArray items { get; private set; }

			// Token: 0x170002FA RID: 762
			// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x0002C25A File Offset: 0x0002A45A
			// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x0002C262 File Offset: 0x0002A462
			public FloatDataArray itemStacks { get; private set; }

			// Token: 0x170002FB RID: 763
			// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x0002C26B File Offset: 0x0002A46B
			// (set) Token: 0x06000DEA RID: 3562 RVA: 0x0002C273 File Offset: 0x0002A473
			public IntDataArray itemKeywords1 { get; private set; }

			// Token: 0x170002FC RID: 764
			// (get) Token: 0x06000DEB RID: 3563 RVA: 0x0002C27C File Offset: 0x0002A47C
			// (set) Token: 0x06000DEC RID: 3564 RVA: 0x0002C284 File Offset: 0x0002A484
			public IntDataArray itemKeywords2 { get; private set; }

			// Token: 0x170002FD RID: 765
			// (get) Token: 0x06000DED RID: 3565 RVA: 0x0002C28D File Offset: 0x0002A48D
			// (set) Token: 0x06000DEE RID: 3566 RVA: 0x0002C295 File Offset: 0x0002A495
			public StringDataArray upgrades { get; private set; }

			// Token: 0x170002FE RID: 766
			// (get) Token: 0x06000DEF RID: 3567 RVA: 0x0002C29E File Offset: 0x0002A49E
			// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x0002C2A6 File Offset: 0x0002A4A6
			public IntDataArray upgradeLevels { get; private set; }

			// Token: 0x170002FF RID: 767
			// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0002C2AF File Offset: 0x0002A4AF
			// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x0002C2B7 File Offset: 0x0002A4B7
			public FloatDataArray upgradeStacks { get; private set; }

			// Token: 0x17000300 RID: 768
			// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x0002C2C0 File Offset: 0x0002A4C0
			// (set) Token: 0x06000DF4 RID: 3572 RVA: 0x0002C2CD File Offset: 0x0002A4CD
			public string abilityBuffs
			{
				get
				{
					return this._abilityBuffs.value;
				}
				set
				{
					this._abilityBuffs.value = value;
				}
			}

			// Token: 0x06000DF5 RID: 3573 RVA: 0x0002C2DC File Offset: 0x0002A4DC
			public void Initialize()
			{
				this.initilaized = true;
				this._hasSave = new BoolData("Save/hasSave", false);
				this._randomSeed = new IntData("Save/randomSeed", false);
				this._health = new IntData("Save/health", false);
				this._chapterIndex = new IntData("Save/chapterIndex", false);
				this._stageIndex = new IntData("Save/stageIndex", false);
				this._pathIndex = new IntData("Save/pathIndex", false);
				this._nodeIndex = new IntData("Save/nodeIndex", false);
				this._currentWeapon = new StringData("Save/currentWeapon", false);
				this._nextWeapon = new StringData("Save/nextWeapon", false);
				this._currentWeaponStack = new FloatData("Save/currentWeaponStack", false);
				this._nextWeaponStack = new FloatData("Save/nextWeaponStack", false);
				this._currentWeaponSkill1 = new StringData("Save/currentWeaponSkill1", false);
				this._currentWeaponSkill2 = new StringData("Save/currentWeaponSkill2", false);
				this._nextWeaponSkill1 = new StringData("Save/nextWeaponSkill1", false);
				this._nextWeaponSkill2 = new StringData("Save/nextWeaponSkill2", false);
				this._essence = new StringData("Save/essence", false);
				this._essenceStack = new FloatData("Save/essenceStack", false);
				this.items = new StringDataArray("Save/items", 9, false);
				this.itemStacks = new FloatDataArray("Save/itemStacks", 9, false);
				this.itemKeywords1 = new IntDataArray("Save/itemKeywords1", 9, false);
				this.itemKeywords2 = new IntDataArray("Save/itemKeywords2", 9, false);
				this.upgrades = new StringDataArray("Save/upgrades", 4, false);
				this.upgradeLevels = new IntDataArray("Save/upgradeLevels", 4, false);
				this.upgradeStacks = new FloatDataArray("Save/upgradeStacks", 4, false);
				this._abilityBuffs = new StringData("Save/abilityBuffs", false);
			}

			// Token: 0x06000DF6 RID: 3574 RVA: 0x0002C4A4 File Offset: 0x0002A6A4
			public void ResetAll()
			{
				this._hasSave.Reset();
				this._randomSeed.value = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
				this._health.Reset();
				this._chapterIndex.Reset();
				this._stageIndex.Reset();
				this._pathIndex.Reset();
				this._nodeIndex.Reset();
				this._currentWeapon.Reset();
				this._nextWeapon.Reset();
				this._currentWeaponStack.Reset();
				this._nextWeaponStack.Reset();
				this._currentWeaponSkill1.Reset();
				this._currentWeaponSkill2.Reset();
				this._nextWeaponSkill1.Reset();
				this._nextWeaponSkill2.Reset();
				this._essence.Reset();
				this._essenceStack.Reset();
				this.items.Reset();
				this.itemStacks.Reset();
				this.itemKeywords1.Reset();
				this.itemKeywords2.Reset();
				this._abilityBuffs.Reset();
				this.upgrades.Reset();
				this.upgradeLevels.Reset();
				this.upgradeStacks.Reset();
				this.SaveAll();
			}

			// Token: 0x06000DF7 RID: 3575 RVA: 0x0002C5DC File Offset: 0x0002A7DC
			public void SaveAll()
			{
				this._hasSave.Save();
				this._randomSeed.Save();
				this._health.Save();
				this._chapterIndex.Save();
				this._stageIndex.Save();
				this._pathIndex.Save();
				this._nodeIndex.Save();
				this._currentWeapon.Save();
				this._nextWeapon.Save();
				this._currentWeaponStack.Save();
				this._nextWeaponStack.Save();
				this._currentWeaponSkill1.Save();
				this._currentWeaponSkill2.Save();
				this._nextWeaponSkill1.Save();
				this._nextWeaponSkill2.Save();
				this._essence.Save();
				this._essenceStack.Save();
				this.items.Save();
				this.itemStacks.Save();
				this.itemKeywords1.Save();
				this.itemKeywords2.Save();
				this._abilityBuffs.Save();
				this.upgrades.Save();
				this.upgradeLevels.Save();
				this.upgradeStacks.Save();
			}

			// Token: 0x06000DF8 RID: 3576 RVA: 0x0002C6FC File Offset: 0x0002A8FC
			public void ResetRandomSeed()
			{
				this._randomSeed.value = UnityEngine.Random.Range(-32768, 32767);
			}

			// Token: 0x06000DF9 RID: 3577 RVA: 0x00002191 File Offset: 0x00000391
			public void DrawEditor()
			{
			}

			// Token: 0x04000B94 RID: 2964
			private const string _key = "Save";

			// Token: 0x04000B95 RID: 2965
			public static readonly GameData.Save instance = new GameData.Save();

			// Token: 0x04000B97 RID: 2967
			private BoolData _hasSave;

			// Token: 0x04000B98 RID: 2968
			private IntData _randomSeed;

			// Token: 0x04000B99 RID: 2969
			private IntData _health;

			// Token: 0x04000B9A RID: 2970
			private IntData _chapterIndex;

			// Token: 0x04000B9B RID: 2971
			private IntData _stageIndex;

			// Token: 0x04000B9C RID: 2972
			private IntData _pathIndex;

			// Token: 0x04000B9D RID: 2973
			private IntData _nodeIndex;

			// Token: 0x04000B9E RID: 2974
			private StringData _currentWeapon;

			// Token: 0x04000B9F RID: 2975
			private StringData _nextWeapon;

			// Token: 0x04000BA0 RID: 2976
			private StringData _currentWeaponSkill1;

			// Token: 0x04000BA1 RID: 2977
			private StringData _currentWeaponSkill2;

			// Token: 0x04000BA2 RID: 2978
			private StringData _nextWeaponSkill1;

			// Token: 0x04000BA3 RID: 2979
			private StringData _nextWeaponSkill2;

			// Token: 0x04000BA4 RID: 2980
			private FloatData _currentWeaponStack;

			// Token: 0x04000BA5 RID: 2981
			private FloatData _nextWeaponStack;

			// Token: 0x04000BA6 RID: 2982
			private StringData _essence;

			// Token: 0x04000BA7 RID: 2983
			private FloatData _essenceStack;

			// Token: 0x04000BAF RID: 2991
			public StringData _abilityBuffs;
		}

		// Token: 0x020002B2 RID: 690
		public class Settings : GameData.IEditorDrawer
		{
			// Token: 0x17000301 RID: 769
			// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0002C724 File Offset: 0x0002A924
			// (set) Token: 0x06000DFD RID: 3581 RVA: 0x0002C735 File Offset: 0x0002A935
			public static string keyBindings
			{
				get
				{
					return GameData.Settings.instance._keyBindings.value;
				}
				set
				{
					GameData.Settings.instance._keyBindings.value = value;
				}
			}

			// Token: 0x17000302 RID: 770
			// (get) Token: 0x06000DFE RID: 3582 RVA: 0x0002C747 File Offset: 0x0002A947
			// (set) Token: 0x06000DFF RID: 3583 RVA: 0x0002C758 File Offset: 0x0002A958
			public static bool arrowDashEnabled
			{
				get
				{
					return GameData.Settings.instance._arrowDashEnabled.value;
				}
				set
				{
					GameData.Settings.instance._arrowDashEnabled.value = value;
				}
			}

			// Token: 0x17000303 RID: 771
			// (get) Token: 0x06000E00 RID: 3584 RVA: 0x0002C76A File Offset: 0x0002A96A
			// (set) Token: 0x06000E01 RID: 3585 RVA: 0x0002C77B File Offset: 0x0002A97B
			public static bool lightEnabled
			{
				get
				{
					return GameData.Settings.instance._lightEnabled.value;
				}
				set
				{
					GameData.Settings.instance._lightEnabled.value = value;
				}
			}

			// Token: 0x17000304 RID: 772
			// (get) Token: 0x06000E02 RID: 3586 RVA: 0x0002C78D File Offset: 0x0002A98D
			// (set) Token: 0x06000E03 RID: 3587 RVA: 0x0002C79E File Offset: 0x0002A99E
			public static float masterVolume
			{
				get
				{
					return GameData.Settings.instance._masterVolume.value;
				}
				set
				{
					GameData.Settings.instance._masterVolume.value = value;
				}
			}

			// Token: 0x17000305 RID: 773
			// (get) Token: 0x06000E04 RID: 3588 RVA: 0x0002C7B0 File Offset: 0x0002A9B0
			// (set) Token: 0x06000E05 RID: 3589 RVA: 0x0002C7C1 File Offset: 0x0002A9C1
			public static bool musicEnabled
			{
				get
				{
					return GameData.Settings.instance._musicEnabled.value;
				}
				set
				{
					GameData.Settings.instance._musicEnabled.value = value;
				}
			}

			// Token: 0x17000306 RID: 774
			// (get) Token: 0x06000E06 RID: 3590 RVA: 0x0002C7D3 File Offset: 0x0002A9D3
			// (set) Token: 0x06000E07 RID: 3591 RVA: 0x0002C7E4 File Offset: 0x0002A9E4
			public static float musicVolume
			{
				get
				{
					return GameData.Settings.instance._musicVolume.value;
				}
				set
				{
					GameData.Settings.instance._musicVolume.value = value;
				}
			}

			// Token: 0x17000307 RID: 775
			// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0002C7F6 File Offset: 0x0002A9F6
			// (set) Token: 0x06000E09 RID: 3593 RVA: 0x0002C807 File Offset: 0x0002AA07
			public static bool sfxEnabled
			{
				get
				{
					return GameData.Settings.instance._sfxEnabled.value;
				}
				set
				{
					GameData.Settings.instance._sfxEnabled.value = value;
				}
			}

			// Token: 0x17000308 RID: 776
			// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0002C819 File Offset: 0x0002AA19
			// (set) Token: 0x06000E0B RID: 3595 RVA: 0x0002C82A File Offset: 0x0002AA2A
			public static float sfxVolume
			{
				get
				{
					return GameData.Settings.instance._sfxVolume.value;
				}
				set
				{
					GameData.Settings.instance._sfxVolume.value = value;
				}
			}

			// Token: 0x17000309 RID: 777
			// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0002C83C File Offset: 0x0002AA3C
			// (set) Token: 0x06000E0D RID: 3597 RVA: 0x0002C84D File Offset: 0x0002AA4D
			public static int language
			{
				get
				{
					return GameData.Settings.instance._language.value;
				}
				set
				{
					GameData.Settings.instance._language.value = value;
				}
			}

			// Token: 0x1700030A RID: 778
			// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0002C85F File Offset: 0x0002AA5F
			// (set) Token: 0x06000E0F RID: 3599 RVA: 0x0002C870 File Offset: 0x0002AA70
			public static float cameraShakeIntensity
			{
				get
				{
					return GameData.Settings.instance._cameraShakeIntensity.value;
				}
				set
				{
					GameData.Settings.instance._cameraShakeIntensity.value = value;
				}
			}

			// Token: 0x1700030B RID: 779
			// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0002C882 File Offset: 0x0002AA82
			// (set) Token: 0x06000E11 RID: 3601 RVA: 0x0002C893 File Offset: 0x0002AA93
			public static float vibrationIntensity
			{
				get
				{
					return GameData.Settings.instance._vibrationIntensity.value;
				}
				set
				{
					GameData.Settings.instance._vibrationIntensity.value = value;
				}
			}

			// Token: 0x1700030C RID: 780
			// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0002C8A5 File Offset: 0x0002AAA5
			// (set) Token: 0x06000E13 RID: 3603 RVA: 0x0002C8B6 File Offset: 0x0002AAB6
			public static int particleQuality
			{
				get
				{
					return GameData.Settings.instance._particleQuality.value;
				}
				set
				{
					GameData.Settings.instance._particleQuality.value = value;
				}
			}

			// Token: 0x1700030D RID: 781
			// (get) Token: 0x06000E14 RID: 3604 RVA: 0x0002C8C8 File Offset: 0x0002AAC8
			// (set) Token: 0x06000E15 RID: 3605 RVA: 0x0002C8D9 File Offset: 0x0002AAD9
			public static bool easyMode
			{
				get
				{
					return GameData.Settings.instance._easyMode.value;
				}
				set
				{
					GameData.Settings.instance._easyMode.value = value;
				}
			}

			// Token: 0x1700030E RID: 782
			// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0002C8EB File Offset: 0x0002AAEB
			// (set) Token: 0x06000E17 RID: 3607 RVA: 0x0002C8FC File Offset: 0x0002AAFC
			public static bool showTimer
			{
				get
				{
					return GameData.Settings.instance._showTimer.value;
				}
				set
				{
					GameData.Settings.instance._showTimer.value = value;
				}
			}

			// Token: 0x06000E18 RID: 3608 RVA: 0x0002C910 File Offset: 0x0002AB10
			public static void Save()
			{
				GameData.Settings.instance._keyBindings.Save();
				GameData.Settings.instance._arrowDashEnabled.Save();
				GameData.Settings.instance._lightEnabled.Save();
				GameData.Settings.instance._masterVolume.Save();
				GameData.Settings.instance._musicEnabled.Save();
				GameData.Settings.instance._musicVolume.Save();
				GameData.Settings.instance._sfxEnabled.Save();
				GameData.Settings.instance._sfxVolume.Save();
				GameData.Settings.instance._language.Save();
				GameData.Settings.instance._cameraShakeIntensity.Save();
				GameData.Settings.instance._vibrationIntensity.Save();
				GameData.Settings.instance._particleQuality.Save();
				GameData.Settings.instance._easyMode.Save();
				GameData.Settings.instance._showTimer.Save();
				PlayerPrefs.Save();
			}

			// Token: 0x06000E19 RID: 3609 RVA: 0x0002C9F4 File Offset: 0x0002ABF4
			public void Initialize()
			{
				this._keyBindings = new StringData("Settings/keyBindings", string.Empty, true);
				this._arrowDashEnabled = new BoolData("Settings/arrowDashEnabled", false, false);
				bool defaultValue = Application.isConsolePlatform || SystemInfo.systemMemorySize >= 4000 || SystemInfo.graphicsMemorySize >= 1000;
				this._lightEnabled = new BoolData("Settings/lightEnabled", defaultValue, false);
				Light2D.lightEnabled = this._lightEnabled.value;
				this._masterVolume = new FloatData("Settings/masterVolume", 0.8f, false);
				this._musicEnabled = new BoolData("Settings/musicEnabled", true, false);
				this._musicVolume = new FloatData("Settings/musicVolume", 0.6f, false);
				this._sfxEnabled = new BoolData("Settings/sfxEnabled", true, false);
				this._sfxVolume = new FloatData("Settings/sfxVolume", 0.8f, false);
				this._language = new IntData("Settings/language", -1, false);
				Localization.ValidateLanguage();
				this._cameraShakeIntensity = new FloatData("Settings/cameraShakeIntensity", 0.5f, false);
				this._vibrationIntensity = new FloatData("Settings/vibrationIntensity", 0.5f, false);
				this._particleQuality = new IntData("Settings/particleQuality", 3, false);
				this._easyMode = new BoolData("Settings/easyMode", false, false);
				this._showTimer = new BoolData("Settings/showTimer", false, false);
			}

			// Token: 0x06000E1A RID: 3610 RVA: 0x0002CB50 File Offset: 0x0002AD50
			public void ResetKeyBindings()
			{
				this._keyBindings.Reset();
			}

			// Token: 0x06000E1B RID: 3611 RVA: 0x0002CB5D File Offset: 0x0002AD5D
			public void ResetLanguage()
			{
				this._language.Reset();
				Localization.ValidateLanguage();
			}

			// Token: 0x06000E1C RID: 3612 RVA: 0x00002191 File Offset: 0x00000391
			public void DrawEditor()
			{
			}

			// Token: 0x04000BB0 RID: 2992
			public static readonly GameData.Settings instance = new GameData.Settings();

			// Token: 0x04000BB1 RID: 2993
			private StringData _keyBindings;

			// Token: 0x04000BB2 RID: 2994
			private BoolData _arrowDashEnabled;

			// Token: 0x04000BB3 RID: 2995
			private BoolData _lightEnabled;

			// Token: 0x04000BB4 RID: 2996
			private FloatData _masterVolume;

			// Token: 0x04000BB5 RID: 2997
			private BoolData _musicEnabled;

			// Token: 0x04000BB6 RID: 2998
			private FloatData _musicVolume;

			// Token: 0x04000BB7 RID: 2999
			private BoolData _sfxEnabled;

			// Token: 0x04000BB8 RID: 3000
			private FloatData _sfxVolume;

			// Token: 0x04000BB9 RID: 3001
			private IntData _language;

			// Token: 0x04000BBA RID: 3002
			private FloatData _cameraShakeIntensity;

			// Token: 0x04000BBB RID: 3003
			private FloatData _vibrationIntensity;

			// Token: 0x04000BBC RID: 3004
			private IntData _particleQuality;

			// Token: 0x04000BBD RID: 3005
			private BoolData _easyMode;

			// Token: 0x04000BBE RID: 3006
			private BoolData _showTimer;
		}
	}
}
