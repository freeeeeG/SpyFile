using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x0200015A RID: 346
	public sealed class DarktechManager : Singleton<DarktechManager>
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00013CAA File Offset: 0x00011EAA
		public DarktechManager.DarktechByLevel[] darkTechByLevels
		{
			get
			{
				return this._darkTechByLevels;
			}
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00013CB4 File Offset: 0x00011EB4
		protected override void Awake()
		{
			base.Awake();
			this._storage = new DarktechDataStorage();
			if (this._resource == null)
			{
				this._resource = Resources.Load<DarktechResource>("HardmodeSetting/DarktechResource");
			}
			if (this._setting == null)
			{
				this._setting = Resources.Load<DarktechSetting>("HardmodeSetting/DarktechSetting");
			}
			this._setting.Initialize();
			Singleton<Service>.Instance.levelManager.onChapterLoaded += this.TryUnlock;
			this.TryUnlock();
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x00013D3A File Offset: 0x00011F3A
		public DarktechResource resource
		{
			get
			{
				return this._resource;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00013D42 File Offset: 0x00011F42
		public DarktechSetting setting
		{
			get
			{
				return this._setting;
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00013D4C File Offset: 0x00011F4C
		private void TryUnlock()
		{
			if (Singleton<Service>.Instance.levelManager.currentChapter.type != Chapter.Type.HardmodeCastle)
			{
				return;
			}
			int clearedLevel = GameData.HardmodeProgress.clearedLevel;
			for (int i = 0; i < this._darkTechByLevels.Length; i++)
			{
				if (i <= clearedLevel - 1)
				{
					foreach (DarktechData.Type type in this._darkTechByLevels[i].types)
					{
						this._storage.Activate(type);
					}
				}
				if (i > clearedLevel)
				{
					foreach (DarktechData.Type type2 in this._darkTechByLevels[i].types)
					{
						this._storage.Lock(type2);
					}
				}
				else
				{
					foreach (DarktechData.Type type3 in this._darkTechByLevels[i].types)
					{
						this.UnlockDarktech(type3);
					}
				}
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00013E2D File Offset: 0x0001202D
		public bool IsUnlocked(DarktechData data)
		{
			return this._storage.IsUnlocked(data.type);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00013E40 File Offset: 0x00012040
		public bool IsUnlocked(DarktechData.Type type)
		{
			return this._storage.IsUnlocked(type);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00013E4E File Offset: 0x0001204E
		public bool IsActivated(DarktechData data)
		{
			return this._storage.IsActivated(data.type);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00013E61 File Offset: 0x00012061
		public bool IsActivated(DarktechData.Type type)
		{
			return this._storage.IsActivated(type);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00013E6F File Offset: 0x0001206F
		public void ActivateDarktech(DarktechData.Type type)
		{
			this._storage.Activate(type);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00013E7D File Offset: 0x0001207D
		public void UnlockDarktech(DarktechData.Type type)
		{
			this._resource.Find(type);
			this._storage.Unlock(type);
		}

		// Token: 0x0400050C RID: 1292
		private DarktechManager.DarktechByLevel[] _darkTechByLevels = new DarktechManager.DarktechByLevel[]
		{
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.SkullManufacturingMachine,
					DarktechData.Type.SuppliesManufacturingMachine
				}
			},
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.OmenAmplifier
				}
			},
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.ItemRotationEquipment
				}
			},
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.HopeExtractor
				}
			},
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.HealthAuxiliaryEquipment
				}
			},
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.LuckyMeasuringInstrument
				}
			},
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.InscriptionSynthesisEquipment
				}
			},
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.NextGenerationHopeExtractor
				}
			},
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.BoneParticleMagnetoscope,
					DarktechData.Type.GoldenCalculator
				}
			},
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.AnxietyAccelerator
				}
			},
			new DarktechManager.DarktechByLevel
			{
				types = new DarktechData.Type[]
				{
					DarktechData.Type.ObservationInstrument
				}
			}
		};

		// Token: 0x0400050D RID: 1293
		private DarktechSetting _setting;

		// Token: 0x0400050E RID: 1294
		private DarktechDataStorage _storage;

		// Token: 0x0400050F RID: 1295
		private DarktechResource _resource;

		// Token: 0x0200015B RID: 347
		public struct DarktechByLevel
		{
			// Token: 0x04000510 RID: 1296
			public DarktechData.Type[] types;
		}
	}
}
