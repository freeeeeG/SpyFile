using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using Characters.AI;
using Characters.Player;
using CutScenes;
using Data;
using GameResources;
using Hardmode;
using Runnables;
using Scenes;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x02000587 RID: 1415
	public class Arachne : InteractiveObject
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x000565EF File Offset: 0x000547EF
		private string arachne
		{
			get
			{
				if (!Singleton<HardmodeManager>.Instance.hardmode)
				{
					return this.normalName;
				}
				return this.hardName;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x0005660A File Offset: 0x0005480A
		private string displayName
		{
			get
			{
				return Localization.GetLocalizedString("npc/" + this.arachne + "/name");
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x00056626 File Offset: 0x00054826
		private string greeting
		{
			get
			{
				return Localization.GetLocalizedString("npc/" + this.arachne + "/greeting");
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x00056642 File Offset: 0x00054842
		private string awakenLabel
		{
			get
			{
				return Localization.GetLocalizedString("npc/" + this.arachne + "/awaken/label");
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x0005665E File Offset: 0x0005485E
		private string notExistsNextGrade
		{
			get
			{
				return Localization.GetLocalizedString("npc/" + this.arachne + "/awaken/notExistsNextGrade");
			}
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x0005667A File Offset: 0x0005487A
		private string askAwaken(int cost)
		{
			return string.Format(Localization.GetLocalizedString("npc/" + this.arachne + "/awaken/ask"), cost);
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000566A1 File Offset: 0x000548A1
		private string askAwakenAgain(int cost)
		{
			return string.Format(Localization.GetLocalizedString("npc/" + this.arachne + "/awaken/askAgain"), cost);
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x000566C8 File Offset: 0x000548C8
		private string awaken
		{
			get
			{
				return Localization.GetLocalizedString("npc/" + this.arachne + "/awaken");
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x000566E4 File Offset: 0x000548E4
		private string noMoney
		{
			get
			{
				return Localization.GetLocalizedString("npc/" + this.arachne + "/awaken/noMoney");
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x00056700 File Offset: 0x00054900
		private string[] skulAwaken
		{
			get
			{
				return Localization.GetLocalizedStringArray("npc/" + this.arachne + "/awaken/skul");
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001BCA RID: 7114 RVA: 0x0005671C File Offset: 0x0005491C
		private string[] tutorial
		{
			get
			{
				return Localization.GetLocalizedStringArray("npc/" + this.arachne + "/tutorial");
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x00056738 File Offset: 0x00054938
		private string[] chat
		{
			get
			{
				return Localization.GetLocalizedStringArrays("npc/" + this.arachne + "/chat").Random<string[]>();
			}
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x00056759 File Offset: 0x00054959
		protected override void Awake()
		{
			base.Awake();
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x00056778 File Offset: 0x00054978
		public override void InteractWith(Character character)
		{
			character.CancelAction();
			this._weaponInventory = character.playerComponents.inventory.weapon;
			this._lineText.gameObject.SetActive(false);
			this._npcConversation.name = this.displayName;
			this._npcConversation.skippable = true;
			this._npcConversation.portrait = null;
			if (!GameData.Progress.cutscene.GetData(this._key))
			{
				this._tutorial.Run();
				return;
			}
			base.StartCoroutine(this.CGreeting());
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x00056806 File Offset: 0x00054A06
		public void SpawnScarecrowWave()
		{
			if (this._scarecrow == null)
			{
				return;
			}
			if (this._scarecrow.gameObject.activeSelf)
			{
				return;
			}
			this._scarecrow.gameObject.SetActive(true);
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0005683C File Offset: 0x00054A3C
		private void SimpleConversationAndClose(params string[] texts)
		{
			Arachne.<>c__DisplayClass38_0 CS$<>8__locals1 = new Arachne.<>c__DisplayClass38_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.texts = texts;
			base.StartCoroutine(CS$<>8__locals1.<SimpleConversationAndClose>g__CRun|0());
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0005686A File Offset: 0x00054A6A
		private IEnumerator CGreeting()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.body = this.greeting;
			this._npcConversation.skippable = false;
			this._npcConversation.Type();
			this._npcConversation.OpenContentSelector(this.awakenLabel, new Action(this.<CGreeting>g__OnSelectAwaken|39_0), new Action(this.<CGreeting>g__OnSelectChat|39_1), new Action(this.Close));
			yield break;
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x00056879 File Offset: 0x00054A79
		private IEnumerator CAskAwaken()
		{
			Arachne.<>c__DisplayClass40_0 CS$<>8__locals1 = new Arachne.<>c__DisplayClass40_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.cost = Settings.instance.bonesToUpgrade[this._weaponInventory.current.rarity];
			this._npcConversation.skippable = true;
			this._npcConversation.body = ((this._phase == Arachne.Phase.First) ? this.askAwaken(CS$<>8__locals1.cost) : this.askAwakenAgain(CS$<>8__locals1.cost));
			this._npcConversation.OpenCurrencyBalancePanel(GameData.Currency.Type.Bone);
			yield return this._npcConversation.CType();
			this._npcConversation.OpenConfirmSelector(new Action(CS$<>8__locals1.<CAskAwaken>g__OnSelectYes|0), new Action(this.Close));
			yield break;
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x00056888 File Offset: 0x00054A88
		private IEnumerator CAwaken()
		{
			this._npcConversation.skippable = true;
			this._npcConversation.body = this.awaken;
			yield return this._npcConversation.CType();
			yield return this._npcConversation.CWaitInput();
			this._npcConversation.visible = false;
			this._phase = Arachne.Phase.Awakened;
			switch (this._weaponInventory.current.nextLevelReference.rarity)
			{
			case Rarity.Common:
			case Rarity.Rare:
				this._awakeningForRare.Run();
				break;
			case Rarity.Unique:
				this._awakeningForUnique.Run();
				break;
			case Rarity.Legendary:
				this._awakeningForLegendary.Run();
				break;
			}
			yield break;
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x00056897 File Offset: 0x00054A97
		public IEnumerator CUpgrade()
		{
			yield return this._weaponInventory.CUpgradeCurrentWeapon();
			yield break;
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000568A6 File Offset: 0x00054AA6
		private IEnumerator CNoMoney()
		{
			this._npcConversation.skippable = true;
			this._npcConversation.body = this.noMoney;
			yield return this._npcConversation.CType();
			yield return this._npcConversation.CWaitInput();
			this.Close();
			yield break;
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x000568B5 File Offset: 0x00054AB5
		private void Close()
		{
			this._npcConversation.visible = false;
			this._npcConversation.CloseCurrencyBalancePanel();
			LetterBox.instance.Disappear(0.4f);
			this._lineText.gameObject.SetActive(true);
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x00056918 File Offset: 0x00054B18
		[CompilerGenerated]
		private void <CGreeting>g__OnSelectAwaken|39_0()
		{
			if (this._weaponInventory.current.name.Equals("skul", StringComparison.OrdinalIgnoreCase))
			{
				this.SimpleConversationAndClose(this.skulAwaken);
				return;
			}
			if (this._weaponInventory.current.upgradable)
			{
				base.StartCoroutine(this.CAskAwaken());
				return;
			}
			base.StartCoroutine(this.<CGreeting>g__CNotExistsNextGrade|39_2());
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x0005697C File Offset: 0x00054B7C
		[CompilerGenerated]
		private IEnumerator <CGreeting>g__CNotExistsNextGrade|39_2()
		{
			this._npcConversation.skippable = true;
			this._npcConversation.CloseCurrencyBalancePanel();
			yield return this._npcConversation.CConversation(new string[]
			{
				this.notExistsNextGrade
			});
			this.Close();
			yield break;
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0005698B File Offset: 0x00054B8B
		[CompilerGenerated]
		private void <CGreeting>g__OnSelectChat|39_1()
		{
			this.SimpleConversationAndClose(this.chat);
		}

		// Token: 0x040017DE RID: 6110
		[SerializeField]
		private NpcLineText _lineText;

		// Token: 0x040017DF RID: 6111
		[SerializeField]
		private ReviveScarecrowOnDie _scarecrow;

		// Token: 0x040017E0 RID: 6112
		[SerializeField]
		private Runnable _awakeningForRare;

		// Token: 0x040017E1 RID: 6113
		[SerializeField]
		private Runnable _awakeningForUnique;

		// Token: 0x040017E2 RID: 6114
		[SerializeField]
		private Runnable _awakeningForLegendary;

		// Token: 0x040017E3 RID: 6115
		[SerializeField]
		private Runnable _tutorial;

		// Token: 0x040017E4 RID: 6116
		private readonly string normalName = "arachne";

		// Token: 0x040017E5 RID: 6117
		private readonly string hardName = "arachneHardmode";

		// Token: 0x040017E6 RID: 6118
		[SerializeField]
		private CutScenes.Key _key = CutScenes.Key.arachne;

		// Token: 0x040017E7 RID: 6119
		private Arachne.Phase _phase;

		// Token: 0x040017E8 RID: 6120
		private NpcConversation _npcConversation;

		// Token: 0x040017E9 RID: 6121
		private WeaponInventory _weaponInventory;

		// Token: 0x02000588 RID: 1416
		private enum Phase
		{
			// Token: 0x040017EB RID: 6123
			First,
			// Token: 0x040017EC RID: 6124
			Awakened
		}
	}
}
