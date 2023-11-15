using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Klei;
using KMod;
using Newtonsoft.Json;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x02000839 RID: 2105
public class KCrashReporter : MonoBehaviour
{
	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06003D29 RID: 15657 RVA: 0x001536D0 File Offset: 0x001518D0
	// (remove) Token: 0x06003D2A RID: 15658 RVA: 0x00153704 File Offset: 0x00151904
	public static event Action<bool> onCrashReported;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06003D2B RID: 15659 RVA: 0x00153738 File Offset: 0x00151938
	// (remove) Token: 0x06003D2C RID: 15660 RVA: 0x0015376C File Offset: 0x0015196C
	public static event Action<float> onCrashUploadProgress;

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x06003D2D RID: 15661 RVA: 0x0015379F File Offset: 0x0015199F
	// (set) Token: 0x06003D2E RID: 15662 RVA: 0x001537A6 File Offset: 0x001519A6
	public static bool hasReportedError { get; private set; }

	// Token: 0x06003D2F RID: 15663 RVA: 0x001537B0 File Offset: 0x001519B0
	private void OnEnable()
	{
		KCrashReporter.dataRoot = Application.dataPath;
		Application.logMessageReceived += this.HandleLog;
		KCrashReporter.ignoreAll = true;
		string path = Path.Combine(KCrashReporter.dataRoot, "hashes.json");
		if (File.Exists(path))
		{
			StringBuilder stringBuilder = new StringBuilder();
			MD5 md = MD5.Create();
			Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
			if (dictionary.Count > 0)
			{
				bool flag = true;
				foreach (KeyValuePair<string, string> keyValuePair in dictionary)
				{
					string key = keyValuePair.Key;
					string value = keyValuePair.Value;
					stringBuilder.Length = 0;
					using (FileStream fileStream = new FileStream(Path.Combine(KCrashReporter.dataRoot, key), FileMode.Open, FileAccess.Read))
					{
						foreach (byte b in md.ComputeHash(fileStream))
						{
							stringBuilder.AppendFormat("{0:x2}", b);
						}
						if (stringBuilder.ToString() != value)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					KCrashReporter.ignoreAll = false;
				}
			}
			else
			{
				KCrashReporter.ignoreAll = false;
			}
		}
		else
		{
			KCrashReporter.ignoreAll = false;
		}
		if (KCrashReporter.ignoreAll)
		{
			global::Debug.Log("Ignoring crash due to mismatched hashes.json entries.");
		}
		if (File.Exists("ignorekcrashreporter.txt"))
		{
			KCrashReporter.ignoreAll = true;
			global::Debug.Log("Ignoring crash due to ignorekcrashreporter.txt");
		}
		if (Application.isEditor && !GenericGameSettings.instance.enableEditorCrashReporting)
		{
			KCrashReporter.terminateOnError = false;
		}
	}

	// Token: 0x06003D30 RID: 15664 RVA: 0x00153958 File Offset: 0x00151B58
	private void OnDisable()
	{
		Application.logMessageReceived -= this.HandleLog;
	}

	// Token: 0x06003D31 RID: 15665 RVA: 0x0015396C File Offset: 0x00151B6C
	private void HandleLog(string msg, string stack_trace, LogType type)
	{
		if ((KCrashReporter.logCount += 1U) == 10000000U)
		{
			DebugUtil.DevLogError("Turning off logging to avoid increasing the file to an unreasonable size, please review the logs as they probably contain spam");
			global::Debug.DisableLogging();
		}
		if (KCrashReporter.ignoreAll)
		{
			return;
		}
		if (Array.IndexOf<string>(KCrashReporter.IgnoreStrings, msg) != -1)
		{
			return;
		}
		if (msg != null && msg.StartsWith("<RI.Hid>"))
		{
			return;
		}
		if (msg != null && msg.StartsWith("Failed to load cursor"))
		{
			return;
		}
		if (msg != null && msg.StartsWith("Failed to save a temporary cursor"))
		{
			return;
		}
		if (type == LogType.Exception)
		{
			RestartWarning.ShouldWarn = true;
		}
		if (this.errorScreen == null && (type == LogType.Exception || type == LogType.Error))
		{
			if (KCrashReporter.terminateOnError && KCrashReporter.hasCrash)
			{
				return;
			}
			if (SpeedControlScreen.Instance != null)
			{
				SpeedControlScreen.Instance.Pause(true, true);
			}
			string text = stack_trace;
			if (string.IsNullOrEmpty(text))
			{
				text = new StackTrace(5, true).ToString();
			}
			if (App.isLoading)
			{
				if (!SceneInitializerLoader.deferred_error.IsValid)
				{
					SceneInitializerLoader.deferred_error = new SceneInitializerLoader.DeferredError
					{
						msg = msg,
						stack_trace = text
					};
					return;
				}
			}
			else
			{
				this.ShowDialog(msg, text);
			}
		}
	}

	// Token: 0x06003D32 RID: 15666 RVA: 0x00153A84 File Offset: 0x00151C84
	public bool ShowDialog(string error, string stack_trace)
	{
		if (this.errorScreen != null)
		{
			return false;
		}
		GameObject gameObject = GameObject.Find(KCrashReporter.error_canvas_name);
		if (gameObject == null)
		{
			gameObject = new GameObject();
			gameObject.name = KCrashReporter.error_canvas_name;
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
			canvas.sortingOrder = 32767;
			gameObject.AddComponent<GraphicRaycaster>();
		}
		this.errorScreen = UnityEngine.Object.Instantiate<GameObject>(this.reportErrorPrefab, Vector3.zero, Quaternion.identity);
		this.errorScreen.transform.SetParent(gameObject.transform, false);
		ReportErrorDialog errorDialog = this.errorScreen.GetComponentInChildren<ReportErrorDialog>();
		string stackTrace = error + "\n\n" + stack_trace;
		KCrashReporter.hasCrash = true;
		if (Global.Instance != null && Global.Instance.modManager != null && Global.Instance.modManager.HasCrashableMods())
		{
			Exception ex = DebugUtil.RetrieveLastExceptionLogged();
			StackTrace stackTrace2 = (ex != null) ? new StackTrace(ex) : new StackTrace(5, true);
			Global.Instance.modManager.SearchForModsInStackTrace(stackTrace2);
			Global.Instance.modManager.SearchForModsInStackTrace(stack_trace);
			errorDialog.PopupDisableModsDialog(stackTrace, new System.Action(this.OnQuitToDesktop), (Global.Instance.modManager.IsInDevMode() || !KCrashReporter.terminateOnError) ? new System.Action(this.OnCloseErrorDialog) : null);
		}
		else
		{
			errorDialog.PopupSubmitErrorDialog(stackTrace, delegate
			{
				KCrashReporter.ReportError(error, stack_trace, this.confirmDialogPrefab, this.errorScreen, errorDialog.UserMessage(), true, null, null);
			}, new System.Action(this.OnQuitToDesktop), KCrashReporter.terminateOnError ? null : new System.Action(this.OnCloseErrorDialog));
		}
		return true;
	}

	// Token: 0x06003D33 RID: 15667 RVA: 0x00153C55 File Offset: 0x00151E55
	private void OnCloseErrorDialog()
	{
		UnityEngine.Object.Destroy(this.errorScreen);
		this.errorScreen = null;
		KCrashReporter.hasCrash = false;
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
	}

	// Token: 0x06003D34 RID: 15668 RVA: 0x00153C87 File Offset: 0x00151E87
	private void OnQuitToDesktop()
	{
		App.Quit();
	}

	// Token: 0x06003D35 RID: 15669 RVA: 0x00153C90 File Offset: 0x00151E90
	private static string GetUserID()
	{
		if (DistributionPlatform.Initialized)
		{
			string[] array = new string[5];
			array[0] = DistributionPlatform.Inst.Name;
			array[1] = "ID_";
			array[2] = DistributionPlatform.Inst.LocalUser.Name;
			array[3] = "_";
			int num = 4;
			DistributionPlatform.UserId id = DistributionPlatform.Inst.LocalUser.Id;
			array[num] = ((id != null) ? id.ToString() : null);
			return string.Concat(array);
		}
		return "LocalUser_" + Environment.UserName;
	}

	// Token: 0x06003D36 RID: 15670 RVA: 0x00153D0C File Offset: 0x00151F0C
	private static string GetLogContents()
	{
		string path = Util.LogFilePath();
		if (File.Exists(path))
		{
			using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}
		return "";
	}

	// Token: 0x06003D37 RID: 15671 RVA: 0x00153D78 File Offset: 0x00151F78
	public static void ReportDevNotification(string notification_name, string stack_trace, string details = "", bool includeSaveFile = false)
	{
		if (KCrashReporter.previouslyReportedDevNotifications == null)
		{
			KCrashReporter.previouslyReportedDevNotifications = new HashSet<int>();
		}
		details = notification_name + " - " + details;
		global::Debug.Log(details);
		int hashValue = new HashedString(notification_name).HashValue;
		bool hasReportedError = KCrashReporter.hasReportedError;
		if (!KCrashReporter.previouslyReportedDevNotifications.Contains(hashValue))
		{
			KCrashReporter.previouslyReportedDevNotifications.Add(hashValue);
			KCrashReporter.ReportError("DevNotification: " + notification_name, stack_trace, null, null, details, includeSaveFile, new string[]
			{
				KCrashReporter.Error.CATEGORIES.DEVNOTIFICATION
			}, null);
		}
		KCrashReporter.hasReportedError = hasReportedError;
	}

	// Token: 0x06003D38 RID: 15672 RVA: 0x00153E04 File Offset: 0x00152004
	public static void ReportError(string msg, string stack_trace, ConfirmDialogScreen confirm_prefab, GameObject confirm_parent, string userMessage = "", bool includeSaveFile = true, string[] extraCategories = null, string[] extraFiles = null)
	{
		if (KPrivacyPrefs.instance.disableDataCollection)
		{
			return;
		}
		if (KCrashReporter.ignoreAll)
		{
			return;
		}
		global::Debug.Log("Reporting error.\n");
		if (msg != null)
		{
			global::Debug.Log(msg);
		}
		if (stack_trace != null)
		{
			global::Debug.Log(stack_trace);
		}
		KCrashReporter.hasReportedError = true;
		if (string.IsNullOrEmpty(msg))
		{
			msg = "No message";
		}
		Match match = KCrashReporter.failedToLoadModuleRegEx.Match(msg);
		if (match.Success)
		{
			string path = match.Groups[1].ToString();
			string text = match.Groups[2].ToString();
			string fileName = Path.GetFileName(path);
			msg = string.Concat(new string[]
			{
				"Failed to load '",
				fileName,
				"' with error '",
				text,
				"'."
			});
		}
		if (string.IsNullOrEmpty(stack_trace))
		{
			string buildText = BuildWatermark.GetBuildText();
			stack_trace = string.Format("No stack trace {0}\n\n{1}", buildText, msg);
		}
		List<string> list = new List<string>();
		if (KCrashReporter.debugWasUsed)
		{
			list.Add("(Debug Used)");
		}
		if (KCrashReporter.haveActiveMods)
		{
			list.Add("(Mods Active)");
		}
		list.Add(msg);
		string[] array = new string[]
		{
			"Debug:LogError",
			"UnityEngine.Debug",
			"Output:LogError",
			"DebugUtil:Assert",
			"System.Array",
			"System.Collections",
			"KCrashReporter.Assert",
			"No stack trace."
		};
		foreach (string text2 in stack_trace.Split(new char[]
		{
			'\n'
		}))
		{
			if (list.Count >= 5)
			{
				break;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				bool flag = false;
				foreach (string value in array)
				{
					if (text2.StartsWith(value))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(text2);
				}
			}
		}
		if (userMessage == UI.CRASHSCREEN.BODY.text || userMessage.IsNullOrWhiteSpace())
		{
			userMessage = "";
		}
		else
		{
			userMessage = "[" + BuildWatermark.GetBuildText() + "]" + userMessage;
		}
		userMessage = userMessage.Replace(stack_trace, "");
		KCrashReporter.Error error = new KCrashReporter.Error();
		if (extraCategories != null)
		{
			error.categories.AddRange(extraCategories);
		}
		error.callstack = stack_trace;
		if (KCrashReporter.disableDeduping)
		{
			error.callstack = error.callstack + "\n" + Guid.NewGuid().ToString();
		}
		error.fullstack = string.Format("{0}\n\n{1}", msg, stack_trace);
		error.summaryline = string.Join("\n", list.ToArray());
		error.userMessage = userMessage;
		List<string> list2 = new List<string>();
		if (includeSaveFile && KCrashReporter.MOST_RECENT_SAVEFILE != null)
		{
			list2.Add(KCrashReporter.MOST_RECENT_SAVEFILE);
			error.saveFilename = Path.GetFileName(KCrashReporter.MOST_RECENT_SAVEFILE);
		}
		if (extraFiles != null)
		{
			foreach (string text3 in extraFiles)
			{
				list2.Add(text3);
				error.extraFilenames.Add(Path.GetFileName(text3));
			}
		}
		string jsonString = JsonConvert.SerializeObject(error);
		byte[] archiveData = KCrashReporter.CreateArchiveZip(KCrashReporter.GetLogContents(), list2);
		global::Debug.Log("Submitting crash...");
		System.Action successCallback = delegate()
		{
			if (confirm_prefab != null && confirm_parent != null)
			{
				((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, confirm_parent)).PopupConfirmDialog(UI.CRASHSCREEN.REPORTEDERROR_SUCCESS, null, null, null, null, null, null, null, null);
			}
		};
		System.Action failureCallback = delegate()
		{
			if (confirm_prefab != null && confirm_parent != null)
			{
				((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, confirm_parent)).PopupConfirmDialog(UI.CRASHSCREEN.REPORTEDERROR_FAILURE, null, null, null, null, null, null, null, null);
			}
		};
		Global.Instance.StartCoroutine(KCrashReporter.SubmitCrashAsync(jsonString, archiveData, successCallback, failureCallback));
	}

	// Token: 0x06003D39 RID: 15673 RVA: 0x0015417F File Offset: 0x0015237F
	private static IEnumerator SubmitCrashAsync(string jsonString, byte[] archiveData, System.Action successCallback, System.Action failureCallback)
	{
		bool success = false;
		Uri uri = new Uri("https://games-feedback.klei.com/submit");
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("metadata", jsonString);
		if (KleiAccount.KleiToken != null)
		{
			wwwform.AddField("loginToken", KleiAccount.KleiToken);
		}
		wwwform.AddBinaryData("archiveFile", archiveData, "Archive.zip", "application/octet-stream");
		using (UnityWebRequest w = UnityWebRequest.Post(uri, wwwform))
		{
			w.SendWebRequest();
			while (!w.isDone)
			{
				yield return null;
				if (KCrashReporter.onCrashUploadProgress != null)
				{
					KCrashReporter.onCrashUploadProgress(w.uploadProgress);
				}
			}
			if (w.result == UnityWebRequest.Result.Success)
			{
				if (successCallback != null)
				{
					successCallback();
				}
				success = true;
			}
			else
			{
				UnityEngine.Debug.Log("CrashReporter: Could not submit crash " + w.result.ToString());
				if (failureCallback != null)
				{
					failureCallback();
				}
			}
		}
		UnityWebRequest w = null;
		if (KCrashReporter.onCrashReported != null)
		{
			KCrashReporter.onCrashReported(success);
		}
		yield break;
		yield break;
	}

	// Token: 0x06003D3A RID: 15674 RVA: 0x001541A4 File Offset: 0x001523A4
	public static void ReportBug(string msg, GameObject confirmParent)
	{
		string stack_trace = "Bug Report From: " + KCrashReporter.GetUserID() + " at " + System.DateTime.Now.ToString();
		KCrashReporter.ReportError(msg, stack_trace, ScreenPrefabs.Instance.ConfirmDialogScreen, confirmParent, "", true, null, null);
	}

	// Token: 0x06003D3B RID: 15675 RVA: 0x001541F0 File Offset: 0x001523F0
	public static void Assert(bool condition, string message)
	{
		if (!condition && !KCrashReporter.hasReportedError)
		{
			StackTrace stackTrace = new StackTrace(1, true);
			KCrashReporter.ReportError("ASSERT: " + message, stackTrace.ToString(), null, null, null, true, null, null);
		}
	}

	// Token: 0x06003D3C RID: 15676 RVA: 0x0015422C File Offset: 0x0015242C
	public static void ReportSimDLLCrash(string msg, string stack_trace, string dmp_filename)
	{
		if (KCrashReporter.hasReportedError)
		{
			return;
		}
		KCrashReporter.ReportError(msg, stack_trace, null, null, "", true, new string[]
		{
			KCrashReporter.Error.CATEGORIES.SIM
		}, new string[]
		{
			dmp_filename
		});
	}

	// Token: 0x06003D3D RID: 15677 RVA: 0x00154268 File Offset: 0x00152468
	private static byte[] CreateArchiveZip(string log, List<string> files)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
			{
				if (files != null)
				{
					using (Stream stream = zipArchive.CreateEntry("Player.log", System.IO.Compression.CompressionLevel.Fastest).Open())
					{
						byte[] bytes = Encoding.UTF8.GetBytes(log);
						stream.Write(bytes, 0, bytes.Length);
					}
					foreach (string text in files)
					{
						try
						{
							if (!File.Exists(text))
							{
								UnityEngine.Debug.Log("CrashReporter: file does not exist to include: " + text);
							}
							else
							{
								using (Stream stream2 = zipArchive.CreateEntry(Path.GetFileName(text), System.IO.Compression.CompressionLevel.Fastest).Open())
								{
									byte[] array = File.ReadAllBytes(text);
									stream2.Write(array, 0, array.Length);
								}
							}
						}
						catch (Exception ex)
						{
							string str = "CrashReporter: Could not add file '";
							string str2 = text;
							string str3 = "' to archive: ";
							Exception ex2 = ex;
							UnityEngine.Debug.Log(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
						}
					}
				}
			}
			result = memoryStream.ToArray();
		}
		return result;
	}

