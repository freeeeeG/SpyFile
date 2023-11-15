using System;
using Characters;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000661 RID: 1633
	public class IronDoor : ControlableTrap
	{
		// Token: 0x060020C4 RID: 8388 RVA: 0x00062FBC File Offset: 0x000611BC
		private void Awake()
		{
			this._hitOperations.Initialize();
			this._character.health.onDied += this.Run;
			this._character.gameObject.SetActive(false);
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x00062FF6 File Offset: 0x000611F6
		private void Run()
		{
			this._character.health.onDied -= this.Run;
			base.StartCoroutine(this._hitOperations.CRun(this._character));
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x0006302C File Offset: 0x0006122C
		public override void Activate()
		{
			if (base.activated)
			{
				return;
			}
			this._character.CancelAction();
			this._character.gameObject.SetActive(true);
			this._blockCollider.enabled = true;
			this._downAction.TryStart();
			base.activated = true;
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x00063080 File Offset: 0x00061280
		public override void Deactivate()
		{
			if (!base.activated)
			{
				return;
			}
			if (!this._character.health.dead)
			{
				this._character.CancelAction();
				this._upAction.TryStart();
				this._blockCollider.enabled = false;
				base.activated = false;
			}
		}

		// Token: 0x04001BD3 RID: 7123
		[SerializeField]
		private Character _character;

		// Token: 0x04001BD4 RID: 7124
		[SerializeField]
		private Collider2D _blockCollider;

		// Token: 0x04001BD5 RID: 7125
		[SerializeField]
		private Characters.Actions.Action _downAction;

		// Token: 0x04001BD6 RID: 7126
		[SerializeField]
		private Characters.Actions.Action _upAction;

		// Token: 0x04001BD7 RID: 7127
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _hitOperations;
	}
}
