using System;
using UnityEngine.UI;

// Token: 0x02000ADF RID: 2783
public class ControlsScreen : KScreen
{
	// Token: 0x060055B8 RID: 21944 RVA: 0x001F3048 File Offset: 0x001F1248
	protected override void OnPrefabInit()
	{
		BindingEntry[] bindingEntries = GameInputMapping.GetBindingEntries();
		string text = "";
		foreach (BindingEntry bindingEntry in bindingEntries)
		{
			text += bindingEntry.mAction.ToString();
			text += ": ";
			text += bindingEntry.mKeyCode.ToString();
			text += "\n";
		}
		this.controlLabel.text = text;
	}

	// Token: 0x060055B9 RID: 21945 RVA: 0x001F30CD File Offset: 0x001F12CD
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Help) || e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
	}

	// Token: 0x0400397F RID: 14719
	public Text controlLabel;
}
