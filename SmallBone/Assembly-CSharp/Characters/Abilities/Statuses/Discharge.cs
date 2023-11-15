using System;
using System.Collections.Generic;
using Characters.Movements;
using FX;
using FX.SpriteEffects;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B6C RID: 2924
	public class Discharge : IAbility, IAbilityInstance
	{
		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06003AB0 RID: 15024 RVA: 0x000AD3D6 File Offset: 0x000AB5D6
		// (set) Token: 0x06003AB1 RID: 15025 RVA: 0x000AD3DE File Offset: 0x000AB5DE
		public Character owner { get; private set; }

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06003AB3 RID: 15027 RVA: 0x000AD3E7 File Offset: 0x000AB5E7
		// (set) Token: 0x06003AB4 RID: 15028 RVA: 0x000AD3EF File Offset: 0x000AB5EF
		public float remainTime { get; set; }

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06003AB5 RID: 15029 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06003AB6 RID: 15030 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06003AB7 RID: 15031 RVA: 0x000AD3F8 File Offset: 0x000AB5F8
		public float iconFillAmount
		{
			get
			{
				return this.remainTime / this.duration;
			}
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06003AB8 RID: 15032 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06003ABA RID: 15034 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06003ABB RID: 15035 RVA: 0x000AD407 File Offset: 0x000AB607
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06003ABC RID: 15036 RVA: 0x000AD419 File Offset: 0x000AB619
		// (set) Token: 0x06003ABD RID: 15037 RVA: 0x000AD421 File Offset: 0x000AB621
		public float duration { get; set; }

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06003ABE RID: 15038 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06003ABF RID: 15039 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x000AD42C File Offset: 0x000AB62C
		public Discharge(Character owner)
		{
			this.owner = owner;
			this._hitParticle = CommonResource.instance.hitParticle;
			this._targets = new List<Target>(128);
			this._targetLayer = new TargetLayer(0, true, false, false, false);
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x000AD488 File Offset: 0x000AB688
		private void GiveDamage(Character attacker, double amount)
		{
			if (attacker == null)
			{
				return;
			}
			if (this.owner == null)
			{
				return;
			}
			LayerMask layerMask = this._targetLayer.Evaluate(this.owner.gameObject);
			if (this.owner.type == Character.Type.Player)
			{
				layerMask |= 1024;
			}
			TargetFinder.FindTargetInRange(this.owner.transform.position, 3f, layerMask, this._targets);
			foreach (Target target in this._targets)
			{
				Damage damage = new Damage(this._attacker, amount * 1.0, MMMaths.RandomPointWithinBounds(target.collider.bounds), Damage.Attribute.Magic, Damage.AttackType.Additional, Damage.MotionType.Status, 1.0, 0.5f, 0.0, 1.0, 1.0, true, false, 0.0, 0.0, 0, null, 1.0);
				this._attacker.Attack(target, ref damage);
				this._hitParticle.Emit(target.transform.position, target.collider.bounds, Vector2.zero, true);
				int num = (this.owner.transform.position.x > target.transform.position.x) ? 0 : 180;
				PushForce force = new PushForce
				{
					power = new CustomFloat(3f, 4f),
					angle = new CustomFloat((float)num)
				};
				PushForce force2 = new PushForce
				{
					power = new CustomFloat(2f),
					angle = new CustomFloat(90f)
				};
				AnimationCurve curve = new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f),
					new Keyframe(0.25f, 0.55f),
					new Keyframe(0.5f, 0.8f),
					new Keyframe(1f, 1f)
				});
				Curve curve2 = new Curve(curve, 1f, 0.8f);
				Curve curve3 = new Curve(curve, 1f, 0.44f);
				PushInfo info = new PushInfo(force, curve2, force2, curve3, false, false);
				if (target.character != null)
				{
					target.character.movement.push.ApplyKnockback(this.owner, info);
				}
			}
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x000AD754 File Offset: 0x000AB954
		public void Add(Character attacker, float duration, double damagePerSecond)
		{
			this._attacker = attacker;
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x000AD760 File Offset: 0x000AB960
		public void Refresh()
		{
			int num = this._stack + 1;
			this._stack = num;
			if (num >= 3)
			{
				this.remainTime = 0f;
				this.GiveDamage(this._attacker, (double)this._attacker.GetComponent<IAttackDamage>().amount);
			}
			this.SpawnFloatingText(string.Format("방전x{0}", this._stack));
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x000AD7C4 File Offset: 0x000AB9C4
		public void Attach()
		{
			this.remainTime = 2.1474836E+09f;
			this._stack = 1;
			this.owner.spriteEffectStack.Add(Discharge._colorBlend);
			this.SpawnFloatingText(string.Format("방전x{0}", this._stack));
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x000AD813 File Offset: 0x000ABA13
		public void Detach()
		{
			this.owner.spriteEffectStack.Remove(Discharge._colorBlend);
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x000AD82B File Offset: 0x000ABA2B
		public void Initialize()
		{
			this._effect = new EffectInfo(CommonResource.instance.poisonEffect);
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x000AD844 File Offset: 0x000ABA44
		private void SpawnFloatingText(string text)
		{
			Vector2 v = MMMaths.RandomPointWithinBounds(this.owner.collider.bounds);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(text, v, "#2cbb00");
		}

		// Token: 0x04002E87 RID: 11911
		private static readonly ColorBlend _colorBlend = new ColorBlend(100, new Color(0.1254902f, 1f, 0.1254902f, 1f), 0f);

		// Token: 0x04002E88 RID: 11912
		private const float _tickInterval = 0.5f;

		// Token: 0x04002E89 RID: 11913
		private const int _maxStack = 3;

		// Token: 0x04002E8A RID: 11914
		private const string _floatingTextKey = "floating/status/poision";

		// Token: 0x04002E8B RID: 11915
		private const string _floatingTextColor = "#2cbb00";

		// Token: 0x04002E8C RID: 11916
		private EffectInfo _effect;

		// Token: 0x04002E8E RID: 11918
		private ParticleEffectInfo _hitParticle;

		// Token: 0x04002E91 RID: 11921
		private readonly List<Discharge.Info> _infos = new List<Discharge.Info>();

		// Token: 0x04002E92 RID: 11922
		private int _stack;

		// Token: 0x04002E93 RID: 11923
		private Character _attacker;

		// Token: 0x04002E94 RID: 11924
		private List<Target> _targets;

		// Token: 0x04002E95 RID: 11925
		private readonly TargetLayer _targetLayer;

		// Token: 0x02000B6D RID: 2925
		private class Info
		{
			// Token: 0x17000C07 RID: 3079
			// (get) Token: 0x06003ACB RID: 15051 RVA: 0x000AD8AE File Offset: 0x000ABAAE
			public double remainDamage
			{
				get
				{
					return this.damagePerTick * (double)this.remainTicks;
				}
			}

			// Token: 0x06003ACC RID: 15052 RVA: 0x000AD8BE File Offset: 0x000ABABE
			public Info(Character attacker, double damagePerTick, int ticks)
			{
				this.attacker = attacker;
				this.damagePerTick = damagePerTick;
				this.remainTicks = ticks;
			}

			// Token: 0x04002E96 RID: 11926
			public readonly Character attacker;

			// Token: 0x04002E97 RID: 11927
			public readonly double damagePerTick;

			// Token: 0x04002E98 RID: 11928
			public int remainTicks;
		}
	}
}
