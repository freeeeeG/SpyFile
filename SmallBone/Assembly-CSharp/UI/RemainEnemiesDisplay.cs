using System;
using Level;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x020003C9 RID: 969
	public class RemainEnemiesDisplay : MonoBehaviour
	{
		// Token: 0x06001210 RID: 4624 RVA: 0x00035378 File Offset: 0x00033578
		private void Update()
		{
			if (Map.Instance == null || Map.Instance.waveContainer == null || (Map.Instance.waveContainer.enemyWaves.Length == 0 && Map.Instance.waveContainer.summonEnemyWave.characters.Count == 0))
			{
				if (this._container.gameObject.activeSelf)
				{
					this._container.gameObject.SetActive(false);
				}
				return;
			}
			if (!this._container.gameObject.activeSelf)
			{
				this._container.gameObject.SetActive(true);
			}
			int num = 0;
			foreach (EnemyWave enemyWave in Map.Instance.waveContainer.enemyWaves)
			{
				if (enemyWave.state == Wave.State.Spawned)
				{
					num += enemyWave.characters.Count;
				}
			}
			num += Map.Instance.waveContainer.summonEnemyWave.characters.Count;
			if (this._count == num)
			{
				return;
			}
			this._count = num;
			this._amount.text = this._count.ToString();
		}

		// Token: 0x04000EE9 RID: 3817
		[SerializeField]
		private TextMeshProUGUI _amount;

		// Token: 0x04000EEA RID: 3818
		[SerializeField]
		private GameObject _container;

		// Token: 0x04000EEB RID: 3819
		private int _count;
	}
}
