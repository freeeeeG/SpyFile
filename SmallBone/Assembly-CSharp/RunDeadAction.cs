using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using Characters;
using Characters.Actions;
using Characters.Operations;
using Runnables;
using UnityEngine;

// Token: 0x0200007C RID: 124
public sealed class RunDeadAction : Runnable
{
	// Token: 0x0600023F RID: 575 RVA: 0x0000992C File Offset: 0x00007B2C
	public override void Run()
	{
		this._character.gameObject.SetActive(true);
		this._character.collider.enabled = false;
		if (this._behaviorTree != null)
		{
			this._behaviorTree.enabled = false;
		}
		this._character.status.RemoveAllStatus();
		this._character.invulnerable.Attach(this);
		this._character.CancelAction();
		if (this._deadAction.TryStart())
		{
			if (this._disableOnActionEnd)
			{
				base.StartCoroutine(this.CDisableOnActionEnd());
				return;
			}
		}
		else
		{
			if (this._whenFailToDeadAction == null)
			{
				return;
			}
			this._whenFailToDeadAction.Initialize();
			this._whenFailToDeadAction.Run(this._character);
		}
	}

	// Token: 0x06000240 RID: 576 RVA: 0x000099EF File Offset: 0x00007BEF
	private IEnumerator CDisableOnActionEnd()
	{
		while (this._deadAction.running)
		{
			yield return null;
		}
		this._character.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x040001F3 RID: 499
	[SerializeField]
	private Character _character;

	// Token: 0x040001F4 RID: 500
	[SerializeField]
	private BehaviorTree _behaviorTree;

	// Token: 0x040001F5 RID: 501
	[SerializeField]
	private Characters.Actions.Action _deadAction;

	// Token: 0x040001F6 RID: 502
	[SerializeField]
	private OperationInfos _whenFailToDeadAction;

	// Token: 0x040001F7 RID: 503
	[SerializeField]
	private bool _disableOnActionEnd = true;
}
