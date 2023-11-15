using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010C4 RID: 4292
	public sealed class HolyKnightsAssassinAI : AIController
	{
		// Token: 0x06005345 RID: 21317 RVA: 0x000F9B7C File Offset: 0x000F7D7C
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._attack,
				this._jumpAttack,
				this._evasion
			};
			this._time = new ChronometerTime(this.character.chronometer.master, this.character);
			this._evasionCounter.Initialize(this._time);
			this.character.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
			this._teleportIn.onStart += delegate()
			{
				this._counter = false;
			};
			this._evasionCounter.onArrival += this.Evasion;
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x000F9C58 File Offset: 0x000F7E58
		private void Evasion()
		{
			this._counter = true;
			base.StopAllBehaviour();
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x000F9C67 File Offset: 0x000F7E67
		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._lastStagger = this._time.time;
			if (this._attack.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				this._attack.StopPropagation();
			}
			this.TryCountForEvasion(damageDealt);
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x000F9C9C File Offset: 0x000F7E9C
		private void TryCountForEvasion(double damageDealt)
		{
			if (this._counter)
			{
				return;
			}
			if (!this._evasion.CanUse())
			{
				return;
			}
			if (this.character.health.dead || base.dead || this.character.health.currentHealth <= damageDealt)
			{
				base.StopAllBehaviour();
				return;
			}
			if (this._evasion.result == Characters.AI.Behaviours.Behaviour.Result.Doing)
			{
				return;
			}
			this._evasionCounter.Count();
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x000F9D0E File Offset: 0x000F7F0E
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x000F9D36 File Offset: 0x000F7F36
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x000F9D45 File Offset: 0x000F7F45
		private IEnumerator CCombat()
		{
			yield return this._wander.CRun(this);
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && !base.stuned)
				{
					if (this._counter && this.character.health.currentHealth > 0.0 && !base.dead)
					{
						yield return this._evasion.CRun(this);
						this._counter = false;
					}
					else if (this._lastStagger <= this._time.time - this._stagger)
					{
						this._teleportIn.TryStart();
						while (this._teleportIn.running)
						{
							yield return null;
						}
						if (this._jumpAttack.CanUse(this.character) && MMMaths.RandomBool())
						{
							yield return this.CRunLightJump();
						}
						else
						{
							yield return this._attack.CRun(this);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x000F9D54 File Offset: 0x000F7F54
		private IEnumerator CRunLightJump()
		{
			this._shiftObject.Run(this.character);
			if (this.character.movement.controller.Teleport(this._destination.position))
			{
				yield return this._jumpAttack.CRun(this);
			}
			else
			{
				yield return this._attack.CRun(this);
			}
			yield break;
		}

		// Token: 0x040042E3 RID: 17123
		[Header("Behaviours")]
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040042E4 RID: 17124
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x040042E5 RID: 17125
		[Subcomponent(typeof(Chase))]
		[SerializeField]
		private Chase _chase;

		// Token: 0x040042E6 RID: 17126
		[SerializeField]
		[Subcomponent(typeof(TeleportAttack))]
		private TeleportAttack _attack;

		// Token: 0x040042E7 RID: 17127
		[SerializeField]
		[Subcomponent(typeof(LightJump))]
		private LightJump _jumpAttack;

		// Token: 0x040042E8 RID: 17128
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _evasion;

		// Token: 0x040042E9 RID: 17129
		[SerializeField]
		private Characters.Actions.Action _teleportIn;

		// Token: 0x040042EA RID: 17130
		[Subcomponent(typeof(ShiftObject))]
		[SerializeField]
		private ShiftObject _shiftObject;

		// Token: 0x040042EB RID: 17131
		[SerializeField]
		private Transform _destination;

		// Token: 0x040042EC RID: 17132
		[SerializeField]
		[Header("Tools")]
		[Space]
		private global::Counter _evasionCounter;

		// Token: 0x040042ED RID: 17133
		[SerializeField]
		private float _stagger;

		// Token: 0x040042EE RID: 17134
		private bool _counter;

		// Token: 0x040042EF RID: 17135
		private float _lastStagger;

		// Token: 0x040042F0 RID: 17136
		private ChronometerTime _time;
	}
}
