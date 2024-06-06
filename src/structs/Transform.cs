using System.Numerics;

namespace psna_lib.structs;

public struct Transform
{
    private Vector3 _position, _localPosition, _localScale;
    private Quaternion _rotation, _localRotation;

    public Transform()
    {
        Position = LocalPosition = Vector3.Zero;
        LocalScale = Vector3.One;
        Rotation = LocalRotation = Quaternion.Zero;
    }

    public Transform(Vector3 position, Vector3 localPosition, Vector3 localScale, Quaternion rotation,
        Quaternion localRotation)
    {
        Position = position; LocalPosition = localPosition;
        LocalScale = localScale;
        Rotation = rotation; LocalRotation = localRotation;
    }

    public Vector3 Position
    {
        get { return this._position; }
        set { this._position = value; }
    }
    
    public Vector3 LocalPosition
    {
        get { return this._localPosition; }
        set { this._localPosition = value; }
    }
    
    public Vector3 LocalScale
    {
        get { return this._localScale; }
        set { this._localScale = value; }
    }
    
    public Quaternion Rotation
    {
        get { return this._rotation; }
        set { this._rotation = value; }
    }
    
    public Quaternion LocalRotation
    {
        get { return this._localRotation; }
        set { this._localRotation = value; }
    }
}