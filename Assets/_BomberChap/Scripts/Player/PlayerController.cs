using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(CharacterMotor))]
	[RequireComponent(typeof(PlayerControls))]
	[RequireComponent(typeof(PlayerAnimatorParameters))]
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private Animator m_animator;

		[SerializeField]
		private AudioClip m_hurtSound;

		private CharacterMotor m_motor;
		private PlayerControls m_controls;
		private PlayerAnimatorParameters m_animParam;
		private Level m_currentLevel;
		private int m_lastHDir;
		private int m_lastVDir;

		public Camera Camera { get; set; }

		private void Start()
		{
			m_motor = GetComponent<CharacterMotor>();
			m_controls = GetComponent<PlayerControls>();
			m_animParam = GetComponent<PlayerAnimatorParameters>();
			m_currentLevel = LevelManager.GetLoadedLevel();
			m_lastHDir = 0;
			m_lastVDir = 0;
		}

		private void Update()
		{
			if(m_motor.IsAtDestination && m_currentLevel != null)
			{
				HandleLocomotionInput();
				HandleActionInput();
			}
		}

		private void HandleLocomotionInput()
		{
			Vector2 tilePos = m_currentLevel.WorldToTile(transform.position);
			int h = 0, v = 0;

			if(Input.GetButton(m_controls.MoveUpButton))
				v = 1;
			else if(Input.GetButton(m_controls.MoveDownButton))
				v = -1;
			else if(Input.GetButton(m_controls.MoveRightButton))
				h = 1;
			else if(Input.GetButton(m_controls.MoveLeftButton))
				h = -1;

			if(v > 0) 
			{
				Tile tile = m_currentLevel.GetAt((int)tilePos.x, (int)tilePos.y - 1);
				if(tile != null && !tile.IsSolid)
				{
					m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x, (int)tilePos.y - 1, transform.position.z));
					if(v != m_lastVDir)
						m_animator.SetTrigger(m_animParam.StartMoveUp);
					SendMessage("OnPlayerChangedDestination", new object[] { m_motor.Destination, h, v, m_lastHDir, m_lastVDir }, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					v = 0;
				}
			}
			else if(v < 0) 
			{
				Tile tile = m_currentLevel.GetAt((int)tilePos.x, (int)tilePos.y + 1);
				if(tile != null && !tile.IsSolid)
				{
					m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x, (int)tilePos.y + 1, transform.position.z));
					if(v != m_lastVDir)
						m_animator.SetTrigger(m_animParam.StartMoveDown);
					SendMessage("OnPlayerChangedDestination", new object[] { m_motor.Destination, h, v, m_lastHDir, m_lastVDir }, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					v = 0;
				}
			}
			else if(h > 0) 
			{
				Tile tile = m_currentLevel.GetAt((int)tilePos.x + 1, (int)tilePos.y);
				if(tile != null && !tile.IsSolid)
				{
					m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x + 1, (int)tilePos.y, transform.position.z));
					if(h != m_lastHDir)
						m_animator.SetTrigger(m_animParam.StartMoveRight);
					SendMessage("OnPlayerChangedDestination", new object[] { m_motor.Destination, h, v, m_lastHDir, m_lastVDir }, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					h = 0;
				}
			}
			else if(h < 0) 
			{

				Tile tile = m_currentLevel.GetAt((int)tilePos.x - 1, (int)tilePos.y);
				if(tile != null && !tile.IsSolid)
				{
					m_motor.SetDestination(m_currentLevel.TileToWorld((int)tilePos.x - 1, (int)tilePos.y, transform.position.z));
					if(h != m_lastHDir)
						m_animator.SetTrigger(m_animParam.StartMoveLeft);
					SendMessage("OnPlayerChangedDestination", new object[] { m_motor.Destination, h, v, m_lastHDir, m_lastVDir }, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					h = 0;
				}
			}

			m_lastHDir = h;
			m_lastVDir = v;

			m_animator.SetInteger(m_animParam.MoveHorizontal, h);
			m_animator.SetInteger(m_animParam.MoveVertical, v);
		}

		private void HandleActionInput()
		{
			if(Input.GetButtonDown(m_controls.DropBombButton))
				SendMessage("DropBomb");
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.tag == Tags.Flame || other.tag == Tags.Enemy)
			{
				AudioManager.PlaySound(m_hurtSound);
				m_motor.enabled = false;
				m_animator.SetInteger(m_animParam.MoveHorizontal, 0);
				m_animator.SetInteger(m_animParam.MoveVertical, 0);
				this.enabled = false;
			}
		}
	}
}