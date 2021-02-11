namespace Engine
{
    public struct Vector2F
    {
        public float x;
        public float y;

        public static Vector2F Zero => new Vector2F(0, 0);
        public static Vector2F Left => new Vector2F(-1, 0);
        public static Vector2F Right => new Vector2F(1, 0);
        public static Vector2F Up => new Vector2F(0, 1);
        public static Vector2F Down => new Vector2F(0, -1);

        public Vector2F(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2F operator +(Vector2F a, Vector2F b)
        {
            return new Vector2F(a.x + b.x, a.y + b.y);
        }
        public static Vector2F operator +(Vector2F a, Vector2I b)
        {
            return new Vector2F(a.x + b.x, a.y + b.y);
        }
        public static Vector2F operator -(Vector2F a, Vector2F b)
        {
            return new Vector2F(a.x - b.x, a.y - b.y);
        }
        public static Vector2F operator -(Vector2F a, Vector2I b)
        {
            return new Vector2F(a.x - b.x, a.y - b.y);
        }
        public static Vector2F operator *(Vector2F a, Vector2F b)
        {
            return new Vector2F(a.x * b.x, a.y * b.y);
        }
        public static Vector2F operator *(Vector2F a, int b)
        {
            return new Vector2F(a.x * b, a.y * b);
        }
        public static Vector2F operator *(Vector2F a, float b)
        {
            return new Vector2F(a.x * b, a.y * b);
        }
        public static Vector2F operator /(Vector2F a, Vector2F b)
        {
            return new Vector2F(a.x * b.x, a.y * b.y);
        }
        public static Vector2F operator /(Vector2F a, int b)
        {
            return new Vector2F(a.x * b, a.y * b);
        }
        public static Vector2F operator /(Vector2F a, float b)
        {
            return new Vector2F(a.x * b, a.y * b);
        }
    }
}
