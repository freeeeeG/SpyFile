using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000093 RID: 147
	public class EnemyAreaProjectile : MonoBehaviour
	{
		// Token: 0x06000553 RID: 1363 RVA: 0x00019D88 File Offset: 0x00017F88
		public void TargetPos(Vector2 pos, float duration)
		{
			this.lobTransform.LeanMoveLocalY(2f, duration / 2f).setLoopPingPong(1).setEase(LeanTweenType.easeOutCubic);
			this.lobTransform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
			LeanTween.move(base.gameObject, pos, duration).setOnComplete(new Action(this.OnTargetReached));
			this._indicator = Object.Instantiate<GameObject>(this.indicatorPrefab);
			this._indicator.transform.position = pos;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00019E28 File Offset: 0x00018028
		private void OnTargetReached()
		{
			SoundEffectSO soundEffectSO = this.hitSFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			Object.Instantiate<GameObject>(this.damagePrefab).transform.position = this._indicator.transform.position;
			if (this._indicator)
			{
				Object.Destroy(this._indicator);
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x04000340 RID: 832
		[SerializeField]
		private Transform lobTransform;

		// Token: 0x04000341 RID: 833
		[SerializeField]
		private GameObject indicatorPrefab;

		// Token: 0x04000342 RID: 834
		[SerializeField]
		private GameObject damagePrefab;

		// Token: 0x04000343 RID: 835
		[SerializeField]
		private SoundEffectSO hitSFX;

		// Token: 0x04000344 RID: 836
		private GameObject _indicator;
	}
}