	// Token: 0x040027E8 RID: 10216
	public static string MOST_RECENT_SAVEFILE = null;

	// Token: 0x040027E9 RID: 10217
	public const string CRASH_REPORTER_SERVER = "https://games-feedback.klei.com";

	// Token: 0x040027EA RID: 10218
	public const uint MAX_LOGS = 10000000U;

	// Token: 0x040027ED RID: 10221
	public static bool ignoreAll = false;

	// Token: 0x040027EE RID: 10222
	public static bool debugWasUsed = false;

	// Token: 0x040027EF RID: 10223
	public static bool haveActiveMods = false;

	// Token: 0x040027F0 RID: 10224
	public static uint logCount = 0U;

	// Token: 0x040027F1 RID: 10225
	public static string error_canvas_name = "ErrorCanvas";

	// Token: 0x040027F2 RID: 10226
	public static bool disableDeduping = false;

	// Token: 0x040027F4 RID: 10228
	public static bool hasCrash = false;

	// Token: 0x040027F5 RID: 10229
	private static readonly Regex failedToLoadModuleRegEx = new Regex("^Failed to load '(.*?)' with error (.*)", RegexOptions.Multiline);

	// Token: 0x040027F6 RID: 10230
	[SerializeField]
	private LoadScreen loadScreenPrefab;

