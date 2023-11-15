using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B08 RID: 2824
[AddComponentMenu("KMonoBehaviour/scripts/GoodnessSlider")]
public class GoodnessSlider : KMonoBehaviour
{
	// Token: 0x0600571B RID: 22299 RVA: 0x001FD29D File Offset: 0x001FB49D
	protected override void OnSpawn()
	{
		base.Spawn();
		this.UpdateValues();
	}

	// Token: 0x0600571C RID: 22300 RVA: 0x001FD2AC File Offset: 0x001FB4AC
	public void UpdateValues()
	{
		this.text.color = (this.fill.color = this.gradient.Evaluate(this.slider.value));
		for (int i = 0; i < this.gradient.colorKeys.Length; i++)
		{
			if (this.gradient.colorKeys[i].time < this.slider.value)
			{
				this.text.text = this.names[i];
			}
			if (i == this.gradient.colorKeys.Length - 1 && this.gradient.colorKeys[i - 1].time < this.slider.value)
			{
				this.text.text = this.names[i];
			}
		}
	}

	// Token: 0x04003AB4 RID: 15028
	public Image icon;

	// Token: 0x04003AB5 RID: 15029
	public Text text;

	// Token: 0x04003AB6 RID: 15030
	public Slider slider;

	// Token: 0x04003AB7 RID: 15031
	public Image fill;

	// Token: 0x04003AB8 RID: 15032
	public Gradient gradient;

	// Token: 0x04003AB9 RID: 15033
	public string[] names;
}
