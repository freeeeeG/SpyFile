using System;
using STRINGS;
using UnityEngine;

// Token: 0x020007F7 RID: 2039
public class HighEnergyParticleConfig : IEntityConfig
{
	// Token: 0x06003A25 RID: 14885 RVA: 0x00144122 File Offset: 0x00142322
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06003A26 RID: 14886 RVA: 0x0014412C File Offset: 0x0014232C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("HighEnergyParticle", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, ITEMS.RADIATION.HIGHENERGYPARITCLE.DESC, 1f, false, Assets.GetAnim("spark_radial_high_energy_particles_kanim"), "travel_pre", Grid.SceneLayer.FXFront2, SimHashes.Creature, null, 293f);
		EntityTemplates.AddCollision(gameObject, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f);
		gameObject.AddOrGet<LoopingSounds>();
		RadiationEmitter radiationEmitter = gameObject.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 3;
		radiationEmitter.emitRadiusY = 3;
		radiationEmitter.emitRads = 0.4f * ((float)radiationEmitter.emitRadiusX / 6f);
		gameObject.AddComponent<HighEnergyParticle>().speed = 8f;
		return gameObject;
	}

	// Token: 0x06003A27 RID: 14887 RVA: 0x001441E3 File Offset: 0x001423E3
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06003A28 RID: 14888 RVA: 0x001441E5 File Offset: 0x001423E5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040026AD RID: 9901
	public const int PARTICLE_SPEED = 8;

	// Token: 0x040026AE RID: 9902
	public const float PARTICLE_COLLISION_SIZE = 0.2f;

	// Token: 0x040026AF RID: 9903
	public const float PER_CELL_FALLOFF = 0.1f;

	// Token: 0x040026B0 RID: 9904
	public const float FALLOUT_RATIO = 0.5f;

	// Token: 0x040026B1 RID: 9905
	public const int MAX_PAYLOAD = 500;

	// Token: 0x040026B2 RID: 9906
	public const int EXPLOSION_FALLOUT_TEMPERATURE = 5000;

	// Token: 0x040026B3 RID: 9907
	public const float EXPLOSION_FALLOUT_MASS_PER_PARTICLE = 0.001f;

	// Token: 0x040026B4 RID: 9908
	public const float EXPLOSION_EMIT_DURRATION = 1f;

	// Token: 0x040026B5 RID: 9909
	public const short EXPLOSION_EMIT_RADIUS = 6;

	// Token: 0x040026B6 RID: 9910
	public const string ID = "HighEnergyParticle";
}
