using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using InControl;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200036C RID: 876
public class EmbeddedTextControlKeyLookup : MonoBehaviour, ITextProcessor
{
	// Token: 0x060010BE RID: 4286 RVA: 0x000603A3 File Offset: 0x0005E7A3
	public bool HasEmbeddedControlKeys(string markupString)
	{
		return Regex.IsMatch(markupString, "<key\\s*index\\s*=\\s*(\\d+)\\s*/>");
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x000603B0 File Offset: 0x0005E7B0
	public bool ProcessText(ref string markupString)
	{
		if (!this.HasEmbeddedControlKeys(markupString))
		{
			return false;
		}
		List<EmbeddedTextControlKeyLookup.ControlKeyReplacementInfo> list = new List<EmbeddedTextControlKeyLookup.ControlKeyReplacementInfo>();
		Match match = Regex.Match(markupString, "<key\\s*index\\s*=\\s*(\\d+)\\s*/>");
		while (match.Success)
		{
			int index = int.Parse(match.Groups[1].Value);
			list.Add(new EmbeddedTextControlKeyLookup.ControlKeyReplacementInfo
			{
				m_Index = match.Index,
				m_Length = match.Length,
				m_Replacement = this.GetKeyText(index)
			});
			match = match.NextMatch();
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			markupString = markupString.Remove(list[i].m_Index, list[i].m_Length);
			markupString = markupString.Insert(list[i].m_Index, list[i].m_Replacement);
		}
		return true;
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x000604B8 File Offset: 0x0005E8B8
	private string GetKeyText(int _index)
	{
		return (_index < 0 || _index >= this.m_ButtonIds.Length) ? " " : this.GetBindingText(this.m_ButtonIds[_index]);
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x000604E8 File Offset: 0x0005E8E8
	private string GetBindingText(PlayerInputLookup.LogicalButtonID logicalButtonId)
	{
		PadSide side = PlayerInputLookup.GetInputConfig().GetInputData(PlayerInputLookup.Player.One).Side;
		ControlPadInput.Button[] realButtons = PlayerInputLookup.GetRealButtons(logicalButtonId, side);
		int i = 0;
		while (i < realButtons.Length)
		{
			List<Key> bindings = PCPadInputProvider.GetBindings(realButtons[i], side != PadSide.Both);
			if (bindings != null && bindings.Count > 0)
			{
				string text = bindings[0].ToString();
				if (text.Length == 1)
				{
					return this.GetKeyString(text.ToUpperInvariant());
				}
				return this.GetKeyString(Localization.Get("Text.ControlsMenu." + text.ToUpperInvariant(), new LocToken[0]));
			}
			else
			{
				i++;
			}
		}
		return " ";
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x0006059F File Offset: 0x0005E99F
	private string GetKeyString(string key)
	{
		return "<color=#bcaba3ff>[</color><color=#f3eaedff>" + key + "</color><color=#bcaba3ff>]</color>";
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x000605B1 File Offset: 0x0005E9B1
	public bool OnPopulateMesh(VertexHelper _helper)
	{
		return false;
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x000605B4 File Offset: 0x0005E9B4
	public bool HasEmbeddedImages(string inputString)
	{
		return false;
	}

	// Token: 0x04000CE6 RID: 3302
	[SerializeField]
	private PlayerInputLookup.LogicalButtonID[] m_ButtonIds;

	// Token: 0x04000CE7 RID: 3303
	private const string kControlKeysLookupPattern = "<key\\s*index\\s*=\\s*(\\d+)\\s*/>";

	// Token: 0x04000CE8 RID: 3304
	private const string kBracketColorMarkup = "<color=#bcaba3ff>";

	// Token: 0x04000CE9 RID: 3305
	private const string kKeyColorMarkup = "<color=#f3eaedff>";

	// Token: 0x04000CEA RID: 3306
	private const string kColorEndMarkup = "</color>";

	// Token: 0x04000CEB RID: 3307
	private const string kOpenBraketString = "<color=#bcaba3ff>[</color>";

	// Token: 0x04000CEC RID: 3308
	private const string kCloseBraketString = "<color=#bcaba3ff>]</color>";

	// Token: 0x0200036D RID: 877
	private struct ControlKeyReplacementInfo
	{
		// Token: 0x04000CED RID: 3309
		public int m_Index;

		// Token: 0x04000CEE RID: 3310
		public int m_Length;

		// Token: 0x04000CEF RID: 3311
		public string m_Replacement;
	}
}
