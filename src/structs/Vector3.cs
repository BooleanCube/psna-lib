using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace psna_lib.structs;

[Serializable]
public struct Vector3
{
	private float x, y, z;

	public Vector3() : this(0f, 0f, 0f) {}
	
	public Vector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public float X
	{
		get { return this.x; }
		set { this.x = value; }
	}
	
	public float Y
	{
		get { return this.y; }
		set { this.y = value; }
	}
	
	public float Z
	{
		get { return this.z; }
		set { this.z = value; }
	}

	public static Vector3 Zero { get { return new Vector3(); } }
	public static Vector3 One { get { return new Vector3(1f, 1f, 1f); } }
	public static Vector3 UnitX { get { return new Vector3(1f, 0f, 0f); } }
	public static Vector3 UnitY { get { return new Vector3(0f, 1f, 0f); } }
	public static Vector3 UnitZ { get { return new Vector3(0f, 0f, 1f); } }
	
	private static int CombineHashCodes(int a, int b) { return (((a << 5) + a) ^ b); }
	public override int GetHashCode()
	{
		int hash = this.x.GetHashCode();
		hash = CombineHashCodes(hash, this.y.GetHashCode());
		hash = CombineHashCodes(hash, this.z.GetHashCode());
		return hash;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		if (!(obj is Vector3))
			return false;
		return Equals((Vector3)obj);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		StringBuilder sb = new StringBuilder();
		string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
		sb.Append('<');
		sb.Append(x.ToString(format, formatProvider)).Append(separator).Append(' ');
		sb.Append(y.ToString(format, formatProvider)).Append(separator).Append(' ');
		sb.Append(z.ToString(format, formatProvider));
		sb.Append('>');
		return sb.ToString();
	}
	
	public string ToString(string format)
	{
		return ToString(format, CultureInfo.CurrentCulture);
	}
	
	public override string ToString()
	{
		return ToString("G", CultureInfo.CurrentCulture);
	}
}