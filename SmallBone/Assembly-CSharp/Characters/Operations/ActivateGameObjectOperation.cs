using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DA6 RID: 3494
	public class ActivateGameObjectOperation : CharacterOperation
	{
		// Token: 0x0600465D RID: 18013 RVA: 0x000CB4C8 File Offset: 0x000C96C8
		public override void Run(Character owner)
		{
			if (this._gameObject == null)
			{
				this._gameObject = owner.gameObject;
			}
			if (this._deactivate)
			{
				this._gameObject.SetActive(false);
				return;
			}
			if (this._activatedPosition != null)
			{
				this._gameObject.transform.position = this._activatedPosition.position;
			}
			this._gameObject.SetActive(true);
			RuntimeAnimatorController component = this._gameObject.GetComponent<RuntimeAnimatorController>();
			if (component != null && this._duration == 0f)
			{
				this._duration = component.animationClips[0].length;
			}
			if (this._duration > 0f)
			{
				this._stopCoroutineReference = this.StartCoroutineWithReference(this.CStop(owner.chronometer.animation));
			}
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x000CB598 File Offset: 0x000C9798
		private IEnumerator CStop(Chronometer chronometer)
		{
			yield return chronometer.WaitForSeconds(this._duration);
			this.Stop();
			yield break;
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x000CB5AE File Offset: 0x000C97AE
		public override void Stop()
		{
			this._stopCoroutineReference.Stop();
			if (this._gameObject != null)
			{
				this._gameObject.SetActive(false);
			}
		}

		// Token: 0x04003545 RID: 13637
		[SerializeField]
		private GameObject _gameObject;

		// Token: 0x04003546 RID: 13638
		[SerializeField]
		private float _duration;

		// Token: 0x04003547 RID: 13639
		[SerializeField]
		private bool _deactivate;

		// Token: 0x04003548 RID: 13640
		[SerializeField]
		private Transform _activatedPosition;

		// Token: 0x04003549 RID: 13641
		private CoroutineReference _stopCoroutineReference;
	}
}
