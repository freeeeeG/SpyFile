using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B6A RID: 2922
	public class Burn : CharacterStatusAbility, IAbility, IAbilityInstance
	{
		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06003A84 RID: 14980 RVA: 0x000ACD6C File Offset: 0x000AAF6C
		// (set) Token: 0x06003A85 RID: 14981 RVA: 0x000ACD74 File Offset: 0x000AAF74
		public Character owner { get; private set; }

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06003A86 RID: 14982 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06003A87 RID: 14983 RVA: 0x000ACD7D File Offset: 0x000AAF7D
		// (set) Token: 0x06003A88 RID: 14984 RVA: 0x000ACD85 File Offset: 0x000AAF85
		public float remainTime { get; set; }

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06003A89 RID: 14985 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06003A8A RID: 14986 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06003A8B RID: 14987 RVA: 0x000ACD8E File Offset: 0x000AAF8E
		public float iconFillAmount
		{
			get
			{
				return this.remainTime / this.duration;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06003A8C RID: 14988 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06003A8D RID: 14989 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06003A8E RID: 14990 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06003A8F RID: 14991 RVA: 0x000ACD9D File Offset: 0x000AAF9D
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06003A90 RID: 14992 RVA: 0x000ACDAF File Offset: 0x000AAFAF
		public float duration
		{
			get
			{
				return CharacterStatusSetting.instance.burn.duration * base.durationMultiplier;
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06003A91 RID: 14993 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06003A92 RID: 14994 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06003A93 RID: 14995 RVA: 0x000ACDC8 File Offset: 0x000AAFC8
		// (remove) Token: 0x06003A94 RID: 14996 RVA: 0x000ACE00 File Offset: 0x000AB000
		public override event CharacterStatus.OnTimeDelegate onAttachEvents;

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06003A95 RID: 14997 RVA: 0x000ACE38 File Offset: 0x000AB038
		// (remove) Token: 0x06003A96 RID: 14998 RVA: 0x000ACE70 File Offset: 0x000AB070
		public override event CharacterStatus.OnTimeDelegate onRefreshEvents;

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06003A97 RID: 14999 RVA: 0x000ACEA8 File Offset: 0x000AB0A8
		// (remove) Token: 0x06003A98 RID: 15000 RVA: 0x000ACEE0 File Offset: 0x000AB0E0
		public override event CharacterStatus.OnTimeDelegate onDetachEvents;

		// Token: 0x06003A99 RID: 15001 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06003A9A RID: 15002 RVA: 0x000ACF15 File Offset: 0x000AB115
		// (set) Token: 0x06003A9B RID: 15003 RVA: 0x000ACF1D File Offset: 0x000AB11D
		public new StatusEffect.BurnHandler effectHandler { get; set; }

		// Token: 0x06003A9C RID: 15004 RVA: 0x000ACF26 File Offset: 0x000AB126
		public Burn(Character owner)
		{
			this.owner = owner;
			this._targets = new List<Target>(128);
			this._targetLayer = new TargetLayer(0, true, false, false, false);
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x000ACF5C File Offset: 0x000AB15C
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
			this._remainTimeToNextTick -= deltaTime;
			this.effectHandler.UpdateTime(deltaTime);
			if (this._remainTimeToNextTick <= 0f)
			{
				this._remainTimeToNextTick += CharacterStatusSetting.instance.burn.tickInterval;
				this.GiveDamage();
			}
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x000ACFC0 File Offset: 0x000AB1C0
		private void GiveDamage()
		{
			if (base.attacker == null)
			{
				return;
			}
			if (this.owner.health.dead)
			{
				return;
			}
			LayerMask layerMask = this._targetLayer.Evaluate(this.owner.gameObject);
			if (this.owner.type == Character.Type.Player)
			{
				layerMask |= 1024;
			}
			Damage damage = base.attacker.stat.GetDamage((double)CharacterStatusSetting.instance.burn.baseTargetTickDamage, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), CharacterStatusSetting.instance.burn.hitInfo);
			damage.canCritical = false;
			damage.multiplier -= damage.multiplier * (1.0 - this.owner.stat.GetStatusResistacneFor(CharacterStatus.Kind.Burn));
			base.attacker.Attack(this.owner, ref damage);
			CharacterStatus.OnTimeDelegate onTimeDelegate = this.onTookBurnDamage;
			if (onTimeDelegate != null)
			{
				onTimeDelegate(base.attacker, this.owner);
			}
			this.effectHandler.HandleOnTookBurnDamage(base.attacker, this.owner);
			if (base.attacker == null)
			{
				return;
			}
			float radius = CharacterStatusSetting.instance.burn.rangeRadius * (float)base.attacker.stat.GetFinal(Stat.Kind.EmberDamage);
			TargetFinder.FindTargetInRange(this.owner.transform.position, radius, layerMask, this._targets);
			foreach (Target target in this._targets)
			{
				if (base.attacker == null)
				{
					break;
				}
				if (!(target.character == null) && !target.character.health.dead && !(target.character == this.owner))
				{
					damage = base.attacker.stat.GetDamage((double)CharacterStatusSetting.instance.burn.baseRangeTickDamage, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), CharacterStatusSetting.instance.burn.rangeHitInfo);
					damage.canCritical = false;
					damage.multiplier *= base.attacker.stat.GetFinal(Stat.Kind.EmberDamage);
					base.attacker.Attack(target, ref damage);
					CharacterStatus.OnTimeDelegate onTimeDelegate2 = this.onTookEmberDamage;
					if (onTimeDelegate2 != null)
					{
						onTimeDelegate2(base.attacker, target.character);
					}
					this.effectHandler.HandleOnTookEmberDamage(base.attacker, target.character);
				}
			}
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x000AD298 File Offset: 0x000AB498
		public void Refresh()
		{
			this.remainTime = this.duration;
			CharacterStatus.OnTimeDelegate onRefreshed = this.onRefreshed;
			if (onRefreshed != null)
			{
				onRefreshed(base.attacker, this.owner);
			}
			this.effectHandler.HandleOnRefresh(base.attacker, this.owner);
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x000AD2E8 File Offset: 0x000AB4E8
		public void Attach()
		{
			this.remainTime = this.duration;
			this._remainTimeToNextTick = 0f;
			CharacterStatus.OnTimeDelegate onAttached = this.onAttached;
			if (onAttached != null)
			{
				onAttached(base.attacker, this.owner);
			}
			this.effectHandler.HandleOnAttach(base.attacker, this.owner);
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x000AD340 File Offset: 0x000AB540
		public void Detach()
		{
			CharacterStatus.OnTimeDelegate onDetached = this.onDetached;
			if (onDetached != null)
			{
				onDetached(base.attacker, this.owner);
			}
			this.effectHandler.HandleOnDetach(base.attacker, this.owner);
			base.attacker = null;
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x000AD37D File Offset: 0x000AB57D
		public void Initialize()
		{
			this.effectHandler = StatusEffect.GetDefaultBurnEffectHanlder(this.owner);
		}

		// Token: 0x04002E78 RID: 11896
		private readonly TargetLayer _targetLayer;

		// Token: 0x04002E79 RID: 11897
		private float _remainTimeToNextTick;

		// Token: 0x04002E7A RID: 11898
		private List<Target> _targets;

		// Token: 0x04002E7B RID: 11899
		public CharacterStatus.OnTimeDelegate onTookBurnDamage;

		// Token: 0x04002E7C RID: 11900
		public CharacterStatus.OnTimeDelegate onTookEmberDamage;
	}
}
