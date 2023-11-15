using System;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200109B RID: 4251
	public sealed class GolemSummonMagicCircle : MonoBehaviour
	{
		// Token: 0x06005256 RID: 21078 RVA: 0x000F71BC File Offset: 0x000F53BC
		private void OnEnable()
		{
			this._leftAlchemist.character.health.onDied += this.CountDeath;
			this._rightAlchemist.character.health.onDied += this.CountDeath;
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x000F720B File Offset: 0x000F540B
		private void CountDeath()
		{
			this._deathCount++;
			if (this._deathCount >= 2)
			{
				this._effectContainer.SetActive(false);
			}
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x000F7230 File Offset: 0x000F5430
		private void OnDestroy()
		{
			if (this._leftAlchemist != null && !this._leftAlchemist.dead)
			{
				this._leftAlchemist.character.health.onDied -= this.CountDeath;
			}
			if (this._rightAlchemist != null && !this._leftAlchemist.dead)
			{
				this._rightAlchemist.character.health.onDied -= this.CountDeath;
			}
		}

		// Token: 0x04004216 RID: 16918
		[SerializeField]
		private AlchemistSummonerAI _leftAlchemist;

		// Token: 0x04004217 RID: 16919
		[SerializeField]
		private AlchemistSummonerAI _rightAlchemist;

		// Token: 0x04004218 RID: 16920
		[SerializeField]
		private GameObject _effectContainer;

		// Token: 0x04004219 RID: 16921
		private int _deathCount;
	}
}
