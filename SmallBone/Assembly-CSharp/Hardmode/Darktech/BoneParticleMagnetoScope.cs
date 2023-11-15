using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x02000152 RID: 338
	public sealed class BoneParticleMagnetoScope : MonoBehaviour
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x00013592 File Offset: 0x00011792
		private void Awake()
		{
			this._grave.onLoot += this.Drop;
			this._multiplier = Singleton<DarktechManager>.Instance.setting.뼈입자검출기변량;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000135C0 File Offset: 0x000117C0
		private void Drop()
		{
			Rarity rarity = this._grave.rarity;
			int num = (int)((float)Singleton<Service>.Instance.levelManager.currentChapter.currentStage.boneRangeByRarity.Evaluate(rarity) * this._multiplier);
			int num2 = UnityEngine.Random.Range(15, 17);
			int currencyAmount = num / num2;
			for (int i = 0; i < num2; i++)
			{
				CurrencyParticle component = this._particle.Spawn(base.transform.position, true).GetComponent<CurrencyParticle>();
				component.currencyType = GameData.Currency.Type.Bone;
				component.currencyAmount = currencyAmount;
				if (i == 0)
				{
					component.currencyAmount += num % num2;
				}
			}
		}

		// Token: 0x040004EA RID: 1258
		[SerializeField]
		private Grave _grave;

		// Token: 0x040004EB RID: 1259
		[SerializeField]
		private PoolObject _particle;

		// Token: 0x040004EC RID: 1260
		private float _multiplier;
	}
}
