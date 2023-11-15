using System;
using Characters;
using Characters.Controllers;
using Data;
using GameResources;
using Level;
using Platforms;
using Scenes;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.TestingTool
{
	// Token: 0x0200040C RID: 1036
	public class Panel : Dialogue
	{
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x0003B35C File Offset: 0x0003955C
		public bool canUse
		{
			get
			{
				return PersistentSingleton<PlatformManager>.Instance.cheatEnabled;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool closeWithPauseKey
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x0003B368 File Offset: 0x00039568
		public void Open(Panel.Type type)
		{
			foreach (GameObject gameObject in this._panels)
			{
				gameObject.SetActive(false);
			}
			this._panels[type].SetActive(true);
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0003B3C8 File Offset: 0x000395C8
		public void OpenMain()
		{
			this.Open(Panel.Type.Main);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0003B3D1 File Offset: 0x000395D1
		public void OpenMapList()
		{
			this.Open(Panel.Type.MapList);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0003B3DA File Offset: 0x000395DA
		public void OpenGearList()
		{
			this.Open(Panel.Type.GearList);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0003B3E3 File Offset: 0x000395E3
		public void OpenLog()
		{
			this.Open(Panel.Type.Log);
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0003B3EC File Offset: 0x000395EC
		public void OpenDataControl()
		{
			this.Open(Panel.Type.DataControl);
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0003B3F5 File Offset: 0x000395F5
		public void OpenBonusStat()
		{
			this.Open(Panel.Type.BonusStat);
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0003B400 File Offset: 0x00039600
		private void Awake()
		{
			this._panels = new EnumArray<Panel.Type, GameObject>(new GameObject[]
			{
				this._main,
				this._mapList,
				this._gearList,
				this._log.gameObject,
				this._dataControl,
				this._bonusStatPanel
			});
			this._log.StartLog();
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			CommonResource instance = CommonResource.instance;
			this._version.text = "version : " + Application.version;
			this._awake.onClick.AddListener(delegate
			{
				levelManager.player.playerComponents.inventory.weapon.UpgradeCurrentWeapon();
			});
			this._testMap.onClick.AddListener(delegate
			{
				levelManager.EnterOutTrack(this._testMapPrefab, false);
			});
			this._chapter1.onClick.AddListener(delegate
			{
				if (GameData.HardmodeProgress.hardmode)
				{
					levelManager.Load(Chapter.Type.HardmodeChapter1);
					return;
				}
				levelManager.Load(Chapter.Type.Chapter1);
			});
			this._chapter2.onClick.AddListener(delegate
			{
				if (GameData.HardmodeProgress.hardmode)
				{
					levelManager.Load(Chapter.Type.HardmodeChapter2);
					return;
				}
				levelManager.Load(Chapter.Type.Chapter2);
			});
			this._chapter3.onClick.AddListener(delegate
			{
				if (GameData.HardmodeProgress.hardmode)
				{
					levelManager.Load(Chapter.Type.HardmodeChapter3);
					return;
				}
				levelManager.Load(Chapter.Type.Chapter3);
			});
			this._chapter4.onClick.AddListener(delegate
			{
				if (GameData.HardmodeProgress.hardmode)
				{
					levelManager.Load(Chapter.Type.HardmodeChapter4);
					return;
				}
				levelManager.Load(Chapter.Type.Chapter4);
			});
			this._chapter5.onClick.AddListener(delegate
			{
				if (GameData.HardmodeProgress.hardmode)
				{
					levelManager.Load(Chapter.Type.HardmodeChapter5);
					return;
				}
				levelManager.Load(Chapter.Type.Chapter5);
			});
			this._nextStage.onClick.AddListener(delegate
			{
				levelManager.LoadNextStage();
			});
			this._nextMap.onClick.AddListener(delegate
			{
				levelManager.LoadNextMap(NodeIndex.Node1);
			});
			this._hideUI.onClick.AddListener(delegate
			{
				Scene<GameBase>.instance.uiManager.headupDisplay.visible = !Scene<GameBase>.instance.uiManager.headupDisplay.visible;
			});
			this._getGold.onClick.AddListener(delegate
			{
				GameData.Currency.gold.Earn(10000);
			});
			this._getDarkquartz.onClick.AddListener(delegate
			{
				GameData.Currency.darkQuartz.Earn(1000);
			});
			this._getBone.onClick.AddListener(delegate
			{
				GameData.Currency.bone.Earn(100);
			});
			this._getHeartQuartz.onClick.AddListener(delegate
			{
				GameData.Currency.heartQuartz.Earn(100);
			});
			this._right3.onClick.AddListener(delegate
			{
				for (int i = 0; i < 3; i++)
				{
					this._damageBuff.onClick.Invoke();
					this._noCooldown.onClick.Invoke();
					this._hp10k.onClick.Invoke();
				}
			});
			this._damageBuff.onClick.AddListener(delegate
			{
				this._damageBuffAttached = !this._damageBuffAttached;
				if (this._damageBuffAttached)
				{
					levelManager.player.stat.AttachValues(this._damageBuffStat);
					return;
				}
				levelManager.player.stat.DetachValues(this._damageBuffStat);
			});
			this._noCooldown.onClick.AddListener(delegate
			{
				this._noCooldownAttached = !this._noCooldownAttached;
				if (this._noCooldownAttached)
				{
					levelManager.player.stat.AttachValues(this._cooldownBuffStat);
					return;
				}
				levelManager.player.stat.DetachValues(this._cooldownBuffStat);
			});
			this._hp10k.onClick.AddListener(delegate
			{
				this._hp10kAttached = !this._hp10kAttached;
				if (this._hp10kAttached)
				{
					levelManager.player.stat.AttachValues(this._hp10kStat);
					levelManager.player.health.ResetToMaximumHealth();
					return;
				}
				levelManager.player.stat.DetachValues(this._hp10kStat);
			});
			this._shield10.onClick.AddListener(delegate
			{
				levelManager.player.health.shield.Add(this, 10f, null);
			});
			this._hardmodeToggle.isOn = GameData.HardmodeProgress.hardmode;
			this._hardmodeToggle.onValueChanged.AddListener(delegate(bool isOn)
			{
				GameData.HardmodeProgress.hardmode = isOn;
			});
			this._hardmodeLevelSlider.onValueChanged.AddListener(delegate(float value)
			{
				int hardmodeLevel = Mathf.Min(new int[]
				{
					(int)Mathf.Ceil(value),
					GameData.HardmodeProgress.clearedLevel + 1,
					GameData.HardmodeProgress.maxLevel
				});
				this._hardmodeLevel.text = hardmodeLevel.ToString();
				GameData.HardmodeProgress.hardmodeLevel = hardmodeLevel;
			});
			this._hardmodeClearedLevelSlider.onValueChanged.AddListener(delegate(float value)
			{
				int clearedLevel = Mathf.Min((int)Mathf.Ceil(value), GameData.HardmodeProgress.maxLevel);
				this._hardmodeClearedLevel.text = clearedLevel.ToString();
				GameData.HardmodeProgress.clearedLevel = clearedLevel;
			});
			this._rerollSkill.onClick.AddListener(delegate
			{
				levelManager.player.playerComponents.inventory.weapon.current.RerollSkills();
			});
			this._timeScaleSlider.onValueChanged.AddListener(delegate(float value)
			{
				Chronometer.global.DetachTimeScale(this._timeScaleSlider);
				Chronometer.global.AttachTimeScale(this._timeScaleSlider, value);
				this._timeScaleValue.text = string.Format("{0:0.00}", value);
			});
			this._timeScaleReset.onClick.AddListener(delegate
			{
				this._timeScaleSlider.value = 1f;
			});
			this._timeScaleValue.text = Chronometer.global.timeScale.ToString();
			this._infiniteRevive.onValueChanged.AddListener(delegate(bool isOn)
			{
				if (isOn)
				{
					levelManager.player.health.onDie += this.Revive;
					levelManager.player.health.onDie -= this.InitReviveCount;
					this._reviveCountText.text = string.Format("부활횟수 ({0})", this._reviveCount);
					return;
				}
				levelManager.player.health.onDie -= this.Revive;
				levelManager.player.health.onDie += this.InitReviveCount;
				this._reviveCountText.text = "무한부활";
			});
			this._verification.onValueChanged.AddListener(delegate(bool isOn)
			{
				this._resistanceValue.gameObject.SetActive(isOn);
				if (isOn)
				{
					levelManager.onMapChangedAndFadedIn += this.AttachHealthNumber;
					return;
				}
				levelManager.onMapChangedAndFadedIn -= this.AttachHealthNumber;
			});
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0003B834 File Offset: 0x00039A34
		private void AttachHealthNumber(Map old, Map @new)
		{
			foreach (Character character in Map.Instance.waveContainer.GetAllEnemies())
			{
				DetailModeHealth detailModeHealth = UnityEngine.Object.Instantiate<DetailModeHealth>(this._detailModeHealth, character.attach.transform);
				detailModeHealth.transform.position = new Vector2(character.collider.bounds.center.x, character.collider.bounds.max.y + 2f);
				detailModeHealth.Initialize(character);
			}
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0003B8F0 File Offset: 0x00039AF0
		private void Revive()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.health.Revive(player.health.maximumHealth * 0.6000000238418579);
			this._reviveCount++;
			this._reviveCountText.text = string.Format("부활횟수 ({0})", this._reviveCount);
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0003B95B File Offset: 0x00039B5B
		private void InitReviveCount()
		{
			this._reviveCount = 0;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0003B964 File Offset: 0x00039B64
		protected override void OnEnable()
		{
			base.OnEnable();
			PlayerInput.blocked.Attach(this);
			Chronometer.global.AttachTimeScale(this, 0f);
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			this._mapName.text = string.Format("Map : {0}/{1}/{2}\nSeed : {3}", new object[]
			{
				levelManager.currentChapter.type,
				levelManager.currentChapter.currentStage.name,
				Map.Instance.name.Replace(" (Clone)", ""),
				GameData.Save.instance.randomSeed
			});
			this._localNow.text = string.Format("Local now : {0}", DateTime.Now);
			this._utcNow.text = string.Format("Utc now : {0}", DateTime.UtcNow);
			this._hardmodeLevelSlider.value = (float)GameData.HardmodeProgress.hardmodeLevel;
			this._hardmodeLevel.text = GameData.HardmodeProgress.hardmodeLevel.ToString();
			this._hardmodeClearedLevelSlider.value = (float)GameData.HardmodeProgress.clearedLevel;
			this._hardmodeClearedLevel.text = GameData.HardmodeProgress.clearedLevel.ToString();
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0003BA9C File Offset: 0x00039C9C
		protected override void OnDisable()
		{
			base.OnDisable();
			PlayerInput.blocked.Detach(this);
			Chronometer.global.DetachTimeScale(this);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0003BABC File Offset: 0x00039CBC
		protected override void Update()
		{
			base.Update();
			if (!this._panels[Panel.Type.Main].activeSelf && KeyMapper.Map.Cancel.WasPressed)
			{
				this.OpenMain();
			}
		}

		// Token: 0x04001079 RID: 4217
		[SerializeField]
		private TMP_Text _mapName;

		// Token: 0x0400107A RID: 4218
		[SerializeField]
		private TMP_Text _version;

		// Token: 0x0400107B RID: 4219
		[Space]
		[SerializeField]
		private GameObject _main;

		// Token: 0x0400107C RID: 4220
		[SerializeField]
		private GameObject _mapList;

		// Token: 0x0400107D RID: 4221
		[SerializeField]
		private GameObject _gearList;

		// Token: 0x0400107E RID: 4222
		[SerializeField]
		private Log _log;

		// Token: 0x0400107F RID: 4223
		[SerializeField]
		private GameObject _dataControl;

		// Token: 0x04001080 RID: 4224
		[SerializeField]
		private GameObject _bonusStatPanel;

		// Token: 0x04001081 RID: 4225
		[SerializeField]
		private UnityEngine.UI.Button _rerollSkill;

		// Token: 0x04001082 RID: 4226
		private EnumArray<Panel.Type, GameObject> _panels;

		// Token: 0x04001083 RID: 4227
		[SerializeField]
		[Space]
		private UnityEngine.UI.Button _openMapList;

		// Token: 0x04001084 RID: 4228
		[SerializeField]
		private UnityEngine.UI.Button _chapter1;

		// Token: 0x04001085 RID: 4229
		[SerializeField]
		private UnityEngine.UI.Button _chapter2;

		// Token: 0x04001086 RID: 4230
		[SerializeField]
		private UnityEngine.UI.Button _chapter3;

		// Token: 0x04001087 RID: 4231
		[SerializeField]
		private UnityEngine.UI.Button _chapter4;

		// Token: 0x04001088 RID: 4232
		[SerializeField]
		private UnityEngine.UI.Button _chapter5;

		// Token: 0x04001089 RID: 4233
		[SerializeField]
		private UnityEngine.UI.Button _nextStage;

		// Token: 0x0400108A RID: 4234
		[SerializeField]
		private UnityEngine.UI.Button _nextMap;

		// Token: 0x0400108B RID: 4235
		[Space]
		[SerializeField]
		private UnityEngine.UI.Button _openGearList;

		// Token: 0x0400108C RID: 4236
		[SerializeField]
		private UnityEngine.UI.Button _hideUI;

		// Token: 0x0400108D RID: 4237
		[SerializeField]
		[Space]
		private UnityEngine.UI.Button _getGold;

		// Token: 0x0400108E RID: 4238
		[SerializeField]
		private UnityEngine.UI.Button _getDarkquartz;

		// Token: 0x0400108F RID: 4239
		[SerializeField]
		private UnityEngine.UI.Button _getBone;

		// Token: 0x04001090 RID: 4240
		[SerializeField]
		private UnityEngine.UI.Button _getHeartQuartz;

		// Token: 0x04001091 RID: 4241
		[SerializeField]
		private UnityEngine.UI.Button _awake;

		// Token: 0x04001092 RID: 4242
		[Space]
		[SerializeField]
		private UnityEngine.UI.Button _right3;

		// Token: 0x04001093 RID: 4243
		[SerializeField]
		private UnityEngine.UI.Button _damageBuff;

		// Token: 0x04001094 RID: 4244
		[SerializeField]
		private UnityEngine.UI.Button _noCooldown;

		// Token: 0x04001095 RID: 4245
		[SerializeField]
		private UnityEngine.UI.Button _hp10k;

		// Token: 0x04001096 RID: 4246
		[SerializeField]
		private UnityEngine.UI.Button _shield10;

		// Token: 0x04001097 RID: 4247
		[SerializeField]
		private UnityEngine.UI.Button _testMap;

		// Token: 0x04001098 RID: 4248
		[SerializeField]
		private Map _testMapPrefab;

		// Token: 0x04001099 RID: 4249
		[Header("하드모드")]
		[SerializeField]
		[Space]
		private Toggle _hardmodeToggle;

		// Token: 0x0400109A RID: 4250
		[SerializeField]
		private Slider _hardmodeLevelSlider;

		// Token: 0x0400109B RID: 4251
		[SerializeField]
		private TMP_Text _hardmodeLevel;

		// Token: 0x0400109C RID: 4252
		[SerializeField]
		private Slider _hardmodeClearedLevelSlider;

		// Token: 0x0400109D RID: 4253
		[SerializeField]
		private TMP_Text _hardmodeClearedLevel;

		// Token: 0x0400109E RID: 4254
		[Header("타임")]
		[SerializeField]
		private Slider _timeScaleSlider;

		// Token: 0x0400109F RID: 4255
		[SerializeField]
		private TMP_Text _timeScaleValue;

		// Token: 0x040010A0 RID: 4256
		[SerializeField]
		private UnityEngine.UI.Button _timeScaleReset;

		// Token: 0x040010A1 RID: 4257
		[Space]
		[SerializeField]
		private TMP_Text _localNow;

		// Token: 0x040010A2 RID: 4258
		[SerializeField]
		private TMP_Text _utcNow;

		// Token: 0x040010A3 RID: 4259
		[SerializeField]
		[Header("밸런싱 테스트 도구")]
		private Toggle _infiniteRevive;

		// Token: 0x040010A4 RID: 4260
		[SerializeField]
		private TMP_Text _reviveCountText;

		// Token: 0x040010A5 RID: 4261
		[SerializeField]
		private Toggle _verification;

		// Token: 0x040010A6 RID: 4262
		[SerializeField]
		private DetailModeHealth _detailModeHealth;

		// Token: 0x040010A7 RID: 4263
		[SerializeField]
		private GameObject _resistanceValue;

		// Token: 0x040010A8 RID: 4264
		private int _reviveCount;

		// Token: 0x040010A9 RID: 4265
		private bool _damageBuffAttached;

		// Token: 0x040010AA RID: 4266
		private bool _noCooldownAttached;

		// Token: 0x040010AB RID: 4267
		private bool _hp10kAttached;

		// Token: 0x040010AC RID: 4268
		private Stat.Values _damageBuffStat = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.AttackDamage, 100.0)
		});

		// Token: 0x040010AD RID: 4269
		private Stat.Values _cooldownBuffStat = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.CooldownSpeed, 100.0)
		});

		// Token: 0x040010AE RID: 4270
		private Stat.Values _hp10kStat = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 9900.0)
		});

		// Token: 0x0200040D RID: 1037
		public class GearOptionData : TMP_Dropdown.OptionData
		{
			// Token: 0x060013A6 RID: 5030 RVA: 0x0003BB87 File Offset: 0x00039D87
			public GearOptionData(GearReference gearInfo, Sprite image) : base(gearInfo.name, image)
			{
				this.gearInfo = gearInfo;
			}

			// Token: 0x040010AF RID: 4271
			public readonly GearReference gearInfo;
		}

		// Token: 0x0200040E RID: 1038
		public enum Type
		{
			// Token: 0x040010B1 RID: 4273
			Main,
			// Token: 0x040010B2 RID: 4274
			MapList,
			// Token: 0x040010B3 RID: 4275
			GearList,
			// Token: 0x040010B4 RID: 4276
			Log,
			// Token: 0x040010B5 RID: 4277
			DataControl,
			// Token: 0x040010B6 RID: 4278
			BonusStat
		}
	}
}
