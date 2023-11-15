using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000656 RID: 1622
public class MissileLauncher : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>
{
	// Token: 0x06002AC2 RID: 10946 RVA: 0x000E3D88 File Offset: 0x000E1F88
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Off;
		this.root.Update(delegate(MissileLauncher.Instance smi, float dt)
		{
			smi.HasLineOfSight();
		}, UpdateRate.SIM_200ms, false);
		this.Off.PlayAnim("inoperational").EventTransition(GameHashes.OperationalChanged, this.On, (MissileLauncher.Instance smi) => smi.Operational.IsOperational).Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.Operational.SetActive(false, false);
		});
		this.On.DefaultState(this.On.opening).EventTransition(GameHashes.OperationalChanged, this.On.shutdown, (MissileLauncher.Instance smi) => !smi.Operational.IsOperational).ParamTransition<bool>(this.fullyBlocked, this.Nosurfacesight, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsTrue).ScheduleGoTo(this.shutdownDuration, this.On.idle).Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.Operational.SetActive(smi.Operational.IsOperational, false);
		});
		this.On.opening.PlayAnim("working_pre").OnAnimQueueComplete(this.On.searching).Target(this.cannonTarget).PlayAnim("Cannon_working_pre");
		this.On.searching.PlayAnim("on", KAnim.PlayMode.Loop).Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.sm.rotationComplete.Set(false, smi, false);
			smi.sm.meteorTarget.Set(null, smi, false);
			smi.cannonRotation = smi.def.scanningAngle;
		}).Update("FindMeteor", delegate(MissileLauncher.Instance smi, float dt)
		{
			smi.Searching(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).EventTransition(GameHashes.OnStorageChange, this.NoAmmo, (MissileLauncher.Instance smi) => smi.MissileStorage.Count <= 0).ParamTransition<GameObject>(this.meteorTarget, this.Launch.targeting, (MissileLauncher.Instance smi, GameObject meteor) => meteor != null).Exit(delegate(MissileLauncher.Instance smi)
		{
			smi.sm.rotationComplete.Set(false, smi, false);
		});
		this.On.idle.Target(this.masterTarget).PlayAnim("idle", KAnim.PlayMode.Loop).UpdateTransition(this.On, (MissileLauncher.Instance smi, float dt) => smi.Operational.IsOperational && smi.MeteorDetected(), UpdateRate.SIM_200ms, false).Target(this.cannonTarget).PlayAnim("Cannon_working_pst");
		this.On.shutdown.Target(this.masterTarget).PlayAnim("working_pst").OnAnimQueueComplete(this.Off).Target(this.cannonTarget).PlayAnim("Cannon_working_pst");
		this.Launch.PlayAnim("target_detected", KAnim.PlayMode.Loop).Update("Rotate", delegate(MissileLauncher.Instance smi, float dt)
		{
			smi.RotateToMeteor(dt);
		}, UpdateRate.SIM_EVERY_TICK, false);
		this.Launch.targeting.Update("Targeting", delegate(MissileLauncher.Instance smi, float dt)
		{
			if (smi.sm.meteorTarget.Get(smi).IsNullOrDestroyed())
			{
				smi.GoTo(this.On.searching);
				return;
			}
			if (smi.cannonAnimController.Rotation < smi.def.maxAngle * -1f || smi.cannonAnimController.Rotation > smi.def.maxAngle)
			{
				smi.sm.meteorTarget.Get(smi).GetComponent<Comet>().Targeted = false;
				smi.sm.meteorTarget.Set(null, smi, false);
				smi.GoTo(this.On.searching);
			}
		}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<bool>(this.rotationComplete, this.Launch.shoot, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsTrue);
		this.Launch.shoot.ScheduleGoTo(this.shootDelayDuration, this.Launch.pst).Exit("LaunchMissile", delegate(MissileLauncher.Instance smi)
		{
			smi.LaunchMissile();
			this.cannonTarget.Get(smi).GetComponent<KBatchedAnimController>().Play("Cannon_shooting_pre", KAnim.PlayMode.Once, 1f, 0f);
		});
		this.Launch.pst.Target(this.masterTarget).Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.SetOreChunk();
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			if (smi.GetComponent<Storage>().Count <= 0)
			{
				component.Play("base_shooting_pst_last", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			component.Play("base_shooting_pst", KAnim.PlayMode.Once, 1f, 0f);
		}).Target(this.cannonTarget).PlayAnim("Cannon_shooting_pst").OnAnimQueueComplete(this.Cooldown);
		this.Cooldown.Update("Rotate", delegate(MissileLauncher.Instance smi, float dt)
		{
			smi.RotateToMeteor(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).Exit(delegate(MissileLauncher.Instance smi)
		{
			smi.SpawnOre();
		}).Enter(delegate(MissileLauncher.Instance smi)
		{
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			if (smi.GetComponent<Storage>().Count <= 0)
			{
				component.Play("base_ejecting_last", KAnim.PlayMode.Once, 1f, 0f);
			}
			else
			{
				component.Play("base_ejecting", KAnim.PlayMode.Once, 1f, 0f);
			}
			smi.sm.rotationComplete.Set(false, smi, false);
			smi.sm.meteorTarget.Set(null, smi, false);
		}).OnAnimQueueComplete(this.On.searching);
		this.Nosurfacesight.Target(this.masterTarget).PlayAnim("working_pst").QueueAnim("error", false, null).ParamTransition<bool>(this.fullyBlocked, this.On, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsFalse).Target(this.cannonTarget).PlayAnim("Cannon_working_pst").Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.Operational.SetActive(false, false);
		});
		this.NoAmmo.PlayAnim("off_open").EventTransition(GameHashes.OnStorageChange, this.On, (MissileLauncher.Instance smi) => smi.MissileStorage.Count > 0).Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.Operational.SetActive(false, false);
		}).Exit(delegate(MissileLauncher.Instance smi)
		{
			smi.GetComponent<KAnimControllerBase>().Play("off_closing", KAnim.PlayMode.Once, 1f, 0f);
		}).Target(this.cannonTarget).PlayAnim("Cannon_working_pst");
	}

	// Token: 0x040018F4 RID: 6388
	private static StatusItem NoSurfaceSight = new StatusItem("MissileLauncher_NoSurfaceSight", "BUILDING", "status_item_no_sky", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);

	// Token: 0x040018F5 RID: 6389
	private static StatusItem PartiallyBlockedStatus = new StatusItem("MissileLauncher_PartiallyBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x040018F6 RID: 6390
	public float shutdownDuration = 50f;

	// Token: 0x040018F7 RID: 6391
	public float shootDelayDuration = 0.25f;

	// Token: 0x040018F8 RID: 6392
	public static float SHELL_MASS = MissileBasicConfig.recipe.ingredients[0].amount / 5f / 2f;

	// Token: 0x040018F9 RID: 6393
	public static float SHELL_TEMPERATURE = 353.15f;

	// Token: 0x040018FA RID: 6394
	public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.BoolParameter rotationComplete;

	// Token: 0x040018FB RID: 6395
	public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.ObjectParameter<GameObject> meteorTarget = new StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.ObjectParameter<GameObject>();

	// Token: 0x040018FC RID: 6396
	public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.TargetParameter cannonTarget;

	// Token: 0x040018FD RID: 6397
	public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.BoolParameter fullyBlocked;

	// Token: 0x040018FE RID: 6398
	public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State Off;

	// Token: 0x040018FF RID: 6399
	public MissileLauncher.OnState On;

	// Token: 0x04001900 RID: 6400
	public MissileLauncher.LaunchState Launch;

	// Token: 0x04001901 RID: 6401
	public MissileLauncher.CooldownState Cooldown;

	// Token: 0x04001902 RID: 6402
	public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State Nosurfacesight;

	// Token: 0x04001903 RID: 6403
	public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State NoAmmo;

	// Token: 0x02001333 RID: 4915
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040061C4 RID: 25028
		public static readonly CellOffset LaunchOffset = new CellOffset(0, 4);

		// Token: 0x040061C5 RID: 25029
		public float launchSpeed = 30f;

		// Token: 0x040061C6 RID: 25030
		public float rotationSpeed = 100f;

		// Token: 0x040061C7 RID: 25031
		public static readonly Vector2I launchRange = new Vector2I(16, 32);

		// Token: 0x040061C8 RID: 25032
		public float scanningAngle = 50f;

		// Token: 0x040061C9 RID: 25033
		public float maxAngle = 80f;
	}

	// Token: 0x02001334 RID: 4916
	public new class Instance : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.GameInstance
	{
		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x0600802B RID: 32811 RVA: 0x002EFE54 File Offset: 0x002EE054
		public WorldContainer myWorld
		{
			get
			{
				if (this.worldContainer == null)
				{
					this.worldContainer = this.GetMyWorld();
				}
				return this.worldContainer;
			}
		}

		// Token: 0x0600802C RID: 32812 RVA: 0x002EFE78 File Offset: 0x002EE078
		public Instance(IStateMachineTarget master, MissileLauncher.Def def) : base(master, def)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			string name = component.name + ".cannon";
			base.smi.cannonGameObject = new GameObject(name);
			base.smi.cannonGameObject.SetActive(false);
			base.smi.cannonGameObject.transform.parent = component.transform;
			base.smi.cannonGameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
			base.smi.cannonAnimController = base.smi.cannonGameObject.AddComponent<KBatchedAnimController>();
			base.smi.cannonAnimController.AnimFiles = new KAnimFile[]
			{
				component.AnimFiles[0]
			};
			base.smi.cannonAnimController.initialAnim = "Cannon_off";
			base.smi.cannonAnimController.isMovable = true;
			base.smi.cannonAnimController.SetSceneLayer(Grid.SceneLayer.Building);
			component.SetSymbolVisiblity("cannon_target", false);
			bool flag;
			Vector3 position = component.GetSymbolTransform(new HashedString("cannon_target"), out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Building);
			base.smi.cannonGameObject.transform.SetPosition(position);
			this.launchPosition = position;
			Grid.PosToXY(this.launchPosition, out this.launchXY);
			base.smi.cannonGameObject.SetActive(true);
			base.smi.sm.cannonTarget.Set(base.smi.cannonGameObject, base.smi, false);
			KAnim.Anim anim = component.AnimFiles[0].GetData().GetAnim("Cannon_shooting_pre");
			if (anim != null)
			{
				this.launchAnimTime = anim.totalTime / 2f;
			}
			else
			{
				global::Debug.LogWarning("MissileLauncher anim data is missing");
				this.launchAnimTime = 1f;
			}
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			base.Subscribe(-1201923725, new Action<object>(this.OnHighlight));
			this.MissileStorage.Subscribe(-1697596308, new Action<object>(this.OnStorage));
			FlatTagFilterable component2 = base.smi.master.GetComponent<FlatTagFilterable>();
			foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.Comet))
			{
				if (!go.HasTag(GameTags.DeprecatedContent))
				{
					if (!component2.tagOptions.Contains(go.PrefabID()))
					{
						component2.tagOptions.Add(go.PrefabID());
						component2.selectedTags.Add(go.PrefabID());
					}
					component2.selectedTags.Remove(GassyMooCometConfig.ID);
				}
			}
		}

		// Token: 0x0600802D RID: 32813 RVA: 0x002F0174 File Offset: 0x002EE374
		public override void StartSM()
		{
			base.StartSM();
			this.OnStorage(null);
		}

		// Token: 0x0600802E RID: 32814 RVA: 0x002F0183 File Offset: 0x002EE383
		protected override void OnCleanUp()
		{
			base.Unsubscribe(-1201923725, new Action<object>(this.OnHighlight));
			base.OnCleanUp();
		}

		// Token: 0x0600802F RID: 32815 RVA: 0x002F01A4 File Offset: 0x002EE3A4
		private void OnHighlight(object data)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			base.smi.cannonAnimController.HighlightColour = component.HighlightColour;
		}

		// Token: 0x06008030 RID: 32816 RVA: 0x002F01CE File Offset: 0x002EE3CE
		private void OnStorage(object data)
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(this.MissileStorage.MassStored() / this.MissileStorage.capacityKg));
		}

		// Token: 0x06008031 RID: 32817 RVA: 0x002F01F8 File Offset: 0x002EE3F8
		public void Searching(float dt)
		{
			this.FindMeteor();
			this.RotateCannon(dt, base.def.rotationSpeed / 2f);
			if (base.smi.sm.rotationComplete.Get(base.smi))
			{
				this.cannonRotation *= -1f;
				base.smi.sm.rotationComplete.Set(false, base.smi, false);
			}
		}

		// Token: 0x06008032 RID: 32818 RVA: 0x002F0270 File Offset: 0x002EE470
		public void FindMeteor()
		{
			GameObject gameObject = this.ChooseClosestInterceptionPoint(this.myWorld.id);
			if (gameObject != null)
			{
				base.smi.sm.meteorTarget.Set(gameObject, base.smi, false);
				gameObject.GetComponent<Comet>().Targeted = true;
				base.smi.cannonRotation = this.CalculateLaunchAngle(gameObject.transform.position);
			}
		}

		// Token: 0x06008033 RID: 32819 RVA: 0x002F02E0 File Offset: 0x002EE4E0
		private float CalculateLaunchAngle(Vector3 targetPosition)
		{
			Vector3 v = Vector3.Normalize(targetPosition - this.launchPosition);
			return MathUtil.AngleSigned(Vector3.up, v, Vector3.forward);
		}

		// Token: 0x06008034 RID: 32820 RVA: 0x002F0310 File Offset: 0x002EE510
		public void LaunchMissile()
		{
			GameObject gameObject = this.MissileStorage.FindFirst("MissileBasic");
			if (gameObject != null)
			{
				Pickupable pickupable = gameObject.GetComponent<Pickupable>();
				if (pickupable.TotalAmount <= 1f)
				{
					this.MissileStorage.Drop(pickupable.gameObject, true);
				}
				else
				{
					pickupable = EntitySplitter.Split(pickupable, 1f, null);
				}
				this.SetMissileElement(gameObject);
				GameObject gameObject2 = base.smi.sm.meteorTarget.Get(base.smi);
				if (!gameObject2.IsNullOrDestroyed())
				{
					pickupable.GetSMI<MissileProjectile.StatesInstance>().PrepareLaunch(gameObject2.GetComponent<Comet>(), base.def.launchSpeed, this.launchPosition, base.smi.cannonRotation);
				}
			}
		}

		// Token: 0x06008035 RID: 32821 RVA: 0x002F03D0 File Offset: 0x002EE5D0
		private void SetMissileElement(GameObject missile)
		{
			this.missileElement = missile.GetComponent<PrimaryElement>().Element.tag;
			if (Assets.GetPrefab(this.missileElement) == null)
			{
				global::Debug.LogWarning(string.Format("Missing element {0} for missile launcher. Defaulting to IronOre", this.missileElement));
				this.missileElement = GameTags.IronOre;
			}
		}

		// Token: 0x06008036 RID: 32822 RVA: 0x002F042C File Offset: 0x002EE62C
		public GameObject ChooseClosestInterceptionPoint(int world_id)
		{
			GameObject result = null;
			List<Comet> items = Components.Meteors.GetItems(world_id);
			float num = (float)MissileLauncher.Def.launchRange.y;
			foreach (Comet comet in items)
			{
				if (!comet.IsNullOrDestroyed() && !comet.Targeted && this.TargetFilter.selectedTags.Contains(comet.typeID))
				{
					Vector3 targetPosition = comet.TargetPosition;
					float num2;
					Vector3 vector = this.CalculateCollisionPoint(targetPosition, comet.Velocity, out num2);
					Grid.PosToCell(vector);
					float num3 = Vector3.Distance(vector, this.launchPosition);
					if (num3 < num && num2 > this.launchAnimTime && this.IsMeteorInRange(vector) && this.IsPathClear(this.launchPosition, targetPosition))
					{
						result = comet.gameObject;
						num = num3;
					}
				}
			}
			return result;
		}

		// Token: 0x06008037 RID: 32823 RVA: 0x002F0530 File Offset: 0x002EE730
		private bool IsMeteorInRange(Vector3 interception_point)
		{
			Vector2I vector2I;
			Grid.PosToXY(interception_point, out vector2I);
			return Math.Abs(vector2I.X - this.launchXY.X) <= MissileLauncher.Def.launchRange.X && vector2I.Y - this.launchXY.Y > 0 && vector2I.Y - this.launchXY.Y <= MissileLauncher.Def.launchRange.Y;
		}

		// Token: 0x06008038 RID: 32824 RVA: 0x002F05AC File Offset: 0x002EE7AC
		public bool IsPathClear(Vector3 startPoint, Vector3 endPoint)
		{
			Vector2I vector2I = Grid.PosToXY(startPoint);
			Vector2I vector2I2 = Grid.PosToXY(endPoint);
			return Grid.TestLineOfSight(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y, new Func<int, bool>(this.IsCellBlockedFromSky), false, true);
		}

		// Token: 0x06008039 RID: 32825 RVA: 0x002F05F4 File Offset: 0x002EE7F4
		public bool IsCellBlockedFromSky(int cell)
		{
			if (Grid.IsValidCell(cell) && (int)Grid.WorldIdx[cell] == this.myWorld.id)
			{
				return Grid.Solid[cell];
			}
			int num;
			int num2;
			Grid.CellToXY(cell, out num, out num2);
			return num2 <= this.launchXY.Y;
		}

		// Token: 0x0600803A RID: 32826 RVA: 0x002F0644 File Offset: 0x002EE844
		public Vector3 CalculateCollisionPoint(Vector3 targetPosition, Vector3 targetVelocity, out float timeToCollision)
		{
			Vector3 vector = targetVelocity - base.smi.def.launchSpeed * (targetPosition - this.launchPosition).normalized;
			timeToCollision = (targetPosition - this.launchPosition).magnitude / vector.magnitude;
			return targetPosition + targetVelocity * timeToCollision;
		}

		// Token: 0x0600803B RID: 32827 RVA: 0x002F06B0 File Offset: 0x002EE8B0
		public void HasLineOfSight()
		{
			bool flag = false;
			bool flag2 = true;
			Extents extents = base.GetComponent<Building>().GetExtents();
			int val = this.launchXY.x - MissileLauncher.Def.launchRange.X;
			int val2 = this.launchXY.x + MissileLauncher.Def.launchRange.X;
			int y = extents.y + extents.height;
			int num = Grid.XYToCell(Math.Max((int)this.myWorld.minimumBounds.x, val), y);
			int num2 = Grid.XYToCell(Math.Min((int)this.myWorld.maximumBounds.x, val2), y);
			for (int i = num; i <= num2; i++)
			{
				flag = (flag || Grid.ExposedToSunlight[i] <= 0);
				flag2 = (flag2 && Grid.ExposedToSunlight[i] <= 0);
			}
			this.Selectable.ToggleStatusItem(MissileLauncher.PartiallyBlockedStatus, flag && !flag2, null);
			this.Selectable.ToggleStatusItem(MissileLauncher.NoSurfaceSight, flag2, null);
			base.smi.sm.fullyBlocked.Set(flag2, base.smi, false);
		}

		// Token: 0x0600803C RID: 32828 RVA: 0x002F07E1 File Offset: 0x002EE9E1
		public bool MeteorDetected()
		{
			return Components.Meteors.GetItems(this.myWorld.id).Count > 0;
		}

		// Token: 0x0600803D RID: 32829 RVA: 0x002F0800 File Offset: 0x002EEA00
		public void SetOreChunk()
		{
			KAnim.Build.Symbol symbolByIndex = Assets.GetPrefab(this.missileElement).GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
			base.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("Shell", symbolByIndex, 0);
		}

		// Token: 0x0600803E RID: 32830 RVA: 0x002F0854 File Offset: 0x002EEA54
		public void SpawnOre()
		{
			bool flag;
			Vector3 position = base.GetComponent<KBatchedAnimController>().GetSymbolTransform("Shell", out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Assets.GetPrefab(this.missileElement).GetComponent<PrimaryElement>().Element.substance.SpawnResource(position, MissileLauncher.SHELL_MASS, MissileLauncher.SHELL_TEMPERATURE, byte.MaxValue, 0, false, false, false);
		}

		// Token: 0x0600803F RID: 32831 RVA: 0x002F08CC File Offset: 0x002EEACC
		public void RotateCannon(float dt, float rotation_speed)
		{
			float num = this.cannonRotation - this.simpleAngle;
			if (num > 180f)
			{
				num -= 360f;
			}
			else if (num < -180f)
			{
				num += 360f;
			}
			float num2 = rotation_speed * dt;
			if (num > 0f && num2 < num)
			{
				this.simpleAngle += num2;
				this.cannonAnimController.Rotation = this.simpleAngle;
				return;
			}
			if (num < 0f && -num2 > num)
			{
				this.simpleAngle -= num2;
				this.cannonAnimController.Rotation = this.simpleAngle;
				return;
			}
			this.simpleAngle = this.cannonRotation;
			this.cannonAnimController.Rotation = this.simpleAngle;
			base.smi.sm.rotationComplete.Set(true, base.smi, false);
		}

		// Token: 0x06008040 RID: 32832 RVA: 0x002F09A4 File Offset: 0x002EEBA4
		public void RotateToMeteor(float dt)
		{
			GameObject gameObject = base.sm.meteorTarget.Get(this);
			if (gameObject.IsNullOrDestroyed())
			{
				return;
			}
			float num = this.CalculateLaunchAngle(gameObject.transform.position) - this.simpleAngle;
			if (num > 180f)
			{
				num -= 360f;
			}
			else if (num < -180f)
			{
				num += 360f;
			}
			float num2 = base.def.rotationSpeed * dt;
			if (num > 0f && num2 < num)
			{
				this.simpleAngle += num2;
				this.cannonAnimController.Rotation = this.simpleAngle;
				return;
			}
			if (num < 0f && -num2 > num)
			{
				this.simpleAngle -= num2;
				this.cannonAnimController.Rotation = this.simpleAngle;
				return;
			}
			base.smi.sm.rotationComplete.Set(true, base.smi, false);
		}

		// Token: 0x040061CA RID: 25034
		[MyCmpReq]
		public Operational Operational;

		// Token: 0x040061CB RID: 25035
		[MyCmpReq]
		public Storage MissileStorage;

		// Token: 0x040061CC RID: 25036
		[MyCmpReq]
		public KSelectable Selectable;

		// Token: 0x040061CD RID: 25037
		[MyCmpReq]
		public FlatTagFilterable TargetFilter;

		// Token: 0x040061CE RID: 25038
		private Vector3 launchPosition;

		// Token: 0x040061CF RID: 25039
		private Vector2I launchXY;

		// Token: 0x040061D0 RID: 25040
		private float launchAnimTime;

		// Token: 0x040061D1 RID: 25041
		public KBatchedAnimController cannonAnimController;

		// Token: 0x040061D2 RID: 25042
		public GameObject cannonGameObject;

		// Token: 0x040061D3 RID: 25043
		public float cannonRotation;

		// Token: 0x040061D4 RID: 25044
		public float simpleAngle;

		// Token: 0x040061D5 RID: 25045
		private Tag missileElement;

		// Token: 0x040061D6 RID: 25046
		private MeterController meter;

		// Token: 0x040061D7 RID: 25047
		private WorldContainer worldContainer;
	}

	// Token: 0x02001335 RID: 4917
	public class OnState : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State
	{
		// Token: 0x040061D8 RID: 25048
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State searching;

		// Token: 0x040061D9 RID: 25049
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State opening;

		// Token: 0x040061DA RID: 25050
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State shutdown;

		// Token: 0x040061DB RID: 25051
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State idle;
	}

	// Token: 0x02001336 RID: 4918
	public class LaunchState : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State
	{
		// Token: 0x040061DC RID: 25052
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State targeting;

		// Token: 0x040061DD RID: 25053
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State shoot;

		// Token: 0x040061DE RID: 25054
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State pst;
	}

	// Token: 0x02001337 RID: 4919
	public class CooldownState : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State
	{
		// Token: 0x040061DF RID: 25055
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State cooling;

		// Token: 0x040061E0 RID: 25056
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State exit;

		// Token: 0x040061E1 RID: 25057
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State exitNoAmmo;
	}
}
