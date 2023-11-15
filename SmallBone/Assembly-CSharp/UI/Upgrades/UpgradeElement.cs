using System;
using System.Collections;
using Characters.Gear;
using Characters.Gear.Upgrades;
using Data;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Upgrades
{
	// Token: 0x020003F0 RID: 1008
	public sealed class UpgradeElement : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x170003DA RID: 986
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x00038D2C File Offset: 0x00036F2C
		public Selectable selectable
		{
			get
			{
				return this._button;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x00038D34 File Offset: 0x00036F34
		public UpgradeResource.Reference reference
		{
			get
			{
				return this._reference;
			}
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00038D3C File Offset: 0x00036F3C
		private void Start()
		{
			Singleton<UpgradeShop>.Instance.onChanged += this.HandleOnChanged;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00038D54 File Offset: 0x00036F54
		private void OnDestroy()
		{
			Singleton<UpgradeShop>.Instance.onChanged -= this.HandleOnChanged;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00038D6C File Offset: 0x00036F6C
		private void HandleOnChanged()
		{
			this.UpdateSelectable(this._reference);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00038D7C File Offset: 0x00036F7C
		public void Initialize(UpgradeResource.Reference reference, Panel panel)
		{
			this._panel = panel;
			this._reference = reference;
			this._icon.sprite = reference.icon;
			this._deactiveMask.sprite = reference.icon;
			UnityAction call = new UnityAction(this.TryUpgrade);
			this._button.onClick.RemoveAllListeners();
			this._button.onClick.AddListener(call);
			this._button.navigation = new Navigation
			{
				mode = Navigation.Mode.Automatic
			};
			this._findEffect.SetActive(false);
			this.UpdateSelectable(reference);
			string typeName = Gear.Type.Upgrade.ToString();
			if (!GameData.Gear.IsFounded(typeName, this._reference.name))
			{
				this._findFrame.sprite = ((reference.type == UpgradeObject.Type.Cursed) ? this._curseFindSprite : this._normalFindSprite);
				this._findFrame.gameObject.SetActive(true);
				base.StartCoroutine(this.CActiveFindEffect());
			}
			this.HandleOnChanged();
			GameData.Gear.SetFounded(typeName, this._reference.name, true);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00038E8F File Offset: 0x0003708F
		private IEnumerator CActiveFindEffect()
		{
			this._findEffect.SetActive(true);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._panel.upgradableContainer.findSoundInfo, this._findFrame.gameObject.transform.position);
			yield return Chronometer.global.WaitForSeconds(1f);
			this._findEffect.SetActive(false);
			yield break;
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00038E9E File Offset: 0x0003709E
		private void OnEnable()
		{
			this._failEffect.SetActive(false);
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x00038EAC File Offset: 0x000370AC
		public void OnSelect(BaseEventData eventData)
		{
			if (!this.selectable.interactable)
			{
				return;
			}
			this._findFrame.gameObject.SetActive(false);
			this._panel.UpdateCurrentOption(this);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._panel.upgradableContainer.moveSoundInfo, this._findFrame.gameObject.transform.position);
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00038F14 File Offset: 0x00037114
		public void UpdateObjectTo(UpgradeResource.Reference reference)
		{
			this._reference = reference;
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00038F20 File Offset: 0x00037120
		private void UpdateSelectable(UpgradeResource.Reference reference)
		{
			if (Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(reference))
			{
				this.selectable.interactable = false;
				this.selectable.targetGraphic.raycastTarget = false;
				this._selectionSpriteSwapper.OnDeselect(null);
				this._selectionSpriteSwapper.enabled = false;
				this._deactiveMask.gameObject.SetActive(true);
				return;
			}
			this.selectable.interactable = true;
			this.selectable.targetGraphic.raycastTarget = true;
			this._selectionSpriteSwapper.enabled = true;
			this._deactiveMask.gameObject.SetActive(false);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00038FD4 File Offset: 0x000371D4
		private void TryUpgrade()
		{
			if (!Singleton<UpgradeShop>.Instance.TryUpgrade(this._reference))
			{
				base.StartCoroutine(this.CEmitFailEffect());
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._panel.upgradableContainer.failSoundInfo, this._findFrame.gameObject.transform.position);
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._panel.upgradableContainer.buySoundInfo, this._findFrame.gameObject.transform.position);
			this._panel.AppendToUpgradedList();
			this._panel.UpdateCurrentOption(this);
			this.UpdateSelectable(this._reference);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00039084 File Offset: 0x00037284
		private IEnumerator CEmitFailEffect()
		{
			this._failEffect.SetActive(true);
			yield return Chronometer.global.WaitForSeconds(0.3f);
			this._failEffect.SetActive(false);
			yield break;
		}

		// Token: 0x04000FF4 RID: 4084
		[SerializeField]
		private Button _button;

		// Token: 0x04000FF5 RID: 4085
		[SerializeField]
		private Image _icon;

		// Token: 0x04000FF6 RID: 4086
		[SerializeField]
		private Image _deactiveMask;

		// Token: 0x04000FF7 RID: 4087
		[SerializeField]
		private Sprite _normalFindSprite;

		// Token: 0x04000FF8 RID: 4088
		[SerializeField]
		private Sprite _curseFindSprite;

		// Token: 0x04000FF9 RID: 4089
		[SerializeField]
		private Image _findFrame;

		// Token: 0x04000FFA RID: 4090
		[SerializeField]
		private GameObject _findEffect;

		// Token: 0x04000FFB RID: 4091
		[SerializeField]
		private GameObject _failEffect;

		// Token: 0x04000FFC RID: 4092
		[SerializeField]
		private SelectionSpriteSwapper _selectionSpriteSwapper;

		// Token: 0x04000FFD RID: 4093
		private UpgradeResource.Reference _reference;

		// Token: 0x04000FFE RID: 4094
		private Panel _panel;
	}
}
