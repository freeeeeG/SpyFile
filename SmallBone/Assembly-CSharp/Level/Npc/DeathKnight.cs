using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using Data;
using GameResources;
using Housing;
using Scenes;
using Services;
using UI;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x02000592 RID: 1426
	public class DeathKnight : InteractiveObject
	{
		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001C08 RID: 7176 RVA: 0x00056F61 File Offset: 0x00055161
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/name", NpcType.DeathKnight));
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001C09 RID: 7177 RVA: 0x00056F78 File Offset: 0x00055178
		public string greeting
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/greeting", NpcType.DeathKnight)).Random<string>();
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001C0A RID: 7178 RVA: 0x00056F94 File Offset: 0x00055194
		public string[] chat
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/chat", NpcType.DeathKnight)).Random<string[]>();
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x00056FB0 File Offset: 0x000551B0
		public string buildLabel
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/build/Label", NpcType.DeathKnight));
			}
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x00056FC7 File Offset: 0x000551C7
		public string BuildText(int cost)
		{
			return string.Format(Localization.GetLocalizedStringArray(string.Format("npc/{0}/build", NpcType.DeathKnight)).Random<string>(), cost);
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x00056FEE File Offset: 0x000551EE
		public string buildSuccess
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/build/Success", NpcType.DeathKnight)).Random<string>();
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001C0E RID: 7182 RVA: 0x0005700A File Offset: 0x0005520A
		public string buildNoMoney
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/build/NoMoney", NpcType.DeathKnight)).Random<string>();
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x00057026 File Offset: 0x00055226
		public string[] buildNoLevel
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/build/NoLevel", NpcType.DeathKnight));
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x0005703D File Offset: 0x0005523D
		public string buildAgain
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/build/Again", NpcType.DeathKnight)).Random<string>();
			}
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x0005705C File Offset: 0x0005525C
		private string GetBuildText(BuildLevel buildLevel)
		{
			for (int i = 0; i < this._specialBuildTexts.Length; i++)
			{
				if (this._specialBuildTexts[i].target == buildLevel)
				{
					return this._specialBuildTexts[i].Text(buildLevel.cost);
				}
			}
			return this.BuildText(buildLevel.cost);
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000570B1 File Offset: 0x000552B1
		protected override void Awake()
		{
			base.Awake();
			if (!GameData.Progress.GetRescued(NpcType.DeathKnight))
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x000570CD File Offset: 0x000552CD
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x000570E4 File Offset: 0x000552E4
		private void OnDisable()
		{
			if (Service.quitting)
			{
				return;
			}
			LetterBox.instance.visible = false;
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x000570F9 File Offset: 0x000552F9
		public override void InteractWith(Character character)
		{
			this._npcConversation.name = this.displayName;
			this._npcConversation.portrait = this._portrait;
			base.StartCoroutine(this.CBuild());
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x0005712A File Offset: 0x0005532A
		private IEnumerator CBuild()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.body = (this._afterBuild ? this.buildAgain : this.greeting);
			this._npcConversation.skippable = false;
			this._npcConversation.Type();
			this._npcConversation.OpenContentSelector(this.buildLabel, new Action(this.Build), new Action(this.Chat), new Action(this.Close));
			yield break;
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x00057139 File Offset: 0x00055339
		private void Chat()
		{
			this._npcConversation.skippable = true;
			base.StartCoroutine(this.<Chat>g__CRun|30_0());
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x00057154 File Offset: 0x00055354
		private void Close()
		{
			this._npcConversation.visible = false;
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x00057174 File Offset: 0x00055374
		private void Build()
		{
			DeathKnight.<>c__DisplayClass32_0 CS$<>8__locals1 = new DeathKnight.<>c__DisplayClass32_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.nextBuildLevel = this._housingBuilder.GetLevelAfterPoint(GameData.Progress.housingPoint);
			base.StartCoroutine(CS$<>8__locals1.<Build>g__CRun|0());
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000571B1 File Offset: 0x000553B1
		[CompilerGenerated]
		private IEnumerator <Chat>g__CRun|30_0()
		{
			if (!LetterBox.instance.visible)
			{
				yield return LetterBox.instance.CAppear(0.4f);
			}
			yield return this._npcConversation.CConversation(this.chat);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x04001807 RID: 6151
		private const NpcType _type = NpcType.DeathKnight;

		// Token: 0x04001808 RID: 6152
		private bool _afterBuild;

		// Token: 0x04001809 RID: 6153
		private NpcConversation _npcConversation;

		// Token: 0x0400180A RID: 6154
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x0400180B RID: 6155
		[SerializeField]
		private HousingBuilder _housingBuilder;

		// Token: 0x0400180C RID: 6156
		[SerializeField]
		private DeathKnight.SpecialBuildText[] _specialBuildTexts;

		// Token: 0x02000593 RID: 1427
		[Serializable]
		public class SpecialBuildText
		{
			// Token: 0x06001C1C RID: 7196 RVA: 0x000571C0 File Offset: 0x000553C0
			public string Text(int cost)
			{
				return string.Format(Localization.GetLocalizedString(string.Format("npc/{0}/build/Special/{1}", NpcType.DeathKnight, this.textKey)), cost);
			}

			// Token: 0x0400180D RID: 6157
			[SerializeField]
			public string textKey;

			// Token: 0x0400180E RID: 6158
			[SerializeField]
			public BuildLevel target;
		}
	}
}
