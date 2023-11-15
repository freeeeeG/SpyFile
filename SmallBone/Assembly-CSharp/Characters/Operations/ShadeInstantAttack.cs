using System;
using System.Collections.Generic;
using FX.BoundsAttackVisualEffect;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E00 RID: 3584
	public class ShadeInstantAttack : CharacterOperation
	{
		// Token: 0x060047A9 RID: 18345 RVA: 0x000D0494 File Offset: 0x000CE694
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(3);
			this._damages = new int[]
			{
				this._damage1,
				this._damage2,
				this._damage3
			};
			this._effects = new BoundsAttackVisualEffect.Subcomponents[]
			{
				this._effect1,
				this._effect2,
				this._effect3
			};
			this._attackRange.enabled = false;
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x000D0508 File Offset: 0x000CE708
		public override void Run(Character owner)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
			this._attackRange.enabled = true;
			Bounds bounds = this._attackRange.bounds;
			this._overlapper.OverlapCollider(this._attackRange);
			this._attackRange.enabled = false;
			List<Target> components = this._overlapper.results.GetComponents(true);
			if (components.Count == 0)
			{
				return;
			}
			int num = this._damages[components.Count - 1];
			BoundsAttackVisualEffect.Subcomponents subcomponents = this._effects[components.Count - 1];
			for (int i = 0; i < components.Count; i++)
			{
				Target target = components[i];
				if (!(target == null) && !(target.character == null) && !(target.character == owner) && target.character.liveAndActive)
				{
					Bounds bounds2 = target.collider.bounds;
					Bounds bounds3 = new Bounds
					{
						min = MMMaths.Max(bounds.min, bounds2.min),
						max = MMMaths.Min(bounds.max, bounds2.max)
					};
					Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds3);
					Damage damage = owner.stat.GetDamage((double)num, hitPoint, this._hitInfo);
					subcomponents.Spawn(owner, bounds3, damage, target);
					if (!target.character.cinematic.value)
					{
						owner.TryAttackCharacter(target, ref damage);
					}
				}
			}
		}

		// Token: 0x040036A9 RID: 13993
		private const int _limit = 3;

		// Token: 0x040036AA RID: 13994
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

		// Token: 0x040036AB RID: 13995
		[SerializeField]
		private Collider2D _attackRange;

		// Token: 0x040036AC RID: 13996
		[SerializeField]
		private int _damage1;

		// Token: 0x040036AD RID: 13997
		[SerializeField]
		private int _damage2;

		// Token: 0x040036AE RID: 13998
		[SerializeField]
		private int _damage3;

		// Token: 0x040036AF RID: 13999
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private BoundsAttackVisualEffect.Subcomponents _effect1;

		// Token: 0x040036B0 RID: 14000
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private BoundsAttackVisualEffect.Subcomponents _effect2;

		// Token: 0x040036B1 RID: 14001
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private BoundsAttackVisualEffect.Subcomponents _effect3;

		// Token: 0x040036B2 RID: 14002
		private int[] _damages;

		// Token: 0x040036B3 RID: 14003
		private BoundsAttackVisualEffect.Subcomponents[] _effects;

		// Token: 0x040036B4 RID: 14004
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x040036B5 RID: 14005
		private NonAllocOverlapper _overlapper;

		// Token: 0x040036B6 RID: 14006
		private float _remainTimeToNextAttack;
	}
}
