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
	// Token: 0x020005E3 RID: 1507
	public sealed class FieldOgre : FieldNpc
	{
		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001E1B RID: 7707 RVA: 0x0005B9BF File Offset: 0x00059BBF
		protected override NpcType _type
		{
			get
			{
				return NpcType.FieldOgre;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001E1C RID: 7708 RVA: 0x0005AE76 File Offset: 0x00059076
		private RarityPossibilities _headPossibilities
		{
			get
			{
				return Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.sleepySekeletonHeadPossibilities;
			}
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x0005B9C4 File Offset: 0x00059BC4
		protected override void Start()
		{
			base.Start();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x0005BA28 File Offset: 0x00059C28
		private void Load()
		{
			do
			{
				Rarity rarity = this._headPossibilities.Evaluate(this._random);
				this._itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, rarity);
			}
			while (this._itemToDrop == null);
			this._itemRequest = this._itemToDrop.LoadAsync();
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x0005BA7C File Offset: 0x00059C7C
		protected override void Interact(Character character)
		{
			base.Interact(character);
			FieldNpc.Phase phase = this._phase;
			if (phase <= FieldNpc.Phase.Greeted)
			{
				base.StartCoroutine(this.CGreetingAndConfirm(character, null));
				return;
			}
			if (phase != FieldNpc.Phase.Gave)
			{
				return;
			}
			base.StartCoroutine(base.CChat());
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0005BABD File Offset: 0x00059CBD
		private IEnumerator CGreetingAndConfirm(Character character, object confirmArg = null)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.skippable = true;
			int lastIndex = 3;
			int num;
			for (int i = 0; i < lastIndex; i = num + 1)
			{
				yield return this._npcConversation.CConversation(new string[]
				{
					base._greeting[i]
				});
				num = i;
			}
			base.StartCoroutine(this.CDropWeapon());
			this._phase = FieldNpc.Phase.Gave;
			for (int i = lastIndex; i < base._greeting.Length; i = num + 1)
			{
				yield return this._npcConversation.CConversation(new string[]
				{
					base._greeting[i]
				});
				num = i;
			}
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x0005BACC File Offset: 0x00059CCC
		private IEnumerator CDropWeapon()
		{
			this.Load();
			while (!this._itemRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged -= this.Load;
			Singleton<Service>.Instance.levelManager.DropItem(this._itemRequest, this._dropPosition.position);
			int num = (int)this._currencyAmount.value;
			Singleton<Service>.Instance.levelManager.DropGold(num, Mathf.Min(num / 10, 30));
			this._dropEffect.Spawn(this._dropPosition.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dropSound, base.transform.position);
			yield break;
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x0005BADB File Offset: 0x00059CDB
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

		// Token: 0x0400196D RID: 6509
		private const int _randomSeed = 2028506624;

		// Token: 0x0400196E RID: 6510
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x0400196F RID: 6511
		[SerializeField]
		private EffectInfo _dropEffect;

		// Token: 0x04001970 RID: 6512
		[SerializeField]
		private SoundInfo _dropSound;

		// Token: 0x04001971 RID: 6513
		[SerializeField]
		private CustomFloat _currencyAmount;

		// Token: 0x04001972 RID: 6514
		private ItemReference _itemToDrop;

		// Token: 0x04001973 RID: 6515
		private ItemRequest _itemRequest;

		// Token: 0x04001974 RID: 6516
		private System.Random _random;
	}
}
