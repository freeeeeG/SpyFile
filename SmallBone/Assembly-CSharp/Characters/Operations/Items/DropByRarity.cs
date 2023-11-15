using System;
using System.Collections;
using Data;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Items
{
	// Token: 0x02000E7C RID: 3708
	public sealed class DropByRarity : CharacterOperation
	{
		// Token: 0x06004978 RID: 18808 RVA: 0x000D6D38 File Offset: 0x000D4F38
		public override void Initialize()
		{
			base.Initialize();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			DropByRarity._extraSeed++;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + DropByRarity._extraSeed + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this._rarity = this._rarityPossibilities.Evaluate(this._random);
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x000D6DC4 File Offset: 0x000D4FC4
		private void Load()
		{
			ItemRequest itemRequest = this._itemRequest;
			if (itemRequest != null)
			{
				itemRequest.Release();
			}
			do
			{
				this._itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, this._rarity);
			}
			while (this._itemToDrop == null);
			this._itemRequest = this._itemToDrop.LoadAsync();
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x000D6E1C File Offset: 0x000D501C
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x000D6E2B File Offset: 0x000D502B
		private IEnumerator CRun()
		{
			this.Load();
			while (!this._itemRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropItem(this._itemRequest, base.transform.position);
			yield break;
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x000D6E3A File Offset: 0x000D503A
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.Load;
			DropByRarity._extraSeed--;
			ItemRequest itemRequest = this._itemRequest;
			if (itemRequest == null)
			{
				return;
			}
			itemRequest.Release();
		}

		// Token: 0x040038B8 RID: 14520
		private static int _extraSeed;

		// Token: 0x040038B9 RID: 14521
		private const int _randomSeed = 2028506624;

		// Token: 0x040038BA RID: 14522
		[SerializeField]
		private RarityPossibilities _rarityPossibilities;

		// Token: 0x040038BB RID: 14523
		private System.Random _random;

		// Token: 0x040038BC RID: 14524
		private Rarity _rarity;

		// Token: 0x040038BD RID: 14525
		private ItemReference _itemToDrop;

		// Token: 0x040038BE RID: 14526
		private ItemRequest _itemRequest;
	}
}
