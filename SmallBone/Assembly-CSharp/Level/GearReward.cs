using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using Characters.Gear;
using Data;
using GameResources;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;

namespace Level
{
	// Token: 0x020004E2 RID: 1250
	public sealed class GearReward : MonoBehaviour
	{
		// Token: 0x06001862 RID: 6242 RVA: 0x0004C474 File Offset: 0x0004A674
		private void Awake()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + -201960844 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x0004C4CF File Offset: 0x0004A6CF
		private void OnDestroy()
		{
			GearRequest gearRequest = this._gearRequest;
			if (gearRequest == null)
			{
				return;
			}
			gearRequest.Release();
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0004C4E1 File Offset: 0x0004A6E1
		public void Lock()
		{
			this._droppedGear.destructible = false;
			this._droppedGear.lootable = false;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x0004C4FB File Offset: 0x0004A6FB
		public void Unlock()
		{
			this._droppedGear.destructible = true;
			this._droppedGear.lootable = true;
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x0004C515 File Offset: 0x0004A715
		public void DropIndestructibleGear()
		{
			base.StartCoroutine("CDropIndestructibleGear");
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x0004C523 File Offset: 0x0004A723
		private IEnumerator CDropIndestructibleGear()
		{
			this.Load();
			while (!this._gearRequest.isDone)
			{
				yield return null;
			}
			this._droppedGear = Singleton<Service>.Instance.levelManager.DropGear(this._gearRequest, this._dropPoint.position);
			if (!this._hasMovements)
			{
				this._droppedGear.dropped.dropMovement.Stop();
			}
			this._droppedGear.destructible = false;
			UnityEvent onDrop = this._onDrop;
			if (onDrop != null)
			{
				onDrop.Invoke();
			}
			this._droppedGear.dropped.onLoot += this.OnGearLoot;
			this._droppedGear.dropped.onLoot += this.<CDropIndestructibleGear>g__OnLootIndestructibleGear|17_0;
			this._droppedGear.dropped.onDestroy += this.OnGearDestroy;
			yield break;
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x0002ED6F File Offset: 0x0002CF6F
		public void Drop()
		{
			base.StartCoroutine("CDrop");
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x0004C532 File Offset: 0x0004A732
		private IEnumerator CDrop()
		{
			this.Load();
			while (!this._gearRequest.isDone)
			{
				yield return null;
			}
			this._droppedGear = Singleton<Service>.Instance.levelManager.DropGear(this._gearRequest, this._dropPoint.position);
			if (!this._hasMovements)
			{
				this._droppedGear.dropped.dropMovement.Stop();
			}
			UnityEvent onDrop = this._onDrop;
			if (onDrop != null)
			{
				onDrop.Invoke();
			}
			this._droppedGear.dropped.onLoot += this.OnGearLoot;
			this._droppedGear.dropped.onDestroy += this.OnGearDestroy;
			yield break;
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x0004C544 File Offset: 0x0004A744
		private void Load()
		{
			do
			{
				Rarity key = this._rarityPossibilities.Evaluate(this._random);
				Gear.Type? type = this._gearPossibilities.Evaluate(this._random);
				Rarity rarity = Settings.instance.containerPossibilities[key].Evaluate(this._random);
				if (type != null)
				{
					switch (type.GetValueOrDefault())
					{
					case Gear.Type.Weapon:
						this._gearReference = Singleton<Service>.Instance.gearManager.GetWeaponToTake(this._random, rarity);
						break;
					case Gear.Type.Item:
						this._gearReference = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, rarity);
						break;
					case Gear.Type.Quintessence:
						this._gearReference = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(this._random, rarity);
						break;
					}
				}
			}
			while (this._gearReference == null);
			this._gearRequest = this._gearReference.LoadAsync();
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x0004C62C File Offset: 0x0004A82C
		private void OnGearLoot(Character character)
		{
			this._droppedGear.dropped.onLoot -= this.OnGearLoot;
			this._droppedGear.dropped.onDestroy -= this.OnGearDestroy;
			UnityEvent onLoot = this._onLoot;
			if (onLoot == null)
			{
				return;
			}
			onLoot.Invoke();
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0004C684 File Offset: 0x0004A884
		private void OnGearDestroy(Character character)
		{
			this._droppedGear.dropped.onLoot -= this.OnGearLoot;
			this._droppedGear.dropped.onDestroy -= this.OnGearDestroy;
			UnityEvent onDestroy = this._onDestroy;
			if (onDestroy == null)
			{
				return;
			}
			onDestroy.Invoke();
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x0004C6D9 File Offset: 0x0004A8D9
		[CompilerGenerated]
		private void <CDropIndestructibleGear>g__OnLootIndestructibleGear|17_0(Character character)
		{
			this.Unlock();
			this._droppedGear.dropped.onLoot -= this.<CDropIndestructibleGear>g__OnLootIndestructibleGear|17_0;
		}

		// Token: 0x0400153B RID: 5435
		private const int _randomSeed = -201960844;

		// Token: 0x0400153C RID: 5436
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x0400153D RID: 5437
		[SerializeField]
		private bool _hasMovements;

		// Token: 0x0400153E RID: 5438
		[SerializeField]
		private GearPossibilities _gearPossibilities;

		// Token: 0x0400153F RID: 5439
		[SerializeField]
		private RarityPossibilities _rarityPossibilities;

		// Token: 0x04001540 RID: 5440
		[SerializeField]
		private UnityEvent _onDrop;

		// Token: 0x04001541 RID: 5441
		[SerializeField]
		private UnityEvent _onDestroy;

		// Token: 0x04001542 RID: 5442
		[SerializeField]
		private UnityEvent _onLoot;

		// Token: 0x04001543 RID: 5443
		private GearReference _gearReference;

		// Token: 0x04001544 RID: 5444
		private GearRequest _gearRequest;

		// Token: 0x04001545 RID: 5445
		private Gear _droppedGear;

		// Token: 0x04001546 RID: 5446
		private System.Random _random;
	}
}
