using UnityEngine;
using System.Collections;

namespace Nanawatai.UI
{
	public class GraphicIgnoreRaycast : MonoBehaviour, ICanvasRaycastFilter
	{
		public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			return false;
		}
	}
}