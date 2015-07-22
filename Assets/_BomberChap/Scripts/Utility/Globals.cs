using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace BomberChap
{
	public static class Globals
	{
		private static Dictionary<string, object> m_data = new Dictionary<string, object>();

		private static void SetObject(string key, object value)
		{
			if(m_data != null)
			{
				if(m_data.ContainsKey(key))
					m_data[key] = value;
				else
					m_data.Add(key, value);
			}
		}
		
		public static void SetString(string key, string value)
		{
			SetObject(key, value);
		}
		
		public static string GetString(string key, string defaultValue)
		{
			if(m_data != null)
			{
				object value = null;
				if(m_data.TryGetValue(key, out value) && value is string)
					return (string)value;
				
				return defaultValue;
			}
			else
			{
				return defaultValue;
			}
		}
		
		public static void SetInt(string key, int value)
		{
			SetObject(key, value);
		}
		
		public static int GetInt(string key, int defaultValue)
		{
			if(m_data != null)
			{
				object value = null;
				if(m_data.TryGetValue(key, out value) && value is int)
					return (int)value;
				
				return defaultValue;
			}
			else
			{
				return defaultValue;
			}
		}
		
		public static void SetFloat(string key, float value)
		{
			SetObject(key, value);
		}
		
		public static float GetFloat(string key, float defaultValue)
		{
			if(m_data != null)
			{
				object value = null;
				if(m_data.TryGetValue(key, out value) && value is float)
					return (float)value;
				
				return defaultValue;
			}
			else
			{
				return defaultValue;
			}
		}
		
		public static void SetBool(string key, bool value)
		{
			SetObject(key, value);
		}
		
		public static bool GetBool(string key, bool defaultValue)
		{
			if(m_data != null)
			{
				object value = null;
				if(m_data.TryGetValue(key, out value) && value is bool)
					return (bool)value;
				
				return defaultValue;
			}
			else
			{
				return defaultValue;
			}
		}
		
		public static bool HasKey(string key)
		{
			if(m_data != null)
				return m_data.ContainsKey(key);
			else
				return false;
		}
		
		public static void DeleteAllKeys()
		{
			m_data.Clear();
		}
	}
}