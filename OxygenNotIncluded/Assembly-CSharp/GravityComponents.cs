﻿using System;
using FMODUnity;
using UnityEngine;

// Token: 0x020007E5 RID: 2021
public class GravityComponents : KGameObjectComponentManager<GravityComponent>
{
	// Token: 0x06003931 RID: 14641 RVA: 0x0013FAA4 File Offset: 0x0013DCA4
	public HandleVector<int>.Handle Add(GameObject go, Vector2 initial_velocity, System.Action on_landed = null)
	{
		bool land_on_fake_floors = false;
		KPrefabID component = go.GetComponent<KPrefabID>();
		if (component != null)
		{
			land_on_fake_floors = component.HasAnyTags(GravityComponents.LANDS_ON_FAKEFLOOR);
		}
		bool mayLeaveWorld = go.GetComponent<MinionIdentity>() != null;
		return base.Add(go, new GravityComponent(go.transform, on_landed, initial_velocity, land_on_fake_floors, mayLeaveWorld));
	}

	// Token: 0x06003932 RID: 14642 RVA: 0x0013FAF4 File Offset: 0x0013DCF4
	public override void FixedUpdate(float dt)
	{
		GravityComponents.Tuning tuning = TuningData<GravityComponents.Tuning>.Get();
		float num = tuning.maxVelocity * tuning.maxVelocity;
		for (int i = 0; i < this.data.Count; i++)
		{
			GravityComponent gravityComponent = this.data[i];
			if (gravityComponent.elapsedTime >= 0f && !(gravityComponent.transform == null) && !base.IsInCleanupList(gravityComponent.transform.gameObject))
			{
				Vector3 position = gravityComponent.transform.GetPosition();
				Vector2 vector = position;
				Vector2 vector2 = new Vector2(gravityComponent.velocity.x, gravityComponent.velocity.y + -9.8f * dt);
				float sqrMagnitude = vector2.sqrMagnitude;
				if (sqrMagnitude > num)
				{
					vector2 *= tuning.maxVelocity / Mathf.Sqrt(sqrMagnitude);
				}
				int num2 = Grid.PosToCell(vector);
				bool flag = Grid.IsVisiblyInLiquid(vector - new Vector2(0f, gravityComponent.bottomYOffset));
				if (flag)
				{
					flag = true;
					float num3 = (float)(gravityComponent.transform.GetInstanceID() % 1000) / 1000f * 0.25f;
					float num4 = tuning.maxVelocityInLiquid + num3 * tuning.maxVelocityInLiquid;
					if (sqrMagnitude > num4 * num4)
					{
						float num5 = Mathf.Sqrt(sqrMagnitude);
						vector2 = vector2 / num5 * Mathf.Lerp(num5, num3, dt * (5f + 5f * num3));
					}
				}
				gravityComponent.velocity = vector2;
				gravityComponent.elapsedTime += dt;
				Vector2 vector3 = vector + vector2 * dt;
				Vector2 pos = vector3;
				pos.y = vector3.y - gravityComponent.bottomYOffset;
				bool flag2 = Grid.IsVisiblyInLiquid(vector3 + new Vector2(0f, gravityComponent.bottomYOffset));
				if (!flag && flag2)
				{
					KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("splash_step_kanim", new Vector3(vector3.x, vector3.y, 0f) + new Vector3(-0.38f, 0.75f, -0.1f), null, false, Grid.SceneLayer.FXFront, false);
					kbatchedAnimController.Play("fx1", KAnim.PlayMode.Once, 1f, 0f);
					kbatchedAnimController.destroyOnAnimComplete = true;
				}
				bool flag3 = false;
				int num6 = Grid.PosToCell(pos);
				if (Grid.IsValidCell(num6))
				{
					if (vector2.sqrMagnitude > 0.2f && Grid.IsValidCell(num2) && !Grid.Element[num2].IsLiquid && Grid.Element[num6].IsLiquid)
					{
						AmbienceType ambience = Grid.Element[num6].substance.GetAmbience();
						if (ambience != AmbienceType.None)
						{
							EventReference event_ref = Sounds.Instance.OreSplashSoundsMigrated[(int)ambience];
							if (CameraController.Instance != null && CameraController.Instance.IsAudibleSound(vector3, event_ref))
							{
								SoundEvent.PlayOneShot(event_ref, vector3, 1f);
							}
						}
					}
					bool flag4 = Grid.Solid[num6];
					if (!flag4 && gravityComponent.landOnFakeFloors && Grid.FakeFloor[num6])
					{
						Navigator component = gravityComponent.transform.GetComponent<Navigator>();
						if (component)
						{
							flag4 = component.NavGrid.NavTable.IsValid(num6, NavType.Floor);
							if (!flag4)
							{
								int cell = Grid.CellAbove(num6);
								flag4 = component.NavGrid.NavTable.IsValid(cell, NavType.Hover);
							}
						}
					}
					if (flag4)
					{
						Vector3 vector4 = Grid.CellToPosCBC(Grid.CellAbove(num6), Grid.SceneLayer.Move);
						vector3.y = vector4.y + gravityComponent.bottomYOffset;
						gravityComponent.velocity.x = 0f;
						flag3 = true;
					}
					else
					{
						Vector2 pos2 = vector3;
						pos2.x -= gravityComponent.extents.x;
						int num7 = Grid.PosToCell(pos2);
						if (Grid.IsValidCell(num7) && Grid.Solid[num7])
						{
							vector3.x = Mathf.Floor(vector3.x - gravityComponent.extents.x) + (1f + gravityComponent.extents.x);
							gravityComponent.velocity.x = -0.1f * gravityComponent.velocity.x;
						}
						else
						{
							Vector3 pos3 = vector3;
							pos3.x += gravityComponent.extents.x;
							int num8 = Grid.PosToCell(pos3);
							if (Grid.IsValidCell(num8) && Grid.Solid[num8])
							{
								vector3.x = Mathf.Floor(vector3.x + gravityComponent.extents.x) - gravityComponent.extents.x;
								gravityComponent.velocity.x = -0.1f * gravityComponent.velocity.x;
							}
						}
					}
				}
				this.data[i] = gravityComponent;
				int cell2 = Grid.PosToCell(vector3);
				if (gravityComponent.mayLeaveWorld || !Grid.IsValidCell(num2) || Grid.WorldIdx[num2] == 255 || Grid.IsValidCellInWorld(cell2, (int)Grid.WorldIdx[num2]))
				{
					gravityComponent.transform.SetPosition(new Vector3(vector3.x, vector3.y, position.z));
					if (flag3)
					{
						gravityComponent.transform.gameObject.Trigger(1188683690, vector2);
						if (gravityComponent.onLanded != null)
						{
							gravityComponent.onLanded();
						}
					}
				}
			}
		}
	}

	// Token: 0x040025E3 RID: 9699
	private const float Acceleration = -9.8f;

	// Token: 0x040025E4 RID: 9700
	private static Tag[] LANDS_ON_FAKEFLOOR = new Tag[]
	{
		GameTags.Minion,
		GameTags.Creatures.Walker,
		GameTags.Creatures.Hoverer
	};

	// Token: 0x0200159E RID: 5534
	public class Tuning : TuningData<GravityComponents.Tuning>
	{
		// Token: 0x0400693B RID: 26939
		public float maxVelocity;

		// Token: 0x0400693C RID: 26940
		public float maxVelocityInLiquid;
	}
}
