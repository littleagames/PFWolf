namespace RaycastSample;

using System;
using System.Drawing;
using System.Windows.Forms;

internal class RaycastingEngine : Form
{
    private Timer renderTimer;
    private int[,] map = {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 1, 0, 1, 0, 0, 1},
        {1, 0, 0, 0, 0, 1, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };
    private float posX = 3.5f, posY = 3.5f; // Player's position
    private float dirX = -1, dirY = 0; // Initial direction vector
    private float planeX = 0, planeY = 0.66f; // Camera plane (determines FOV)

    public RaycastingEngine()
    {
        this.Width = 800;
        this.Height = 400;
        this.Text = "Raycasting Engine";
        renderTimer = new Timer();
        renderTimer.Interval = 16;
        renderTimer.Tick += (sender, args) => Invalidate();
        renderTimer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.Clear(Color.Black);

        int screenWidth = this.ClientSize.Width;
        int screenHeight = this.ClientSize.Height;

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

            Color color = (side == 1) ? Color.DarkGray : Color.Gray;
            g.DrawLine(new Pen(color), x, drawStart, x, drawEnd);
        }
    }

    [STAThread]
    internal static void Main()
    {
        Application.Run(new RaycastingEngine());
    }
}