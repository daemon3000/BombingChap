using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class CameraShake : MonoBehaviour 
	{
		[SerializeField] private float m_shakeIntensity;
		[SerializeField] private float m_shakeDuration;
		[SerializeField] private float m_shakeDistance;
		[SerializeField] private float m_shakeDecay;

		private float m_elapsedTime;
		private float m_duration;
		private float m_shakeProgress;
		private float m_currentShakeDistance;
		private bool m_isAnimating;
		
		private void Awake()
		{
			m_elapsedTime = 0.0f;
			m_shakeProgress = 0.0f;
			m_currentShakeDistance = 0.0f;
			m_isAnimating = false;
		}
		
		private void LateUpdate()
		{
			if(!m_isAnimating)
				return;
			
			m_elapsedTime += Time.deltaTime;
			if(m_elapsedTime <= m_duration)
			{
				m_shakeProgress += Time.deltaTime * m_shakeIntensity;
				if(m_shakeProgress > Mathf.PI * 2)
				{
					m_shakeProgress = 0.0f;
					m_currentShakeDistance *= m_shakeDecay;
				}
				transform.localPosition += transform.right * Mathf.Sin(m_shakeProgress) * m_currentShakeDistance;
			}
			else
			{
				m_isAnimating = false;
			}
		}
		
		public void ShakeCamera()
		{
			m_elapsedTime = 0.0f;
			m_duration = m_shakeDuration;
			m_shakeProgress = 0.0f;
			m_currentShakeDistance = m_shakeDistance;
			m_isAnimating = true;
		}
	}
}