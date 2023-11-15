using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class AmplifierSkill : InitialSkill
{
	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000418 RID: 1048 RVA: 0x0000B29F File Offset: 0x0000949F
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Amplifier;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000419 RID: 1049 RVA: 0x0000B2A3 File Offset: 0x000094A3
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x0600041A RID: 1050 RVA: 0x0000B2AC File Offset: 0x000094AC
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0000B2E8 File Offset: 0x000094E8
	public override void StartTurn()
	{
		base.StartTurn();
		this.intensifiedValue = this.KeyValue;
		RangeType rangeType = this.strategy.RangeType;
		if (rangeType != RangeType.Circle)
		{
			if (rangeType != RangeType.Line)
			{
				return;
			}
			float num = Vector2.Dot(Vector2.up, this.strategy.Concrete.transform.up);
			Vector3 vector = Vector3.Cross(Vector2.up, this.strategy.Concrete.transform.up);
			using (List<Vector2Int>.Enumerator enumerator = StaticData.GetLinePoints(this.strategy.FinalRange, 0).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Vector2Int v = enumerator.Current;
					Vector2 a;
					if (vector.z < -0.1f)
					{
						a = new Vector2Int(v.y, v.x);
					}
					else if (vector.z > 0.1f)
					{
						a = new Vector2Int(-v.y, v.x);
					}
					else if (num < -0.1f)
					{
						a = new Vector2Int(v.x, -v.y);
					}
					else
					{
						a = v;
					}
					Collider2D collider2D = StaticData.RaycastCollider(a + this.strategy.Concrete.transform.position, LayerMask.GetMask(new string[]
					{
						StaticData.TrapMask
					}));
					if (collider2D != null)
					{
						TrapContent component = collider2D.GetComponent<TrapContent>();
						component.TrapIntensify += this.intensifiedValue;
						this.intensifiedTraps.Add(component);
					}
				}
				return;
			}
		}
		foreach (Vector2Int v2 in StaticData.GetCirclePoints(this.strategy.FinalRange, 0))
		{
			Collider2D collider2D2 = StaticData.RaycastCollider(v2 + this.strategy.Concrete.transform.position, LayerMask.GetMask(new string[]
			{
				StaticData.TrapMask
			}));
			if (collider2D2 != null)
			{
				TrapContent component2 = collider2D2.GetComponent<TrapContent>();
				component2.TrapIntensify += this.intensifiedValue;
				this.intensifiedTraps.Add(component2);
			}
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0000B594 File Offset: 0x00009794
	public override void EndTurn()
	{
		foreach (TrapContent trapContent in this.intensifiedTraps)
		{
			trapContent.TrapIntensify -= this.intensifiedValue;
		}
		this.intensifiedValue = 0f;
		this.intensifiedTraps.Clear();
		base.EndTurn();
	}

	// Token: 0x04000191 RID: 401
	private List<TrapContent> intensifiedTraps = new List<TrapContent>();

	// Token: 0x04000192 RID: 402
	private float intensifiedValue;
}
