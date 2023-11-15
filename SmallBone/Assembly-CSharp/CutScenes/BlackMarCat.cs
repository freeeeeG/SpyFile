using System;
using System.Collections;
using Characters;
using Characters.Actions;
using Characters.Controllers;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace CutScenes
{
	// Token: 0x0200019C RID: 412
	public class BlackMarCat : MonoBehaviour
	{
		// Token: 0x060008E2 RID: 2274 RVA: 0x000194B0 File Offset: 0x000176B0
		private void Start()
		{
			if (GameData.Progress.cutscene.GetData(Key.strangeCat))
			{
				base.transform.position = this._endPoint.position;
				this._spriteRenderer.enabled = true;
				return;
			}
			this._texts = Localization.GetLocalizedStringArray(BlackMarCat._textKey);
			if (this._texts == null || this._texts.Length < 10)
			{
				base.transform.position = this._endPoint.position;
				this._spriteRenderer.enabled = true;
				return;
			}
			this.Activate();
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001953E File Offset: 0x0001773E
		private IEnumerator CSmallTalk()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			this._character.ForceToLookAt(Character.LookingDirection.Left);
			this._actions[0].TryStart();
			this._spriteRenderer.enabled = true;
			while (this._actions[0].running)
			{
				yield return null;
			}
			yield return this.MoveTo(this._character, this._talkPoint.position);
			this._actions[2].TryStart();
			while (this._actions[2].running)
			{
				yield return null;
			}
			this._actions[3].TryStart();
			while (this._actions[3].running)
			{
				yield return null;
			}
			this._actions[4].TryStart();
			while (this._actions[4].running)
			{
				yield return null;
			}
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[0]);
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[1]);
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[2]);
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[3]);
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[4]);
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[5]);
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[6]);
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[7]);
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[8]);
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[9]);
			yield return npcConversation.CTalkRaw(Localization.GetLocalizedString(BlackMarCat._nameKey), this._texts[10]);
			this._actions[6].TryStart();
			while (this._actions[6].running)
			{
				yield return null;
			}
			npcConversation.Done();
			yield return this.MoveTo(this._character, this._endPoint.position);
			this._character.ForceToLookAt(Character.LookingDirection.Right);
			this.Deactivate();
			yield break;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00019550 File Offset: 0x00017750
		private void OnDestroy()
		{
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			PlayerInput.blocked.Detach(this);
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
			Singleton<Service>.Instance.levelManager.player.movement.blocked.Detach(this);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x000195B4 File Offset: 0x000177B4
		public void Activate()
		{
			PlayerInput.blocked.Attach(this);
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = false;
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.ForceToLookAt(Character.LookingDirection.Right);
			player.movement.blocked.Attach(this);
			base.StartCoroutine(this.CActivate());
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00019614 File Offset: 0x00017814
		private IEnumerator CActivate()
		{
			yield return Scene<GameBase>.instance.uiManager.letterBox.CAppear(0.4f);
			base.StartCoroutine(this.CSmallTalk());
			yield break;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00019623 File Offset: 0x00017823
		private void Deactivate()
		{
			PlayerInput.blocked.Detach(this);
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			base.StartCoroutine(this.CDeactivate());
			GameData.Progress.cutscene.SetData(Key.strangeCat, true);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00019662 File Offset: 0x00017862
		private IEnumerator CDeactivate()
		{
			yield return Scene<GameBase>.instance.uiManager.letterBox.CDisappear(0.4f);
			PlayerInput.blocked.Detach(this);
			Singleton<Service>.Instance.levelManager.player.movement.blocked.Detach(this);
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
			yield break;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00019671 File Offset: 0x00017871
		private IEnumerator MoveTo(Character owner, Vector3 destination)
		{
			for (;;)
			{
				float num = destination.x - owner.transform.position.x;
				if (Mathf.Abs(num) < 0.5f)
				{
					break;
				}
				Vector2 move = (num > 0f) ? Vector2.right : Vector2.left;
				owner.movement.move = move;
				yield return null;
			}
			yield break;
		}

		// Token: 0x04000715 RID: 1813
		private static readonly string _nameKey = "CutScene/name/StrangeCat";

		// Token: 0x04000716 RID: 1814
		private static readonly string _textKey = "CutScene/BlackMarketIntro/StrangeCat/0";

		// Token: 0x04000717 RID: 1815
		[SerializeField]
		private Character _character;

		// Token: 0x04000718 RID: 1816
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04000719 RID: 1817
		[SerializeField]
		private Transform _talkPoint;

		// Token: 0x0400071A RID: 1818
		[SerializeField]
		private Transform _endPoint;

		// Token: 0x0400071B RID: 1819
		[SerializeField]
		private Characters.Actions.Action[] _actions;

		// Token: 0x0400071C RID: 1820
		private string[] _texts;

		// Token: 0x0200019D RID: 413
		private enum ActionType
		{
			// Token: 0x0400071E RID: 1822
			Appearance,
			// Token: 0x0400071F RID: 1823
			Walk,
			// Token: 0x04000720 RID: 1824
			Freeze,
			// Token: 0x04000721 RID: 1825
			Amazing,
			// Token: 0x04000722 RID: 1826
			AmazingFreeze,
			// Token: 0x04000723 RID: 1827
			Idle,
			// Token: 0x04000724 RID: 1828
			Bye
		}

		// Token: 0x0200019E RID: 414
		private enum TextType
		{
			// Token: 0x04000726 RID: 1830
			Idle_01,
			// Token: 0x04000727 RID: 1831
			Idle_02,
			// Token: 0x04000728 RID: 1832
			Idle_03,
			// Token: 0x04000729 RID: 1833
			Idle_04,
			// Token: 0x0400072A RID: 1834
			Idle_05,
			// Token: 0x0400072B RID: 1835
			Idle_06,
			// Token: 0x0400072C RID: 1836
			Idle_07,
			// Token: 0x0400072D RID: 1837
			Idle_08,
			// Token: 0x0400072E RID: 1838
			Idle_09,
			// Token: 0x0400072F RID: 1839
			Idle_10,
			// Token: 0x04000730 RID: 1840
			Idle_11
		}
	}
}
