using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Gear;
using Characters.Operations;
using Characters.Operations.Fx;
using CutScenes;
using Data;
using GameResources;
using Runnables;
using Scenes;
using Services;
using Singletons;
using TMPro;
using UI;
using UnityEditor;
using UnityEngine;
using UserInput;

namespace Hardmode.Darktech
{
	// Token: 0x02000172 RID: 370
	public sealed class ManufacturingMachineInteractive : InteractiveObject
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x00016685 File Offset: 0x00014885
		private new string name
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/{1}", this._darktechType, "name"));
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x000166A6 File Offset: 0x000148A6
		private string body
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/body", this._darktechType));
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x000166C2 File Offset: 0x000148C2
		private string keyGuide
		{
			get
			{
				return Localization.GetLocalizedString("label/get");
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x000166CE File Offset: 0x000148CE
		private bool isEnhanced
		{
			get
			{
				return this._type == Gear.Type.Weapon && GameData.HardmodeProgress.clearedLevel >= 7;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x000166E5 File Offset: 0x000148E5
		private bool needEnhancedCutscene
		{
			get
			{
				return this._type == Gear.Type.Weapon && this.isEnhanced && !GameData.Progress.cutscene.GetData(CutScenes.Key.강화두개골제조기_Intro);
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001670C File Offset: 0x0001490C
		private void Start()
		{
			this._remainSelectCount = this._selectCount;
			this._price = ((this._type == Gear.Type.Weapon) ? Singleton<DarktechManager>.Instance.setting.두개골제조기가격 : Singleton<DarktechManager>.Instance.setting.보급품제조기가격);
			this._cacheUIObject = this._uiObject;
			this._cacheUIObjects = this._uiObjects;
			this._onSelect.Initialize();
			if (!Singleton<DarktechManager>.Instance.IsActivated(this._darktechType))
			{
				this._uiObject = this._searchGuide;
				this._uiObjects = new GameObject[0];
				this._animator.Play(this._deactiveHash, 0, 0f);
			}
			if (this.needEnhancedCutscene)
			{
				this._uiObject = this._searchGuide;
				this._uiObjects = new GameObject[0];
				this._animator.Play(this._deactiveHash, 0, 0f);
			}
			this._introAnimation = (this.isEnhanced ? this._intro7Hash : this._introHash);
			this._endAnimation = (this.isEnhanced ? this._end7Hash : this._endHash);
			this._mode2Animation = (this.isEnhanced ? this._mode7_2Hash : this._mode2Hash);
			base.StartCoroutine(this.CLoad());
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001684F File Offset: 0x00014A4F
		private IEnumerator CLoad()
		{
			while (!Singleton<Service>.Instance.gearManager.initialized)
			{
				yield return null;
			}
			this._gearList = new List<GearReference>(Singleton<Service>.Instance.gearManager.GetGearListByRarity(this._type, Rarity.Common));
			if (this.isEnhanced && this._type == Gear.Type.Weapon)
			{
				this._gearList.AddRange(Singleton<Service>.Instance.gearManager.GetGearListByRarity(this._type, Rarity.Rare));
			}
			if (this.needEnhancedCutscene)
			{
				yield break;
			}
			if (Singleton<DarktechManager>.Instance.IsActivated(this._darktechType))
			{
				this.ActivateMachine();
			}
			yield break;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00016860 File Offset: 0x00014A60
		public void ActivateMachine()
		{
			Singleton<DarktechManager>.Instance.ActivateDarktech(this._darktechType);
			this._uiObject.gameObject.SetActive(false);
			this._uiObject = this._cacheUIObject;
			this._uiObjects = this._cacheUIObjects;
			this._animator.Play(this._introAnimation, 0, 0f);
			this.Load();
			this._loopSound.Run(Singleton<Service>.Instance.levelManager.player);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x000168E0 File Offset: 0x00014AE0
		public void ActiavateEnhanvedMachine()
		{
			this._uiObject.gameObject.SetActive(false);
			this._uiObject = this._cacheUIObject;
			this._uiObjects = this._cacheUIObjects;
			SpriteRenderer[] displays = this._displays;
			for (int i = 0; i < displays.Length; i++)
			{
				displays[i].gameObject.SetActive(true);
			}
			this.Load();
			this._loopSound.Run(Singleton<Service>.Instance.levelManager.player);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001695C File Offset: 0x00014B5C
		public void TempDisplayOff()
		{
			SpriteRenderer[] displays = this._displays;
			for (int i = 0; i < displays.Length; i++)
			{
				displays[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001698C File Offset: 0x00014B8C
		public void TempDisplayOn()
		{
			SpriteRenderer[] displays = this._displays;
			for (int i = 0; i < displays.Length; i++)
			{
				displays[i].gameObject.SetActive(true);
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x000169BC File Offset: 0x00014BBC
		public override void InteractWith(Character character)
		{
			if (!Singleton<DarktechManager>.Instance.IsActivated(this._darktechType))
			{
				return;
			}
			if (this.needEnhancedCutscene)
			{
				this.StartEnhancedIntro(character);
				return;
			}
			if (this._remainSelectCount < this._selectCount && !GameData.Currency.darkQuartz.Has(this._price))
			{
				return;
			}
			if (this._remainSelectCount > 0)
			{
				this.Select();
				return;
			}
			base.StartCoroutine(this.CTalk());
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00016A2A File Offset: 0x00014C2A
		private void StartEnhancedIntro(Character character)
		{
			this._enhancedIntro.Run();
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00016A37 File Offset: 0x00014C37
		private IEnumerator CTalk()
		{
			SystemDialogue ui = Scene<GameBase>.instance.uiManager.systemDialogue;
			yield return LetterBox.instance.CAppear(0.4f);
			yield return ui.CShow(this.name, this.body, true);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00016A48 File Offset: 0x00014C48
		public void Select()
		{
			if (this._running)
			{
				return;
			}
			if (this._remainSelectCount < this._selectCount)
			{
				GameData.Currency.darkQuartz.Consume(this._price);
			}
			this._selectSound.Run(Singleton<Service>.Instance.levelManager.player);
			base.StartCoroutine(this._onSelect.CRun(Singleton<Service>.Instance.levelManager.player));
			this._remainSelectCount--;
			base.StartCoroutine(this.CDropGear());
			this._gearList.RemoveAt(this._currentIndex);
			this.Down();
			if (this._remainSelectCount <= 0)
			{
				this.ClosePopup();
				this._uiObject = this._searchGuide;
				this._uiObjects = new GameObject[0];
				this._uiObject.gameObject.SetActive(true);
				this.OpenPopupBy(this._character);
				this._animator.Play(this._endAnimation, 0, 0f);
				this.HideDisplay();
				return;
			}
			if (this._remainSelectCount == 1)
			{
				this._animator.Play(this._mode2Animation, 0, 0f);
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00016B6C File Offset: 0x00014D6C
		private void HideDisplay()
		{
			foreach (SpriteRenderer spriteRenderer in this._displays)
			{
				spriteRenderer.sprite = null;
				spriteRenderer.gameObject.SetActive(false);
			}
			this._display.sprite = null;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00016BAF File Offset: 0x00014DAF
		public override void OnDeactivate()
		{
			this.HideDisplay();
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00016BB7 File Offset: 0x00014DB7
		private IEnumerator CDropGear()
		{
			GearRequest request = this._gearList[this._currentIndex].LoadAsync();
			while (!request.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropGear(request, this._spawnPoint.position);
			yield break;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00016BC8 File Offset: 0x00014DC8
		private void Update()
		{
			if (!base.popupVisible)
			{
				return;
			}
			if (!base.activated)
			{
				return;
			}
			if (!Singleton<DarktechManager>.Instance.IsActivated(this._darktechType))
			{
				return;
			}
			if (this.needEnhancedCutscene)
			{
				return;
			}
			if (this._remainSelectCount <= 0)
			{
				return;
			}
			string arg = GameData.Currency.darkQuartz.colorCode;
			if (!GameData.Currency.darkQuartz.Has(this._price))
			{
				arg = ColorUtility.ToHtmlStringRGB(Color.red);
			}
			if (this._remainSelectCount < this._selectCount)
			{
				this._priceText.text = string.Format("{0} (<color=#{1}>{2}</color>)", this.keyGuide, arg, this._price);
			}
			else
			{
				this._priceText.text = this.keyGuide;
			}
			if (KeyMapper.Map.Up.WasPressed)
			{
				this.Up();
				return;
			}
			if (KeyMapper.Map.Down.WasPressed)
			{
				this.Down();
			}
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00016CAC File Offset: 0x00014EAC
		private void Load()
		{
			int index = (this._currentIndex + 1) % this._gearList.Count;
			int currentIndex = this._currentIndex;
			int index2 = (this._currentIndex == 0) ? (this._gearList.Count - 1) : (this._currentIndex - 1);
			this._displays[0].sprite = this._gearList[index2].icon;
			this._displays[1].sprite = this._gearList[currentIndex].icon;
			this._displays[2].sprite = this._gearList[index].icon;
			this._displays[0].enabled = false;
			this._displays[2].enabled = false;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00016D6C File Offset: 0x00014F6C
		private void Up()
		{
			if (this._running)
			{
				return;
			}
			this._moveSound.Run(Singleton<Service>.Instance.levelManager.player);
			this._currentIndex = (this._currentIndex + 1) % this._gearList.Count;
			base.StartCoroutine(this.CUp());
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00016DC4 File Offset: 0x00014FC4
		private void Down()
		{
			if (this._running)
			{
				return;
			}
			this._moveSound.Run(Singleton<Service>.Instance.levelManager.player);
			this._currentIndex--;
			if (this._currentIndex < 0)
			{
				this._currentIndex += this._gearList.Count;
			}
			base.StartCoroutine(this.CDown());
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00016E30 File Offset: 0x00015030
		public void InitialGearSetting(int _startGearSetting)
		{
			this._currentIndex = _startGearSetting;
			this.Load();
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00016E3F File Offset: 0x0001503F
		private IEnumerator CShake(Vector3 origin, Transform target)
		{
			float elapsed = 0f;
			while (elapsed < 0.08f)
			{
				target.position = new Vector2(origin.x, origin.y + UnityEngine.Random.Range(-0.05f, 0.05f));
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			target.position = origin;
			yield break;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00016E55 File Offset: 0x00015055
		private IEnumerator CUp()
		{
			this._running = true;
			float elapsed = 0f;
			Vector3 upPoint = this._displays[2].transform.position;
			Vector3 fromMid = this._displays[1].transform.position;
			Vector3 toDown = this._displays[0].transform.position;
			Vector3 fromUp = this._displays[2].transform.position;
			Vector3 toMid = this._displays[1].transform.position;
			this._displays[0].enabled = true;
			this._displays[2].enabled = true;
			while (elapsed < this._animationDuration)
			{
				this._displays[1].transform.position = Vector2.Lerp(fromMid, toDown, this._animationCurve.Evaluate(elapsed / this._animationDuration));
				this._displays[2].transform.position = Vector2.Lerp(fromUp, toMid, this._animationCurve.Evaluate(elapsed / this._animationDuration));
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			this._displays[0].transform.position = upPoint;
			this._displays[1].transform.position = toDown;
			this._displays[2].transform.position = toMid;
			yield return this.CShake(toMid, this._displays[2].transform);
			SpriteRenderer spriteRenderer = this._displays[0];
			SpriteRenderer spriteRenderer2 = this._displays[1];
			SpriteRenderer spriteRenderer3 = this._displays[2];
			this._displays[0] = spriteRenderer2;
			this._displays[1] = spriteRenderer3;
			this._displays[2] = spriteRenderer;
			this.Load();
			this._running = false;
			yield break;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00016E64 File Offset: 0x00015064
		private IEnumerator CDown()
		{
			this._running = true;
			float elapsed = 0f;
			Vector3 downPoint = this._displays[0].transform.position;
			Vector3 fromDown = this._displays[0].transform.position;
			Vector3 toMid = this._displays[1].transform.position;
			Vector3 fromMid = this._displays[1].transform.position;
			Vector3 toUp = this._displays[2].transform.position;
			this._displays[0].enabled = true;
			this._displays[2].enabled = true;
			while (elapsed < this._animationDuration)
			{
				this._displays[0].transform.position = Vector2.Lerp(fromDown, toMid, this._animationCurve.Evaluate(elapsed / this._animationDuration));
				this._displays[1].transform.position = Vector2.Lerp(fromMid, toUp, this._animationCurve.Evaluate(elapsed / this._animationDuration));
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			this._displays[0].transform.position = toMid;
			this._displays[1].transform.position = toUp;
			this._displays[2].transform.position = downPoint;
			yield return this.CShake(toMid, this._displays[0].transform);
			SpriteRenderer spriteRenderer = this._displays[0];
			SpriteRenderer spriteRenderer2 = this._displays[1];
			SpriteRenderer spriteRenderer3 = this._displays[2];
			this._displays[0] = spriteRenderer3;
			this._displays[1] = spriteRenderer;
			this._displays[2] = spriteRenderer2;
			this.Load();
			this._running = false;
			yield break;
		}

		// Token: 0x040005A5 RID: 1445
		private readonly int _deactiveHash = Animator.StringToHash("SMM_Deactivate");

		// Token: 0x040005A6 RID: 1446
		private readonly int _introHash = Animator.StringToHash("SMM_Intro");

		// Token: 0x040005A7 RID: 1447
		private readonly int _endHash = Animator.StringToHash("SMM_End");

		// Token: 0x040005A8 RID: 1448
		private readonly int _mode2Hash = Animator.StringToHash("SMM_Mode_2");

		// Token: 0x040005A9 RID: 1449
		private readonly int _intro7Hash = Animator.StringToHash("SMM2_Intro");

		// Token: 0x040005AA RID: 1450
		private readonly int _end7Hash = Animator.StringToHash("SMM2_End");

		// Token: 0x040005AB RID: 1451
		private readonly int _mode7_2Hash = Animator.StringToHash("SMM2_Mode_2");

		// Token: 0x040005AC RID: 1452
		[SerializeField]
		private DarktechData.Type _darktechType;

		// Token: 0x040005AD RID: 1453
		[SerializeField]
		private Gear.Type _type;

		// Token: 0x040005AE RID: 1454
		[SerializeField]
		private int _selectCount;

		// Token: 0x040005AF RID: 1455
		[SerializeField]
		private Animator _animator;

		// Token: 0x040005B0 RID: 1456
		[SerializeField]
		private Animator _displayAnimator;

		// Token: 0x040005B1 RID: 1457
		[SerializeField]
		private SpriteRenderer _display;

		// Token: 0x040005B2 RID: 1458
		[SerializeField]
		private Transform _spawnPoint;

		// Token: 0x040005B3 RID: 1459
		[SerializeField]
		private TMP_Text _priceText;

		// Token: 0x040005B4 RID: 1460
		[SerializeField]
		private GameObject _searchGuide;

		// Token: 0x040005B5 RID: 1461
		private List<GearReference> _gearList;

		// Token: 0x040005B6 RID: 1462
		private int _currentIndex;

		// Token: 0x040005B7 RID: 1463
		private int _remainSelectCount;

		// Token: 0x040005B8 RID: 1464
		private int _price;

		// Token: 0x040005B9 RID: 1465
		private GameObject _cacheUIObject;

		// Token: 0x040005BA RID: 1466
		private GameObject[] _cacheUIObjects;

		// Token: 0x040005BB RID: 1467
		[SerializeField]
		[Subcomponent(typeof(PlaySound))]
		private PlaySound _loopSound;

		// Token: 0x040005BC RID: 1468
		[Subcomponent(typeof(PlaySound))]
		[SerializeField]
		private PlaySound _moveSound;

		// Token: 0x040005BD RID: 1469
		[Subcomponent(typeof(PlaySound))]
		[SerializeField]
		private PlaySound _selectSound;

		// Token: 0x040005BE RID: 1470
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onSelect;

		// Token: 0x040005BF RID: 1471
		[SerializeField]
		private Runnable _enhancedIntro;

		// Token: 0x040005C0 RID: 1472
		[SerializeField]
		private SpriteRenderer[] _displays;

		// Token: 0x040005C1 RID: 1473
		[Range(0f, 1f)]
		[SerializeField]
		private float _animationDuration = 0.3f;

		// Token: 0x040005C2 RID: 1474
		[SerializeField]
		private AnimationCurve _animationCurve;

		// Token: 0x040005C3 RID: 1475
		private const float shakeDuration = 0.08f;

		// Token: 0x040005C4 RID: 1476
		private const float shakeIntensity = 0.05f;

		// Token: 0x040005C5 RID: 1477
		private int _introAnimation;

		// Token: 0x040005C6 RID: 1478
		private int _endAnimation;

		// Token: 0x040005C7 RID: 1479
		private int _mode2Animation;

		// Token: 0x040005C8 RID: 1480
		private bool _running;
	}
}
