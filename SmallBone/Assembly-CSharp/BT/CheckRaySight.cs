using System;
using BT.SharedValues;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BT
{
	// Token: 0x02001414 RID: 5140
	public sealed class CheckRaySight : Node
	{
		// Token: 0x06006514 RID: 25876 RVA: 0x001248D4 File Offset: 0x00122AD4
		protected override void OnInitialize()
		{
			this._rightLineRaycaster = new LineSequenceNonAllocCaster(this._rayCount, this._rayCount)
			{
				caster = new RayCaster
				{
					direction = Vector2.right
				}
			};
			this._leftLineRaycaster = new LineSequenceNonAllocCaster(this._rayCount, this._rayCount)
			{
				caster = new RayCaster
				{
					direction = Vector2.left
				}
			};
		}

		// Token: 0x06006515 RID: 25877 RVA: 0x0012493C File Offset: 0x00122B3C
		protected override NodeState UpdateDeltatime(Context context)
		{
			if (this._owner == null)
			{
				this._owner = context.Get<Character>(Key.OwnerCharacter);
			}
			Character character = this.FindTarget();
			if (character == null)
			{
				return NodeState.Fail;
			}
			context.Set<Character>(Key.Target, new SharedValue<Character>(character));
			return NodeState.Success;
		}

		// Token: 0x06006516 RID: 25878 RVA: 0x0012498C File Offset: 0x00122B8C
		private Character FindTarget()
		{
			this.SetBounds();
			Character character = this.CheckRight();
			if (character != null)
			{
				return character;
			}
			return this.CheckLeft();
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x001249B8 File Offset: 0x00122BB8
		private void SetBounds()
		{
			Bounds bounds = this._rayBounds.bounds;
			this._leftLineRaycaster.start = new Vector2(bounds.min.x, bounds.min.y);
			this._leftLineRaycaster.end = new Vector2(bounds.min.x, bounds.max.y);
			this._rightLineRaycaster.start = new Vector2(bounds.max.x, bounds.min.y);
			this._rightLineRaycaster.end = new Vector2(bounds.max.x, bounds.max.y);
		}

		// Token: 0x06006518 RID: 25880 RVA: 0x00124A74 File Offset: 0x00122C74
		private Character CheckRight()
		{
			LineSequenceNonAllocCaster rightLineRaycaster = this._rightLineRaycaster;
			rightLineRaycaster.caster.contactFilter.SetLayerMask(this._targetLayer.Evaluate(this._owner.gameObject));
			rightLineRaycaster.caster.origin = Vector2.zero;
			rightLineRaycaster.caster.distance = ((this._owner.lookingDirection == Character.LookingDirection.Right) ? this._rightRayDistance : this._leftRayDistance);
			rightLineRaycaster.Cast();
			for (int i = 0; i < rightLineRaycaster.nonAllocCasters.Count; i++)
			{
				ReadonlyBoundedList<RaycastHit2D> results = rightLineRaycaster.nonAllocCasters[i].results;
				if (results.Count != 0)
				{
					Target component = results[0].collider.GetComponent<Target>();
					if (!(component == null) && !(component.character == null) && this._characterTypeFilter[component.character.type])
					{
						return component.character;
					}
				}
			}
			return null;
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x00124B68 File Offset: 0x00122D68
		private Character CheckLeft()
		{
			LineSequenceNonAllocCaster leftLineRaycaster = this._leftLineRaycaster;
			leftLineRaycaster.caster.contactFilter.SetLayerMask(this._targetLayer.Evaluate(this._owner.gameObject));
			leftLineRaycaster.caster.origin = Vector2.zero;
			leftLineRaycaster.caster.distance = ((this._owner.lookingDirection == Character.LookingDirection.Right) ? this._leftRayDistance : this._rightRayDistance);
			leftLineRaycaster.Cast();
			for (int i = 0; i < leftLineRaycaster.nonAllocCasters.Count; i++)
			{
				ReadonlyBoundedList<RaycastHit2D> results = leftLineRaycaster.nonAllocCasters[i].results;
				if (results.Count != 0)
				{
					Target component = results[0].collider.GetComponent<Target>();
					if (!(component == null) && !(component.character == null) && this._characterTypeFilter[component.character.type])
					{
						return component.character;
					}
				}
			}
			return null;
		}

		// Token: 0x0400515F RID: 20831
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter;

		// Token: 0x04005160 RID: 20832
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x04005161 RID: 20833
		[SerializeField]
		private Collider2D _rayBounds;

		// Token: 0x04005162 RID: 20834
		[SerializeField]
		private float _rightRayDistance;

		// Token: 0x04005163 RID: 20835
		[SerializeField]
		private float _leftRayDistance;

		// Token: 0x04005164 RID: 20836
		[SerializeField]
		[Range(1f, 10f)]
		private int _rayCount;

		// Token: 0x04005165 RID: 20837
		private Character _owner;

		// Token: 0x04005166 RID: 20838
		private LineSequenceNonAllocCaster _rightLineRaycaster;

		// Token: 0x04005167 RID: 20839
		private LineSequenceNonAllocCaster _leftLineRaycaster;
	}
}
