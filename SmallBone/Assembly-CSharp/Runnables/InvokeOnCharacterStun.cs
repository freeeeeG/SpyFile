using System;
using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002C4 RID: 708
	public class InvokeOnCharacterStun : MonoBehaviour
	{
		// Token: 0x06000E55 RID: 3669 RVA: 0x0002D219 File Offset: 0x0002B419
		public void Initialize(IStatusEvent statusEvent)
		{
			if (this._owner == null)
			{
				this._owner = base.GetComponentInParent<Character>();
			}
			this._statusEvent = statusEvent;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0002D23C File Offset: 0x0002B43C
		private void Awake()
		{
			this._owner.status.stun.onAttachEvents += this.ApplyStatusEvent;
			this._owner.status.stun.onDetachEvents += this.ReleaseStatusEvent;
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0002D28C File Offset: 0x0002B48C
		private void OnDestroy()
		{
			this._owner.status.stun.onAttachEvents -= this.ApplyStatusEvent;
			this._owner.status.stun.onDetachEvents -= this.ReleaseStatusEvent;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0002D2DB File Offset: 0x0002B4DB
		private void ApplyStatusEvent(Character owner, Character target)
		{
			if (this._trigger.IsSatisfied())
			{
				this._statusEvent.Apply(owner, target);
			}
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0002D2F7 File Offset: 0x0002B4F7
		private void ReleaseStatusEvent(Character owner, Character target)
		{
			if (this._trigger.IsSatisfied())
			{
				this._statusEvent.Release(owner, target);
			}
		}

		// Token: 0x04000BEB RID: 3051
		[Trigger.SubcomponentAttribute]
		[SerializeField]
		private Trigger _trigger;

		// Token: 0x04000BEC RID: 3052
		[SerializeField]
		private Character _owner;

		// Token: 0x04000BED RID: 3053
		[SubclassSelector]
		[SerializeReference]
		private IStatusEvent _statusEvent;
	}
}
