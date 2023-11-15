using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012E8 RID: 4840
	public class Hide : Behaviour
	{
		// Token: 0x06005FB5 RID: 24501 RVA: 0x00118415 File Offset: 0x00116615
		public override IEnumerator CRun(AIController controller)
		{
			float seconds = UnityEngine.Random.Range(this._duration.x, this._duration.y);
			this._spriteRenderer.enabled = false;
			if (this._collider2D != null)
			{
				this._collider2D.enabled = false;
			}
			controller.character.attach.SetActive(false);
			yield return controller.character.chronometer.master.WaitForSeconds(seconds);
			this._spriteRenderer.enabled = true;
			if (this._collider2D != null)
			{
				this._collider2D.enabled = true;
			}
			controller.character.attach.SetActive(true);
			yield break;
		}

		// Token: 0x04004CF8 RID: 19704
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04004CF9 RID: 19705
		[SerializeField]
		private Collider2D _collider2D;

		// Token: 0x04004CFA RID: 19706
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2 _duration;
	}
}
