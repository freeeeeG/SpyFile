using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200052E RID: 1326
public class BipedTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x06001FA4 RID: 8100 RVA: 0x000A8ADC File Offset: 0x000A6CDC
	public BipedTransitionLayer(Navigator navigator, float floor_speed, float ladder_speed) : base(navigator)
	{
		navigator.Subscribe(1773898642, delegate(object data)
		{
			this.isWalking = true;
		});
		navigator.Subscribe(1597112836, delegate(object data)
		{
			this.isWalking = false;
		});
		this.floorSpeed = floor_speed;
		this.ladderSpeed = ladder_speed;
		this.jetPackSpeed = floor_speed;
		this.movementSpeed = Db.Get().AttributeConverters.MovementSpeed.Lookup(navigator.gameObject);
		this.attributeLevels = navigator.GetComponent<AttributeLevels>();
	}

	// Token: 0x06001FA5 RID: 8101 RVA: 0x000A8B64 File Offset: 0x000A6D64
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		float num = 1f;
		bool flag = (transition.start == NavType.Pole || transition.end == NavType.Pole) && transition.y < 0 && transition.x == 0;
		bool flag2 = transition.start == NavType.Tube || transition.end == NavType.Tube;
		bool flag3 = transition.start == NavType.Hover || transition.end == NavType.Hover;
		if (!flag && !flag2 && !flag3)
		{
			if (this.isWalking)
			{
				return;
			}
			num = this.GetMovementSpeedMultiplier(navigator);
		}
		int cell = Grid.PosToCell(navigator);
		float num2 = 1f;
		bool flag4 = (navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) > PathFinder.PotentialPath.Flags.None;
		bool flag5 = (navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
		bool flag6 = (navigator.flags & PathFinder.PotentialPath.Flags.HasLeadSuit) > PathFinder.PotentialPath.Flags.None;
		if (!flag5 && !flag4 && !flag6 && Grid.IsSubstantialLiquid(cell, 0.35f))
		{
			num2 = 0.5f;
		}
		num *= num2;
		if (transition.x == 0 && (transition.start == NavType.Ladder || transition.start == NavType.Pole) && transition.start == transition.end)
		{
			if (flag)
			{
				transition.speed = 15f * num2;
			}
			else
			{
				transition.speed = this.ladderSpeed * num;
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					Ladder component = gameObject.GetComponent<Ladder>();
					if (component != null)
					{
						float num3 = component.upwardsMovementSpeedMultiplier;
						if (transition.y < 0)
						{
							num3 = component.downwardsMovementSpeedMultiplier;
						}
						transition.speed *= num3;
						transition.animSpeed *= num3;
					}
				}
			}
		}
		else if (flag2)
		{
			transition.speed = this.GetTubeTravellingSpeedMultiplier(navigator);
		}
		else if (flag3)
		{
			transition.speed = this.jetPackSpeed;
		}
		else
		{
			transition.speed = this.floorSpeed * num;
		}
		float num4 = num - 1f;
		transition.animSpeed += transition.animSpeed * num4 / 2f;
		if (transition.start == NavType.Floor && transition.end == NavType.Floor)
		{
			int num5 = Grid.CellBelow(cell);
			if (Grid.Foundation[num5])
			{
				GameObject gameObject2 = Grid.Objects[num5, 1];
				if (gameObject2 != null)
				{
					SimCellOccupier component2 = gameObject2.GetComponent<SimCellOccupier>();
					if (component2 != null)
					{
						transition.speed *= component2.movementSpeedMultiplier;
						transition.animSpeed *= component2.movementSpeedMultiplier;
					}
				}
			}
		}
		this.startTime = Time.time;
	}

	// Token: 0x06001FA6 RID: 8102 RVA: 0x000A8DE4 File Offset: 0x000A6FE4
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		bool flag = (transition.start == NavType.Pole || transition.end == NavType.Pole) && transition.y < 0 && transition.x == 0;
		bool flag2 = transition.start == NavType.Tube || transition.end == NavType.Tube;
		if (!this.isWalking && !flag && !flag2 && this.attributeLevels != null)
		{
			this.attributeLevels.AddExperience(Db.Get().Attributes.Athletics.Id, Time.time - this.startTime, DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
		}
	}

	// Token: 0x06001FA7 RID: 8103 RVA: 0x000A8E84 File Offset: 0x000A7084
	public float GetTubeTravellingSpeedMultiplier(Navigator navigator)
	{
		AttributeInstance attributeInstance = Db.Get().Attributes.TransitTubeTravelSpeed.Lookup(navigator.gameObject);
		if (attributeInstance != null)
		{
			return attributeInstance.GetTotalValue();
		}
		return 18f;
	}

	// Token: 0x06001FA8 RID: 8104 RVA: 0x000A8EBC File Offset: 0x000A70BC
	public float GetMovementSpeedMultiplier(Navigator navigator)
	{
		float num = 1f;
		if (this.movementSpeed != null)
		{
			num += this.movementSpeed.Evaluate();
		}
		return Mathf.Max(0.1f, num);
	}

	// Token: 0x040011AC RID: 4524
	private bool isWalking;

	// Token: 0x040011AD RID: 4525
	private float floorSpeed;

	// Token: 0x040011AE RID: 4526
	private float ladderSpeed;

	// Token: 0x040011AF RID: 4527
	private float startTime;

	// Token: 0x040011B0 RID: 4528
	private float jetPackSpeed;

	// Token: 0x040011B1 RID: 4529
	private const float downPoleSpeed = 15f;

	// Token: 0x040011B2 RID: 4530
	private const float WATER_SPEED_PENALTY = 0.5f;

	// Token: 0x040011B3 RID: 4531
	private AttributeConverterInstance movementSpeed;

	// Token: 0x040011B4 RID: 4532
	private AttributeLevels attributeLevels;
}
