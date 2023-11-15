using System;
using System.Collections;
using Characters;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000E9 RID: 233
	public abstract class Tutorial : MonoBehaviour
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0000ED35 File Offset: 0x0000CF35
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x0000ED3D File Offset: 0x0000CF3D
		protected Tutorial.State state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000ED46 File Offset: 0x0000CF46
		protected virtual void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000ED74 File Offset: 0x0000CF74
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (this._startCondition != Tutorial.StartCondition.EnterZone)
			{
				return;
			}
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			if (component != this._player)
			{
				return;
			}
			if (this._state != Tutorial.State.Wait)
			{
				return;
			}
			this.Activate();
		}

		// Token: 0x06000481 RID: 1153
		protected abstract IEnumerator Process();

		// Token: 0x06000482 RID: 1154 RVA: 0x0000EDB9 File Offset: 0x0000CFB9
		public virtual void Activate()
		{
			this._state = Tutorial.State.Progress;
			LetterBox.instance.Appear(0.4f);
			base.StartCoroutine(this.Process());
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000EDDE File Offset: 0x0000CFDE
		public virtual void Deactivate()
		{
			this.state = Tutorial.State.Done;
			this._npcConversation.Done();
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000EE01 File Offset: 0x0000D001
		protected IEnumerator CDeactivate()
		{
			yield return Scene<GameBase>.instance.uiManager.letterBox.CDisappear(0.4f);
			yield break;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000EE09 File Offset: 0x0000D009
		protected virtual IEnumerator Converse(float delay = 2f)
		{
			yield return null;
			yield break;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000EE11 File Offset: 0x0000D011
		protected virtual IEnumerator CConversation(TextMessageInfo message)
		{
			string nameKey = message.nameKey;
			string messageKey = message.messageKey;
			this._npcConversation.name = Localization.GetLocalizedString(nameKey);
			this._npcConversation.portrait = null;
			this._npcConversation.skippable = true;
			yield return this._npcConversation.CConversation(new string[]
			{
				Localization.GetLocalizedString(messageKey)
			});
			yield break;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000EE27 File Offset: 0x0000D027
		protected IEnumerator MoveTo(Vector3 destination)
		{
			for (;;)
			{
				float num = destination.x - this._player.transform.position.x;
				if (Mathf.Abs(num) < 0.1f)
				{
					break;
				}
				Vector2 move = (num > 0f) ? Vector2.right : Vector2.left;
				this._player.movement.move = move;
				yield return null;
			}
			this._player.transform.position = destination;
			yield break;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000EE3D File Offset: 0x0000D03D
		protected virtual void OnDisable()
		{
			NpcConversation npcConversation = this._npcConversation;
			if (npcConversation != null)
			{
				npcConversation.Done();
			}
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x04000374 RID: 884
		[SerializeField]
		protected TextMessageInfo _messageInfo;

		// Token: 0x04000375 RID: 885
		[SerializeField]
		protected LineText _lineText;

		// Token: 0x04000376 RID: 886
		protected Character _player;

		// Token: 0x04000377 RID: 887
		protected Tutorial.State _state;

		// Token: 0x04000378 RID: 888
		[SerializeField]
		protected Tutorial.StartCondition _startCondition;

		// Token: 0x04000379 RID: 889
		protected NpcConversation _npcConversation;

		// Token: 0x0400037A RID: 890
		protected string _displayNameKey;

		// Token: 0x020000EA RID: 234
		protected enum State
		{
			// Token: 0x0400037C RID: 892
			Wait,
			// Token: 0x0400037D RID: 893
			Progress,
			// Token: 0x0400037E RID: 894
			Done
		}

		// Token: 0x020000EB RID: 235
		protected enum StartCondition
		{
			// Token: 0x04000380 RID: 896
			EnterZone,
			// Token: 0x04000381 RID: 897
			Manually
		}
	}
}
