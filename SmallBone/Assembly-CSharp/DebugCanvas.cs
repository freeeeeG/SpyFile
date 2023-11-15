using System;
using System.Text;
using TMPro;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class DebugCanvas : MonoBehaviour
{
	// Token: 0x0600036E RID: 878 RVA: 0x0000CBF7 File Offset: 0x0000ADF7
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this._stringBuilder = new StringBuilder();
		Application.logMessageReceived += this.Application_logMessageReceived;
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0000CC20 File Offset: 0x0000AE20
	private void OnDestroy()
	{
		Application.logMessageReceived -= this.Application_logMessageReceived;
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0000CC33 File Offset: 0x0000AE33
	private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
	{
		if (type != LogType.Exception && type != LogType.Error)
		{
			return;
		}
		this._stringBuilder.AppendLine(condition);
		this._stringBuilder.AppendLine(stackTrace);
		this._text.text = this._stringBuilder.ToString();
	}

	// Token: 0x040002CB RID: 715
	[SerializeField]
	private TMP_Text _text;

	// Token: 0x040002CC RID: 716
	private StringBuilder _stringBuilder;
}
