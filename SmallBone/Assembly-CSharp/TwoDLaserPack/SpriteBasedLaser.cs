using System;
using System.Collections;
using UnityEngine;

namespace TwoDLaserPack
{
	// Token: 0x02001665 RID: 5733
	public class SpriteBasedLaser : MonoBehaviour
	{
		// Token: 0x14000100 RID: 256
		// (add) Token: 0x06006D5D RID: 27997 RVA: 0x00139808 File Offset: 0x00137A08
		// (remove) Token: 0x06006D5E RID: 27998 RVA: 0x00139840 File Offset: 0x00137A40
		public event SpriteBasedLaser.LaserHitTriggerHandler OnLaserHitTriggered;

		// Token: 0x06006D5F RID: 27999 RVA: 0x00139875 File Offset: 0x00137A75
		private void Awake()
		{
			this.hitSparkEmission = this.hitSparkParticleSystem.emission;
		}

		// Token: 0x06006D60 RID: 28000 RVA: 0x00139888 File Offset: 0x00137A88
		private void OnEnable()
		{
			this.gameObjectCached = base.gameObject;
			if (this.laserLineRendererArc != null)
			{
				this.laserLineRendererArc.SetVertexCount(this.laserArcSegments);
			}
		}

		// Token: 0x06006D61 RID: 28001 RVA: 0x001398B5 File Offset: 0x00137AB5
		private void Start()
		{
			this.startLaserLength = this.maxLaserLength;
			if (this.laserOscillationPositionerScript != null)
			{
				this.laserOscillationPositionerScript.radius = this.oscillationThreshold;
			}
		}

		// Token: 0x06006D62 RID: 28002 RVA: 0x001398E4 File Offset: 0x00137AE4
		private void OscillateLaserParts(float currentLaserDistance)
		{
			if (this.laserOscillationPositionerScript == null)
			{
				return;
			}
			this.lerpYValue = Mathf.Lerp(this.middleGoPiece.transform.localPosition.y, this.laserOscillationPositionerScript.randomPointInCircle.y, Time.deltaTime * this.oscillationSpeed);
			if (this.startGoPiece != null && this.middleGoPiece != null)
			{
				Vector2 b = new Vector2(this.startGoPiece.transform.localPosition.x, this.laserOscillationPositionerScript.randomPointInCircle.y);
				Vector2 v = Vector2.Lerp(this.startGoPiece.transform.localPosition, b, Time.deltaTime * this.oscillationSpeed);
				this.startGoPiece.transform.localPosition = v;
				Vector2 v2 = new Vector2(currentLaserDistance / 2f + this.startSpriteWidth / 4f, this.lerpYValue);
				this.middleGoPiece.transform.localPosition = v2;
			}
			if (this.endGoPiece != null)
			{
				Vector2 v3 = new Vector2(currentLaserDistance + this.startSpriteWidth / 2f, this.lerpYValue);
				this.endGoPiece.transform.localPosition = v3;
			}
		}

		// Token: 0x06006D63 RID: 28003 RVA: 0x00139A40 File Offset: 0x00137C40
		private void SetLaserArcVertices(float distancePoint, bool useHitPoint)
		{
			for (int i = 1; i < this.laserArcSegments; i++)
			{
				float y = Mathf.Clamp(Mathf.Sin((float)i + Time.time * UnityEngine.Random.Range(0.5f, 1.3f)), this.laserArcMaxYDown, this.laserArcMaxYUp);
				Vector2 v = new Vector2((float)i * 1.2f, y);
				if (useHitPoint && i == this.laserArcSegments - 1)
				{
					this.laserLineRendererArc.SetPosition(i, new Vector2(distancePoint, 0f));
				}
				else
				{
					this.laserLineRendererArc.SetPosition(i, v);
				}
			}
		}

