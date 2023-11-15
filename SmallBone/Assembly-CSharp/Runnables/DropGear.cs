using System;
using System.Collections;
using Characters.Gear;
using Data;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200031C RID: 796
	public sealed class DropGear : Runnable
	{
		// Token: 0x06000F5B RID: 3931 RVA: 0x0002ED6F File Offset: 0x0002CF6F
		public override void Run()
		{
			base.StartCoroutine("CDrop");
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0002ED7D File Offset: 0x0002CF7D
		private void OnDestroy()
		{
			GearRequest gearRequest = this._gearRequest;
			if (gearRequest == null)
			{
				return;
			}
			gearRequest.Release();
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0002ED8F File Offset: 0x0002CF8F
		private IEnumerator CDrop()
		{
			this.Load();
			while (!this._gearRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropGear(this._gearRequest, base.transform.position);
			yield break;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0002EDA0 File Offset: 0x0002CFA0
		private void Load()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			System.Random random = new System.Random((int)(GameData.Save.instance.randomSeed + -1728020458 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			Rarity key = this._rarityPossibilities.Evaluate();
			Gear.Type? type = this._gearPossibilities.Evaluate();
			do
			{
				Rarity rarity = Settings.instance.containerPossibilities[key].Evaluate(random);
				if (type != null)
				{
					switch (type.GetValueOrDefault())
					{
					case Gear.Type.Weapon:
						this._gearReference = Singleton<Service>.Instance.gearManager.GetWeaponToTake(random, rarity);
						break;
					case Gear.Type.Item:
						this._gearReference = Singleton<Service>.Instance.gearManager.GetItemToTake(random, rarity);
						break;
					case Gear.Type.Quintessence:
						this._gearReference = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(random, rarity);
						break;
					}
				}
			}
			while (this._gearReference == null);
			this._gearRequest = this._gearReference.LoadAsync();
		}

		// Token: 0x04000CAB RID: 3243
		private const int _randomSeed = -1728020458;

		// Token: 0x04000CAC RID: 3244
		[SerializeField]
		private GearPossibilities _gearPossibilities;

		// Token: 0x04000CAD RID: 3245
		[SerializeField]
		private RarityPossibilities _rarityPossibilities;

		// Token: 0x04000CAE RID: 3246
		private GearReference _gearReference;

		// Token: 0x04000CAF RID: 3247
		private GearRequest _gearRequest;
	}
}
