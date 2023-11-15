using System;
using UnityEngine;

// Token: 0x02000B18 RID: 2840
[AddComponentMenu("KMonoBehaviour/scripts/HierarchyReferences")]
public class HierarchyReferences : KMonoBehaviour
{
	// Token: 0x06005787 RID: 22407 RVA: 0x00200C78 File Offset: 0x001FEE78
	public bool HasReference(string name)
	{
		ElementReference[] array = this.references;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Name == name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005788 RID: 22408 RVA: 0x00200CB4 File Offset: 0x001FEEB4
	public SpecifiedType GetReference<SpecifiedType>(string name) where SpecifiedType : Component
	{
		foreach (ElementReference elementReference in this.references)
		{
			if (elementReference.Name == name)
			{
				if (elementReference.behaviour is SpecifiedType)
				{
					return (SpecifiedType)((object)elementReference.behaviour);
				}
				global::Debug.LogError(string.Format("Behavior is not specified type", Array.Empty<object>()));
			}
		}
		global::Debug.LogError(string.Format("Could not find UI reference '{0}' or convert to specified type)", name));
		return default(SpecifiedType);
	}

	// Token: 0x06005789 RID: 22409 RVA: 0x00200D34 File Offset: 0x001FEF34
	public Component GetReference(string name)
	{
		foreach (ElementReference elementReference in this.references)
		{
			if (elementReference.Name == name)
			{
				return elementReference.behaviour;
			}
		}
		global::Debug.LogWarning("Couldn't find reference to object named {0} Make sure the name matches the field in the inspector.");
		return null;
	}

	// Token: 0x04003B30 RID: 15152
	public ElementReference[] references;
}
