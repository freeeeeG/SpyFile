using System;
using System.Collections;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010BE RID: 4286
	public class HighFanaticAI : AIController
	{
		// Token: 0x06005322 RID: 21282 RVA: 0x000F949A File Offset: 0x000F769A
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x000F94C2 File Offset: 0x000F76C2
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this.Combat();
			yield break;
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x000F94D1 File Offset: 0x000F76D1
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null))
				{
					if (this._fanaticCall.CanUse() && base.FindClosestPlayerBody(this._fanaticCallTrigger))
					{
						yield return this._fanaticCall.CRun(this);
						yield return this._fanaticCallIdle.CRun(this);
					}
					else if (base.FindClosestPlayerBody(this._massSacrificeTrigger) && this._massSacrifice.CanUse(this))
					{
						yield return this._massSacrifice.CRun(this);
					}
					else if (base.FindClosestPlayerBody(this._keepDistanceTrigger) && this._keepDistance.CanUseBackMove())
					{
						yield return this._keepDistance.CRun(this);
						yield return this._keepDistanceIdle.CRun(this);
					}
					else
					{
						yield return this._chase.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x040042BE RID: 17086
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		[Header("Behaviours")]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040042BF RID: 17087
		[Subcomponent(typeof(KeepDistance))]
		[SerializeField]
		private KeepDistance _keepDistance;

		// Token: 0x040042C0 RID: 17088
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _keepDistanceIdle;

		// Token: 0x040042C1 RID: 17089
		[SerializeField]
		[Chase.SubcomponentAttribute(true)]
		private Chase _chase;

		// Token: 0x040042C2 RID: 17090
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x040042C3 RID: 17091
		[Header("Fanatic Call", order = 2)]
		[Subcomponent(typeof(FanaticCall))]
		[Space]
		[SerializeField]
		private FanaticCall _fanaticCall;

		// Token: 0x040042C4 RID: 17092
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _fanaticCallIdle;

		// Token: 0x040042C5 RID: 17093
		[Header("Mass Sacrifice", order = 3)]
		[Subcomponent(typeof(MassSacrifice))]
		[SerializeField]
		private MassSacrifice _massSacrifice;

		// Token: 0x040042C6 RID: 17094
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _massSacrificeIdle;

		// Token: 0x040042C7 RID: 17095
		[SerializeField]
		[Header("Tools")]
		private Collider2D _fanaticCallTrigger;

		// Token: 0x040042C8 RID: 17096
		[SerializeField]
		private Collider2D _massSacrificeTrigger;

		// Token: 0x040042C9 RID: 17097
		[SerializeField]
		private Collider2D _keepDistanceTrigger;
	}
}
