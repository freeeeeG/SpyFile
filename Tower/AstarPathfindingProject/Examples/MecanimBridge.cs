using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000ED RID: 237
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_mecanim_bridge.php")]
	public class MecanimBridge : VersionedMonoBehaviour
	{
		// Token: 0x060009F4 RID: 2548 RVA: 0x000413F8 File Offset: 0x0003F5F8
		protected override void Awake()
		{
			base.Awake();
			this.ai = base.GetComponent<IAstarAI>();
			this.anim = base.GetComponent<Animator>();
			this.tr = base.transform;
			this.footTransforms = new Transform[]
			{
				this.anim.GetBoneTransform(HumanBodyBones.LeftFoot),
				this.anim.GetBoneTransform(HumanBodyBones.RightFoot)
			};
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00041459 File Offset: 0x0003F659
		private void Update()
		{
			(this.ai as AIBase).canMove = false;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0004146C File Offset: 0x0003F66C
		private Vector3 CalculateBlendPoint()
		{
			if (this.footTransforms[0] == null || this.footTransforms[1] == null)
			{
				return this.tr.position;
			}
			Vector3 position = this.footTransforms[0].position;
			Vector3 position2 = this.footTransforms[1].position;
			Vector3 vector = (position - this.prevFootPos[0]) / Time.deltaTime;
			Vector3 vector2 = (position2 - this.prevFootPos[1]) / Time.deltaTime;
			float num = vector.magnitude + vector2.magnitude;
			float t = (num > 0f) ? (vector.magnitude / num) : 0.5f;
			this.prevFootPos[0] = position;
			this.prevFootPos[1] = position2;
			return Vector3.Lerp(position, position2, t);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0004154C File Offset: 0x0003F74C
		private void OnAnimatorMove()
		{
			Vector3 vector;
			Quaternion quaternion;
			this.ai.MovementUpdate(Time.deltaTime, out vector, out quaternion);
			Vector3 desiredVelocity = this.ai.desiredVelocity;
			Vector3 direction = desiredVelocity;
			direction.y = 0f;
			this.anim.SetFloat("InputMagnitude", (this.ai.reachedEndOfPath || direction.magnitude < 0.1f) ? 0f : 1f);
			Vector3 b = this.tr.InverseTransformDirection(direction);
			this.smoothedVelocity = Vector3.Lerp(this.smoothedVelocity, b, (this.velocitySmoothing > 0f) ? (Time.deltaTime / this.velocitySmoothing) : 1f);
			if (this.smoothedVelocity.magnitude < 0.4f)
			{
				this.smoothedVelocity = this.smoothedVelocity.normalized * 0.4f;
			}
			this.anim.SetFloat("X", this.smoothedVelocity.x);
			this.anim.SetFloat("Y", this.smoothedVelocity.z);
			float num = 360f;
			AIPath aipath = this.ai as AIPath;
			if (aipath != null)
			{
				num = aipath.rotationSpeed;
			}
			else
			{
				RichAI richAI = this.ai as RichAI;
				if (richAI != null)
				{
					num = richAI.rotationSpeed;
				}
			}
			Quaternion quaternion2 = this.RotateTowards(direction, Time.deltaTime * num);
			vector = this.ai.position;
			quaternion = this.ai.rotation;
			vector = MecanimBridge.RotatePointAround(vector, this.CalculateBlendPoint(), quaternion2 * Quaternion.Inverse(quaternion));
			quaternion = quaternion2;
			quaternion = this.anim.deltaRotation * quaternion;
			Vector3 deltaPosition = this.anim.deltaPosition;
			deltaPosition.y = desiredVelocity.y * Time.deltaTime;
			vector += deltaPosition;
			this.ai.FinalizeMovement(vector, quaternion);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0004172D File Offset: 0x0003F92D
		private static Vector3 RotatePointAround(Vector3 point, Vector3 around, Quaternion rotation)
		{
			return rotation * (point - around) + around;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00041744 File Offset: 0x0003F944
		protected virtual Quaternion RotateTowards(Vector3 direction, float maxDegrees)
		{
			if (direction != Vector3.zero)
			{
				Quaternion to = Quaternion.LookRotation(direction);
				return Quaternion.RotateTowards(this.tr.rotation, to, maxDegrees);
			}
			return this.tr.rotation;
		}

		// Token: 0x0400060F RID: 1551
		public float velocitySmoothing = 1f;

		// Token: 0x04000610 RID: 1552
		private IAstarAI ai;

		// Token: 0x04000611 RID: 1553
		private Animator anim;

		// Token: 0x04000612 RID: 1554
		private Transform tr;

		// Token: 0x04000613 RID: 1555
		private Vector3 smoothedVelocity;

		// Token: 0x04000614 RID: 1556
		private Vector3[] prevFootPos = new Vector3[2];

		// Token: 0x04000615 RID: 1557
		private Transform[] footTransforms;
	}
}
