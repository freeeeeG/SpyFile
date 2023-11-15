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
	// Token: 0x020005B4 RID: 1460
	public class Ogre : InteractiveObject
	{
		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001CEB RID: 7403 RVA: 0x00058CE1 File Offset: 0x00056EE1
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/name", NpcType.Ogre));
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x00058CF8 File Offset: 0x00056EF8
		public string greeting
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/greeting", NpcType.Ogre)).Random<string>();
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x00058D14 File Offset: 0x00056F14
		public string[] chat
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/chat", NpcType.Ogre)).Random<string[]>();
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x00058D30 File Offset: 0x00056F30
		public string[] giveItemScripts
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/GiveItem", NpcType.Ogre)).Random<string[]>();
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x00058D4C File Offset: 0x00056F4C
		public string giveExtraItem
		{
			get
			{
				return string.Format(Localization.GetLocalizedStringArray(string.Format("npc/{0}/GiveExtraItem", NpcType.Ogre)).Random<string>(), this._extraItemDarkQuartzCost);
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x00058D78 File Offset: 0x00056F78
		public string giveExtraItemLabel
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/GiveExtraItem/label", NpcType.Ogre));
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x00058D8F File Offset: 0x00056F8F
		public string[] giveExtraItemNoMoney
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/GiveExtraItem/NoMoney", NpcType.Ogre)).Random<string[]>();
			}
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x00058DAB File Offset: 0x00056FAB
		protected override void Awake()
		{
			base.Awake();
			if (!GameData.Progress.GetRescued(NpcType.Ogre))
			{
				base.gameObject.SetActive(false);
			}
			this._random = new System.Random(GameData.Save.instance.randomSeed + -1539017818);
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x00058DE4 File Offset: 0x00056FE4
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			this._itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, this._itemPossibilities.Evaluate(this._random)).LoadAsync();
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x00058E37 File Offset: 0x00057037
		private void OnDisable()
		{
			if (Service.quitting)
			{
				return;
			}
			LetterBox.instance.visible = false;
			ItemRequest itemToDrop = this._itemToDrop;
			if (itemToDrop != null)
			{
				itemToDrop.Release();
			}
			ItemRequest extraItemToDrop = this._extraItemToDrop;
			if (extraItemToDrop == null)
			{
				return;
			}
			extraItemToDrop.Release();
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x00058E70 File Offset: 0x00057070
		public override void InteractWith(Character character)
		{
			this._npcConversation.name = this.displayName;
			this._npcConversation.portrait = this._portrait;
			switch (this._phase)
			{
			case Ogre.Phase.Initial:
				this._phase = Ogre.Phase.Gave;
				base.StartCoroutine(this.CGiveItem(character));
				return;
			case Ogre.Phase.Gave:
				base.StartCoroutine(this.CSelectContent());
				return;
			case Ogre.Phase.ExtraGave:
				this.Chat();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x00058EE3 File Offset: 0x000570E3
		private IEnumerator CSelectContent()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.body = this.greeting;
			this._npcConversation.skippable = false;
			this._npcConversation.Type();
			this._npcConversation.OpenContentSelector(this.giveExtraItemLabel, new Action(this.GetExtraItem), new Action(this.Chat), new Action(this.Close));
			yield break;
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x00058EF2 File Offset: 0x000570F2
		private void Chat()
		{
			this._npcConversation.skippable = true;
			base.StartCoroutine(this.<Chat>g__CRun|31_0());
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x00058F0D File Offset: 0x0005710D
		private void Close()
		{
			this._npcConversation.visible = false;
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x00058F2A File Offset: 0x0005712A
		private void GetExtraItem()
		{
			base.StartCoroutine(this.<GetExtraItem>g__CRun|33_0());
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x00058F39 File Offset: 0x00057139
		private IEnumerator CGiveItem(Character character)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.skippable = true;
			yield return this._npcConversation.CConversation(this.giveItemScripts);
			LetterBox.instance.Disappear(0.4f);
			while (!this._itemToDrop.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropItem(this._itemToDrop, this._dropPosition.position);
			this._extraItemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, this._itemPossibilities.Evaluate(this._random)).LoadAsync();
			yield break;
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x00058F48 File Offset: 0x00057148
		[CompilerGenerated]
		private IEnumerator <Chat>g__CRun|31_0()
		{
			if (!LetterBox.instance.visible)
			{
				yield return LetterBox.instance.CAppear(0.4f);
			}
			yield return this._npcConversation.CConversation(this.chat);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x00058F57 File Offset: 0x00057157
		[CompilerGenerated]
		private IEnumerator <GetExtraItem>g__CRun|33_0()
		{
			this._npcConversation.skippable = true;
			this._npcConversation.body = this.giveExtraItem;
			yield return this._npcConversation.CType();
			this._npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.DarkQuartz);
			this._npcConversation.OpenConfirmSelector(new Action(this.<GetExtraItem>g__OnYesSelected|33_1), new Action(this.Close));
			yield break;
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x00058F68 File Offset: 0x00057168
		[CompilerGenerated]
		private void <GetExtraItem>g__OnYesSelected|33_1()
		{
			if (GameData.Currency.darkQuartz.Consume(this._extraItemDarkQuartzCost))
			{
				this._phase = Ogre.Phase.ExtraGave;
				base.StartCoroutine(this.<GetExtraItem>g__CDropExtraHead|33_2());
				this._npcConversation.visible = false;
				LetterBox.instance.Disappear(0.4f);
				return;
			}
			base.StartCoroutine(this.<GetExtraItem>g__CNoMoneyAndClose|33_3());
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x00058FC4 File Offset: 0x000571C4
		[CompilerGenerated]
		private IEnumerator <GetExtraItem>g__CDropExtraHead|33_2()
		{
			while (!this._extraItemToDrop.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropItem(this._extraItemToDrop, this._dropPosition.position);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x00058FD3 File Offset: 0x000571D3
		[CompilerGenerated]
		private IEnumerator <GetExtraItem>g__CNoMoneyAndClose|33_3()
		{
			this._npcConversation.skippable = true;
			yield return this._npcConversation.CConversation(this.giveExtraItemNoMoney);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x04001897 RID: 6295
		private const int _randomSeed = -1539017818;

		// Token: 0x04001898 RID: 6296
		private const NpcType _type = NpcType.Ogre;

		// Token: 0x04001899 RID: 6297
		[SerializeField]
		private int _extraItemDarkQuartzCost;

		// Token: 0x0400189A RID: 6298
		[SerializeField]
		private RarityPossibilities _itemPossibilities;

		// Token: 0x0400189B RID: 6299
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x0400189C RID: 6300
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x0400189D RID: 6301
		private Ogre.Phase _phase;

		// Token: 0x0400189E RID: 6302
		private NpcConversation _npcConversation;

		// Token: 0x0400189F RID: 6303
		private ItemRequest _itemToDrop;

		// Token: 0x040018A0 RID: 6304
		private ItemRequest _extraItemToDrop;

		// Token: 0x040018A1 RID: 6305
		private System.Random _random;

		// Token: 0x020005B5 RID: 1461
		private enum Phase
		{
			// Token: 0x040018A3 RID: 6307
			Initial,
			// Token: 0x040018A4 RID: 6308
			Gave,
			// Token: 0x040018A5 RID: 6309
			ExtraGave
		}
	}
}
