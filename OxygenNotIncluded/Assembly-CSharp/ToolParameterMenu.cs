using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C7D RID: 3197
[AddComponentMenu("KMonoBehaviour/scripts/ToolParameterMenu")]
public class ToolParameterMenu : KMonoBehaviour
{
	// Token: 0x1400002E RID: 46
	// (add) Token: 0x060065F1 RID: 26097 RVA: 0x00260B70 File Offset: 0x0025ED70
	// (remove) Token: 0x060065F2 RID: 26098 RVA: 0x00260BA8 File Offset: 0x0025EDA8
	public event System.Action onParametersChanged;

	// Token: 0x060065F3 RID: 26099 RVA: 0x00260BDD File Offset: 0x0025EDDD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ClearMenu();
	}

	// Token: 0x060065F4 RID: 26100 RVA: 0x00260BEC File Offset: 0x0025EDEC
	public void PopulateMenu(Dictionary<string, ToolParameterMenu.ToggleState> parameters)
	{
		this.ClearMenu();
		this.currentParameters = parameters;
		foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> keyValuePair in parameters)
		{
			GameObject gameObject = Util.KInstantiateUI(this.widgetPrefab, this.widgetContainer, true);
			gameObject.GetComponentInChildren<LocText>().text = Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + keyValuePair.Key);
			this.widgets.Add(keyValuePair.Key, gameObject);
			MultiToggle toggle = gameObject.GetComponentInChildren<MultiToggle>();
			ToolParameterMenu.ToggleState value = keyValuePair.Value;
			if (value == ToolParameterMenu.ToggleState.Disabled)
			{
				toggle.ChangeState(2);
			}
			else if (value == ToolParameterMenu.ToggleState.On)
			{
				toggle.ChangeState(1);
				this.lastEnabledFilter = keyValuePair.Key;
			}
			else
			{
				toggle.ChangeState(0);
			}
			MultiToggle toggle2 = toggle;
			toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
			{
				foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.widgets)
				{
					if (keyValuePair2.Value == toggle.transform.parent.gameObject)
					{
						if (this.currentParameters[keyValuePair2.Key] == ToolParameterMenu.ToggleState.Disabled)
						{
							break;
						}
						this.ChangeToSetting(keyValuePair2.Key);
						this.OnChange();
						break;
					}
				}
			}));
		}
		this.content.SetActive(true);
	}

	// Token: 0x060065F5 RID: 26101 RVA: 0x00260D28 File Offset: 0x0025EF28
	public void ClearMenu()
	{
		this.content.SetActive(false);
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.widgets.Clear();
	}

	// Token: 0x060065F6 RID: 26102 RVA: 0x00260D98 File Offset: 0x0025EF98
	private void ChangeToSetting(string key)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			if (this.currentParameters[keyValuePair.Key] != ToolParameterMenu.ToggleState.Disabled)
			{
				this.currentParameters[keyValuePair.Key] = ToolParameterMenu.ToggleState.Off;
			}
		}
		this.currentParameters[key] = ToolParameterMenu.ToggleState.On;
	}

	// Token: 0x060065F7 RID: 26103 RVA: 0x00260E1C File Offset: 0x0025F01C
	private void OnChange()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			switch (this.currentParameters[keyValuePair.Key])
			{
			case ToolParameterMenu.ToggleState.On:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(1);
				this.lastEnabledFilter = keyValuePair.Key;
				break;
			case ToolParameterMenu.ToggleState.Off:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(0);
				break;
			case ToolParameterMenu.ToggleState.Disabled:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(2);
				break;
			}
		}
		if (this.onParametersChanged != null)
		{
			this.onParametersChanged();
		}
	}

	// Token: 0x060065F8 RID: 26104 RVA: 0x00260EEC File Offset: 0x0025F0EC
	public string GetLastEnabledFilter()
	{
		return this.lastEnabledFilter;
	}

	// Token: 0x04004637 RID: 17975
	public GameObject content;

	// Token: 0x04004638 RID: 17976
	public GameObject widgetContainer;

	// Token: 0x04004639 RID: 17977
	public GameObject widgetPrefab;

	// Token: 0x0400463B RID: 17979
	private Dictionary<string, GameObject> widgets = new Dictionary<string, GameObject>();

	// Token: 0x0400463C RID: 17980
	private Dictionary<string, ToolParameterMenu.ToggleState> currentParameters;

	// Token: 0x0400463D RID: 17981
	private string lastEnabledFilter;

	// Token: 0x02001BAE RID: 7086
	public class FILTERLAYERS
	{
		// Token: 0x04007D71 RID: 32113
		public static string BUILDINGS = "BUILDINGS";

		// Token: 0x04007D72 RID: 32114
		public static string TILES = "TILES";

		// Token: 0x04007D73 RID: 32115
		public static string WIRES = "WIRES";

		// Token: 0x04007D74 RID: 32116
		public static string LIQUIDCONDUIT = "LIQUIDPIPES";

		// Token: 0x04007D75 RID: 32117
		public static string GASCONDUIT = "GASPIPES";

		// Token: 0x04007D76 RID: 32118
		public static string SOLIDCONDUIT = "SOLIDCONDUITS";

		// Token: 0x04007D77 RID: 32119
		public static string CLEANANDCLEAR = "CLEANANDCLEAR";

		// Token: 0x04007D78 RID: 32120
		public static string DIGPLACER = "DIGPLACER";

		// Token: 0x04007D79 RID: 32121
		public static string LOGIC = "LOGIC";

		// Token: 0x04007D7A RID: 32122
		public static string BACKWALL = "BACKWALL";

		// Token: 0x04007D7B RID: 32123
		public static string CONSTRUCTION = "CONSTRUCTION";

		// Token: 0x04007D7C RID: 32124
		public static string DIG = "DIG";

		// Token: 0x04007D7D RID: 32125
		public static string CLEAN = "CLEAN";

		// Token: 0x04007D7E RID: 32126
		public static string OPERATE = "OPERATE";

		// Token: 0x04007D7F RID: 32127
		public static string METAL = "METAL";

		// Token: 0x04007D80 RID: 32128
		public static string BUILDABLE = "BUILDABLE";

		// Token: 0x04007D81 RID: 32129
		public static string FILTER = "FILTER";

		// Token: 0x04007D82 RID: 32130
		public static string LIQUIFIABLE = "LIQUIFIABLE";

		// Token: 0x04007D83 RID: 32131
		public static string LIQUID = "LIQUID";

		// Token: 0x04007D84 RID: 32132
		public static string CONSUMABLEORE = "CONSUMABLEORE";

		// Token: 0x04007D85 RID: 32133
		public static string ORGANICS = "ORGANICS";

		// Token: 0x04007D86 RID: 32134
		public static string FARMABLE = "FARMABLE";

		// Token: 0x04007D87 RID: 32135
		public static string GAS = "GAS";

		// Token: 0x04007D88 RID: 32136
		public static string MISC = "MISC";

		// Token: 0x04007D89 RID: 32137
		public static string HEATFLOW = "HEATFLOW";

		// Token: 0x04007D8A RID: 32138
		public static string ABSOLUTETEMPERATURE = "ABSOLUTETEMPERATURE";

		// Token: 0x04007D8B RID: 32139
		public static string ADAPTIVETEMPERATURE = "ADAPTIVETEMPERATURE";

		// Token: 0x04007D8C RID: 32140
		public static string STATECHANGE = "STATECHANGE";

		// Token: 0x04007D8D RID: 32141
		public static string ALL = "ALL";
	}

	// Token: 0x02001BAF RID: 7087
	public enum ToggleState
	{
		// Token: 0x04007D8F RID: 32143
		On,
		// Token: 0x04007D90 RID: 32144
		Off,
		// Token: 0x04007D91 RID: 32145
		Disabled
	}
}
