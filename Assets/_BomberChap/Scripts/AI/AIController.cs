using UnityEngine;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(CharacterMotor))]
	[RequireComponent(typeof(AIAnimatorParameters))]
	public class AIController : MonoBehaviour 
	{
		private const int DIR_UP = 0;
		private const int DIR_DOWN = 1;
		private const int DIR_RIGHT = 2;
		private const int DIR_LEFT = 3;
		
		[SerializeField]
		private Animator m_animator;
		
		private CharacterMotor m_motor;
		private Level m_currentLevel;
		private AIAnimatorParameters m_animParam;
		private int m_lastHDir;
		private int m_lastVDir;
		private int[] m_randDir;

		private void Start()
		{
			m_motor = GetComponent<CharacterMotor>();
			m_animParam = GetComponent<AIAnimatorParameters>();
			m_currentLevel = LevelManager.GetLoadedLevel();
			m_lastHDir = 0;
			m_lastVDir = 0;
			m_randDir = new int[4];
			for(int i = 0; i < m_randDir.Length; i++)
				m_randDir[i] = i;
		}

		private void Update()
		{
			if(!m_motor.IsAtDestination || m_currentLevel == null)
				return;

			Vector2 tilePos = m_currentLevel.WorldToTile(transform.position);
			int h = m_lastHDir, v = m_lastVDir;

			if((h == 0 && v == 0) || !CanMoveInDirection((int)tilePos.x, (int)tilePos.y, h, v))
				ChooseNewMoveDirection((int)tilePos.x, (int)tilePos.y, out h, out v);

			if(v > 0) 
			{
				m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x, (int)tilePos.y - 1, transform.position.z));
				if(v != m_lastVDir)
					m_animator.SetTrigger(m_animParam.StartMoveUp);
			}
			else if(v < 0) 
			{
				m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x, (int)tilePos.y + 1, transform.position.z));
				if(v != m_lastVDir)
					m_animator.SetTrigger(m_animParam.StartMoveDown);
			}
			else if(h > 0) 
			{
				m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x + 1, (int)tilePos.y, transform.position.z));
				if(h != m_lastHDir)
					m_animator.SetTrigger(m_animParam.StartMoveRight);
			}
			else if(h < 0) 
			{
				m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x - 1, (int)tilePos.y, transform.position.z));
				if(h != m_lastHDir)
					m_animator.SetTrigger(m_animParam.StartMoveLeft);
			}

			m_lastHDir = h;
			m_lastVDir = v;
			m_animator.SetInteger(m_animParam.MoveHorizontal, h);
			m_animator.SetInteger(m_animParam.MoveVertical, v);
		}

		private bool CanMoveInDirection(int c, int r, int hDir, int vDir) 
		{
			Tile tile = m_currentLevel.GetAt(c + hDir, r - vDir);
			return tile != null && !tile.IsSolid;
		}
		
		private void ChooseNewMoveDirection(int c, int r, out int hDir, out int vDir) 
		{
			Utils.Shuffle(m_randDir);
			
			hDir = vDir = 0;
			for(int i = 0; i < m_randDir.Length; i++) {
				switch(m_randDir[i]) {
				case DIR_UP:
					if(CanMoveInDirection(c, r, 0, 1)) 
					{
						vDir = 1;
						return;
					}
					break;
				case DIR_DOWN:
					if(CanMoveInDirection(c, r, 0, -1)) 
					{
						vDir = -1;
						return;
					}
					break;
				case DIR_RIGHT:
					if(CanMoveInDirection(c, r, 1, 0)) 
					{
						hDir = 1;
						return;
					}
					break;
				case DIR_LEFT:
					if(CanMoveInDirection(c, r, -1, 0)) 
					{
						hDir = -1;
						return;
					}
					break;
				default:
					break;
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.tag == Tags.Flame)
			{
				NotificationCenter.Dispatch(Notifications.ON_ENEMY_DEAD);
				GameObject.Destroy(gameObject);
			}
		}
	}
}