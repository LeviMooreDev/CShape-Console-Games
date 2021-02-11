﻿namespace Engine
{
    public struct Vector2I
    {
        public int x;
        public int y;

        public static Vector2I Zero => new Vector2I(0, 0);
        public static Vector2I Left => new Vector2I(-1, 0);
        public static Vector2I Right => new Vector2I(1, 0);
        public static Vector2I Up => new Vector2I(0, 1);
        public static Vector2I Down => new Vector2I(0, -1);

        public Vector2I(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2I(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }

        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2I operator +(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.x + b.x, a.y + b.y);
        }
        public static Vector2I operator -(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.x - b.x, a.y - b.y);
        }
        public static Vector2I operator *(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.x * b.x, a.y * b.y);
        }
        public static Vector2I operator *(Vector2I a, int b)
        {
            return new Vector2I(a.x * b, a.y * b);
        }
        public static Vector2I operator /(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.x * b.x, a.y * b.y);
        }
        public static Vector2I operator /(Vector2I a, int b)
        {
            return new Vector2I(a.x * b, a.y * b);
        }
    }
}
