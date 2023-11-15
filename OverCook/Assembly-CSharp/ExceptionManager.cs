using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x0200063C RID: 1596
public class ExceptionManager : Manager
{
	// Token: 0x06001E60 RID: 7776 RVA: 0x00092F00 File Offset: 0x00091300
	private void Awake()
	{
	}

	// Token: 0x06001E61 RID: 7777 RVA: 0x00092F02 File Offset: 0x00091302
	private void Start()
	{
	}

	// Token: 0x06001E62 RID: 7778 RVA: 0x00092F04 File Offset: 0x00091304
	private void OnEnable()
	{
		Application.logMessageReceived += this.HandleException;
	}

	// Token: 0x06001E63 RID: 7779 RVA: 0x00092F17 File Offset: 0x00091317
	private void OnDisable()
	{
		Application.logMessageReceived -= this.HandleException;
	}

	// Token: 0x06001E64 RID: 7780 RVA: 0x00092F2A File Offset: 0x0009132A
	private void OnDestroy()
	{
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x00092F2C File Offset: 0x0009132C
	private void HandleException(string logString, string stackTrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			this.m_LastException.Set(logString, stackTrace);
			this.LogLastException();
			this.DisplayLastException(true);
		}
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x00092F4F File Offset: 0x0009134F
	private void LogLastException()
	{
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x00092F51 File Offset: 0x00091351
	private void DisplayLastException(bool bJustOccured)
	{
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x00092F54 File Offset: 0x00091354
	public void LogACaughtException(Exception e, string log)
	{
		string userData = GameUtils.DebugGetState();
		string stackTrace = e.ToString();
		Analytics analytics = GameUtils.RequestManager<Analytics>();
		if (null != analytics)
		{
			analytics.LogAnException(log, stackTrace, userData);
		}
		this.HandleException(log, stackTrace, LogType.Exception);
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x00092F94 File Offset: 0x00091394
	public void LogAnException(string log)
	{
		StackTrace stackTrace = new StackTrace(1, true);
		string text = string.Empty;
		if (stackTrace != null)
		{
			for (int i = 0; i < stackTrace.FrameCount; i++)
			{
				StackFrame frame = stackTrace.GetFrame(i);
				if (frame != null)
				{
					MethodBase method = frame.GetMethod();
					string text2;
					if (method != null)
					{
						ParameterInfo[] parameters = method.GetParameters();
						if (parameters != null && method.ReflectedType != null)
						{
							text2 = text;
							text = string.Concat(new string[]
							{
								text2,
								method.ReflectedType.Name,
								".",
								method.Name,
								"( "
							});
							for (int j = 0; j < parameters.Length; j++)
							{
								if (parameters[j] != null)
								{
									if (j != 0)
									{
										text += ", ";
									}
									text2 = text;
									text = string.Concat(new object[]
									{
										text2,
										parameters[j].ParameterType,
										" ",
										parameters[j].Name
									});
								}
							}
						}
						text += " )";
					}
					string fileName = frame.GetFileName();
					string text3 = string.Empty;
					if (frame.GetFileName() != null)
					{
						int num = frame.GetFileName().LastIndexOf('\\') + 1;
						if (num != -1)
						{
							text3 = fileName.Substring(num);
						}
						else
						{
							text3 = Path.GetFileName(fileName);
						}
					}
					text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						" in ",
						text3,
						":",
						frame.GetFileLineNumber(),
						" \n"
					});
				}
			}
		}
		string userData = GameUtils.DebugGetState();
		Analytics analytics = GameUtils.RequestManager<Analytics>();
		if (null != analytics)
		{
			analytics.LogAnException(log, text, userData);
		}
		this.HandleException(log, text, LogType.Exception);
	}

	// Token: 0x04001761 RID: 5985
	private ExceptionManager.ExceptionInfo m_LastException = new ExceptionManager.ExceptionInfo();

	// Token: 0x0200063D RID: 1597
	private class ExceptionInfo
	{
		// Token: 0x06001E6B RID: 7787 RVA: 0x00093192 File Offset: 0x00091592
		public void Set(string exceptionString, string stackTrace)
		{
			this.m_exceptionString = exceptionString;
			this.m_stackTrace = stackTrace;
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x000931A2 File Offset: 0x000915A2
		public bool IsValid()
		{
			return !string.IsNullOrEmpty(this.m_exceptionString) && !string.IsNullOrEmpty(this.m_stackTrace);
		}

		// Token: 0x04001762 RID: 5986
		public string m_exceptionString = string.Empty;

		// Token: 0x04001763 RID: 5987
		public string m_stackTrace = string.Empty;
	}
}
