using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.TestingTool
{
	// Token: 0x02000406 RID: 1030
	public class Log : MonoBehaviour
	{
		// Token: 0x06001370 RID: 4976 RVA: 0x0003ABD2 File Offset: 0x00038DD2
		public void StartLog()
		{
			Application.logMessageReceived += this.ApplicationLogMessageReceived;
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0003ABE5 File Offset: 0x00038DE5
		private void Awake()
		{
			this._copy.onClick.AddListener(new UnityAction(this.Copy));
			this._clear.onClick.AddListener(new UnityAction(this.Clear));
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0003AC1F File Offset: 0x00038E1F
		private void OnEnable()
		{
			this._text.text = this._sb.ToString();
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0003AC37 File Offset: 0x00038E37
		private void Copy()
		{
			GUIUtility.systemCopyBuffer = this._text.text;
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0003AC49 File Offset: 0x00038E49
		private void Clear()
		{
			this._sb.Clear();
			this._text.text = string.Empty;
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0003AC67 File Offset: 0x00038E67
		private void ApplicationLogMessageReceived(string condition, string stackTrace, LogType type)
		{
			if (type == LogType.Log || type == LogType.Warning)
			{
				return;
			}
			if (this._sb.Length > 10000)
			{
				return;
			}
			this._sb.AppendFormat("[{0}] {1}\n{2}\n\n", type, condition, stackTrace);
		}

		// Token: 0x04001051 RID: 4177
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x04001052 RID: 4178
		[SerializeField]
		private Button _copy;

		// Token: 0x04001053 RID: 4179
		[SerializeField]
		private Button _clear;

		// Token: 0x04001054 RID: 4180
		private readonly StringBuilder _sb = new StringBuilder();
	}
}