		// Token: 0x06006D64 RID: 28004 RVA: 0x00139AE4 File Offset: 0x00137CE4
		private void Update()
		{
			if (this.gameObjectCached != null && this.laserActive)
			{
				if (this.startGoPiece == null)
				{
					this.InstantiateLaserPart(ref this.startGoPiece, this.laserStartPiece);
					this.startGoPiece.transform.parent = base.transform;
					this.startGoPiece.transform.localPosition = Vector2.zero;
					this.startSpriteWidth = this.laserStartPiece.GetComponent<Renderer>().bounds.size.x;
				}
				if (this.middleGoPiece == null)
				{
					this.InstantiateLaserPart(ref this.middleGoPiece, this.laserMiddlePiece);
					this.middleGoPiece.transform.parent = base.transform;
					this.middleGoPiece.transform.localPosition = Vector2.zero;
				}
				this.middleGoPiece.transform.localScale = new Vector3(this.maxLaserLength - this.startSpriteWidth + 0.2f, this.middleGoPiece.transform.localScale.y, this.middleGoPiece.transform.localScale.z);
				if (this.oscillateLaser)
				{
					this.OscillateLaserParts(this.maxLaserLength);
				}
				else
				{
					if (this.middleGoPiece != null)
					{
						this.middleGoPiece.transform.localPosition = new Vector2(this.maxLaserLength / 2f + this.startSpriteWidth / 4f, this.lerpYValue);
					}
					if (this.endGoPiece != null)
					{
						this.endGoPiece.transform.localPosition = new Vector2(this.maxLaserLength + this.startSpriteWidth / 2f, 0f);
					}
				}
				RaycastHit2D hit;
				if (this.laserRotationEnabled && this.targetGo != null)
				{
					Vector3 vector = this.targetGo.transform.position - this.gameObjectCached.transform.position;
					this.laserAngle = Mathf.Atan2(vector.y, vector.x);
					if (this.laserAngle < 0f)
					{
						this.laserAngle = 6.2831855f + this.laserAngle;
					}
					float angle = this.laserAngle * 57.29578f;
					if (this.lerpLaserRotation)
					{
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.AngleAxis(angle, base.transform.forward), Time.deltaTime * this.turningRate);
						Vector3 v = base.transform.rotation * Vector3.right;
						hit = Physics2D.Raycast(base.transform.position, v, this.maxLaserRaycastDistance, this.mask);
					}
					else
					{
						base.transform.rotation = Quaternion.AngleAxis(angle, base.transform.forward);
						hit = Physics2D.Raycast(base.transform.position, vector, this.maxLaserRaycastDistance, this.mask);
					}
				}
				else
				{
					hit = Physics2D.Raycast(base.transform.position, base.transform.right, this.maxLaserRaycastDistance, this.mask);
				}
				if (!this.ignoreCollisions)
				{
					if (hit.collider != null)
					{
						this.maxLaserLength = Vector2.Distance(hit.point, base.transform.position) + this.startSpriteWidth / 4f;
						this.InstantiateLaserPart(ref this.endGoPiece, this.laserEndPiece);
						if (this.hitSparkParticleSystem != null)
						{
							this.hitSparkParticleSystem.transform.position = hit.point;
							this.hitSparkEmission.enabled = true;
						}
						if (this.useArc)
						{
							if (!this.laserLineRendererArc.enabled)
							{
								this.laserLineRendererArc.enabled = true;
							}
							this.SetLaserArcVertices(this.maxLaserLength, true);
							this.SetLaserArcSegmentLength();
						}
						else if (this.laserLineRendererArc.enabled)
						{
							this.laserLineRendererArc.enabled = false;
						}
						if (!this.waitingForTriggerTime)
						{
							base.StartCoroutine(this.HitTrigger(this.collisionTriggerInterval, hit));
							return;
						}
					}
					else
					{
						this.SetLaserBackToDefaults();
						if (this.useArc)
						{
							if (!this.laserLineRendererArc.enabled)
							{
								this.laserLineRendererArc.enabled = true;
							}
							this.SetLaserArcSegmentLength();
							this.SetLaserArcVertices(0f, false);
							return;
						}
						if (this.laserLineRendererArc.enabled)
						{
							this.laserLineRendererArc.enabled = false;
							return;
						}
					}
				}
				else
				{
					this.SetLaserBackToDefaults();
					this.SetLaserArcVertices(0f, false);
					this.SetLaserArcSegmentLength();
				}
			}
		}

		// Token: 0x06006D65 RID: 28005 RVA: 0x00139FC5 File Offset: 0x001381C5
		private IEnumerator HitTrigger(float triggerInterval, RaycastHit2D hit)
		{
			this.waitingForTriggerTime = true;
			if (this.OnLaserHitTriggered != null)
			{
				this.OnLaserHitTriggered(hit);
			}
			yield return new WaitForSeconds(triggerInterval);
			this.waitingForTriggerTime = false;
			yield break;
		}

		// Token: 0x06006D66 RID: 28006 RVA: 0x00139FE4 File Offset: 0x001381E4
		public void SetLaserState(bool enabledStatus)
		{
			this.laserActive = enabledStatus;
			if (this.startGoPiece != null)
			{
				this.startGoPiece.SetActive(enabledStatus);
			}
			if (this.middleGoPiece != null)
			{
				this.middleGoPiece.SetActive(enabledStatus);
			}
			if (this.endGoPiece != null)
			{
				this.endGoPiece.SetActive(enabledStatus);
			}
			if (this.laserLineRendererArc != null)
			{
				this.laserLineRendererArc.enabled = enabledStatus;
			}
			if (this.hitSparkParticleSystem != null)
			{
				this.hitSparkEmission.enabled = enabledStatus;
			}
		}

