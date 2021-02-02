namespace Engine
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
    }
}
