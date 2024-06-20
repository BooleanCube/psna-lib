using System.Text;
using System.Text.Json;

namespace psna_lib.utils;

public class ContentHelper
{
	public static byte[] EncryptContent(dynamic value)
	{
		string serializedString = JsonSerializer.Serialize(value);
		byte[] encodedStruct = Encoding.ASCII.GetBytes(serializedString);

		return encodedStruct;
	}

	public static dynamic? DecryptContent<T>(byte[] buffer)
	{
		string decodedString = Encoding.ASCII.GetString(buffer);
		T? deserializedStruct = JsonSerializer.Deserialize<T>(decodedString);

		return deserializedStruct;
	}

	public static byte[] CopyFrom(byte[] arr, int startIdx)
	{
		byte[] value = new byte[arr.Length - startIdx];
		for (int i = 0; i < value.Length; i++)
		{
			value[i] = arr[i + startIdx];
		}

		return value;
	}
	
	public static byte[] CopyTo(byte[] arr, int startIdx)
	{
		byte[] value = new byte[arr.Length + startIdx];
		for (int i = startIdx; i < value.Length; i++)
		{
			value[i] = arr[i - startIdx];
		}

		return value;
	}

	public static byte[] MarkBuffer(byte[] arr, byte mId, byte tId)
	{
		byte[] buffer = CopyTo(arr, 2);
		buffer[0] = mId;
		buffer[1] = tId;

		return buffer;
	}
}