using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Chimera
{
	// Token: 0x0200123C RID: 4668
	public class ChimeraAnimation : MonoBehaviour
	{
		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06005BE2 RID: 23522 RVA: 0x0010F287 File Offset: 0x0010D487
		// (set) Token: 0x06005BE3 RID: 23523 RVA: 0x0010F28F File Offset: 0x0010D48F
		public float speed { get; set; } = 1f;

		// Token: 0x06005BE4 RID: 23524 RVA: 0x0010F298 File Offset: 0x0010D498
		private IEnumerator PlayAndWaitAnimation(CharacterAnimationController.AnimationInfo animationInfo, float extraLength = 0f)
		{
			this._character.animationController.Play(animationInfo, this.speed);
			yield return this._character.chronometer.animation.WaitForSeconds(animationInfo.dictionary["Body"].length / this.speed + extraLength);
			yield break;
		}

		// Token: 0x06005BE5 RID: 23525 RVA: 0x0010F2B5 File Offset: 0x0010D4B5
		public IEnumerator PlaySleepAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.sleep, 0f);
			yield break;
		}

		// Token: 0x06005BE6 RID: 23526 RVA: 0x0010F2C4 File Offset: 0x0010D4C4
		public IEnumerator PlayBiteAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.bite, 0f);
			yield break;
		}

		// Token: 0x06005BE7 RID: 23527 RVA: 0x0010F2D3 File Offset: 0x0010D4D3
		public IEnumerator PlayStompAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.stomp, 0f);
			yield break;
		}

		// Token: 0x06005BE8 RID: 23528 RVA: 0x0010F2E2 File Offset: 0x0010D4E2
		public IEnumerator PlayVenomFallAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.venomFall, 0f);
			yield break;
		}

		// Token: 0x06005BE9 RID: 23529 RVA: 0x0010F2F1 File Offset: 0x0010D4F1
		public IEnumerator PlayVenomBallAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.venomBall, 0f);
			yield break;
		}

		// Token: 0x06005BEA RID: 23530 RVA: 0x0010F300 File Offset: 0x0010D500
		public IEnumerator PlayVenomCannonAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.venomCannon, 0f);
			yield break;
		}

		// Token: 0x06005BEB RID: 23531 RVA: 0x0010F30F File Offset: 0x0010D50F
		public IEnumerator PlaySubjectDropAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.subjectDrop, 0f);
			yield break;
		}

		// Token: 0x06005BEC RID: 23532 RVA: 0x0010F31E File Offset: 0x0010D51E
		public IEnumerator PlayWreckDropAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.wreckDrop, 0.1f);
			yield break;
		}

		// Token: 0x06005BED RID: 23533 RVA: 0x0010F32D File Offset: 0x0010D52D
		public IEnumerator PlayWreckDestroyAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.bigstomp, 0f);
			yield break;
		}

		// Token: 0x06005BEE RID: 23534 RVA: 0x0010F33C File Offset: 0x0010D53C
		public IEnumerator PlayVenomBreathAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.venomBreath, 0f);
			yield break;
		}

		// Token: 0x06005BEF RID: 23535 RVA: 0x0010F34B File Offset: 0x0010D54B
		public IEnumerator PlayIdleAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.idle, 0f);
			yield break;
		}

		// Token: 0x06005BF0 RID: 23536 RVA: 0x0010F35A File Offset: 0x0010D55A
		public IEnumerator PlayIntroAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.intro, 0f);
			yield break;
		}

		// Token: 0x06005BF1 RID: 23537 RVA: 0x0010F369 File Offset: 0x0010D569
		public IEnumerator PlayDieAnimation()
		{
			yield return this.PlayAndWaitAnimation(this._phase1.die, 0f);
			yield break;
		}

		// Token: 0x040049E3 RID: 18915
		[SerializeField]
		private Character _character;

		// Token: 0x040049E4 RID: 18916
		[SerializeField]
		private ChimeraAnimation.Phase1 _phase1;

		// Token: 0x0200123D RID: 4669
		[Serializable]
		private class Phase1
		{
			// Token: 0x040049E6 RID: 18918
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo sleep;

			// Token: 0x040049E7 RID: 18919
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo bite;

			// Token: 0x040049E8 RID: 18920
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo stomp;

			// Token: 0x040049E9 RID: 18921
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo bigstomp;

			// Token: 0x040049EA RID: 18922
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo venomFall;

			// Token: 0x040049EB RID: 18923
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo venomBall;

			// Token: 0x040049EC RID: 18924
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo venomCannon;

			// Token: 0x040049ED RID: 18925
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo subjectDrop;

			// Token: 0x040049EE RID: 18926
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo wreckDrop;

			// Token: 0x040049EF RID: 18927
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo venomBreath;

			// Token: 0x040049F0 RID: 18928
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo idle;

			// Token: 0x040049F1 RID: 18929
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo intro;

			// Token: 0x040049F2 RID: 18930
			[SerializeField]
			internal CharacterAnimationController.AnimationInfo die;
		}
	}
}
