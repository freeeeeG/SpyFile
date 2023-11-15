using System;
using UnityEngine;

// Token: 0x02000724 RID: 1828
[CreateAssetMenu(fileName = "LocalizationData", menuName = "Team17/Create Localization Data")]
[Serializable]
public class LocalizationData : ScriptableObject
{
	// Token: 0x04001AA2 RID: 6818
	public TextAsset[] m_Localizations;
}
