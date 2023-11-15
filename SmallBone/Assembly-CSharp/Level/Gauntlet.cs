using System;
using System.Collections;
using Characters.Gear;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004DF RID: 1247
	public class Gauntlet : MonoBehaviour
	{
		// Token: 0x0600184E RID: 6222 RVA: 0x0004C202 File Offset: 0x0004A402
		public void Unlock()
		{
			this._droppedGear.destructible = true;
			this._droppedGear.lootable = true;
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0002ED6F File Offset: 0x0002CF6F
		private void Start()
		{
			base.StartCoroutine("CDrop");
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0004C21C File Offset: 0x0004A41C
		private void OnDestroy()
		{
			GearRequest gearRequest = this._gearRequest;
			if (gearRequest == null)
			{
				return;
			}
			gearRequest.Release();
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0004C22E File Offset: 0x0004A42E
		private IEnumerator CDrop()
		{
			this.Load();
			while (!this._gearRequest.isDone)
			{
				yield return null;
			}
			this._droppedGear = Singleton<Service>.Instance.levelManager.DropGear(this._gearRequest, this._dropPoint.position);
			this._droppedGear.dropped.dropMovement.Stop();
			this.Lock();
			yield break;
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0004C240 File Offset: 0x0004A440
		private void Load()
		{
			Rarity key = this._rarityPossibilities.Evaluate();
			Gear.Type? type = this._gearPossibilities.Evaluate();
			do
			{
				Rarity rarity = Settings.instance.containerPossibilities[key].Evaluate();
				if (type != null)
				{
					switch (type.GetValueOrDefault())
					{
					case Gear.Type.Weapon:
						this._gearReference = Singleton<Service>.Instance.gearManager.GetWeaponToTake(rarity);
						break;
					case Gear.Type.Item:
						this._gearReference = Singleton<Service>.Instance.gearManager.GetItemToTake(rarity);
						break;
					case Gear.Type.Quintessence:
						this._gearReference = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(rarity);
						break;
					}
				}
			}
			while (this._gearReference == null);
			this._gearRequest = this._gearReference.LoadAsync();
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x0004C302 File Offset: 0x0004A502
		private void Lock()
		{
			this._droppedGear.destructible = false;
			this._droppedGear.lootable = false;
		}

		// Token: 0x04001530 RID: 5424
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x04001531 RID: 5425
		[SerializeField]
		private GearPossibilities _gearPossibilities;

		// Token: 0x04001532 RID: 5426
		[SerializeField]
		private RarityPossibilities _rarityPossibilities;

		// Token: 0x04001533 RID: 5427
		private GearReference _gearReference;

		// Token: 0x04001534 RID: 5428
		private GearRequest _gearRequest;

		// Token: 0x04001535 RID: 5429
		private Gear _droppedGear;
	}
}
