using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200023A RID: 570
public class UI_ChoiceElements : MonoBehaviour
{
	// Token: 0x06000EB2 RID: 3762 RVA: 0x00025F5C File Offset: 0x0002415C
	public void SetElements(ElementSkill skill)
	{
		this.elementSkillName.text = skill.SkillName;
		for (int i = 0; i < skill.Elements.Count; i++)
		{
			this.elements[i].sprite = Singleton<StaticData>.Instance.ElementSprites[skill.Elements[i] % 10];
		}
	}

	// Token: 0x04000713 RID: 1811
	[SerializeField]
	private TextMeshProUGUI elementSkillName;

	// Token: 0x04000714 RID: 1812
	[SerializeField]
	private Image[] elements;
}
