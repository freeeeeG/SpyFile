using System;
using System.Collections;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000F6 RID: 246
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_animation_link_traverser.php")]
	public class AnimationLinkTraverser : VersionedMonoBehaviour
	{
		// Token: 0x06000A18 RID: 2584 RVA: 0x000423D4 File Offset: 0x000405D4
		private void OnEnable()
		{
			this.ai = base.GetComponent<RichAI>();
			if (this.ai != null)
			{
				RichAI richAI = this.ai;
				richAI.onTraverseOffMeshLink = (Func<RichSpecial, IEnumerator>)Delegate.Combine(richAI.onTraverseOffMeshLink, new Func<RichSpecial, IEnumerator>(this.TraverseOffMeshLink));
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00042423 File Offset: 0x00040623
		private void OnDisable()
		{
			if (this.ai != null)
			{
				RichAI richAI = this.ai;
				richAI.onTraverseOffMeshLink = (Func<RichSpecial, IEnumerator>)Delegate.Remove(richAI.onTraverseOffMeshLink, new Func<RichSpecial, IEnumerator>(this.TraverseOffMeshLink));
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0004245B File Offset: 0x0004065B
		protected virtual IEnumerator TraverseOffMeshLink(RichSpecial rs)
		{
			AnimationLink link = rs.nodeLink as AnimationLink;
			if (link == null)
			{
				Debug.LogError("Unhandled RichSpecial");
				yield break;
			}
			for (;;)
			{
				Quaternion rotation = this.ai.rotation;
				Quaternion quaternion = this.ai.SimulateRotationTowards(rs.first.forward, this.ai.rotationSpeed * Time.deltaTime);
				if (rotation == quaternion)
				{
					break;
				}
				this.ai.FinalizeMovement(this.ai.position, quaternion);
				yield return null;
			}
			base.transform.parent.position = base.transform.position;
			base.transform.parent.rotation = base.transform.rotation;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			if (rs.reverse && link.reverseAnim)
			{
				this.anim[link.clip].speed = -link.animSpeed;
				this.anim[link.clip].normalizedTime = 1f;
				this.anim.Play(link.clip);
				this.anim.Sample();
			}
			else
			{
				this.anim[link.clip].speed = link.animSpeed;
				this.anim.Rewind(link.clip);
				this.anim.Play(link.clip);
			}
			base.transform.parent.position -= base.transform.position - base.transform.parent.position;
			yield return new WaitForSeconds(Mathf.Abs(this.anim[link.clip].length / link.animSpeed));
			yield break;
		}

		// Token: 0x04000641 RID: 1601
		public Animation anim;

		// Token: 0x04000642 RID: 1602
		private RichAI ai;
	}
}
