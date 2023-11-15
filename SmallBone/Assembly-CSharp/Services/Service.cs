using System;
using Data;
using Level;
using Platforms;
using Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
	// Token: 0x0200013C RID: 316
	public sealed class Service : Singleton<Service>
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x00012A8D File Offset: 0x00010C8D
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x00012A94 File Offset: 0x00010C94
		public static bool quitting { get; private set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00012A9C File Offset: 0x00010C9C
		public ControllerManager controllerVibation
		{
			get
			{
				return this._controllerVibration;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00012AA4 File Offset: 0x00010CA4
		public LevelManager levelManager
		{
			get
			{
				return this._levelManager;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00012AAC File Offset: 0x00010CAC
		public GearManager gearManager
		{
			get
			{
				return this._gearManager;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00012AB4 File Offset: 0x00010CB4
		public FloatingTextSpawner floatingTextSpawner
		{
			get
			{
				return this._floatingTextSpawner;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00012ABC File Offset: 0x00010CBC
		public LineTextManager lineTextManager
		{
			get
			{
				return this._lineTextManager;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00012AC4 File Offset: 0x00010CC4
		public FadeInOut fadeInOut
		{
			get
			{
				return this._fadeInOut;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00012ACC File Offset: 0x00010CCC
		protected override void Awake()
		{
			base.Awake();
			GameData.Initialize();
			Application.quitting += delegate()
			{
				Service.quitting = true;
			};
			QualitySettings.vSyncCount = 1;
			Physics2D.autoSyncTransforms = false;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00012B09 File Offset: 0x00010D09
		private void Update()
		{
			Physics2D.SyncTransforms();
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00012B10 File Offset: 0x00010D10
		public void ResetGameScene()
		{
			SceneManager.LoadScene("Main");
			this.levelManager.ClearEvents();
			GameData.Initialize();
			GameData.Save.instance.ResetAll();
			PoolObject.DespawnAll();
			PoolObject.Clear();
			PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00012B09 File Offset: 0x00010D09
		private void LateUpdate()
		{
			Physics2D.SyncTransforms();
		}

		// Token: 0x040004A9 RID: 1193
		[SerializeField]
		[GetComponent]
		private ControllerManager _controllerVibration;

		// Token: 0x040004AA RID: 1194
		[SerializeField]
		[GetComponent]
		private LevelManager _levelManager;

		// Token: 0x040004AB RID: 1195
		[SerializeField]
		[GetComponent]
		private GearManager _gearManager;

		// Token: 0x040004AC RID: 1196
		[SerializeField]
		[GetComponent]
		private FloatingTextSpawner _floatingTextSpawner;

		// Token: 0x040004AD RID: 1197
		[GetComponent]
		[SerializeField]
		private LineTextManager _lineTextManager;

		// Token: 0x040004AE RID: 1198
		[SerializeField]
		[GetComponent]
		private FadeInOut _fadeInOut;
	}
}
