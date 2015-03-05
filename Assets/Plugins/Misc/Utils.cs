using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public static class Utils
	{
		public static void Shuffle(int[] array) 
		{
			int count = array.Length;
			for (int i = count; i > 1; i--) 
			{
				Swap(array, i - 1, UnityEngine.Random.Range(0, i));
			}
		}
		
		private static void Swap(int[] array, int i, int j) 
		{
			int temp = array[i];
			array[i] = array[j];
			array[j] = temp;
		}
	}
}