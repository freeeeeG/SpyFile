using System;
using System.Collections;
using Characters.Actions;
using Characters.Controllers;
using Scenes;
using UnityEngine;

namespace Tutorial
{
	// Token: 0x020000CE RID: 206
	public class Witch : NPC
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x0000D768 File Offset: 0x0000B968
		protected override void Activate()
		{
			if (this._done)
			{
				return;
			}
			this._done = true;
			PlayerInput.blocked.Attach(this);
			Scene<GameBase>.instance.uiManager.letterBox.Appear(1f);
			base.StartCoroutine(this.Converse());
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00002191 File Offset: 0x00000391
		protected override void Deactivate()
		{
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000D7B6 File Offset: 0x0000B9B6
		private IEnumerator Converse()
		{
			yield return Chronometer.global.WaitForSeconds(2f);
			this._effetctSpawnPoint.position = this._player.transform.position;
			this._messageBox.sprite = this._messages[0];
			yield return Chronometer.global.WaitForSeconds((float)this._duration[0]);
			this._messageBox.sprite = this._messages[1];
			this._attack.TryStart();
			Scene<GameBase>.instance.cameraController.Shake(1.3f, 0.5f);
			yield return Chronometer.global.WaitForSeconds((float)this._duration[1]);
			this._messageBox.sprite = this._messages[2];
			yield return Chronometer.global.WaitForSeconds((float)this._duration[2]);
			this._messageBox.sprite = this._messages[3];
			yield return Chronometer.global.WaitForSeconds((float)this._duration[3]);
			this._messageBox.sprite = this._messages[4];
			yield return Chronometer.global.WaitForSeconds((float)this._duration[4]);
			PlayerInput.blocked.Detach(this);
			Scene<GameBase>.instance.uiManager.letterBox.Disappear(0.5f);
			this._heal.TryStart();
			this._player.health.Heal(9999.0, true);
			yield break;
		}

		// Token: 0x04000311 RID: 785
		[SerializeField]
		private SpriteRenderer _messageBox;

		// Token: 0x04000312 RID: 786
		[SerializeField]
		private int[] _duration;

		// Token: 0x04000313 RID: 787
		[SerializeField]
		private Sprite[] _messages;

		// Token: 0x04000314 RID: 788
		[SerializeField]
		private Transform _effetctSpawnPoint;

		// Token: 0x04000315 RID: 789
		[SerializeField]
		private Characters.Actions.Action _attack;

		// Token: 0x04000316 RID: 790
		[SerializeField]
		private Characters.Actions.Action _heal;

		// Token: 0x04000317 RID: 791
		private bool _done;
	}
}
