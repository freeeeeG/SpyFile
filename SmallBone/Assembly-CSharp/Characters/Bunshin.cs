using System;
using Characters.Actions;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200070C RID: 1804
	[RequireComponent(typeof(Minion))]
	public sealed class Bunshin : MonoBehaviour
	{
		// Token: 0x0600248E RID: 9358 RVA: 0x0006DF9A File Offset: 0x0006C19A
		private void OnEnable()
		{
			this._minion.onSummon += this.AttachEvent;
			this._minion.onUnsummon += this.DetachEvent;
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x0006DFCA File Offset: 0x0006C1CA
		private void AttachEvent(Character owner, Character minion)
		{
			owner.onStartAction += this.StartAction;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x0006DFDE File Offset: 0x0006C1DE
		private void DetachEvent(Character owner, Character minion)
		{
			owner.onStartAction -= this.StartAction;
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x0006DFF4 File Offset: 0x0006C1F4
		private void StartAction(Characters.Actions.Action action)
		{
			switch (action.type)
			{
			case Characters.Actions.Action.Type.Dash:
			case Characters.Actions.Action.Type.JumpAttack:
			case Characters.Actions.Action.Type.Jump:
			case Characters.Actions.Action.Type.Skill:
			case Characters.Actions.Action.Type.Swap:
				break;
			case Characters.Actions.Action.Type.BasicAttack:
				if (this._onBasicAttack != null)
				{
					this._onBasicAttack.TryStart();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x04001F0E RID: 7950
		[GetComponent]
		[SerializeField]
		private Minion _minion;

		// Token: 0x04001F0F RID: 7951
		[SerializeField]
		private Characters.Actions.Action _onBasicAttack;
	}
}
