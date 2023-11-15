using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pathfinding.Examples
{
	// Token: 0x020000EB RID: 235
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_turn_based_manager.php")]
	public class TurnBasedManager : MonoBehaviour
	{
		// Token: 0x060009E6 RID: 2534 RVA: 0x000410FE File Offset: 0x0003F2FE
		private void Awake()
		{
			this.eventSystem = Object.FindObjectOfType<EventSystem>();
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0004110C File Offset: 0x0003F30C
		private void Update()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (this.eventSystem.IsPointerOverGameObject())
			{
				return;
			}
			if (this.state == TurnBasedManager.State.SelectTarget)
			{
				this.HandleButtonUnderRay(ray);
			}
			if ((this.state == TurnBasedManager.State.SelectUnit || this.state == TurnBasedManager.State.SelectTarget) && Input.GetKeyDown(KeyCode.Mouse0))
			{
				TurnBasedAI byRay = this.GetByRay<TurnBasedAI>(ray);
				if (byRay != null)
				{
					this.Select(byRay);
					this.DestroyPossibleMoves();
					this.GeneratePossibleMoves(this.selected);
					this.state = TurnBasedManager.State.SelectTarget;
				}
			}
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00041198 File Offset: 0x0003F398
		private void HandleButtonUnderRay(Ray ray)
		{
			Astar3DButton byRay = this.GetByRay<Astar3DButton>(ray);
			if (byRay != null && Input.GetKeyDown(KeyCode.Mouse0))
			{
				byRay.OnClick();
				this.DestroyPossibleMoves();
				this.state = TurnBasedManager.State.Move;
				base.StartCoroutine(this.MoveToNode(this.selected, byRay.node));
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x000411F0 File Offset: 0x0003F3F0
		private T GetByRay<T>(Ray ray) where T : class
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, this.layerMask))
			{
				return raycastHit.transform.GetComponentInParent<T>();
			}
			return default(T);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0004122D File Offset: 0x0003F42D
		private void Select(TurnBasedAI unit)
		{
			this.selected = unit;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00041236 File Offset: 0x0003F436
		private IEnumerator MoveToNode(TurnBasedAI unit, GraphNode node)
		{
			ABPath path = ABPath.Construct(unit.transform.position, (Vector3)node.position, null);
			path.traversalProvider = unit.traversalProvider;
			AstarPath.StartPath(path, false);
			yield return base.StartCoroutine(path.WaitForPath());
			if (path.error)
			{
				Debug.LogError("Path failed:\n" + path.errorLog);
				this.state = TurnBasedManager.State.SelectTarget;
				this.GeneratePossibleMoves(this.selected);
				yield break;
			}
			unit.targetNode = path.path[path.path.Count - 1];
			yield return base.StartCoroutine(TurnBasedManager.MoveAlongPath(unit, path, this.movementSpeed));
			unit.blocker.BlockAtCurrentPosition();
			this.state = TurnBasedManager.State.SelectUnit;
			yield break;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00041253 File Offset: 0x0003F453
		private static IEnumerator MoveAlongPath(TurnBasedAI unit, ABPath path, float speed)
		{
			if (path.error || path.vectorPath.Count == 0)
			{
				throw new ArgumentException("Cannot follow an empty path");
			}
			float distanceAlongSegment = 0f;
			int num;
			for (int i = 0; i < path.vectorPath.Count - 1; i = num + 1)
			{
				Vector3 p0 = path.vectorPath[Mathf.Max(i - 1, 0)];
				Vector3 p = path.vectorPath[i];
				Vector3 p2 = path.vectorPath[i + 1];
				Vector3 p3 = path.vectorPath[Mathf.Min(i + 2, path.vectorPath.Count - 1)];
				float segmentLength = Vector3.Distance(p, p2);
				while (distanceAlongSegment < segmentLength)
				{
					Vector3 position = AstarSplines.CatmullRom(p0, p, p2, p3, distanceAlongSegment / segmentLength);
					unit.transform.position = position;
					yield return null;
					distanceAlongSegment += Time.deltaTime * speed;
				}
				distanceAlongSegment -= segmentLength;
				p0 = default(Vector3);
				p = default(Vector3);
				p2 = default(Vector3);
				p3 = default(Vector3);
				num = i;
			}
			unit.transform.position = path.vectorPath[path.vectorPath.Count - 1];
			yield break;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00041270 File Offset: 0x0003F470
		private void DestroyPossibleMoves()
		{
			foreach (GameObject obj in this.possibleMoves)
			{
				Object.Destroy(obj);
			}
			this.possibleMoves.Clear();
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x000412CC File Offset: 0x0003F4CC
		private void GeneratePossibleMoves(TurnBasedAI unit)
		{
			ConstantPath constantPath = ConstantPath.Construct(unit.transform.position, unit.movementPoints * 1000 + 1, null);
			constantPath.traversalProvider = unit.traversalProvider;
			AstarPath.StartPath(constantPath, false);
			constantPath.BlockUntilCalculated();
			foreach (GraphNode graphNode in constantPath.allNodes)
			{
				if (graphNode != constantPath.startNode)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.nodePrefab, (Vector3)graphNode.position, Quaternion.identity);
					this.possibleMoves.Add(gameObject);
					gameObject.GetComponent<Astar3DButton>().node = graphNode;
				}
			}
		}

		// Token: 0x04000606 RID: 1542
		private TurnBasedAI selected;

		// Token: 0x04000607 RID: 1543
		public float movementSpeed;

		// Token: 0x04000608 RID: 1544
		public GameObject nodePrefab;

		// Token: 0x04000609 RID: 1545
		public LayerMask layerMask;

		// Token: 0x0400060A RID: 1546
		private List<GameObject> possibleMoves = new List<GameObject>();

		// Token: 0x0400060B RID: 1547
		private EventSystem eventSystem;

		// Token: 0x0400060C RID: 1548
		public TurnBasedManager.State state;

		// Token: 0x02000182 RID: 386
		public enum State
		{
			// Token: 0x04000893 RID: 2195
			SelectUnit,
			// Token: 0x04000894 RID: 2196
			SelectTarget,
			// Token: 0x04000895 RID: 2197
			Move
		}
	}
}
