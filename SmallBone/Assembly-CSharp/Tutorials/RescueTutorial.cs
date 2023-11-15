using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using FX;
using Level;
using Scenes;
using Singletons;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000E4 RID: 228
	public class RescueTutorial : Tutorial
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x0000E94F File Offset: 0x0000CB4F
		protected override void Start()
		{
			base.Start();
			this._wave.onClear += delegate()
			{
				TextMessageInfo.Message message = this._cageNextSkeletonTextMessage.messages[0];
				this._cage.collider.enabled = true;
			};
			this._cage.onDestroyed += delegate()
			{
				this._cageNextSkeleton.Play("Dead");
				TextMessageInfo.Message message = this._cageNextSkeletonTextMessage.messages[1];
				base.StartCoroutine(this.<Start>g__EscapeCage|8_2());
			};
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000E985 File Offset: 0x0000CB85
		protected override IEnumerator Process()
		{
			yield return base.MoveTo(this._conversationPoint.position);
			this._player.lookingDirection = Character.LookingDirection.Right;
			for (int i = 0; i < this._messageInfo.messages.Length - 1; i++)
			{
				Scene<GameBase>.instance.uiManager.npcConversation.Done();
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._soundInfo, this._witch.transform.position);
			yield return this._witch.TurnIntoCat();
			this._witch.gameObject.SetActive(false);
			TextMessageInfo.Message message = this._messageInfo.messages[this._messageInfo.messages.Length - 1];
			UnityEngine.Object.Destroy(this._lineText.gameObject);
			this.Deactivate();
			yield break;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000E9E2 File Offset: 0x0000CBE2
		[CompilerGenerated]
		private IEnumerator <Start>g__EscapeCage|8_2()
		{
			this.Activate();
			yield return this._witch.EscapeCage();
			yield break;
		}

		// Token: 0x0400035F RID: 863
		[SerializeField]
		private EnemyWave _wave;

		// Token: 0x04000360 RID: 864
		[SerializeField]
		private Cage _cage;

		// Token: 0x04000361 RID: 865
		[SerializeField]
		private Transform _conversationPoint;

		// Token: 0x04000362 RID: 866
		[SerializeField]
		private Witch _witch;

		// Token: 0x04000363 RID: 867
		[SerializeField]
		private SoundInfo _soundInfo;

		// Token: 0x04000364 RID: 868
		[SerializeField]
		private Animator _cageNextSkeleton;

		// Token: 0x04000365 RID: 869
		[SerializeField]
		private LineText _cageNextSkeletonLineText;

		// Token: 0x04000366 RID: 870
		[SerializeField]
		private TextMessageInfo _cageNextSkeletonTextMessage;
	}
}
