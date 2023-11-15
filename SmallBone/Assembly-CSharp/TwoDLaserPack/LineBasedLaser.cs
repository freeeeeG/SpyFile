using System;
using System.Collections;
using Characters;
using UnityEngine;

namespace TwoDLaserPack
{
	// Token: 0x02001662 RID: 5730
	public class LineBasedLaser : MonoBehaviour
	{
		// Token: 0x140000FF RID: 255
		// (add) Token: 0x06006D47 RID: 27975 RVA: 0x00138FF8 File Offset: 0x001371F8
		// (remove) Token: 0x06006D48 RID: 27976 RVA: 0x00139030 File Offset: 0x00137230
		public event LineBasedLaser.LaserHitTriggerHandler OnLaserHitTriggered;

		// Token: 0x06006D49 RID: 27977 RVA: 0x00139065 File Offset: 0x00137265
		private void Awake()
		{
			this.hitSparkEmission = this.hitSparkParticleSystem.emission;
		}

		// Token: 0x06006D4A RID: 27978 RVA: 0x00139078 File Offset: 0x00137278
		private void Start()
		{
			this.startLaserTextureXScale = this.laserLineRenderer.material.mainTextureScale.x;
			this.startLaserSegmentLength = this.laserArcSegments;
			this.laserLineRenderer.sortingLayerName = this.sortLayer;
			this.laserLineRenderer.sortingOrder = this.sortOrder;
			this.laserLineRendererArc.sortingLayerName = this.sortLayer;
			this.laserLineRendererArc.sortingOrder = this.sortOrder;
		}

		// Token: 0x06006D4B RID: 27979 RVA: 0x001390F0 File Offset: 0x001372F0
		private void OnEnable()
		{
			this.gameObjectCached = base.gameObject;
			if (this.laserLineRendererArc != null)
			{
				this.laserLineRendererArc.SetVertexCount(this.laserArcSegments);
			}
		}

