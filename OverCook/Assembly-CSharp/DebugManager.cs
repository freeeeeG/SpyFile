using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200041E RID: 1054
public class DebugManager : MonoBehaviour
{
	// Token: 0x1700023F RID: 575
	// (get) Token: 0x0600131D RID: 4893 RVA: 0x0006AC94 File Offset: 0x00069094
	public static DebugManager Instance
	{
		get
		{
			return DebugManager.s_Instance;
		}
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x0006AC9C File Offset: 0x0006909C
	public void Awake()
	{
		if (DebugManager.s_Instance != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		DebugManager.s_Instance = this;
		this.AddOption("New Gravity", true);
		this.AddOption("Unlock all levels", false);
		this.AddOption("Unlock All DLC Packs", false);
		this.AddOption("Chef VS Chef Prediction", true);
		this.AddOption("Raise all ramps", false);
		this.AddOption("Unlock all chefs", false);
		this.AddOption("Deliver Current Recipe", false);
		this.AddOption("Skip Level 4 star", false);
		this.AddOption("Skip Level 3 star", false);
		this.AddOption("Skip Level 2 star", false);
		this.AddOption("Skip Level 1 star", false);
		this.AddOption("Skip Level", false);
		this.AddOption("Freeze time", false);
		this.AddOption("Fast NetworkChefs", true);
		this.AddOption("Auto Load Levels", false);
		this.AddOption("On Screen Debug Text", false);
		this.AddOption("Toggle UI", true);
		this.AddOption("Reset network stats", false);
		this.AddOption("Corrupt All Saves", false);
		this.AddOption("Corrupt Save 0", false);
		this.AddOption("Corrupt Save 1", false);
		this.AddOption("Corrupt Save 2", false);
		this.AddOption("Corrupt Meta Save", false);
		this.AddOption("Fake No Space", false);
		this.AddOption("Delete All Saves", false);
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x0006ADF2 File Offset: 0x000691F2
	private void Start()
	{
		this.m_button = PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.DebugMenu, PlayerInputLookup.Player.One);
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x0006AE02 File Offset: 0x00069202
	private void OnDestroy()
	{
		if (DebugManager.s_Instance != null)
		{
			UnityEngine.Object.Destroy(DebugManager.s_Instance);
			DebugManager.s_Instance = null;
		}
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x0006AE24 File Offset: 0x00069224
	private void AddOption(string optionName, bool defaultValue)
	{
		this.m_debugOptions.Add(optionName, defaultValue);
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x0006AE34 File Offset: 0x00069234
	public bool GetOption(string optionName)
	{
		bool result = false;
		if (this.m_debugOptions.TryGetValue(optionName, out result))
		{
			return result;
		}
		return result;
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x0006AE59 File Offset: 0x00069259
	public Dictionary<string, bool> GetOptions()
	{
		return this.m_debugOptions;
	}

	// Token: 0x04000F06 RID: 3846
	[SerializeField]
	private BaseMenuBehaviour m_baseMenu;

	// Token: 0x04000F07 RID: 3847
	private ILogicalButton m_button;

	// Token: 0x04000F08 RID: 3848
	private static DebugManager s_Instance;

	// Token: 0x04000F09 RID: 3849
	private Dictionary<string, bool> m_debugOptions = new Dictionary<string, bool>();
}
