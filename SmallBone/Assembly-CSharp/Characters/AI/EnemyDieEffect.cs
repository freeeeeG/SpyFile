using System;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010F2 RID: 4338
	public class EnemyDieEffect : CharacterDieEffect
	{
		// Token: 0x06005443 RID: 21571 RVA: 0x000FC3EE File Offset: 0x000FA5EE
		protected override void Awake()
		{
			base.Awake();
			this._character.onDie += this.OnDie;
		}

		// Token: 0x06005444 RID: 21572 RVA: 0x000FC410 File Offset: 0x000FA610
		protected void OnDie()
		{
			if (this._aiController.dead)
			{
				return;
			}
			this._character.collider.enabled = false;
			this._aiController.StopAllCoroutinesWithBehaviour();
			SequentialAction onDie = this._onDie;
			if (onDie != null)
			{
				onDie.TryStart();
			}
			this._character.onDie -= this.OnDie;
		}

		// Token: 0x040043A6 RID: 17318
		[SerializeField]
		private AIController _aiController;

		// Token: 0x040043A7 RID: 17319
		[SerializeField]
		[Subcomponent(true, typeof(SequentialAction))]
		private SequentialAction _onDie;
	}
}
