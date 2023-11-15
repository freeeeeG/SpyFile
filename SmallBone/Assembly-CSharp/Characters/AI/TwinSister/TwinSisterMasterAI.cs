using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.TwinSister
{
	// Token: 0x0200119F RID: 4511
	public class TwinSisterMasterAI : MonoBehaviour
	{
		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x06005899 RID: 22681 RVA: 0x001087DF File Offset: 0x001069DF
		// (set) Token: 0x0600589A RID: 22682 RVA: 0x001087E7 File Offset: 0x001069E7
		public int goldenAideDiedCount { get; private set; }

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x0600589B RID: 22683 RVA: 0x001087F0 File Offset: 0x001069F0
		// (set) Token: 0x0600589C RID: 22684 RVA: 0x001087F8 File Offset: 0x001069F8
		public bool singlePattern { get; set; }

		// Token: 0x0600589D RID: 22685 RVA: 0x00108801 File Offset: 0x00106A01
		public void RemovePlayerHitReaction()
		{
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage -= new TookDamageDelegate(this.PlayPlayerHitReaction);
		}

		// Token: 0x0600589E RID: 22686 RVA: 0x00108840 File Offset: 0x00106A40
		private void PlayPlayerHitReaction(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._attackSuccess.TryStart();
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x0010884E File Offset: 0x00106A4E
		private void PlayAideHitReaction(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._hit.TryStart();
		}

		// Token: 0x060058A0 RID: 22688 RVA: 0x0010885C File Offset: 0x00106A5C
		public IEnumerator CPlaySurpriseReaction()
		{
			yield return Chronometer.global.WaitForSeconds(4f);
			this._surprise.TryStart();
			yield break;
		}

		// Token: 0x060058A1 RID: 22689 RVA: 0x0010886B File Offset: 0x00106A6B
		public void PlayAwakenDieReaction()
		{
			this._surpriseFreeze.TryStart();
		}

		// Token: 0x060058A2 RID: 22690 RVA: 0x00108879 File Offset: 0x00106A79
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			this.RemovePlayerHitReaction();
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x0010888C File Offset: 0x00106A8C
		private void Start()
		{
			this._longHairAide.character.health.onDiedTryCatch += delegate()
			{
				this.goldenAideDiedCount++;
			};
			this._shortHairAide.character.health.onDiedTryCatch += delegate()
			{
				this.goldenAideDiedCount++;
			};
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage += new TookDamageDelegate(this.PlayPlayerHitReaction);
			this._longHairAide.character.health.onTookDamage += new TookDamageDelegate(this.PlayAideHitReaction);
			this._shortHairAide.character.health.onTookDamage += new TookDamageDelegate(this.PlayAideHitReaction);
			this._dualPatterns = new List<TwinSisterMasterAI.DualPattern>(this._twinMeteor + this._twinMeteorChain + this._twinMeteorGround + this._twinHomingPierce);
			for (int i = 0; i < this._twinMeteor; i++)
			{
				this._dualPatterns.Add(TwinSisterMasterAI.DualPattern.TwinMeteor);
			}
			for (int j = 0; j < this._twinMeteorChain; j++)
			{
				this._dualPatterns.Add(TwinSisterMasterAI.DualPattern.TwinMeteorGround);
			}
			for (int k = 0; k < this._twinMeteorGround; k++)
			{
				this._dualPatterns.Add(TwinSisterMasterAI.DualPattern.TwinMeteorChain);
			}
			for (int l = 0; l < this._twinHomingPierce; l++)
			{
				this._dualPatterns.Add(TwinSisterMasterAI.DualPattern.TwinHomingPierce);
			}
			this._meleePatterns = new List<TwinSisterMasterAI.SinglePattern>(this._meteorAirWeightMelee + this._dimensionPierceWeightMelee + this._rushWeightMelee + this._risingPierceWeightMelee + this._backStepWeightMelee);
			for (int m = 0; m < this._meteorAirWeightMelee; m++)
			{
				this._meleePatterns.Add(TwinSisterMasterAI.SinglePattern.MeteorAir);
			}
			for (int n = 0; n < this._dimensionPierceWeightMelee; n++)
			{
				this._meleePatterns.Add(TwinSisterMasterAI.SinglePattern.DimensionPierce);
			}
			for (int num = 0; num < this._rushWeightMelee; num++)
			{
				this._meleePatterns.Add(TwinSisterMasterAI.SinglePattern.Rush);
			}
			for (int num2 = 0; num2 < this._risingPierceWeightMelee; num2++)
			{
				this._meleePatterns.Add(TwinSisterMasterAI.SinglePattern.RisingPierce);
			}
			for (int num3 = 0; num3 < this._backStepWeightMelee; num3++)
			{
				this._meleePatterns.Add(TwinSisterMasterAI.SinglePattern.Backstep);
			}
			for (int num4 = 0; num4 < this._meteorGroundWeightMelee; num4++)
			{
				this._meleePatterns.Add(TwinSisterMasterAI.SinglePattern.MeteorGround);
			}
			this._rangePatterns = new List<TwinSisterMasterAI.SinglePattern>(this._meteorAirWeightRange + this._dimensionPierceWeightRange + this._dashWeightRange);
			for (int num5 = 0; num5 < this._meteorAirWeightRange; num5++)
			{
				this._rangePatterns.Add(TwinSisterMasterAI.SinglePattern.MeteorAir);
			}
			for (int num6 = 0; num6 < this._dimensionPierceWeightRange; num6++)
			{
				this._rangePatterns.Add(TwinSisterMasterAI.SinglePattern.DimensionPierce);
			}
			for (int num7 = 0; num7 < this._dashWeightRange; num7++)
			{
				this._rangePatterns.Add(TwinSisterMasterAI.SinglePattern.Dash);
			}
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x00108B51 File Offset: 0x00106D51
		public IEnumerator CIntro()
		{
			this._master.gameObject.SetActive(true);
			this._intro.TryStart();
			Vector3 source = Vector3.one * 0.6f;
			Vector3 dest = Vector3.one;
			float duration = 2.6399999f;
			for (float elapsed = 0f; elapsed < duration; elapsed += this._master.chronometer.master.deltaTime)
			{
				yield return null;
				this._master.transform.localScale = Vector3.Lerp(source, dest, elapsed / duration);
			}
			this._master.transform.localScale = dest;
			yield break;
		}

		// Token: 0x060058A5 RID: 22693 RVA: 0x00108B60 File Offset: 0x00106D60
		public IEnumerator RunIntroOut()
		{
			yield return this.OrderSisterToEscape();
			yield break;
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x00108B6F File Offset: 0x00106D6F
		public IEnumerator ProcessDualCombat()
		{
			float count = UnityEngine.Random.Range(this._dualCombatCount.x, this._dualCombatCount.y);
			TwinSisterMasterAI.DualPattern before = this._dualPatterns[this._dualPatterns.RandomIndex<TwinSisterMasterAI.DualPattern>()];
			int i = 0;
			while ((float)i < count)
			{
				TwinSisterMasterAI.DualPattern pattern;
				do
				{
					pattern = this._dualPatterns[this._dualPatterns.RandomIndex<TwinSisterMasterAI.DualPattern>()];
				}
				while (before == pattern);
				before = pattern;
				float interval = UnityEngine.Random.Range(this._delayBetweenPattern.x, this._delayBetweenPattern.y);
				float elapsed = 0f;
				while (elapsed <= interval)
				{
					if (!this._shortHairAide.character.stunedOrFreezed && !this._longHairAide.character.stunedOrFreezed)
					{
						elapsed += Chronometer.global.deltaTime;
					}
					yield return null;
				}
				if (this.goldenAideDiedCount > 0)
				{
					yield break;
				}
				this.lockForAwakening = true;
				yield return this.RunDualPattern(pattern);
				this.lockForAwakening = false;
				if (this.goldenAideDiedCount > 0)
				{
					yield break;
				}
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x060058A7 RID: 22695 RVA: 0x00108B7E File Offset: 0x00106D7E
		public IEnumerator ProcessSingleCombat(GoldenAideAI fieldAide, GoldenAideAI behindAide)
		{
			this._fieldAide = fieldAide;
			this._behindAide = behindAide;
			if (fieldAide.dead || behindAide.dead)
			{
				yield break;
			}
			behindAide.character.collider.enabled = false;
			behindAide.character.cinematic.Attach(this);
			CoroutineReference predelayCoroutine = this.StartCoroutineWithReference(this._fieldAide.CStartSinglePhasePreDelay());
			yield return this.OrderToGoldmaneMeteor();
			while (this.singlePattern)
			{
				if (fieldAide.dead || behindAide.dead)
				{
					yield break;
				}
				if (fieldAide.CanUseRisingPierce())
				{
					yield return this.RunSinglePattern(TwinSisterMasterAI.SinglePattern.RisingPierce);
					if (!this.singlePattern)
					{
						break;
					}
					yield return this.RunSinglePattern(TwinSisterMasterAI.SinglePattern.Idle);
				}
				else if (fieldAide.CanUseDimensionPierce())
				{
					yield return this.RunSinglePattern(TwinSisterMasterAI.SinglePattern.DimensionPierce);
					if (!this.singlePattern)
					{
						break;
					}
					yield return this.RunSinglePattern(TwinSisterMasterAI.SinglePattern.SkippableIdle);
				}
				else
				{
					TwinSisterMasterAI.SinglePattern pattern;
					if (this.IsMeleeCombat())
					{
						pattern = this._meleePatterns.Random<TwinSisterMasterAI.SinglePattern>();
					}
					else
					{
						pattern = this._rangePatterns.Random<TwinSisterMasterAI.SinglePattern>();
					}
					yield return this.RunSinglePattern(pattern);
					if (!this.singlePattern)
					{
						break;
					}
					if (pattern == TwinSisterMasterAI.SinglePattern.Dash)
					{
						if (MMMaths.Chance(0.3))
						{
							pattern = TwinSisterMasterAI.SinglePattern.Rush;
						}
						else
						{
							pattern = TwinSisterMasterAI.SinglePattern.MeteorGround;
						}
					}
					else if (pattern == TwinSisterMasterAI.SinglePattern.Backstep)
					{
						if (MMMaths.Chance(0.6))
						{
							pattern = TwinSisterMasterAI.SinglePattern.HomingPierce;
						}
						else
						{
							pattern = TwinSisterMasterAI.SinglePattern.SkippableIdle;
						}
					}
					else if (pattern == TwinSisterMasterAI.SinglePattern.MeteorAir || pattern == TwinSisterMasterAI.SinglePattern.MeteorGround || pattern == TwinSisterMasterAI.SinglePattern.HomingPierce)
					{
						pattern = TwinSisterMasterAI.SinglePattern.SkippableIdle;
					}
					else if (pattern == TwinSisterMasterAI.SinglePattern.Rush)
					{
						pattern = TwinSisterMasterAI.SinglePattern.Idle;
					}
					yield return this.RunSinglePattern(pattern);
					if (!this.singlePattern)
					{
						break;
					}
					if (pattern == TwinSisterMasterAI.SinglePattern.MeteorAir || pattern == TwinSisterMasterAI.SinglePattern.MeteorGround || pattern == TwinSisterMasterAI.SinglePattern.HomingPierce)
					{
						yield return this.RunSinglePattern(TwinSisterMasterAI.SinglePattern.SkippableIdle);
					}
					else if (pattern == TwinSisterMasterAI.SinglePattern.Rush)
					{
						yield return this.RunSinglePattern(TwinSisterMasterAI.SinglePattern.Idle);
					}
					if (!this.singlePattern)
					{
						break;
					}
				}
			}
			behindAide.character.collider.enabled = true;
			behindAide.character.cinematic.Detach(this);
			predelayCoroutine.Stop();
			yield return this.OrderToEscape(this._fieldAide);
			yield break;
		}

		// Token: 0x060058A8 RID: 22696 RVA: 0x00108B9B File Offset: 0x00106D9B
		private bool IsMeleeCombat()
		{
			return this._fieldAide.IsMeleeCombat();
		}

		// Token: 0x060058A9 RID: 22697 RVA: 0x00108BA8 File Offset: 0x00106DA8
		private IEnumerator RunSinglePattern(TwinSisterMasterAI.SinglePattern pattern)
		{
			switch (pattern)
			{
			case TwinSisterMasterAI.SinglePattern.MeteorAir:
				yield return this.OrderToMeteorInAir();
				break;
			case TwinSisterMasterAI.SinglePattern.DimensionPierce:
				yield return this.OrderToDimensionPierce();
				break;
			case TwinSisterMasterAI.SinglePattern.Rush:
				yield return this.OrderToRush();
				break;
			case TwinSisterMasterAI.SinglePattern.RisingPierce:
				yield return this.OrderToRisingPierce();
				break;
			case TwinSisterMasterAI.SinglePattern.Backstep:
				yield return this.OrderBackStep();
				break;
			case TwinSisterMasterAI.SinglePattern.MeteorGround:
				yield return this.OrderToMeteorInGround();
				break;
			case TwinSisterMasterAI.SinglePattern.HomingPierce:
				yield return this.OrderToHoming();
				break;
			case TwinSisterMasterAI.SinglePattern.Dash:
				yield return this.OrderToDash();
				break;
			case TwinSisterMasterAI.SinglePattern.Idle:
				yield return this.OrderToIdle();
				break;
			case TwinSisterMasterAI.SinglePattern.SkippableIdle:
				yield return this.OrderToSkippableIdle();
				break;
			}
			yield break;
		}

		// Token: 0x060058AA RID: 22698 RVA: 0x00108BBE File Offset: 0x00106DBE
		private IEnumerator RunDualPattern(TwinSisterMasterAI.DualPattern pattern)
		{
			switch (pattern)
			{
			case TwinSisterMasterAI.DualPattern.TwinMeteor:
				yield return this.OrderTwinMeteor();
				yield return this.OrderSisterToEscape();
				break;
			case TwinSisterMasterAI.DualPattern.TwinMeteorGround:
				yield return this.OrderTwinMeteorGround();
				break;
			case TwinSisterMasterAI.DualPattern.TwinMeteorChain:
				yield return this.OrderTwinMeteorChain();
				break;
			case TwinSisterMasterAI.DualPattern.TwinHomingPierce:
				yield return this.OrderTwinHomingPierce();
				break;
			}
			yield break;
		}

		// Token: 0x060058AB RID: 22699 RVA: 0x00108BD4 File Offset: 0x00106DD4
		private List<TwinSisterMasterAI.SinglePattern> GetSinglePatterns(List<TwinSisterMasterAI.SinglePattern> singlePatterns)
		{
			List<TwinSisterMasterAI.SinglePattern> list = new List<TwinSisterMasterAI.SinglePattern>(this._resetPoint);
			for (int i = 0; i < this._resetPoint; i++)
			{
				int index = singlePatterns.RandomIndex<TwinSisterMasterAI.SinglePattern>();
				list.Add(singlePatterns[index]);
				singlePatterns.Remove(singlePatterns[index]);
			}
			singlePatterns.AddRange(list);
			return list;
		}

		// Token: 0x060058AC RID: 22700 RVA: 0x00108C28 File Offset: 0x00106E28
		private IEnumerator OrderBackStep()
		{
			yield return this._fieldAide.CastBackstep();
			yield break;
		}

		// Token: 0x060058AD RID: 22701 RVA: 0x00108C37 File Offset: 0x00106E37
		private IEnumerator OrderTwinMeteor()
		{
			bool flag = MMMaths.RandomBool();
			bool flag2 = MMMaths.RandomBool();
			if (flag)
			{
				yield return this.DoDualBehaviour(this._longHairAide.CastTwinMeteor(flag2), this._shortHairAide.CastPredictTwinMeteor(!flag2), 0f);
			}
			else
			{
				yield return this.DoDualBehaviour(this._longHairAide.CastPredictTwinMeteor(flag2), this._shortHairAide.CastTwinMeteor(!flag2), 0f);
			}
			yield break;
		}

		// Token: 0x060058AE RID: 22702 RVA: 0x00108C46 File Offset: 0x00106E46
		private IEnumerator OrderTwinMeteorGround()
		{
			bool flag = MMMaths.RandomBool();
			yield return this.DoDualBehaviour(this._longHairAide.CastTwinMeteorGround(flag), this._shortHairAide.CastTwinMeteorGround(!flag), 0f);
			yield break;
		}

		// Token: 0x060058AF RID: 22703 RVA: 0x00108C55 File Offset: 0x00106E55
		private IEnumerator OrderTwinMeteorChain()
		{
			int count = UnityEngine.Random.Range(this._twinMeteorChainCount.x, this._twinMeteorChainCount.y);
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				bool flag = MMMaths.RandomBool();
				bool flag2 = MMMaths.RandomBool();
				float term = UnityEngine.Random.Range(this._twinMeteorChainTerm.x, this._twinMeteorChainTerm.y);
				if (flag)
				{
					yield return this.DoDualBehaviour(this._longHairAide.CastTwinMeteorChain(flag2, MMMaths.RandomBool()), this._shortHairAide.CastTwinMeteorChain(!flag2, MMMaths.RandomBool()), term);
				}
				else
				{
					yield return this.DoDualBehaviour(this._shortHairAide.CastTwinMeteorChain(flag2, MMMaths.RandomBool()), this._longHairAide.CastTwinMeteorChain(!flag2, MMMaths.RandomBool()), term);
				}
				if (this.goldenAideDiedCount > 0)
				{
					yield break;
				}
				if (this.singlePattern)
				{
					yield break;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060058B0 RID: 22704 RVA: 0x00108C64 File Offset: 0x00106E64
		private IEnumerator OrderTwinHomingPierce()
		{
			bool flag = MMMaths.RandomBool();
			yield return this.DoDualBehaviour(this._longHairAide.CastTwinMeteorPierce(flag), this._shortHairAide.CastTwinMeteorPierce(!flag), 0f);
			yield break;
		}

		// Token: 0x060058B1 RID: 22705 RVA: 0x00108C73 File Offset: 0x00106E73
		private IEnumerator DoDualBehaviour(IEnumerator behaviour1, IEnumerator behaviour2, float term = 0f)
		{
			TwinSisterMasterAI.<>c__DisplayClass66_0 CS$<>8__locals1 = new TwinSisterMasterAI.<>c__DisplayClass66_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.success = 0;
			base.StartCoroutine(CS$<>8__locals1.<DoDualBehaviour>g__StartBehaviour|0(behaviour1));
			if (term > 0f)
			{
				yield return Chronometer.global.WaitForSeconds(term);
			}
			base.StartCoroutine(CS$<>8__locals1.<DoDualBehaviour>g__StartBehaviour|0(behaviour2));
			while (CS$<>8__locals1.success < 2)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060058B2 RID: 22706 RVA: 0x00108C97 File Offset: 0x00106E97
		private IEnumerator OrderToGoldmaneMeteor()
		{
			yield return this._fieldAide.CastGoldenMeteor();
			yield break;
		}

		// Token: 0x060058B3 RID: 22707 RVA: 0x00108CA6 File Offset: 0x00106EA6
		private IEnumerator OrderToMeteorInAir()
		{
			yield return this._fieldAide.CastMeteorInAir();
			yield break;
		}

		// Token: 0x060058B4 RID: 22708 RVA: 0x00108CB5 File Offset: 0x00106EB5
		private IEnumerator OrderToMeteorInGround()
		{
			yield return this._fieldAide.CastMeteorInGround();
			yield break;
		}

		// Token: 0x060058B5 RID: 22709 RVA: 0x00108CC4 File Offset: 0x00106EC4
		private IEnumerator OrderToMeteorInGround2()
		{
			yield return this._fieldAide.CastMeteorInGround2();
			yield break;
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x00108CD3 File Offset: 0x00106ED3
		private IEnumerator OrderToRush()
		{
			yield return this._fieldAide.CastRush();
			yield break;
		}

		// Token: 0x060058B7 RID: 22711 RVA: 0x00108CE2 File Offset: 0x00106EE2
		private IEnumerator OrderToHoming()
		{
			yield return this._fieldAide.CastRangeAttackHoming(false);
			yield break;
		}

		// Token: 0x060058B8 RID: 22712 RVA: 0x00108CF1 File Offset: 0x00106EF1
		private IEnumerator OrderToDash()
		{
			yield return this._fieldAide.CastDash(0f);
			yield break;
		}

		// Token: 0x060058B9 RID: 22713 RVA: 0x00108D00 File Offset: 0x00106F00
		private IEnumerator OrderToDimensionPierce()
		{
			yield return this._fieldAide.CastDimensionPierce();
			yield break;
		}

		// Token: 0x060058BA RID: 22714 RVA: 0x00108D0F File Offset: 0x00106F0F
		private IEnumerator OrderToRisingPierce()
		{
			yield return this._fieldAide.CastRisingPierce();
			yield break;
		}

		// Token: 0x060058BB RID: 22715 RVA: 0x00108D1E File Offset: 0x00106F1E
		private IEnumerator OrderToIdle()
		{
			yield return this._fieldAide.CastIdle();
			yield break;
		}

		// Token: 0x060058BC RID: 22716 RVA: 0x00108D2D File Offset: 0x00106F2D
		private IEnumerator OrderToSkippableIdle()
		{
			yield return this._fieldAide.CastSkippableIdle();
			yield break;
		}

		// Token: 0x060058BD RID: 22717 RVA: 0x00108D3C File Offset: 0x00106F3C
		private IEnumerator OrderToEscape(GoldenAideAI goldenAide)
		{
			yield return goldenAide.EscapeForTwinMeteor();
			yield break;
		}

		// Token: 0x060058BE RID: 22718 RVA: 0x00108D4B File Offset: 0x00106F4B
		private IEnumerator OrderSisterToEscape()
		{
			yield return this.DoDualBehaviour(this._longHairAide.EscapeForTwinMeteor(), this._shortHairAide.EscapeForTwinMeteor(), 0f);
			yield break;
		}

		// Token: 0x060058BF RID: 22719 RVA: 0x00108D5A File Offset: 0x00106F5A
		public IEnumerator COutro()
		{
			this._outro.TryStart();
			Vector3 source = Vector3.one;
			Vector3 dest = Vector3.one * 0.6f;
			float duration = 2.6399999f;
			for (float elapsed = 0f; elapsed < duration; elapsed += this._master.chronometer.master.deltaTime)
			{
				yield return null;
				this._master.transform.localScale = Vector3.Lerp(source, dest, elapsed / duration);
			}
			this._master.transform.localScale = dest;
			while (this._outro.running)
			{
				yield return null;
			}
			this._master.gameObject.SetActive(false);
			yield return Chronometer.global.WaitForSeconds(1f);
			yield break;
		}

		// Token: 0x04004797 RID: 18327
		[SerializeField]
		private Character _master;

		// Token: 0x04004798 RID: 18328
		[Header("Intro")]
		[SerializeField]
		private Characters.Actions.Action _intro;

		// Token: 0x04004799 RID: 18329
		[SerializeField]
		[Space]
		[Header("InGame")]
		private Characters.Actions.Action _attackSuccess;

		// Token: 0x0400479A RID: 18330
		[SerializeField]
		private Characters.Actions.Action _hit;

		// Token: 0x0400479B RID: 18331
		[SerializeField]
		private Characters.Actions.Action _surprise;

		// Token: 0x0400479C RID: 18332
		[SerializeField]
		private Characters.Actions.Action _surpriseFreeze;

		// Token: 0x0400479D RID: 18333
		[SerializeField]
		private GoldenAideAI _longHairAide;

		// Token: 0x0400479E RID: 18334
		[SerializeField]
		private GoldenAideAI _shortHairAide;

		// Token: 0x0400479F RID: 18335
		[MinMaxSlider(0f, 10f)]
		[SerializeField]
		private Vector2 _delayBetweenPattern;

		// Token: 0x040047A0 RID: 18336
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2 _dualCombatCount;

		// Token: 0x040047A1 RID: 18337
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2Int _twinMeteorChainCount;

		// Token: 0x040047A2 RID: 18338
		[MinMaxSlider(0f, 10f)]
		[SerializeField]
		private Vector2 _twinMeteorChainTerm;

		// Token: 0x040047A3 RID: 18339
		[SerializeField]
		[Header("Dual Pattern Weight")]
		[Space]
		[Range(0f, 10f)]
		private int _twinMeteor;

		// Token: 0x040047A4 RID: 18340
		[SerializeField]
		[Range(0f, 10f)]
		private int _twinMeteorChain;

		// Token: 0x040047A5 RID: 18341
		[SerializeField]
		[Range(0f, 10f)]
		private int _twinMeteorGround;

		// Token: 0x040047A6 RID: 18342
		[SerializeField]
		[Range(0f, 10f)]
		private int _twinHomingPierce;

		// Token: 0x040047A7 RID: 18343
		[Header("Single Pattern Weight")]
		[Space]
		[SerializeField]
		[Range(1f, 10f)]
		private int _resetPoint = 4;

		// Token: 0x040047A8 RID: 18344
		[Header("Melee")]
		[Space]
		[Range(0f, 10f)]
		[SerializeField]
		private int _meteorAirWeightMelee;

		// Token: 0x040047A9 RID: 18345
		[Range(0f, 10f)]
		[SerializeField]
		private int _dimensionPierceWeightMelee;

		// Token: 0x040047AA RID: 18346
		[Range(0f, 10f)]
		[SerializeField]
		private int _rushWeightMelee = 3;

		// Token: 0x040047AB RID: 18347
		[Range(0f, 10f)]
		[SerializeField]
		private int _risingPierceWeightMelee;

		// Token: 0x040047AC RID: 18348
		[Range(0f, 10f)]
		[SerializeField]
		private int _backStepWeightMelee = 3;

		// Token: 0x040047AD RID: 18349
		[Range(0f, 10f)]
		[SerializeField]
		private int _meteorGroundWeightMelee = 4;

		// Token: 0x040047AE RID: 18350
		[SerializeField]
		[Header("Range")]
		[Space]
		[Range(0f, 10f)]
		private int _meteorAirWeightRange = 5;

		// Token: 0x040047AF RID: 18351
		[Range(0f, 10f)]
		[SerializeField]
		private int _dimensionPierceWeightRange;

		// Token: 0x040047B0 RID: 18352
		[SerializeField]
		[Range(0f, 10f)]
		private int _dashWeightRange = 5;

		// Token: 0x040047B1 RID: 18353
		private GoldenAideAI _fieldAide;

		// Token: 0x040047B2 RID: 18354
		private GoldenAideAI _behindAide;

		// Token: 0x040047B4 RID: 18356
		private bool _aliveAlone;

		// Token: 0x040047B5 RID: 18357
		public bool lockForAwakening;

		// Token: 0x040047B6 RID: 18358
		private bool _lockForEasterEgg;

		// Token: 0x040047B8 RID: 18360
		private CoroutineReference _dualPhase;

		// Token: 0x040047B9 RID: 18361
		[SerializeField]
		[Header("Outro")]
		[Space]
		private Characters.Actions.Action _outro;

		// Token: 0x040047BA RID: 18362
		private List<TwinSisterMasterAI.DualPattern> _dualPatterns;

		// Token: 0x040047BB RID: 18363
		private List<TwinSisterMasterAI.SinglePattern> _meleePatterns;

		// Token: 0x040047BC RID: 18364
		private List<TwinSisterMasterAI.SinglePattern> _rangePatterns;

		// Token: 0x020011A0 RID: 4512
		private enum DualPattern
		{
			// Token: 0x040047BE RID: 18366
			TwinMeteor,
			// Token: 0x040047BF RID: 18367
			TwinMeteorGround,
			// Token: 0x040047C0 RID: 18368
			TwinMeteorChain,
			// Token: 0x040047C1 RID: 18369
			TwinHomingPierce
		}

		// Token: 0x020011A1 RID: 4513
		private enum SinglePattern
		{
			// Token: 0x040047C3 RID: 18371
			MeteorAir,
			// Token: 0x040047C4 RID: 18372
			DimensionPierce,
			// Token: 0x040047C5 RID: 18373
			Rush,
			// Token: 0x040047C6 RID: 18374
			RisingPierce,
			// Token: 0x040047C7 RID: 18375
			Backstep,
			// Token: 0x040047C8 RID: 18376
			MeteorGround,
			// Token: 0x040047C9 RID: 18377
			HomingPierce,
			// Token: 0x040047CA RID: 18378
			Dash,
			// Token: 0x040047CB RID: 18379
			Idle,
			// Token: 0x040047CC RID: 18380
			SkippableIdle
		}
	}
}
