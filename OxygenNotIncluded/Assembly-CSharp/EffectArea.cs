using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020004AC RID: 1196
[AddComponentMenu("KMonoBehaviour/scripts/EffectArea")]
public class EffectArea : KMonoBehaviour
{
	// Token: 0x06001B35 RID: 6965 RVA: 0x00092185 File Offset: 0x00090385
	protected override void OnPrefabInit()
	{
		this.Effect = Db.Get().effects.Get(this.EffectName);
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x000921A4 File Offset: 0x000903A4
	private void Update()
	{
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(base.transform.GetPosition(), out num, out num2);
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			int num3 = 0;
			int num4 = 0;
			Grid.PosToXY(minionIdentity.transform.GetPosition(), out num3, out num4);
			if (Math.Abs(num3 - num) <= this.Area && Math.Abs(num4 - num2) <= this.Area)
			{
				minionIdentity.GetComponent<Effects>().Add(this.Effect, true);
			}
		}
	}

	// Token: 0x04000F21 RID: 3873
	public string EffectName;

	// Token: 0x04000F22 RID: 3874
	public int Area;

	// Token: 0x04000F23 RID: 3875
	private Effect Effect;
}
