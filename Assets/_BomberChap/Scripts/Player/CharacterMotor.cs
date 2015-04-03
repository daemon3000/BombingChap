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
		public bool IsAtDestination { get; private set; }
		public float Speed
		{
			get { return m_speed; }
			set { m_speed = Math.Max(value, 0.0f); }
		}

		private void Awake()
		{
			IsAtDestination = true;
		}

		private void Update()
		{
			if(!IsAtDestination)
			{
				Vector3 position = transform.position;
				Vector3 motion = m_destination - transform.position;
				motion = motion.normalized * m_speed * Time.deltaTime;
				position += motion;

				if(motion == Vector3.zero)
				{
					IsAtDestination = true;
				}
				else if((motion.y > 0.0f && position.y > m_destination.y) || (motion.y < 0.0f && position.y < m_destination.y))
				{
					position.y = m_destination.y;
					IsAtDestination = true;
				}
				else if((motion.x > 0.0f && position.x > m_destination.x) || (motion.x < 0.0f && position.x < m_destination.x))
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