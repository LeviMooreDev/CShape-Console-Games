using System;
using Engine;

namespace FirstPerson
{
    class Program
    {
        //shading
        private const Color ceilingColor = Color.Black;
        private const Color floorColor = Color.Black;
        private static WallShade[] wallShading = new WallShade[]
        {
            new WallShade((char)0x2591, Color.Black), //far
            new WallShade(' ', Color.Gray),
            new WallShade((char)0x2591, Color.Gray),
            new WallShade((char)0x2592, Color.Black),
            new WallShade(' ', Color.White),
            new WallShade((char)0x2592, Color.Bright_White),
            new WallShade((char)0x2591, Color.Bright_White),
            new WallShade(' ', Color.Bright_White) //near
        };
        private static FloorShade[] floorShading = new FloorShade[]
        {
            new FloorShade('=', Color.White, 2), //near
            new FloorShade('=', Color.White, 4),
            new FloorShade('=', Color.Gray, 4),
            new FloorShade('-', Color.Gray, 4),
            new FloorShade('-', Color.Gray, 6),
            new FloorShade('-', Color.Gray, 8),
            new FloorShade(' ', Color.Gray, 8) //far
        };

        //minimap
        private static char mapWall = '#';
        private static char mapPlayer = 'P';
        private static char mapPlayerForward = '@';
        private static string[] map = new string[]
        {
            "################",
            "#            # #",
            "#  #         # #",
            "#              #",
            "#       #      #",
            "#       #      #",
            "#       ##     #",
            "#       #     ##",
            "#       #      #",
            "#             ##",
            "#####          #",
            "#              #",
            "################"
        };

        //player
        private static Vector2F playerPosition = new Vector2F(2, 8);
        private static float playerAngle = 1.5708f;
        private static float playerSpeed = 2;
        private static float fov = 0.6f;

        //rendering
        private const float rayStepSize = 0.1f;
        private const float rayMaxDepth = 16;


        static void Main(string[] args)
        {
            Game.OnUpdate += Update;
            //Game.Begin("FPS", new Vector2I(80, 40));
            Game.Begin("FPS", new Vector2I(120, 80));
            //Game.Begin("FPS", new Vector2I(300, 150));
        }
        private static void Update()
        {
            Movement();
            Rendering();
            Minimap();
        }

