using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Characters;
using Characters.Player;
using Data;
using InControl;
using Level;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources
{
	// Token: 0x02000179 RID: 377
	[PreferBinarySerialization]
	public class CommonResource : ScriptableObject
	{
		// Token: 0x060007C6 RID: 1990 RVA: 0x00017739 File Offset: 0x00015939
		public static string AssetPathToResourcesPath(string path)
		{
			return path.Replace('\\', '/');
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00017745 File Offset: 0x00015945
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x0001774C File Offset: 0x0001594C
		public static CommonResource instance { get; private set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00017754 File Offset: 0x00015954
		public AudioClip bossRewardActiveSound
		{
			get
			{
				return this._bossRewardActiveSound;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x0001775C File Offset: 0x0001595C
		public RuntimeAnimatorController bossRewardActive
		{
			get
			{
				return this._bossRewardActive;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00017764 File Offset: 0x00015964
		public RuntimeAnimatorController bossRewardDeactive
		{
			get
			{
				return this._bossRewardDeactive;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x0001776C File Offset: 0x0001596C
		public RuntimeAnimatorController destroyWeapon
		{
			get
			{
				return this._destroyWeapon;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00017774 File Offset: 0x00015974
		public RuntimeAnimatorController destroyItem
		{
			get
			{
				return this._destroyItem;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0001777C File Offset: 0x0001597C
		public RuntimeAnimatorController destroyEssence
		{
			get
			{
				return this._destroyEssence;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00017784 File Offset: 0x00015984
		public Character player
		{
			get
			{
				return this._player;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0001778C File Offset: 0x0001598C
		public PlayerDieHeadParts playerDieHeadParts
		{
			get
			{
				return this._playerDieHeadParts;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00017794 File Offset: 0x00015994
		public ParticleEffectInfo freezeLargeParticle
		{
			get
			{
				return this._freezeLargeParticle;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0001779C File Offset: 0x0001599C
		public ParticleEffectInfo freezeMediumParticle
		{
			get
			{
				return this._freezeMediumParticle;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x000177A4 File Offset: 0x000159A4
		public ParticleEffectInfo freezeMediumParticle2
		{
			get
			{
				return this._freezeMediumParticle2;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x000177AC File Offset: 0x000159AC
		public ParticleEffectInfo freezeSmallParticle
		{
			get
			{
				return this._freezeSmallParticle;
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x000177B4 File Offset: 0x000159B4
		public RuntimeAnimatorController GetFreezeAnimator(Vector2 pixelSize)
		{
			CommonResource.<>c__DisplayClass76_0 CS$<>8__locals1;
			CS$<>8__locals1.pixelSize = pixelSize;
			int num = 1;
			while (!CommonResource.<GetFreezeAnimator>g__Fit|76_0((float)(40 * num), (float)(50 * num), ref CS$<>8__locals1))
			{
				if (CommonResource.<GetFreezeAnimator>g__Fit|76_0((float)(64 * num), (float)(66 * num), ref CS$<>8__locals1))
				{
					return this._freezeMedium1;
				}
				if (CommonResource.<GetFreezeAnimator>g__Fit|76_0((float)(79 * num), (float)(71 * num), ref CS$<>8__locals1))
				{
					return this._freezeMedium2;
				}
				if (CommonResource.<GetFreezeAnimator>g__Fit|76_0((float)(125 * num), (float)(120 * num), ref CS$<>8__locals1))
				{
					return this._freezeLarge;
				}
				num++;
			}
			return this._freezeSmall;
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x00017838 File Offset: 0x00015A38
		public Potion smallPotion
		{
			get
			{
				return this._smallPotion;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x00017840 File Offset: 0x00015A40
		public Potion mediumPotion
		{
			get
			{
				return this._mediumPotion;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x00017848 File Offset: 0x00015A48
		public Potion largePotion
		{
			get
			{
				return this._largePotion;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00017850 File Offset: 0x00015A50
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x00017858 File Offset: 0x00015A58
		public EnumArray<Potion.Size, Potion> potions { get; private set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00017861 File Offset: 0x00015A61
		public Sprite flexibleSpineIcon
		{
			get
			{
				return this._flexibleSpineIcon;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00017869 File Offset: 0x00015A69
		public Sprite soulAccelerationIcon
		{
			get
			{
				return this._soulAccelerationIcon;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00017871 File Offset: 0x00015A71
		public Sprite reassembleIcon
		{
			get
			{
				return this._reassembleIcon;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00017879 File Offset: 0x00015A79
		public PoolObject emptyEffect
		{
			get
			{
				return this._emptyEffect;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00017881 File Offset: 0x00015A81
		public PoolObject vignetteEffect
		{
			get
			{
				return this._vignetteEffect;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x00017889 File Offset: 0x00015A89
		public PoolObject screenFlashEffect
		{
			get
			{
				return this._screenFlashEffect;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00017891 File Offset: 0x00015A91
		public Sprite curseOfLightIcon
		{
			get
			{
				return this._curseOfLightIcon;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00017899 File Offset: 0x00015A99
		public RuntimeAnimatorController curseOfLightAttachEffect
		{
			get
			{
				return this._curseOfLightAttachEffect;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x000178A1 File Offset: 0x00015AA1
		public RuntimeAnimatorController enemyInSightEffect
		{
			get
			{
				return this._enemyInSightEffect;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x000178A9 File Offset: 0x00015AA9
		public RuntimeAnimatorController enemyAppearanceEffect
		{
			get
			{
				return this._enemyAppearanceEffect;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x000178B1 File Offset: 0x00015AB1
		public RuntimeAnimatorController poisonEffect
		{
			get
			{
				return this._poisonEffect;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x000178B9 File Offset: 0x00015AB9
		public RuntimeAnimatorController slowEffect
		{
			get
			{
				return this._slowEffect;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x000178C1 File Offset: 0x00015AC1
		public RuntimeAnimatorController bindingEffect
		{
			get
			{
				return this._bindingEffect;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x000178C9 File Offset: 0x00015AC9
		public RuntimeAnimatorController bleedEffect
		{
			get
			{
				return this._bleedEffect;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x000178D1 File Offset: 0x00015AD1
		public RuntimeAnimatorController stunEffect
		{
			get
			{
				return this._stunEffect;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x000178D9 File Offset: 0x00015AD9
		public RuntimeAnimatorController swapEffect
		{
			get
			{
				return this._swapEffect;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x000178E1 File Offset: 0x00015AE1
		public GameObject hardmodeChest
		{
			get
			{
				return this._hardmodeChest;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x000178E9 File Offset: 0x00015AE9
		public PoolObject goldParticle
		{
			get
			{
				return this._goldParticle;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x000178F1 File Offset: 0x00015AF1
		public PoolObject darkQuartzParticle
		{
			get
			{
				return this._darkQuartzParticle;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x000178F9 File Offset: 0x00015AF9
		public PoolObject boneParticle
		{
			get
			{
				return this._boneParticle;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x00017901 File Offset: 0x00015B01
		public PoolObject heartQuartzParticle
		{
			get
			{
				return this._heartQuartzParticle;
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00017909 File Offset: 0x00015B09
		public PoolObject GetCurrencyParticle(GameData.Currency.Type type)
		{
			switch (type)
			{
			case GameData.Currency.Type.Gold:
				return this._goldParticle;
			case GameData.Currency.Type.DarkQuartz:
				return this._darkQuartzParticle;
			case GameData.Currency.Type.Bone:
				return this._boneParticle;
			case GameData.Currency.Type.HeartQuartz:
				return this._heartQuartzParticle;
			default:
				return null;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00017940 File Offset: 0x00015B40
		public PoolObject droppedSkulHead
		{
			get
			{
				return this._droppedSkulHead;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00017948 File Offset: 0x00015B48
		public PoolObject droppedHeroSkulHead
		{
			get
			{
				return this._droppedHeroSkulHead;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x00017950 File Offset: 0x00015B50
		public Sprite pixelSprite
		{
			get
			{
				return this._pixelSprite;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x00017958 File Offset: 0x00015B58
		public Sprite emptySprite
		{
			get
			{
				return this._emptySprite;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00017960 File Offset: 0x00015B60
		public SpriteRenderer footShadow
		{
			get
			{
				return this._footShadow;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x00017968 File Offset: 0x00015B68
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x00017970 File Offset: 0x00015B70
		public RenderTexture deathCamRenderTexture { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x00017979 File Offset: 0x00015B79
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x00017981 File Offset: 0x00015B81
		public Dictionary<string, Sprite> keywordIconDictionary { get; private set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001798A File Offset: 0x00015B8A
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x00017992 File Offset: 0x00015B92
		public Dictionary<string, Sprite> keywordFullactiveIconDictionary { get; private set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x0001799B File Offset: 0x00015B9B
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x000179A3 File Offset: 0x00015BA3
		public Dictionary<string, Sprite> keywordDeactiveIconDictionary { get; private set; }

		// Token: 0x060007FE RID: 2046 RVA: 0x000179AC File Offset: 0x00015BAC
		public Sprite GetKeyIconOrDefault(BindingSource bindingSource, bool outline = false)
		{
			Sprite result;
			if (this.TryGetKeyIcon(bindingSource, out result, outline))
			{
				return result;
			}
			return (outline ? this._controllerButtonOutlineDictionary : this._controllerButtonDictionary)["unknown"];
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000179E4 File Offset: 0x00015BE4
		public bool TryGetKeyIcon(BindingSource bindingSource, out Sprite sprite, bool outline = false)
		{
			KeyBindingSource keyBindingSource = bindingSource as KeyBindingSource;
			if (keyBindingSource != null)
			{
				return (outline ? this._keyboardButtonOutlineDictionary : this._keyboardButtonDictionary).TryGetValue(keyBindingSource.Control.GetInclude(0).ToString().Trim(), out sprite);
			}
			MouseBindingSource mouseBindingSource = bindingSource as MouseBindingSource;
			if (mouseBindingSource != null)
			{
				return (outline ? this._mouseButtonOutlineDictionary : this._mouseButtonDictionary).TryGetValue(mouseBindingSource.Control.ToString(), out sprite);
			}
			DeviceBindingSource deviceBindingSource = bindingSource as DeviceBindingSource;
			if (deviceBindingSource == null)
			{
				sprite = null;
				return false;
			}
			string text = deviceBindingSource.Control.ToString();
			if (deviceBindingSource.Control == InputControlType.Start || deviceBindingSource.Control == InputControlType.Options || deviceBindingSource.Control == InputControlType.RightCommand)
			{
				text = InputControlType.Menu.ToString();
			}
			if (deviceBindingSource.Control == InputControlType.Back || deviceBindingSource.Control == InputControlType.View || deviceBindingSource.Control == InputControlType.Share || deviceBindingSource.Control == InputControlType.Pause || deviceBindingSource.Control == InputControlType.LeftCommand)
			{
				text = InputControlType.Select.ToString();
			}
			Dictionary<string, Sprite> dictionary = outline ? this._controllerButtonOutlineDictionary : this._controllerButtonDictionary;
			if ((deviceBindingSource.DeviceStyle == InputDeviceStyle.PlayStation2 || deviceBindingSource.DeviceStyle == InputDeviceStyle.PlayStation3 || deviceBindingSource.DeviceStyle == InputDeviceStyle.PlayStation4 || deviceBindingSource.DeviceStyle == InputDeviceStyle.PlayStation5 || deviceBindingSource.DeviceStyle == InputDeviceStyle.PlayStationMove || deviceBindingSource.DeviceStyle == InputDeviceStyle.PlayStationVita) && dictionary.TryGetValue("PS_" + text, out sprite))
			{
				return sprite;
			}
			if (deviceBindingSource.DeviceStyle == InputDeviceStyle.Nintendo64 || deviceBindingSource.DeviceStyle == InputDeviceStyle.NintendoGameCube || deviceBindingSource.DeviceStyle == InputDeviceStyle.NintendoNES || deviceBindingSource.DeviceStyle == InputDeviceStyle.NintendoSNES || deviceBindingSource.DeviceStyle == InputDeviceStyle.NintendoSwitch || deviceBindingSource.DeviceStyle == InputDeviceStyle.NintendoWii || deviceBindingSource.DeviceStyle == InputDeviceStyle.NintendoWiiU)
			{
				if (text.Equals(InputControlType.Menu.ToString(), StringComparison.InvariantCultureIgnoreCase))
				{
					text = InputControlType.Plus.ToString();
				}
				if (text.Equals(InputControlType.Select.ToString(), StringComparison.InvariantCultureIgnoreCase))
				{
					text = InputControlType.Minus.ToString();
				}
				if (dictionary.TryGetValue("NSW_" + text, out sprite))
				{
					return sprite;
				}
			}
			return dictionary.TryGetValue(text, out sprite);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00017C5B File Offset: 0x00015E5B
		public static T[] LoadAll<T>(string path, string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly) where T : UnityEngine.Object
		{
			return new T[0];
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00017C63 File Offset: 0x00015E63
		public static KeyValuePair<string, T>[] LoadAllWithPath<T>(string path, string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly) where T : UnityEngine.Object
		{
			return new KeyValuePair<string, T>[0];
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateResource(bool updateWeaponUpgrades)
		{
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00017C6C File Offset: 0x00015E6C
		public void Initialize()
		{
			CommonResource.instance = this;
			base.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			this.potions = new EnumArray<Potion.Size, Potion>(new Potion[]
			{
				this._smallPotion,
				this._mediumPotion,
				this._largePotion
			});
			this._keyboardButtonDictionary = this._keyboardButtons.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this._keyboardButtonOutlineDictionary = this._keyboardButtonsOutline.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this._mouseButtonDictionary = this._mouseButtons.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this._mouseButtonOutlineDictionary = this._mouseButtonsOutline.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this._controllerButtonDictionary = this._controllerButtons.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this._controllerButtonOutlineDictionary = this._controllerButtonsOutline.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this.keywordIconDictionary = this._keywordIcons.ToDictionary((Sprite sprite) => sprite.name);
			this.keywordFullactiveIconDictionary = this._keywordFullactiveIcons.ToDictionary((Sprite sprite) => sprite.name.Split(new char[]
			{
				'_'
			})[0]);
			this.keywordDeactiveIconDictionary = this._keywordDeactiveIcons.ToDictionary((Sprite sprite) => sprite.name.Split(new char[]
			{
				'_'
			})[0]);
			this.deathCamRenderTexture = this._deathCamRenderTexture.LoadAssetAsync<RenderTexture>().WaitForCompletion();
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00017EA1 File Offset: 0x000160A1
		[CompilerGenerated]
		internal static bool <GetFreezeAnimator>g__Fit|76_0(float x, float y, ref CommonResource.<>c__DisplayClass76_0 A_2)
		{
			return A_2.pixelSize.x < x && A_2.pixelSize.y < y;
		}

		// Token: 0x040005EB RID: 1515
		public const string assets = "Assets";

		// Token: 0x040005EC RID: 1516
		public const string resources = "Resources";

		// Token: 0x040005ED RID: 1517
		private const string audio = "Audio/";

		// Token: 0x040005EE RID: 1518
		public const string sfx = "Audio/Sfx/";

		// Token: 0x040005EF RID: 1519
		public const string music = "Audio/Music/";

		// Token: 0x040005F0 RID: 1520
		public const string level = "Assets/Level/";

		// Token: 0x040005F1 RID: 1521
		public const string levelCastle = "Assets/Level/Castle/";

		// Token: 0x040005F2 RID: 1522
		public const string levelTest = "Assets/Level/Test/";

		// Token: 0x040005F3 RID: 1523
		public const string levelChapter1 = "Assets/Level/Chapter1/";

		// Token: 0x040005F4 RID: 1524
		public const string levelChapter2 = "Assets/Level/Chapter2/";

		// Token: 0x040005F5 RID: 1525
		public const string levelChapter3 = "Assets/Level/Chapter3/";

		// Token: 0x040005F6 RID: 1526
		public const string levelChapter4 = "Assets/Level/Chapter4/";

		// Token: 0x040005F7 RID: 1527
		public const string levelChapter5 = "Assets/Level/Chapter5/";

		// Token: 0x040005F8 RID: 1528
		public const string strings = "Strings";

		// Token: 0x040005F9 RID: 1529
		public const string shaders = "Shaders/";

		// Token: 0x040005FA RID: 1530
		public const string enemy = "Assets/Enemies/";

		// Token: 0x040005FB RID: 1531
		public const string enemyElite = "Assets/Enemies/Elite/";

		// Token: 0x040005FC RID: 1532
		public const string enemyBoss = "Assets/Enemies/Boss/";

		// Token: 0x040005FD RID: 1533
		public const string enemyChapter1 = "Assets/Enemies/Chapter1/";

		// Token: 0x040005FE RID: 1534
		public const string enemyChapter2 = "Assets/Enemies/Chapter2/";

		// Token: 0x040005FF RID: 1535
		public const string enemyChapter3 = "Assets/Enemies/Chapter3/";

		// Token: 0x04000600 RID: 1536
		public const string enemyChapter4 = "Assets/Enemies/Chapter4/";

		// Token: 0x04000601 RID: 1537
		public const string parts = "Parts/";

		// Token: 0x04000602 RID: 1538
		public const string followers = "Followers/";

		// Token: 0x04000603 RID: 1539
		public const string monsters = "Monsters/";

		// Token: 0x04000604 RID: 1540
		public const string weaponDirectory = "Gear/Weapons/";

		// Token: 0x04000605 RID: 1541
		public const string itemDirectory = "Gear/Items/";

		// Token: 0x04000606 RID: 1542
		public const string quintessenceDirectory = "Gear/Quintessences/";

		// Token: 0x04000607 RID: 1543
		public const string upgradeDirectory = "Upgrades/Objects/";

		// Token: 0x04000609 RID: 1545
		[SerializeField]
		private AudioClip _bossRewardActiveSound;

		// Token: 0x0400060A RID: 1546
		[SerializeField]
		private RuntimeAnimatorController _bossRewardActive;

		// Token: 0x0400060B RID: 1547
		[SerializeField]
		private RuntimeAnimatorController _bossRewardDeactive;

		// Token: 0x0400060C RID: 1548
		[SerializeField]
		private RuntimeAnimatorController _destroyWeapon;

		// Token: 0x0400060D RID: 1549
		[SerializeField]
		private RuntimeAnimatorController _destroyItem;

		// Token: 0x0400060E RID: 1550
		[SerializeField]
		private RuntimeAnimatorController _destroyEssence;

		// Token: 0x0400060F RID: 1551
		public ParticleEffectInfo hitParticle;

		// Token: 0x04000610 RID: 1552
		public ParticleEffectInfo reassembleParticle;

		// Token: 0x04000611 RID: 1553
		[SerializeField]
		private Character _player;

		// Token: 0x04000612 RID: 1554
		[SerializeField]
		private PlayerDieHeadParts _playerDieHeadParts;

		// Token: 0x04000613 RID: 1555
		[SerializeField]
		[Space]
		private ParticleEffectInfo _freezeLargeParticle;

		// Token: 0x04000614 RID: 1556
		[SerializeField]
		private ParticleEffectInfo _freezeMediumParticle;

		// Token: 0x04000615 RID: 1557
		[SerializeField]
		private ParticleEffectInfo _freezeMediumParticle2;

		// Token: 0x04000616 RID: 1558
		[SerializeField]
		private ParticleEffectInfo _freezeSmallParticle;

		// Token: 0x04000617 RID: 1559
		[SerializeField]
		[Space]
		private RuntimeAnimatorController _freezeLarge;

		// Token: 0x04000618 RID: 1560
		[SerializeField]
		private RuntimeAnimatorController _freezeMedium1;

		// Token: 0x04000619 RID: 1561
		[SerializeField]
		private RuntimeAnimatorController _freezeMedium2;

		// Token: 0x0400061A RID: 1562
		[SerializeField]
		private RuntimeAnimatorController _freezeSmall;

		// Token: 0x0400061B RID: 1563
		[SerializeField]
		[Space]
		private Potion _smallPotion;

		// Token: 0x0400061C RID: 1564
		[SerializeField]
		private Potion _mediumPotion;

		// Token: 0x0400061D RID: 1565
		[SerializeField]
		private Potion _largePotion;

		// Token: 0x0400061F RID: 1567
		[SerializeField]
		[Space]
		private Sprite _flexibleSpineIcon;

		// Token: 0x04000620 RID: 1568
		[SerializeField]
		private Sprite _soulAccelerationIcon;

		// Token: 0x04000621 RID: 1569
		[SerializeField]
		private Sprite _reassembleIcon;

		// Token: 0x04000622 RID: 1570
		[SerializeField]
		private PoolObject _emptyEffect;

		// Token: 0x04000623 RID: 1571
		[SerializeField]
		private PoolObject _vignetteEffect;

		// Token: 0x04000624 RID: 1572
		[SerializeField]
		private PoolObject _screenFlashEffect;

		// Token: 0x04000625 RID: 1573
		[SerializeField]
		private Sprite _curseOfLightIcon;

		// Token: 0x04000626 RID: 1574
		[SerializeField]
		[Space]
		private RuntimeAnimatorController _curseOfLightAttachEffect;

		// Token: 0x04000627 RID: 1575
		[SerializeField]
		private RuntimeAnimatorController _enemyInSightEffect;

		// Token: 0x04000628 RID: 1576
		[SerializeField]
		private RuntimeAnimatorController _enemyAppearanceEffect;

		// Token: 0x04000629 RID: 1577
		[SerializeField]
		private RuntimeAnimatorController _poisonEffect;

		// Token: 0x0400062A RID: 1578
		[SerializeField]
		private RuntimeAnimatorController _slowEffect;

		// Token: 0x0400062B RID: 1579
		[SerializeField]
		private RuntimeAnimatorController _bindingEffect;

		// Token: 0x0400062C RID: 1580
		[SerializeField]
		private RuntimeAnimatorController _bleedEffect;

		// Token: 0x0400062D RID: 1581
		[SerializeField]
		private RuntimeAnimatorController _stunEffect;

		// Token: 0x0400062E RID: 1582
		[SerializeField]
		private RuntimeAnimatorController _swapEffect;

		// Token: 0x0400062F RID: 1583
		[Space]
		[SerializeField]
		private GameObject _hardmodeChest;

		// Token: 0x04000630 RID: 1584
		[SerializeField]
		[Space]
		private PoolObject _goldParticle;

		// Token: 0x04000631 RID: 1585
		[SerializeField]
		private PoolObject _darkQuartzParticle;

		// Token: 0x04000632 RID: 1586
		[SerializeField]
		private PoolObject _boneParticle;

		// Token: 0x04000633 RID: 1587
		[SerializeField]
		private PoolObject _heartQuartzParticle;

		// Token: 0x04000634 RID: 1588
		[SerializeField]
		private PoolObject _droppedSkulHead;

		// Token: 0x04000635 RID: 1589
		[SerializeField]
		private PoolObject _droppedHeroSkulHead;

		// Token: 0x04000636 RID: 1590
		[SerializeField]
		private Sprite _pixelSprite;

		// Token: 0x04000637 RID: 1591
		[SerializeField]
		private Sprite _emptySprite;

		// Token: 0x04000638 RID: 1592
		[SerializeField]
		private SpriteRenderer _footShadow;

		// Token: 0x04000639 RID: 1593
		[SerializeField]
		private AssetReference _deathCamRenderTexture;

		// Token: 0x0400063B RID: 1595
		[SerializeField]
		[Space]
		private Sprite[] _keyboardButtons;

		// Token: 0x0400063C RID: 1596
		private Dictionary<string, Sprite> _keyboardButtonDictionary;

		// Token: 0x0400063D RID: 1597
		[SerializeField]
		private Sprite[] _keyboardButtonsOutline;

		// Token: 0x0400063E RID: 1598
		private Dictionary<string, Sprite> _keyboardButtonOutlineDictionary;

		// Token: 0x0400063F RID: 1599
		[SerializeField]
		private Sprite[] _mouseButtons;

		// Token: 0x04000640 RID: 1600
		private Dictionary<string, Sprite> _mouseButtonDictionary;

		// Token: 0x04000641 RID: 1601
		[SerializeField]
		private Sprite[] _mouseButtonsOutline;

		// Token: 0x04000642 RID: 1602
		private Dictionary<string, Sprite> _mouseButtonOutlineDictionary;

		// Token: 0x04000643 RID: 1603
		[SerializeField]
		private Sprite[] _controllerButtons;

		// Token: 0x04000644 RID: 1604
		private Dictionary<string, Sprite> _controllerButtonDictionary;

		// Token: 0x04000645 RID: 1605
		[SerializeField]
		private Sprite[] _controllerButtonsOutline;

		// Token: 0x04000646 RID: 1606
		private Dictionary<string, Sprite> _controllerButtonOutlineDictionary;

		// Token: 0x04000647 RID: 1607
		[SerializeField]
		[Space]
		private Sprite[] _keywordIcons;

		// Token: 0x04000648 RID: 1608
		[SerializeField]
		private Sprite[] _keywordFullactiveIcons;

		// Token: 0x04000649 RID: 1609
		[SerializeField]
		private Sprite[] _keywordDeactiveIcons;
	}
}
