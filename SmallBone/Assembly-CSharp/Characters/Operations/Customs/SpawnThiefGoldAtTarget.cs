using System;
using Data;
using Level;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FED RID: 4077
	public class SpawnThiefGoldAtTarget : TargetedCharacterOperation
	{
		// Token: 0x06004EC2 RID: 20162 RVA: 0x000EC4A0 File Offset: 0x000EA6A0
		public override void Run(Character owner, Character target)
		{
			if (this._amount == 0 || this._count == 0)
			{
				return;
			}
			if (!this._characterTypeFilter[target.type])
			{
				return;
			}
			if (!MMMaths.PercentChance(this._possibility))
			{
				return;
			}
			Vector3 position = this._spawnAtOwner ? owner.transform.position : target.transform.position;
			this.SpawnGold(position);
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x000EC508 File Offset: 0x000EA708
		private void SpawnGold(Vector3 position)
		{
			int currencyAmount = this._amount / this._count;
			for (int i = 0; i < this._count; i++)
			{
				CurrencyParticle component = this._thiefGold.Spawn(position, true).GetComponent<CurrencyParticle>();
				component.currencyType = GameData.Currency.Type.Gold;
				component.currencyAmount = currencyAmount;
				if (i == 0)
				{
					component.currencyAmount += this._amount % this._count;
				}
			}
		}

		// Token: 0x04003EDE RID: 16094
		[SerializeField]
		private PoolObject _thiefGold;

		// Token: 0x04003EDF RID: 16095
		[SerializeField]
		[Range(0f, 100f)]
		private int _possibility;

		// Token: 0x04003EE0 RID: 16096
		[SerializeField]
		private int _amount;

		// Token: 0x04003EE1 RID: 16097
		[SerializeField]
		private int _count;

		// Token: 0x04003EE2 RID: 16098
		[SerializeField]
		private bool _spawnAtOwner;

		// Token: 0x04003EE3 RID: 16099
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter;
	}
}
