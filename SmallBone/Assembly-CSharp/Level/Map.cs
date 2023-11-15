using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using PhysicsUtils;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Serialization;

namespace Level
{
	// Token: 0x020004F8 RID: 1272
	public class Map : MonoBehaviour
	{
		// Token: 0x060018F8 RID: 6392 RVA: 0x0004E476 File Offset: 0x0004C676
		static Map()
		{
			Map._rewardActivatingRangeFinder.contactFilter.SetLayerMask(512);
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x0004E49C File Offset: 0x0004C69C
		// (set) Token: 0x060018FA RID: 6394 RVA: 0x0004E4A3 File Offset: 0x0004C6A3
		public static Map Instance { get; private set; }

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x0004E4AB File Offset: 0x0004C6AB
		public Map.Type type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060018FC RID: 6396 RVA: 0x0004E4B3 File Offset: 0x0004C6B3
		public ParallaxBackground background
		{
			get
			{
				return this._background;
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x0004E4BB File Offset: 0x0004C6BB
		// (set) Token: 0x060018FE RID: 6398 RVA: 0x0004E4C3 File Offset: 0x0004C6C3
		public EnemyWaveContainer waveContainer { get; private set; }

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060018FF RID: 6399 RVA: 0x0004E4CC File Offset: 0x0004C6CC
		// (set) Token: 0x06001900 RID: 6400 RVA: 0x0004E4D4 File Offset: 0x0004C6D4
		public Cage cage { get; private set; }

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x0004E4DD File Offset: 0x0004C6DD
		public Light2D globalLight
		{
			get
			{
				return this._globalLight;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001902 RID: 6402 RVA: 0x0004E4E5 File Offset: 0x0004C6E5
		// (set) Token: 0x06001903 RID: 6403 RVA: 0x0004E4ED File Offset: 0x0004C6ED
		public Color originalLightColor { get; private set; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x0004E4F6 File Offset: 0x0004C6F6
		// (set) Token: 0x06001905 RID: 6405 RVA: 0x0004E4FE File Offset: 0x0004C6FE
		public float originalLightIntensity { get; private set; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001906 RID: 6406 RVA: 0x0004E507 File Offset: 0x0004C707
		// (set) Token: 0x06001907 RID: 6407 RVA: 0x0004E50F File Offset: 0x0004C70F
		public CameraZone cameraZone
		{
			get
			{
				return this._cameraZone;
			}
			set
			{
				this._cameraZone = value;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001908 RID: 6408 RVA: 0x0004E518 File Offset: 0x0004C718
		// (set) Token: 0x06001909 RID: 6409 RVA: 0x0004E520 File Offset: 0x0004C720
		public Bounds bounds { get; private set; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x0600190A RID: 6410 RVA: 0x0004E529 File Offset: 0x0004C729
		public Vector3 playerOrigin
		{
			get
			{
				return this._playerOrigin.transform.position;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x0600190B RID: 6411 RVA: 0x0004E53B File Offset: 0x0004C73B
		public Vector3 backgroundOrigin
		{
			get
			{
				return this._backgroundOrigin.transform.position;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x0600190C RID: 6412 RVA: 0x0004E54D File Offset: 0x0004C74D
		public bool pauseTimer
		{
			get
			{
				return this._pauseTimer;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x0600190D RID: 6413 RVA: 0x0004E555 File Offset: 0x0004C755
		public bool displayStageName
		{
			get
			{
				return this._displayStageName;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x0600190E RID: 6414 RVA: 0x0004E55D File Offset: 0x0004C75D
		// (set) Token: 0x0600190F RID: 6415 RVA: 0x0004E565 File Offset: 0x0004C765
		public bool darkEnemy { get; set; }

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001910 RID: 6416 RVA: 0x0004E56E File Offset: 0x0004C76E
		public MapReward mapReward
		{
			get
			{
				return this._mapReward;
			}
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x0004E578 File Offset: 0x0004C778
		private void Awake()
		{
			Map.Instance = this;
			TilemapBaker tilemapBaker;
			this.<Awake>g__FindRequiredComponents|72_0(out tilemapBaker);
			tilemapBaker.Bake();
			this.bounds = tilemapBaker.bounds;
			this.<Awake>g__InitializeGates|72_1();
			if (this.waveContainer != null)
			{
				this.waveContainer.Initialize();
				if (Map.TestingTool.safeZone)
				{
					this.waveContainer.HideAll();
				}
			}
			this.SetCameraZoneOrDefault();
			this.<Awake>g__MakeBorders|72_2();
			this.originalLightColor = this._globalLight.color;
			this.originalLightIntensity = this._globalLight.intensity;
			UIManager uiManager = Scene<GameBase>.instance.uiManager;
			uiManager.headupDisplay.minimapVisible = !this._hideMinimap;
			uiManager.scaler.SetVerticalLetterBox(this._verticalLetterBox);
			base.StartCoroutine(this.CCheckRewardActivating());
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x0004E640 File Offset: 0x0004C840
		public void SetReward(MapReward.Type rewardType)
		{
			this._mapReward.type = rewardType;
			Sprite sprite = null;
			if (this._mapReward.hasReward)
			{
				Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
				sprite = ((rewardType == MapReward.Type.Adventurer) ? currentChapter.gateChoiceTable : currentChapter.gateTable);
			}
			this._gateTable.sprite = sprite;
			this._mapReward.LoadReward();
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x0004E6A4 File Offset: 0x0004C8A4
		public void SetExits(PathNode node1, PathNode node2)
		{
			if (this._gate1 == null || this._gate2 == null)
			{
				return;
			}
			this._gate1.GetComponent<SpriteRenderer>().sortingOrder = -1;
			this._gate2.GetComponent<SpriteRenderer>().sortingOrder = -2;
			if (node1.gate == node2.gate && node1.gate != Gate.Type.Boss)
			{
				if (MMMaths.RandomBool())
				{
					this._gate1.Set(node1.gate, NodeIndex.Node1);
					this._gate2.ShowDestroyed(node1.gate == Gate.Type.Terminal);
					return;
				}
				this._gate1.ShowDestroyed(node1.gate == Gate.Type.Terminal);
				this._gate2.Set(node2.gate, NodeIndex.Node2);
				return;
			}
			else
			{
				if (node1.gate == Gate.Type.None)
				{
					this._gate1.ShowDestroyed(node1.gate == Gate.Type.Terminal);
				}
				else
				{
					this._gate1.Set(node1.gate, NodeIndex.Node1);
				}
				if (node2.gate == Gate.Type.None)
				{
					this._gate2.ShowDestroyed(node2.gate == Gate.Type.Terminal);
					return;
				}
				this._gate2.Set(node2.gate, NodeIndex.Node2);
				return;
			}
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x0004E7BC File Offset: 0x0004C9BC
		public bool IsInMap(Vector3 position)
		{
			Vector3 min = this.bounds.min;
			Vector3 max = this.bounds.max;
			bool flag = position.x > min.x && position.x < max.x;
			bool flag2 = position.y > min.y && ((this._hasCeil && position.y < max.y + this._ceilHeight) || !this._hasCeil);
			return flag && flag2;
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x0004E843 File Offset: 0x0004CA43
		public void ChangeLight(Color color, float intensity, float seconds)
		{
			this._lightLerpReference.Stop();
			this._lightLerpReference = this.StartCoroutineWithReference(this.CLerp(this._globalLight.color, this._globalLight.intensity, color, intensity, seconds));
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x0004E87C File Offset: 0x0004CA7C
		public void SetCameraZoneOrDefault()
		{
			GameBase instance = Scene<GameBase>.instance;
			if (this._cameraZone == null)
			{
				this._cameraZone = instance.cameraController.gameObject.AddComponent<CameraZone>();
				this._cameraZone.bounds = this.bounds;
				this._cameraZone.hasCeil = this._hasCeil;
				Vector3 max = this._cameraZone.bounds.max;
				max.y += this._ceilHeight;
				this._cameraZone.bounds.max = max;
				instance.cameraController.zone = this._cameraZone;
				instance.minimapCameraController.zone = this._cameraZone;
			}
			instance.cameraController.zone = this._cameraZone;
			instance.minimapCameraController.zone = this._cameraZone;
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x0004E950 File Offset: 0x0004CB50
		public void ResetCameraZone()
		{
			GameBase instance = Scene<GameBase>.instance;
			if (instance.cameraController.gameObject.GetComponent<CameraZone>() == null)
			{
				this._cameraZone = instance.cameraController.gameObject.AddComponent<CameraZone>();
			}
			this._cameraZone.bounds = this.bounds;
			this._cameraZone.hasCeil = this._hasCeil;
			Vector3 max = this._cameraZone.bounds.max;
			max.y += this._ceilHeight;
			this._cameraZone.bounds.max = max;
			instance.cameraController.zone = this._cameraZone;
			instance.minimapCameraController.zone = this._cameraZone;
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x0004EA08 File Offset: 0x0004CC08
		public void RestoreLight(float seconds)
		{
			this._lightLerpReference.Stop();
			this._lightLerpReference = this.StartCoroutineWithReference(this.CLerp(this.globalLight.color, this.globalLight.intensity, this.originalLightColor, this.originalLightIntensity, seconds));
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0004EA55 File Offset: 0x0004CC55
		private IEnumerator CLerp(Color colorA, float intensityA, Color colorB, float intensityB, float seconds)
		{
			for (float t = 0f; t < 1f; t += Chronometer.global.deltaTime / seconds)
			{
				yield return null;
				this.globalLight.color = Color.Lerp(colorA, colorB, t);
				this.globalLight.intensity = Mathf.Lerp(intensityA, intensityB, t);
			}
			yield break;
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0004EA89 File Offset: 0x0004CC89
		private IEnumerator CCheckRewardActivating()
		{
			yield return null;
			while (this.waveContainer.enemyWaves.Length != 0 && (this.waveContainer.state == EnemyWaveContainer.State.Remain || Map._rewardActivatingRangeFinder.OverlapCollider(this._mapRewardActivatingRange).GetComponent<Target>() == null))
			{
				yield return null;
			}
			this.waveContainer.Stop();
			if (this._mapReward.Activate())
			{
				this._mapReward.onLoot += this.<CCheckRewardActivating>g__ActivateGates|81_0;
				Singleton<Service>.Instance.levelManager.InvokeOnActivateMapReward();
			}
			else
			{
				this.<CCheckRewardActivating>g__ActivateGates|81_0();
			}
			yield break;
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0004EAAC File Offset: 0x0004CCAC
		[CompilerGenerated]
		private void <Awake>g__FindRequiredComponents|72_0(out TilemapBaker tilemapBaker)
		{
			tilemapBaker = null;
			this.waveContainer = null;
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				EnemyWaveContainer component = transform.GetComponent<EnemyWaveContainer>();
				if (component != null)
				{
					this.waveContainer = component;
				}
				else
				{
					TilemapBaker component2 = transform.GetComponent<TilemapBaker>();
					if (component2 != null)
					{
						tilemapBaker = component2;
					}
					else
					{
						Cage component3 = transform.GetComponent<Cage>();
						if (component3 != null)
						{
							this.cage = component3;
						}
					}
				}
			}
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0004EB54 File Offset: 0x0004CD54
		[CompilerGenerated]
		private void <Awake>g__InitializeGates|72_1()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._gateWall.sprite = currentChapter.gateWall;
			Gate gatePrefab = currentChapter.gatePrefab;
			if (gatePrefab != null)
			{
				this._gate1 = UnityEngine.Object.Instantiate<Gate>(gatePrefab, this._gate1Position.position, Quaternion.identity, base.transform);
				this._gate2 = UnityEngine.Object.Instantiate<Gate>(gatePrefab, this._gate2Position.position, Quaternion.identity, base.transform);
			}
			UnityEngine.Object.Destroy(this._gate1Position.gameObject);
			UnityEngine.Object.Destroy(this._gate2Position.gameObject);
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x0004EBF8 File Offset: 0x0004CDF8
		[CompilerGenerated]
		private void <Awake>g__MakeBorders|72_2()
		{
			Vector2 vector = default(Vector2);
			Vector2 offset = default(Vector2);
			int num = 20;
			BoxCollider2D boxCollider2D = base.gameObject.AddComponent<BoxCollider2D>();
			vector.x = (float)num;
			vector.y = this.bounds.size.y + 50f;
			offset.x = this.bounds.min.x - vector.x * 0.5f;
			offset.y = this.bounds.min.y + vector.y * 0.5f;
			boxCollider2D.size = vector;
			boxCollider2D.offset = offset;
			BoxCollider2D boxCollider2D2 = base.gameObject.AddComponent<BoxCollider2D>();
			vector.x = (float)num;
			vector.y = this.bounds.size.y + 50f;
			offset.x = this.bounds.max.x + vector.x * 0.5f;
			offset.y = this.bounds.min.y + vector.y * 0.5f;
			boxCollider2D2.size = vector;
			boxCollider2D2.offset = offset;
			if (this.cameraZone.hasCeil)
			{
				BoxCollider2D boxCollider2D3 = base.gameObject.AddComponent<BoxCollider2D>();
				vector.x = this.bounds.size.x + (float)(num * 2);
				vector.y = (float)num;
				offset.x = this.bounds.center.x;
				offset.y = this.bounds.max.y + this._ceilHeight + vector.y * 0.5f;
				boxCollider2D3.size = vector;
				boxCollider2D3.offset = offset;
			}
			BoxCollider2D boxCollider2D4 = base.gameObject.AddComponent<BoxCollider2D>();
			vector.x = this.bounds.size.x + (float)(num * 2);
			vector.y = (float)num;
			offset.x = this.bounds.center.x;
			offset.y = this.bounds.min.y - vector.y * 0.5f;
			boxCollider2D4.size = vector;
			boxCollider2D4.offset = offset;
			base.gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x0004EE60 File Offset: 0x0004D060
		[CompilerGenerated]
		private void <CCheckRewardActivating>g__ActivateGates|81_0()
		{
			if (this._gate1 != null)
			{
				this._gate1.Activate();
			}
			if (this._gate2 != null)
			{
				this._gate2.Activate();
			}
		}

		// Token: 0x040015C7 RID: 5575
		private static readonly NonAllocOverlapper _rewardActivatingRangeFinder = new NonAllocOverlapper(1);

		// Token: 0x040015C9 RID: 5577
		[Space]
		[SerializeField]
		private Map.Type _type;

		// Token: 0x040015CA RID: 5578
		[SerializeField]
		private ParallaxBackground _background;

		// Token: 0x040015CB RID: 5579
		[SerializeField]
		private bool _pauseTimer;

		// Token: 0x040015CC RID: 5580
		[SerializeField]
		private bool _displayStageName;

		// Token: 0x040015CD RID: 5581
		[SerializeField]
		[FormerlySerializedAs("_hideRightBottomHud")]
		private bool _hideMinimap;

		// Token: 0x040015CE RID: 5582
		[SerializeField]
		private bool _verticalLetterBox;

		// Token: 0x040015CF RID: 5583
		[SerializeField]
		private CameraZone _cameraZone;

		// Token: 0x040015D0 RID: 5584
		[SerializeField]
		private Light2D _globalLight;

		// Token: 0x040015D1 RID: 5585
		[Space]
		[SerializeField]
		private Transform _playerOrigin;

		// Token: 0x040015D2 RID: 5586
		[SerializeField]
		private Transform _backgroundOrigin;

		// Token: 0x040015D3 RID: 5587
		[Space]
		[SerializeField]
		private bool _hasCeil;

		// Token: 0x040015D4 RID: 5588
		[SerializeField]
		private float _ceilHeight = 10f;

		// Token: 0x040015D5 RID: 5589
		[SerializeField]
		[Header("Gate")]
		private SpriteRenderer _gateWall;

		// Token: 0x040015D6 RID: 5590
		[SerializeField]
		private SpriteRenderer _gateTable;

		// Token: 0x040015D7 RID: 5591
		[SerializeField]
		private Transform _gate1Position;

		// Token: 0x040015D8 RID: 5592
		[SerializeField]
		private Transform _gate2Position;

		// Token: 0x040015D9 RID: 5593
		[SerializeField]
		[Header("Reward")]
		private MapReward _mapReward;

		// Token: 0x040015DA RID: 5594
		[SerializeField]
		private Collider2D _mapRewardActivatingRange;

		// Token: 0x040015DB RID: 5595
		private CoroutineReference _lightLerpReference;

		// Token: 0x040015DC RID: 5596
		private Gate _gate1;

		// Token: 0x040015DD RID: 5597
		private Gate _gate2;

		// Token: 0x020004F9 RID: 1273
		public static class TestingTool
		{
			// Token: 0x1700050E RID: 1294
			// (get) Token: 0x06001920 RID: 6432 RVA: 0x0004EE94 File Offset: 0x0004D094
			// (set) Token: 0x06001921 RID: 6433 RVA: 0x0004EE9B File Offset: 0x0004D09B
			public static bool safeZone { get; set; } = false;

			// Token: 0x1700050F RID: 1295
			// (get) Token: 0x06001922 RID: 6434 RVA: 0x0004EEA3 File Offset: 0x0004D0A3
			// (set) Token: 0x06001923 RID: 6435 RVA: 0x0004EEAA File Offset: 0x0004D0AA
			public static bool fieldNPC { get; set; } = true;

			// Token: 0x17000510 RID: 1296
			// (get) Token: 0x06001924 RID: 6436 RVA: 0x0004EEB2 File Offset: 0x0004D0B2
			// (set) Token: 0x06001925 RID: 6437 RVA: 0x0004EEB9 File Offset: 0x0004D0B9
			public static bool darkenemy { get; set; } = true;
		}

		// Token: 0x020004FA RID: 1274
		public enum Type
		{
			// Token: 0x040015E8 RID: 5608
			Normal,
			// Token: 0x040015E9 RID: 5609
			Npc,
			// Token: 0x040015EA RID: 5610
			Manual,
			// Token: 0x040015EB RID: 5611
			Special
		}
	}
}
