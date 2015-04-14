using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class SplitScreenCameraController : MonoBehaviour 
	{
		private Transform m_target;
		private Camera m_camera;

		public Vector2 Offset { get; set; }
		public Camera Camera 
		{ 
			get { return m_camera; }
		}

		private void Awake()
		{
			m_camera = GetComponent<Camera>();
		}

		private void Update()
		{
			if(m_target != null)
			{
				Vector3 position = m_target.transform.position;
				position.x += Offset.x;
				position.y += Offset.y;
				position.z = transform.position.z;

				transform.position = position;
			}
		}

		public void SetTarget(Transform target)
		{
			m_target = target;
		}
	}
}