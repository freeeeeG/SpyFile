using System;
using BehaviorDesigner.Runtime;
using Level;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E3A RID: 3642
	public class SpawnWave : CharacterOperation
	{
		// Token: 0x0600488B RID: 18571 RVA: 0x000D2FB8 File Offset: 0x000D11B8
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
						enemyWave.Spawn(this._effectOn);
						return;
					}
				}
			}
			Debug.LogError(owner.name + "의 BehaviorDesignerCommunicator의 웨이브 Key값에 해당되는 EnemyWave key : " + text + "를 찾을 수 없습니다.");
		}

		// Token: 0x040037A5 RID: 14245
		private readonly string TargetBDWaveKeyVariable = "WaveKey";

		// Token: 0x040037A6 RID: 14246
		[SerializeField]
		private bool _effectOn;
	}
}
