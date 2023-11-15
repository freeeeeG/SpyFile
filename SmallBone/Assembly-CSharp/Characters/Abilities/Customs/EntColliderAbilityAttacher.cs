using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Gear.Weapons.Gauges;
using Characters.Usables;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D1A RID: 3354
	public class EntColliderAbilityAttacher : AbilityAttacher
	{
		// Token: 0x060043A5 RID: 17317 RVA: 0x000C4F76 File Offset: 0x000C3176
		private void Awake()
		{
			if (this._optimizedCollider)
			{
				this._collider.enabled = false;
			}
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x000C4F8C File Offset: 0x000C318C
		public override void OnIntialize()
		{
			this._abilityComponents.Initialize();
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x000C4F99 File Offset: 0x000C3199
		public override void StartAttach()
		{
			this._cCheckReference.Stop();
			this._cCheckReference = this.StartCoroutineWithReference(this.CCheck());
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x000C4FB8 File Offset: 0x000C31B8
		public override void StopAttach()
		{
			this._cCheckReference.Stop();
			if (base.owner == null)
			{
				return;
			}
			foreach (AbilityComponent abilityComponent in this._abilityComponents.components)
			{
				base.owner.ability.Remove(abilityComponent.ability);
			}
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x000C5014 File Offset: 0x000C3214
		private IEnumerator CCheck()
		{
			for (;;)
			{
				this._collider.enabled = true;
				EntColliderAbilityAttacher._sharedOverlapper.contactFilter.SetLayerMask(this._layer.Evaluate(base.owner.gameObject));
				List<Liquid> components = EntColliderAbilityAttacher._sharedOverlapper.OverlapCollider(this._collider).GetComponents<Liquid>(true);
				if (this._optimizedCollider)
				{
					this._collider.enabled = false;
				}
				if (components.Count > 0)
				{
					for (int i = 0; i < this._abilityComponents.components.Length; i++)
					{
						base.owner.ability.Add(this._abilityComponents.components[i].ability);
					}
					if (this._gaugeControls)
					{
						this._gauge.defaultBarGaugeColor.baseColor = this._defaultBarColor;
					}
				}
				else
				{
					if (this._gaugeControls)
					{
						float deltaTime = Chronometer.global.deltaTime;
						this._gaugeAnimationTime += deltaTime * 2f;
						if (this._gaugeAnimationTime > 2f)
						{
							this._gaugeAnimationTime = 0f;
						}
						this._gauge.defaultBarGaugeColor.baseColor = Color.LerpUnclamped(this._defaultBarColor, this._buffBarColor, (this._gaugeAnimationTime < 1f) ? this._gaugeAnimationTime : (2f - this._gaugeAnimationTime));
					}
					for (int j = 0; j < this._abilityComponents.components.Length; j++)
					{
						base.owner.ability.Remove(this._abilityComponents.components[j].ability);
					}
				}
				yield return Chronometer.global.WaitForSeconds(this._checkInterval);
			}
			yield break;
		}

		// Token: 0x040033A9 RID: 13225
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

		// Token: 0x040033AA RID: 13226
		[SerializeField]
		private float _checkInterval = 0.1f;

		// Token: 0x040033AB RID: 13227
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x040033AC RID: 13228
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x040033AD RID: 13229
		[SerializeField]
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		private bool _optimizedCollider = true;

		// Token: 0x040033AE RID: 13230
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		[Header("Abilities")]
		private AbilityComponent.Subcomponents _abilityComponents;

		// Token: 0x040033AF RID: 13231
		[SerializeField]
		private bool _gaugeControls;

		// Token: 0x040033B0 RID: 13232
		[SerializeField]
		private Color _defaultBarColor;

		// Token: 0x040033B1 RID: 13233
		[SerializeField]
		private Color _buffBarColor;

		// Token: 0x040033B2 RID: 13234
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x040033B3 RID: 13235
		private CoroutineReference _cCheckReference;

		// Token: 0x040033B4 RID: 13236
		private float _gaugeAnimationTime;
	}
}
