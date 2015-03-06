using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class PlayerControls : MonoBehaviour 
	{
		[SerializeField]
		private string m_moveUpButton;
		[SerializeField]
		private string m_moveDownButton;
		[SerializeField]
		private string m_moveRightButton;
		[SerializeField]
		private string m_moveLeftButton;
		[SerializeField]
		private string m_dropBombButton;

		public string MoveUpButton { get { return m_moveUpButton; } }
		public string MoveDownButton { get { return m_moveDownButton; } }
		public string MoveRightButton { get { return m_moveRightButton; } }
		public string MoveLeftButton { get { return m_moveLeftButton; } }
		public string DropBombButton { get { return m_dropBombButton; } }
	}
}