using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class PlayerAnimatorParameters : MonoBehaviour 
	{
		[SerializeField]
		private string m_moveVertical;
		[SerializeField]
		private string m_moveHorizontal;
		[SerializeField]
		private string m_startMoveUp;
		[SerializeField]
		private string m_startMoveDown;
		[SerializeField]
		private string m_startMoveRight;
		[SerializeField]
		private string m_startMoveLeft;
		
		public string MoveVertical { get { return m_moveVertical; } }
		public string MoveHorizontal { get { return m_moveHorizontal; } }
		public string StartMoveUp { get { return m_startMoveUp; } }
		public string StartMoveDown { get { return m_startMoveDown; } }
		public string StartMoveRight { get { return m_startMoveRight; } }
		public string StartMoveLeft { get { return m_startMoveLeft; } }
	}
}