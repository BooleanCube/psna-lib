using System.Globalization;

namespace psna_lib.structs;

[Serializable]
public struct Quaternion
{
	private float x, y, z, w;
	private const double TOLERANCE = 1e-6;
	
	public Quaternion() : this(0f, 0f, 0f, 0f) {}

	public Quaternion(Vector3 vectorPart, float scalarPart) : this(vectorPart.X, vectorPart.Y, vectorPart.Z, scalarPart) {}

	public Quaternion(float x, float y, float z, float w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
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
	
	public float W
	{
		get { return this.w; }
		set { this.w = value; }
	}
	
	public static Quaternion Zero { get { return new Quaternion(); } }
	public static Quaternion Identity { get { return new Quaternion(0f, 0f, 0f, 1f); } }

	public bool IsZero { get { return x == 0f && y == 0f && z == 0f && w == 0f; } }
	public bool IsIdentity { get { return x == 0f && y == 0f && z == 0f && Math.Abs(w - 1f) < TOLERANCE; } }
	
	public bool Equals(Quaternion other)
	{
		return (Math.Abs(x - other.x) < TOLERANCE &&
		        Math.Abs(y - other.y) < TOLERANCE &&
		        Math.Abs(z - other.z) < TOLERANCE &&
		        Math.Abs(w - other.w) < TOLERANCE);
	}

	public override bool Equals(object obj)
	{
		if (obj is Quaternion)
		{
			return Equals((Quaternion)obj);
		}

		return false;
	}

	public override string ToString()
	{
		CultureInfo ci = CultureInfo.CurrentCulture;

		return String.Format(ci, "{{X:{0} Y:{1} Z:{2} W:{3}}}", x.ToString(ci), y.ToString(ci), z.ToString(ci), w.ToString(ci));
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() + y.GetHashCode() + z.GetHashCode() + w.GetHashCode();
	}
}