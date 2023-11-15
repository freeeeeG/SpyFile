using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DentedPixel.LTExamples
{
	// Token: 0x0200026D RID: 621
	public class TestingUnitTests : MonoBehaviour
	{
		// Token: 0x06000D54 RID: 3412 RVA: 0x000306BC File Offset: 0x0002E8BC
		private void Awake()
		{
			this.boxNoCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Object.Destroy(this.boxNoCollider.GetComponent(typeof(BoxCollider)));
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000306E4 File Offset: 0x0002E8E4
		private void Start()
		{
			LeanTest.timeout = 46f;
			LeanTest.expected = 62;
			LeanTween.init(1300);
			LeanTween.addListener(this.cube1, 0, new Action<LTEvent>(this.eventGameObjectCalled));
			LeanTest.expect(!LeanTween.isTweening(null), "NOTHING TWEEENING AT BEGINNING", null);
			LeanTest.expect(!LeanTween.isTweening(this.cube1), "OBJECT NOT TWEEENING AT BEGINNING", null);
			LeanTween.scaleX(this.cube4, 2f, 0f).setOnComplete(delegate()
			{
				LeanTest.expect(this.cube4.transform.localScale.x == 2f, "TWEENED WITH ZERO TIME", null);
			});
			LeanTween.dispatchEvent(0);
			LeanTest.expect(this.eventGameObjectWasCalled, "EVENT GAMEOBJECT RECEIVED", null);
			LeanTest.expect(!LeanTween.removeListener(this.cube2, 0, new Action<LTEvent>(this.eventGameObjectCalled)), "EVENT GAMEOBJECT NOT REMOVED", null);
			LeanTest.expect(LeanTween.removeListener(this.cube1, 0, new Action<LTEvent>(this.eventGameObjectCalled)), "EVENT GAMEOBJECT REMOVED", null);
			LeanTween.addListener(1, new Action<LTEvent>(this.eventGeneralCalled));
			LeanTween.dispatchEvent(1);
			LeanTest.expect(this.eventGeneralWasCalled, "EVENT ALL RECEIVED", null);
			LeanTest.expect(LeanTween.removeListener(1, new Action<LTEvent>(this.eventGeneralCalled)), "EVENT ALL REMOVED", null);
			this.lt1Id = LeanTween.move(this.cube1, new Vector3(3f, 2f, 0.5f), 1.1f).id;
			LeanTween.move(this.cube2, new Vector3(-3f, -2f, -0.5f), 1.1f);
			LeanTween.reset();
			GameObject[] cubes = new GameObject[99];
			int[] tweenIds = new int[cubes.Length];
			for (int i = 0; i < cubes.Length; i++)
			{
				GameObject gameObject = this.cubeNamed("cancel" + i);
				tweenIds[i] = LeanTween.moveX(gameObject, 100f, 1f).id;
				cubes[i] = gameObject;
			}
			int onCompleteCount = 0;
			Action <>9__21;
			LeanTween.delayedCall(cubes[0], 0.2f, delegate()
			{
				for (int l = 0; l < cubes.Length; l++)
				{
					if (l % 3 == 0)
					{
						LeanTween.cancel(cubes[l]);
					}
					else if (l % 3 == 1)
					{
						LeanTween.cancel(tweenIds[l]);
					}
					else if (l % 3 == 2)
					{
						LTDescr ltdescr3 = LeanTween.descr(tweenIds[l]);
						Action onComplete2;
						if ((onComplete2 = <>9__21) == null)
						{
							onComplete2 = (<>9__21 = delegate()
							{
								int onCompleteCount = onCompleteCount;
								onCompleteCount++;
								if (onCompleteCount >= 33)
								{
									LeanTest.expect(true, "CANCELS DO NOT EFFECT FINISHING", null);
								}
							});
						}
						ltdescr3.setOnComplete(onComplete2);
					}
				}
			});
			new LTSpline(new Vector3[]
			{
				new Vector3(-1f, 0f, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(4f, 0f, 0f),
				new Vector3(20f, 0f, 0f),
				new Vector3(30f, 0f, 0f)
			}).place(this.cube4.transform, 0.5f);
			LeanTest.expect(Vector3.Distance(this.cube4.transform.position, new Vector3(10f, 0f, 0f)) <= 0.7f, "SPLINE POSITIONING AT HALFWAY", "position is:" + this.cube4.transform.position + " but should be:(10f,0f,0f)");
			LeanTween.color(this.cube4, Color.green, 0.01f);
			GameObject gameObject2 = this.cubeNamed("cubeDest");
			Vector3 cubeDestEnd = new Vector3(100f, 20f, 0f);
			LeanTween.move(gameObject2, cubeDestEnd, 0.7f);
			GameObject cubeToTrans = this.cubeNamed("cubeToTrans");
			LeanTween.move(cubeToTrans, gameObject2.transform, 1.2f).setEase(LeanTweenType.easeOutQuad).setOnComplete(delegate()
			{
				LeanTest.expect(cubeToTrans.transform.position == cubeDestEnd, "MOVE TO TRANSFORM WORKS", null);
			});
			GameObject gameObject3 = this.cubeNamed("cubeDestroy");
			LeanTween.moveX(gameObject3, 200f, 0.05f).setDelay(0.02f).setDestroyOnComplete(true);
			LeanTween.moveX(gameObject3, 200f, 0.1f).setDestroyOnComplete(true).setOnComplete(delegate()
			{
				LeanTest.expect(true, "TWO DESTROY ON COMPLETE'S SUCCEED", null);
			});
			GameObject cubeSpline = this.cubeNamed("cubeSpline");
			LeanTween.moveSpline(cubeSpline, new Vector3[]
			{
				new Vector3(0.5f, 0f, 0.5f),
				new Vector3(0.75f, 0f, 0.75f),
				new Vector3(1f, 0f, 1f),
				new Vector3(1f, 0f, 1f)
			}, 0.1f).setOnComplete(delegate()
			{
				LeanTest.expect(Vector3.Distance(new Vector3(1f, 0f, 1f), cubeSpline.transform.position) < 0.01f, "SPLINE WITH TWO POINTS SUCCEEDS", null);
			});
			GameObject jumpCube = this.cubeNamed("jumpTime");
			jumpCube.transform.position = new Vector3(100f, 0f, 0f);
			jumpCube.transform.localScale *= 100f;
			int jumpTimeId = LeanTween.moveX(jumpCube, 200f, 1f).id;
			LeanTween.delayedCall(base.gameObject, 0.2f, delegate()
			{
				LTDescr ltdescr3 = LeanTween.descr(jumpTimeId);
				float beforeX = jumpCube.transform.position.x;
				ltdescr3.setTime(0.5f);
				LeanTween.delayedCall(0f, delegate()
				{
				}).setOnStart(delegate
				{
					float num = 1f;
					beforeX += Time.deltaTime * 100f * 2f;
					LeanTest.expect(Mathf.Abs(jumpCube.transform.position.x - beforeX) < num, "CHANGING TIME DOESN'T JUMP AHEAD", string.Concat(new object[]
					{
						"Difference:",
						Mathf.Abs(jumpCube.transform.position.x - beforeX),
						" beforeX:",
						beforeX,
						" now:",
						jumpCube.transform.position.x,
						" dt:",
						Time.deltaTime
					}));
				});
			});
			GameObject zeroCube = this.cubeNamed("zeroCube");
			LeanTween.moveX(zeroCube, 10f, 0f).setOnComplete(delegate()
			{
				LeanTest.expect(zeroCube.transform.position.x == 10f, "ZERO TIME FINSHES CORRECTLY", "final x:" + zeroCube.transform.position.x);
			});
			GameObject cubeScale = this.cubeNamed("cubeScale");
			LeanTween.scale(cubeScale, new Vector3(5f, 5f, 5f), 0.01f).setOnStart(delegate
			{
				LeanTest.expect(true, "ON START WAS CALLED", null);
			}).setOnComplete(delegate()
			{
				LeanTest.expect(cubeScale.transform.localScale.z == 5f, "SCALE", string.Concat(new object[]
				{
					"expected scale z:",
					5f,
					" returned:",
					cubeScale.transform.localScale.z
				}));
			});
			GameObject cubeRotate = this.cubeNamed("cubeRotate");
			LeanTween.rotate(cubeRotate, new Vector3(0f, 180f, 0f), 0.02f).setOnComplete(delegate()
			{
				LeanTest.expect(cubeRotate.transform.eulerAngles.y == 180f, "ROTATE", string.Concat(new object[]
				{
					"expected rotate y:",
					180f,
					" returned:",
					cubeRotate.transform.eulerAngles.y
				}));
			});
			GameObject cubeRotateA = this.cubeNamed("cubeRotateA");
			LeanTween.rotateAround(cubeRotateA, Vector3.forward, 90f, 0.3f).setOnComplete(delegate()
			{
				LeanTest.expect(cubeRotateA.transform.eulerAngles.z == 90f, "ROTATE AROUND", string.Concat(new object[]
				{
					"expected rotate z:",
					90f,
					" returned:",
					cubeRotateA.transform.eulerAngles.z
				}));
			});
			GameObject cubeRotateB = this.cubeNamed("cubeRotateB");
			cubeRotateB.transform.position = new Vector3(200f, 10f, 8f);
			LeanTween.rotateAround(cubeRotateB, Vector3.forward, 360f, 0.3f).setPoint(new Vector3(5f, 3f, 2f)).setOnComplete(delegate()
			{
				LeanTest.expect(cubeRotateB.transform.position.ToString() == new Vector3(200f, 10f, 8f).ToString(), "ROTATE AROUND 360", string.Concat(new object[]
				{
					"expected rotate pos:",
					new Vector3(200f, 10f, 8f),
					" returned:",
					cubeRotateB.transform.position
				}));
			});
			LeanTween.alpha(this.cubeAlpha1, 0.5f, 0.1f).setOnUpdate(delegate(float val)
			{
				LeanTest.expect(val != 0f, "ON UPDATE VAL", null);
			}).setOnCompleteParam("Hi!").setOnComplete(delegate(object completeObj)
			{
				LeanTest.expect((string)completeObj == "Hi!", "ONCOMPLETE OBJECT", null);
				LeanTest.expect(this.cubeAlpha1.GetComponent<Renderer>().material.color.a == 0.5f, "ALPHA", null);
			});
			float onStartTime = -1f;
			LeanTween.color(this.cubeAlpha2, Color.cyan, 0.3f).setOnComplete(delegate()
			{
				LeanTest.expect(this.cubeAlpha2.GetComponent<Renderer>().material.color == Color.cyan, "COLOR", null);
				LeanTest.expect(onStartTime >= 0f && onStartTime < Time.time, "ON START", string.Concat(new object[]
				{
					"onStartTime:",
					onStartTime,
					" time:",
					Time.time
				}));
			}).setOnStart(delegate
			{
				onStartTime = Time.time;
			});
			Vector3 beforePos = this.cubeAlpha1.transform.position;
			LeanTween.moveY(this.cubeAlpha1, 3f, 0.2f).setOnComplete(delegate()
			{
				LeanTest.expect(this.cubeAlpha1.transform.position.x == beforePos.x && this.cubeAlpha1.transform.position.z == beforePos.z, "MOVE Y", null);
			});
			Vector3 beforePos2 = this.cubeAlpha2.transform.localPosition;
			LeanTween.moveLocalZ(this.cubeAlpha2, 12f, 0.2f).setOnComplete(delegate()
			{
				LeanTest.expect(this.cubeAlpha2.transform.localPosition.x == beforePos2.x && this.cubeAlpha2.transform.localPosition.y == beforePos2.y, "MOVE LOCAL Z", string.Concat(new object[]
				{
					"ax:",
					this.cubeAlpha2.transform.localPosition.x,
					" bx:",
					beforePos.x,
					" ay:",
					this.cubeAlpha2.transform.localPosition.y,
					" by:",
					beforePos2.y
				}));
			});
			AudioClip audio = LeanAudio.createAudio(new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 1f, 0f, -1f),
				new Keyframe(1f, 0f, -1f, 0f)
			}), new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.001f, 0f, 0f),
				new Keyframe(1f, 0.001f, 0f, 0f)
			}), LeanAudio.options());
			LeanTween.delayedSound(base.gameObject, audio, new Vector3(0f, 0f, 0f), 0.1f).setDelay(0.2f).setOnComplete(delegate()
			{
				LeanTest.expect(Time.time > 0f, "DELAYED SOUND", null);
			});
			int totalEasingCheck = 0;
			int totalEasingCheckSuccess = 0;
			for (int j = 0; j < 2; j++)
			{
				bool flag = j == 1;
				int totalTweenTypeLength = 33;
				Action<object> <>9__24;
				for (int k = 0; k < totalTweenTypeLength; k++)
				{
					LeanTweenType leanTweenType = (LeanTweenType)k;
					GameObject gameObject4 = this.cubeNamed("cube" + leanTweenType);
					LTDescr ltdescr = LeanTween.moveLocalX(gameObject4, 5f, 0.1f);
					Action<object> onComplete;
					if ((onComplete = <>9__24) == null)
					{
						onComplete = (<>9__24 = delegate(object obj)
						{
							GameObject gameObject5 = obj as GameObject;
							int num = totalEasingCheck;
							totalEasingCheck = num + 1;
							if (gameObject5.transform.position.x == 5f)
							{
								num = totalEasingCheckSuccess;
								totalEasingCheckSuccess = num + 1;
							}
							if (totalEasingCheck == 2 * totalTweenTypeLength)
							{
								LeanTest.expect(totalEasingCheck == totalEasingCheckSuccess, "EASING TYPES", null);
							}
						});
					}
					LTDescr ltdescr2 = ltdescr.setOnComplete(onComplete).setOnCompleteParam(gameObject4);
					if (flag)
					{
						ltdescr2.setFrom(-5f);
					}
				}
			}
			bool value2UpdateCalled = false;
			LeanTween.value(base.gameObject, new Vector2(0f, 0f), new Vector2(256f, 96f), 0.1f).setOnUpdate(delegate(Vector2 value)
			{
				value2UpdateCalled = true;
			}, null);
			LeanTween.delayedCall(0.2f, delegate()
			{
				LeanTest.expect(value2UpdateCalled, "VALUE2 UPDATE", null);
			});
			base.StartCoroutine(this.timeBasedTesting());
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00031130 File Offset: 0x0002F330
		private GameObject cubeNamed(string name)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.boxNoCollider);
			gameObject.name = name;
			return gameObject;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00031144 File Offset: 0x0002F344
		private IEnumerator timeBasedTesting()
		{
			yield return new WaitForEndOfFrame();
			GameObject gameObject = this.cubeNamed("normalTimeScale");
			LeanTween.moveX(gameObject, 12f, 1.5f).setIgnoreTimeScale(false).setOnComplete(delegate()
			{
				this.timeElapsedNormalTimeScale = Time.time;
			});
			LTDescr[] array = LeanTween.descriptions(gameObject);
			LeanTest.expect(array.Length >= 0 && array[0].to.x == 12f, "WE CAN RETRIEVE A DESCRIPTION", null);
			LeanTween.moveX(this.cubeNamed("ignoreTimeScale"), 5f, 1.5f).setIgnoreTimeScale(true).setOnComplete(delegate()
			{
				this.timeElapsedIgnoreTimeScale = Time.time;
			});
			yield return new WaitForSeconds(1.5f);
			LeanTest.expect(Mathf.Abs(this.timeElapsedNormalTimeScale - this.timeElapsedIgnoreTimeScale) < 0.7f, "START IGNORE TIMING", string.Concat(new object[]
			{
				"timeElapsedIgnoreTimeScale:",
				this.timeElapsedIgnoreTimeScale,
				" timeElapsedNormalTimeScale:",
				this.timeElapsedNormalTimeScale
			}));
			Time.timeScale = 4f;
			int pauseCount = 0;
			LeanTween.value(base.gameObject, 0f, 1f, 1f).setOnUpdate(delegate(float val)
			{
				int pauseCount = pauseCount;
				pauseCount++;
			}).pause();
			Vector3[] array2 = new Vector3[]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(-9.1f, 25.1f, 0f),
				new Vector3(-1.2f, 15.9f, 0f),
				new Vector3(-25f, 25f, 0f),
				new Vector3(-25f, 25f, 0f),
				new Vector3(-50.1f, 15.9f, 0f),
				new Vector3(-40.9f, 25.1f, 0f),
				new Vector3(-50f, 0f, 0f),
				new Vector3(-50f, 0f, 0f),
				new Vector3(-40.9f, -25.1f, 0f),
				new Vector3(-50.1f, -15.9f, 0f),
				new Vector3(-25f, -25f, 0f),
				new Vector3(-25f, -25f, 0f),
				new Vector3(0f, -15.9f, 0f),
				new Vector3(-9.1f, -25.1f, 0f),
				new Vector3(0f, 0f, 0f)
			};
			GameObject cubeRound = this.cubeNamed("bRound");
			Vector3 onStartPos = cubeRound.transform.position;
			LeanTween.moveLocal(cubeRound, array2, 0.5f).setOnComplete(delegate()
			{
				LeanTest.expect(cubeRound.transform.position == onStartPos, "BEZIER CLOSED LOOP SHOULD END AT START", string.Concat(new object[]
				{
					"onStartPos:",
					onStartPos,
					" onEnd:",
					cubeRound.transform.position
				}));
			});
			LeanTest.expect(object.Equals(new LTBezierPath(array2).ratioAtPoint(new Vector3(-25f, 25f, 0f), 0.01f), 0.25f), "BEZIER RATIO POINT", null);
			Vector3[] to = new Vector3[]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(2f, 0f, 0f),
				new Vector3(0.9f, 2f, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(0f, 0f, 0f)
			};
			GameObject cubeSpline = this.cubeNamed("bSpline");
			Vector3 onStartPosSpline = cubeSpline.transform.position;
			LeanTween.moveSplineLocal(cubeSpline, to, 0.5f).setOnComplete(delegate()
			{
				LeanTest.expect(Vector3.Distance(onStartPosSpline, cubeSpline.transform.position) <= 0.01f, "SPLINE CLOSED LOOP SHOULD END AT START", string.Concat(new object[]
				{
					"onStartPos:",
					onStartPosSpline,
					" onEnd:",
					cubeSpline.transform.position,
					" dist:",
					Vector3.Distance(onStartPosSpline, cubeSpline.transform.position)
				}));
			});
			GameObject cubeSeq = this.cubeNamed("cSeq");
			LTSeq ltseq = LeanTween.sequence(true).append(LeanTween.moveX(cubeSeq, 100f, 0.2f));
			ltseq.append(0.1f).append(LeanTween.scaleX(cubeSeq, 2f, 0.1f));
			ltseq.append(delegate()
			{
				LeanTest.expect(cubeSeq.transform.position.x == 100f, "SEQ MOVE X FINISHED", "move x:" + cubeSeq.transform.position.x);
				LeanTest.expect(cubeSeq.transform.localScale.x == 2f, "SEQ SCALE X FINISHED", "scale x:" + cubeSeq.transform.localScale.x);
			}).setScale(0.2f);
			GameObject cubeBounds = this.cubeNamed("cBounds");
			bool didPassBounds = true;
			Vector3 failPoint = Vector3.zero;
			LeanTween.move(cubeBounds, new Vector3(10f, 10f, 10f), 0.1f).setOnUpdate(delegate(float val)
			{
				if (cubeBounds.transform.position.x < 0f || cubeBounds.transform.position.x > 10f || cubeBounds.transform.position.y < 0f || cubeBounds.transform.position.y > 10f || cubeBounds.transform.position.z < 0f || cubeBounds.transform.position.z > 10f)
				{
					didPassBounds = false;
					failPoint = cubeBounds.transform.position;
				}
			}).setLoopPingPong().setRepeat(8).setOnComplete(delegate()
			{
				LeanTest.expect(didPassBounds, "OUT OF BOUNDS", string.Concat(new object[]
				{
					"pos x:",
					failPoint.x,
					" y:",
					failPoint.y,
					" z:",
					failPoint.z
				}));
			});
			this.groupTweens = new LTDescr[1200];
			this.groupGOs = new GameObject[this.groupTweens.Length];
			this.groupTweensCnt = 0;
			int descriptionMatchCount = 0;
			for (int i = 0; i < this.groupTweens.Length; i++)
			{
				GameObject gameObject2 = this.cubeNamed("c" + i);
				gameObject2.transform.position = new Vector3(0f, 0f, (float)(i * 3));
				this.groupGOs[i] = gameObject2;
			}
			yield return new WaitForEndOfFrame();
			bool hasGroupTweensCheckStarted = false;
			int setOnStartNum = 0;
			int setPosNum = 0;
			bool setPosOnUpdate = true;
			Action <>9__13;
			Action<Vector3> <>9__14;
			Action <>9__16;
			Action<object> <>9__15;
			for (int j = 0; j < this.groupTweens.Length; j++)
			{
				Vector3 vector = base.transform.position + Vector3.one * 3f;
				Dictionary<string, object> onCompleteParam = new Dictionary<string, object>
				{
					{
						"final",
						vector
					},
					{
						"go",
						this.groupGOs[j]
					}
				};
				LTDescr[] array3 = this.groupTweens;
				int num = j;
				LTDescr ltdescr = LeanTween.move(this.groupGOs[j], vector, 3f);
				Action onStart;
				if ((onStart = <>9__13) == null)
				{
					onStart = (<>9__13 = delegate()
					{
						int setOnStartNum = setOnStartNum;
						setOnStartNum++;
					});
				}
				LTDescr ltdescr2 = ltdescr.setOnStart(onStart);
				Action<Vector3> onUpdate;
				if ((onUpdate = <>9__14) == null)
				{
					onUpdate = (<>9__14 = delegate(Vector3 newPosition)
					{
						if (this.transform.position.z > newPosition.z)
						{
							setPosOnUpdate = false;
						}
					});
				}
				LTDescr ltdescr3 = ltdescr2.setOnUpdate(onUpdate, null).setOnCompleteParam(onCompleteParam);
				Action<object> onComplete;
				if ((onComplete = <>9__15) == null)
				{
					onComplete = (<>9__15 = delegate(object param)
					{
						Dictionary<string, object> dictionary = param as Dictionary<string, object>;
						Vector3 vector2 = (Vector3)dictionary["final"];
						GameObject gameObject3 = dictionary["go"] as GameObject;
						int setPosNum;
						if (vector2.ToString() == gameObject3.transform.position.ToString())
						{
							setPosNum = setPosNum;
							setPosNum++;
						}
						if (!hasGroupTweensCheckStarted)
						{
							hasGroupTweensCheckStarted = true;
							GameObject gameObject4 = this.gameObject;
							float delayTime = 0.1f;
							Action callback;
							if ((callback = <>9__16) == null)
							{
								callback = (<>9__16 = delegate()
								{
									LeanTest.expect(setOnStartNum == this.groupTweens.Length, "SETONSTART CALLS", string.Concat(new object[]
									{
										"expected:",
										this.groupTweens.Length,
										" was:",
										setOnStartNum
									}));
									LeanTest.expect(this.groupTweensCnt == this.groupTweens.Length, "GROUP FINISH", string.Concat(new object[]
									{
										"expected ",
										this.groupTweens.Length,
										" tweens but got ",
										this.groupTweensCnt
									}));
									LeanTest.expect(setPosNum == this.groupTweens.Length, "GROUP POSITION FINISH", string.Concat(new object[]
									{
										"expected ",
										this.groupTweens.Length,
										" tweens but got ",
										setPosNum
									}));
									LeanTest.expect(setPosOnUpdate, "GROUP POSITION ON UPDATE", null);
								});
							}
							LeanTween.delayedCall(gameObject4, delayTime, callback);
						}
						this.groupTweensCnt++;
					});
				}
				array3[num] = ltdescr3.setOnComplete(onComplete);
				if (LeanTween.description(this.groupTweens[j].id).trans == this.groupTweens[j].trans)
				{
					int k = descriptionMatchCount;
					descriptionMatchCount = k + 1;
				}
			}
			while (LeanTween.tweensRunning < this.groupTweens.Length)
			{
				yield return null;
			}
			LeanTest.expect(descriptionMatchCount == this.groupTweens.Length, "GROUP IDS MATCH", null);
			int num2 = this.groupTweens.Length + 7;
			LeanTest.expect(LeanTween.maxSearch <= num2, "MAX SEARCH OPTIMIZED", string.Concat(new object[]
			{
				"maxSearch:",
				LeanTween.maxSearch,
				" should be:",
				num2
			}));
			LeanTest.expect(LeanTween.isTweening(null), "SOMETHING IS TWEENING", null);
			float previousXlt4 = this.cube4.transform.position.x;
			this.lt4 = LeanTween.moveX(this.cube4, 5f, 1.1f).setOnComplete(delegate()
			{
				LeanTest.expect(this.cube4 != null && previousXlt4 != this.cube4.transform.position.x, "RESUME OUT OF ORDER", string.Concat(new object[]
				{
					"cube4:",
					this.cube4,
					" previousXlt4:",
					previousXlt4,
					" cube4.transform.position.x:",
					(this.cube4 != null) ? this.cube4.transform.position.x : 0f
				}));
			}).setDestroyOnComplete(true);
			this.lt4.resume();
			this.rotateRepeat = (this.rotateRepeatAngle = 0);
			LeanTween.rotateAround(this.cube3, Vector3.forward, 360f, 0.1f).setRepeat(3).setOnComplete(new Action(this.rotateRepeatFinished)).setOnCompleteOnRepeat(true).setDestroyOnComplete(true);
			yield return new WaitForEndOfFrame();
			LeanTween.delayedCall(1.8f, new Action(this.rotateRepeatAllFinished));
			int tweensRunning = LeanTween.tweensRunning;
			LeanTween.cancel(this.lt1Id);
			LeanTest.expect(tweensRunning == LeanTween.tweensRunning, "CANCEL AFTER RESET SHOULD FAIL", string.Concat(new object[]
			{
				"expected ",
				tweensRunning,
				" but got ",
				LeanTween.tweensRunning
			}));
			LeanTween.cancel(this.cube2);
			int num3 = 0;
			for (int l = 0; l < this.groupTweens.Length; l++)
			{
				if (LeanTween.isTweening(this.groupGOs[l]))
				{
					num3++;
				}
				if (l % 3 == 0)
				{
					LeanTween.pause(this.groupGOs[l]);
				}
				else if (l % 3 == 1)
				{
					this.groupTweens[l].pause();
				}
				else
				{
					LeanTween.pause(this.groupTweens[l].id);
				}
			}
			LeanTest.expect(num3 == this.groupTweens.Length, "GROUP ISTWEENING", string.Concat(new object[]
			{
				"expected ",
				this.groupTweens.Length,
				" tweens but got ",
				num3
			}));
			yield return new WaitForEndOfFrame();
			num3 = 0;
			for (int m = 0; m < this.groupTweens.Length; m++)
			{
				if (m % 3 == 0)
				{
					LeanTween.resume(this.groupGOs[m]);
				}
				else if (m % 3 == 1)
				{
					this.groupTweens[m].resume();
				}
				else
				{
					LeanTween.resume(this.groupTweens[m].id);
				}
				if ((m % 2 == 0) ? LeanTween.isTweening(this.groupTweens[m].id) : LeanTween.isTweening(this.groupGOs[m]))
				{
					num3++;
				}
			}
			LeanTest.expect(num3 == this.groupTweens.Length, "GROUP RESUME", null);
			LeanTest.expect(!LeanTween.isTweening(this.cube1), "CANCEL TWEEN LTDESCR", null);
			LeanTest.expect(!LeanTween.isTweening(this.cube2), "CANCEL TWEEN LEANTWEEN", null);
			LeanTest.expect(pauseCount == 0, "ON UPDATE NOT CALLED DURING PAUSE", "expect pause count of 0, but got " + pauseCount);
			yield return new WaitForEndOfFrame();
			Time.timeScale = 0.25f;
			float num4 = 0.2f;
			float expectedTime = num4 * (1f / Time.timeScale);
			float start = Time.realtimeSinceStartup;
			bool onUpdateWasCalled = false;
			LeanTween.moveX(this.cube1, -5f, num4).setOnUpdate(delegate(float val)
			{
				onUpdateWasCalled = true;
			}).setOnComplete(delegate()
			{
				float num6 = Time.realtimeSinceStartup - start;
				LeanTest.expect(Mathf.Abs(expectedTime - num6) < 0.06f, "SCALED TIMING DIFFERENCE", string.Concat(new object[]
				{
					"expected to complete in roughly ",
					expectedTime,
					" but completed in ",
					num6
				}));
				LeanTest.expect(Mathf.Approximately(this.cube1.transform.position.x, -5f), "SCALED ENDING POSITION", "expected to end at -5f, but it ended at " + this.cube1.transform.position.x);
				LeanTest.expect(onUpdateWasCalled, "ON UPDATE FIRED", null);
			});
			bool didGetCorrectOnUpdate = false;
			LeanTween.value(base.gameObject, new Vector3(1f, 1f, 1f), new Vector3(10f, 10f, 10f), 1f).setOnUpdate(delegate(Vector3 val)
			{
				didGetCorrectOnUpdate = (val.x >= 1f && val.y >= 1f && val.z >= 1f);
			}, null).setOnComplete(delegate()
			{
				LeanTest.expect(didGetCorrectOnUpdate, "VECTOR3 CALLBACK CALLED", null);
			});
			yield return new WaitForSeconds(expectedTime);
			Time.timeScale = 1f;
			int num5 = 0;
			GameObject[] array4 = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
			for (int k = 0; k < array4.Length; k++)
			{
				if (array4[k].name == "~LeanTween")
				{
					num5++;
				}
			}
			LeanTest.expect(num5 == 1, "RESET CORRECTLY CLEANS UP", null);
			base.StartCoroutine(this.lotsOfCancels());
			yield break;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00031153 File Offset: 0x0002F353
		private IEnumerator lotsOfCancels()
		{
			yield return new WaitForEndOfFrame();
			Time.timeScale = 4f;
			int cubeCount = 10;
			int[] tweensA = new int[cubeCount];
			GameObject[] aGOs = new GameObject[cubeCount];
			for (int i = 0; i < aGOs.Length; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.boxNoCollider);
				gameObject.transform.position = new Vector3(0f, 0f, (float)i * 2f);
				gameObject.name = "a" + i;
				aGOs[i] = gameObject;
				tweensA[i] = LeanTween.move(gameObject, gameObject.transform.position + new Vector3(10f, 0f, 0f), 0.5f + 1f * (1f / (float)aGOs.Length)).id;
				LeanTween.color(gameObject, Color.red, 0.01f);
			}
			yield return new WaitForSeconds(1f);
			int[] tweensB = new int[cubeCount];
			GameObject[] bGOs = new GameObject[cubeCount];
			for (int j = 0; j < bGOs.Length; j++)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.boxNoCollider);
				gameObject2.transform.position = new Vector3(0f, 0f, (float)j * 2f);
				gameObject2.name = "b" + j;
				bGOs[j] = gameObject2;
				tweensB[j] = LeanTween.move(gameObject2, gameObject2.transform.position + new Vector3(10f, 0f, 0f), 2f).id;
			}
			for (int k = 0; k < aGOs.Length; k++)
			{
				LeanTween.cancel(aGOs[k]);
				GameObject gameObject3 = aGOs[k];
				tweensA[k] = LeanTween.move(gameObject3, new Vector3(0f, 0f, (float)k * 2f), 2f).id;
			}
			yield return new WaitForSeconds(0.5f);
			for (int l = 0; l < aGOs.Length; l++)
			{
				LeanTween.cancel(aGOs[l]);
				GameObject gameObject4 = aGOs[l];
				tweensA[l] = LeanTween.move(gameObject4, new Vector3(0f, 0f, (float)l * 2f) + new Vector3(10f, 0f, 0f), 2f).id;
			}
			for (int m = 0; m < bGOs.Length; m++)
			{
				LeanTween.cancel(bGOs[m]);
				GameObject gameObject5 = bGOs[m];
				tweensB[m] = LeanTween.move(gameObject5, new Vector3(0f, 0f, (float)m * 2f), 2f).id;
			}
			yield return new WaitForSeconds(2.1f);
			bool didPass = true;
			for (int n = 0; n < aGOs.Length; n++)
			{
				if (Vector3.Distance(aGOs[n].transform.position, new Vector3(0f, 0f, (float)n * 2f) + new Vector3(10f, 0f, 0f)) > 0.1f)
				{
					didPass = false;
				}
			}
			for (int num = 0; num < bGOs.Length; num++)
			{
				if (Vector3.Distance(bGOs[num].transform.position, new Vector3(0f, 0f, (float)num * 2f)) > 0.1f)
				{
					didPass = false;
				}
			}
			LeanTest.expect(didPass, "AFTER LOTS OF CANCELS", null);
			this.cubeNamed("cPaused").LeanMoveX(10f, 1f).setOnComplete(delegate()
			{
				this.pauseTweenDidFinish = true;
			});
			base.StartCoroutine(this.pauseTimeNow());
			yield break;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00031162 File Offset: 0x0002F362
		private IEnumerator pauseTimeNow()
		{
			yield return new WaitForSeconds(0.5f);
			Time.timeScale = 0f;
			LeanTween.delayedCall(0.5f, delegate()
			{
				Time.timeScale = 1f;
			}).setUseEstimatedTime(true);
			LeanTween.delayedCall(1.5f, delegate()
			{
				LeanTest.expect(this.pauseTweenDidFinish, "PAUSE BY TIMESCALE FINISHES", null);
			}).setUseEstimatedTime(true);
			yield break;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00031171 File Offset: 0x0002F371
		private void rotateRepeatFinished()
		{
			if (Mathf.Abs(this.cube3.transform.eulerAngles.z) < 0.0001f)
			{
				this.rotateRepeatAngle++;
			}
			this.rotateRepeat++;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000311B0 File Offset: 0x0002F3B0
		private void rotateRepeatAllFinished()
		{
			LeanTest.expect(this.rotateRepeatAngle == 3, "ROTATE AROUND MULTIPLE", "expected 3 times received " + this.rotateRepeatAngle + " times");
			LeanTest.expect(this.rotateRepeat == 3, "ROTATE REPEAT", "expected 3 times received " + this.rotateRepeat + " times");
			LeanTest.expect(this.cube3 == null, "DESTROY ON COMPLETE", "cube3:" + this.cube3);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0003123D File Offset: 0x0002F43D
		private void eventGameObjectCalled(LTEvent e)
		{
			this.eventGameObjectWasCalled = true;
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00031246 File Offset: 0x0002F446
		private void eventGeneralCalled(LTEvent e)
		{
			this.eventGeneralWasCalled = true;
		}

		// Token: 0x040009B3 RID: 2483
		public GameObject cube1;

		// Token: 0x040009B4 RID: 2484
		public GameObject cube2;

		// Token: 0x040009B5 RID: 2485
		public GameObject cube3;

		// Token: 0x040009B6 RID: 2486
		public GameObject cube4;

		// Token: 0x040009B7 RID: 2487
		public GameObject cubeAlpha1;

		// Token: 0x040009B8 RID: 2488
		public GameObject cubeAlpha2;

		// Token: 0x040009B9 RID: 2489
		private bool eventGameObjectWasCalled;

		// Token: 0x040009BA RID: 2490
		private bool eventGeneralWasCalled;

		// Token: 0x040009BB RID: 2491
		private int lt1Id;

		// Token: 0x040009BC RID: 2492
		private LTDescr lt2;

		// Token: 0x040009BD RID: 2493
		private LTDescr lt3;

		// Token: 0x040009BE RID: 2494
		private LTDescr lt4;

		// Token: 0x040009BF RID: 2495
		private LTDescr[] groupTweens;

		// Token: 0x040009C0 RID: 2496
		private GameObject[] groupGOs;

		// Token: 0x040009C1 RID: 2497
		private int groupTweensCnt;

		// Token: 0x040009C2 RID: 2498
		private int rotateRepeat;

		// Token: 0x040009C3 RID: 2499
		private int rotateRepeatAngle;

		// Token: 0x040009C4 RID: 2500
		private GameObject boxNoCollider;

		// Token: 0x040009C5 RID: 2501
		private float timeElapsedNormalTimeScale;

		// Token: 0x040009C6 RID: 2502
		private float timeElapsedIgnoreTimeScale;

		// Token: 0x040009C7 RID: 2503
		private bool pauseTweenDidFinish;
	}
}
