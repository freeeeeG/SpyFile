using System;
using KSerialization;
using UnityEngine;

// Token: 0x020008E2 RID: 2274
[AddComponentMenu("KMonoBehaviour/scripts/HeatBulb")]
public class HeatBulb : KMonoBehaviour, ISim200ms
{
	// Token: 0x060041C3 RID: 16835 RVA: 0x0016FF29 File Offset: 0x0016E129
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.kanim.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060041C4 RID: 16836 RVA: 0x0016FF54 File Offset: 0x0016E154
	public void Sim200ms(float dt)
	{
		float num = this.kjConsumptionRate * dt;
		Vector2I vector2I = this.maxCheckOffset - this.minCheckOffset + 1;
		int num2 = vector2I.x * vector2I.y;
		float num3 = num / (float)num2;
		int num4;
		int num5;
		Grid.PosToXY(base.transform.GetPosition(), out num4, out num5);
		for (int i = this.minCheckOffset.y; i <= this.maxCheckOffset.y; i++)
		{
			for (int j = this.minCheckOffset.x; j <= this.maxCheckOffset.x; j++)
			{
				int num6 = Grid.XYToCell(num4 + j, num5 + i);
				if (Grid.IsValidCell(num6) && Grid.Temperature[num6] > this.minTemperature)
				{
					this.kjConsumed += num3;
					SimMessages.ModifyEnergy(num6, -num3, 5000f, SimMessages.EnergySourceID.HeatBulb);
				}
			}
		}
		float num7 = this.lightKJConsumptionRate * dt;
		if (this.kjConsumed > num7)
		{
			if (!this.lightSource.enabled)
			{
				this.kanim.Play("open", KAnim.PlayMode.Once, 1f, 0f);
				this.kanim.Queue("on", KAnim.PlayMode.Once, 1f, 0f);
				this.lightSource.enabled = true;
			}
			this.kjConsumed -= num7;
			return;
		}
		if (this.lightSource.enabled)
		{
			this.kanim.Play("close", KAnim.PlayMode.Once, 1f, 0f);
			this.kanim.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.lightSource.enabled = false;
	}

	// Token: 0x04002AE4 RID: 10980
	[SerializeField]
	private float minTemperature;

	// Token: 0x04002AE5 RID: 10981
	[SerializeField]
	private float kjConsumptionRate;

	// Token: 0x04002AE6 RID: 10982
	[SerializeField]
	private float lightKJConsumptionRate;

	// Token: 0x04002AE7 RID: 10983
	[SerializeField]
	private Vector2I minCheckOffset;

	// Token: 0x04002AE8 RID: 10984
	[SerializeField]
	private Vector2I maxCheckOffset;

	// Token: 0x04002AE9 RID: 10985
	[MyCmpGet]
	private Light2D lightSource;

	// Token: 0x04002AEA RID: 10986
	[MyCmpGet]
	private KBatchedAnimController kanim;

	// Token: 0x04002AEB RID: 10987
	[Serialize]
	private float kjConsumed;
}
