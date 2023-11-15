using System;
using System.Collections;
using Characters.Operations;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.AI.DarkHero
{
	// Token: 0x02001228 RID: 4648
	public sealed class TripleThorn : MonoBehaviour
	{
		// Token: 0x06005B21 RID: 23329 RVA: 0x0010DCDA File Offset: 0x0010BEDA
		private void Awake()
		{
			this._operationInfos.Initialize();
		}

		// Token: 0x06005B22 RID: 23330 RVA: 0x0010DCE7 File Offset: 0x0010BEE7
		private void OnDestroy()
		{
			this._endClip = null;
		}

		// Token: 0x06005B23 RID: 23331 RVA: 0x0010DCF0 File Offset: 0x0010BEF0
		public void Attack()
		{
			this._operationInfos.gameObject.SetActive(true);
			this._operationInfos.Run(this._owner);
			Animator[] thorns = this._thorns;
			for (int i = 0; i < thorns.Length; i++)
			{
				thorns[i].Play(this.attackHash, 0, 0f);
			}
		}

		// Token: 0x06005B24 RID: 23332 RVA: 0x0010DD48 File Offset: 0x0010BF48
		public void Return()
		{
			this._operationInfos.Stop();
			Animator[] thorns = this._thorns;
			for (int i = 0; i < thorns.Length; i++)
			{
				thorns[i].Play(this.returnHash);
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._endSound, base.transform.position);
			base.StartCoroutine(this.CReturn());
		}

		// Token: 0x06005B25 RID: 23333 RVA: 0x0010DDAC File Offset: 0x0010BFAC
		private IEnumerator CReturn()
		{
			yield return Chronometer.global.WaitForSeconds(this._endClip.length);
			Animator[] thorns = this._thorns;
			for (int i = 0; i < thorns.Length; i++)
			{
				thorns[i].gameObject.SetActive(false);
			}
			yield break;
		}

		// Token: 0x04004991 RID: 18833
		[SerializeField]
		private Character _owner;

		// Token: 0x04004992 RID: 18834
		[SerializeField]
		private Animator[] _thorns;

		// Token: 0x04004993 RID: 18835
		[SerializeField]
		private OperationInfos _operationInfos;

		// Token: 0x04004994 RID: 18836
		[SerializeField]
		private AnimationClip _endClip;

		// Token: 0x04004995 RID: 18837
		[SerializeField]
		protected SoundInfo _endSound;

		// Token: 0x04004996 RID: 18838
		private readonly int attackHash = Animator.StringToHash("Start");

		// Token: 0x04004997 RID: 18839
		private readonly int returnHash = Animator.StringToHash("End");
	}
}
