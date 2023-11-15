using System;
using Characters.Monsters;
using Level;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F25 RID: 3877
	public sealed class SummonDarkOrb : CharacterOperation
	{
		// Token: 0x06004B96 RID: 19350 RVA: 0x000DE7A8 File Offset: 0x000DC9A8
		public override void Run(Character owner)
		{
			if (this._spawnPositions.Length == 0)
			{
				Monster monster = this._monsterPrefab.Summon(owner.transform.position);
				this.SetHealth(monster.character, owner);
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
				this.SetHealth(monster2.character, owner);
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

		// Token: 0x06004B97 RID: 19351 RVA: 0x000DE890 File Offset: 0x000DCA90
		private void SetHealth(Character target, Character owner)
		{
			double maximumHealth = owner.health.maximumHealth;
			target.health.SetMaximumHealth(maximumHealth * (double)this._healthPercent * 0.009999999776482582);
			target.health.SetCurrentHealth(maximumHealth * (double)this._healthPercent * 0.009999999776482582);
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x000DE8E8 File Offset: 0x000DCAE8
		private void AddContainer(Monster summoned)
		{
			SummonDarkOrb.<>c__DisplayClass7_0 CS$<>8__locals1 = new SummonDarkOrb.<>c__DisplayClass7_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.summoned = summoned;
			this._container.Add(CS$<>8__locals1.summoned);
			CS$<>8__locals1.summoned.character.health.onDied += CS$<>8__locals1.<AddContainer>g__OnDied|0;
		}

		// Token: 0x04003AD8 RID: 15064
		[SerializeField]
		[Range(0f, 100f)]
		private int _healthPercent;

		// Token: 0x04003AD9 RID: 15065
		[SerializeField]
		private bool _containSummonWave;

		// Token: 0x04003ADA RID: 15066
		[Information("비어 있어도 문제 없음,", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private MonsterContainer _container;

		// Token: 0x04003ADB RID: 15067
		[SerializeField]
		private Monster _monsterPrefab;

		// Token: 0x04003ADC RID: 15068
		[Information("비워둘 경우 플레이어 위치에 1마리 소환, 그 외에는 지정된 위치마다 소환됨", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private Transform[] _spawnPositions;
	}
}