	// Token: 0x040027F7 RID: 10231
	[SerializeField]
	private GameObject reportErrorPrefab;

	// Token: 0x040027F8 RID: 10232
	[SerializeField]
	private ConfirmDialogScreen confirmDialogPrefab;

	// Token: 0x040027F9 RID: 10233
	private GameObject errorScreen;

	// Token: 0x040027FA RID: 10234
	public static bool terminateOnError = true;

	// Token: 0x040027FB RID: 10235
	private static string dataRoot;

	// Token: 0x040027FC RID: 10236
	private static readonly string[] IgnoreStrings = new string[]
	{
		"Releasing render texture whose render buffer is set as Camera's target buffer with Camera.SetTargetBuffers!",
		"The profiler has run out of samples for this frame. This frame will be skipped. Increase the sample limit using Profiler.maxNumberOfSamplesPerFrame",
		"Trying to add Text (LocText) for graphic rebuild while we are already inside a graphic rebuild loop. This is not supported.",
		"Texture has out of range width / height",
		"<I> Failed to get cursor position:\r\nSuccess.\r\n"
	};

	// Token: 0x040027FD RID: 10237
	private static HashSet<int> previouslyReportedDevNotifications;

	// Token: 0x02001607 RID: 5639
	private class Error
	{
		// Token: 0x06008925 RID: 35109 RVA: 0x00310B1C File Offset: 0x0030ED1C
		public Error()
		{
			this.userName = KCrashReporter.GetUserID();
			this.InitDefaultCategories();
			this.InitSku();
			this.InitSlackSummary();
			if (DistributionPlatform.Inst.Initialized)
			{
				string a;
				bool currentBetaName = SteamApps.GetCurrentBetaName(out a, 100);
				this.branch = a;
				if (currentBetaName || (a == "public_testing" && !UnityEngine.Debug.isDebugBuild))
				{
					this.branch = "default";
				}
			}
		}

