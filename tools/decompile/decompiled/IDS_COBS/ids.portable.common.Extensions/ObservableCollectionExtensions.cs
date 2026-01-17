using System;
using System.Collections.ObjectModel;

namespace IDS.Portable.Common.Extensions;

public static class ObservableCollectionExtensions
{
	public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
	{
		for (int num = ((Collection<T>)(object)collection).Count - 1; num >= 0; num--)
		{
			if (condition.Invoke(((Collection<T>)(object)collection)[num]))
			{
				((Collection<T>)(object)collection).RemoveAt(num);
			}
		}
	}
}
