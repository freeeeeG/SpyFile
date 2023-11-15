using System;
using System.Collections;
using Characters;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005F3 RID: 1523
	public class Plebby : FieldNpc
	{
		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001E76 RID: 7798 RVA: 0x000147BD File Offset: 0x000129BD
		protected override NpcType _type
		{
			get
			{
				return NpcType.Plebby;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001E77 RID: 7799 RVA: 0x0005CB06 File Offset: 0x0005AD06
		protected string _displayNameA
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/A/name", this._type));
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001E78 RID: 7800 RVA: 0x0005CB22 File Offset: 0x0005AD22
		protected string _displayNameB
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/B/name", this._type));
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001E79 RID: 7801 RVA: 0x0005CB3E File Offset: 0x0005AD3E
		private int _goldCost
		{
			get
			{
				return Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.plebbyGoldCost;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001E7A RID: 7802 RVA: 0x0005CB5E File Offset: 0x0005AD5E
		private RarityPossibilities _itemPossibilities
		{
			get
			{
				return Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.plebbyItemPossibilities;
			}
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0005CB80 File Offset: 0x0005AD80
		protected override void Start()
		{
			base.Start();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0005CBE1 File Offset: 0x0005ADE1
		protected override void OnDestroy()
		{
			base.OnDestroy();
			ItemRequest itemRequest = this._itemRequest;
			if (itemRequest == null)
			{
				return;
			}
			itemRequest.Release();
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0005CBFC File Offset: 0x0005ADFC
		private void Load()
		{
			ItemRequest itemRequest = this._itemRequest;
			if (itemRequest != null)
			{
				itemRequest.Release();
			}
			do
			{
				Rarity rarity = this._itemPossibilities.Evaluate(this._random);
				this._itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, rarity);
			}
			while (this._itemToDrop == null);
			this._itemRequest = this._itemToDrop.LoadAsync();
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0005CC64 File Offset: 0x0005AE64
		protected override void Interact(Character character)
		{
			base.Interact(character);
			FieldNpc.Phase phase = this._phase;
			if (phase <= FieldNpc.Phase.Greeted)
			{
				base.StartCoroutine(this.CGreetingAndConfirm(character));
				return;
			}
			if (phase != FieldNpc.Phase.Gave)
			{
				return;
			}
			base.StartCoroutine(this.CChat());
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0005CCA4 File Offset: 0x0005AEA4
		private IEnumerator CGreetingAndConfirm(Character character)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.skippable = true;
			string arg = (this._phase == FieldNpc.Phase.Initial) ? "greeting" : "regreeting";
			string[] greeting = Localization.GetLocalizedStringArray(string.Format("npc/{0}/{1}", this._type, arg));
			string[] speaker = Localization.GetLocalizedStringArray(string.Format("npc/{0}/{1}/speaker", this._type, arg));
			this._phase = FieldNpc.Phase.Greeted;
			int lastIndex = greeting.Length - 1;
			int num;
			for (int i = 0; i < lastIndex; i = num + 1)
			{
				this._npcConversation.name = speaker[i];
				yield return this._npcConversation.CConversation(new string[]
				{
					greeting[i]
				});
				num = i;
			}
			this._npcConversation.name = speaker[lastIndex];
			this._npcConversation.body = string.Format(greeting[lastIndex], this._goldCost);
			this._npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.Gold);
			yield return this._npcConversation.CType();
			yield return new WaitForSecondsRealtime(0.3f);
			this._npcConversation.OpenConfirmSelector(new Action(this.OnConfirmed), new Action(base.Close));
			yield break;
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x0005CCB4 File Offset: 0x0005AEB4
		private void OnConfirmed()
		{
			this._npcConversation.CloseCurrencyBalancePanel();
			if (GameData.Currency.gold.Has(this._goldCost))
			{
				this._phase = FieldNpc.Phase.Gave;
				base.StartCoroutine(this.CConfirmed());
				return;
			}
			base.StartCoroutine(this.CNoMoneyAndClose());
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x0005CD00 File Offset: 0x0005AF00
		private IEnumerator CConversation(string key)
		{
			string[] localizedStringArray = Localization.GetLocalizedStringArray(key);
			string[] localizedStringArray2 = Localization.GetLocalizedStringArray(key + "/speaker");
			yield return this.CConversation(localizedStringArray2, localizedStringArray);
			yield break;
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0005CD16 File Offset: 0x0005AF16
		private IEnumerator CConversation(string[] speakers, string[] scripts)
		{
			int num;
			for (int i = 0; i < scripts.Length; i = num + 1)
			{
				this._npcConversation.name = speakers[i];
				yield return this._npcConversation.CConversation(new string[]
				{
					scripts[i]
				});
				num = i;
			}
			yield break;
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0005CD33 File Offset: 0x0005AF33
		private IEnumerator CConfirmed()
		{
			this._npcConversation.skippable = true;
			this.Load();
			yield return this.CDropItem();
			GameData.Currency.gold.Consume(this._goldCost);
			yield return this.CConversation(string.Format("npc/{0}/confirmed", this._type));
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x0005CD42 File Offset: 0x0005AF42
		private IEnumerator CDropItem()
		{
			while (!this._itemRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.Load;
			Singleton<Service>.Instance.levelManager.DropItem(this._itemRequest, this._dropPosition.position);
			this._dropEffect.Spawn(this._dropPosition.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dropSound, base.transform.position);
			yield break;
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0005CD51 File Offset: 0x0005AF51
		private IEnumerator CNoMoneyAndClose()
		{
			this._npcConversation.skippable = true;
			yield return this.CConversation(string.Format("npc/{0}/noMoney", this._type));
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x0005CD60 File Offset: 0x0005AF60
		private new IEnumerator CChat()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.skippable = true;
			string[][] localizedStringArrays = Localization.GetLocalizedStringArrays(string.Format("npc/{0}/chat", this._type));
			string[][] localizedStringArrays2 = Localization.GetLocalizedStringArrays(string.Format("npc/{0}/chat/speaker", this._type));
			int num = localizedStringArrays.RandomIndex<string[]>();
			yield return this.CConversation(localizedStringArrays2[num], localizedStringArrays[num]);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x040019C6 RID: 6598
		private const int _randomSeed = 2028506624;

		// Token: 0x040019C7 RID: 6599
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x040019C8 RID: 6600
		[SerializeField]
		private EffectInfo _dropEffect;

		// Token: 0x040019C9 RID: 6601
		[SerializeField]
		private SoundInfo _dropSound;

		// Token: 0x040019CA RID: 6602
		private ItemReference _itemToDrop;

		// Token: 0x040019CB RID: 6603
		private ItemRequest _itemRequest;

		// Token: 0x040019CC RID: 6604
		private System.Random _random;
	}
}
