using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
	public static class LinqExtensions
	{

		/// <summary>
		/// Counterpart of IEnumerable.Any()
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerable"></param>
		/// <returns></returns>
		public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			return !enumerable.Any();
		}

        /// <summary>
        /// Same as IsEmpty()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
		public static bool None<T>(this IEnumerable<T> enumerable)
		{
			return !enumerable.Any();
		}


		public static void ForEach(this IEnumerable enumerable, Action<object> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}

		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}

		/// <summary>
		/// Get a random element from a collection
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static TResult Random<TResult>(this IEnumerable<TResult> source)
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