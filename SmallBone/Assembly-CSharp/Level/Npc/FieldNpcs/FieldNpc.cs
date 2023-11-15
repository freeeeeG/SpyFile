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

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005D9 RID: 1497
	public abstract class FieldNpc : InteractiveObject
	{
		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001DDB RID: 7643 RVA: 0x0005B27C File Offset: 0x0005947C
		// (set) Token: 0x06001DDC RID: 7644 RVA: 0x0005B28E File Offset: 0x0005948E
		public bool encountered
		{
			get
			{
				return GameData.Progress.fieldNpcEncountered.GetData(this._type);
			}
			set
			{
				GameData.Progress.fieldNpcEncountered.SetData(this._type, value);
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001DDD RID: 7645
		protected abstract NpcType _type { get; }

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001DDE RID: 7646 RVA: 0x0005B2A1 File Offset: 0x000594A1
		public NpcType type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x0005B2A9 File Offset: 0x000594A9
		protected string _displayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/name", this._type));
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x0005B2C5 File Offset: 0x000594C5
		protected string[] _greeting
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/greeting", this._type));
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x0005B2E1 File Offset: 0x000594E1
		protected string[] _regreeting
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/regreeting", this._type));
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x0005B2FD File Offset: 0x000594FD
		protected string[] _confirmed
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/confirmed", this._type));
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x0005B319 File Offset: 0x00059519
		protected string[] _noMoney
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/noMoney", this._type));
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x0005B335 File Offset: 0x00059535
		protected string[] _chat
		{
			get
			{
				return Localization.GetLocalizedStringArrays(string.Format("npc/{0}/chat", this._type)).Random<string[]>();
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001DE5 RID: 7653 RVA: 0x0005B356 File Offset: 0x00059556
		public string cageDestroyedLine
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/line/resqued", this._type));
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x0005B372 File Offset: 0x00059572
		public string normalLine
		{
			get
			{
				return Localization.GetLocalizedStringArray(string.Format("npc/{0}/line", this._type)).Random<string>();
			}
		}

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06001DE7 RID: 7655 RVA: 0x0005B394 File Offset: 0x00059594
		// (remove) Token: 0x06001DE8 RID: 7656 RVA: 0x0005B3CC File Offset: 0x000595CC
		public event FieldNpc.OnReleaseDelegate onRelease;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06001DE9 RID: 7657 RVA: 0x0005B404 File Offset: 0x00059604
		// (remove) Token: 0x06001DEA RID: 7658 RVA: 0x0005B43C File Offset: 0x0005963C
		public event FieldNpc.OnCageDestroyedDelegate onCageDestroyed;

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001DEB RID: 7659 RVA: 0x0005B471 File Offset: 0x00059671
		public bool release
		{
			get
			{
				return this._release;
			}
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x0005B47C File Offset: 0x0005967C
		protected override void Awake()
		{
			base.Awake();
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			this._npcConversation.name = this._displayName;
			this._npcConversation.skippable = true;
			this._npcConversation.portrait = this._portrait;
			this._animator.Play(FieldNpc._idleCageHash, 0, 0f);
			this._interactRange.enabled = false;
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x0005B4F4 File Offset: 0x000596F4
		public void SetCage(Cage cage)
		{
			cage.onDestroyed += this.OnCageDestroyed;
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x0005B508 File Offset: 0x00059708
		protected void OnCageDestroyed()
		{
			this._interactRange.enabled = true;
			FieldNpc.OnCageDestroyedDelegate onCageDestroyedDelegate = this.onCageDestroyed;
			if (onCageDestroyedDelegate == null)
			{
				return;
			}
			onCageDestroyedDelegate();
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x0005B526 File Offset: 0x00059726
		public void Flip()
		{
			this._animator.transform.localScale = new Vector3(-1f, 1f, 1f);
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x0005B54C File Offset: 0x0005974C
		private void Release()
		{
			this.encountered = true;
			this._release = true;
			this._animator.Play(FieldNpc._idleHash, 0, 0f);
			this._uiObject.SetActive(false);
			this._talkUiObject.SetActive(true);
			this._uiObject = this._talkUiObject;
			FieldNpc.OnReleaseDelegate onReleaseDelegate = this.onRelease;
			if (onReleaseDelegate == null)
			{
				return;
			}
			onReleaseDelegate();
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0005B5B1 File Offset: 0x000597B1
		protected virtual void Start()
		{
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage += new TookDamageDelegate(this.StopConversation);
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x0005B5D8 File Offset: 0x000597D8
		protected virtual void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage -= new TookDamageDelegate(this.StopConversation);
			this.Close();
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0005B60D File Offset: 0x0005980D
		private void StopConversation(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._releasePressingButton.StopPressing();
			base.StopAllCoroutines();
			this.Close();
			if (!this._release)
			{
				return;
			}
			this.OnStopConversation();
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x00002191 File Offset: 0x00000391
		protected virtual void OnStopConversation()
		{
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x0005B635 File Offset: 0x00059835
		protected void Close()
		{
			this._npcConversation.portrait = null;
			this._npcConversation.visible = false;
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x0005B65E File Offset: 0x0005985E
		public override void InteractWithByPressing(Character character)
		{
			this.Release();
			this.Interact(character);
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x0005B66D File Offset: 0x0005986D
		public override void InteractWith(Character character)
		{
			if (!this._release)
			{
				return;
			}
			this.Interact(character);
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0005B67F File Offset: 0x0005987F
		protected virtual void Interact(Character character)
		{
			this._npcConversation.name = this._displayName;
			this._npcConversation.portrait = this._portrait;
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0005B6A3 File Offset: 0x000598A3
		protected IEnumerator CGreeting()
		{
			this._npcConversation.skippable = true;
			yield return this._npcConversation.CConversation(this._greeting);
			yield break;
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0005B6B2 File Offset: 0x000598B2
		protected IEnumerator CChat()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.skippable = true;
			yield return this._npcConversation.CConversation(this._chat);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x0400194C RID: 6476
		protected static readonly int _idleHash = Animator.StringToHash("Idle");

		// Token: 0x0400194D RID: 6477
		protected static readonly int _idleCageHash = Animator.StringToHash("Idle_Cage");

		// Token: 0x0400194E RID: 6478
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x0400194F RID: 6479
		[SerializeField]
		protected Animator _animator;

		// Token: 0x04001950 RID: 6480
		[SerializeField]
		private Collider2D _interactRange;

		// Token: 0x04001951 RID: 6481
		[SerializeField]
		private PressingButton _releasePressingButton;

		// Token: 0x04001952 RID: 6482
		[SerializeField]
		private GameObject _talkUiObject;

		// Token: 0x04001953 RID: 6483
		protected FieldNpc.Phase _phase;

		// Token: 0x04001954 RID: 6484
		protected NpcConversation _npcConversation;

		// Token: 0x04001955 RID: 6485
		private bool _release;

		// Token: 0x020005DA RID: 1498
		protected enum Phase
		{
			// Token: 0x04001959 RID: 6489
			Initial,
			// Token: 0x0400195A RID: 6490
			Greeted,
			// Token: 0x0400195B RID: 6491
			Gave
		}

		// Token: 0x020005DB RID: 1499
		// (Invoke) Token: 0x06001DFE RID: 7678
		public delegate void OnReleaseDelegate();

		// Token: 0x020005DC RID: 1500
		// (Invoke) Token: 0x06001E02 RID: 7682
		public delegate void OnCageDestroyedDelegate();
	}
}
