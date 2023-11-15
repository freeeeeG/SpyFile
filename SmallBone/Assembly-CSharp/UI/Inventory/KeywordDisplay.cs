using System;
using System.Linq;
using Characters.Gear.Synergy.Inscriptions;
using Hardmode;
using Services;
using Singletons;
using UnityEngine;
using UserInput;

namespace UI.Inventory
{
	// Token: 0x02000434 RID: 1076
	public class KeywordDisplay : MonoBehaviour
	{
		// Token: 0x06001486 RID: 5254 RVA: 0x0003F24C File Offset: 0x0003D44C
		public void UpdateElements()
		{
			this._detailFrame.SetActive(false);
			this._statFrame.SetActive(false);
			Inscription[] array = (from keyword in Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions
			where keyword.count > 0
			select keyword).OrderByDescending(delegate(Inscription keyword)
			{
				if (keyword.isMaxStep)
				{
					return 2;
				}
				if (keyword.step >= 1)
				{
					return 1;
				}
				return 0;
			}).ThenByDescending((Inscription keyword) => keyword.count).ToArray<Inscription>();
			this._count = Math.Min(array.Length, this._keywordElements.Length);
			KeywordElement[] keywordElements = this._keywordElements;
			for (int i = 0; i < keywordElements.Length; i++)
			{
				keywordElements[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < this._count; j++)
			{
				if (j < this._keywordElements.Length / 2)
				{
					this._keywordElements[j].gameObject.SetActive(true);
				}
				this._keywordElements[j].Set(array[j].key);
			}
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0003F388 File Offset: 0x0003D588
		private void OnDisable()
		{
			this._gear.SetActive(true);
			this._option.SetActive(true);
			for (int i = this._keywordElements.Length / 2; i < this._count; i++)
			{
				this._keywordElements[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0003F3DC File Offset: 0x0003D5DC
		private void Update()
		{
			if (!this._detailFrame.activeSelf && KeyMapper.Map.UiInteraction2.IsPressed)
			{
				this._detailFrame.SetActive(true);
				this._statFrame.SetActive(true);
				this._gear.SetActive(false);
				this._upgrades.SetActive(false);
				this._option.SetActive(false);
				for (int i = this._keywordElements.Length / 2; i < this._count; i++)
				{
					this._keywordElements[i].gameObject.SetActive(true);
				}
				return;
			}
			if (this._detailFrame.activeSelf && !KeyMapper.Map.UiInteraction2.IsPressed)
			{
				this._gear.SetActive(true);
				this._option.SetActive(true);
				if (Singleton<HardmodeManager>.Instance.hardmode)
				{
					this._upgrades.SetActive(true);
				}
				this._detailFrame.SetActive(false);
				this._statFrame.SetActive(false);
				for (int j = this._keywordElements.Length / 2; j < this._count; j++)
				{
					this._keywordElements[j].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04001184 RID: 4484
		[SerializeField]
		private KeywordElement[] _keywordElements;

		// Token: 0x04001185 RID: 4485
		[SerializeField]
		private GameObject _detailFrame;

		// Token: 0x04001186 RID: 4486
		[SerializeField]
		private GameObject _statFrame;

		// Token: 0x04001187 RID: 4487
		[SerializeField]
		private GameObject _gear;

		// Token: 0x04001188 RID: 4488
		[SerializeField]
		private GameObject _option;

		// Token: 0x04001189 RID: 4489
		[SerializeField]
		private GameObject _upgrades;

		// Token: 0x0400118A RID: 4490
		[SerializeField]
		private GameObject _viewDetailKeyGuide;

		// Token: 0x0400118B RID: 4491
		private bool _needDetail;

		// Token: 0x0400118C RID: 4492
		private int _count;
	}
}
