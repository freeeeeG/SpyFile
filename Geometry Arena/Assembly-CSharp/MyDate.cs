using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x0200006F RID: 111
public class MyDate : MonoBehaviour
{
	// Token: 0x06000431 RID: 1073 RVA: 0x0001A550 File Offset: 0x00018750
	private void Update()
	{
		if (this.timeUpdateLeft <= 0f)
		{
			this.timeUpdateLeft = 1f;
			base.StartCoroutine(this.GetTime());
			return;
		}
		this.timeUpdateLeft -= Time.deltaTime;
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0001A58A File Offset: 0x0001878A
	private IEnumerator GetTime()
	{
		UnityWebRequest www = new UnityWebRequest(this.timeURL);
		DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();
		www.downloadHandler = downloadHandler;
		yield return www.SendWebRequest();
		if (www.error != null)
		{
			Debug.LogError(www.error);
		}
		else
		{
			Debug.Log(www.downloadHandler.text);
			string value = www.downloadHandler.text.Substring(2);
			string str = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds(Convert.ToDouble(value)).ToString("yyyy:MM:dd HH:mm:ss");
			Debug.Log("当前北京时间：" + str);
		}
		yield break;
	}

	// Token: 0x04000390 RID: 912
	private string timeURL = "http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=1";

	// Token: 0x04000391 RID: 913
	public float timeUpdateLeft;
}
