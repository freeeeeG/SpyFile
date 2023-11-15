using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009F5 RID: 2549
[RequireComponent(typeof(MapAvatarGroundCast), typeof(MapAvatarTransformer))]
public class MapAvatarControls : MonoBehaviour
{
	// Token: 0x17000379 RID: 889
	// (get) Token: 0x060031B9 RID: 12729 RVA: 0x000E8DEB File Offset: 0x000E71EB
	public GridManager GridManager
	{
		get
		{
			return this.m_gridManager;
		}
	}

	// Token: 0x060031BA RID: 12730 RVA: 0x000E8DF4 File Offset: 0x000E71F4
	private void Start()
	{
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
		this.m_VanMeshTransform = this.m_vanMesh.transform;
		this.m_Transform = base.transform;
		this.m_PreviousPosition = this.m_Transform.position;
		this.m_previousParent = this.m_Transform.parent;
	}

	// Token: 0x060031BB RID: 12731 RVA: 0x000E8E51 File Offset: 0x000E7251
	public float GetUnclampedMovementSpeed()
	{
		return this.m_UnclampedMovementSpeed;
	}

	// Token: 0x060031BC RID: 12732 RVA: 0x000E8E5C File Offset: 0x000E725C
	public bool CalculateCurrentSelectable<T>(GridManager gridManager, MapAvatarGroundCast groundCast, out T selectable) where T : class
	{
		if (groundCast.HasGroundContact())
		{
			GridIndex gridLocationFromPos = gridManager.GetGridLocationFromPos(base.transform.position);
			GameObject gridOccupant = gridManager.GetGridOccupant(gridLocationFromPos);
			if (gridOccupant != null)
			{
				GridIndex unclampedGridLocationFromPos = gridManager.GetUnclampedGridLocationFromPos(base.transform.position);
				GridIndex unclampedGridLocationFromPos2 = gridManager.GetUnclampedGridLocationFromPos(gridOccupant.transform.position);
				if (unclampedGridLocationFromPos == unclampedGridLocationFromPos2)
				{
					selectable = gridOccupant.RequestInterfaceRecursive<T>();
					if (selectable == null)
					{
						SelectableRedirect selectableRedirect = gridOccupant.RequestComponentRecursive<SelectableRedirect>();
						if (selectableRedirect != null)
						{
							selectable = ((!(selectableRedirect.m_target != null)) ? ((T)((object)null)) : selectableRedirect.m_target.gameObject.RequestInterfaceRecursive<T>());
						}
					}
					return true;
				}
			}
		}
		selectable = (T)((object)null);
		return false;
	}

	// Token: 0x060031BD RID: 12733 RVA: 0x000E8F3E File Offset: 0x000E733E
	public void SetOriginalVanRotation(Quaternion _originalRotation)
	{
		this.m_originalVanRotation = _originalRotation;
	}

