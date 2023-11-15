using System;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x02001133 RID: 4403
	public class PlayAnimationOnDied : MonoBehaviour
	{
		// Token: 0x06005599 RID: 21913 RVA: 0x000FF5D3 File Offset: 0x000FD7D3
		private void Start()
		{
			this._ai.character.health.onDied += this.OnDied;
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x000FF5F8 File Offset: 0x000FD7F8
		private void OnDied()
		{
			this._ai.character.health.onDied -= this.OnDied;
			Character character = this._ai.character;
			if (character == null)
			{
				return;
			}
			this._ai.StopAllCoroutinesWithBehaviour();
			character.animationController.Play(this._onDieAnimationInfo, 1f);
		}

		// Token: 0x04004499 RID: 17561
		[SerializeField]
		private AIController _ai;

		// Token: 0x0400449A RID: 17562
		[SerializeField]
		private CharacterAnimationController.AnimationInfo _onDieAnimationInfo;
	}
}
