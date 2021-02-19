using System;

namespace Engine
{
    public struct Vector2
    {
        public float x;
        public float y;

        public int XI => (int)x;
        public int YI => (int)y;

        public static Vector2 Zero => new Vector2(0, 0);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Right => new Vector2(1, 0);
        public static Vector2 Up => new Vector2(0, 1);
        public static Vector2 Down => new Vector2(0, -1);

        public float Magnitude => (float)Math.Sqrt(x * x + y * y);
        public float SqrMagnitude => x * x + y * y;

        public void Normalize()
        {
            float magnitude = Magnitude;
            this = magnitude > float.Epsilon ? this / magnitude : Zero;
        }
        public Vector2 Normalized
        {
            get
            {
                Vector2 vector = new Vector2(x, y);
                vector.Normalize();
                return vector;
            }
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }
        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public void Set(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }

        public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
        {
            Vector2 direction = target - current;
            float sqrDistance = direction.SqrMagnitude;
            float distance = (float)Math.Sqrt(sqrDistance);

            if (sqrDistance == 0 || (maxDistanceDelta >= 0 && sqrDistance <= maxDistanceDelta * maxDistanceDelta))
                return target;

            return new Vector2(current.x + direction.x / distance * maxDistanceDelta, current.y + direction.y / distance * maxDistanceDelta);
        }
        public static Vector2 Reflect(Vector2 direction, Vector2 normal)
        {
            float factor = -2F * Dot(normal, direction);
            return new Vector2(factor * normal.x + direction.x, factor * normal.y + direction.y);
        }
        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }
        public static float Distance(Vector2 from, Vector2 to)
        {
            return (from - to).Magnitude;
        }
        public static float SqrDistance(Vector2 from, Vector2 to)
        {
            return (from - to).SqrMagnitude;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator +(Vector2 a, int b)
        {
            return new Vector2(a.x + b, a.y + b);
        }
        public static Vector2 operator +(Vector2 a, float b)
        {
            return new Vector2(a.x + b, a.y + b);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator -(Vector2 a, int b)
        {
            return new Vector2(a.x - b, a.y - b);
        }
        public static Vector2 operator -(Vector2 a, float b)
        {
            return new Vector2(a.x + b, a.y + b);
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }
        public static Vector2 operator *(Vector2 a, int b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }
        public static Vector2 operator /(Vector2 a, int b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            Vector2 vector = (Vector2)obj;
            return x == vector.x && y == vector.y;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }
        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
