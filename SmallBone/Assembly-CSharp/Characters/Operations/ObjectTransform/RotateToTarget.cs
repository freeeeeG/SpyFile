using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FAD RID: 4013
	public class RotateToTarget : CharacterOperation
	{
		// Token: 0x06004DE6 RID: 19942 RVA: 0x000E8FEC File Offset: 0x000E71EC
		public override void Run(Character owner)
		{
			Transform closestTargetTransform = this.GetClosestTargetTransform(owner);
			if (closestTargetTransform == null)
			{
				float z = (owner.lookingDirection == Character.LookingDirection.Right) ? this._defaultRotation : (this._defaultRotation + 180f);
				this._object.rotation = Quaternion.Euler(0f, 0f, z);
				return;
			}
			Vector3 vector = closestTargetTransform.position - this._object.position;
			float z2 = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this._object.rotation = Quaternion.Euler(0f, 0f, z2);
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x000E9090 File Offset: 0x000E7290
		private Transform GetClosestTargetTransform(Character owner)
		{
			LayerMask layerMask = this._targetLayer.Evaluate(owner.gameObject);
			RotateToTarget._overlapper.contactFilter.SetLayerMask(layerMask);
			List<Target> components = RotateToTarget._overlapper.OverlapCollider(this._range).GetComponents<Target>(true);
			if (components.Count == 0)
			{
				return null;
			}
			if (components.Count == 1)
			{
				return components[0].transform;
			}
			float num = float.MaxValue;
			int index = 0;
			for (int i = 1; i < components.Count; i++)
			{
				if (!(components[i].character == null))
				{
					float distance = Physics2D.Distance(components[i].character.collider, owner.collider).distance;
					if (num > distance)
					{
						index = i;
						num = distance;
					}
				}
			}
			return components[index].transform;
		}

		// Token: 0x04003DD2 RID: 15826
		[SerializeField]
		private Transform _object;

		// Token: 0x04003DD3 RID: 15827
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04003DD4 RID: 15828
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x04003DD5 RID: 15829
		[SerializeField]
		private float _defaultRotation;

		// Token: 0x04003DD6 RID: 15830
		private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(15);
	}
}