	// Token: 0x060031BE RID: 12734 RVA: 0x000E8F48 File Offset: 0x000E7348
	public void OrientateVan(float _deltaTime, Vector3 _groundNormal)
	{
		Vector3 normalized = Vector3.Cross(_groundNormal, this.m_Transform.forward).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, _groundNormal).normalized;
		Quaternion to = Quaternion.LookRotation(normalized2, _groundNormal) * this.m_originalVanRotation;
		Quaternion rotation = Quaternion.RotateTowards(this.m_VanMeshTransform.rotation, to, this.m_rotateSmoothing * _deltaTime);
		this.m_VanMeshTransform.rotation = rotation;
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x000E8FC0 File Offset: 0x000E73C0
	public void UpdateMovement()
	{
		if (this.m_previousParent != this.m_Transform.parent)
		{
			this.m_gridManager = GameUtils.GetGridManager(this.m_Transform);
			this.m_previousParent = this.m_Transform.parent;
		}
		Vector3 position = this.m_Transform.position;
		this.m_Velocity = position - this.m_PreviousPosition;
		this.m_PreviousPosition = position;
		this.CalculateSpeed();
		this.m_Velocity = this.m_Velocity.normalized * this.m_Speed;
		Vector3 velocity = this.m_Velocity;
		this.m_UnclampedMovementSpeed = (velocity - Vector3.Dot(velocity, Vector3.up) * Vector3.up).magnitude / this.m_movementSpeed;
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x000E9088 File Offset: 0x000E7488
	public Vector3 GetVelocity()
	{
		return this.m_Velocity;
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x000E9090 File Offset: 0x000E7490
	public void DashStarted()
	{
		this.m_DashTimer = this.m_dashTime;
	}

	// Token: 0x060031C2 RID: 12738 RVA: 0x000E909E File Offset: 0x000E749E
	public float GetSpeed()
	{
		return this.m_Speed;
	}

	// Token: 0x060031C3 RID: 12739 RVA: 0x000E90A8 File Offset: 0x000E74A8
	private void CalculateSpeed()
	{
		if (this.m_Velocity.magnitude > 0.001f)
		{
			this.m_Speed = this.m_movementSpeed;
		}
		else
		{
			this.m_Speed = 0f;
		}
		if (this.m_DashTimer > 0f)
		{
			float num = MathUtils.SinusoidalSCurve(this.m_DashTimer / this.m_dashTime);
			this.m_Speed = (1f - num) * this.m_Speed + num * this.m_dashSpeed;
		}
		this.m_DashTimer -= TimeManager.GetDeltaTime(base.gameObject);
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x000E9140 File Offset: 0x000E7540
	public IEnumerator DebugAutoLoadRandomLevel()
	{
		int lastLevelIndex = GameUtils.GetGameSession().Progress.SaveData.LastLevelEntered;
		yield return new WaitForSecondsRealtime(2f);
		LevelPortalMapNode[] allNodes = GameObjectUtils.FindComponentsOfTypeInScene<LevelPortalMapNode>();
		LevelPortalMapNode[] newNodes = allNodes.AllRemoved_Predicate((LevelPortalMapNode x) => x.LevelIndex == lastLevelIndex);
		newNodes = newNodes.AllRemoved_Predicate((LevelPortalMapNode x) => !x.m_sceneDirectoryEntry.HasScoreBoundaries);
		LevelPortalMapNode node = null;
		if (newNodes.Length > 0)
		{
			node = newNodes[UnityEngine.Random.Range(0, newNodes.Length)];
		}
		else
		{
			int num = allNodes.FindIndex_Predicate((LevelPortalMapNode x) => x.LevelIndex == lastLevelIndex);
			node = allNodes[num];
		}
		if (node != null)
		{
			base.gameObject.transform.position = node.transform.position;
			yield return new WaitForSecondsRealtime(2f);
			ServerWorldMapFlowController serverFlow = node.m_worldMapFlowController.GetComponent<ServerWorldMapFlowController>();
			if (serverFlow != null)
			{
				serverFlow.OnSelectLevelPortal(this, node);
			}
		}
		yield break;
	}

	// Token: 0x040027EA RID: 10218
	[SerializeField]
	[AssignChild("Thomas", Editorbility.NonEditable)]
	public GameObject m_vanMesh;

	// Token: 0x040027EB RID: 10219
	[SerializeField]
	[AssignComponentRecursive(Editorbility.NonEditable)]
	public Animator m_animator;

	// Token: 0x040027EC RID: 10220
	[SerializeField]
	public float m_movementSpeed = 4f;

	// Token: 0x040027ED RID: 10221
	[SerializeField]
	public float m_gravityStrength = 1f;

	// Token: 0x040027EE RID: 10222
	[SerializeField]
	public float m_rotateSmoothing = 1f;

	// Token: 0x040027EF RID: 10223
	[SerializeField]
	public float m_turningCircle = 0.5f;

	// Token: 0x040027F0 RID: 10224
	[SerializeField]
	public float m_dashTime = 0.3f;

	// Token: 0x040027F1 RID: 10225
	[SerializeField]
	public float m_dashCooldown = 0.3f;

	// Token: 0x040027F2 RID: 10226
	[SerializeField]
	public float m_dashSpeed = 12f;

	// Token: 0x040027F3 RID: 10227
	[SerializeField]
	public GameObject m_dashPFXPrefab;

	// Token: 0x040027F4 RID: 10228
	[SerializeField]
	public MapAvatarControls.MapAvatarRevTags m_dashTags;

	// Token: 0x040027F5 RID: 10229
	[SerializeField]
	public float m_selectionRadius = 4f;

	// Token: 0x040027F6 RID: 10230
	[SerializeField]
	public LayerMask m_selectionLayerMask = -1;

	// Token: 0x040027F7 RID: 10231
	[SerializeField]
	public bool m_bStartOnFirstNode = true;

	// Token: 0x040027F8 RID: 10232
	private GridManager m_gridManager;

	// Token: 0x040027F9 RID: 10233
	private Transform m_previousParent;

	// Token: 0x040027FA RID: 10234
	private Transform m_VanMeshTransform;

	// Token: 0x040027FB RID: 10235
	private Transform m_Transform;

	// Token: 0x040027FC RID: 10236
	private Quaternion m_originalVanRotation = default(Quaternion);

	// Token: 0x040027FD RID: 10237
	private Vector3 m_PreviousPosition = default(Vector3);

	// Token: 0x040027FE RID: 10238
	private Vector3 m_Velocity = default(Vector3);

	// Token: 0x040027FF RID: 10239
	private float m_UnclampedMovementSpeed;

	// Token: 0x04002800 RID: 10240
	private float m_DashTimer;

	// Token: 0x04002801 RID: 10241
	private float m_Speed;

	// Token: 0x020009F6 RID: 2550
	[Serializable]
	public class MapAvatarRevTags
	{
		// Token: 0x04002802 RID: 10242
		[SerializeField]
		public GameOneShotAudioTag m_landTag = GameOneShotAudioTag.VanRev;

		// Token: 0x04002803 RID: 10243
		[SerializeField]
		public GameOneShotAudioTag m_waterTag = GameOneShotAudioTag.WorldMapBoatRev;

		// Token: 0x04002804 RID: 10244
		[SerializeField]
		public GameOneShotAudioTag m_flyingTag = GameOneShotAudioTag.WorldMapPlaneRev;
	}
}
