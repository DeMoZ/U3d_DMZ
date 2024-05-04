using System.Collections.Generic;

namespace DMZ.Extensions
{
	public static class CollectionExtensions
	{
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			items.ForEach(collection.Add);
		}
	}
}