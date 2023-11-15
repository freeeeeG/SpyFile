using System;
using Characters;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.ReactiveProps
{
	// Token: 0x0200056A RID: 1386
	public sealed class DarkMirrorDemonCastleHanger : MonoBehaviour
	{
		// Token: 0x06001B3B RID: 6971 RVA: 0x000549C0 File Offset: 0x00052BC0
		private void Awake()
		{
			this._operations.Initialize();
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000549D0 File Offset: 0x00052BD0
		private void OnTriggerStay2D(Collider2D collision)
		{
			if (this._broken)
			{
				return;
			}
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			if (component.movement.velocity.y > 0f)
			{
				return;
			}
			foreach (Characters.Actions.Action action in component.actions)
			{
				if (action.running && action.type == Characters.Actions.Action.Type.JumpAttack && !(action.GetComponent<PowerbombAction>() == null))
				{
					this.Break(component);
				}
			}
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x00054A74 File Offset: 0x00052C74
		private void Break(Character owner)
		{
			this._broken = true;
			Map.Instance.StartCoroutine(this._operations.CRun(owner));
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x04001766 RID: 5990
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04001767 RID: 5991
		private bool _broken;
	}
}
