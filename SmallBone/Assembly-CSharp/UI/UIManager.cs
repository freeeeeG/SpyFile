using System;
using Characters.Controllers;
using EndingCredit;
using FX;
using InControl;
using Level;
using Services;
using Singletons;
using SkulStories;
using UI.Boss;
using UI.GearPopup;
using UI.Hud;
using UI.Inventory;
using UI.Pause;
using UI.SkulStories;
using UI.TestingTool;
using UI.Upgrades;
using UnityEngine;
using UnityEngine.EventSystems;
using UserInput;

namespace UI
{
	// Token: 0x020003E0 RID: 992
	public class UIManager : MonoBehaviour
	{
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x000374AB File Offset: 0x000356AB
		public RectTransform rectTransform
		{
			get
			{
				return this._rectTransform;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600126D RID: 4717 RVA: 0x000374B3 File Offset: 0x000356B3
		public RectTransform content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x000374BB File Offset: 0x000356BB
		public Scaler scaler
		{
			get
			{
				return this._scaler;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x000374C3 File Offset: 0x000356C3
		public HeadupDisplay headupDisplay
		{
			get
			{
				return this._headupDisplay;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x000374CB File Offset: 0x000356CB
		public LetterBox letterBox
		{
			get
			{
				return this._letterBox;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x000374D3 File Offset: 0x000356D3
		public NpcContent npcContent
		{
			get
			{
				return this._npcContent;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x000374DB File Offset: 0x000356DB
		public NpcConversation npcConversation
		{
			get
			{
				return this._npcConversation;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x000374E3 File Offset: 0x000356E3
		public Narration narration
		{
			get
			{
				return this._narration;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x000374EB File Offset: 0x000356EB
		public NarrationScene narrationScene
		{
			get
			{
				return this._narrationScene;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x000374F3 File Offset: 0x000356F3
		public BossUIContainer bossUI
		{
			get
			{
				return this._bossUI;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x000374FB File Offset: 0x000356FB
		public StageName stageName
		{
			get
			{
				return this._stageName;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x00037503 File Offset: 0x00035703
		public SystemDialogue systemDialogue
		{
			get
			{
				return this._systemDialogue;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x0003750B File Offset: 0x0003570B
		public UnlockNotice unlockNotice
		{
			get
			{
				return this._unlockNotice;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001279 RID: 4729 RVA: 0x00037513 File Offset: 0x00035713
		public GameResult gameResult
		{
			get
			{
				return this._gameResult;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x0003751B File Offset: 0x0003571B
		public PauseEventSystem pauseEventSystem
		{
			get
			{
				return this._pauseEventSystem;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x00037523 File Offset: 0x00035723
		public UI.TestingTool.Panel testingTool
		{
			get
			{
				return this._testingTool;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x0003752B File Offset: 0x0003572B
		public UI.Inventory.Panel inventory
		{
			get
			{
				return this._inventory;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x00037533 File Offset: 0x00035733
		public AdventurerHealthBarUIController adventurerHealthBarUIController
		{
			get
			{
				return this._adventurerHealthBarUIController;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x0600127E RID: 4734 RVA: 0x0003753B File Offset: 0x0003573B
		public Confirm confirm
		{
			get
			{
				return this._confirm;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x00037543 File Offset: 0x00035743
		public GearPopupCanvas gearPopupCanvas
		{
			get
			{
				return this._gearPopupCanvas;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x0003754B File Offset: 0x0003574B
		public ItemSelect itemSelect
		{
			get
			{
				return this._itemSelect;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x00037553 File Offset: 0x00035753
		public StackVignette curseOfLightVignette
		{
			get
			{
				return this._curseOfLightVignette;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x0003755B File Offset: 0x0003575B
		public CreditRoll endingCredit
		{
			get
			{
				return this._endingCredit;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x00037563 File Offset: 0x00035763
		public UI.Upgrades.Panel upgradeShop
		{
			get
			{
				return this._upgradeShop;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x0003756B File Offset: 0x0003576B
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x00037578 File Offset: 0x00035778
		public bool allowMouseInput
		{
			get
			{
				return this._inputModule.allowMouseInput;
			}
			set
			{
				InControlInputModule inputModule = this._inputModule;
				this._inputModule.focusOnMouseHover = value;
				inputModule.allowMouseInput = value;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x0003759F File Offset: 0x0003579F
		// (set) Token: 0x06001287 RID: 4743 RVA: 0x000375A7 File Offset: 0x000357A7
		public UIManager.HideOption hideOption { get; private set; }

		// Token: 0x06001288 RID: 4744 RVA: 0x000375B0 File Offset: 0x000357B0
		public void ShowPausePopup()
		{
			if (Dialogue.GetCurrent() == null)
			{
				this._pause.Open();
			}
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x000375CC File Offset: 0x000357CC
		private void Update()
		{
			EventSystem current = EventSystem.current;
			if (current.currentSelectedGameObject == null)
			{
				current.SetSelectedGameObject(this._lastSelectedGameObject);
			}
			else
			{
				this._lastSelectedGameObject = current.currentSelectedGameObject;
			}
			Dialogue current2 = Dialogue.GetCurrent();
			if (current2 == null)
			{
				if (KeyMapper.Map.Inventory.WasPressed && !PlayerInput.blocked.value && Singleton<Service>.Instance.levelManager.currentChapter.type != Chapter.Type.Tutorial)
				{
					if (this._inventory.gameObject.activeSelf)
					{
						this._inventory.Close();
					}
					else if (!PlayerInput.blocked.value)
					{
						this._inventory.Open();
					}
				}
				if (KeyMapper.Map.Pause.WasPressed)
				{
					this._pauseEventSystem.Run();
				}
			}
			else if (current2.closeWithPauseKey && KeyMapper.Map.Pause.WasPressed)
			{
				current2.Close();
				UI.Upgrades.Panel panel = (UI.Upgrades.Panel)current2;
				if (panel)
				{
					PersistentSingleton<SoundManager>.Instance.PlaySound(panel.upgradableContainer.closeSoundInfo, base.gameObject.transform.position);
				}
			}
			if (this.testingTool.canUse && KeyMapper.Map.OpenTestingTool.WasPressed)
			{
				this.testingTool.Toggle();
			}
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00037720 File Offset: 0x00035920
		public void SetHideOption(UIManager.HideOption hideOption)
		{
			this.hideOption = hideOption;
			switch (hideOption)
			{
			case UIManager.HideOption.ShowAll:
				this._hidedByHideHud.SetActive(true);
				this._hidedByHideAll.SetActive(true);
				return;
			case UIManager.HideOption.HideHUD:
				this._hidedByHideHud.SetActive(false);
				this._hidedByHideAll.SetActive(true);
				return;
			case UIManager.HideOption.HideAll:
				this._hidedByHideHud.SetActive(false);
				this._hidedByHideAll.SetActive(false);
				return;
			default:
				return;
			}
		}

		// Token: 0x04000F67 RID: 3943
		[SerializeField]
		private RectTransform _rectTransform;

		// Token: 0x04000F68 RID: 3944
		[SerializeField]
		[Space]
		private RectTransform _content;

		// Token: 0x04000F69 RID: 3945
		[SerializeField]
		[Space]
		private HeadupDisplay _headupDisplay;

		// Token: 0x04000F6A RID: 3946
		[SerializeField]
		private LetterBox _letterBox;

		// Token: 0x04000F6B RID: 3947
		[SerializeField]
		private NpcContent _npcContent;

		// Token: 0x04000F6C RID: 3948
		[SerializeField]
		private NpcConversation _npcConversation;

		// Token: 0x04000F6D RID: 3949
		[SerializeField]
		private Narration _narration;

		// Token: 0x04000F6E RID: 3950
		[SerializeField]
		private NarrationScene _narrationScene;

		// Token: 0x04000F6F RID: 3951
		[SerializeField]
		private BossUIContainer _bossUI;

		// Token: 0x04000F70 RID: 3952
		[SerializeField]
		private AdventurerHealthBarUIController _adventurerHealthBarUIController;

		// Token: 0x04000F71 RID: 3953
		[SerializeField]
		private StageName _stageName;

		// Token: 0x04000F72 RID: 3954
		[SerializeField]
		private UnlockNotice _unlockNotice;

		// Token: 0x04000F73 RID: 3955
		[SerializeField]
		private SystemDialogue _systemDialogue;

		// Token: 0x04000F74 RID: 3956
		[SerializeField]
		private GameResult _gameResult;

		// Token: 0x04000F75 RID: 3957
		[SerializeField]
		private UI.Inventory.Panel _inventory;

		// Token: 0x04000F76 RID: 3958
		[SerializeField]
		private UI.TestingTool.Panel _testingTool;

		// Token: 0x04000F77 RID: 3959
		[SerializeField]
		private Confirm _confirm;

		// Token: 0x04000F78 RID: 3960
		[SerializeField]
		private Confirm _skulstoryConfirm;

		// Token: 0x04000F79 RID: 3961
		[SerializeField]
		private GearPopupCanvas _gearPopupCanvas;

		// Token: 0x04000F7A RID: 3962
		[SerializeField]
		private ItemSelect _itemSelect;

		// Token: 0x04000F7B RID: 3963
		[SerializeField]
		private InControlInputModule _inputModule;

		// Token: 0x04000F7C RID: 3964
		[SerializeField]
		private StackVignette _curseOfLightVignette;

		// Token: 0x04000F7D RID: 3965
		[SerializeField]
		private CreditRoll _endingCredit;

		// Token: 0x04000F7E RID: 3966
		[SerializeField]
		private UI.Pause.Panel _pause;

		// Token: 0x04000F7F RID: 3967
		[SerializeField]
		private PauseEventSystem _pauseEventSystem;

		// Token: 0x04000F80 RID: 3968
		[SerializeField]
		[Header("Upgrade")]
		private UI.Upgrades.Panel _upgradeShop;

		// Token: 0x04000F81 RID: 3969
		[Header("Hide")]
		[SerializeField]
		private GameObject _hidedByHideHud;

		// Token: 0x04000F82 RID: 3970
		[SerializeField]
		private GameObject _hidedByHideAll;

		// Token: 0x04000F83 RID: 3971
		[SerializeField]
		private Scaler _scaler;

		// Token: 0x04000F84 RID: 3972
		private GameObject _lastSelectedGameObject;

		// Token: 0x020003E1 RID: 993
		public enum HideOption
		{
			// Token: 0x04000F87 RID: 3975
			ShowAll,
			// Token: 0x04000F88 RID: 3976
			HideHUD,
			// Token: 0x04000F89 RID: 3977
			HideAll
		}
	}
}
