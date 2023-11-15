using System;
using System.Collections.Generic;
using Characters.Gear.Upgrades;
using FX;
using Singletons;
using UnityEngine;

namespace UI.Upgrades
{
	// Token: 0x020003EF RID: 1007
	public sealed class UpgradableContainer : MonoBehaviour
	{
		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x00038AEF File Offset: 0x00036CEF
		public SoundInfo moveSoundInfo
		{
			get
			{
				return this._moveSoundInfo;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00038AF7 File Offset: 0x00036CF7
		public SoundInfo closeSoundInfo
		{
			get
			{
				return this._closeSoundInfo;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x00038AFF File Offset: 0x00036CFF
		public SoundInfo buySoundInfo
		{
			get
			{
				return this._buySoundInfo;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x00038B07 File Offset: 0x00036D07
		public SoundInfo upgradeSoundInfo
		{
			get
			{
				return this._upgradeSoundInfo;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x00038B0F File Offset: 0x00036D0F
		public SoundInfo clearSoundInfo
		{
			get
			{
				return this._clearSoundInfo;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x00038B17 File Offset: 0x00036D17
		public SoundInfo failSoundInfo
		{
			get
			{
				return this._failSoundInfo;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x00038B1F File Offset: 0x00036D1F
		public SoundInfo findSoundInfo
		{
			get
			{
				return this._findSoundInfo;
			}
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00038B27 File Offset: 0x00036D27
		public void Initialize(Panel panel)
		{
			this._panel = panel;
			this._upgradeElements = new List<UpgradeElement>();
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00038B3B File Offset: 0x00036D3B
		public void UpdateElements()
		{
			this.DestroyElements();
			this.CreateElements();
			if (this._upgradeElements.Count > 0)
			{
				this._panel.Focus(this.GetDefaultFocusTarget().selectable);
			}
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00038B70 File Offset: 0x00036D70
		public void CreateElements()
		{
			List<UpgradeResource.Reference> riskObjects = Singleton<UpgradeShop>.Instance.GetRiskObjects();
			for (int i = 0; i < 2; i++)
			{
				UpgradeResource.Reference reference = riskObjects[i];
				UpgradeElement upgradeElement = UnityEngine.Object.Instantiate<UpgradeElement>(this._elementPrefab, this._elementParents[reference.type]);
				upgradeElement.Initialize(reference, this._panel);
				this._upgradeElements.Add(upgradeElement);
			}
			foreach (UpgradeResource.Reference reference2 in Singleton<UpgradeShop>.Instance.GetUpgradables())
			{
				if (reference2.type != UpgradeObject.Type.Cursed)
				{
					UpgradeElement upgradeElement2 = UnityEngine.Object.Instantiate<UpgradeElement>(this._elementPrefab, this._elementParents[UpgradeObject.Type.Normal]);
					upgradeElement2.Initialize(reference2, this._panel);
					this._upgradeElements.Add(upgradeElement2);
				}
			}
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x00038C58 File Offset: 0x00036E58
		public void DestroyElements()
		{
			if (this._upgradeElements.Count == 0)
			{
				return;
			}
			for (int i = this._upgradeElements.Count - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this._upgradeElements[i].gameObject);
			}
			this._upgradeElements.Clear();
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00038CAC File Offset: 0x00036EAC
		public UpgradeElement GetDefaultFocusTarget()
		{
			foreach (UpgradeElement upgradeElement in this._upgradeElements)
			{
				if (!(upgradeElement == null) && upgradeElement.reference != null && upgradeElement.reference.type != UpgradeObject.Type.Cursed && upgradeElement.selectable.interactable)
				{
					return upgradeElement;
				}
			}
			return null;
		}

		// Token: 0x04000FE9 RID: 4073
		[SerializeField]
		private UpgradeElement _elementPrefab;

		// Token: 0x04000FEA RID: 4074
		[SerializeField]
		private EnumArray<UpgradeObject.Type, Transform> _elementParents;

		// Token: 0x04000FEB RID: 4075
		private Panel _panel;

		// Token: 0x04000FEC RID: 4076
		private List<UpgradeElement> _upgradeElements;

		// Token: 0x04000FED RID: 4077
		[SerializeField]
		private SoundInfo _moveSoundInfo;

		// Token: 0x04000FEE RID: 4078
		[SerializeField]
		private SoundInfo _closeSoundInfo;

		// Token: 0x04000FEF RID: 4079
		[SerializeField]
		private SoundInfo _buySoundInfo;

		// Token: 0x04000FF0 RID: 4080
		[SerializeField]
		private SoundInfo _upgradeSoundInfo;

		// Token: 0x04000FF1 RID: 4081
		[SerializeField]
		private SoundInfo _clearSoundInfo;

		// Token: 0x04000FF2 RID: 4082
		[SerializeField]
		private SoundInfo _failSoundInfo;

		// Token: 0x04000FF3 RID: 4083
		[SerializeField]
		private SoundInfo _findSoundInfo;
	}
}
