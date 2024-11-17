[PfWolfScene("wolf3d:InitializeGameScene")]
public class InitializeGameScene : Scene
{
    private Fader _fadeInFader { get; }
    private Fader _fadeOutFader { get; }
    private Render _render { get; }

    private Position PlayerPosition { get; set; } = new(0,0);
    private float PlayerAngle { get; set; } = 0.0f;
    
    
    private int[,] map = {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    };
    private float posX = 12.5f, posY = 3.5f; // Player's position
    private float dirX = 0, dirY = 0; // Initial direction vector
    private float planeX = 0, planeY = 0.66f; // Camera plane (determines FOV)

    
    public InitializeGameScene()
    {
        _fadeInFader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, 20);
        _fadeOutFader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 20);
        _render = Render.Create(0, 0, 320, 200);
        _render.Hidden = true;
        
        Components.Add(Background.Create(0x7f));
        Components.Add(Graphic.Create("getpsyched",((320-224)/16)*8, (200-(40+48))/2));
        
        Components.Add(_fadeInFader);
        Components.Add(_fadeOutFader);
        Components.Add(_render);
    }

    public override void OnUpdate()
    {
        if (!_fadeInFader.IsFading)
            _fadeInFader.BeginFade();

        if (!_fadeInFader.IsComplete)
            return;
        _render.Hidden = false;

        PollControls();
        
        // DoDoors
        // DoPushwalls
        // DoActor
        // PaletteShifts?
        ThreeDRefresh();
    }

    private void PollControls()
    {
        if (Input.IsKeyDown(Keys.Left))
        {
            posX -= 0.1f;
            PlayerAngle = (PlayerAngle - 1.0f) % 360;
            Console.WriteLine($"PlayerAngle: {PlayerAngle}");
        }
        else if (Input.IsKeyDown(Keys.Right))
        {
            posX += 0.1f;
            PlayerAngle = (PlayerAngle + 1.0f) % 360;
            Console.WriteLine($"PlayerAngle: {PlayerAngle}");
        }
        
        if (Input.IsKeyDown(Keys.Up))
        {
            Console.WriteLine($"Move up");
            posY += 0.1f;
        }
        else if (Input.IsKeyDown(Keys.Down))
        {
            Console.WriteLine($"Move back");
            posY -= 0.1f;
        }
        // TODO: Play loop scene?
    }

    private void ThreeDRefresh()
    {
        _render.Clear();
        int screenWidth = _render.Width;
        int screenHeight = _render.Height;

        for (int x = 0; x < screenWidth; x++)
        {
            float cameraX = 2 * x / (float)screenWidth - 1;
            float rayDirX = dirX + planeX * cameraX;
            float rayDirY = dirY + planeY * cameraX;

            int mapX = (int)posX;
            int mapY = (int)posY;

            float sideDistX, sideDistY;
            float deltaDistX = (rayDirX == 0) ? float.MaxValue : Math.Abs(1 / rayDirX);
            float deltaDistY = (rayDirY == 0) ? float.MaxValue : Math.Abs(1 / rayDirY);
            float perpWallDist;

            int stepX, stepY;
            bool hit = false;
            int side = 0;

            if (rayDirX < 0)
            {
                stepX = -1;
                sideDistX = (posX - mapX) * deltaDistX;
            }
            else
            {
                stepX = 1;
                sideDistX = (mapX + 1.0f - posX) * deltaDistX;
            }
            if (rayDirY < 0)
            {
                stepY = -1;
                sideDistY = (posY - mapY) * deltaDistY;
            }
            else
            {
                stepY = 1;
                sideDistY = (mapY + 1.0f - posY) * deltaDistY;
            }

            while (!hit)
            {
                if (sideDistX < sideDistY)
                {
                    sideDistX += deltaDistX;
                    mapX += stepX;
                    side = 0;
                }
                else
                {
                    sideDistY += deltaDistY;
                    mapY += stepY;
                    side = 1;
                }
                if (map[mapX, mapY] > 0) hit = true;
            }

            if (side == 0)
                perpWallDist = (mapX - posX + (1 - stepX) / 2) / rayDirX;
            else
                perpWallDist = (mapY - posY + (1 - stepY) / 2) / rayDirY;

            int lineHeight = (int)(screenHeight / perpWallDist);
            int drawStart = -lineHeight / 2 + screenHeight / 2;
            int drawEnd = lineHeight / 2 + screenHeight / 2;

            //Color color = (side == 1) ? Color.DarkGray : Color.Gray;
            byte color = (byte)((side == 1) ? 0x13 : 0x1a);

            _render.DrawLine(x, drawStart, x, drawEnd, color);
        }
    }
}