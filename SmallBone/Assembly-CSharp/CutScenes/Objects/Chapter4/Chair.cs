using System;
using Characters.Operations;
using Level.Chapter4;
using UnityEngine;

namespace CutScenes.Objects.Chapter4
{
	// Token: 0x02000221 RID: 545
	[RequireComponent(typeof(SpriteRenderer))]
	public class Chair : MonoBehaviour
	{
		// Token: 0x06000AC1 RID: 2753 RVA: 0x00002191 File Offset: 0x00000391
		private void Awake()
		{
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0001D3C3 File Offset: 0x0001B5C3
		public void Hide()
		{
			this._animator.speed = 0.4f;
			this._animator.Play("down");
		}

		// Token: 0x040008C8 RID: 2248
		[SerializeField]
		private OperationInfo Operations;

		// Token: 0x040008C9 RID: 2249
		[GetComponent]
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x040008CA RID: 2250
		[GetComponent]
		[SerializeField]
		private Animator _animator;

		// Token: 0x040008CB RID: 2251
		[SerializeField]
		private Scenario _scenario;
	}
}
