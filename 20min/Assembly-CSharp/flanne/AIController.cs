using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000091 RID: 145
	public class AIController : MonoBehaviour
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x000196BF File Offset: 0x000178BF
		public List<AIComponent> enemies
		{
			get
			{
				return new List<AIComponent>(AIController.aiComponents);
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000196CB File Offset: 0x000178CB
		private void Awake()
		{
			AIController.SharedInstance = this;
			AIController.aiComponents = new List<AIComponent>();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000196DD File Offset: 0x000178DD
		private void Start()
		{
			this._layer = 1 << TagLayerUtil.Enemy;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x000196F4 File Offset: 0x000178F4
		private void FixedUpdate()
		{
			for (int i = 0; i < AIController.aiComponents.Count; i++)
			{
				if (AIController.aiComponents[i].frozen)
				{
					AIController.aiComponents[i].moveComponent.vector = Vector2.zero;
				}
				else if (AIController.aiComponents[i].gameObject.activeInHierarchy)
				{
					AIComponent aicomponent = AIController.aiComponents[i];
					Vector2 vector = this.followTransform.position - aicomponent.transform.position;
					if (this.playerRepel)
					{
						Vector2 vector2 = -1f * vector;
						if (Vector3.Dot(aicomponent.moveComponent.vector, vector2.normalized) < aicomponent.maxMoveSpeed)
						{
							aicomponent.moveComponent.vector += vector2.normalized * aicomponent.acceleration * Time.fixedDeltaTime;
						}
					}
					else if (vector.sqrMagnitude < aicomponent.specialRangeSqr || aicomponent.specialTimer > 0f)
					{
						if (!aicomponent.dontFaceDuringSpecial)
						{
							this.AILookTowards(aicomponent, vector);
						}
						if (aicomponent.specialTimer <= 0f)
						{
							aicomponent.specialTimer += aicomponent.specialCooldown;
							aicomponent.specialSO.Use(aicomponent, this.followTransform);
							if (!aicomponent.dontLookAtPlayer)
							{
								this.AILookTowards(aicomponent, vector);
							}
						}
						aicomponent.specialTimer -= Time.fixedDeltaTime;
					}
					else
					{
						if (!aicomponent.dontLookAtPlayer)
						{
							this.AILookTowards(aicomponent, vector);
						}
						Vector2 vector3 = Vector2.zero;
						Transform closestAI = this.GetClosestAI(aicomponent);
						if (closestAI != null && !aicomponent.ignoreFlock)
						{
							vector3 = aicomponent.transform.position - closestAI.position;
						}
						else
						{
							vector3 = vector;
						}
						if (Vector3.Dot(aicomponent.moveComponent.vector, vector3.normalized) < aicomponent.maxMoveSpeed)
						{
							aicomponent.moveComponent.vector += vector3.normalized * aicomponent.acceleration * Time.fixedDeltaTime;
						}
					}
				}
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00019940 File Offset: 0x00017B40
		public void Register(AIComponent ai)
		{
			AIController.aiComponents.Add(ai);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001994D File Offset: 0x00017B4D
		public void UnRegister(AIComponent ai)
		{
			AIController.aiComponents.Remove(ai);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001995C File Offset: 0x00017B5C
		private void AILookTowards(AIComponent ai, Vector2 direction)
		{
			if (ai.rotateTowardsPlayer)
			{
				float angle = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
				ai.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				return;
			}
			if (direction.x < 0f)
			{
				ai.transform.localScale = new Vector2(-1f, 1f);
				return;
			}
			if (direction.x > 0f)
			{
				ai.transform.localScale = new Vector2(1f, 1f);
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000199FC File Offset: 0x00017BFC
		private Transform GetClosestAI(AIComponent ai)
		{
			if (ai.maxMoveSpeed <= 0f)
			{
				return null;
			}
			if (ai.gameObject.layer != TagLayerUtil.Enemy)
			{
				return null;
			}
			int num = Physics2D.OverlapCircleNonAlloc(ai.transform.position, this.flockDistance, this._colliders, this._layer);
			if (num < 2)
			{
				return null;
			}
			for (int i = 0; i < num; i++)
			{
				if (this._colliders[i].gameObject != ai.gameObject)
				{
					return this._colliders[i].gameObject.transform;
				}
			}
			return null;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00019A9C File Offset: 0x00017C9C
		public static Vector2 GetClosestAIPos(Vector2 center)
		{
			AIComponent aicomponent = null;
			float num = float.PositiveInfinity;
			for (int i = 0; i < AIController.aiComponents.Count; i++)
			{
				if (AIController.aiComponents[i].gameObject.activeSelf && !AIController.aiComponents[i].tag.Contains("Passive"))
				{
					Vector2 b = AIController.aiComponents[i].transform.position;
					float sqrMagnitude = (center - b).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						aicomponent = AIController.aiComponents[i];
					}
				}
			}
			Vector2 result = Vector2.zero;
			if (aicomponent != null)
			{
				result = aicomponent.gameObject.transform.position;
			}
			return result;
		}

		// Token: 0x04000337 RID: 823
		public static AIController SharedInstance;

		// Token: 0x04000338 RID: 824
		public bool playerRepel;

		// Token: 0x04000339 RID: 825
		[SerializeField]
		private Transform followTransform;

		// Token: 0x0400033A RID: 826
		[SerializeField]
		private float flockDistance;

		// Token: 0x0400033B RID: 827
		private static List<AIComponent> aiComponents;

		// Token: 0x0400033C RID: 828
		private Collider2D[] _colliders = new Collider2D[2];

		// Token: 0x0400033D RID: 829
		private int _layer;
	}
}
