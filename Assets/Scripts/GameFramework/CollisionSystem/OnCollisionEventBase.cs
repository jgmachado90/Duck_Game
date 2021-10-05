using System.Collections.Generic;
using CoreInterfaces;
using UnityEngine;

namespace GameFramework
{
    public class OnCollisionEventBase : CollisionEventBase
    {
        public event CollisionEvent OnSendCollision;

        public delegate void CollisionEvent(PhysicsBodyCollision col);

        protected void SendCollision(PhysicsBodyCollision physicsBodyCollision)
        {
            OnSendCollision?.Invoke(physicsBodyCollision);
        }
    }

    public struct PhysicsBodyCollision
    {
        private Collision2D _col2d;
        private Collision _col3d;

        public PhysicsBodyCollision(Collision2D col)
        {
            _col2d = col;
            _col3d = null;
        }

        public PhysicsBodyCollision(Collision col)
        {
            _col2d = null;
            _col3d = col;
        }

        public Transform Other => _col2d?.transform ?? _col3d.transform;
        public int Length => _col2d?.contactCount ?? _col3d.contactCount;

        public Contact this[int index]
        {
            get
            {
                Contact contact;

                if (_col2d != null)
                {
                    contact = new Contact(_col2d.GetContact(index));
                }
                else
                {
                    contact = new Contact(_col3d.GetContact(index));
                }

                return contact;
            }
        }

        public struct Contact
        {
            private readonly bool _is2d;
            private ContactPoint2D _contact2d;
            private ContactPoint _contact3d;

            public Contact(ContactPoint2D contact)
            {
                _is2d = true;
                _contact2d = contact;
                _contact3d = default;
            }

            public Contact(ContactPoint contact)
            {
                _is2d = false;
                _contact2d = default;
                _contact3d = contact;
            }

            public Vector3 Point => _is2d ? (Vector3) _contact2d.point : _contact3d.point;
            public Vector3 Normal => _is2d ? (Vector3) _contact2d.normal : _contact3d.normal;
            public float Separation => _is2d ? _contact2d.separation : _contact3d.separation;
        }
    }
}
