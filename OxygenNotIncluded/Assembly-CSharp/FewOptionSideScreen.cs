using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C19 RID: 3097
public class FewOptionSideScreen : SideScreenContent
{
	// Token: 0x060061FF RID: 25087 RVA: 0x00242B6F File Offset: 0x00240D6F
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshOptions();
		}
	}

	// Token: 0x06006200 RID: 25088 RVA: 0x00242B84 File Offset: 0x00240D84
	private void RefreshOptions()
	{
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == this.targetFewOptions.GetSelectedOption()) ? 1 : 0);
		}
	}

	// Token: 0x06006201 RID: 25089 RVA: 0x00242C00 File Offset: 0x00240E00
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x06006202 RID: 25090 RVA: 0x00242C44 File Offset: 0x00240E44
	private void SpawnRows()
	{
		FewOptionSideScreen.IFewOptionSideScreen.Option[] options = this.targetFewOptions.GetOptions();
		for (int i = 0; i < options.Length; i++)
		{
			FewOptionSideScreen.IFewOptionSideScreen.Option option = options[i];
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(option.labelText);
			component.GetReference<Image>("icon").sprite = option.iconSpriteColorTuple.first;
			component.GetReference<Image>("icon").color = option.iconSpriteColorTuple.second;
			gameObject.GetComponent<ToolTip>().toolTip = option.tooltipText;
			gameObject.GetComponent<MultiToggle>().onClick = delegate()
			{
				this.targetFewOptions.OnOptionSelected(option);
				this.RefreshOptions();
			};
			this.rows.Add(option.tag, gameObject);
		}
		this.RefreshOptions();
	}

	// Token: 0x06006203 RID: 25091 RVA: 0x00242D4D File Offset: 0x00240F4D
	public override void SetTarget(GameObject target)
	{
		this.ClearRows();
		this.targetFewOptions = target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>();
		this.SpawnRows();
	}

	// Token: 0x06006204 RID: 25092 RVA: 0x00242D67 File Offset: 0x00240F67
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>() != null;
	}

	// Token: 0x040042C0 RID: 17088
	public GameObject rowPrefab;

	// Token: 0x040042C1 RID: 17089
	public RectTransform rowContainer;

	// Token: 0x040042C2 RID: 17090
	public Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();

	// Token: 0x040042C3 RID: 17091
	private FewOptionSideScreen.IFewOptionSideScreen targetFewOptions;

	// Token: 0x02001B65 RID: 7013
	public interface IFewOptionSideScreen
	{
		// Token: 0x060099CA RID: 39370
		FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions();

		// Token: 0x060099CB RID: 39371
		void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option);

		// Token: 0x060099CC RID: 39372
		Tag GetSelectedOption();

		// Token: 0x02002239 RID: 8761
		public struct Option
		{
			// Token: 0x0600AD32 RID: 44338 RVA: 0x00379241 File Offset: 0x00377441
			public Option(Tag tag, string labelText, global::Tuple<Sprite, Color> iconSpriteColorTuple, string tooltipText = "")
			{
				this.tag = tag;
				this.labelText = labelText;
				this.iconSpriteColorTuple = iconSpriteColorTuple;
				this.tooltipText = tooltipText;
			}

			// Token: 0x040098EF RID: 39151
			public Tag tag;

			// Token: 0x040098F0 RID: 39152
			public string labelText;

			// Token: 0x040098F1 RID: 39153
			public string tooltipText;

			// Token: 0x040098F2 RID: 39154
			public global::Tuple<Sprite, Color> iconSpriteColorTuple;
		}
	}
}
