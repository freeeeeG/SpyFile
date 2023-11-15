using System;
using System.Collections.Generic;
using Characters;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Operations;
using Characters.Operations.Fx;
using Characters.Player;
using Data;
using GameResources;
using Level;
using Platforms;
using Services;
using Singletons;
using TMPro;
using UnityEditor;
using UnityEngine;
using UserInput;

namespace Hardmode.Darktech
{
	// Token: 0x0200016A RID: 362
	public sealed class InscriptionSynthesisEquipmentSlot : InteractiveObject
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x00015069 File Offset: 0x00013269
		public Inscription.Key? selected
		{
			get
			{
				return this._selected;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00015071 File Offset: 0x00013271
		public string changeInteraction
		{
			get
			{
				return Localization.GetLocalizedString("label/interaction/changeInscription");
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0001507D File Offset: 0x0001327D
		public string fixInscription
		{
			get
			{
				return Localization.GetLocalizedString("label/interaction/fixInscription");
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001508C File Offset: 0x0001328C
		protected override void Awake()
		{
			base.Awake();
			this._keys = new List<Inscription.Key>(Inscription.keys.Count);
			foreach (Inscription.Key key in Inscription.keys)
			{
				if (key != Inscription.Key.None)
				{
					this._keys.Add(key);
				}
			}
			this._keyComparison = delegate(Inscription.Key key1, Inscription.Key key2)
			{
				Synergy synergy = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy;
				if (synergy.inscriptions[key1].isMaxStep)
				{
					if (synergy.inscriptions[key2].isMaxStep)
					{
						return 0;
					}
					return -2;
				}
				else
				{
					if (synergy.inscriptions[key2].isMaxStep)
					{
						return 2;
					}
					if (synergy.inscriptions[key1].step > synergy.inscriptions[key2].step)
					{
						return -1;
					}
					if (synergy.inscriptions[key1].step < synergy.inscriptions[key2].step)
					{
						return 1;
					}
					if (synergy.inscriptions[key1].count > synergy.inscriptions[key2].count)
					{
						return -1;
					}
					if (synergy.inscriptions[key1].count < synergy.inscriptions[key2].count)
					{
						return 1;
					}
					return 0;
				}
			};
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._price = Singleton<DarktechManager>.Instance.setting.GetInscriptionBonusCostByStage(currentChapter.type, currentChapter.stageIndex);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00015154 File Offset: 0x00013354
		private void Start()
		{
			Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.onChanged += this.UpdateDisplayToSelected;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00015188 File Offset: 0x00013388
		private void OnDestroy()
		{
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.onChanged -= this.UpdateDisplayToSelected;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000151DC File Offset: 0x000133DC
		private void Update()
		{
			if (!base.activated)
			{
				return;
			}
			this.UpdateSelectingState();
			if (!this._selecting)
			{
				return;
			}
			if (KeyMapper.Map.Up.WasPressed)
			{
				this._animator.Play(InscriptionSynthesisEquipmentSlot._upHash);
				this._moveSound.Run(Singleton<Service>.Instance.levelManager.player);
				this.MoveFocus(1);
				return;
			}
			if (KeyMapper.Map.Down.WasPressed)
			{
				this._animator.Play(InscriptionSynthesisEquipmentSlot._downHash);
				this._moveSound.Run(Singleton<Service>.Instance.levelManager.player);
				this.MoveFocus(-1);
			}
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00015288 File Offset: 0x00013488
		private void UpdateSelectingState()
		{
			if (this._selecting && !this._interactionCache.IsInteracting(this))
			{
				if (this._selected != null)
				{
					this._animator.Play(InscriptionSynthesisEquipmentSlot._fixLoopHash);
				}
				this._selecting = false;
				this.UpdateCurrentIndex();
				this.UpdateDisplay();
			}
			if (!this._selecting && this._interactionCache.IsInteracting(this))
			{
				this._selecting = true;
				this.UpdateCurrentIndex();
				this.UpdateDisplayToSelected();
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00015304 File Offset: 0x00013504
		public void Initialize(InscriptionSynthesisEquipment machine, int slotIndex)
		{
			this._machine = machine;
			this._slotIndex = slotIndex;
			this._onSelect.Initialize();
			Character player = Singleton<Service>.Instance.levelManager.player;
			this._interactionCache = player.GetComponent<CharacterInteraction>();
			this._inventoryCache = player.playerComponents.inventory;
			if (GameData.HardmodeProgress.inscriptionSynthesisEquipment[this._slotIndex].isDefaultValue)
			{
				this.UpdateDisplayToSelected();
				return;
			}
			this.LoadSelectedData();
			this.UpdateDisplayToSelected();
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00015384 File Offset: 0x00013584
		public override void InteractWith(Character character)
		{
			if (!this.IsSelectable())
			{
				this._animator.Play(InscriptionSynthesisEquipmentSlot._idleHash);
				return;
			}
			this._selectCurrency.Consume(this._price);
			this.SelectCurrentKey();
			this._animator.Play(InscriptionSynthesisEquipmentSlot._fixHash);
			int num = 0;
			while (num < GameData.HardmodeProgress.InscriptionSynthesisEquipment.count && GameData.HardmodeProgress.inscriptionSynthesisEquipment[num].value != -1)
			{
				if (num == GameData.HardmodeProgress.InscriptionSynthesisEquipment.count - 1)
				{
					Achievement.Type.CareerChoice.Set();
				}
				num++;
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00015408 File Offset: 0x00013608
		public void Select(Inscription.Key key)
		{
			if (this._selected != null)
			{
				this._inventoryCache.synergy.inscriptions[this._selected.Value].bonusCount -= this._amount;
			}
			this._selected = new Inscription.Key?(key);
			this._inventoryCache.synergy.inscriptions[this._selected.Value].bonusCount += this._amount;
			this._inventoryCache.UpdateSynergy();
			this.SaveSelectedData();
			this.UpdateDisplayToSelected();
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x000154AC File Offset: 0x000136AC
		private void UpdateSelectGuide()
		{
			if (this._currentIndex == -1)
			{
				this._selectGuide.SetActive(false);
				return;
			}
			Inscription.Key key = this._keys[this._currentIndex];
			if (this._selected != null && key == this._selected.Value)
			{
				this._selectGuide.SetActive(false);
				return;
			}
			string text = ColorUtility.ToHtmlStringRGB(this._selectCurrency.Has(this._price) ? Color.yellow : Color.red);
			this._selectGuideText.text = string.Format("{0} {1}(<color=#{2}>{3}</color>)", new object[]
			{
				Inscription.GetName(key),
				this.fixInscription,
				text,
				this._price
			});
			this._selectGuide.SetActive(true);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001557C File Offset: 0x0001377C
		private void UpdateDisplay()
		{
			this.UpdateSelectGuide();
			if (this._currentIndex == -1)
			{
				this._display.sprite = null;
				return;
			}
			Inscription.Key key = this._keys[this._currentIndex];
			Inscription inscription = this._inventoryCache.synergy.inscriptions[key];
			int step = inscription.step;
			this._display.sprite = inscription.icon;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000155E8 File Offset: 0x000137E8
		public override void OpenPopupBy(Character character)
		{
			base.OpenPopupBy(character);
			this._selecting = true;
			string arg = ColorUtility.ToHtmlStringRGB(this._selectCurrency.Has(this._price) ? Color.yellow : Color.red);
			this._selectGuideText.text = string.Format("{0}(<color=#{1}>{2}</color>)", this.fixInscription, arg, this._price);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00015650 File Offset: 0x00013850
		private bool IsSelectable()
		{
			if (this._currentIndex == -1)
			{
				return false;
			}
			Inscription.Key key = this._keys[this._currentIndex];
			return this._inventoryCache.synergy.inscriptions[key].count >= 1 && this._selectCurrency.Has(this._price) && (this._selected == null || this._selected.Value != key);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000156D0 File Offset: 0x000138D0
		private void SelectCurrentKey()
		{
			base.StartCoroutine(this._onSelect.CRun(Singleton<Service>.Instance.levelManager.player));
			this._selectSound.Run(Singleton<Service>.Instance.levelManager.player);
			this.Select(this._keys[this._currentIndex]);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00015730 File Offset: 0x00013930
		private void MoveFocus(int moveAmount)
		{
			int num = 0;
			int currentIndex = this._currentIndex;
			for (;;)
			{
				int num2 = this._currentIndex + moveAmount;
				if (num2 < 0)
				{
					num2 += this._keys.Count;
				}
				this._currentIndex = num2 % this._keys.Count;
				Inscription.Key key = this._keys[this._currentIndex];
				num++;
				if (num >= this._keys.Count)
				{
					break;
				}
				if (this._inventoryCache.synergy.inscriptions[key].count >= 1 && this._machine.IsSelectable(this, key) && (this._selected == null || this._selected.Value != key))
				{
					goto IL_B2;
				}
			}
			this._currentIndex = currentIndex;
			IL_B2:
			if (num < this._keys.Count)
			{
				this.UpdateDisplay();
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00015804 File Offset: 0x00013A04
		private void UpdateDisplayToSelected()
		{
			this.UpdateSelectGuide();
			if (this._selected == null)
			{
				this._display.sprite = null;
				return;
			}
			Inscription.Key value = this._selected.Value;
			Inscription inscription = this._inventoryCache.synergy.inscriptions[value];
			this._display.sprite = inscription.icon;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00015868 File Offset: 0x00013A68
		private void UpdateCurrentIndex()
		{
			this._keys.Sort(this._keyComparison);
			this._currentIndex = -1;
			if (this._selected == null)
			{
				return;
			}
			for (int i = 0; i < this._keys.Count; i++)
			{
				if (this._keys[i] == this._selected.Value)
				{
					this._currentIndex = i;
				}
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x000158D1 File Offset: 0x00013AD1
		private void SaveSelectedData()
		{
			GameData.HardmodeProgress.inscriptionSynthesisEquipment[this._slotIndex].value = (int)this._selected.Value;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000158F4 File Offset: 0x00013AF4
		private void LoadSelectedData()
		{
			int num = 0;
			foreach (Inscription.Key value in Inscription.keys)
			{
				if (num == GameData.HardmodeProgress.inscriptionSynthesisEquipment[this._slotIndex].value)
				{
					this._selected = new Inscription.Key?(value);
					this._animator.Play(InscriptionSynthesisEquipmentSlot._fixLoopHash);
					break;
				}
				num++;
			}
		}

		// Token: 0x04000565 RID: 1381
		private static readonly int _idleHash = Animator.StringToHash("Activate");

		// Token: 0x04000566 RID: 1382
		private static readonly int _upHash = Animator.StringToHash("Up");

		// Token: 0x04000567 RID: 1383
		private static readonly int _downHash = Animator.StringToHash("Down");

		// Token: 0x04000568 RID: 1384
		private static readonly int _fixHash = Animator.StringToHash("Fix");

		// Token: 0x04000569 RID: 1385
		private static readonly int _fixLoopHash = Animator.StringToHash("Fix_Loop");

		// Token: 0x0400056A RID: 1386
		[SerializeField]
		private int _amount;

		// Token: 0x0400056B RID: 1387
		[SerializeField]
		private SpriteRenderer _display;

		// Token: 0x0400056C RID: 1388
		[SerializeField]
		private Animator _animator;

		// Token: 0x0400056D RID: 1389
		[SerializeField]
		private GameObject _selectGuide;

		// Token: 0x0400056E RID: 1390
		[SerializeField]
		private TMP_Text _selectGuideText;

		// Token: 0x0400056F RID: 1391
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onSelect;

		// Token: 0x04000570 RID: 1392
		[Subcomponent(typeof(PlaySound))]
		[SerializeField]
		private PlaySound _moveSound;

		// Token: 0x04000571 RID: 1393
		[SerializeField]
		[Subcomponent(typeof(PlaySound))]
		private PlaySound _selectSound;

		// Token: 0x04000572 RID: 1394
		private int _currentIndex = -1;

		// Token: 0x04000573 RID: 1395
		private Inscription.Key? _selected;

		// Token: 0x04000574 RID: 1396
		private bool _selecting;

		// Token: 0x04000575 RID: 1397
		private List<Inscription.Key> _keys;

		// Token: 0x04000576 RID: 1398
		private Comparison<Inscription.Key> _keyComparison;

		// Token: 0x04000577 RID: 1399
		private int _slotIndex;

		// Token: 0x04000578 RID: 1400
		private InscriptionSynthesisEquipment _machine;

		// Token: 0x04000579 RID: 1401
		private int _price;

		// Token: 0x0400057A RID: 1402
		private GameData.Currency _selectCurrency = GameData.Currency.gold;

		// Token: 0x0400057B RID: 1403
		private CharacterInteraction _interactionCache;

		// Token: 0x0400057C RID: 1404
		private Inventory _inventoryCache;

		// Token: 0x0400057D RID: 1405
		private const int nullIndex = -1;
	}
}
