using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace IDS.Portable.Common.Manifest;

[JsonObject(/*Could not decode attribute arguments.*/)]
public class Manifest : IManifest, global::System.Collections.Generic.IEnumerable<IManifestProduct>, global::System.Collections.IEnumerable
{
	[JsonProperty(PropertyName = "ProductList")]
	private List<IManifestProduct> _productList;

	[JsonProperty(PropertyName = "RVConfig")]
	[field: CompilerGenerated]
	public IMainfestRvConfig RvConfig
	{
		[CompilerGenerated]
		get;
	}

	public global::System.Collections.Generic.IEnumerable<IManifestProduct> Products => (global::System.Collections.Generic.IEnumerable<IManifestProduct>)_productList;

	public Manifest()
	{
		_productList = new List<IManifestProduct>();
		RvConfig = new MainfestRvConfig();
	}

	[JsonConstructor]
	public Manifest(List<ManifestProduct> productList, MainfestRvConfig rvConfig)
		: this()
	{
		_productList.AddRange((global::System.Collections.Generic.IEnumerable<IManifestProduct>)productList);
		RvConfig = rvConfig;
	}

	public IManifestProduct AddProduct(IManifestProduct product, bool updateSoftwarePartNumber)
	{
		IManifestProduct manifestProduct = FindProduct(product.UniqueID);
		if (manifestProduct != null)
		{
			if (updateSoftwarePartNumber && string.IsNullOrEmpty(manifestProduct.SoftwarePartNumber) && !string.IsNullOrEmpty(product.SoftwarePartNumber))
			{
				manifestProduct.SoftwarePartNumber = product.SoftwarePartNumber;
			}
			return manifestProduct;
		}
		_productList.Add(product);
		return product;
	}

	public IManifestProduct? FindProduct(string uniqueProductId)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (uniqueProductId == null)
		{
			return null;
		}
		Enumerator<IManifestProduct> enumerator = _productList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IManifestProduct current = enumerator.Current;
				if (current.UniqueID == uniqueProductId)
				{
					return current;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
		return null;
	}

	public string ToJSON()
	{
		return JsonConvert.SerializeObject((object)this, (Formatting)1);
	}

	public static Manifest MakeManifestFromJSON(string json)
	{
		try
		{
			return JsonConvert.DeserializeObject<Manifest>(json);
		}
		catch (global::System.Exception ex)
		{
			throw new global::System.Exception("MakeManifestFromJSON error processing JSON", ex);
		}
	}

	public virtual string ToString()
	{
		try
		{
			return ToJSON();
		}
		catch
		{
			return base.ToString();
		}
	}

	public string MakeProductBlueprintId()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<ushort, byte> val = new Dictionary<ushort, byte>();
		Enumerator<IManifestProduct> enumerator = _productList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IManifestProduct current = enumerator.Current;
				byte b = ((IReadOnlyDictionary<ushort, byte>)(object)val).TryGetValue<ushort, byte>(current.TypeID);
				b++;
				val[current.TypeID] = b;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
		StringBuilder val2 = new StringBuilder();
		global::System.Collections.Generic.IEnumerator<KeyValuePair<ushort, byte>> enumerator2 = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<ushort, byte>>)Enumerable.OrderBy<KeyValuePair<ushort, byte>, ushort>((global::System.Collections.Generic.IEnumerable<KeyValuePair<ushort, byte>>)val, (Func<KeyValuePair<ushort, byte>, ushort>)((KeyValuePair<ushort, byte> i) => i.Key))).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator2).MoveNext())
			{
				KeyValuePair<ushort, byte> current2 = enumerator2.Current;
				StringBuilder val3 = val2;
				AppendInterpolatedStringHandler val4 = new AppendInterpolatedStringHandler(0, 2, val3);
				((AppendInterpolatedStringHandler)(ref val4)).AppendFormatted<byte>(current2.Value, "X2");
				((AppendInterpolatedStringHandler)(ref val4)).AppendFormatted<ushort>(current2.Key, "X2");
				val3.Append(ref val4);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator2)?.Dispose();
		}
		return $"BL{val2}";
	}

	public global::System.Collections.Generic.IEnumerator<IManifestProduct> GetEnumerator()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IEnumerator<IManifestProduct>)(object)_productList.GetEnumerator();
	}

	global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
	{
		return (global::System.Collections.IEnumerator)GetEnumerator();
	}
}
