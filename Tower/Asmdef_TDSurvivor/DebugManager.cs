using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public class DebugManager : Singleton<DebugManager>
{
	// Token: 0x060005AC RID: 1452 RVA: 0x00016914 File Offset: 0x00014B14
	private new void Awake()
	{
		this.Initialize();
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0001691C File Offset: 0x00014B1C
	private void Initialize()
	{
		if (this.isInitialized)
		{
			return;
		}
		if (this.dic_Data == null)
		{
			this.dic_Data = new Dictionary<eDebugKey, DebugSettingData>();
		}
		DebugManager.Log(eDebugKey.BASE_SYSTEM, base.GetType().ToString() + " Init", null);
		this.timeFormat = DebugManager.eTimeFormat.DISABLED;
		this.fontSize = DebugManager.eFontSize.NORMAL;
		if (this.settings == null)
		{
			this.settings = (Resources.Load("DebugSettingData") as DebugSettingSO);
			if (this.settings == null)
			{
				this.BasicLog(DebugManager.eLogType.ERROR, "DebugManager沒有指定設定檔!!");
			}
		}
		else
		{
			foreach (DebugSettingData debugSettingData in this.settings.list_DebugSettingData)
			{
				if (!this.dic_Data.ContainsKey(debugSettingData.Key))
				{
					this.dic_Data.Add(debugSettingData.Key, debugSettingData);
				}
			}
		}
		this.isInitialized = true;
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00016A20 File Offset: 0x00014C20
	public static void Log(eDebugKey key, string msg, Object context = null)
	{
		Singleton<DebugManager>.Instance.log(DebugManager.eLogType.LOG, key, msg, context);
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00016A30 File Offset: 0x00014C30
	public static void LogWarning(eDebugKey key, string msg, Object context = null)
	{
		Singleton<DebugManager>.Instance.log(DebugManager.eLogType.WARNING, key, msg, context);
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00016A40 File Offset: 0x00014C40
	public static void LogError(eDebugKey key, string msg, Object context = null)
	{
		Singleton<DebugManager>.Instance.log(DebugManager.eLogType.ERROR, key, msg, context);
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00016A50 File Offset: 0x00014C50
	public static void LogAssertion(eDebugKey key, string msg, Object context = null)
	{
		Singleton<DebugManager>.Instance.log(DebugManager.eLogType.ASSERTION, key, msg, context);
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00016A60 File Offset: 0x00014C60
	private void log(DebugManager.eLogType logType, eDebugKey key, string msg, Object context = null)
	{
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00016A6D File Offset: 0x00014C6D
	private void PrintLog(DebugManager.eLogType logType, string fullMsg, Object context = null)
	{
		switch (logType)
		{
		case DebugManager.eLogType.LOG:
			if (context != null)
			{
				Debug.Log(fullMsg, context);
				return;
			}
			Debug.Log(fullMsg);
			return;
		case DebugManager.eLogType.WARNING:
			Debug.LogWarning(fullMsg, context);
			return;
		case DebugManager.eLogType.ASSERTION:
			break;
		case DebugManager.eLogType.ERROR:
			Debug.LogError(fullMsg, context);
			break;
		default:
			return;
		}
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x00016AAD File Offset: 0x00014CAD
	private void BasicLog(DebugManager.eLogType logType, string msg)
	{
		switch (logType)
		{
		case DebugManager.eLogType.LOG:
			Debug.Log(msg);
			return;
		case DebugManager.eLogType.WARNING:
			Debug.LogWarning(msg);
			return;
		case DebugManager.eLogType.ASSERTION:
			break;
		case DebugManager.eLogType.ERROR:
			Debug.LogError(msg);
			break;
		default:
			return;
		}
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x00016ADA File Offset: 0x00014CDA
	public int GetFontSize(DebugManager.eFontSize fontsize)
	{
		switch (fontsize)
		{
		case DebugManager.eFontSize.NORMAL:
			return 11;
		case DebugManager.eFontSize.LARGE:
			return 14;
		case DebugManager.eFontSize.SUPER_LARGE:
			return 18;
		default:
			return 11;
		}
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x00016AFB File Offset: 0x00014CFB
	private void Update()
	{
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x00016AFD File Offset: 0x00014CFD
	private void Start()
	{
	}

	// Token: 0x0400051F RID: 1311
	[SerializeField]
	private DebugSettingSO settings;

	// Token: 0x04000520 RID: 1312
	private Dictionary<eDebugKey, DebugSettingData> dic_Data;

	// Token: 0x04000521 RID: 1313
	private DebugManager.eTimeFormat timeFormat;

	// Token: 0x04000522 RID: 1314
	private DebugManager.eFontSize fontSize;

	// Token: 0x04000523 RID: 1315
	private const int FONTSIZE_NORMAL = 11;

	// Token: 0x04000524 RID: 1316
	private const int FONTSIZE_LARGE = 14;

	// Token: 0x04000525 RID: 1317
	private const int FONTSIZE_SUPER_LARGE = 18;

	// Token: 0x04000526 RID: 1318
	private bool isInitialized;

	// Token: 0x02000252 RID: 594
	public enum eFontSize
	{
		// Token: 0x04000B5E RID: 2910
		NORMAL,
		// Token: 0x04000B5F RID: 2911
		LARGE,
		// Token: 0x04000B60 RID: 2912
		SUPER_LARGE
	}

	// Token: 0x02000253 RID: 595
	public enum eTimeFormat
	{
		// Token: 0x04000B62 RID: 2914
		DISABLED,
		// Token: 0x04000B63 RID: 2915
		SHORT,
		// Token: 0x04000B64 RID: 2916
		LONG,
		// Token: 0x04000B65 RID: 2917
		MILLISECONDS
	}

	// Token: 0x02000254 RID: 596
	private enum eLogType
	{
		// Token: 0x04000B67 RID: 2919
		LOG,
		// Token: 0x04000B68 RID: 2920
		WARNING,
		// Token: 0x04000B69 RID: 2921
		ASSERTION,
		// Token: 0x04000B6A RID: 2922
		ERROR
	}
}
