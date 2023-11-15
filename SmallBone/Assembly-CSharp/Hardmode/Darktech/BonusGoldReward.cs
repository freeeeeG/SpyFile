using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x02000153 RID: 339
	public sealed class BonusGoldReward : MonoBehaviour
	{
		// Token: 0x060006BB RID: 1723 RVA: 0x00013663 File Offset: 0x00011863
		private void Awake()
		{
			this._goldReward.onLoot += this.Drop;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001367C File Offset: 0x0001187C
		private void Drop()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			System.Random random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			int goldCalculatorCount = Singleton<DarktechManager>.Instance.setting.GetGoldCalculatorCount(random, currentChapter.type, currentChapter.stageIndex);
			for (int i = 0; i < goldCalculatorCount; i++)
			{
				CurrencyBag currencyBag = UnityEngine.Object.Instantiate<CurrencyBag>(this._pureGoldBar, base.transform.position + new Vector3(0f, 0.6f), Quaternion.identity, Map.Instance.transform);
				currencyBag.name = this._pureGoldBar.name;
				currencyBag.released = true;
				currencyBag.count = UnityEngine.Random.Range(20, 30);
				currencyBag.dropMovement.Pause();
				currencyBag.dropMovement.Move(MMMaths.RandomBool() ? UnityEngine.Random.Range(-2.5f, -0.2f) : UnityEngine.Random.Range(0.2f, 2.5f), (float)UnityEngine.Random.Range(14, 17));
			}
		}

		// Token: 0x040004ED RID: 1261
		private const int _randomSeed = 2028506624;

		// Token: 0x040004EE RID: 1262
		[SerializeField]
		private CurrencyBag _pureGoldBar;

		// Token: 0x040004EF RID: 1263
		[SerializeField]
		private GoldReward _goldReward;
	}
}
