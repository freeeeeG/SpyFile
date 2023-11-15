using System;
using Characters.Monsters;
using Level;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F3E RID: 3902
	public class SummonMonster : CharacterOperation
	{
		// Token: 0x06004BEA RID: 19434 RVA: 0x000DF780 File Offset: 0x000DD980
		public override void Run(Character owner)
		{
			if (this._spawnPositions.Length == 0)
			{
				Monster monster = this._monsterPrefab.Summon(owner.transform.position);
				if (this._container != null)
				{
					this.AddContainer(monster);
				}
				if (this._containSummonWave)
				{
					Map.Instance.waveContainer.summonWave.Attach(monster.character);
				}
				return;
			}
			foreach (Transform transform in this._spawnPositions)
			{
				Monster monster2 = this._monsterPrefab.Summon(transform.position);
				if (this._container != null)
				{
					this.AddContainer(monster2);
				}
				if (this._containSummonWave)
				{
					Map.Instance.waveContainer.summonWave.Attach(monster2.character);
				}
			}
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x000DF84C File Offset: 0x000DDA4C
		private void AddContainer(Monster summoned)
		{
			SummonMonster.<>c__DisplayClass5_0 CS$<>8__locals1 = new SummonMonster.<>c__DisplayClass5_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.summoned = summoned;
			this._container.Add(CS$<>8__locals1.summoned);
			CS$<>8__locals1.summoned.character.health.onDied += CS$<>8__locals1.<AddContainer>g__OnDied|0;
		}

		// Token: 0x04003B20 RID: 15136
		[SerializeField]
		private bool _containSummonWave;

		// Token: 0x04003B21 RID: 15137
		[Information("비어 있어도 문제 없음,", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private MonsterContainer _container;

		// Token: 0x04003B22 RID: 15138
		[SerializeField]
		private Monster _monsterPrefab;

		// Token: 0x04003B23 RID: 15139
		[SerializeField]
		[Information("비워둘 경우 플레이어 위치에 1마리 소환, 그 외에는 지정된 위치마다 소환됨", InformationAttribute.InformationType.Info, false)]
		private Transform[] _spawnPositions;
	}
}
