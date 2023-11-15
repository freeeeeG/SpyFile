using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000219 RID: 537
public class WindowsAccessibility : MonoBehaviour
{
	// Token: 0x060008EE RID: 2286
	[DllImport("user32.dll")]
	private static extern bool SystemParametersInfo(uint action, uint param, ref WindowsAccessibility.SKEY vparam, uint init);

	// Token: 0x060008EF RID: 2287
	[DllImport("user32.dll")]
	private static extern bool SystemParametersInfo(uint action, uint param, ref WindowsAccessibility.FILTERKEY vparam, uint init);

	// Token: 0x060008F0 RID: 2288 RVA: 0x000352FC File Offset: 0x000336FC
	public static void ToggleAccessibilityShortcutKeys(bool ReturnToStarting)
	{
		if (!WindowsAccessibility.StartupAccessibilitySet)
		{
			WindowsAccessibility.StartupStickyKeys.cbSize = WindowsAccessibility.SKEYSize;
			WindowsAccessibility.StartupToggleKeys.cbSize = WindowsAccessibility.SKEYSize;
			WindowsAccessibility.StartupFilterKeys.cbSize = WindowsAccessibility.FKEYSize;
			WindowsAccessibility.SystemParametersInfo(58U, WindowsAccessibility.SKEYSize, ref WindowsAccessibility.StartupStickyKeys, 0U);
			WindowsAccessibility.SystemParametersInfo(52U, WindowsAccessibility.SKEYSize, ref WindowsAccessibility.StartupToggleKeys, 0U);
			WindowsAccessibility.SystemParametersInfo(50U, WindowsAccessibility.FKEYSize, ref WindowsAccessibility.StartupFilterKeys, 0U);
			WindowsAccessibility.StartupAccessibilitySet = true;
		}
		if (ReturnToStarting)
		{
			WindowsAccessibility.SystemParametersInfo(59U, WindowsAccessibility.SKEYSize, ref WindowsAccessibility.StartupStickyKeys, 0U);
			WindowsAccessibility.SystemParametersInfo(53U, WindowsAccessibility.SKEYSize, ref WindowsAccessibility.StartupToggleKeys, 0U);
			WindowsAccessibility.SystemParametersInfo(51U, WindowsAccessibility.FKEYSize, ref WindowsAccessibility.StartupFilterKeys, 0U);
		}
		else
		{
			WindowsAccessibility.SKEY startupStickyKeys = WindowsAccessibility.StartupStickyKeys;
			startupStickyKeys.dwFlags &= 4294967291U;
			startupStickyKeys.dwFlags &= 4294967287U;
			WindowsAccessibility.SystemParametersInfo(59U, WindowsAccessibility.SKEYSize, ref startupStickyKeys, 0U);
			WindowsAccessibility.SKEY startupToggleKeys = WindowsAccessibility.StartupToggleKeys;
			startupToggleKeys.dwFlags &= 4294967291U;
			startupToggleKeys.dwFlags &= 4294967287U;
			WindowsAccessibility.SystemParametersInfo(53U, WindowsAccessibility.SKEYSize, ref startupToggleKeys, 0U);
			WindowsAccessibility.FILTERKEY startupFilterKeys = WindowsAccessibility.StartupFilterKeys;
			startupFilterKeys.dwFlags &= 4294967291U;
			startupFilterKeys.dwFlags &= 4294967287U;
			WindowsAccessibility.SystemParametersInfo(51U, WindowsAccessibility.FKEYSize, ref startupFilterKeys, 0U);
		}
	}

	// Token: 0x040007CC RID: 1996
	private const uint SPI_GETFILTERKEYS = 50U;

	// Token: 0x040007CD RID: 1997
	private const uint SPI_SETFILTERKEYS = 51U;

	// Token: 0x040007CE RID: 1998
	private const uint SPI_GETTOGGLEKEYS = 52U;

	// Token: 0x040007CF RID: 1999
	private const uint SPI_SETTOGGLEKEYS = 53U;

	// Token: 0x040007D0 RID: 2000
	private const uint SPI_GETSTICKYKEYS = 58U;

	// Token: 0x040007D1 RID: 2001
	private const uint SPI_SETSTICKYKEYS = 59U;

	// Token: 0x040007D2 RID: 2002
	private static bool StartupAccessibilitySet;

	// Token: 0x040007D3 RID: 2003
	private static WindowsAccessibility.SKEY StartupStickyKeys;

	// Token: 0x040007D4 RID: 2004
	private static WindowsAccessibility.SKEY StartupToggleKeys;

	// Token: 0x040007D5 RID: 2005
	private static WindowsAccessibility.FILTERKEY StartupFilterKeys;

	// Token: 0x040007D6 RID: 2006
	private const uint SKF_STICKYKEYSON = 1U;

	// Token: 0x040007D7 RID: 2007
	private const uint TKF_TOGGLEKEYSON = 1U;

	// Token: 0x040007D8 RID: 2008
	private const uint SKF_CONFIRMHOTKEY = 8U;

	// Token: 0x040007D9 RID: 2009
	private const uint SKF_HOTKEYACTIVE = 4U;

	// Token: 0x040007DA RID: 2010
	private const uint TKF_CONFIRMHOTKEY = 8U;

	// Token: 0x040007DB RID: 2011
	private const uint TKF_HOTKEYACTIVE = 4U;

	// Token: 0x040007DC RID: 2012
	private const uint FKF_CONFIRMHOTKEY = 8U;

	// Token: 0x040007DD RID: 2013
	private const uint FKF_HOTKEYACTIVE = 4U;

	// Token: 0x040007DE RID: 2014
	private static uint SKEYSize = 8U;

	// Token: 0x040007DF RID: 2015
	private static uint FKEYSize = 24U;

	// Token: 0x0200021A RID: 538
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct SKEY
	{
		// Token: 0x040007E0 RID: 2016
		public uint cbSize;

		// Token: 0x040007E1 RID: 2017
		public uint dwFlags;
	}

	// Token: 0x0200021B RID: 539
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct FILTERKEY
	{
		// Token: 0x040007E2 RID: 2018
		public uint cbSize;

		// Token: 0x040007E3 RID: 2019
		public uint dwFlags;

		// Token: 0x040007E4 RID: 2020
		public uint iWaitMSec;

		// Token: 0x040007E5 RID: 2021
		public uint iDelayMSec;

		// Token: 0x040007E6 RID: 2022
		public uint iRepeatMSec;

		// Token: 0x040007E7 RID: 2023
		public uint iBounceMSec;
	}
}
