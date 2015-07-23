using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class CharacterMotor : MonoBehaviour
	{
		[SerializeField]
		private float m_speed;

		private Vector3 m_destination;
		private Vector3 m_velocity = Vector3.zero;

		public Vector3 Destination
		{
			get { return m_destination; }
		}

		public Vector3 Velocity
		{
			get { return m_velocity; }
		}

		public float Speed
		{
			get { return m_speed; }
			set { m_speed = Math.Max(value, 0.0f); }
		}

		public bool IsAtDestination { get; private set; }

		private void Start()
		{
			m_destination = transform.position;
			IsAtDestination = true;
		}

		private void Update()
		{
			m_velocity.Set(0, 0, 0);

			if(!IsAtDestination)
			{
				Vector3 position = transform.position;
				m_velocity = m_destination - transform.position;
				m_velocity = m_velocity.normalized * m_speed;
				position += m_velocity * Time.deltaTime;

				if(m_velocity == Vector3.zero)
				{
					IsAtDestination = true;
				}
				else if((m_velocity.y > 0.0f && position.y > m_destination.y) || (m_velocity.y < 0.0f && position.y < m_destination.y))
				{
					position.y = m_destination.y;
					IsAtDestination = true;
				}
				else if((m_velocity.x > 0.0f && position.x > m_destination.x) || (m_velocity.x < 0.0f && position.x < m_destination.x))
				{
					position.x = m_destination.x;
					IsAtDestination = true;
				}

				transform.position = position;
				if(IsAtDestination)
					SendMessage("OnCharacterIsAtDestination", SendMessageOptions.DontRequireReceiver);
			}
		}

		public void SetDestination(Vector3 destination)
		{
			m_destination = destination;
			IsAtDestination = false;
		}
	}
}