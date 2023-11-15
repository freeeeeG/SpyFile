using System;
using System.Collections;
using Characters;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x020005BC RID: 1468
	public class RescuableNpc : InteractiveObject
	{
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x0005946A File Offset: 0x0005766A
		private string displayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/name", this._npcType));
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001D26 RID: 7462 RVA: 0x00059486 File Offset: 0x00057686
		private string[][] rescuedScripts
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/rescue/rescued", this._npcType));
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001D27 RID: 7463 RVA: 0x000594A2 File Offset: 0x000576A2
		private string[][] chatScripts
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/rescue/chat", this._npcType));
			}
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x000594C0 File Offset: 0x000576C0
		protected override void Awake()
		{
			base.Awake();
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			this._npcConversation.name = this.displayName;
			this._npcConversation.skippable = true;
			this._npcConversation.portrait = this._portrait;
			this._animator.Play(RescuableNpc._idleCageHash, 0, 0f);
			this._interactRange.enabled = false;
			this._cage.onDestroyed += delegate()
			{
				this._interactRange.enabled = true;
				GameData.Progress.SetRescued(this._npcType, true);
				this._animator.Play(RescuableNpc._idleHash, 0, 0f);
				base.StartCoroutine(this.CRescueConversation());
			};
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x0005954F File Offset: 0x0005774F
		private void OnDisable()
		{
			this._npcConversation.portrait = null;
			LetterBox.instance.visible = false;
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x00059568 File Offset: 0x00057768
		public override void InteractWith(Character character)
		{
			base.StartCoroutine(this.CChat());
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x00059577 File Offset: 0x00057777
		private IEnumerator CChat()
		{
			LetterBox.instance.Appear(0.4f);
			yield return this._npcConversation.CConversation(this.chatScripts.Random<string[]>());
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x00059586 File Offset: 0x00057786
		private IEnumerator CRescueConversation()
		{
			LetterBox.instance.Appear(0.4f);
			yield return this.MoveTo(this._conversationPosition.position);
			yield return this._npcConversation.CConversation(this.rescuedScripts.Random<string[]>());
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x00059595 File Offset: 0x00057795
		private IEnumerator MoveTo(Vector3 destination)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.ForceToLookAt(base.transform.position.x);
			for (;;)
			{
				float num = destination.x - player.transform.position.x;
				if (Mathf.Abs(num) < 0.1f)
				{
					break;
				}
				Vector2 move = (num > 0f) ? Vector2.right : Vector2.left;
				player.movement.move = move;
				yield return null;
			}
			player.ForceToLookAt(Character.LookingDirection.Right);
			yield break;
		}

		// Token: 0x040018B8 RID: 6328
		protected static readonly int _idleHash = Animator.StringToHash("Idle");

		// Token: 0x040018B9 RID: 6329
		protected static readonly int _idleCageHash = Animator.StringToHash("Idle_Cage");

		// Token: 0x040018BA RID: 6330
		[SerializeField]
		private NpcType _npcType;

		// Token: 0x040018BB RID: 6331
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x040018BC RID: 6332
		[SerializeField]
		private Animator _animator;

		// Token: 0x040018BD RID: 6333
		[SerializeField]
		private Transform _conversationPosition;

		// Token: 0x040018BE RID: 6334
		[SerializeField]
		private Cage _cage;

		// Token: 0x040018BF RID: 6335
		[SerializeField]
		private Collider2D _interactRange;

		// Token: 0x040018C0 RID: 6336
		private NpcConversation _npcConversation;
	}
}
