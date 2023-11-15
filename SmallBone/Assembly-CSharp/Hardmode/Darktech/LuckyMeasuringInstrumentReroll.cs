using System;
using System.Collections;
using Characters;
using Data;
using FX;
using GameResources;
using Platforms;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Hardmode.Darktech
{
	// Token: 0x0200016F RID: 367
	public class LuckyMeasuringInstrumentReroll : InteractiveObject
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000768 RID: 1896 RVA: 0x00016070 File Offset: 0x00014270
		// (remove) Token: 0x06000769 RID: 1897 RVA: 0x000160A8 File Offset: 0x000142A8
		public event Action onInteracted;

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x000160DD File Offset: 0x000142DD
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x000160E8 File Offset: 0x000142E8
		public LuckyMeasuringInstrument @base
		{
			get
			{
				return this._base;
			}
			set
			{
				this._base = value;
				if (this.@base.remainLootCount == 2)
				{
					this._animator.Play(LuckyMeasuringInstrumentReroll._idleFirstHash);
					return;
				}
				if (this.@base.remainLootCount == 1)
				{
					this._animator.Play(LuckyMeasuringInstrumentReroll._idleSecondHash);
					return;
				}
				this._animator.Play(LuckyMeasuringInstrumentReroll._endSecondHash);
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x0001614A File Offset: 0x0001434A
		public int lootCount
		{
			get
			{
				return this.@base.lootCount;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00016157 File Offset: 0x00014357
		public int remainLootCount
		{
			get
			{
				return this.@base.remainLootCount;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x00016164 File Offset: 0x00014364
		private GameData.Currency rerollCurrency
		{
			get
			{
				return GameData.Currency.darkQuartz;
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001616B File Offset: 0x0001436B
		private void OnEnable()
		{
			this._cost = Singleton<DarktechManager>.Instance.setting.행운계측기설정.refreshPrice;
			this.UpdateInteractionGuide();
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00016190 File Offset: 0x00014390
		public void Initialize()
		{
			Data<int> refreshCount = GameData.HardmodeProgress.luckyMeasuringInstrument.refreshCount;
			int maxRefreshCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.maxRefreshCount;
			if (refreshCount.value >= maxRefreshCount)
			{
				Achievement.Type.Allin.Set();
				base.Deactivate();
			}
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000161D4 File Offset: 0x000143D4
		public override void InteractWith(Character character)
		{
			if (!this.rerollCurrency.Consume(this._cost))
			{
				this.FailReroll();
				return;
			}
			Data<int> refreshCount = GameData.HardmodeProgress.luckyMeasuringInstrument.refreshCount;
			int maxRefreshCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.maxRefreshCount;
			if (refreshCount.value >= maxRefreshCount)
			{
				this.FailReroll();
				return;
			}
			this.Reroll();
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0001622F File Offset: 0x0001442F
		private void FailReroll()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactionFailedSound, base.transform.position);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00016250 File Offset: 0x00014450
		public void Reroll()
		{
			int stateNameHash = (this.remainLootCount == this.lootCount) ? LuckyMeasuringInstrumentReroll._interactFirstHash : LuckyMeasuringInstrumentReroll._interactSecondHash;
			this._animator.Play(stateNameHash, 0, 0f);
			IntData refreshCount = GameData.HardmodeProgress.luckyMeasuringInstrument.refreshCount;
			int maxRefreshCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.maxRefreshCount;
			int value = refreshCount.value;
			refreshCount.value = value + 1;
			this.UpdateInteractionGuide();
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			Action action = this.onInteracted;
			if (action != null)
			{
				action();
			}
			UnityEvent onReroll = this._onReroll;
			if (onReroll != null)
			{
				onReroll.Invoke();
			}
			if (refreshCount.value >= maxRefreshCount)
			{
				Achievement.Type.Allin.Set();
				base.Deactivate();
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00016312 File Offset: 0x00014512
		private IEnumerator CPlayEndAnimation()
		{
			yield return Chronometer.global.WaitForSeconds(1f);
			this.PlayEndAnimation();
			yield break;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00016321 File Offset: 0x00014521
		public void PlayZeroDeactivate()
		{
			this._animator.Play(LuckyMeasuringInstrumentReroll._endZeroHash, 0, 0f);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00016339 File Offset: 0x00014539
		public void PlayFirstEnd()
		{
			this._animator.Play(LuckyMeasuringInstrumentReroll._endFirstHash, 0, 0f);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00016351 File Offset: 0x00014551
		public void PlayEndAnimation()
		{
			this._animator.Play(LuckyMeasuringInstrumentReroll._endSecondHash, 0, 0f);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001636C File Offset: 0x0001456C
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			Data<int> refreshCount = GameData.HardmodeProgress.luckyMeasuringInstrument.refreshCount;
			int maxRefreshCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.maxRefreshCount;
			if (refreshCount.value > maxRefreshCount)
			{
				Debug.Log(string.Format("remainlootCount 1 {0}", this.remainLootCount));
				if (this.remainLootCount == 2)
				{
					this._rerollCount.color = Color.gray;
					this.UpdateInteractionGuide();
					this.PlayZeroDeactivate();
					return;
				}
				if (this.remainLootCount == 1)
				{
					this._rerollCount.color = Color.gray;
					this.UpdateInteractionGuide();
					this.PlayFirstEnd();
					return;
				}
				base.StartCoroutine(this.CPlayEndAnimation());
				return;
			}
			else
			{
				Debug.Log(string.Format("remainlootCount 2 {0}", this.remainLootCount));
				if (this.remainLootCount == 2)
				{
					this._rerollCount.color = Color.gray;
					this.UpdateInteractionGuide();
					this.PlayZeroDeactivate();
					return;
				}
				if (this.remainLootCount == 1)
				{
					this._rerollCount.color = Color.gray;
					this.UpdateInteractionGuide();
					this.PlayFirstEnd();
					return;
				}
				this.PlayEndAnimation();
				this._rerollCount.text = "-----";
				return;
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00016496 File Offset: 0x00014696
		public override void OpenPopupBy(Character character)
		{
			base.OpenPopupBy(character);
			this.UpdateInteractionGuide();
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x000164A8 File Offset: 0x000146A8
		private void UpdateInteractionGuide()
		{
			IntData refreshCount = GameData.HardmodeProgress.luckyMeasuringInstrument.refreshCount;
			int maxRefreshCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.maxRefreshCount;
			string text = GameData.Currency.darkQuartz.Has(this._cost) ? GameData.Currency.darkQuartz.colorCode : GameData.Currency.noMoneyColorCode;
			int num = maxRefreshCount - refreshCount.value;
			this._rerollCount.text = string.Format("{0}", num);
			this._text.text = string.Format("{0}( {1}  <color=#{2}>{3}</color> )", new object[]
			{
				Localization.GetLocalizedString("label/interaction/refresh"),
				GameData.Currency.darkQuartz.spriteTMPKey,
				text,
				this._cost
			});
			if (num <= 0)
			{
				this.ClosePopup();
				this._uiObject = null;
				this._uiObjects = new GameObject[0];
			}
		}

		// Token: 0x04000590 RID: 1424
		private static readonly int _idleFirstHash = Animator.StringToHash("LMI_First_Stop");

		// Token: 0x04000591 RID: 1425
		private static readonly int _interactFirstHash = Animator.StringToHash("LMI_First_Move");

		// Token: 0x04000592 RID: 1426
		private static readonly int _idleSecondHash = Animator.StringToHash("LMI_Second_Stop");

		// Token: 0x04000593 RID: 1427
		private static readonly int _interactSecondHash = Animator.StringToHash("LMI_Second_Move");

		// Token: 0x04000594 RID: 1428
		private static readonly int _endZeroHash = Animator.StringToHash("LMI_Deactivate");

		// Token: 0x04000595 RID: 1429
		private static readonly int _endFirstHash = Animator.StringToHash("LMI_First_Get");

		// Token: 0x04000596 RID: 1430
		private static readonly int _endSecondHash = Animator.StringToHash("LMI_End");

		// Token: 0x04000598 RID: 1432
		[SerializeField]
		private SoundInfo _interactionFailedSound;

		// Token: 0x04000599 RID: 1433
		[SerializeField]
		private Animator _animator;

		// Token: 0x0400059A RID: 1434
		[SerializeField]
		private TMP_Text _rerollPrice;

		// Token: 0x0400059B RID: 1435
		[SerializeField]
		private TMP_Text _rerollCount;

		// Token: 0x0400059C RID: 1436
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x0400059D RID: 1437
		[SerializeField]
		private UnityEvent _onReroll;

		// Token: 0x0400059E RID: 1438
		private LuckyMeasuringInstrument _base;

		// Token: 0x0400059F RID: 1439
		private int _cost;
	}
}
