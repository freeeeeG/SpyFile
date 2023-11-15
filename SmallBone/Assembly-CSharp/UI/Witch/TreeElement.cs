using System;
using Characters;
using FX;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Witch
{
	// Token: 0x020003E9 RID: 1001
	public class TreeElement : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060012B2 RID: 4786 RVA: 0x00037E84 File Offset: 0x00036084
		// (set) Token: 0x060012B3 RID: 4787 RVA: 0x00037E91 File Offset: 0x00036091
		public bool interactable
		{
			get
			{
				return this._button.interactable;
			}
			set
			{
				this._button.interactable = value;
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00037EA0 File Offset: 0x000360A0
		private void Awake()
		{
			UnityAction call = delegate()
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._getAbility, base.transform.position);
				this._bonus.LevelUp();
				this._level.text = this._bonus.level.ToString();
				this._panel.UpdateCurrentOption();
			};
			this._button.onClick.AddListener(call);
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00037ECB File Offset: 0x000360CB
		public void Initialize(Panel panel)
		{
			this._panel = panel;
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00037ED4 File Offset: 0x000360D4
		public void Set(WitchBonus.Bonus bonus)
		{
			this._bonus = bonus;
			this._name.text = this._bonus.displayName;
			this._level.text = this._bonus.level.ToString();
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00037F1C File Offset: 0x0003611C
		public void OnSelect(BaseEventData eventData)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._select, base.transform.position);
			this._panel.Set(this._bonus);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00037F4C File Offset: 0x0003614C
		private void Update()
		{
			this._deactivateMask.enabled = !this._bonus.ready;
			if (this._ready != null && !this._ready.activeSelf && this._bonus.ready)
			{
				this._ready.SetActive(true);
			}
			if (this._mastered != null && !this._mastered.activeSelf && this._bonus.level == this._bonus.maxLevel)
			{
				this._mastered.SetActive(true);
			}
		}

		// Token: 0x04000FAE RID: 4014
		private Panel _panel;

		// Token: 0x04000FAF RID: 4015
		[SerializeField]
		private Button _button;

		// Token: 0x04000FB0 RID: 4016
		[SerializeField]
		private TMP_Text _level;

		// Token: 0x04000FB1 RID: 4017
		[SerializeField]
		private GameObject _ready;

		// Token: 0x04000FB2 RID: 4018
		[SerializeField]
		private GameObject _mastered;

		// Token: 0x04000FB3 RID: 4019
		[SerializeField]
		private Image _icon;

		// Token: 0x04000FB4 RID: 4020
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x04000FB5 RID: 4021
		[SerializeField]
		private Image _deactivateMask;

		// Token: 0x04000FB6 RID: 4022
		private WitchBonus.Bonus _bonus;

		// Token: 0x04000FB7 RID: 4023
		[SerializeField]
		private SoundInfo _getAbility;

		// Token: 0x04000FB8 RID: 4024
		[SerializeField]
		private SoundInfo _select;
	}
}
