using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Movements;
using Characters.Operations.Attack;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Actions
{
	// Token: 0x0200093A RID: 2362
	[RequireComponent(typeof(GrabBoard))]
	public sealed class GrabAction : Action
	{
		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x060032C2 RID: 12994 RVA: 0x00096CD7 File Offset: 0x00094ED7
		public override Motion[] motions
		{
			get
			{
				return this._grabMotions.components.Concat(this._maintainMotions.components).Concat(this._grabFailMotions.components).ToArray<Motion>();
			}
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x00096D0C File Offset: 0x00094F0C
		protected override void Awake()
		{
			GrabAction.<>c__DisplayClass11_0 CS$<>8__locals1 = new GrabAction.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			base.Awake();
			CS$<>8__locals1.blockLookBefore = false;
			CS$<>8__locals1.<Awake>g__JoinGrabMotion|1(this._grabMotions);
			CS$<>8__locals1.<Awake>g__JoinMotion|0(this._maintainMotions);
			CS$<>8__locals1.<Awake>g__JoinMotion|0(this._grabFailMotions);
			if (this._attackHitTrigger)
			{
				this._attacks = new List<IAttack>();
				Motion[] components = this._grabMotions.components;
				for (int i = 0; i < components.Length; i++)
				{
					IAttack componentInChildren = components[i].GetComponentInChildren<IAttack>();
					if (componentInChildren != null)
					{
						this._attacks.Add(componentInChildren);
					}
				}
				if (this._attacks.Count == 0)
				{
					Debug.LogError("Attack is null " + base.gameObject.name);
					return;
				}
				this._attacks.ForEach(delegate(IAttack attack)
				{
					attack.onHit += CS$<>8__locals1.<>4__this.OnAttackHit;
				});
			}
			int num = this._maintainMotions.components.Length;
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x00096DE8 File Offset: 0x00094FE8
		private void OnAttackHit(Target target, ref Damage damage)
		{
			if (!this._grabbing)
			{
				return;
			}
			this._grabbing = false;
			if (target.character.movement.config.type == Movement.Config.Type.Static || target.character.stat.GetFinal(Stat.Kind.KnockbackResistance) == 0.0)
			{
				this._grabBoard.AddFailed(target);
				this._grabMotions.EndBehaviour();
				return;
			}
			this._grabBoard.Add(target);
			this._grabMotions.EndBehaviour();
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x060032C5 RID: 12997 RVA: 0x000950CB File Offset: 0x000932CB
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this.motions[0]);
			}
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x00096E6C File Offset: 0x0009506C
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			for (int i = 0; i < this.motions.Length; i++)
			{
				this.motions[i].Initialize(this);
			}
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x00096EA1 File Offset: 0x000950A1
		public override bool TryStart()
		{
			if (!base.gameObject.activeSelf || !this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			this._grabbing = true;
			base.DoAction(this._grabMotions.components[0]);
			return true;
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x00096EDD File Offset: 0x000950DD
		private void OnDestroy()
		{
			if (this._attacks != null)
			{
				this._attacks.ForEach(delegate(IAttack attack)
				{
					attack.onHit -= this.OnAttackHit;
				});
			}
		}

		// Token: 0x04002962 RID: 10594
		[SerializeField]
		[GetComponent]
		private GrabBoard _grabBoard;

		// Token: 0x04002963 RID: 10595
		[SerializeField]
		private bool _attackHitTrigger;

		// Token: 0x04002964 RID: 10596
		[SerializeField]
		private bool _doFailMotion;

		// Token: 0x04002965 RID: 10597
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		private Motion.Subcomponents _grabMotions;

		// Token: 0x04002966 RID: 10598
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		private Motion.Subcomponents _maintainMotions;

		// Token: 0x04002967 RID: 10599
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		private Motion.Subcomponents _grabFailMotions;

		// Token: 0x04002968 RID: 10600
		private Character.LookingDirection _lookingDirection;

		// Token: 0x04002969 RID: 10601
		private List<IAttack> _attacks;

		// Token: 0x0400296A RID: 10602
		private bool _grabbing;
	}
}