		// Token: 0x06008926 RID: 35110 RVA: 0x00310C4C File Offset: 0x0030EE4C
		private void InitDefaultCategories()
		{
			if (DlcManager.IsPureVanilla())
			{
				this.categories.Add(KCrashReporter.Error.CATEGORIES.VANILLA);
			}
			if (DlcManager.IsExpansion1Active())
			{
				this.categories.Add(KCrashReporter.Error.CATEGORIES.SPACEDOUT);
			}
			if (KCrashReporter.debugWasUsed)
			{
				this.categories.Add(KCrashReporter.Error.CATEGORIES.DEBUGUSED);
			}
			if (KCrashReporter.haveActiveMods)
			{
				this.categories.Add(KCrashReporter.Error.CATEGORIES.MODDED);
			}
			if (SaveGame.Instance != null && SaveGame.Instance.sandboxEnabled)
			{
				this.categories.Add(KCrashReporter.Error.CATEGORIES.SANDBOX);
			}
			if (DistributionPlatform.Inst.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				this.categories.Add(KCrashReporter.Error.CATEGORIES.STEAMDECK);
			}
		}

		// Token: 0x06008927 RID: 35111 RVA: 0x00310D04 File Offset: 0x0030EF04
		private void InitSku()
		{
			this.sku = "steam";
			if (DistributionPlatform.Inst.Initialized)
			{
				string a;
				bool flag = !SteamApps.GetCurrentBetaName(out a, 100);
				if (a == "public_testing" || a == "preview")
				{
					if (UnityEngine.Debug.isDebugBuild)
					{
						this.sku = "steam-public-testing";
					}
					else
					{
						this.sku = "steam-release";
					}
				}
				if (flag || a == "release")
				{
					this.sku = "steam-release";
				}
			}
		}

