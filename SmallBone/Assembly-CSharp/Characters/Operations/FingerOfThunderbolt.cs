using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Operations
{
	// Token: 0x02000DDC RID: 3548
	public class FingerOfThunderbolt : CharacterOperation
	{
		// Token: 0x06004724 RID: 18212 RVA: 0x000CE85C File Offset: 0x000CCA5C
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(5);
			this._groundFinder = new RayCaster
			{
				direction = Vector2.down,
				distance = 5f
			};
			this._groundFinder.contactFilter.SetLayerMask(Layers.groundMask);
			this._searchRange.enabled = false;
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x000CE8B7 File Offset: 0x000CCAB7
		private void OnEnable()
		{
			this._thunderboltPosition.transform.parent = null;
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x000CE8CA File Offset: 0x000CCACA
		protected override void OnDestroy()
		{
			UnityEngine.Object.Destroy(this._thunderboltPosition.gameObject);
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x000CE8DC File Offset: 0x000CCADC
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x000CE8F0 File Offset: 0x000CCAF0
		public override void Run(Character owner)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
			this._searchRange.enabled = true;
			this._overlapper.OverlapCollider(this._searchRange);
			List<Target> components = this._overlapper.results.GetComponents(true);
			if (components.Count == 0)
			{
				this._overlapper.contactFilter.SetLayerMask(2048);
				this._overlapper.OverlapCollider(this._searchRange);
				components = this._overlapper.results.GetComponents(true);
				if (components.Count == 0)
				{
					this._searchRange.enabled = false;
					return;
				}
			}
			this._searchRange.enabled = false;
			Target target = components.Random<Target>();
			this._groundFinder.origin = target.transform.position;
			RaycastHit2D hit = this._groundFinder.SingleCast();
			if (!hit)
			{
				return;
			}
			this._thunderboltPosition.position = hit.point;
			base.StartCoroutine(this._operations.CRun(owner));
		}

		// Token: 0x04003608 RID: 13832
		[FormerlySerializedAs("_serachRange")]
		[SerializeField]
		private Collider2D _searchRange;

		// Token: 0x04003609 RID: 13833
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x0400360A RID: 13834
		private NonAllocOverlapper _overlapper;

		// Token: 0x0400360B RID: 13835
		private RayCaster _groundFinder;

		// Token: 0x0400360C RID: 13836
		[SerializeField]
		private Transform _thunderboltPosition;

		// Token: 0x0400360D RID: 13837
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;
	}
}
