using System;
using BehaviorDesigner.Runtime;
using Characters;
using Characters.AI;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class GuardColliderController : MonoBehaviour
{
	// Token: 0x06000050 RID: 80 RVA: 0x0000371C File Offset: 0x0000191C
	private Character GetTarget()
	{
		if (this._target == null)
		{
			if (this._ai != null)
			{
				this._target = this._ai.target;
			}
			else
			{
				this._target = this._communicator.GetVariable<SharedCharacter>(this._targetKey).Value;
			}
		}
		return this._target;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x0000377A File Offset: 0x0000197A
	private void Awake()
	{
		if (this._ai != null)
		{
			this._owner = this._ai.character;
			return;
		}
		this._owner = this._communicator.GetVariable<SharedCharacter>(this._ownerKey).Value;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000037B8 File Offset: 0x000019B8
	private void Update()
	{
		if (this.GetTarget() == null)
		{
			return;
		}
		if (this.GetTarget().status.stuned || this.GetTarget().status.unmovable)
		{
			this._collider.gameObject.SetActive(false);
			base.gameObject.SetActive(false);
		}
		if (this._owner.lookingDirection == Character.LookingDirection.Right)
		{
			if (this.GetTarget().transform.position.x < base.transform.position.x)
			{
				this._collider.enabled = false;
				return;
			}
			this._collider.enabled = true;
			return;
		}
		else
		{
			if (this.GetTarget().transform.position.x > base.transform.position.x)
			{
				this._collider.enabled = false;
				return;
			}
			this._collider.enabled = true;
			return;
		}
	}

	// Token: 0x04000054 RID: 84
	[SerializeField]
	private Collider2D _collider;

	// Token: 0x04000055 RID: 85
	[SerializeField]
	private AIController _ai;

	// Token: 0x04000056 RID: 86
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	// Token: 0x04000057 RID: 87
	[SerializeField]
	private string _ownerKey = "Owner";

	// Token: 0x04000058 RID: 88
	[SerializeField]
	private string _targetKey = "Target";

	// Token: 0x04000059 RID: 89
	private Character _owner;

	// Token: 0x0400005A RID: 90
	private Character _target;
}
