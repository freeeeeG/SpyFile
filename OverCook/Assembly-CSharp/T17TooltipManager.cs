using System;
using UnityEngine;

// Token: 0x02000B1B RID: 2843
public class T17TooltipManager : MonoBehaviour
{
	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x0600398F RID: 14735 RVA: 0x0011116D File Offset: 0x0010F56D
	public static T17TooltipManager Instance
	{
		get
		{
			return T17TooltipManager.s_Instance;
		}
	}

	// Token: 0x06003990 RID: 14736 RVA: 0x00111174 File Offset: 0x0010F574
	private void Awake()
	{
		if (T17TooltipManager.s_Instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			T17TooltipManager.s_Instance = this;
		}
	}

	// Token: 0x06003991 RID: 14737 RVA: 0x00111197 File Offset: 0x0010F597
	private void OnDestroy()
	{
		if (T17TooltipManager.s_Instance == this)
		{
			T17TooltipManager.s_Instance = null;
		}
	}

	// Token: 0x06003992 RID: 14738 RVA: 0x001111AF File Offset: 0x0010F5AF
	private void Start()
	{
		if (!this.m_bIsSetup)
		{
			this.Setup();
		}
	}

	// Token: 0x06003993 RID: 14739 RVA: 0x001111C4 File Offset: 0x0010F5C4
	private void Setup()
	{
		if (this.m_TooltipPrefab != null && this.m_TooltipCanvas != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_TooltipPrefab);
			gameObject.transform.SetParent(this.m_TooltipCanvas, false);
			gameObject.SetActive(true);
			this.m_Tooltip = gameObject.GetComponent<T17Tooltip>();
			this.m_Tooltip.m_Text.m_LocalizationTag = this.m_DefaultTooltip;
			this.m_Tooltip.m_Text.Convert();
			this.m_bIsSetup = true;
		}
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x00111251 File Offset: 0x0010F651
	public void SetDefaultTooltip(string _tag, LocToken[] _replacements = null)
	{
		this.m_DefaultTooltip = _tag;
		this.m_defaultReplacements = _replacements;
	}

	// Token: 0x06003995 RID: 14741 RVA: 0x00111264 File Offset: 0x0010F664
	public void Show(string message, bool isLocalised = false)
	{
		if (!this.m_bIsSetup)
		{
			this.Setup();
		}
		if (!isLocalised)
		{
			message = Localization.Get(message, new LocToken[0]);
		}
		bool flag = !string.IsNullOrEmpty(message);
		if (flag)
		{
			this.m_Tooltip.m_Text.SetNonLocalizedText(message);
		}
		else if (this.m_defaultReplacements != null)
		{
			this.m_Tooltip.m_Text.SetNonLocalizedText(Localization.Get(this.m_DefaultTooltip, this.m_defaultReplacements));
		}
		else
		{
			this.m_Tooltip.m_Text.SetNonLocalizedText(Localization.Get(this.m_DefaultTooltip, new LocToken[0]));
		}
		if (!this.m_Tooltip.gameObject.activeInHierarchy)
		{
			this.m_Tooltip.gameObject.SetActive(true);
		}
	}

	// Token: 0x04002E3C RID: 11836
	private static T17TooltipManager s_Instance;

	// Token: 0x04002E3D RID: 11837
	public Transform m_TooltipCanvas;

	// Token: 0x04002E3E RID: 11838
	public GameObject m_TooltipPrefab;

	// Token: 0x04002E3F RID: 11839
	private T17Tooltip m_Tooltip;

	// Token: 0x04002E40 RID: 11840
	private string m_DefaultTooltip = "Text.Menu.DefaultTooltip";

	// Token: 0x04002E41 RID: 11841
	private LocToken[] m_defaultReplacements;

	// Token: 0x04002E42 RID: 11842
	private bool m_bIsSetup;
}
