using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IDS.Portable.Common;

public static class Crypto
{
	public static string GetMd5HashString(string fromString)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		MD5CryptoServiceProvider val = new MD5CryptoServiceProvider();
		try
		{
			((HashAlgorithm)val).Initialize();
			byte[] array = ((HashAlgorithm)val).ComputeHash(Encoding.UTF8.GetBytes(fromString));
			((HashAlgorithm)val).Clear();
			return Encoding.UTF8.GetString(array);
		}
		finally
		{
			((global::System.IDisposable)val)?.Dispose();
		}
	}

	public static string Decrypt(this string encryptedString, string encryptionKey)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		if (string.IsNullOrEmpty(encryptedString))
		{
			return "";
		}
		byte[] bytes = Encoding.ASCII.GetBytes(encryptionKey);
		byte[] array = Convert.FromBase64String(encryptedString);
		DESCryptoServiceProvider val = new DESCryptoServiceProvider();
		try
		{
			ICryptoTransform val2 = ((SymmetricAlgorithm)val).CreateDecryptor(bytes, bytes);
			try
			{
				MemoryStream val3 = new MemoryStream(array);
				try
				{
					StreamReader val4 = new StreamReader((Stream)new CryptoStream((Stream)(object)val3, val2, (CryptoStreamMode)0));
					try
					{
						return ((TextReader)val4).ReadToEnd();
					}
					finally
					{
						((global::System.IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((global::System.IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((global::System.IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((global::System.IDisposable)val)?.Dispose();
		}
	}

	public static string Encrypt(this string originalString, string encryptionKey)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		if (string.IsNullOrEmpty(originalString))
		{
			return "";
		}
		byte[] bytes = Encoding.ASCII.GetBytes(encryptionKey);
		byte[] bytes2 = Encoding.ASCII.GetBytes(originalString);
		DESCryptoServiceProvider val = new DESCryptoServiceProvider();
		try
		{
			ICryptoTransform val2 = ((SymmetricAlgorithm)val).CreateEncryptor(bytes, bytes);
			try
			{
				MemoryStream val3 = new MemoryStream(bytes2);
				try
				{
					CryptoStream val4 = new CryptoStream((Stream)(object)val3, val2, (CryptoStreamMode)0);
					MemoryStream val5 = new MemoryStream();
					try
					{
						((Stream)val4).CopyTo((Stream)(object)val5);
						return Convert.ToBase64String(val5.ToArray());
					}
					finally
					{
						((global::System.IDisposable)val5)?.Dispose();
					}
				}
				finally
				{
					((global::System.IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((global::System.IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((global::System.IDisposable)val)?.Dispose();
		}
	}
}
