using System;
using Characters;
using Characters.Gear;
using GameResources;
using Runnables;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.SpecialMap
{
	// Token: 0x020001B7 RID: 439
	public class MysticalRuin : MonoBehaviour
	{
		// Token: 0x0600094F RID: 2383 RVA: 0x0001A9BC File Offset: 0x00018BBC
		private void Awake()
		{
			this._weightedRandomizer = WeightedRandomizer.From<Gear.Type>(new ValueTuple<Gear.Type, float>[]
			{
				new ValueTuple<Gear.Type, float>(Gear.Type.Item, this._itemWeight),
				new ValueTuple<Gear.Type, float>(Gear.Type.Weapon, this._weaponWeight),
				new ValueTuple<Gear.Type, float>(Gear.Type.Quintessence, this._quintessenceWeight)
			});
			this.DropGear();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0001AA19 File Offset: 0x00018C19
		private Gear.Type EvaluateGearType()
		{
			return this._weightedRandomizer.TakeOne();
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0001AA28 File Offset: 0x00018C28
		private void DropGear()
		{
			switch (this.EvaluateGearType())
			{
			case Gear.Type.Weapon:
				this.DropWeapon();
				return;
			case Gear.Type.Item:
				this.DropItem();
				return;
			case Gear.Type.Quintessence:
				this.DropQuintessence();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0001AA64 File Offset: 0x00018C64
		private void DropItem()
		{
			MysticalRuin.<>c__DisplayClass10_0 CS$<>8__locals1 = new MysticalRuin.<>c__DisplayClass10_0();
			CS$<>8__locals1.<>4__this = this;
			ItemReference itemToTake;
			do
			{
				Rarity rarity = this._rarityPossibilities.Evaluate();
				itemToTake = Singleton<Service>.Instance.gearManager.GetItemToTake(rarity);
			}
			while (itemToTake == null);
			CS$<>8__locals1.request = itemToTake.LoadAsync();
			base.StartCoroutine(CS$<>8__locals1.<DropItem>g__CDrop|0());
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0001AAB8 File Offset: 0x00018CB8
		private void DropWeapon()
		{
			MysticalRuin.<>c__DisplayClass11_0 CS$<>8__locals1 = new MysticalRuin.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			ItemReference itemToTake;
			do
			{
				Rarity rarity = this._rarityPossibilities.Evaluate();
				itemToTake = Singleton<Service>.Instance.gearManager.GetItemToTake(rarity);
			}
			while (itemToTake == null);
			CS$<>8__locals1.request = itemToTake.LoadAsync();
			base.StartCoroutine(CS$<>8__locals1.<DropWeapon>g__CDrop|0());
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0001AB0C File Offset: 0x00018D0C
		private void DropQuintessence()
		{
			MysticalRuin.<>c__DisplayClass12_0 CS$<>8__locals1 = new MysticalRuin.<>c__DisplayClass12_0();
			CS$<>8__locals1.<>4__this = this;
			EssenceReference quintessenceToTake;
			do
			{
				Rarity rarity = this._rarityPossibilities.Evaluate();
				quintessenceToTake = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(rarity);
			}
			while (quintessenceToTake == null);
			CS$<>8__locals1.request = quintessenceToTake.LoadAsync();
			base.StartCoroutine(CS$<>8__locals1.<DropQuintessence>g__CDrop|0());
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0001AB60 File Offset: 0x00018D60
		private void Run(Character character)
		{
			this._gear.dropped.onLoot -= this.Run;
			this._gear.dropped.onDestroy -= this.Run;
			this._runnable.Run();
		}

		// Token: 0x040007B6 RID: 1974
		[SerializeField]
		private Runnable _runnable;

		// Token: 0x040007B7 RID: 1975
		[SerializeField]
		[Range(0f, 100f)]
		private float _weaponWeight;

		// Token: 0x040007B8 RID: 1976
		[Range(0f, 100f)]
		[SerializeField]
		private float _itemWeight;

		// Token: 0x040007B9 RID: 1977
		[Range(0f, 100f)]
		[SerializeField]
		private float _quintessenceWeight;

		// Token: 0x040007BA RID: 1978
		[SerializeField]
		private RarityPossibilities _rarityPossibilities;

		// Token: 0x040007BB RID: 1979
		private WeightedRandomizer<Gear.Type> _weightedRandomizer;

		// Token: 0x040007BC RID: 1980
		private Gear _gear;
	}
}
