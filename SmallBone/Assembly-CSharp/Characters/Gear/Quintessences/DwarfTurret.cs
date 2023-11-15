using System;
using System.Collections;
using Characters.Abilities;
using Characters.Actions;
using Characters.Operations;
using Level;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences
{
	// Token: 0x020008D7 RID: 2263
	public class DwarfTurret : DestructibleObject
	{
		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x0600303C RID: 12348 RVA: 0x00090ADA File Offset: 0x0008ECDA
		public override Collider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x00090AE2 File Offset: 0x0008ECE2
		public Minion minion
		{
			get
			{
				return this._minion;
			}
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x00090AEC File Offset: 0x0008ECEC
		public override void Hit(Character from, ref Damage damage, Vector2 force)
		{
			if (damage.motionType != Damage.MotionType.Basic && damage.motionType != Damage.MotionType.Dash)
			{
				return;
			}
			if (from.type != Character.Type.Player)
			{
				return;
			}
			this._instance = this._minion.character.ability.Add(this._attackSpeedAbility.ability);
			base.StartCoroutine(this._onHitOperations.CRun(this._minion.character));
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x00090B58 File Offset: 0x0008ED58
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(31);
			this.minion.onSummon += delegate(Character owner, Character summoned)
			{
				this._remainAttackTime = this._attackInterval;
				this._remainLaserAttackTime = this._laserAttackInterval;
				base.StartCoroutine(this.CUpdate());
			};
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x00090B7E File Offset: 0x0008ED7E
		private IEnumerator CUpdate()
		{
			this._intro.TryStart();
			while (this._intro.running)
			{
				yield return null;
			}
			for (;;)
			{
				float num = this._minion.character.chronometer.master.deltaTime;
				bool hasTarget = true;
				this.HandleLaserAttack(num, hasTarget);
				if (this._instance != null && this._instance.attached)
				{
					num *= this._attackSpeedMultiplier;
				}
				this.HandleAttack(num, hasTarget);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x00090B90 File Offset: 0x0008ED90
		private void HandleAttack(float deltaTime, bool hasTarget)
		{
			this._remainAttackTime -= deltaTime;
			if (this._remainAttackTime > 0f)
			{
				return;
			}
			this._remainAttackTime += this._attackInterval;
			if (!hasTarget)
			{
				return;
			}
			if (this._minion.character.ability.Contains(this._attackSpeedAbility.ability))
			{
				this._fastAction.TryStart();
				return;
			}
			this._action.TryStart();
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x00090C0C File Offset: 0x0008EE0C
		private void HandleLaserAttack(float deltaTime, bool hasTarget)
		{
			if (this._laserAction == null)
			{
				return;
			}
			this._remainLaserAttackTime -= deltaTime;
			if (this._remainLaserAttackTime > 0f)
			{
				return;
			}
			this._remainLaserAttackTime += this._laserAttackInterval;
			if (!hasTarget)
			{
				return;
			}
			this._laserAction.TryStart();
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x00090C67 File Offset: 0x0008EE67
		private Character FindTarget()
		{
			this._overlapper.contactFilter.SetLayerMask(1024);
			return TargetFinder.FindClosestTarget(this._overlapper, this._sight, this._collider);
		}

		// Token: 0x040027F3 RID: 10227
		[SerializeField]
		private Minion _minion;

		// Token: 0x040027F4 RID: 10228
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x040027F5 RID: 10229
		[SerializeField]
		private Collider2D _sight;

		// Token: 0x040027F6 RID: 10230
		[Header("Intro")]
		[SerializeField]
		private Characters.Actions.Action _intro;

		// Token: 0x040027F7 RID: 10231
		[Header("Attack")]
		[SerializeField]
		private float _attackInterval = 1f;

		// Token: 0x040027F8 RID: 10232
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x040027F9 RID: 10233
		[SerializeField]
		private Characters.Actions.Action _fastAction;

		// Token: 0x040027FA RID: 10234
		[Header("Laser Attack")]
		[SerializeField]
		private float _laserAttackInterval = 5f;

		// Token: 0x040027FB RID: 10235
		[SerializeField]
		private Characters.Actions.Action _laserAction;

		// Token: 0x040027FC RID: 10236
		[Header("Attack Speed")]
		[SerializeField]
		private float _attackSpeedMultiplier;

		// Token: 0x040027FD RID: 10237
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _attackSpeedAbility;

		// Token: 0x040027FE RID: 10238
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onHitOperations;

		// Token: 0x040027FF RID: 10239
		private float _remainAttackTime;

		// Token: 0x04002800 RID: 10240
		private float _remainLaserAttackTime;

		// Token: 0x04002801 RID: 10241
		private IAbilityInstance _instance;

		// Token: 0x04002802 RID: 10242
		private NonAllocOverlapper _overlapper;
	}
}
