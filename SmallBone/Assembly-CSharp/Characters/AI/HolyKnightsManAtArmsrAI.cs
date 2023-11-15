using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010C8 RID: 4296
	public sealed class HolyKnightsManAtArmsrAI : AIController
	{
		// Token: 0x06005361 RID: 21345 RVA: 0x000FA08C File Offset: 0x000F828C
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._attack,
				this._tackle,
				this._holyWord
			};
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x000FA0EC File Offset: 0x000F82EC
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this.CUpdateStopTrigger());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x000FA121 File Offset: 0x000F8321
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x000FA130 File Offset: 0x000F8330
		private IEnumerator CCombat()
		{
			yield return this._wander.CRun(this);
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && !base.stuned)
				{
					if (this._holyWord.CanUse() && this._buffStack < this._maxBuffStack)
					{
						yield return this._holyWord.CRun(this);
						if (this._holyWord.result == Characters.AI.Behaviours.Behaviour.Result.Success && this._buffStack >= 0)
						{
							this.character.stat.AttachValues(this._HolyWordBuff);
						}
						this._buffStack++;
					}
					if (base.FindClosestPlayerBody(this._tackleTrigger) != null && this._tackle.CanUse())
					{
						yield return this._tackle.CRun(this);
						yield return this._attack.CRun(this);
					}
					else if (base.FindClosestPlayerBody(this._attackTrigger) != null)
					{
						yield return this._attack.CRun(this);
					}
					else
					{
						yield return this._chase.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x000FA13F File Offset: 0x000F833F
		private IEnumerator CUpdateStopTrigger()
		{
			while (!base.dead)
			{
				yield return null;
				if (this._tackle.CanUse())
				{
					this.stopTrigger = this._tackleTrigger;
				}
				else
				{
					this.stopTrigger = this._attackTrigger;
				}
			}
			yield break;
		}

		// Token: 0x040042FA RID: 17146
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		[Header("Behaviours")]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040042FB RID: 17147
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x040042FC RID: 17148
		[Subcomponent(typeof(Chase))]
		[SerializeField]
		private Chase _chase;

		// Token: 0x040042FD RID: 17149
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x040042FE RID: 17150
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _tackle;

		// Token: 0x040042FF RID: 17151
		[SerializeField]
		[Subcomponent(typeof(ContinuousTackle))]
		private ContinuousTackle _trippleTackle;

		// Token: 0x04004300 RID: 17152
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _holyWord;

		// Token: 0x04004301 RID: 17153
		[Space]
		[Header("Holy Word Buff")]
		[SerializeField]
		private Stat.Values _HolyWordBuff;

		// Token: 0x04004302 RID: 17154
		[SerializeField]
		private int _maxBuffStack;

		// Token: 0x04004303 RID: 17155
		[SerializeField]
		[Space]
		[Header("Tools")]
		private Collider2D _attackTrigger;

		// Token: 0x04004304 RID: 17156
		[SerializeField]
		private Collider2D _tackleTrigger;

		// Token: 0x04004305 RID: 17157
		private int _buffStack = -1;
	}
}
