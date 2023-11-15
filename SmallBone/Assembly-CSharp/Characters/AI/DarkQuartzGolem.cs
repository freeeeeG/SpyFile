using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.DarkQuartzGolem;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001091 RID: 4241
	public sealed class DarkQuartzGolem : AIController
	{
		// Token: 0x0600521A RID: 21018 RVA: 0x000F6770 File Offset: 0x000F4970
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._melee,
				this._rush,
				this._range,
				this._targeting,
				this._idle
			};
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x000F67E8 File Offset: 0x000F49E8
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x000F6810 File Offset: 0x000F4A10
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CIntro();
			yield return this._idle.CRun(this);
			yield return this._rush.CRun(this);
			yield return this._idle.CRun(this);
			base.StartCoroutine(this.CChangeStopTrigger());
			while (!base.dead)
			{
				yield return this.Combat();
			}
			yield break;
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x000F681F File Offset: 0x000F4A1F
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				if (base.target == null)
				{
					yield return null;
				}
				else if (base.stuned)
				{
					yield return null;
				}
				else if (this._targeting.CanUse(this))
				{
					yield return this._targeting.CRun(this);
					yield return this._idle.CRun(this);
				}
				else if (this._rush.CanUse(this))
				{
					yield return this._rush.CRun(this);
					yield return this._idle.CRun(this);
				}
				else if (this._melee.CanUse(this))
				{
					if (MMMaths.RandomBool())
					{
						yield return this._range.CRun(this);
					}
					else
					{
						yield return this._melee.CRun(this);
					}
				}
				else if (this._range.CanUse(this))
				{
					yield return this._range.CRun(this);
				}
				else
				{
					yield return this._targeting.CRun(this);
					yield return this._idle.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x000F682E File Offset: 0x000F4A2E
		private IEnumerator CIntro()
		{
			this._summonAction.TryStart();
			while (this._summonAction.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x000F683D File Offset: 0x000F4A3D
		private IEnumerator CChangeStopTrigger()
		{
			while (!base.dead)
			{
				if (this._rush.CanUse(this))
				{
					this.stopTrigger = this._range.trigger;
				}
				else
				{
					this.stopTrigger = this._melee.trigger;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x040041EB RID: 16875
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040041EC RID: 16876
		[Subcomponent(typeof(Wander))]
		[SerializeField]
		private Wander _wander;

		// Token: 0x040041ED RID: 16877
		[Subcomponent(typeof(Chase))]
		[SerializeField]
		private Chase _chase;

		// Token: 0x040041EE RID: 16878
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x040041EF RID: 16879
		[Subcomponent(true, typeof(SimpleAction))]
		[SerializeField]
		private SimpleAction _summonAction;

		// Token: 0x040041F0 RID: 16880
		[SerializeField]
		[Subcomponent(typeof(Melee))]
		private Melee _melee;

		// Token: 0x040041F1 RID: 16881
		[Subcomponent(typeof(Rush))]
		[SerializeField]
		private Rush _rush;

		// Token: 0x040041F2 RID: 16882
		[Subcomponent(typeof(Range))]
		[SerializeField]
		private Range _range;

		// Token: 0x040041F3 RID: 16883
		[Subcomponent(typeof(Targeting))]
		[SerializeField]
		private Targeting _targeting;
	}
}
