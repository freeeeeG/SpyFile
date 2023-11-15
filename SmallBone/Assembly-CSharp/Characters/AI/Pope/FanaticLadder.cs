using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x0200120C RID: 4620
	public sealed class FanaticLadder : MonoBehaviour
	{
		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06005A9F RID: 23199 RVA: 0x0010CB60 File Offset: 0x0010AD60
		public Vector3 spawnPoint
		{
			get
			{
				return this._spawnPoint.position;
			}
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x0010CB6D File Offset: 0x0010AD6D
		// (set) Token: 0x06005AA1 RID: 23201 RVA: 0x0010CB75 File Offset: 0x0010AD75
		public FanaticFactory.SummonType fanatic { get; set; }

		// Token: 0x06005AA2 RID: 23202 RVA: 0x0010CB80 File Offset: 0x0010AD80
		private void Awake()
		{
			this._animations = new Dictionary<FanaticFactory.SummonType, FanaticLadder.LadderAnimationInfo>(this._infos.Length);
			foreach (FanaticLadder.LadderAnimationInfo ladderAnimationInfo in this._infos)
			{
				if (this._animations.ContainsKey(ladderAnimationInfo.tag))
				{
					Debug.LogError(string.Format("An item with the same key has alread been added. Key:{0}", ladderAnimationInfo.tag));
					return;
				}
				this._animations.Add(ladderAnimationInfo.tag, ladderAnimationInfo);
			}
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x0010CBFC File Offset: 0x0010ADFC
		private void OnDestroy()
		{
			this._ladderAnimationController = null;
			this._introClip = null;
			this._outroClip = null;
			this._fallClip = null;
			FanaticLadder.LadderAnimationInfo[] infos = this._infos;
			for (int i = 0; i < infos.Length; i++)
			{
				infos[i].Dispose();
			}
			this._infos = null;
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x0010CC49 File Offset: 0x0010AE49
		public void SetFanatic(FanaticFactory.SummonType who)
		{
			this.fanatic = who;
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x0010CC52 File Offset: 0x0010AE52
		public IEnumerator CClimb()
		{
			this._ladderAnimationController.gameObject.SetActive(true);
			this._ladderAnimationController.Play("Intro", 0, 0f);
			yield return this._animations[this.fanatic].CRun();
			base.StartCoroutine(this.CHide());
			yield break;
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x0010CC61 File Offset: 0x0010AE61
		private IEnumerator CHide()
		{
			this._ladderAnimationController.Play("Outtro", 0, 0f);
			yield return Chronometer.global.WaitForSeconds(this._outroClip.length);
			this._ladderAnimationController.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x0010CC70 File Offset: 0x0010AE70
		public IEnumerator CFall()
		{
			yield return this._animations[this.fanatic].CFall(base.transform);
			this._ladderAnimationController.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0400492C RID: 18732
		[SerializeField]
		private Transform _spawnPoint;

		// Token: 0x0400492D RID: 18733
		[SerializeField]
		private FanaticLadder.LadderAnimationInfo[] _infos;

		// Token: 0x0400492E RID: 18734
		[SerializeField]
		private Animator _ladderAnimationController;

		// Token: 0x0400492F RID: 18735
		[SerializeField]
		private AnimationClip _introClip;

		// Token: 0x04004930 RID: 18736
		[SerializeField]
		private AnimationClip _outroClip;

		// Token: 0x04004931 RID: 18737
		[SerializeField]
		private AnimationClip _fallClip;

		// Token: 0x04004932 RID: 18738
		private Dictionary<FanaticFactory.SummonType, FanaticLadder.LadderAnimationInfo> _animations;

		// Token: 0x0200120D RID: 4621
		[Serializable]
		private class LadderAnimationInfo
		{
			// Token: 0x17001213 RID: 4627
			// (get) Token: 0x06005AA9 RID: 23209 RVA: 0x0010CC7F File Offset: 0x0010AE7F
			internal FanaticFactory.SummonType tag
			{
				get
				{
					return this._tag;
				}
			}

			// Token: 0x06005AAA RID: 23210 RVA: 0x0010CC87 File Offset: 0x0010AE87
			internal void Active()
			{
				this._object.SetActive(true);
			}

			// Token: 0x06005AAB RID: 23211 RVA: 0x0010CC95 File Offset: 0x0010AE95
			internal void Deactive()
			{
				this._object.SetActive(false);
			}

			// Token: 0x06005AAC RID: 23212 RVA: 0x0010CCA3 File Offset: 0x0010AEA3
			internal IEnumerator CRun()
			{
				this.Active();
				yield return Chronometer.global.WaitForSeconds(this._duration);
				this.Deactive();
				yield break;
			}

			// Token: 0x06005AAD RID: 23213 RVA: 0x0010CCB2 File Offset: 0x0010AEB2
			internal IEnumerator CFall(Transform transform)
			{
				this._object.GetComponent<Animator>().speed = 0f;
				float elapsed = 0f;
				float speed = 6f;
				while (elapsed < 0.4f)
				{
					transform.Translate(Vector2.up * speed * elapsed * Chronometer.global.deltaTime);
					elapsed += Chronometer.global.deltaTime;
					speed -= Chronometer.global.deltaTime;
					yield return null;
				}
				elapsed = 0f;
				while (elapsed < 3f)
				{
					transform.Translate(Vector2.down * 5f * (1f + elapsed) * Chronometer.global.deltaTime);
					elapsed += Chronometer.global.deltaTime;
					yield return null;
				}
				yield break;
			}

			// Token: 0x06005AAE RID: 23214 RVA: 0x0010CCC8 File Offset: 0x0010AEC8
			internal void Dispose()
			{
				this._object = null;
			}

			// Token: 0x04004934 RID: 18740
			[SerializeField]
			private FanaticFactory.SummonType _tag;

			// Token: 0x04004935 RID: 18741
			[SerializeField]
			private GameObject _object;

			// Token: 0x04004936 RID: 18742
			[SerializeField]
			private float _duration;
		}
	}
}
