using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace IDS.Portable.Common.Utils;

public class ObjectPool<TObject>
{
	private readonly ConcurrentBag<TObject> _objects;

	private readonly Func<TObject> _objectGenerator;

	private int _totalCreated;

	public static ObjectPool<TObject> MakeObjectPool<TNewObject>() where TNewObject : TObject, new()
	{
		return new ObjectPool<TObject>(() => (TObject)(object)new TNewObject());
	}

	public static ObjectPool<TObject> MakeObjectPool(Func<TObject> objectGenerator)
	{
		return new ObjectPool<TObject>(objectGenerator);
	}

	private ObjectPool(Func<TObject> objectGenerator)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		_objects = new ConcurrentBag<TObject>();
		_objectGenerator = objectGenerator ?? throw new ArgumentNullException("objectGenerator");
	}

	public TObject TakeObject()
	{
		TObject result = default(TObject);
		if (!_objects.TryTake(ref result))
		{
			_totalCreated++;
			return _objectGenerator.Invoke();
		}
		return result;
	}

	public void PutObject(TObject item)
	{
		if (item != null)
		{
			_objects.Add(item);
		}
	}

	public virtual string ToString()
	{
		return $"ObjectPool<{((MemberInfo)typeof(TObject)).Name}>: Total Created {_totalCreated} Total Unused {_objects.Count}";
	}
}
