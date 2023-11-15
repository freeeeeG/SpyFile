using System;
using System.Collections;
using System.Linq;
using Characters.Usables;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009E7 RID: 2535
	public class EffectZoneAbilityAttacher : AbilityAttacher
	{
		// Token: 0x060035EC RID: 13804 RVA: 0x000A000C File Offset: 0x0009E20C
		public override void OnIntialize()
		{
			this._abilityComponents.Initialize();
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x000A0019 File Offset: 0x0009E219
		public override void StartAttach()
		{
			this._cCheckReference.Stop();
			this._cCheckReference = this.StartCoroutineWithReference(this.CCheck());
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000A0038 File Offset: 0x0009E238
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

		// Token: 0x060035EF RID: 13807 RVA: 0x000A0094 File Offset: 0x0009E294
		private IEnumerator CCheck()
		{
			for (;;)
			{
				this._collider.enabled = true;
				EffectZoneAbilityAttacher._sharedOverlapper.contactFilter.SetLayerMask(this._layer.Evaluate(base.owner.gameObject));
				if (EffectZoneAbilityAttacher._sharedOverlapper.OverlapCollider(this._collider).GetComponents<EffectZone>(true).Count((EffectZone target) => target.MatchKey(this._keys)) > 0)
				{
					if (!this._attached)
					{
						for (int i = 0; i < this._abilityComponents.components.Length; i++)
						{
							base.owner.ability.Add(this._abilityComponents.components[i].ability);
						}
						this._attached = true;
					}
				}
				else
				{
					for (int j = 0; j < this._abilityComponents.components.Length; j++)
					{
						base.owner.ability.Remove(this._abilityComponents.components[j].ability);
					}
					this._attached = false;
				}
				yield return new WaitForSecondsRealtime(this._checkInterval);
			}
			yield break;
		}

		// Token: 0x04002B4D RID: 11085
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

		// Token: 0x04002B4E RID: 11086
		[SerializeField]
		[Header("Finder")]
		[FrameTime]
		private float _checkInterval = 0.33f;

		// Token: 0x04002B4F RID: 11087
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04002B50 RID: 11088
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x04002B51 RID: 11089
		[SerializeField]
		private string[] _keys;

		// Token: 0x04002B52 RID: 11090
		[SerializeField]
		[Header("Abilities")]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent.Subcomponents _abilityComponents;

		// Token: 0x04002B53 RID: 11091
		private CoroutineReference _cCheckReference;

		// Token: 0x04002B54 RID: 11092
		private bool _attached;
	}
}
