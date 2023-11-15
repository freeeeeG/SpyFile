using System;
using System.Collections;
using Characters.Controllers;
using Scenes;
using UnityEngine;

namespace Tutorial
{
	// Token: 0x020000BB RID: 187
	public class DeadCastleWitch : NPC
	{
		// Token: 0x0600039B RID: 923 RVA: 0x0000D0B4 File Offset: 0x0000B2B4
		protected override void Activate()
		{
			if (this._done)
			{
				return;
			}
			this._done = true;
			PlayerInput.blocked.Attach(this);
			Scene<GameBase>.instance.uiManager.letterBox.Appear(1.5f);
			base.StartCoroutine(this.Converse());
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000D102 File Offset: 0x0000B302
		protected override void Deactivate()
		{
			PlayerInput.blocked.Detach(this);
			Scene<GameBase>.instance.uiManager.letterBox.Disappear(0.5f);
			this._messageBox.sprite = null;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000D135 File Offset: 0x0000B335
		private IEnumerator Converse()
		{
			yield return Chronometer.global.WaitForSeconds(3f);
			this._messageBox.sprite = this._messages[0];
			yield return this.Skip((float)this._duration[0]);
			this._messageBox.sprite = this._messages[1];
			yield return this.Skip((float)this._duration[1]);
			this._messageBox.sprite = this._messages[2];
			yield return this.Skip((float)this._duration[2]);
			PlayerInput.blocked.Detach(this);
			Scene<GameBase>.instance.uiManager.letterBox.Disappear(0.5f);
			this._messageBox.sprite = null;
			yield break;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000D144 File Offset: 0x0000B344
		private IEnumerator Skip(float duration)
		{
			float elapsed = 0f;
			while (!Input.anyKeyDown)
			{
				yield return null;
				elapsed += Chronometer.global.deltaTime;
				if (elapsed > duration)
				{
					break;
				}
			}
			yield break;
		}

		// Token: 0x040002DC RID: 732
		[SerializeField]
		private SpriteRenderer _messageBox;

		// Token: 0x040002DD RID: 733
		[SerializeField]
		private int[] _duration;

		// Token: 0x040002DE RID: 734
		[SerializeField]
		private Sprite[] _messages;

		// Token: 0x040002DF RID: 735
		private bool _done;
	}
}
