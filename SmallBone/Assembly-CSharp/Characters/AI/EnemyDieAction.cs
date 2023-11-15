using System;
using Characters.Actions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.AI
{
	// Token: 0x020010F1 RID: 4337
	public class EnemyDieAction : MonoBehaviour
	{
		// Token: 0x06005440 RID: 21568 RVA: 0x000FC345 File Offset: 0x000FA545
		private void Awake()
		{
			this._aiController.character.health.onDie += this.OnDie;
		}

		// Token: 0x06005441 RID: 21569 RVA: 0x000FC368 File Offset: 0x000FA568
		private void OnDie()
		{
			if (this._aiController.dead)
			{
				return;
			}
			this._aiController.StopAllCoroutinesWithBehaviour();
			SequentialAction onDie = this._onDie;
			if (onDie != null)
			{
				onDie.TryStart();
			}
			if (this._deathRattleCount > 0)
			{
				this._deathRattleCount--;
				this._aiController.character.health.ResetToMaximumHealth();
				return;
			}
			this._aiController.character.health.onDie -= this.OnDie;
		}

		// Token: 0x040043A2 RID: 17314
		[SerializeField]
		private AIController _aiController;

		// Token: 0x040043A3 RID: 17315
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040043A4 RID: 17316
		[SerializeField]
		[FormerlySerializedAs("_onDied")]
		[Subcomponent(true, typeof(SequentialAction))]
		private SequentialAction _onDie;

		// Token: 0x040043A5 RID: 17317
		[SerializeField]
		private int _deathRattleCount;
	}
}
