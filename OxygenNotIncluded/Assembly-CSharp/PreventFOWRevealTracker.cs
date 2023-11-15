using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020008F0 RID: 2288
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PreventFOWRevealTracker")]
public class PreventFOWRevealTracker : KMonoBehaviour
{
	// Token: 0x06004233 RID: 16947 RVA: 0x00172234 File Offset: 0x00170434
	[OnSerializing]
	private void OnSerialize()
	{
		this.preventFOWRevealCells.Clear();
		for (int i = 0; i < Grid.VisMasks.Length; i++)
		{
			if (Grid.PreventFogOfWarReveal[i])
			{
				this.preventFOWRevealCells.Add(i);
			}
		}
	}

	// Token: 0x06004234 RID: 16948 RVA: 0x00172278 File Offset: 0x00170478
	[OnDeserialized]
	private void OnDeserialized()
	{
		foreach (int i in this.preventFOWRevealCells)
		{
			Grid.PreventFogOfWarReveal[i] = true;
		}
	}

	// Token: 0x04002B3C RID: 11068
	[Serialize]
	public List<int> preventFOWRevealCells;
}
