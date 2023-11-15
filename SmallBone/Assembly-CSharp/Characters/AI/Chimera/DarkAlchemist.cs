using System;
using System.Collections;
using Characters.Actions;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Characters.AI.Chimera
{
	// Token: 0x02001250 RID: 4688
	public class DarkAlchemist : MonoBehaviour
	{
		// Token: 0x06005CF3 RID: 23795 RVA: 0x0011182D File Offset: 0x0010FA2D
		private void Start()
		{
			this._texts = Localization.GetLocalizedStringArray(DarkAlchemist._textKey);
			this._charcter.health.onDied += delegate()
			{
				this._particleEffectInfo.Emit(this._charcter.transform.position, this._charcter.collider.bounds, (Vector2.up + Vector2.left) * 4f, true);
			};
		}

		// Token: 0x06005CF4 RID: 23796 RVA: 0x0011185C File Offset: 0x0010FA5C
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			LetterBox.instance.Disappear(0.4f);
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
			if (Scene<GameBase>.instance.cameraController == null || Singleton<Service>.Instance.levelManager.player == null)
			{
				return;
			}
			Scene<GameBase>.instance.cameraController.StartTrack(Singleton<Service>.Instance.levelManager.player.transform);
		}

		// Token: 0x06005CF5 RID: 23797 RVA: 0x0011190F File Offset: 0x0010FB0F
		private IEnumerator CSmallTalk()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = null;
			npcConversation.skippable = true;
			npcConversation.name = Localization.GetLocalizedString(DarkAlchemist._nameKey);
			yield return Chronometer.global.WaitForSeconds(3f);
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[0],
				this._texts[1],
				this._texts[2],
				this._texts[3],
				this._texts[4]
			});
			Characters.Actions.Action action = this._actions[0];
			if (action != null)
			{
				action.TryStart();
			}
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[5]
			});
			Characters.Actions.Action action2 = this._actions[1];
			if (action2 != null)
			{
				action2.TryStart();
			}
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[6]
			});
			Characters.Actions.Action action3 = this._actions[2];
			if (action3 != null)
			{
				action3.TryStart();
			}
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[7]
			});
			Characters.Actions.Action action4 = this._actions[3];
			if (action4 != null)
			{
				action4.TryStart();
			}
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[8]
			});
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			Characters.Actions.Action action5 = this._actions[4];
			if (action5 != null)
			{
				action5.TryStart();
			}
			while (this._actions[4].running)
			{
				yield return null;
			}
			Characters.Actions.Action action6 = this._actions[5];
			if (action6 != null)
			{
				action6.TryStart();
			}
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[9]
			});
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[10]
			});
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			Characters.Actions.Action action7 = this._actions[6];
			if (action7 != null)
			{
				action7.TryStart();
			}
			while (this._actions[6].running)
			{
				yield return null;
			}
			Characters.Actions.Action action8 = this._actions[7];
			if (action8 != null)
			{
				action8.TryStart();
			}
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[11]
			});
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[12]
			});
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			Characters.Actions.Action action9 = this._actions[8];
			if (action9 != null)
			{
				action9.TryStart();
			}
			while (this._actions[8].running)
			{
				yield return null;
			}
			Characters.Actions.Action action10 = this._actions[9];
			if (action10 != null)
			{
				action10.TryStart();
			}
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[13]
			});
			Characters.Actions.Action action11 = this._actions[10];
			if (action11 != null)
			{
				action11.TryStart();
			}
			this.Deactivate();
			yield break;
		}

		// Token: 0x06005CF6 RID: 23798 RVA: 0x0011191E File Offset: 0x0010FB1E
		public void Activate()
		{
			base.StartCoroutine(this.CSmallTalk());
		}

		// Token: 0x06005CF7 RID: 23799 RVA: 0x0011192D File Offset: 0x0010FB2D
		private IEnumerator CActivate()
		{
			yield return Scene<GameBase>.instance.uiManager.letterBox.CAppear(0.4f);
			base.StartCoroutine(this.CSmallTalk());
			yield break;
		}

		// Token: 0x06005CF8 RID: 23800 RVA: 0x0011193C File Offset: 0x0010FB3C
		private void Deactivate()
		{
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			base.StartCoroutine(this.CDeactivate());
		}

		// Token: 0x06005CF9 RID: 23801 RVA: 0x0011195F File Offset: 0x0010FB5F
		private IEnumerator CDeactivate()
		{
			yield return Scene<GameBase>.instance.uiManager.letterBox.CDisappear(0.4f);
			this._script.EndIntro();
			yield break;
		}

		// Token: 0x04004A6D RID: 19053
		private static readonly string _nameKey = "CutScene/name/DarkLabChief";

		// Token: 0x04004A6E RID: 19054
		private static readonly string _textKey = "CutScene/Ch3BossIntro/DarkLabChief/0";

		// Token: 0x04004A6F RID: 19055
		[SerializeField]
		private Chapter3Script _script;

		// Token: 0x04004A70 RID: 19056
		[SerializeField]
		private Character _charcter;

		// Token: 0x04004A71 RID: 19057
		[SerializeField]
		private ParticleEffectInfo _particleEffectInfo;

		// Token: 0x04004A72 RID: 19058
		[SerializeField]
		private Characters.Actions.Action[] _actions;

		// Token: 0x04004A73 RID: 19059
		private string[] _texts;

		// Token: 0x02001251 RID: 4689
		private enum TextType
		{
			// Token: 0x04004A75 RID: 19061
			Idle_01,
			// Token: 0x04004A76 RID: 19062
			Idle_02,
			// Token: 0x04004A77 RID: 19063
			Idle_03,
			// Token: 0x04004A78 RID: 19064
			Idle_04,
			// Token: 0x04004A79 RID: 19065
			Idle_05,
			// Token: 0x04004A7A RID: 19066
			Laugh_01,
			// Token: 0x04004A7B RID: 19067
			Crazy_01,
			// Token: 0x04004A7C RID: 19068
			CrazyLaugh_01,
			// Token: 0x04004A7D RID: 19069
			Wait_01,
			// Token: 0x04004A7E RID: 19070
			Disappoint_01,
			// Token: 0x04004A7F RID: 19071
			Disappoint_02,
			// Token: 0x04004A80 RID: 19072
			Attack_Fire_01,
			// Token: 0x04004A81 RID: 19073
			Attack_Fire_02,
			// Token: 0x04004A82 RID: 19074
			Attack_Second2_01
		}

		// Token: 0x02001252 RID: 4690
		private enum ActionType
		{
			// Token: 0x04004A84 RID: 19076
			Laugh,
			// Token: 0x04004A85 RID: 19077
			Crazy,
			// Token: 0x04004A86 RID: 19078
			CrazyLaugh,
			// Token: 0x04004A87 RID: 19079
			Wait,
			// Token: 0x04004A88 RID: 19080
			Back_LoopX,
			// Token: 0x04004A89 RID: 19081
			Disappoint,
			// Token: 0x04004A8A RID: 19082
			Stand_LoopX,
			// Token: 0x04004A8B RID: 19083
			Attack_Fire,
			// Token: 0x04004A8C RID: 19084
			Attack_Second_LoopX,
			// Token: 0x04004A8D RID: 19085
			Attack_Second2,
			// Token: 0x04004A8E RID: 19086
			Dead
		}
	}
}
