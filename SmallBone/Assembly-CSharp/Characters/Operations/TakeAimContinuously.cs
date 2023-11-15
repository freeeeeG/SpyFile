using System;
using System.Collections;
using Characters.AI;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E09 RID: 3593
	public class TakeAimContinuously : CharacterOperation
	{
		// Token: 0x060047C9 RID: 18377 RVA: 0x000D0F21 File Offset: 0x000CF121
		private void Awake()
		{
			this._originalDirection = 0f;
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x000D0F2E File Offset: 0x000CF12E
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x000D0F3E File Offset: 0x000CF13E
		private IEnumerator CRun(Character owner)
		{
			Character character = Singleton<Service>.Instance.levelManager.player;
			if (this._controller != null)
			{
				character = this._controller.target;
			}
			Transform targetTransform = character.transform;
			float targetHalfHeight = character.collider.bounds.extents.y;
			float elapsed = 0f;
			this._stop = false;
			while (!this._stop && elapsed < this._duration)
			{
				yield return null;
				Vector3 vector = new Vector3(targetTransform.position.x, targetTransform.position.y + targetHalfHeight) - this._centerAxisPosition.transform.position;
				float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				this._centerAxisPosition.rotation = Quaternion.Euler(0f, 0f, this._originalDirection + num);
				elapsed += owner.chronometer.master.deltaTime;
			}
			yield break;
		}

		// Token: 0x060047CC RID: 18380 RVA: 0x000D0F54 File Offset: 0x000CF154
		public override void Stop()
		{
			this._stop = true;
		}

		// Token: 0x040036E9 RID: 14057
		[SerializeField]
		private Transform _centerAxisPosition;

		// Token: 0x040036EA RID: 14058
		[SerializeField]
		private AIController _controller;

		// Token: 0x040036EB RID: 14059
		[SerializeField]
		private float _duration;

		// Token: 0x040036EC RID: 14060
		private float _originalDirection;

		// Token: 0x040036ED RID: 14061
		private bool _stop;
	}
}
