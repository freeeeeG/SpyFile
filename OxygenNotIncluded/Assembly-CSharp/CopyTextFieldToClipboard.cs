using System;
using UnityEngine;

// Token: 0x02000C95 RID: 3221
[AddComponentMenu("KMonoBehaviour/scripts/CopyTextFieldToClipboard")]
public class CopyTextFieldToClipboard : KMonoBehaviour
{
	// Token: 0x0600669B RID: 26267 RVA: 0x002640D0 File Offset: 0x002622D0
	protected override void OnPrefabInit()
	{
		this.button.onClick += this.OnClick;
	}

	// Token: 0x0600669C RID: 26268 RVA: 0x002640E9 File Offset: 0x002622E9
	private void OnClick()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.GetText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x040046C5 RID: 18117
	public KButton button;

	// Token: 0x040046C6 RID: 18118
	public Func<string> GetText;
}
