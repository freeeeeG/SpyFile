using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200078B RID: 1931
[Serializable]
public class TutorialPopup
{
	// Token: 0x06002558 RID: 9560 RVA: 0x000B0DFD File Offset: 0x000AF1FD
	public bool CanSpawn()
	{
		return this.SplashImage != null;
	}

	// Token: 0x06002559 RID: 9561 RVA: 0x000B0E0C File Offset: 0x000AF20C
	public GameObject Spawn()
	{
		if (this.CanSpawn())
		{
			GameObject gameObject = GameUtils.InstantiateUIController(this.Prefab, "UICanvas");
			GameObject gameObject2 = gameObject.RequireChild("Title");
			if (gameObject2 != null)
			{
				T17Text t17Text = gameObject2.RequireComponent<T17Text>();
				t17Text.SetNewPlaceHolder(this.Title);
				t17Text.SetNewLocalizationTag(this.Title);
			}
			GameObject gameObject3 = gameObject.RequestChild("Description");
			if (gameObject3 != null)
			{
				T17Text t17Text2 = gameObject3.RequireComponent<T17Text>();
				t17Text2.SetNewPlaceHolder(this.Description);
				t17Text2.SetNewLocalizationTag(this.Description);
			}
			GameObject obj = gameObject.RequestChild("Label");
			if (gameObject3 != null)
			{
				T17Text t17Text3 = obj.RequireComponent<T17Text>();
				t17Text3.SetNewPlaceHolder(this.LabelText);
				t17Text3.SetNewLocalizationTag(this.LabelText);
			}
			GameObject obj2 = gameObject.RequireChild("StepsImage");
			obj2.RequireComponent<Image>().sprite = this.SplashImage;
			return gameObject;
		}
		return null;
	}

	// Token: 0x04001CEC RID: 7404
	[AssignResource("TutorialSplash", Editorbility.Editable)]
	public GameObject Prefab;

	// Token: 0x04001CED RID: 7405
	public string Title;

	// Token: 0x04001CEE RID: 7406
	public string Description;

	// Token: 0x04001CEF RID: 7407
	public string LabelText = "Text.Tutorial.NewRecipe";

	// Token: 0x04001CF0 RID: 7408
	public Sprite SplashImage;
}
