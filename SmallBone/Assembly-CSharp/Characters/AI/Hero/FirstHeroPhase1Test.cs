using System;
using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Hero;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x02001279 RID: 4729
	public class FirstHeroPhase1Test : AIController
	{
		// Token: 0x06005DBA RID: 23994 RVA: 0x00113932 File Offset: 0x00111B32
		private void Awake()
		{
			this._slash = new Characters.AI.Behaviours.Behaviour[]
			{
				this._basicSlash,
				this._horizontalSlash,
				this._verticalSlash
			};
		}

		// Token: 0x06005DBB RID: 23995 RVA: 0x000F0D27 File Offset: 0x000EEF27
		private new void OnEnable()
		{
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06005DBC RID: 23996 RVA: 0x0011395B File Offset: 0x00111B5B
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			for (;;)
			{
				if (this._trigger.InShortRange(this))
				{
					if (this._trigger.CanRunDashBreakAway(this))
					{
						yield return this._dashBreakAway.CRun(this);
					}
					else if (this._trigger.CanRunBehavourE(this))
					{
						yield return this._behaviourE.CRun(this);
						yield return this._idle.CRun(this);
					}
					else
					{
						switch (UnityEngine.Random.Range(0, 3))
						{
						case 0:
							yield return this._slash.Random<Characters.AI.Behaviours.Behaviour>().CRun(this);
							if (MMMaths.Chance(this._behaviourA_Chance))
							{
								yield return this._behaviourA.CRun(this);
							}
							yield return this._skipableIdle.CRun(this);
							break;
						case 1:
							yield return this._landing.CRun(this);
							if (MMMaths.Chance(this._behaviourB_Chance))
							{
								yield return this._behaviourB.CRun(this);
								if (MMMaths.Chance(this._behaviourC_Chance))
								{
									yield return this._behaviourC.CRun(this);
								}
							}
							yield return this._skipableIdle.CRun(this);
							break;
						case 2:
							yield return this._behaviourD.CRun(this);
							yield return this._skipableIdle.CRun(this);
							break;
						}
					}
				}
				else if (this._trigger.InMiddleRange(this))
				{
					if (this._trigger.CanRunBehavourJ(this))
					{
						yield return this._behaviourJ.CRun(this);
						if (MMMaths.Chance(this._behaviourK_Chance))
						{
							yield return this._behaviourK.CRun(this);
						}
						yield return this._idle.CRun(this);
					}
					else
					{
						switch (UnityEngine.Random.Range(0, 3))
						{
						case 0:
							yield return this._dash.CRun(this);
							yield return this._behaviourF.CRun(this);
							yield return this._skipableIdle.CRun(this);
							break;
						case 1:
							yield return this._landing.CRun(this);
							if (MMMaths.Chance(this._behaviourG_Chance))
							{
								yield return this._behaviourG.CRun(this);
								if (MMMaths.Chance(this._behaviourH_Chance))
								{
									yield return this._behaviourH.CRun(this);
								}
							}
							yield return this._skipableIdle.CRun(this);
							break;
						case 2:
							yield return this._behaviourJ.CRun(this);
							yield return this._skipableIdle.CRun(this);
							break;
						}
					}
				}
				else
				{
					if (MMMaths.RandomBool())
					{
						yield return this._dash.CRun(this);
						yield return this._behaviourL.CRun(this);
					}
					else
					{
						yield return this._behaviourM.CRun(this);
						if (MMMaths.Chance(this._behaviourN_Chance))
						{
							yield return this._behaviourN.CRun(this);
						}
					}
					yield return this._skipableIdle.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x04004B37 RID: 19255
		[Subcomponent(typeof(BackSlashA))]
		[SerializeField]
		[Header("Slash")]
		private BackSlashA _basicSlash;

		// Token: 0x04004B38 RID: 19256
		[SerializeField]
		[Subcomponent(typeof(BackSlashB))]
		private BackSlashB _horizontalSlash;

		// Token: 0x04004B39 RID: 19257
		[Subcomponent(typeof(VerticalSlash))]
		[SerializeField]
		private VerticalSlash _verticalSlash;

		// Token: 0x04004B3A RID: 19258
		[Subcomponent(typeof(Landing))]
		[Header("Basic Skill")]
		[SerializeField]
		private Landing _landing;

		// Token: 0x04004B3B RID: 19259
		[SerializeField]
		[Subcomponent(typeof(Characters.AI.Behaviours.Hero.Dash))]
		[Header("Dash")]
		private Characters.AI.Behaviours.Hero.Dash _dash;

		// Token: 0x04004B3C RID: 19260
		[Subcomponent(typeof(DashBreakAway))]
		[SerializeField]
		private DashBreakAway _dashBreakAway;

		// Token: 0x04004B3D RID: 19261
		[Header("Template")]
		[SerializeField]
		private BehaviourTemplate _behaviourA;

		// Token: 0x04004B3E RID: 19262
		[SerializeField]
		private BehaviourTemplate _behaviourB;

		// Token: 0x04004B3F RID: 19263
		[SerializeField]
		private BehaviourTemplate _behaviourC;

		// Token: 0x04004B40 RID: 19264
		[SerializeField]
		private BehaviourTemplate _behaviourD;

		// Token: 0x04004B41 RID: 19265
		[SerializeField]
		private BehaviourTemplate _behaviourE;

		// Token: 0x04004B42 RID: 19266
		[SerializeField]
		private BehaviourTemplate _behaviourF;

		// Token: 0x04004B43 RID: 19267
		[SerializeField]
		private BehaviourTemplate _behaviourG;

		// Token: 0x04004B44 RID: 19268
		[SerializeField]
		private BehaviourTemplate _behaviourH;

		// Token: 0x04004B45 RID: 19269
		[SerializeField]
		private BehaviourTemplate _behaviourI;

		// Token: 0x04004B46 RID: 19270
		[SerializeField]
		private BehaviourTemplate _behaviourJ;

		// Token: 0x04004B47 RID: 19271
		[SerializeField]
		private BehaviourTemplate _behaviourK;

		// Token: 0x04004B48 RID: 19272
		[SerializeField]
		private BehaviourTemplate _behaviourL;

		// Token: 0x04004B49 RID: 19273
		[SerializeField]
		private BehaviourTemplate _behaviourM;

		// Token: 0x04004B4A RID: 19274
		[SerializeField]
		private BehaviourTemplate _behaviourN;

		// Token: 0x04004B4B RID: 19275
		[Subcomponent(typeof(SkipableIdle))]
		[SerializeField]
		[Header("Idle")]
		private SkipableIdle _skipableIdle;

		// Token: 0x04004B4C RID: 19276
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x04004B4D RID: 19277
		[Range(0f, 1f)]
		[Header("Behaviour Chance")]
		[SerializeField]
		private float _behaviourA_Chance = 0.5f;

		// Token: 0x04004B4E RID: 19278
		[SerializeField]
		[Range(0f, 1f)]
		private float _behaviourB_Chance = 0.5f;

		// Token: 0x04004B4F RID: 19279
		[SerializeField]
		[Range(0f, 1f)]
		private float _behaviourC_Chance = 0.5f;

		// Token: 0x04004B50 RID: 19280
		[Range(0f, 1f)]
		[SerializeField]
		private float _behaviourG_Chance = 0.5f;

		// Token: 0x04004B51 RID: 19281
		[Range(0f, 1f)]
		[SerializeField]
		private float _behaviourH_Chance = 0.5f;

		// Token: 0x04004B52 RID: 19282
		[Range(0f, 1f)]
		[SerializeField]
		private float _behaviourK_Chance = 0.5f;

		// Token: 0x04004B53 RID: 19283
		[Range(0f, 1f)]
		[SerializeField]
		private float _behaviourN_Chance = 0.5f;

		// Token: 0x04004B54 RID: 19284
		[Header("Trigger")]
		[Subcomponent(typeof(Trigger))]
		[SerializeField]
		private Trigger _trigger;

		// Token: 0x04004B55 RID: 19285
		private Characters.AI.Behaviours.Behaviour[] _slash;
	}
}