		// Token: 0x06008928 RID: 35112 RVA: 0x00310D88 File Offset: 0x0030EF88
		private void InitSlackSummary()
		{
			string buildText = BuildWatermark.GetBuildText();
			string text = (GameClock.Instance != null) ? string.Format(" - Cycle {0}", GameClock.Instance.GetCycle() + 1) : "";
			int num;
			if (!(Global.Instance != null) || Global.Instance.modManager == null)
			{
				num = 0;
			}
			else
			{
				num = Global.Instance.modManager.mods.Count((Mod x) => x.IsEnabledForActiveDlc());
			}
			int num2 = num;
			string text2 = (num2 > 0) ? string.Format(" - {0} active mods", num2) : "";
			this.slackSummary = string.Concat(new string[]
			{
				buildText,
				" ",
				this.platform,
				text,
				text2
			});
		}

		// Token: 0x04006A36 RID: 27190
		public string game = "ONI";

		// Token: 0x04006A37 RID: 27191
		public string userName;

		// Token: 0x04006A38 RID: 27192
		public string platform = SystemInfo.operatingSystem;

		// Token: 0x04006A39 RID: 27193
		public string version = LaunchInitializer.BuildPrefix();

		// Token: 0x04006A3A RID: 27194
		public string branch = "default";

		// Token: 0x04006A3B RID: 27195
		public string sku = "";

