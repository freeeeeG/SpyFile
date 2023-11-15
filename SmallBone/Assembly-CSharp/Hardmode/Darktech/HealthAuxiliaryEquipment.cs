using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using Characters.Abilities;
using Data;
using FX;
using GameResources;
using Platforms;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x02000166 RID: 358
	public sealed class HealthAuxiliaryEquipment : InteractiveObject
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00014763 File Offset: 0x00012963
		private string displayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/{1}", this._ability, "name"));
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x00014784 File Offset: 0x00012984
		private string description
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("darktech/equipment/{0}/{1}", this._ability, "desc"));
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x000147A5 File Offset: 0x000129A5
		private string noMoney
		{
			get
			{
				return Localization.GetLocalizedString("darktech/equipment/HealthAuxiliaryEquipment/noMoney");
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x000147B1 File Offset: 0x000129B1
		private string end
		{
			get
			{
				return Localization.GetLocalizedString("darktech/equipment/HealthAuxiliaryEquipment/end");
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x000147BD File Offset: 0x000129BD
		private GameData.Currency.Type _currencyType
		{
			get
			{
				return GameData.Currency.Type.Bone;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x000147C0 File Offset: 0x000129C0
		private GameData.Currency _currency
		{
			get
			{
				return GameData.Currency.bone;
			}
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000147C8 File Offset: 0x000129C8
		private void Start()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			this._currentIndex = (int)player.playerComponents.savableAbilityManager.GetStack(this._ability);
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			switch (this._ability)
			{
			case SavableAbilityManager.Name.HealthAuxiliaryDamage:
				this._maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프가격.Length;
				if (this._currentIndex >= this._maxIndex)
				{
					this._cost = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프가격[this._maxIndex - 1];
				}
				else
				{
					this._cost = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프가격[this._currentIndex];
				}
				this._statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프스텟;
				break;
			case SavableAbilityManager.Name.HealthAuxiliaryHealth:
				this._maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프가격.Length;
				if (this._currentIndex >= this._maxIndex)
				{
					this._cost = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프가격[this._maxIndex - 1];
				}
				else
				{
					this._cost = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프가격[this._currentIndex];
				}
				this._statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프스텟;
				break;
			case SavableAbilityManager.Name.HealthAuxiliarySpeed:
				this._maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프가격.Length;
				if (this._currentIndex >= this._maxIndex)
				{
					this._cost = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프가격[this._maxIndex - 1];
				}
				else
				{
					this._cost = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프가격[this._currentIndex];
				}
				this._statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프스텟;
				break;
			}
			for (int i = 0; i < this._levelDisplay.Length; i++)
			{
				this._levelDisplay[i].SetActive(false);
			}
			for (int j = 0; j < this._currentIndex; j++)
			{
				this._levelDisplay[j].SetActive(true);
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x000149E0 File Offset: 0x00012BE0
		public override void InteractWith(Character character)
		{
			base.StartCoroutine(this.COpen());
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000149EF File Offset: 0x00012BEF
		private IEnumerator COpen()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.name = this.displayName;
			if (this._ability == SavableAbilityManager.Name.HealthAuxiliaryHealth)
			{
				this._npcConversation.body = string.Format(this.description, new object[]
				{
					this.displayName,
					this._cost,
					this._statValues[this._currentIndex],
					Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프스텟[this._currentIndex]
				});
			}
			else
			{
				this._npcConversation.body = string.Format(this.description, this.displayName, this._cost, this._statValues[this._currentIndex] * 100f);
			}
			this._npcConversation.skippable = true;
			this._npcConversation.portrait = null;
			this._npcConversation.Type();
			this._npcConversation.OpenCurrencyBalancePanel(this._currencyType);
			yield return this._npcConversation.CType();
			this._npcConversation.OpenConfirmSelector(new Action(this.<COpen>g__OnSelectYes|25_0), new Action(this.Close));
			yield break;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00014A00 File Offset: 0x00012C00
		private void UpdateStack()
		{
			if (this._currentIndex >= this._maxIndex)
			{
				return;
			}
			Character player = Singleton<Service>.Instance.levelManager.player;
			this._currentIndex = (int)player.playerComponents.savableAbilityManager.GetStack(this._ability);
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			switch (this._ability)
			{
			case SavableAbilityManager.Name.HealthAuxiliaryDamage:
				this._cost = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프가격[this._currentIndex];
				this._maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프가격.Length;
				this._statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프스텟;
				break;
			case SavableAbilityManager.Name.HealthAuxiliaryHealth:
				this._cost = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프가격[this._currentIndex];
				this._maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프가격.Length;
				this._statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프스텟;
				break;
			case SavableAbilityManager.Name.HealthAuxiliarySpeed:
				this._cost = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프가격[this._currentIndex];
				this._maxIndex = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프가격.Length;
				this._statValues = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프스텟;
				break;
			}
			for (int i = 0; i < this._levelDisplay.Length; i++)
			{
				this._levelDisplay[i].SetActive(false);
			}
			for (int j = 0; j < this._currentIndex; j++)
			{
				this._levelDisplay[j].SetActive(true);
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00014BA0 File Offset: 0x00012DA0
		private void GiveBuff()
		{
			this._onLevelUpEffect.Spawn(this._levelDisplay[this._currentIndex].transform.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._onLevelUpSound, this._levelDisplay[this._currentIndex].transform.position);
			this._levelDisplay[this._currentIndex].SetActive(true);
			this._currentIndex++;
			if (this._currentIndex >= this._maxIndex)
			{
				Achievement.Type.BlessingOfSpiderGod.Set();
			}
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.playerComponents.savableAbilityManager.Apply(this._ability, this._currentIndex);
			if (this._ability == SavableAbilityManager.Name.HealthAuxiliaryHealth)
			{
				player.health.Heal((double)Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프회복량[this._currentIndex], true);
			}
			this._animator.SetTrigger(this._animatorTriggerKey);
			this.Close();
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00014CA8 File Offset: 0x00012EA8
		private IEnumerator CFail(string body)
		{
			this._npcConversation.skippable = true;
			this._npcConversation.body = body;
			yield return this._npcConversation.CType();
			yield return this._npcConversation.CWaitInput();
			this.Close();
			yield break;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00014CBE File Offset: 0x00012EBE
		private void Close()
		{
			this._npcConversation.visible = false;
			this._npcConversation.CloseCurrencyBalancePanel();
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00014CF0 File Offset: 0x00012EF0
		[CompilerGenerated]
		private void <COpen>g__OnSelectYes|25_0()
		{
			this._npcConversation.CloseCurrencyBalancePanel();
			if (this._currentIndex >= this._maxIndex)
			{
				base.StartCoroutine(this.CFail(this.end));
				return;
			}
			if (this._currency.Consume(this._cost))
			{
				this.GiveBuff();
				this.UpdateStack();
				return;
			}
			base.StartCoroutine(this.CFail(this.noMoney));
		}

		// Token: 0x04000551 RID: 1361
		[SerializeField]
		private SavableAbilityManager.Name _ability;

		// Token: 0x04000552 RID: 1362
		[SerializeField]
		private GameObject[] _levelDisplay;

		// Token: 0x04000553 RID: 1363
		[SerializeField]
		private EffectInfo _onLevelUpEffect;

		// Token: 0x04000554 RID: 1364
		[SerializeField]
		private SoundInfo _onLevelUpSound;

		// Token: 0x04000555 RID: 1365
		[SerializeField]
		private Animator _animator;

		// Token: 0x04000556 RID: 1366
		[SerializeField]
		private string _animatorTriggerKey;

		// Token: 0x04000557 RID: 1367
		private NpcConversation _npcConversation;

		// Token: 0x04000558 RID: 1368
		private int _currentIndex;

		// Token: 0x04000559 RID: 1369
		private int _maxIndex;

		// Token: 0x0400055A RID: 1370
		private int _cost;

		// Token: 0x0400055B RID: 1371
		private float[] _statValues;
	}
}