		// Token: 0x06006D4C RID: 27980 RVA: 0x00139120 File Offset: 0x00137320
		private void Update()
		{
			if (this.gameObjectCached != null && this.laserActive)
			{
				this.laserLineRenderer.material.mainTextureOffset = new Vector2(this.laserTextureOffset, 0f);
				this.laserTextureOffset -= this._character.chronometer.master.deltaTime * this.laserTexOffsetSpeed;
				RaycastHit2D raycastHit2D;
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
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.AngleAxis(angle, base.transform.forward), this._character.chronometer.master.deltaTime * this.turningRate);
						Vector3 v = base.transform.rotation * Vector3.right;
						raycastHit2D = Physics2D.Raycast(base.transform.position, v, this.maxLaserRaycastDistance, this.mask);
					}
					else
					{
						base.transform.rotation = Quaternion.AngleAxis(angle, base.transform.forward);
						raycastHit2D = Physics2D.Raycast(base.transform.position, vector, this.maxLaserRaycastDistance, this.mask);
					}
				}
				else
				{
					raycastHit2D = Physics2D.Raycast(base.transform.position, base.transform.right, this.maxLaserRaycastDistance, this.mask);
				}
				if (!this.ignoreCollisions)
				{
					if (!(raycastHit2D.collider != null))
					{
						this.SetLaserToDefaultLength();
						return;
					}
					this.SetLaserEndToTargetLocation(raycastHit2D);
					if (!this.waitingForTriggerTime)
					{
						base.StartCoroutine(this.HitTrigger(this.collisionTriggerInterval, raycastHit2D));
						return;
					}
				}
				else
				{
					this.SetLaserToDefaultLength();
				}
			}
		}

		// Token: 0x06006D4D RID: 27981 RVA: 0x0013937C File Offset: 0x0013757C
		private IEnumerator HitTrigger(float triggerInterval, RaycastHit2D hit)
		{
			this.waitingForTriggerTime = true;
			LineBasedLaser.LaserHitTriggerHandler onLaserHitTriggered = this.OnLaserHitTriggered;
			if (onLaserHitTriggered != null)
			{
				onLaserHitTriggered(hit);
			}
			yield return this._character.chronometer.master.WaitForSeconds(triggerInterval);
			this.waitingForTriggerTime = false;
			yield break;
		}

		// Token: 0x06006D4E RID: 27982 RVA: 0x0013939C File Offset: 0x0013759C
		public void SetLaserState(bool enabledStatus)
		{
			this.laserActive = enabledStatus;
			this.laserLineRenderer.enabled = enabledStatus;
			if (this.laserLineRendererArc != null)
			{
				this.laserLineRendererArc.enabled = enabledStatus;
			}
			if (this.hitSparkParticleSystem != null)
			{
				this.hitSparkEmission.enabled = enabledStatus;
			}
		}

		// Token: 0x06006D4F RID: 27983 RVA: 0x001393F0 File Offset: 0x001375F0
		private void SetLaserEndToTargetLocation(RaycastHit2D hit)
		{
			float num = Vector2.Distance(hit.point, this.laserLineRenderer.transform.position);
			this.laserLineRenderer.SetPosition(1, new Vector2(num, 0f));
			this.laserTextureXScale = this.startLaserTextureXScale * num;
			this.laserLineRenderer.material.mainTextureScale = new Vector2(this.laserTextureXScale, 1f);
			if (this.useArc)
			{
				if (!this.laserLineRendererArc.enabled)
				{
					this.laserLineRendererArc.enabled = true;
				}
				int vertexCount = Mathf.Abs((int)num);
				this.laserLineRendererArc.SetVertexCount(vertexCount);
				this.laserArcSegments = vertexCount;
				this.SetLaserArcVertices(num, true);
			}
			else if (this.laserLineRendererArc.enabled)
			{
				this.laserLineRendererArc.enabled = false;
			}
			if (this.hitSparkParticleSystem != null)
			{
				this.hitSparkParticleSystem.transform.position = hit.point;
				if (this._hitEffect == null)
				{
					this.laserhitEffect.Spawn(hit.point, true);
				}
				this._hitEffect.transform.position = hit.point;
				this.hitSparkEmission.enabled = true;
			}
		}

		// Token: 0x06006D50 RID: 27984 RVA: 0x00139544 File Offset: 0x00137744
		private void SetLaserToDefaultLength()
		{
			this.laserLineRenderer.SetPosition(1, new Vector2((float)this.laserArcSegments, 0f));
			this.laserTextureXScale = this.startLaserTextureXScale * (float)this.laserArcSegments;
			this.laserLineRenderer.material.mainTextureScale = new Vector2(this.laserTextureXScale, 1f);
			if (this.useArc)
			{
				if (!this.laserLineRendererArc.enabled)
				{
					this.laserLineRendererArc.enabled = true;
				}
				this.laserLineRendererArc.SetVertexCount(this.startLaserSegmentLength);
				this.laserArcSegments = this.startLaserSegmentLength;
				this.SetLaserArcVertices(0f, false);
			}
			else
			{
				if (this.laserLineRendererArc.enabled)
				{
					this.laserLineRendererArc.enabled = false;
				}
				this.laserLineRendererArc.SetVertexCount(this.startLaserSegmentLength);
				this.laserArcSegments = this.startLaserSegmentLength;
			}
			if (this.hitSparkParticleSystem != null)
			{
				this.hitSparkEmission.enabled = false;
				this.hitSparkParticleSystem.transform.position = new Vector2((float)this.laserArcSegments, base.transform.position.y);
			}
		}

		// Token: 0x06006D51 RID: 27985 RVA: 0x00139674 File Offset: 0x00137874
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

		// Token: 0x0400591A RID: 22810
		[SerializeField]
		private Character _character;

		// Token: 0x0400591B RID: 22811
		public LineRenderer laserLineRendererArc;

		// Token: 0x0400591C RID: 22812
		public LineRenderer laserLineRenderer;

		// Token: 0x0400591D RID: 22813
		public int laserArcSegments = 20;

		// Token: 0x0400591E RID: 22814
		public bool laserActive;

		// Token: 0x0400591F RID: 22815
		public bool ignoreCollisions;

		// Token: 0x04005920 RID: 22816
		public GameObject targetGo;

		// Token: 0x04005921 RID: 22817
		public float laserTexOffsetSpeed = 1f;

		// Token: 0x04005922 RID: 22818
		public ParticleSystem hitSparkParticleSystem;

		// Token: 0x04005923 RID: 22819
		[SerializeField]
		private PoolObject laserhitEffect;

		// Token: 0x04005924 RID: 22820
		private PoolObject _hitEffect;

		// Token: 0x04005925 RID: 22821
		public float laserArcMaxYDown;

		// Token: 0x04005926 RID: 22822
		public float laserArcMaxYUp;

		// Token: 0x04005927 RID: 22823
		public float maxLaserRaycastDistance = 20f;

		// Token: 0x04005928 RID: 22824
		public bool laserRotationEnabled;

		// Token: 0x04005929 RID: 22825
		public bool lerpLaserRotation;

		// Token: 0x0400592A RID: 22826
		public float turningRate = 3f;

		// Token: 0x0400592B RID: 22827
		public float collisionTriggerInterval = 0.25f;

		// Token: 0x0400592C RID: 22828
		public LayerMask mask;

		// Token: 0x0400592D RID: 22829
		public string sortLayer = "Default";

		// Token: 0x0400592E RID: 22830
		public int sortOrder;

		// Token: 0x04005930 RID: 22832
		public bool useArc;

		// Token: 0x04005931 RID: 22833
		private GameObject gameObjectCached;

		// Token: 0x04005932 RID: 22834
		private float laserAngle;

		// Token: 0x04005933 RID: 22835
		private float laserTextureOffset;

		// Token: 0x04005934 RID: 22836
		private float laserTextureXScale;

		// Token: 0x04005935 RID: 22837
		private float startLaserTextureXScale;

		// Token: 0x04005936 RID: 22838
		private int startLaserSegmentLength;

		// Token: 0x04005937 RID: 22839
		private bool waitingForTriggerTime;

		// Token: 0x04005938 RID: 22840
		private ParticleSystem.EmissionModule hitSparkEmission;

		// Token: 0x02001663 RID: 5731
		// (Invoke) Token: 0x06006D54 RID: 27988
		public delegate void LaserHitTriggerHandler(RaycastHit2D hitInfo);
	}
}
