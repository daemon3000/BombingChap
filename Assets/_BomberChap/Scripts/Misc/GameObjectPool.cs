using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BomberChap
{
	public class GameObjectPool
	{
		private GameObject m_prefab;
		private Queue<GameObject> m_pool;

		public GameObjectPool(GameObject prefab)
		{
			m_prefab = prefab;
			m_pool = new Queue<GameObject>();
		}

		public void Free(GameObject instance)
		{
			instance.SendMessage("OnPooledInstanceReset");
			instance.SetActive(false);
			m_pool.Enqueue(instance);
		}

		public GameObject Get()
		{
			if(m_pool.Count > 0)
			{
				GameObject instance = m_pool.Dequeue();
				instance.SetActive(true);
				instance.SendMessage("OnPooledInstanceInitialize");
				return instance;
			}
			else
			{
				GameObject instance = GameObject.Instantiate(m_prefab) as GameObject;
				instance.SendMessage("OnPooledInstanceInitialize");
				return instance;
			}
		}

		public void Clear(bool destroyInstances)
		{
			if(destroyInstances)
			{
				while(m_pool.Count > 0)
				{
					GameObject go = m_pool.Dequeue();
					GameObject.Destroy(go);
				}
				m_pool.Clear();
			}
			else
			{
				m_pool.Clear();
			}
		}
	}
}