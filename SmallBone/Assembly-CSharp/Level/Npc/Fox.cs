using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x020005A4 RID: 1444
	public class Fox : InteractiveObject
	{
		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001C94 RID: 7316 RVA: 0x000581E9 File Offset: 0x000563E9
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/name", NpcType.Fox));
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x00058200 File Offset: 0x00056400
		public string greeting
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/greeting", NpcType.Fox)).Random<string>();
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001C96 RID: 7318 RVA: 0x0005821C File Offset: 0x0005641C
		public string[] chat
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/chat", NpcType.Fox)).Random<string[]>();
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x00058238 File Offset: 0x00056438
		public string[] giveHeadScripts
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/GiveHead", NpcType.Fox)).Random<string[]>();
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001C98 RID: 7320 RVA: 0x00058254 File Offset: 0x00056454
		public string giveExtraHead
		{
			get
			{
				return string.Format(Localization.GetLocalizedStringArray(string.Format("npc/{0}/GiveExtraHead", NpcType.Fox)).Random<string>(), this._extraHeadDarkQuartzCost);
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001C99 RID: 7321 RVA: 0x00058280 File Offset: 0x00056480
		public string giveExtraHeadLabel
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/GiveExtraHead/label", NpcType.Fox));
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001C9A RID: 7322 RVA: 0x00058297 File Offset: 0x00056497
		public string[] giveExtraHeadNoMoney
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/GiveExtraHead/NoMoney", NpcType.Fox)).Random<string[]>();
			}
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000582B3 File Offset: 0x000564B3
		protected override void Awake()
		{
			base.Awake();
			if (!GameData.Progress.GetRescued(NpcType.Fox))
			{
				base.gameObject.SetActive(false);
			}
			this._random = new System.Random(GameData.Save.instance.randomSeed + 1141070443);
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x000582EC File Offset: 0x000564EC
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			this._weaponToDrop = Singleton<Service>.Instance.gearManager.GetWeaponToTake(this._random, this._headPossibilities.Evaluate(this._random)).LoadAsync();
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000570E4 File Offset: 0x000552E4
		private void OnDisable()
		{
			if (Service.quitting)
			{
				return;
			}
			LetterBox.instance.visible = false;
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x0005833F File Offset: 0x0005653F
		private void OnDestroy()
		{
			WeaponRequest weaponToDrop = this._weaponToDrop;
			if (weaponToDrop != null)
			{
				weaponToDrop.Release();
			}
			WeaponRequest extraWeaponToDrop = this._extraWeaponToDrop;
			if (extraWeaponToDrop == null)
			{
				return;
			}
			extraWeaponToDrop.Release();
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x00058364 File Offset: 0x00056564
		public override void InteractWith(Character character)
		{
			this._npcConversation.name = this.displayName;
			this._npcConversation.portrait = this._portrait;
			switch (this._phase)
			{
			case Fox.Phase.Initial:
				this._phase = Fox.Phase.Gave;
				base.StartCoroutine(this.CGiveHead(character));
				return;
			case Fox.Phase.Gave:
				base.StartCoroutine(this.CSelectContent());
				return;
			case Fox.Phase.ExtraGave:
				this.Chat();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x000583D7 File Offset: 0x000565D7
		private IEnumerator CSelectContent()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.body = this.greeting;
			this._npcConversation.skippable = false;
			this._npcConversation.Type();
			this._npcConversation.OpenContentSelector(this.giveExtraHeadLabel, new Action(this.GetExtraHead), new Action(this.Chat), new Action(this.Close));
			yield break;
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x000583E6 File Offset: 0x000565E6
		private void Chat()
		{
			this._npcConversation.skippable = true;
			base.StartCoroutine(this.<Chat>g__CRun|32_0());
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x00058401 File Offset: 0x00056601
		private void Close()
		{
			this._npcConversation.visible = false;
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0005841E File Offset: 0x0005661E
		private void GetExtraHead()
		{
			base.StartCoroutine(this.<GetExtraHead>g__CRun|34_0());
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x0005842D File Offset: 0x0005662D
		private IEnumerator CGiveHead(Character character)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.skippable = true;
			yield return this._npcConversation.CConversation(this.giveHeadScripts);
			LetterBox.instance.Disappear(0.4f);
			while (!this._weaponToDrop.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropWeapon(this._weaponToDrop, this._dropPosition.position);
			this._extraWeaponToDrop = Singleton<Service>.Instance.gearManager.GetWeaponToTake(this._random, this._headPossibilities.Evaluate(this._random)).LoadAsync();
			yield break;
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x0005843C File Offset: 0x0005663C
		[CompilerGenerated]
		private IEnumerator <Chat>g__CRun|32_0()
		{
			if (!LetterBox.instance.visible)
			{
				yield return LetterBox.instance.CAppear(0.4f);
			}
			yield return this._npcConversation.CConversation(this.chat);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0005844B File Offset: 0x0005664B
		[CompilerGenerated]
		private IEnumerator <GetExtraHead>g__CRun|34_0()
		{
			this._npcConversation.skippable = true;
			this._npcConversation.body = this.giveExtraHead;
			yield return this._npcConversation.CType();
			this._npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.DarkQuartz);
			this._npcConversation.OpenConfirmSelector(new Action(this.<GetExtraHead>g__OnYesSelected|34_1), new Action(this.Close));
			yield break;
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0005845C File Offset: 0x0005665C
		[CompilerGenerated]
		private void <GetExtraHead>g__OnYesSelected|34_1()
		{
			if (GameData.Currency.darkQuartz.Consume(this._extraHeadDarkQuartzCost))
			{
				this._phase = Fox.Phase.ExtraGave;
				base.StartCoroutine(this.<GetExtraHead>g__CDropExtraHead|34_2());
				this._npcConversation.visible = false;
				return;
			}
			base.StartCoroutine(this.<GetExtraHead>g__CNoMoneyAndClose|34_3());
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x000584A9 File Offset: 0x000566A9
		[CompilerGenerated]
		private IEnumerator <GetExtraHead>g__CDropExtraHead|34_2()
		{
			while (!this._extraWeaponToDrop.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropWeapon(this._extraWeaponToDrop, this._dropPosition.position);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x000584B8 File Offset: 0x000566B8
		[CompilerGenerated]
		private IEnumerator <GetExtraHead>g__CNoMoneyAndClose|34_3()
		{
			this._npcConversation.skippable = true;
			yield return this._npcConversation.CConversation(this.giveExtraHeadNoMoney);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x04001851 RID: 6225
		private const int _randomSeed = 1141070443;

		// Token: 0x04001852 RID: 6226
		private const NpcType _type = NpcType.Fox;

		// Token: 0x04001853 RID: 6227
		[SerializeField]
		private int _extraHeadDarkQuartzCost;

		// Token: 0x04001854 RID: 6228
		[SerializeField]
		private RarityPossibilities _headPossibilities;

		// Token: 0x04001855 RID: 6229
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x04001856 RID: 6230
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x04001857 RID: 6231
		private Fox.Phase _phase;

		// Token: 0x04001858 RID: 6232
		private NpcConversation _npcConversation;

		// Token: 0x04001859 RID: 6233
		private WeaponRequest _weaponToDrop;

		// Token: 0x0400185A RID: 6234
		private WeaponRequest _extraWeaponToDrop;

		// Token: 0x0400185B RID: 6235
		private System.Random _random;

		// Token: 0x020005A5 RID: 1445
		private enum Phase
		{
			// Token: 0x0400185D RID: 6237
			Initial,
			// Token: 0x0400185E RID: 6238
			Gave,
			// Token: 0x0400185F RID: 6239
			ExtraGave
		}
	}
}
