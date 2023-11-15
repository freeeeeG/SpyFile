using System;
using UnityEngine;

// Token: 0x02000B25 RID: 2853
[AddComponentMenu("KMonoBehaviour/scripts/InstantiateUIPrefabChild")]
public class InstantiateUIPrefabChild : KMonoBehaviour
{
	// Token: 0x060057D8 RID: 22488 RVA: 0x00201F1B File Offset: 0x0020011B
	protected override void OnPrefabInit()
	{
		if (this.InstantiateOnAwake)
		{
			this.Instantiate();
		}
	}

	// Token: 0x060057D9 RID: 22489 RVA: 0x00201F2C File Offset: 0x0020012C
	public void Instantiate()
	{
		if (this.alreadyInstantiated)
		{
			global::Debug.LogWarning(base.gameObject.name + "trying to instantiate UI prefabs multiple times.");
			return;
		}
		this.alreadyInstantiated = true;
		foreach (GameObject gameObject in this.prefabs)
		{
			if (!(gameObject == null))
			{
				Vector3 v = gameObject.rectTransform().anchoredPosition;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.transform.SetParent(base.transform);
				gameObject2.rectTransform().anchoredPosition = v;
				gameObject2.rectTransform().localScale = Vector3.one;
				if (this.setAsFirstSibling)
				{
					gameObject2.transform.SetAsFirstSibling();
				}
			}
		}
	}

	// Token: 0x04003B65 RID: 15205
	public GameObject[] prefabs;

	// Token: 0x04003B66 RID: 15206
	public bool InstantiateOnAwake = true;

	// Token: 0x04003B67 RID: 15207
	private bool alreadyInstantiated;

	// Token: 0x04003B68 RID: 15208
	public bool setAsFirstSibling;
}
