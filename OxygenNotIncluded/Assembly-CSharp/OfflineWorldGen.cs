using System;
using System.Collections.Generic;
using System.Threading;
using Klei.CustomSettings;
using ProcGenGame;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000CA4 RID: 3236
[AddComponentMenu("KMonoBehaviour/scripts/OfflineWorldGen")]
public class OfflineWorldGen : KMonoBehaviour
{
	// Token: 0x06006702 RID: 26370 RVA: 0x0026744C File Offset: 0x0026564C
	private void TrackProgress(string text)
	{
		if (this.trackProgress)
		{
			global::Debug.Log(text);
		}
	}

	// Token: 0x06006703 RID: 26371 RVA: 0x0026745C File Offset: 0x0026565C
	public static bool CanLoadSave()
	{
		bool flag = WorldGen.CanLoad(SaveLoader.GetActiveSaveFilePath());
		if (!flag)
		{
			SaveLoader.SetActiveSaveFilePath(null);
			flag = WorldGen.CanLoad(WorldGen.GetSIMSaveFilename(0));
		}
		return flag;
	}

	// Token: 0x06006704 RID: 26372 RVA: 0x0026748C File Offset: 0x0026568C
	public void Generate()
	{
		this.doWorldGen = !OfflineWorldGen.CanLoadSave();
		this.updateText.gameObject.SetActive(false);
		this.percentText.gameObject.SetActive(false);
		this.doWorldGen |= this.debug;
		if (this.doWorldGen)
		{
			this.seedText.text = string.Format(UI.WORLDGEN.USING_PLAYER_SEED, this.seed);
			this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.TITLE.ToString();
			this.mainText.text = UI.WORLDGEN.CHOOSEWORLDSIZE.ToString();
			for (int i = 0; i < this.validDimensions.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
				gameObject.SetActive(true);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(this.buttonRoot);
				component.localScale = Vector3.one;
				TMP_Text componentInChildren = gameObject.GetComponentInChildren<LocText>();
				OfflineWorldGen.ValidDimensions validDimensions = this.validDimensions[i];
				componentInChildren.text = validDimensions.name.ToString();
				int idx = i;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.DoWorldGen(idx);
					this.ToggleGenerationUI();
				};
			}
			if (this.validDimensions.Length == 1)
			{
				this.DoWorldGen(0);
				this.ToggleGenerationUI();
			}
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
			this.OnResize();
		}
		else
		{
			this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.LOADINGGAME.ToString();
			this.mainText.gameObject.SetActive(false);
			this.currentConvertedCurrentStage = UI.WORLDGEN.COMPLETE.key;
			this.currentPercent = 1f;
			this.updateText.gameObject.SetActive(false);
			this.percentText.gameObject.SetActive(false);
			this.RemoveButtons();
		}
		this.buttonPrefab.SetActive(false);
	}

	// Token: 0x06006705 RID: 26373 RVA: 0x0026768C File Offset: 0x0026588C
	private void OnResize()
	{
		float canvasScale = base.GetComponentInParent<KCanvasScaler>().GetCanvasScale();
		if (this.asteriodAnim != null)
		{
			this.asteriodAnim.animScale = 0.005f * (1f / canvasScale);
		}
	}

	// Token: 0x06006706 RID: 26374 RVA: 0x002676D4 File Offset: 0x002658D4
	private void ToggleGenerationUI()
	{
		this.percentText.gameObject.SetActive(false);
		this.updateText.gameObject.SetActive(true);
		this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.GENERATINGWORLD.ToString();
		if (this.titleText != null && this.titleText.gameObject != null)
		{
			this.titleText.gameObject.SetActive(false);
		}
		if (this.buttonRoot != null && this.buttonRoot.gameObject != null)
		{
			this.buttonRoot.gameObject.SetActive(false);
		}
	}

	// Token: 0x06006707 RID: 26375 RVA: 0x0026777C File Offset: 0x0026597C
	private bool UpdateProgress(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage)
	{
		if (this.currentStage != stage)
		{
			this.currentStage = stage;
		}
		if (this.currentStringKeyRoot.Hash != stringKeyRoot.Hash)
		{
			this.currentConvertedCurrentStage = stringKeyRoot;
			this.currentStringKeyRoot = stringKeyRoot;
		}
		else
		{
			int num = (int)completePercent * 10;
			LocString locString = this.convertList.Find((LocString s) => s.key.Hash == stringKeyRoot.Hash);
			if (num != 0 && locString != null)
			{
				this.currentConvertedCurrentStage = new StringKey(locString.key.String + num.ToString());
			}
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4 = WorldGenProgressStages.StageWeights[(int)stage].Value * completePercent;
		for (int i = 0; i < WorldGenProgressStages.StageWeights.Length; i++)
		{
			num3 += WorldGenProgressStages.StageWeights[i].Value;
			if (i < (int)this.currentStage)
			{
				num2 += WorldGenProgressStages.StageWeights[i].Value;
			}
		}
		float num5 = (num2 + num4) / num3;
		this.currentPercent = num5;
		return !this.shouldStop;
	}

	// Token: 0x06006708 RID: 26376 RVA: 0x002678A4 File Offset: 0x00265AA4
	private void Update()
	{
		if (this.loadTriggered)
		{
			return;
		}
		if (this.currentConvertedCurrentStage.String == null)
		{
			return;
		}
		this.errorMutex.WaitOne();
		int count = this.errors.Count;
		this.errorMutex.ReleaseMutex();
		if (count > 0)
		{
			this.DoExitFlow();
			return;
		}
		this.updateText.text = Strings.Get(this.currentConvertedCurrentStage.String);
		if (!this.debug && this.currentConvertedCurrentStage.Hash == UI.WORLDGEN.COMPLETE.key.Hash && this.currentPercent >= 1f && this.clusterLayout.IsGenerationComplete)
		{
			if (KCrashReporter.terminateOnError && KCrashReporter.hasCrash)
			{
				return;
			}
			this.percentText.text = "";
			this.loadTriggered = true;
			App.LoadScene(this.mainGameLevel);
			return;
		}
		else
		{
			if (this.currentPercent < 0f)
			{
				this.DoExitFlow();
				return;
			}
			if (this.currentPercent > 0f && !this.percentText.gameObject.activeSelf)
			{
				this.percentText.gameObject.SetActive(false);
			}
			this.percentText.text = GameUtil.GetFormattedPercent(this.currentPercent * 100f, GameUtil.TimeSlice.None);
			this.meterAnim.SetPositionPercent(this.currentPercent);
			return;
		}
	}

	// Token: 0x06006709 RID: 26377 RVA: 0x002679F8 File Offset: 0x00265BF8
	private void DisplayErrors()
	{
		this.errorMutex.WaitOne();
		if (this.errors.Count > 0)
		{
			foreach (OfflineWorldGen.ErrorInfo errorInfo in this.errors)
			{
				Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, FrontEndManager.Instance.gameObject, true).PopupConfirmDialog(errorInfo.errorDesc, new System.Action(this.OnConfirmExit), null, null, null, null, null, null, null);
			}
		}
		this.errorMutex.ReleaseMutex();
	}

	// Token: 0x0600670A RID: 26378 RVA: 0x00267AA8 File Offset: 0x00265CA8
	private void DoExitFlow()
	{
		if (this.startedExitFlow)
		{
			return;
		}
		this.startedExitFlow = true;
		this.percentText.text = UI.WORLDGEN.RESTARTING.ToString();
		this.loadTriggered = true;
		Sim.Shutdown();
		this.DisplayErrors();
	}

	// Token: 0x0600670B RID: 26379 RVA: 0x00267AE1 File Offset: 0x00265CE1
	private void OnConfirmExit()
	{
		App.LoadScene(this.frontendGameLevel);
	}

	// Token: 0x0600670C RID: 26380 RVA: 0x00267AF0 File Offset: 0x00265CF0
	private void RemoveButtons()
	{
		for (int i = this.buttonRoot.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.buttonRoot.GetChild(i).gameObject);
		}
	}

	// Token: 0x0600670D RID: 26381 RVA: 0x00267B2B File Offset: 0x00265D2B
	private void DoWorldGen(int selectedDimension)
	{
		this.RemoveButtons();
		this.DoWorldGenInitialize();
	}

	// Token: 0x0600670E RID: 26382 RVA: 0x00267B3C File Offset: 0x00265D3C
	private void DoWorldGenInitialize()
	{
		string name = "";
		Func<int, WorldGen, bool> shouldSkipWorldCallback = null;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
		this.seed = int.Parse(currentQualitySetting.id);
		name = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
		List<string> list = new List<string>();
		foreach (string id in CustomGameSettings.Instance.GetCurrentStories())
		{
			list.Add(Db.Get().Stories.Get(id).worldgenStoryTraitKey);
		}
		this.clusterLayout = new Cluster(name, this.seed, list, true, false);
		this.clusterLayout.ShouldSkipWorldCallback = shouldSkipWorldCallback;
		this.clusterLayout.Generate(new WorldGen.OfflineCallbackFunction(this.UpdateProgress), new Action<OfflineWorldGen.ErrorInfo>(this.OnError), this.seed, this.seed, this.seed, this.seed, true, false);
	}

	// Token: 0x0600670F RID: 26383 RVA: 0x00267C4C File Offset: 0x00265E4C
	private void OnError(OfflineWorldGen.ErrorInfo error)
	{
		this.errorMutex.WaitOne();
		this.errors.Add(error);
		this.errorMutex.ReleaseMutex();
	}

	// Token: 0x04004736 RID: 18230
	[SerializeField]
	private RectTransform buttonRoot;

	// Token: 0x04004737 RID: 18231
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04004738 RID: 18232
	[SerializeField]
	private RectTransform chooseLocationPanel;

	// Token: 0x04004739 RID: 18233
	[SerializeField]
	private GameObject locationButtonPrefab;

	// Token: 0x0400473A RID: 18234
	private const float baseScale = 0.005f;

	// Token: 0x0400473B RID: 18235
	private Mutex errorMutex = new Mutex();

	// Token: 0x0400473C RID: 18236
	private List<OfflineWorldGen.ErrorInfo> errors = new List<OfflineWorldGen.ErrorInfo>();

	// Token: 0x0400473D RID: 18237
	private OfflineWorldGen.ValidDimensions[] validDimensions = new OfflineWorldGen.ValidDimensions[]
	{
		new OfflineWorldGen.ValidDimensions
		{
			width = 256,
			height = 384,
			name = UI.FRONTEND.WORLDGENSCREEN.SIZES.STANDARD.key
		}
	};

	// Token: 0x0400473E RID: 18238
	public string frontendGameLevel = "frontend";

	// Token: 0x0400473F RID: 18239
	public string mainGameLevel = "backend";

	// Token: 0x04004740 RID: 18240
	private bool shouldStop;

	// Token: 0x04004741 RID: 18241
	private StringKey currentConvertedCurrentStage;

	// Token: 0x04004742 RID: 18242
	private float currentPercent;

	// Token: 0x04004743 RID: 18243
	public bool debug;

	// Token: 0x04004744 RID: 18244
	private bool trackProgress = true;

	// Token: 0x04004745 RID: 18245
	private bool doWorldGen;

	// Token: 0x04004746 RID: 18246
	[SerializeField]
	private LocText titleText;

	// Token: 0x04004747 RID: 18247
	[SerializeField]
	private LocText mainText;

	// Token: 0x04004748 RID: 18248
	[SerializeField]
	private LocText updateText;

	// Token: 0x04004749 RID: 18249
	[SerializeField]
	private LocText percentText;

	// Token: 0x0400474A RID: 18250
	[SerializeField]
	private LocText seedText;

	// Token: 0x0400474B RID: 18251
	[SerializeField]
	private KBatchedAnimController meterAnim;

	// Token: 0x0400474C RID: 18252
	[SerializeField]
	private KBatchedAnimController asteriodAnim;

	// Token: 0x0400474D RID: 18253
	private Cluster clusterLayout;

	// Token: 0x0400474E RID: 18254
	private StringKey currentStringKeyRoot;

	// Token: 0x0400474F RID: 18255
	private List<LocString> convertList = new List<LocString>
	{
		UI.WORLDGEN.SETTLESIM,
		UI.WORLDGEN.BORDERS,
		UI.WORLDGEN.PROCESSING,
		UI.WORLDGEN.COMPLETELAYOUT,
		UI.WORLDGEN.WORLDLAYOUT,
		UI.WORLDGEN.GENERATENOISE,
		UI.WORLDGEN.BUILDNOISESOURCE,
		UI.WORLDGEN.GENERATESOLARSYSTEM
	};

	// Token: 0x04004750 RID: 18256
	private WorldGenProgressStages.Stages currentStage;

	// Token: 0x04004751 RID: 18257
	private bool loadTriggered;

	// Token: 0x04004752 RID: 18258
	private bool startedExitFlow;

	// Token: 0x04004753 RID: 18259
	private int seed;

	// Token: 0x02001BCC RID: 7116
	public struct ErrorInfo
	{
		// Token: 0x04007DF8 RID: 32248
		public string errorDesc;

		// Token: 0x04007DF9 RID: 32249
		public Exception exception;
	}

	// Token: 0x02001BCD RID: 7117
	[Serializable]
	private struct ValidDimensions
	{
		// Token: 0x04007DFA RID: 32250
		public int width;

		// Token: 0x04007DFB RID: 32251
		public int height;

		// Token: 0x04007DFC RID: 32252
		public StringKey name;
	}
}