		// Token: 0x06006D67 RID: 28007 RVA: 0x0013A07C File Offset: 0x0013827C
		private void SetLaserArcSegmentLength()
		{
			int vertexCount = Mathf.Abs((int)this.maxLaserLength);
			this.laserLineRendererArc.SetVertexCount(vertexCount);
			this.laserArcSegments = vertexCount;
		}

		// Token: 0x06006D68 RID: 28008 RVA: 0x0013A0AC File Offset: 0x001382AC
		private void SetLaserBackToDefaults()
		{
			UnityEngine.Object.Destroy(this.endGoPiece);
			this.maxLaserLength = this.startLaserLength;
			if (this.hitSparkParticleSystem != null)
			{
				this.hitSparkEmission.enabled = false;
				this.hitSparkParticleSystem.transform.position = new Vector2(this.maxLaserLength, base.transform.position.y);
			}
		}

		// Token: 0x06006D69 RID: 28009 RVA: 0x0013A11C File Offset: 0x0013831C
		private void InstantiateLaserPart(ref GameObject laserComponent, GameObject laserPart)
		{
			if (laserComponent == null)
			{
				laserComponent = UnityEngine.Object.Instantiate<GameObject>(laserPart);
				laserComponent.transform.parent = base.gameObject.transform;
				laserComponent.transform.localPosition = Vector2.zero;
				laserComponent.transform.localEulerAngles = Vector2.zero;
			}
		}

		// Token: 0x06006D6A RID: 28010 RVA: 0x0013A17E File Offset: 0x0013837E
		public void DisableLaserGameObjectComponents()
		{
			UnityEngine.Object.Destroy(this.startGoPiece);
			UnityEngine.Object.Destroy(this.middleGoPiece);
			UnityEngine.Object.Destroy(this.endGoPiece);
		}

		// Token: 0x0400593E RID: 22846
		public GameObject laserStartPiece;

		// Token: 0x0400593F RID: 22847
		public GameObject laserMiddlePiece;

		// Token: 0x04005940 RID: 22848
		public GameObject laserEndPiece;

		// Token: 0x04005941 RID: 22849
		public LineRenderer laserLineRendererArc;

		// Token: 0x04005942 RID: 22850
		public int laserArcSegments = 20;

		// Token: 0x04005943 RID: 22851
		public RandomPositionMover laserOscillationPositionerScript;

		// Token: 0x04005944 RID: 22852
		public bool oscillateLaser;

		// Token: 0x04005945 RID: 22853
		public float maxLaserLength = 20f;

		// Token: 0x04005946 RID: 22854
		public float oscillationSpeed = 1f;

		// Token: 0x04005947 RID: 22855
		public bool laserActive;

		// Token: 0x04005948 RID: 22856
		public bool ignoreCollisions;

		// Token: 0x04005949 RID: 22857
		public GameObject targetGo;

		// Token: 0x0400594A RID: 22858
		public ParticleSystem hitSparkParticleSystem;

		// Token: 0x0400594B RID: 22859
		public float laserArcMaxYDown;

		// Token: 0x0400594C RID: 22860
		public float laserArcMaxYUp;

		// Token: 0x0400594D RID: 22861
		public float maxLaserRaycastDistance;

		// Token: 0x0400594E RID: 22862
		public bool laserRotationEnabled;

		// Token: 0x0400594F RID: 22863
		public bool lerpLaserRotation;

		// Token: 0x04005950 RID: 22864
		public float turningRate = 3f;

		// Token: 0x04005951 RID: 22865
		public float collisionTriggerInterval = 0.25f;

		// Token: 0x04005952 RID: 22866
		public LayerMask mask;

		// Token: 0x04005954 RID: 22868
		public bool useArc;

		// Token: 0x04005955 RID: 22869
		public float oscillationThreshold = 0.2f;

		// Token: 0x04005956 RID: 22870
		private GameObject gameObjectCached;

		// Token: 0x04005957 RID: 22871
		private float laserAngle;

		// Token: 0x04005958 RID: 22872
		private float lerpYValue;

		// Token: 0x04005959 RID: 22873
		private float startLaserLength;

		// Token: 0x0400595A RID: 22874
		private GameObject startGoPiece;

		// Token: 0x0400595B RID: 22875
		private GameObject middleGoPiece;

		// Token: 0x0400595C RID: 22876
		private GameObject endGoPiece;

		// Token: 0x0400595D RID: 22877
		private float startSpriteWidth;

		// Token: 0x0400595E RID: 22878
		private bool waitingForTriggerTime;

		// Token: 0x0400595F RID: 22879
		private ParticleSystem.EmissionModule hitSparkEmission;

		// Token: 0x02001666 RID: 5734
		// (Invoke) Token: 0x06006D6D RID: 28013
		public delegate void LaserHitTriggerHandler(RaycastHit2D hitInfo);
	}
}
