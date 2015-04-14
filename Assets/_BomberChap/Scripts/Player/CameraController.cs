using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(Camera))]
	public class CameraController : MonoBehaviour
	{
		private Transform m_target;
		private float m_levelWidth;
		private float m_levelHeight;
		private float m_viewWidth;
		private float m_viewHeight;
		private Camera m_camera;

		public Camera Camera 
		{ 
			get { return m_camera; }
		}

		private void Start()
		{
			Level currentLevel = LevelManager.GetLoadedLevel();
			m_levelWidth = currentLevel != null ? currentLevel.Width * currentLevel.TileWidth * currentLevel.PixelToUnit : 0;
			m_levelHeight = currentLevel != null ? currentLevel.Height * currentLevel.TileHeight * currentLevel.PixelToUnit : 0;

			m_camera = GetComponent<Camera>();
			m_viewHeight = m_camera.orthographicSize * 2;
			m_viewWidth = m_viewHeight * m_camera.aspect;
		}

		private void Update()
		{
			if(m_target == null)
				return;

			Vector3 position = transform.position;
			if(m_levelWidth > m_viewWidth)
				position.x = Mathf.Clamp(m_target.position.x, m_viewWidth / 2.0f, m_levelWidth - m_viewWidth / 2.0f);
			else
				position.x = m_levelWidth / 2.0f;
			if(m_levelHeight > m_viewHeight)
				position.y = Mathf.Clamp(m_target.position.y, -(m_levelHeight - m_viewHeight / 2), -m_viewHeight / 2.0f);
			else
				position.y = -m_levelHeight / 2.0f;

			transform.position = position;
		}

		public void SetTarget(Transform target)
		{
			m_target = target;
		}
	}
}