using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001038 RID: 4152
	public class BDEnemyDiedAction : MonoBehaviour
	{
		// Token: 0x06005014 RID: 20500 RVA: 0x000F1933 File Offset: 0x000EFB33
		private void Start()
		{
			if (this._behaviorTree == null)
			{
				return;
			}
			this._character.health.onDiedTryCatch += this.OnDied;
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x000F1960 File Offset: 0x000EFB60
		private void OnDied()
		{
			if (this._run)
			{
				return;
			}
			this._run = true;
			this.ActiveCharacterSprite();
			this._behaviorTree.StopAllCoroutines();
			this._behaviorTree.StopAllTaskCoroutines();
			this._behaviorTree.enabled = false;
			if (this._dieAction != null)
			{
				base.StartCoroutine(this.PlayDieAction());
			}
			this._character.health.onDiedTryCatch -= this.OnDied;
		}

		// Token: 0x06005016 RID: 20502 RVA: 0x000F19DC File Offset: 0x000EFBDC
		private void ActiveCharacterSprite()
		{
			this._character.collider.enabled = false;
			this._character.gameObject.SetActive(true);
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x000F1A00 File Offset: 0x000EFC00
		private IEnumerator PlayDieAction()
		{
			bool flag = this._dieAction.TryStart();
			while (!flag)
			{
				yield return null;
				flag = this._dieAction.TryStart();
			}
			if (this._diedAction != null)
			{
				base.StartCoroutine(this.PlayDiedAction());
			}
			yield break;
		}

		// Token: 0x06005018 RID: 20504 RVA: 0x000F1A0F File Offset: 0x000EFC0F
		private IEnumerator PlayDiedAction()
		{
			while (this._dieAction.running)
			{
				yield return null;
			}
			this._diedAction.TryStart();
			yield break;
		}

		// Token: 0x0400406A RID: 16490
		[SerializeField]
		private BehaviorTree _behaviorTree;

		// Token: 0x0400406B RID: 16491
		[SerializeField]
		private Character _character;

		// Token: 0x0400406C RID: 16492
		[SerializeField]
		private Characters.Actions.Action _dieAction;

		// Token: 0x0400406D RID: 16493
		[SerializeField]
		private Characters.Actions.Action _diedAction;

		// Token: 0x0400406E RID: 16494
		private bool _run;
	}
}
