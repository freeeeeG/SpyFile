using System;
using System.Collections;
using Characters.Actions;
using Level;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012E2 RID: 4834
	public class Heal : Behaviour
	{
		// Token: 0x06005F9A RID: 24474 RVA: 0x001180F6 File Offset: 0x001162F6
		private void Start()
		{
			this._enemyWaveContainer = Map.Instance.waveContainer;
		}

		// Token: 0x06005F9B RID: 24475 RVA: 0x00118108 File Offset: 0x00116308
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			UnityEngine.Random.Range(this._amountRange.x, this._amountRange.y);
			this._enemyWaveContainer.GetAllEnemies().Random<Character>();
			this._healMotion.TryStart();
			yield break;
		}

		// Token: 0x04004CDA RID: 19674
		[Range(1f, 100f)]
		[SerializeField]
		private int _count;

		// Token: 0x04004CDB RID: 19675
		[SerializeField]
		private float _delay;

		// Token: 0x04004CDC RID: 19676
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2 _amountRange;

		// Token: 0x04004CDD RID: 19677
		[SerializeField]
		private Characters.Actions.Action _healMotion;

		// Token: 0x04004CDE RID: 19678
		private EnemyWaveContainer _enemyWaveContainer;
	}
}
