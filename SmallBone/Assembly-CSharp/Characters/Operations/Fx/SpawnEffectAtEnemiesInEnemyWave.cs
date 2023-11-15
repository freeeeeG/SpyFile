using System;
using BehaviorDesigner.Runtime;
using FX;
using Level;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F09 RID: 3849
	public class SpawnEffectAtEnemiesInEnemyWave : CharacterOperation
	{
		// Token: 0x06004B38 RID: 19256 RVA: 0x000DD668 File Offset: 0x000DB868
		public override void Run(Character owner)
		{
			EnemyWaveContainer waveContainer = Map.Instance.waveContainer;
			string text = (string)owner.GetComponent<BehaviorDesignerCommunicator>().GetVariable(this.TargetBDWaveKeyVariable).GetValue();
			foreach (EnemyWave enemyWave in waveContainer.enemyWaves)
			{
				foreach (string b in enemyWave.keys)
				{
					if (text == b)
					{
						foreach (Character character in enemyWave.characters)
						{
							this._info.Spawn(character.transform.position, 0f, 1f);
						}
						return;
					}
				}
			}
			Debug.LogError(owner.name + "의 BehaviorDesignerCommunicator의 웨이브 Key값에 해당되는 EnemyWave key : " + text + "를 찾을 수 없습니다.");
		}

		// Token: 0x06004B39 RID: 19257 RVA: 0x000DD764 File Offset: 0x000DB964
		public override void Stop()
		{
			this._info.DespawnChildren();
		}

		// Token: 0x04003A61 RID: 14945
		private readonly string TargetBDWaveKeyVariable = "WaveKey";

		// Token: 0x04003A62 RID: 14946
		[SerializeField]
		private EffectInfo _info;
	}
}