		// Token: 0x04006A3C RID: 27196
		public int build = 577063;

		// Token: 0x04006A3D RID: 27197
		public string callstack = "";

		// Token: 0x04006A3E RID: 27198
		public string fullstack = "";

		// Token: 0x04006A3F RID: 27199
		public string summaryline = "";

		// Token: 0x04006A40 RID: 27200
		public string userMessage = "";

		// Token: 0x04006A41 RID: 27201
		public List<string> categories = new List<string>();

		// Token: 0x04006A42 RID: 27202
		public string slackSummary;

		// Token: 0x04006A43 RID: 27203
		public string logFilename = "Player.log";

		// Token: 0x04006A44 RID: 27204
		public string saveFilename = "";

		// Token: 0x04006A45 RID: 27205
		public string screenshotFilename = "";

		// Token: 0x04006A46 RID: 27206
		public List<string> extraFilenames = new List<string>();

		// Token: 0x04006A47 RID: 27207
		public string title = "";

		// Token: 0x04006A48 RID: 27208
		public bool isServer;

		// Token: 0x04006A49 RID: 27209
		public bool isDedicated;

		// Token: 0x04006A4A RID: 27210
		public bool isError = true;

		// Token: 0x04006A4B RID: 27211
		public string emote = "";

		// Token: 0x0200219C RID: 8604
		public class CATEGORIES
		{
			// Token: 0x0400969B RID: 38555
			public static string DEVNOTIFICATION = "DevNotification";

			// Token: 0x0400969C RID: 38556
			public static string VANILLA = "Vanilla";

			// Token: 0x0400969D RID: 38557
			public static string SPACEDOUT = "SpacedOut";

			// Token: 0x0400969E RID: 38558
			public static string MODDED = "Modded";

			// Token: 0x0400969F RID: 38559
			public static string DEBUGUSED = "DebugUsed";

			// Token: 0x040096A0 RID: 38560
			public static string SANDBOX = "Sandbox";

			// Token: 0x040096A1 RID: 38561
			public static string STEAMDECK = "SteamDeck";

			// Token: 0x040096A2 RID: 38562
			public static string SIM = "SimDll";
		}
	}
}
