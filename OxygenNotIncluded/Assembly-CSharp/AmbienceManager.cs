using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000462 RID: 1122
[AddComponentMenu("KMonoBehaviour/scripts/AmbienceManager")]
public class AmbienceManager : KMonoBehaviour
{
	// Token: 0x06001898 RID: 6296 RVA: 0x0007FC04 File Offset: 0x0007DE04
	protected override void OnSpawn()
	{
		if (!RuntimeManager.IsInitialized)
		{
			base.enabled = false;
			return;
		}
		for (int i = 0; i < this.quadrants.Length; i++)
		{
			this.quadrants[i] = new AmbienceManager.Quadrant(this.quadrantDefs[i]);
		}
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x0007FC48 File Offset: 0x0007DE48
	protected override void OnForcedCleanUp()
	{
		AmbienceManager.Quadrant[] array = this.quadrants;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (AmbienceManager.Layer layer in array[i].GetAllLayers())
			{
				layer.Stop();
			}
		}
	}

	// Token: 0x0600189A RID: 6298 RVA: 0x0007FCB0 File Offset: 0x0007DEB0
	private void LateUpdate()
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I min = visibleArea.Min;
		Vector2I max = visibleArea.Max;
		Vector2I vector2I = min + (max - min) / 2;
		Vector3 a = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = vector + (a - vector) / 2f;
		Vector3 vector3 = a - vector;
		if (vector3.x > vector3.y)
		{
			vector3.y = vector3.x;
		}
		else
		{
			vector3.x = vector3.y;
		}
		a = vector2 + vector3 / 2f;
		vector = vector2 - vector3 / 2f;
		Vector3 vector4 = vector3 / 2f / 2f;
		this.quadrants[0].Update(new Vector2I(min.x, min.y), new Vector2I(vector2I.x, vector2I.y), new Vector3(vector.x + vector4.x, vector.y + vector4.y, this.emitterZPosition));
		this.quadrants[1].Update(new Vector2I(vector2I.x, min.y), new Vector2I(max.x, vector2I.y), new Vector3(vector2.x + vector4.x, vector.y + vector4.y, this.emitterZPosition));
		this.quadrants[2].Update(new Vector2I(min.x, vector2I.y), new Vector2I(vector2I.x, max.y), new Vector3(vector.x + vector4.x, vector2.y + vector4.y, this.emitterZPosition));
		this.quadrants[3].Update(new Vector2I(vector2I.x, vector2I.y), new Vector2I(max.x, max.y), new Vector3(vector2.x + vector4.x, vector2.y + vector4.y, this.emitterZPosition));
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < this.quadrants.Length; i++)
		{
			num += (float)this.quadrants[i].spaceLayer.tileCount;
			num2 += (float)this.quadrants[i].facilityLayer.tileCount;
			num3 += (float)this.quadrants[i].totalTileCount;
		}
		AudioMixer.instance.UpdateSpaceVisibleSnapshot(num / num3);
		AudioMixer.instance.UpdateFacilityVisibleSnapshot(num2 / num3);
	}

	// Token: 0x04000D8C RID: 3468
	private float emitterZPosition;

	// Token: 0x04000D8D RID: 3469
	public AmbienceManager.QuadrantDef[] quadrantDefs;

	// Token: 0x04000D8E RID: 3470
	public AmbienceManager.Quadrant[] quadrants = new AmbienceManager.Quadrant[4];

	// Token: 0x020010E3 RID: 4323
	public class Tuning : TuningData<AmbienceManager.Tuning>
	{
		// Token: 0x04005A61 RID: 23137
		public int backwallTileValue = 1;

		// Token: 0x04005A62 RID: 23138
		public int foundationTileValue = 2;

		// Token: 0x04005A63 RID: 23139
		public int buildingTileValue = 3;
	}

	// Token: 0x020010E4 RID: 4324
	public class Layer : IComparable<AmbienceManager.Layer>
	{
		// Token: 0x0600779D RID: 30621 RVA: 0x002D40AA File Offset: 0x002D22AA
		public Layer(EventReference sound, EventReference one_shot_sound = default(EventReference))
		{
			this.sound = sound;
			this.oneShotSound = one_shot_sound;
		}

		// Token: 0x0600779E RID: 30622 RVA: 0x002D40C0 File Offset: 0x002D22C0
		public void Reset()
		{
			this.tileCount = 0;
			this.averageTemperature = 0f;
			this.averageRadiation = 0f;
		}

		// Token: 0x0600779F RID: 30623 RVA: 0x002D40DF File Offset: 0x002D22DF
		public void UpdatePercentage(int cell_count)
		{
			this.tilePercentage = (float)this.tileCount / (float)cell_count;
		}

		// Token: 0x060077A0 RID: 30624 RVA: 0x002D40F1 File Offset: 0x002D22F1
		public void UpdateAverageTemperature()
		{
			this.averageTemperature /= (float)this.tileCount;
			this.soundEvent.setParameterByName("averageTemperature", this.averageTemperature, false);
		}

		// Token: 0x060077A1 RID: 30625 RVA: 0x002D411F File Offset: 0x002D231F
		public void UpdateAverageRadiation()
		{
			this.averageRadiation = ((this.tileCount > 0) ? (this.averageRadiation / (float)this.tileCount) : 0f);
			this.soundEvent.setParameterByName("averageRadiation", this.averageRadiation, false);
		}

		// Token: 0x060077A2 RID: 30626 RVA: 0x002D4160 File Offset: 0x002D2360
		public void UpdateParameters(Vector3 emitter_position)
		{
			if (!this.soundEvent.isValid())
			{
				return;
			}
			Vector3 pos = new Vector3(emitter_position.x, emitter_position.y, 0f);
			this.soundEvent.set3DAttributes(pos.To3DAttributes());
			this.soundEvent.setParameterByName("tilePercentage", this.tilePercentage, false);
		}

		// Token: 0x060077A3 RID: 30627 RVA: 0x002D41BD File Offset: 0x002D23BD
		public int CompareTo(AmbienceManager.Layer layer)
		{
			return layer.tileCount - this.tileCount;
		}

		// Token: 0x060077A4 RID: 30628 RVA: 0x002D41CC File Offset: 0x002D23CC
		public void Stop()
		{
			if (this.soundEvent.isValid())
			{
				this.soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.soundEvent.release();
			}
			this.isRunning = false;
		}

		// Token: 0x060077A5 RID: 30629 RVA: 0x002D41FC File Offset: 0x002D23FC
		public void Start(Vector3 emitter_position)
		{
			if (!this.isRunning)
			{
				if (!this.oneShotSound.IsNull)
				{
					EventInstance eventInstance = KFMOD.CreateInstance(this.oneShotSound);
					if (!eventInstance.isValid())
					{
						string str = "Could not find event: ";
						EventReference eventReference = this.oneShotSound;
						global::Debug.LogWarning(str + eventReference.ToString());
						return;
					}
					ATTRIBUTES_3D attributes = new Vector3(emitter_position.x, emitter_position.y, 0f).To3DAttributes();
					eventInstance.set3DAttributes(attributes);
					eventInstance.setVolume(this.tilePercentage * 2f);
					eventInstance.start();
					eventInstance.release();
					return;
				}
				else
				{
					this.soundEvent = KFMOD.CreateInstance(this.sound);
					if (this.soundEvent.isValid())
					{
						this.soundEvent.start();
					}
					this.isRunning = true;
				}
			}
		}

		// Token: 0x04005A64 RID: 23140
		private const string TILE_PERCENTAGE_ID = "tilePercentage";

		// Token: 0x04005A65 RID: 23141
		private const string AVERAGE_TEMPERATURE_ID = "averageTemperature";

		// Token: 0x04005A66 RID: 23142
		private const string AVERAGE_RADIATION_ID = "averageRadiation";

		// Token: 0x04005A67 RID: 23143
		public EventReference sound;

		// Token: 0x04005A68 RID: 23144
		public EventReference oneShotSound;

		// Token: 0x04005A69 RID: 23145
		public int tileCount;

		// Token: 0x04005A6A RID: 23146
		public float tilePercentage;

		// Token: 0x04005A6B RID: 23147
		public float volume;

		// Token: 0x04005A6C RID: 23148
		public bool isRunning;

		// Token: 0x04005A6D RID: 23149
		private EventInstance soundEvent;

		// Token: 0x04005A6E RID: 23150
		public float averageTemperature;

		// Token: 0x04005A6F RID: 23151
		public float averageRadiation;
	}

	// Token: 0x020010E5 RID: 4325
	[Serializable]
	public class QuadrantDef
	{
		// Token: 0x04005A70 RID: 23152
		public string name;

		// Token: 0x04005A71 RID: 23153
		public EventReference[] liquidSounds;

		// Token: 0x04005A72 RID: 23154
		public EventReference[] gasSounds;

		// Token: 0x04005A73 RID: 23155
		public EventReference[] solidSounds;

		// Token: 0x04005A74 RID: 23156
		public EventReference fogSound;

		// Token: 0x04005A75 RID: 23157
		public EventReference spaceSound;

		// Token: 0x04005A76 RID: 23158
		public EventReference facilitySound;

		// Token: 0x04005A77 RID: 23159
		public EventReference radiationSound;
	}

	// Token: 0x020010E6 RID: 4326
	public class Quadrant
	{
		// Token: 0x060077A7 RID: 30631 RVA: 0x002D42E0 File Offset: 0x002D24E0
		public Quadrant(AmbienceManager.QuadrantDef def)
		{
			this.name = def.name;
			this.fogLayer = new AmbienceManager.Layer(def.fogSound, default(EventReference));
			this.allLayers.Add(this.fogLayer);
			this.loopingLayers.Add(this.fogLayer);
			this.spaceLayer = new AmbienceManager.Layer(def.spaceSound, default(EventReference));
			this.allLayers.Add(this.spaceLayer);
			this.loopingLayers.Add(this.spaceLayer);
			this.facilityLayer = new AmbienceManager.Layer(def.facilitySound, default(EventReference));
			this.allLayers.Add(this.facilityLayer);
			this.loopingLayers.Add(this.facilityLayer);
			this.m_isRadiationEnabled = Sim.IsRadiationEnabled();
			if (this.m_isRadiationEnabled)
			{
				this.radiationLayer = new AmbienceManager.Layer(def.radiationSound, default(EventReference));
				this.allLayers.Add(this.radiationLayer);
			}
			for (int i = 0; i < 4; i++)
			{
				this.gasLayers[i] = new AmbienceManager.Layer(def.gasSounds[i], default(EventReference));
				this.liquidLayers[i] = new AmbienceManager.Layer(def.liquidSounds[i], default(EventReference));
				this.allLayers.Add(this.gasLayers[i]);
				this.allLayers.Add(this.liquidLayers[i]);
				this.loopingLayers.Add(this.gasLayers[i]);
				this.loopingLayers.Add(this.liquidLayers[i]);
			}
			for (int j = 0; j < this.solidLayers.Length; j++)
			{
				if (j >= def.solidSounds.Length)
				{
					string str = "Missing solid layer: ";
					SolidAmbienceType solidAmbienceType = (SolidAmbienceType)j;
					global::Debug.LogError(str + solidAmbienceType.ToString());
				}
				this.solidLayers[j] = new AmbienceManager.Layer(default(EventReference), def.solidSounds[j]);
				this.allLayers.Add(this.solidLayers[j]);
				this.oneShotLayers.Add(this.solidLayers[j]);
			}
			this.solidTimers = new AmbienceManager.Quadrant.SolidTimer[AmbienceManager.Quadrant.activeSolidLayerCount];
			for (int k = 0; k < AmbienceManager.Quadrant.activeSolidLayerCount; k++)
			{
				this.solidTimers[k] = new AmbienceManager.Quadrant.SolidTimer();
			}
		}

		// Token: 0x060077A8 RID: 30632 RVA: 0x002D459C File Offset: 0x002D279C
		public void Update(Vector2I min, Vector2I max, Vector3 emitter_position)
		{
			this.emitterPosition = emitter_position;
			this.totalTileCount = 0;
			for (int i = 0; i < this.allLayers.Count; i++)
			{
				this.allLayers[i].Reset();
			}
			for (int j = min.y; j < max.y; j++)
			{
				if (j % 2 != 1)
				{
					for (int k = min.x; k < max.x; k++)
					{
						if (k % 2 != 0)
						{
							int num = Grid.XYToCell(k, j);
							if (Grid.IsValidCell(num))
							{
								this.totalTileCount++;
								if (Grid.IsVisible(num))
								{
									if (Grid.GravitasFacility[num])
									{
										this.facilityLayer.tileCount += 8;
									}
									else
									{
										Element element = Grid.Element[num];
										if (element != null)
										{
											if (element.IsLiquid && Grid.IsSubstantialLiquid(num, 0.35f))
											{
												AmbienceType ambience = element.substance.GetAmbience();
												if (ambience != AmbienceType.None)
												{
													this.liquidLayers[(int)ambience].tileCount++;
													this.liquidLayers[(int)ambience].averageTemperature += Grid.Temperature[num];
												}
											}
											else if (element.IsGas)
											{
												AmbienceType ambience2 = element.substance.GetAmbience();
												if (ambience2 != AmbienceType.None)
												{
													this.gasLayers[(int)ambience2].tileCount++;
													this.gasLayers[(int)ambience2].averageTemperature += Grid.Temperature[num];
												}
											}
											else if (element.IsSolid)
											{
												SolidAmbienceType solidAmbienceType = element.substance.GetSolidAmbience();
												if (Grid.Foundation[num])
												{
													solidAmbienceType = SolidAmbienceType.Tile;
													this.solidLayers[(int)solidAmbienceType].tileCount += TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
												}
												else if (Grid.Objects[num, 2] != null)
												{
													solidAmbienceType = SolidAmbienceType.Tile;
													this.solidLayers[(int)solidAmbienceType].tileCount += TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
												}
												else if (solidAmbienceType != SolidAmbienceType.None)
												{
													this.solidLayers[(int)solidAmbienceType].tileCount++;
												}
												else if (element.id == SimHashes.Regolith || element.id == SimHashes.MaficRock)
												{
													this.spaceLayer.tileCount++;
												}
											}
											else if (element.id == SimHashes.Vacuum && CellSelectionObject.IsExposedToSpace(num))
											{
												if (Grid.Objects[num, 1] != null)
												{
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().buildingTileValue;
												}
												this.spaceLayer.tileCount++;
											}
										}
									}
									if (Grid.Radiation[num] > 0f)
									{
										this.radiationLayer.averageRadiation += Grid.Radiation[num];
										this.radiationLayer.tileCount++;
									}
								}
								else
								{
									this.fogLayer.tileCount++;
								}
							}
						}
					}
				}
			}
			Vector2I vector2I = max - min;
			int cell_count = vector2I.x * vector2I.y;
			for (int l = 0; l < this.allLayers.Count; l++)
			{
				this.allLayers[l].UpdatePercentage(cell_count);
			}
			this.loopingLayers.Sort();
			this.topLayers.Clear();
			for (int m = 0; m < this.loopingLayers.Count; m++)
			{
				AmbienceManager.Layer layer = this.loopingLayers[m];
				if (m < 3 && layer.tilePercentage > 0f)
				{
					layer.Start(emitter_position);
					layer.UpdateAverageTemperature();
					layer.UpdateParameters(emitter_position);
					this.topLayers.Add(layer);
				}
				else
				{
					layer.Stop();
				}
			}
			if (this.m_isRadiationEnabled)
			{
				this.radiationLayer.Start(emitter_position);
				this.radiationLayer.UpdateAverageRadiation();
				this.radiationLayer.UpdateParameters(emitter_position);
			}
			this.oneShotLayers.Sort();
			for (int n = 0; n < AmbienceManager.Quadrant.activeSolidLayerCount; n++)
			{
				if (this.solidTimers[n].ShouldPlay() && this.oneShotLayers[n].tilePercentage > 0f)
				{
					this.oneShotLayers[n].Start(emitter_position);
				}
			}
		}

		// Token: 0x060077A9 RID: 30633 RVA: 0x002D4A63 File Offset: 0x002D2C63
		public List<AmbienceManager.Layer> GetAllLayers()
		{
			return this.allLayers;
		}

		// Token: 0x04005A78 RID: 23160
		public string name;

		// Token: 0x04005A79 RID: 23161
		public Vector3 emitterPosition;

		// Token: 0x04005A7A RID: 23162
		public AmbienceManager.Layer[] gasLayers = new AmbienceManager.Layer[4];

		// Token: 0x04005A7B RID: 23163
		public AmbienceManager.Layer[] liquidLayers = new AmbienceManager.Layer[4];

		// Token: 0x04005A7C RID: 23164
		public AmbienceManager.Layer fogLayer;

		// Token: 0x04005A7D RID: 23165
		public AmbienceManager.Layer spaceLayer;

		// Token: 0x04005A7E RID: 23166
		public AmbienceManager.Layer facilityLayer;

		// Token: 0x04005A7F RID: 23167
		public AmbienceManager.Layer radiationLayer;

		// Token: 0x04005A80 RID: 23168
		public AmbienceManager.Layer[] solidLayers = new AmbienceManager.Layer[16];

		// Token: 0x04005A81 RID: 23169
		private List<AmbienceManager.Layer> allLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04005A82 RID: 23170
		private List<AmbienceManager.Layer> loopingLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04005A83 RID: 23171
		private List<AmbienceManager.Layer> oneShotLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04005A84 RID: 23172
		private List<AmbienceManager.Layer> topLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04005A85 RID: 23173
		public static int activeSolidLayerCount = 2;

		// Token: 0x04005A86 RID: 23174
		public int totalTileCount;

		// Token: 0x04005A87 RID: 23175
		private bool m_isRadiationEnabled;

		// Token: 0x04005A88 RID: 23176
		private AmbienceManager.Quadrant.SolidTimer[] solidTimers;

		// Token: 0x0200208B RID: 8331
		public class SolidTimer
		{
			// Token: 0x0600A5FF RID: 42495 RVA: 0x0036D351 File Offset: 0x0036B551
			public SolidTimer()
			{
				this.solidTargetTime = Time.unscaledTime + UnityEngine.Random.value * AmbienceManager.Quadrant.SolidTimer.solidMinTime;
			}

			// Token: 0x0600A600 RID: 42496 RVA: 0x0036D370 File Offset: 0x0036B570
			public bool ShouldPlay()
			{
				if (Time.unscaledTime > this.solidTargetTime)
				{
					this.solidTargetTime = Time.unscaledTime + AmbienceManager.Quadrant.SolidTimer.solidMinTime + UnityEngine.Random.value * (AmbienceManager.Quadrant.SolidTimer.solidMaxTime - AmbienceManager.Quadrant.SolidTimer.solidMinTime);
					return true;
				}
				return false;
			}

			// Token: 0x0400917B RID: 37243
			public static float solidMinTime = 9f;

			// Token: 0x0400917C RID: 37244
			public static float solidMaxTime = 15f;

			// Token: 0x0400917D RID: 37245
			public float solidTargetTime;
		}
	}
}
