using System;
using UnityEngine;

// Token: 0x0200095E RID: 2398
[AddComponentMenu("KMonoBehaviour/scripts/ScreenPrefabs")]
public class ScreenPrefabs : KMonoBehaviour
{
	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06004681 RID: 18049 RVA: 0x0018EA49 File Offset: 0x0018CC49
	// (set) Token: 0x06004682 RID: 18050 RVA: 0x0018EA50 File Offset: 0x0018CC50
	public static ScreenPrefabs Instance { get; private set; }

	// Token: 0x06004683 RID: 18051 RVA: 0x0018EA58 File Offset: 0x0018CC58
	protected override void OnPrefabInit()
	{
		ScreenPrefabs.Instance = this;
	}

	// Token: 0x06004684 RID: 18052 RVA: 0x0018EA60 File Offset: 0x0018CC60
	public void ConfirmDoAction(string message, System.Action action, Transform parent)
	{
		((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent.gameObject)).PopupConfirmDialog(message, action, delegate
		{
		}, null, null, null, null, null, null);
	}

	// Token: 0x04002EAB RID: 11947
	public ControlsScreen ControlsScreen;

	// Token: 0x04002EAC RID: 11948
	public Hud HudScreen;

	// Token: 0x04002EAD RID: 11949
	public HoverTextScreen HoverTextScreen;

	// Token: 0x04002EAE RID: 11950
	public OverlayScreen OverlayScreen;

	// Token: 0x04002EAF RID: 11951
	public TileScreen TileScreen;

	// Token: 0x04002EB0 RID: 11952
	public SpeedControlScreen SpeedControlScreen;

	// Token: 0x04002EB1 RID: 11953
	public ManagementMenu ManagementMenu;

	// Token: 0x04002EB2 RID: 11954
	public ToolTipScreen ToolTipScreen;

	// Token: 0x04002EB3 RID: 11955
	public DebugPaintElementScreen DebugPaintElementScreen;

	// Token: 0x04002EB4 RID: 11956
	public UserMenuScreen UserMenuScreen;

	// Token: 0x04002EB5 RID: 11957
	public KButtonMenu OwnerScreen;

	// Token: 0x04002EB6 RID: 11958
	public EnergyInfoScreen EnergyInfoScreen;

	// Token: 0x04002EB7 RID: 11959
	public KButtonMenu ButtonGrid;

	// Token: 0x04002EB8 RID: 11960
	public NameDisplayScreen NameDisplayScreen;

	// Token: 0x04002EB9 RID: 11961
	public ConfirmDialogScreen ConfirmDialogScreen;

	// Token: 0x04002EBA RID: 11962
	public CustomizableDialogScreen CustomizableDialogScreen;

	// Token: 0x04002EBB RID: 11963
	public SpriteListDialogScreen SpriteListDialogScreen;

	// Token: 0x04002EBC RID: 11964
	public InfoDialogScreen InfoDialogScreen;

	// Token: 0x04002EBD RID: 11965
	public StoryMessageScreen StoryMessageScreen;

	// Token: 0x04002EBE RID: 11966
	public SubSpeciesInfoScreen SubSpeciesInfoScreen;

	// Token: 0x04002EBF RID: 11967
	public EventInfoScreen eventInfoScreen;

	// Token: 0x04002EC0 RID: 11968
	public FileNameDialog FileNameDialog;

	// Token: 0x04002EC1 RID: 11969
	public TagFilterScreen TagFilterScreen;

	// Token: 0x04002EC2 RID: 11970
	public ResearchScreen ResearchScreen;

	// Token: 0x04002EC3 RID: 11971
	public MessageDialogFrame MessageDialogFrame;

	// Token: 0x04002EC4 RID: 11972
	public ResourceCategoryScreen ResourceCategoryScreen;

	// Token: 0x04002EC5 RID: 11973
	public ColonyDiagnosticScreen ColonyDiagnosticScreen;

	// Token: 0x04002EC6 RID: 11974
	public LanguageOptionsScreen languageOptionsScreen;

	// Token: 0x04002EC7 RID: 11975
	public ModsScreen modsMenu;

	// Token: 0x04002EC8 RID: 11976
	public RailModUploadScreen RailModUploadMenu;

	// Token: 0x04002EC9 RID: 11977
	public GameObject GameOverScreen;

	// Token: 0x04002ECA RID: 11978
	public GameObject VictoryScreen;

	// Token: 0x04002ECB RID: 11979
	public GameObject StatusItemIndicatorScreen;

	// Token: 0x04002ECC RID: 11980
	public GameObject CollapsableContentPanel;

	// Token: 0x04002ECD RID: 11981
	public GameObject DescriptionLabel;

	// Token: 0x04002ECE RID: 11982
	public LoadingOverlay loadingOverlay;

	// Token: 0x04002ECF RID: 11983
	public LoadScreen LoadScreen;

	// Token: 0x04002ED0 RID: 11984
	public InspectSaveScreen InspectSaveScreen;

	// Token: 0x04002ED1 RID: 11985
	public OptionsMenuScreen OptionsScreen;

	// Token: 0x04002ED2 RID: 11986
	public WorldGenScreen WorldGenScreen;

	// Token: 0x04002ED3 RID: 11987
	public ModeSelectScreen ModeSelectScreen;

	// Token: 0x04002ED4 RID: 11988
	public ColonyDestinationSelectScreen ColonyDestinationSelectScreen;

	// Token: 0x04002ED5 RID: 11989
	public RetiredColonyInfoScreen RetiredColonyInfoScreen;

	// Token: 0x04002ED6 RID: 11990
	public VideoScreen VideoScreen;

	// Token: 0x04002ED7 RID: 11991
	public ComicViewer ComicViewer;

	// Token: 0x04002ED8 RID: 11992
	public GameObject OldVersionWarningScreen;

	// Token: 0x04002ED9 RID: 11993
	[Header("Klei Items")]
	public GameObject KleiItemDropScreen;

	// Token: 0x04002EDA RID: 11994
	public GameObject LockerMenuScreen;

	// Token: 0x04002EDB RID: 11995
	public GameObject LockerNavigator;

	// Token: 0x04002EDC RID: 11996
	[Header("Main Menu")]
	public GameObject MainMenuForVanilla;

	// Token: 0x04002EDD RID: 11997
	public GameObject MainMenuForSpacedOut;

	// Token: 0x04002EDE RID: 11998
	public GameObject MainMenuIntroShort;

	// Token: 0x04002EDF RID: 11999
	public GameObject MainMenuHealthyGameMessage;
}
