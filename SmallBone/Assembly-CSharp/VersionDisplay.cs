using System;
using TMPro;
using UnityEngine;

// Token: 0x020000AF RID: 175
[SerializeField]
public class VersionDisplay : MonoBehaviour
{
	// Token: 0x0600037E RID: 894 RVA: 0x0000CDB9 File Offset: 0x0000AFB9
	private void Awake()
	{
		this._text.text = Application.version;
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x040002D5 RID: 725
	[SerializeField]
	private TMP_Text _text;
}
