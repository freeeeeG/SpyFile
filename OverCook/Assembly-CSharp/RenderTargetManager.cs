using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class RenderTargetManager : MonoBehaviour
{
	// Token: 0x060007F1 RID: 2033 RVA: 0x00030FF0 File Offset: 0x0002F3F0
	private static RenderTargetManager.RT CreateNewRT(int width, int height, int depth, RenderTextureFormat format, string debugName)
	{
		RenderTargetManager.RT rt = new RenderTargetManager.RT();
		rt.m_ID = 6 + RenderTargetManager.m_ListOfRTs.Count;
		rt.m_RenderTarget = new RenderTexture(width, height, depth, format);
		rt.m_RenderTarget.filterMode = FilterMode.Point;
		rt.m_RenderTarget.Create();
		rt.m_Width = width;
		rt.m_Height = height;
		rt.m_Depth = depth;
		rt.m_Format = format;
		rt.m_DebugName = debugName;
		RenderTargetManager.m_ListOfRTs.Add(rt);
		return rt;
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x0003106C File Offset: 0x0002F46C
	public static void DebugMinorAlloc()
	{
		int num = 0;
		RenderTargetManager.RequestRenderTarget(640 + RenderTargetManager.m_DebugCount, 480, 0, RenderTextureFormat.ARGB32, ref num, "DebugMinorAlloc");
		RenderTargetManager.m_DebugCount++;
		Debug.Log("   ****** RenderTaret Debugs " + RenderTargetManager.m_DebugCount);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x000310C0 File Offset: 0x0002F4C0
	public static RenderTexture RequestRenderTarget(int width, int height, int depth, RenderTextureFormat format, ref int ID, string debugName = "DN")
	{
		int count = RenderTargetManager.m_ListOfRTs.Count;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			RenderTargetManager.RT rt = RenderTargetManager.m_ListOfRTs[i];
			if (!rt.m_bUsed && rt.m_Width == width && rt.m_Height == height && rt.m_Depth == depth && rt.m_Format == format)
			{
				if (!rt.m_RenderTarget.IsCreated())
				{
					RenderTargetManager.CheckForLostRTs();
				}
				rt.m_bUsed = true;
				ID = rt.m_ID;
				return rt.m_RenderTarget;
			}
		}
		if (!flag)
		{
			RenderTargetManager.RT rt2 = RenderTargetManager.CreateNewRT(width, height, depth, format, debugName);
			rt2.m_bUsed = true;
			ID = rt2.m_ID;
			return rt2.m_RenderTarget;
		}
		return null;
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00031190 File Offset: 0x0002F590
	public static void ReleaseRenderTarget(ref int ID)
	{
		int count = RenderTargetManager.m_ListOfRTs.Count;
		if (ID > 0)
		{
			for (int i = 0; i < count; i++)
			{
				if (RenderTargetManager.m_ListOfRTs[i].m_ID == ID)
				{
					RenderTargetManager.m_ListOfRTs[i].m_bUsed = false;
					ID = 0;
					break;
				}
			}
		}
		else if (ID == 0)
		{
			Debug.Log(" *****  ");
		}
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00031208 File Offset: 0x0002F608
	public static void DebugInfo()
	{
		int count = RenderTargetManager.m_ListOfRTs.Count;
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			if (RenderTargetManager.m_ListOfRTs[i].m_bUsed)
			{
				Debug.Log(string.Concat(new object[]
				{
					"  *** Used   ",
					RenderTargetManager.m_ListOfRTs[i].m_DebugName,
					"    ID=",
					RenderTargetManager.m_ListOfRTs[i].m_ID
				}));
				num++;
			}
		}
		Debug.Log(string.Concat(new object[]
		{
			"   *** Render Target  ",
			num,
			"  used    out of ",
			count
		}));
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x000312CC File Offset: 0x0002F6CC
	public static void CheckForLostRTs()
	{
		int count = RenderTargetManager.m_ListOfRTs.Count;
		for (int i = 0; i < count; i++)
		{
			RenderTargetManager.RT rt = RenderTargetManager.m_ListOfRTs[i];
			if (rt != null && rt.m_RenderTarget != null && !rt.m_RenderTarget.IsCreated())
			{
				rt.m_RenderTarget.Create();
			}
		}
		if (RenderTargetManager.m_OnRTRecreated != null)
		{
			RenderTargetManager.m_OnRTRecreated();
		}
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x00031349 File Offset: 0x0002F749
	private void OnApplicationFocus(bool focus)
	{
		RenderTargetManager.CheckForLostRTs();
	}

	// Token: 0x04000652 RID: 1618
	private static List<RenderTargetManager.RT> m_ListOfRTs = new List<RenderTargetManager.RT>();

	// Token: 0x04000653 RID: 1619
	private static int m_DebugCount = 0;

	// Token: 0x04000654 RID: 1620
	public static RenderTargetManager.OnRTRecreated m_OnRTRecreated;

	// Token: 0x020001D3 RID: 467
	// (Invoke) Token: 0x060007FA RID: 2042
	public delegate void OnRTRecreated();

	// Token: 0x020001D4 RID: 468
	private class RT
	{
		// Token: 0x04000655 RID: 1621
		public RenderTexture m_RenderTarget;

		// Token: 0x04000656 RID: 1622
		public int m_ID;

		// Token: 0x04000657 RID: 1623
		public bool m_bUsed;

		// Token: 0x04000658 RID: 1624
		public int m_Width;

		// Token: 0x04000659 RID: 1625
		public int m_Height;

		// Token: 0x0400065A RID: 1626
		public int m_Depth;

		// Token: 0x0400065B RID: 1627
		public RenderTextureFormat m_Format;

		// Token: 0x0400065C RID: 1628
		public string m_DebugName;
	}
}
