using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace Level
{
	// Token: 0x02000482 RID: 1154
	public sealed class CheatMap : MonoBehaviour
	{
		// Token: 0x060015FD RID: 5629 RVA: 0x00044EAD File Offset: 0x000430AD
		private void Awake()
		{
			this._chefFoods.Spawn();
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00044EBA File Offset: 0x000430BA
		public void SummonCombatTarget()
		{
			this._combat.SummonCombatTarget();
		}

		// Token: 0x0400133E RID: 4926
		[SerializeField]
		private CheatMap.ChefFoods _chefFoods;

		// Token: 0x0400133F RID: 4927
		[SerializeField]
		private CheatMap.Combat _combat;

		// Token: 0x02000483 RID: 1155
		[Serializable]
		private class ChefFoods
		{
			// Token: 0x06001600 RID: 5632 RVA: 0x00044EC8 File Offset: 0x000430C8
			public void Spawn()
			{
				int num = (this._maxCount == 0) ? int.MaxValue : this._maxCount;
				for (int i = 0; i < num; i++)
				{
					AbilityBuff abilityBuff = this._abilityBuffList.abilityBuff[i];
					AbilityBuff abilityBuff2 = UnityEngine.Object.Instantiate<AbilityBuff>(abilityBuff, this._chefFoodsDropPoint);
					abilityBuff2.name = abilityBuff.name;
					abilityBuff2.price = 0;
					abilityBuff2.Initialize();
					abilityBuff2.transform.position = new Vector2(this._chefFoodsDropPoint.position.x + this._distance * (float)i, this._chefFoodsDropPoint.position.y);
				}
			}

			// Token: 0x04001340 RID: 4928
			[SerializeField]
			private AbilityBuffList _abilityBuffList;

			// Token: 0x04001341 RID: 4929
			[SerializeField]
			private Transform _chefFoodsDropPoint;

			// Token: 0x04001342 RID: 4930
			[SerializeField]
			private float _distance;

			// Token: 0x04001343 RID: 4931
			[SerializeField]
			private int _maxCount;
		}

		// Token: 0x02000484 RID: 1156
		[Serializable]
		private class Combat
		{
			// Token: 0x06001602 RID: 5634 RVA: 0x00044F68 File Offset: 0x00043168
			public void SummonCombatTarget()
			{
				UnityEngine.Object.Instantiate<Character>(this._enemy, this._spawnPoint.position, Quaternion.identity, Map.Instance.transform);
			}

			// Token: 0x04001344 RID: 4932
			[SerializeField]
			private Character _enemy;

			// Token: 0x04001345 RID: 4933
			[SerializeField]
			private Transform _spawnPoint;
		}
	}
}
