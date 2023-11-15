using System;
using System.Collections;
using Characters.Actions;
using Characters.Controllers;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

namespace CutScenes
{
	// Token: 0x020001A3 RID: 419
	public class LeianaMaster : MonoBehaviour
	{
		// Token: 0x06000904 RID: 2308 RVA: 0x00019CFB File Offset: 0x00017EFB
		private void Start()
		{
			this._texts = Localization.GetLocalizedStringArray(LeianaMaster._textKey);
			this._outroTexts = Localization.GetLocalizedStringArray(LeianaMaster._outroTextkey);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00019D20 File Offset: 0x00017F20
		private void OnDestroy()
		{
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			PlayerInput.blocked.Detach(this);
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00019D7B File Offset: 0x00017F7B
		public IEnumerator CSmallTalk()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = null;
			npcConversation.skippable = true;
			npcConversation.name = Localization.GetLocalizedString(LeianaMaster._nameKey);
			this._actions[0].TryStart();
			while (this._actions[0].running)
			{
				yield return null;
			}
			this._actions[1].TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[0]
			});
			this._actions[2].TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[1]
			});
			this._actions[1].TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[2]
			});
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[3]
			});
			npcConversation.Done();
			this._actions[3].TryStart();
			while (this._actions[3].running)
			{
				yield return null;
			}
			this._actions[5].TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[4]
			});
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[5]
			});
			this._actions[4].TryStart();
			npcConversation.Done();
			yield break;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00019D8A File Offset: 0x00017F8A
		public IEnumerator CTalkToStartCombat()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = null;
			npcConversation.skippable = true;
			npcConversation.name = Localization.GetLocalizedString(LeianaMaster._nameKey);
			this._actions[6].TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[6]
			});
			yield return npcConversation.CConversation(new string[]
			{
				this._texts[7]
			});
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			yield break;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00019D99 File Offset: 0x00017F99
		public IEnumerator COutroTalk()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = null;
			npcConversation.skippable = true;
			npcConversation.name = Localization.GetLocalizedString(LeianaMaster._nameKey);
			this._actions[7].TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._outroTexts[0]
			});
			yield return npcConversation.CConversation(new string[]
			{
				this._outroTexts[1]
			});
			this._actions[8].TryStart();
			yield return npcConversation.CConversation(new string[]
			{
				this._outroTexts[2]
			});
			yield return npcConversation.CConversation(new string[]
			{
				this._outroTexts[3]
			});
			this._actions[9].TryStart();
			yield return Chronometer.global.WaitForSeconds(1f);
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			yield break;
		}

		// Token: 0x0400073F RID: 1855
		private static readonly string _nameKey = "CutScene/name/GrandMaster";

		// Token: 0x04000740 RID: 1856
		private static readonly string _textKey = "CutScene/Ch2BossIntro/GrandMaster/0";

		// Token: 0x04000741 RID: 1857
		private static readonly string _outroTextkey = "CutScene/Ch2BossOutro/GrandMaster/0";

		// Token: 0x04000742 RID: 1858
		[SerializeField]
		private Characters.Actions.Action[] _actions;

		// Token: 0x04000743 RID: 1859
		private string[] _texts;

		// Token: 0x04000744 RID: 1860
		private string[] _outroTexts;

		// Token: 0x020001A4 RID: 420
		private enum Animation
		{
			// Token: 0x04000746 RID: 1862
			Arrival,
			// Token: 0x04000747 RID: 1863
			Talk1,
			// Token: 0x04000748 RID: 1864
			Laugh,
			// Token: 0x04000749 RID: 1865
			Pose_Loop,
			// Token: 0x0400074A RID: 1866
			Pose_Freeze,
			// Token: 0x0400074B RID: 1867
			Talk2,
			// Token: 0x0400074C RID: 1868
			Idle,
			// Token: 0x0400074D RID: 1869
			Dead,
			// Token: 0x0400074E RID: 1870
			DeadFreeze,
			// Token: 0x0400074F RID: 1871
			BackIdle,
			// Token: 0x04000750 RID: 1872
			Outro
		}

		// Token: 0x020001A5 RID: 421
		private enum IntroText
		{
			// Token: 0x04000752 RID: 1874
			Talk1_01,
			// Token: 0x04000753 RID: 1875
			Laugh_01,
			// Token: 0x04000754 RID: 1876
			Talk1_02,
			// Token: 0x04000755 RID: 1877
			Talk1_03,
			// Token: 0x04000756 RID: 1878
			Talk2_01,
			// Token: 0x04000757 RID: 1879
			Talk2_02,
			// Token: 0x04000758 RID: 1880
			Idle_01,
			// Token: 0x04000759 RID: 1881
			Idle_02
		}

		// Token: 0x020001A6 RID: 422
		private enum OutroText
		{
			// Token: 0x0400075B RID: 1883
			Outro_01,
			// Token: 0x0400075C RID: 1884
			Outro_02,
			// Token: 0x0400075D RID: 1885
			Back_01,
			// Token: 0x0400075E RID: 1886
			Back_02
		}
	}
}
