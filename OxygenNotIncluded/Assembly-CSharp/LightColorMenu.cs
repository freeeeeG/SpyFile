using System;
using UnityEngine;

// Token: 0x020004C9 RID: 1225
[AddComponentMenu("KMonoBehaviour/scripts/LightColorMenu")]
public class LightColorMenu : KMonoBehaviour
{
	// Token: 0x06001BEE RID: 7150 RVA: 0x00094D9A File Offset: 0x00092F9A
	protected override void OnPrefabInit()
	{
		base.Subscribe<LightColorMenu>(493375141, LightColorMenu.OnRefreshUserMenuDelegate);
		this.SetColor(0);
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x00094DB4 File Offset: 0x00092FB4
	private void OnRefreshUserMenu(object data)
	{
		if (this.lightColors.Length != 0)
		{
			int num = this.lightColors.Length;
			for (int i = 0; i < num; i++)
			{
				if (i != this.currentColor)
				{
					int new_color = i;
					Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(this.lightColors[i].name, this.lightColors[i].name, delegate()
					{
						this.SetColor(new_color);
					}, global::Action.NumActions, null, null, null, "", true), 1f);
				}
			}
		}
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x00094E5C File Offset: 0x0009305C
	private void SetColor(int color_index)
	{
		if (this.lightColors.Length != 0 && color_index < this.lightColors.Length)
		{
			Light2D[] componentsInChildren = base.GetComponentsInChildren<Light2D>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Color = this.lightColors[color_index].color;
			}
			MeshRenderer[] componentsInChildren2 = base.GetComponentsInChildren<MeshRenderer>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				foreach (Material material in componentsInChildren2[i].materials)
				{
					if (material.name.StartsWith("matScriptedGlow01"))
					{
						material.color = this.lightColors[color_index].color;
					}
				}
			}
		}
		this.currentColor = color_index;
	}

	// Token: 0x04000F73 RID: 3955
	public LightColorMenu.LightColor[] lightColors;

	// Token: 0x04000F74 RID: 3956
	private int currentColor;

	// Token: 0x04000F75 RID: 3957
	private static readonly EventSystem.IntraObjectHandler<LightColorMenu> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<LightColorMenu>(delegate(LightColorMenu component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0200116F RID: 4463
	[Serializable]
	public struct LightColor
	{
		// Token: 0x06007997 RID: 31127 RVA: 0x002D986C File Offset: 0x002D7A6C
		public LightColor(string name, Color color)
		{
			this.name = name;
			this.color = color;
		}

		// Token: 0x04005C47 RID: 23623
		public string name;

		// Token: 0x04005C48 RID: 23624
		public Color color;
	}
}
