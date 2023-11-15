using System;
using System.Collections;
using Data;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Chimera
{
	// Token: 0x0200124C RID: 4684
	public class ChimeraCombat : MonoBehaviour
	{
		// Token: 0x06005C48 RID: 23624 RVA: 0x0010F9F0 File Offset: 0x0010DBF0
		public IEnumerator Combat()
		{
			if (!this._speedUpState && this._chimera.character.health.percent < (double)this._speedUpHealthCondition)
			{
				this._chimera.SetAnimationSpeed(this._animationHighSpeed);
				this._speedUpState = true;
			}
			if (this._chimera.CanUseWreckDrop())
			{
				yield return this._chimera.RunPattern(Pattern.WreckDrop);
				yield return this._chimera.RunPattern(Pattern.VenomBreath);
				yield return this._chimera.RunPattern(Pattern.WreckDestroy);
			}
			else if (this._chimera.CanUseSubjectDrop())
			{
				yield return this._chimera.RunPattern(Pattern.SubjectDrop);
				yield return this._chimera.RunPattern(Pattern.SkippableIdle);
			}
			else if (this._chimera.CanUseStomp() && MMMaths.RandomBool())
			{
				if (this._speedUpState)
				{
					this._chimera.SetAnimationSpeed(1.2f);
				}
				int count = UnityEngine.Random.Range(1, 4);
				int num;
				for (int i = 0; i < count; i = num + 1)
				{
					yield return this._chimera.RunPattern(Pattern.Stomp);
					num = i;
				}
				if (this._speedUpState)
				{
					this._chimera.SetAnimationSpeed(this._animationHighSpeed);
				}
				yield return this._chimera.RunPattern(Pattern.SkippableIdle);
			}
			else if (this._chimera.CanUseBite() && MMMaths.Chance(0.1f))
			{
				yield return this._chimera.RunPattern(Pattern.Bite);
			}
			else
			{
				Pattern pattern = this._normalPatterns.TakeOne();
				if (pattern == Pattern.VenomFall && !this._chimera.CanUseVenomFall())
				{
					do
					{
						pattern = this._normalPatterns.TakeOne();
					}
					while (pattern == Pattern.VenomFall);
				}
				yield return this._chimera.RunPattern(pattern);
				if (pattern == Pattern.VenomFall)
				{
					yield return this._chimera.RunPattern(Pattern.Idle);
					if (GameData.HardmodeProgress.hardmode)
					{
						yield return this._chimera.RunPattern(Pattern.Idle);
					}
					if (this._speedUpState)
					{
						yield return this._chimera.RunPattern(Pattern.Idle);
					}
				}
				else if (pattern != Pattern.VenomBall)
				{
					yield return this._chimera.RunPattern(Pattern.SkippableIdle);
				}
			}
			yield break;
		}

		// Token: 0x04004A1F RID: 18975
		[SerializeField]
		private Chimera _chimera;

		// Token: 0x04004A20 RID: 18976
		[SerializeField]
		[Subcomponent(typeof(WeightedPattern))]
		private WeightedPattern _normalPatterns;

		// Token: 0x04004A21 RID: 18977
		[Range(0f, 100f)]
		[SerializeField]
		private float _speedUpHealthCondition = 0.6f;

		// Token: 0x04004A22 RID: 18978
		[SerializeField]
		private float _animationHighSpeed = 1.5f;

		// Token: 0x04004A23 RID: 18979
		private const float _stompHighSpeed = 1.2f;

		// Token: 0x04004A24 RID: 18980
		private bool _speedUpState;
	}
}
