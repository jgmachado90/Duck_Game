using UnityEngine;
using UnityEngine.Assertions;

public class PhysicsBody
{
    public bool Is2d => _body2d != null;

    Rigidbody2D _body2d;
    private Rigidbody _body3d;

    public PhysicsBody(GameObject gameObject, bool useGravity = false)
    {
        _body2d = gameObject.GetComponent<Rigidbody2D>();
        if (!_body2d)
        {
            _body3d = gameObject.GetComponent<Rigidbody>();
            Assert.IsNotNull(_body3d,"game object with a PhysicsObject component doesn't have a Rigidbody (be it 2d or 3d) attached to it.");
        }
    }

    public bool IsKinematic {
        get => _body2d?.isKinematic ?? _body3d.isKinematic;
        set {
            if (_body2d)
            {
                _body2d.isKinematic = value;
                _body2d.simulated = !value;
            }
            else
            {
                _body3d.isKinematic = value;
            }
        }
    }

    public Vector3 Velocity {
        get {
            Vector3 value;
            if (_body2d)
            {
                value = _body2d.velocity;
            }
            else
            {
                value = _body3d.velocity;
            }
            return value;
        }
        set {
            if (_body2d)
            {
                _body2d.velocity = value;
            }
            else
            {
                _body3d.velocity = value;
            }
        }
    }
    
    public void AddForce(Vector3 force)
    {
        if (_body2d)
        {
            _body2d.AddForce(force, ForceMode2D.Force);
        }
        else
        {
            _body3d.AddForce(force, ForceMode.Force);
        }
    }
    
    public void AddImpulse(Vector3 force)
    {
        if (_body2d)
        {
            _body2d.AddForce(force, ForceMode2D.Impulse);
        }
        else
        {
            _body3d.AddForce(force, ForceMode.Impulse);
        }
    }

    public void AddAcceleration(Vector3 force)
    {
        Assert.IsFalse(_body2d,"AddAcceleration not yet implemented for body2d. Why are you even calling this function?");
        if (_body3d)
        {
            _body3d.AddForce(force, ForceMode.Acceleration);
        }
    }

    public void MovePosition(Vector3 pos)
    {
        if (_body2d)
        {
            _body2d.MovePosition(pos);
        }
        else
        {
            _body3d.MovePosition(pos);
        }
    }

    public void IgnoreLayerCollision(int layer1, int layer2, bool ignore)
    {
        if (_body2d)
        {
            Physics2D.IgnoreLayerCollision(layer1, layer2, ignore);
        }
        else
        {
            Physics.IgnoreLayerCollision(layer1, layer2, ignore);
        }
    }
}