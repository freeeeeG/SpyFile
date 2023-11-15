using System;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x0200113F RID: 4415
	public class WoodPlatform : MonoBehaviour
	{
		// Token: 0x060055EB RID: 21995 RVA: 0x00100023 File Offset: 0x000FE223
		private void Awake()
		{
			this._spriteRenderer.sprite = null;
			this._collider.enabled = false;
			this._animator.Play("Empty");
		}

		// Token: 0x060055EC RID: 21996 RVA: 0x0010004D File Offset: 0x000FE24D
		public void Spawn()
		{
			this._animator.Play("Appearance");
			this._collider.enabled = true;
		}

		// Token: 0x060055ED RID: 21997 RVA: 0x0010006B File Offset: 0x000FE26B
		public void Despawn()
		{
			this._animator.Play("Disappearance");
			this._collider.enabled = false;
		}

		// Token: 0x040044DC RID: 17628
		[GetComponent]
		[SerializeField]
		private Animator _animator;

		// Token: 0x040044DD RID: 17629
		[SerializeField]
		[GetComponent]
		private Collider2D _collider;

		// Token: 0x040044DE RID: 17630
		[GetComponent]
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040044DF RID: 17631
		private bool _spawned;
	}
}
