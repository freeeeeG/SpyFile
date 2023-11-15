using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x02000849 RID: 2121
	public sealed class AssetManagement : UpgradeAbility
	{
		// Token: 0x06002C2D RID: 11309 RVA: 0x00087482 File Offset: 0x00085682
		public override void Attach(Character target)
		{
			this._target = target;
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += this.Settle;
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x000874A8 File Offset: 0x000856A8
		private void Settle(Map old, Map @new)
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			int amount;
			string colorValue;
			if (MMMaths.PercentChance(this._random, this._benefitChance))
			{
				amount = (int)((float)(GameData.Currency.gold.balance * this._benefitPercent) * 0.01f);
				colorValue = ColorUtility.ToHtmlStringRGB(Color.yellow);
				GameData.Currency.gold.Earn(amount);
			}
			else
			{
				amount = (int)((float)(GameData.Currency.gold.balance * this._lossPercent) * 0.01f);
				colorValue = ColorUtility.ToHtmlStringRGB(Color.gray);
				GameData.Currency.gold.Consume(amount);
			}
			Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(amount.ToString(), MMMaths.RandomPointWithinBounds(this._target.collider.bounds) + Vector2.up * 2f, colorValue);
		}

		// Token: 0x06002C2F RID: 11311 RVA: 0x000875BE File Offset: 0x000857BE
		public override void Detach()
		{
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= this.Settle;
		}

		// Token: 0x04002556 RID: 9558
		private const int _randomSeed = 2028506624;

		// Token: 0x04002557 RID: 9559
		[SerializeField]
		[Range(0f, 100f)]
		private int _benefitChance;

		// Token: 0x04002558 RID: 9560
		[SerializeField]
		[Range(0f, 100f)]
		private int _benefitPercent;

		// Token: 0x04002559 RID: 9561
		[Range(0f, 100f)]
		[SerializeField]
		private int _lossPercent;

		// Token: 0x0400255A RID: 9562
		private System.Random _random;

		// Token: 0x0400255B RID: 9563
		private Character _target;
	}
}
