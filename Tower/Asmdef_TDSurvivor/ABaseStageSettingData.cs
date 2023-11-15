using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
public abstract class ABaseStageSettingData : ScriptableObject
{
	// Token: 0x060000D4 RID: 212
	public abstract string GetLocalization_Title();

	// Token: 0x060000D5 RID: 213
	public abstract string GetLocalization_Description();

	// Token: 0x0400009B RID: 155
	[SerializeField]
	[TextArea]
	private string Note;

	// Token: 0x0400009C RID: 156
	[SerializeField]
	protected eStageType stageType;
}
