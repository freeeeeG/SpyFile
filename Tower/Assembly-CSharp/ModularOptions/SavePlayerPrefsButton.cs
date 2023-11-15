using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x02000082 RID: 130
	[AddComponentMenu("Modular Options/Button/Save PlayerPrefs")]
	[RequireComponent(typeof(Button))]
	public class SavePlayerPrefsButton : MonoBehaviour
	{
		// Token: 0x060001E6 RID: 486 RVA: 0x000085EA File Offset: 0x000067EA
		private void Awake()
		{
			base.GetComponent<Button>().onClick.AddListener(delegate()
			{
				OptionSaveSystem.SaveToDisk();
			});
		}
	}
}
