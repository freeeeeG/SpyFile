using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Operations.Fx;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001102 RID: 4354
	public class FirstHeroAITeleport : AIController
	{
		// Token: 0x060054AA RID: 21674 RVA: 0x000FD57C File Offset: 0x000FB77C
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._advent,
				this._teleportBehind,
				this._commboAttack,
				this._energyBlast
			};
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x000FD5D0 File Offset: 0x000FB7D0
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x000FD5F8 File Offset: 0x000FB7F8
		protected override IEnumerator CProcess()
		{
			yield return Chronometer.global.WaitForSeconds(1f);
			yield return this.DoAdvent();
			while (!base.dead)
			{
				if (base.target == null)
				{
					yield return null;
				}
				else
				{
					yield return this.DoDash();
					if (MMMaths.RandomBool())
					{
						yield return this.DoEnergyBlast();
					}
					else
					{
						yield return this.DoComoboAttack();
					}
				}
			}
			yield break;
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x000FD607 File Offset: 0x000FB807
		private IEnumerator DoComoboAttack()
		{
			float num = base.target.transform.position.x - this.character.transform.position.x;
			this.character.lookingDirection = ((num > 0f) ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			yield return this._commboAttack.CRun(this);
			yield break;
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x000FD616 File Offset: 0x000FB816
		private IEnumerator DoAdvent()
		{
			this.character.transform.position = Singleton<Service>.Instance.levelManager.player.transform.position;
			yield return this._advent.CRun(this);
			this._adventIdle.TryStart();
			while (this._adventIdle.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x000FD625 File Offset: 0x000FB825
		private IEnumerator DoDash()
		{
			base.destination = base.target.transform.position;
			this._motionTrail.Run(this.character);
			yield return this._teleportBehind.CRun(this);
			this._motionTrail.Stop();
			yield break;
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x000FD634 File Offset: 0x000FB834
		private IEnumerator DoEnergyBlast()
		{
			yield return this._energyBlast.CRun(this);
			yield break;
		}

		// Token: 0x060054B1 RID: 21681 RVA: 0x000FD643 File Offset: 0x000FB843
		private IEnumerator DoRangeContinuousAttack()
		{
			while (!base.dead)
			{
				yield return Chronometer.global.WaitForSeconds(this._rangeContinuousAttackDuration);
				this._rangeContinuousAttack.TryStart();
			}
			yield break;
		}

		// Token: 0x040043F4 RID: 17396
		[SerializeField]
		private Characters.Actions.Action _adventIdle;

		// Token: 0x040043F5 RID: 17397
		[SerializeField]
		private Characters.Actions.Action _rangeContinuousAttack;

		// Token: 0x040043F6 RID: 17398
		[SerializeField]
		private float _rangeContinuousAttackDuration;

		// Token: 0x040043F7 RID: 17399
		[SerializeField]
		private MotionTrail _motionTrail;

		// Token: 0x040043F8 RID: 17400
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040043F9 RID: 17401
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _advent;

		// Token: 0x040043FA RID: 17402
		[Subcomponent(typeof(TeleportBehind))]
		[SerializeField]
		private TeleportBehind _teleportBehind;

		// Token: 0x040043FB RID: 17403
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _commboAttack;

		// Token: 0x040043FC RID: 17404
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _energyBlast;
	}
}
