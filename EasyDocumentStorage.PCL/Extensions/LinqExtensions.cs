﻿using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
	internal static class LinqExtensions
	{

        static Random _rnd = new Random();

		/// <summary>
		/// Counterpart of IEnumerable.Any()
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerable"></param>
		/// <returns></returns>
		internal static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			return !enumerable.Any();
		}

        /// <summary>
        /// Same as IsEmpty()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
		internal static bool None<T>(this IEnumerable<T> enumerable)
		{
			return !enumerable.Any();
		}


		internal static void ForEach(this IEnumerable enumerable, Action<object> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}

		internal static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}

	    internal static void Times(this int number, Action<int> action)
	    {
	        for (var i = 0; i < number; i++)
	        {
	            action(i);
	        }
	    }

		/// <summary>
		/// Get a random element from a collection
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		internal static TResult Random<TResult>(this IEnumerable<TResult> source)
		{
			var enumerable = source as TResult[] ?? source.ToArray();

			if (enumerable.IsEmpty())
				return default(TResult);

			var rnd = new Random(DateTime.Now.Millisecond);

			var max = enumerable.Count() - 1;

			var element = enumerable.ElementAt(rnd.Next(0, max));

			return element;

		}


	
	}
}