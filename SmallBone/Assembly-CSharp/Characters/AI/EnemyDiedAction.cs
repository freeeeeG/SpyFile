using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.AI
{
	// Token: 0x020010F3 RID: 4339
	public class EnemyDiedAction : MonoBehaviour
	{
		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06005446 RID: 21574 RVA: 0x000FC478 File Offset: 0x000FA678
		public Characters.Actions.Action diedAction
		{
			get
			{
				return this._dieAction;
			}
		}

		// Token: 0x06005447 RID: 21575 RVA: 0x000FC480 File Offset: 0x000FA680
		private void Start()
		{
			if (this._aiController == null)
			{
				return;
			}
			this._aiController.character.health.onDiedTryCatch += this.OnDied;
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x000FC4B4 File Offset: 0x000FA6B4
		private void OnDied()
		{
			if (this._run)
			{
				return;
			}
			this._run = true;
			this.ActiveCharacterSprite();
			this._aiController.StopAllCoroutinesWithBehaviour();
			if (this._dieAction != null)
			{
				base.StartCoroutine(this.PlayDieAction());
			}
			this._aiController.character.health.onDiedTryCatch -= this.OnDied;
		}

		// Token: 0x06005449 RID: 21577 RVA: 0x000FC51E File Offset: 0x000FA71E
		private void ActiveCharacterSprite()
		{
			this._aiController.character.collider.enabled = false;
			this._aiController.character.gameObject.SetActive(true);
			this._spriteRenderer.enabled = true;
		}

		// Token: 0x0600544A RID: 21578 RVA: 0x000FC558 File Offset: 0x000FA758
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

		// Token: 0x0600544B RID: 21579 RVA: 0x000FC567 File Offset: 0x000FA767
		private IEnumerator PlayDiedAction()
		{
			while (this._dieAction.running)
			{
				yield return null;
			}
			this._diedAction.TryStart();
			yield break;
		}

		// Token: 0x040043A8 RID: 17320
		[SerializeField]
		private AIController _aiController;

		// Token: 0x040043A9 RID: 17321
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040043AA RID: 17322
		[SerializeField]
		[FormerlySerializedAs("_deadAction")]
		private Characters.Actions.Action _dieAction;

		// Token: 0x040043AB RID: 17323
		[SerializeField]
		private Characters.Actions.Action _diedAction;

		// Token: 0x040043AC RID: 17324
		private bool _run;
	}
}
