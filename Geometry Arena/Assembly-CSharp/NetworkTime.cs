using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

// Token: 0x02000074 RID: 116
[Serializable]
public class NetworkTime
{
	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06000469 RID: 1129 RVA: 0x0001BB98 File Offset: 0x00019D98
	public static NetworkTime Inst
	{
		get
		{
			return TempData.inst.networkTime;
		}
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0001BBA4 File Offset: 0x00019DA4
	public void GetNetworkTime(string hostName)
	{
		byte[] array = new byte[48];
		array[0] = 27;
		IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry(hostName).AddressList[0], 123);
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		socket.ReceiveTimeout = this.setting_Timeout;
		socket.Connect(remoteEP);
		socket.Send(array);
		socket.Receive(array);
		socket.Close();
		ulong num = (ulong)array[40] << 24 | (ulong)array[41] << 16 | (ulong)array[42] << 8 | (ulong)array[43];
		ulong num2 = (ulong)array[44] << 24 | (ulong)array[45] << 16 | (ulong)array[46] << 8 | (ulong)array[47];
		ulong num3 = num * 1000UL + num2 * 1000UL / 4294967296UL;
		DateTime dateTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local).AddMilliseconds((double)num3);
		this.datetimeNetwork = dateTime;
		this.ifError = false;
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0001BC8A File Offset: 0x00019E8A
	private DateTime GetDateTime_NowNetwork()
	{
		if (this.testOn)
		{
			return Convert.ToDateTime(this.testTimeB, CultureInfo.InvariantCulture);
		}
		return this.datetimeNetwork;
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0001BCAB File Offset: 0x00019EAB
	private DateTime GetDateTime_Origin()
	{
		if (this.testOn)
		{
			return Convert.ToDateTime(this.testTimeA, CultureInfo.InvariantCulture);
		}
		return Convert.ToDateTime(NetworkTime.datetimeOrigin, CultureInfo.InvariantCulture);
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0001BCD8 File Offset: 0x00019ED8
	public void UpdateTimeData()
	{
		string text = "";
		int i = 0;
		while (i < this.servers.Length)
		{
			string text2 = this.servers[i];
			bool flag = false;
			try
			{
				this.GetNetworkTime(text2);
				flag = true;
				TempData.inst.NTP_ThreadFinish_Success();
			}
			catch (Exception arg)
			{
				text = text + string.Format("NTP{0}连接失败，原因{1}", text2, arg) + "\n";
				if (i == this.servers.Length - 1)
				{
					this.ifError = true;
					Debug.LogError("Error_无法连接全部NTP!\n" + text);
					TempData.inst.NTP_ThreadFinish_Error();
				}
				goto IL_8D;
			}
			goto IL_7D;
			IL_8D:
			i++;
			continue;
			IL_7D:
			if (flag)
			{
				TempData.inst.NTP_ThreadFinish_Success();
				break;
			}
			goto IL_8D;
		}
		this.dayIndex = this.GetTimeSpan_DayDelta(this.GetDateTime_Origin(), this.GetDateTime_NowNetwork()).Days;
		TimeSpan timeSpan_DayDelta = this.GetTimeSpan_DayDelta(this.GetDateTime_NowNetwork().AddDays(1.0).Date, this.GetDateTime_NowNetwork());
		if (this.ifError)
		{
			this.refreshTimeLeft = LanguageText.Inst.dailyChallenge.error_CantConnectTimeServer;
		}
		else
		{
			this.refreshTimeLeft = string.Format("{0:00}:{1:00}:{2:00}", timeSpan_DayDelta.Hours, timeSpan_DayDelta.Minutes, timeSpan_DayDelta.Seconds);
		}
		if (this.testOn)
		{
			this.TestTimeB_AddOneSec();
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0001BE44 File Offset: 0x0001A044
	public void TestTimeB_AddOneSec()
	{
		this.testTimeB = Convert.ToDateTime(this.testTimeB, CultureInfo.InvariantCulture).AddSeconds(1.0).ToString();
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0001BE80 File Offset: 0x0001A080
	public void TestTimeB_AddOneDay()
	{
		this.testTimeB = Convert.ToDateTime(this.testTimeB, CultureInfo.InvariantCulture).AddDays(1.0).ToString();
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0001BEBC File Offset: 0x0001A0BC
	public TimeSpan GetTimeSpan_DayDelta(DateTime dateTimeA, DateTime dateTimeB)
	{
		TimeSpan timeSpan = new TimeSpan(dateTimeA.Ticks);
		TimeSpan ts = new TimeSpan(dateTimeB.Ticks);
		return timeSpan.Subtract(ts).Duration();
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0001BEF8 File Offset: 0x0001A0F8
	public TimeSpan GetTimeSpan_DayDelta(string dateTimeAstring, string dateTimeBstring)
	{
		DateTime dateTimeA = Convert.ToDateTime(dateTimeAstring, CultureInfo.InvariantCulture);
		DateTime dateTimeB = Convert.ToDateTime(dateTimeBstring, CultureInfo.InvariantCulture);
		return this.GetTimeSpan_DayDelta(dateTimeA, dateTimeB);
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0001BF28 File Offset: 0x0001A128
	public static string GetString_TimeWithDayIndex(int dayIndex)
	{
		DateTime dateTime = NetworkTime.Inst.GetDateTime_Origin().AddDays((double)dayIndex);
		return string.Format("{0}-{1:00}-{2:00}", dateTime.Year, dateTime.Month, dateTime.Day);
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0001BF78 File Offset: 0x0001A178
	public static string GetString_DayWithDayIndex(int dayIndex)
	{
		return string.Format("{0:00}", NetworkTime.Inst.GetDateTime_Origin().AddDays((double)dayIndex).Day);
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0001BFB0 File Offset: 0x0001A1B0
	public static string GetString_CountdownOrError()
	{
		if (NetworkTime.Inst.ifError)
		{
			return LanguageText.Inst.dailyChallenge.error_CantConnectTimeServer;
		}
		return LanguageText.Inst.dailyChallenge.timeCountDown.Replace("dailyLeftTime", NetworkTime.Inst.refreshTimeLeft);
	}

	// Token: 0x04000394 RID: 916
	public static string datetimeOrigin = "11/1/2020 00:00:00 AM";

	// Token: 0x04000395 RID: 917
	private static readonly object locker = new object();

	// Token: 0x04000396 RID: 918
	[SerializeField]
	public DateTime datetimeNetwork;

	// Token: 0x04000397 RID: 919
	[Header("PublicTimeData")]
	public bool ifError;

	// Token: 0x04000398 RID: 920
	public int dayIndex = -1;

	// Token: 0x04000399 RID: 921
	public string refreshTimeLeft = "";

	// Token: 0x0400039A RID: 922
	[Header("Test")]
	[SerializeField]
	private bool testOn;

	// Token: 0x0400039B RID: 923
	[SerializeField]
	private string testTimeA = "11/1/2020 00:00:00 AM";

	// Token: 0x0400039C RID: 924
	[SerializeField]
	private string testTimeB = "11/1/2020 00:00:00 AM";

	// Token: 0x0400039D RID: 925
	[Header("Server")]
	[SerializeField]
	private string[] servers = new string[0];

	// Token: 0x0400039E RID: 926
	[Header("Settings")]
	[SerializeField]
	private int setting_Timeout = 5000;
}