        private static void Movement()
        {
            //walk
            Vector2F targetPosition = playerPosition;
            if (Input.KeyHold(ConsoleKey.W))
            {
                targetPosition += Vector2F.Right * MathF.Sin(playerAngle) * playerSpeed * Game.DeltaTime;
                targetPosition += Vector2F.Up * MathF.Cos(playerAngle) * playerSpeed * Game.DeltaTime;
            }
            if (Input.KeyHold(ConsoleKey.S))
            {
                targetPosition -= Vector2F.Right * MathF.Sin(playerAngle) * playerSpeed * Game.DeltaTime;
                targetPosition -= Vector2F.Up * MathF.Cos(playerAngle) * playerSpeed * Game.DeltaTime;
            }
            if (Input.KeyHold(ConsoleKey.A))
            {
                targetPosition -= Vector2F.Up * MathF.Sin(-playerAngle) * playerSpeed * Game.DeltaTime;
                targetPosition -= Vector2F.Right * MathF.Cos(-playerAngle) * playerSpeed * Game.DeltaTime;
            }
            if (Input.KeyHold(ConsoleKey.D))
            {
                targetPosition += Vector2F.Up * MathF.Sin(-playerAngle) * playerSpeed * Game.DeltaTime;
                targetPosition += Vector2F.Right * MathF.Cos(-playerAngle) * playerSpeed * Game.DeltaTime;
            }
            if (map[(int)targetPosition.y][(int)targetPosition.x] != mapWall)
            {
                playerPosition = targetPosition;
            }

            //look
            if (Input.MouseLeftHold)
            {
                playerAngle -= 1 * Game.DeltaTime;
            }
            if (Input.MouseRightHold)
            {
                playerAngle += 1 * Game.DeltaTime;
            }
        }
        private static void Rendering()
        {
            for (int x = 0; x < Game.Size.x; x++)
            {
                //raycast forward
                float rayAngle = playerAngle - fov / 2f + (float)x / Game.Size.x * fov;
                Vector2F rayDirection = new Vector2F(MathF.Sin(rayAngle), MathF.Cos(rayAngle));
                float distanceToWall = 0;
                Vector2I hit;

                while (distanceToWall < rayMaxDepth)
                {
                    //step hit position forward
                    distanceToWall += rayStepSize;
                    hit = new Vector2I(playerPosition.x + rayDirection.x * distanceToWall, playerPosition.y + rayDirection.y * distanceToWall);

                    //if outside map
                    if (hit.x < 0 || hit.x >= map[0].Length || hit.y < 0 || hit.y >= map.Length)
                    {
                        distanceToWall = rayMaxDepth;
                        break;
                    }
                    //if inside map
                    else
                    {
                        //hit wall
                        if (map[hit.y][hit.x] == mapWall)
                        {
                            break;
                        }
                    }
                }

                //draw vertical
                int ceilingStart = (int)(Game.Size.y / 2f - Game.Size.y / distanceToWall);
                int floorStart = Game.Size.y - ceilingStart;

                for (int y = 0; y < Game.Size.y; y++)
                {
                    Vector2I pos = new Vector2I(x, y);

                    //ceiling
                    if (y <= ceilingStart)
                    {
                        Render.DrawText(pos, ' ');
                        Render.DrawBackgroundColor(pos, ceilingColor);
                    }
                    //wall
                    else if (y > ceilingStart && y <= floorStart)
                    {
                        for (int i = wallShading.Length - 1; i >= 0; i--)
                        {
                            if (distanceToWall < rayMaxDepth / (i + 1))
                            {
                                Render.DrawText(pos, wallShading[i].Texture);
                                Render.DrawTextColor(pos, Color.White);
                                Render.DrawBackgroundColor(pos, wallShading[i].Color);
                                break;
                            }
                        }
                    }
                    //floor
                    else
                    {
                        Render.DrawBackgroundColor(pos, floorColor);

                        float distance = 1f - (y - Game.Size.y / 2) / (Game.Size.y / 2f);
                        float step = 1f / floorShading.Length;
                        for (int i = 0; i < floorShading.Length; i++)
                        {
                            if (distance < i * step + step)
                            {
                                Render.DrawTextColor(pos, floorShading[i].Color);
                                if (y % 2 == 0 && x % floorShading[i].Offset == 0)
                                {
                                    Render.DrawText(pos, floorShading[i].Texture);
                                }
                                else if (y % 2 != 0 && (x + floorShading[i].Offset / 2) % floorShading[i].Offset == 0)
                                {
                                    Render.DrawText(pos, floorShading[i].Texture);
                                }
                                else
                                {
                                    Render.DrawText(pos, ' ');
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
        private static void Minimap()
        {
            Vector2I offset = new Vector2I(Game.Size.x - map[0].Length, 0);

            //map
            for (int x = 0; x < map.Length; x++)
            {
                for (int y = 0; y < map[0].Length; y++)
                {
                    Vector2I pos = new Vector2I(y, x) + offset;

                    Render.DrawBackgroundColor(pos, Color.Black);
                    Render.DrawTextColor(pos, Color.White);
                    Render.DrawText(pos, map[x][y]);
                }
            }

            //current player position
            Render.DrawText(playerPosition + offset, mapPlayer);

            //next player position
            Vector2F forwardPosition = playerPosition;
            forwardPosition += Vector2F.Right * MathF.Sin(playerAngle);
            forwardPosition += Vector2F.Up * MathF.Cos(playerAngle);
            Render.DrawTextColor(forwardPosition + offset, Color.Gray);
            Render.DrawText(forwardPosition + offset, mapPlayerForward);
        }

        public struct WallShade
        {
            public char Texture { get; private set; }
            public Color Color { get; private set; }

            public WallShade(char texture, Color color)
            {
                Texture = texture;
                Color = color;
            }
        }
        public struct FloorShade
        {
            public char Texture { get; private set; }
            public Color Color { get; private set; }
            public int Offset { get; private set; }

            public FloorShade(char texture, Color color, int offset)
            {
                Texture = texture;
                Color = color;
                Offset = offset;
            }
        }
    }
}
