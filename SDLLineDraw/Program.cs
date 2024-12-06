using System;
using SDL2;



class Program
{
    //-----------------------------MAP----------------------------------------------
    private const int mapX =  8;     //map width
    private const int mapY =  8;     //map height
    private const int mapS = 64;     //map cube size
    private int[] map =           //the map array. Edit to change level but keep the outer walls
    {
        1,1,1,1,1,1,1,1,
        1,0,1,0,0,0,0,1,
        1,0,1,0,0,0,0,1,
        1,0,1,0,0,0,0,1,
        1,0,0,0,0,0,0,1,
        1,0,0,0,0,1,0,1,
        1,0,0,0,0,0,0,1,
        1,1,1,1,1,1,1,1,	
    };

    private void drawMap2D(IntPtr renderer)
    {
        int x,y,xo,yo;
        for(y=0;y<mapY;y++)
        {
            for(x=0;x<mapX;x++)
            {
                if (map[y * mapX + x] == 1)
                {
                    // Set draw color (R, G, B, A)
                    SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
                }
                else
                {
                    // Set draw color (R, G, B, A)
                    SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
                }
                xo=x*mapS; yo=y*mapS;
                
                var posLeftTop = ( X:0   +xo+1, Y:0   +yo+1);  // left top
                var posLeftBottom = ( X:0   +xo+1, Y:mapS+yo-1);  // left bottom
                var posRightBottom = ( X:mapS+xo-1, Y:mapS+yo-1);  // right bottom
                var posRightTop = ( X:mapS+xo-1, Y:0   +yo+1);  // right top
                
                SDL.SDL_RenderDrawLine(renderer, posLeftTop.X, posLeftTop.Y, posLeftBottom.X, posLeftBottom.Y);
                SDL.SDL_RenderDrawLine(renderer, posLeftBottom.X, posLeftBottom.Y, posRightBottom.X, posRightBottom.Y);
                SDL.SDL_RenderDrawLine(renderer, posRightBottom.X, posRightBottom.Y, posRightTop.X, posRightTop.Y);
                SDL.SDL_RenderDrawLine(renderer, posRightTop.X, posRightTop.Y, posLeftTop.X, posLeftTop.Y);
            } 
        } 
    }
    
    //------------------------PLAYER------------------------------------------------
    float degToRad(int a)
    {
        return (float)(a * Math.PI / 180.0d);
    }
    
    float degToRad(float a)
    {
        return (float)(a * Math.PI / 180.0d);
    }
    
    int FixAng(int a){ if(a>359){ a-=360;} if(a<0){ a+=360;} return a;}
    float FixAng(float a){ if(a>359){ a-=360;} if(a<0){ a+=360;} return a;}

    float px,py,pdx,pdy,pa;

    void drawPlayer2D(IntPtr renderer)
    {
        SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 0, 255);
        
        //glPointSize(8);
        SDL.SDL_RenderDrawPoint(renderer, (int)px, (int)py);
        
        //glLineWidth(4);
        SDL.SDL_RenderDrawLine(renderer, (int)px, (int)py,(int)(px+pdx*20),(int)(py+pdy*20));
        SDL.SDL_RenderDrawLine(renderer, (int)px+1, (int)py,(int)(px+pdx*20)+1,(int)(py+pdy*20));
        SDL.SDL_RenderDrawLine(renderer, (int)px+2, (int)py,(int)(px+pdx*20)+2,(int)(py+pdy*20));
        SDL.SDL_RenderDrawLine(renderer, (int)px+3, (int)py,(int)(px+pdx*20)+3,(int)(py+pdy*20));
    }
    
    void Buttons(SDL.SDL_Keycode key)
    {
        if(key==SDL.SDL_Keycode.SDLK_a){ pa+=5; pa=FixAng(pa); pdx=(float)Math.Cos(degToRad(pa)); pdy=(float)-Math.Sin(degToRad(pa));} 	
        if(key==SDL.SDL_Keycode.SDLK_d){ pa-=5; pa=FixAng(pa); pdx=(float)Math.Cos(degToRad(pa)); pdy=(float)-Math.Sin(degToRad(pa));} 
        if(key==SDL.SDL_Keycode.SDLK_w){ px+=pdx*5; py+=pdy*5;}
        if(key==SDL.SDL_Keycode.SDLK_s){ px-=pdx*5; py-=pdy*5;}
        //glutPostRedisplay();
    }
    //-----------------------------------------------------------------------------


//---------------------------Draw Rays and Walls--------------------------------
    float distance(float ax, float ay, float bx, float by, float ang)
    {
        return (float)Math.Cos(degToRad(ang))*(bx-ax)-(float)Math.Sin(degToRad(ang))*(by-ay);
    }

void drawRays2D(IntPtr renderer)
{
    SDL.SDL_SetRenderDrawColor(renderer, 0, 255, 255, 255);
    
    SDL.SDL_RenderDrawLine(renderer, 526, 0, 1006, 0);
    SDL.SDL_RenderDrawLine(renderer, 1006, 0, 1006, 160);
    SDL.SDL_RenderDrawLine(renderer, 1006, 160, 526, 160);
    SDL.SDL_RenderDrawLine(renderer, 526, 160, 526, 0);
 
    SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 255, 255);
    
    SDL.SDL_RenderDrawLine(renderer, 526, 160, 1006, 160);
    SDL.SDL_RenderDrawLine(renderer, 1006, 160, 1006, 320);
    SDL.SDL_RenderDrawLine(renderer, 1006, 320, 526,320);
    SDL.SDL_RenderDrawLine(renderer, 526,320, 526, 160);	 	
	
 int r,mx,my,mp,dof,side;
 float vx,vy,rx,ry,ra,xo=0,yo=0,disV,disH; 
 
 ra=FixAng(pa+30);                                                              //ray set back 30 degrees
 
 for(r=0;r<60;r++)
 {
  //---Vertical--- 
  dof=0; side=0; disV=100000;
  float Tan=(float)Math.Tan(degToRad(ra));
  if (Math.Cos(degToRad(ra)) > 0.001)
  {
      rx=(((int)px>>6)<<6)+64;
      ry=(px-rx)*Tan+py;
      xo= 64;
      yo=-xo*Tan;
  }//looking left
  else if(Math.Cos(degToRad(ra))<-0.001){ rx=(((int)px>>6)<<6) -0.0001f; ry=(px-rx)*Tan+py; xo=-64; yo=-xo*Tan;}//looking right
  else { rx=px; ry=py; dof=8;}                                                  //looking up or down. no hit  

  while(dof<8) 
  { 
   mx=(int)(rx)>>6; my=(int)(ry)>>6; mp=my*mapX+mx;                     
   if(mp>0 && mp<mapX*mapY && map[mp]==1){ dof=8; disV=(float)Math.Cos(degToRad(ra))*(rx-px)-(float)Math.Sin(degToRad(ra))*(ry-py);}//hit         
   else{ rx+=xo; ry+=yo; dof+=1;}                                               //check next horizontal
  } 
  vx=rx; vy=ry;

  //---Horizontal---
  dof=0; disH=100000;
  Tan=1.0f/Tan; 
       if(Math.Sin(degToRad(ra))> 0.001f){ ry=(((int)py>>6)<<6) -0.0001f; rx=(py-ry)*Tan+px; yo=-64; xo=-yo*Tan;}//looking up 
  else if(Math.Sin(degToRad(ra))<-0.001f){ ry=(((int)py>>6)<<6)+64;      rx=(py-ry)*Tan+px; yo= 64; xo=-yo*Tan;}//looking down
  else{ rx=px; ry=py; dof=8;}                                                   //looking straight left or right
 
  while(dof<8) 
  { 
   mx=(int)(rx)>>6; my=(int)(ry)>>6; mp=my*mapX+mx;                          
   if(mp>0 && mp<mapX*mapY && map[mp]==1){ dof=8; disH=(float)Math.Cos(degToRad(ra))*(rx-px)-(float)Math.Sin(degToRad(ra))*(ry-py);}//hit         
   else{ rx+=xo; ry+=yo; dof+=1;}                                               //check next horizontal
  } 
  
  SDL.SDL_SetRenderDrawColor(renderer, 0, 204, 0, 255);
  if (disV < disH)
  {
      rx=vx; ry=vy; 
      disH=disV;
      
      SDL.SDL_SetRenderDrawColor(renderer, 0, 152, 0, 255);
  }                  //horizontal hit first
  //glLineWidth(2);
  //glBegin(GL_LINES);
  
  SDL.SDL_RenderDrawLine(renderer, (int)px, (int)py, (int)rx, (int)ry);
  SDL.SDL_RenderDrawLine(renderer, (int)px+1, (int)py, (int)rx+1, (int)ry);
    
  int ca=(int)FixAng(pa-ra); disH=disH*(float)Math.Cos(degToRad(ca));                            //fix fisheye 
  int lineH = (int)((mapS*320)/(disH)); if(lineH>320){ lineH=320;}                     //line height and limit
  int lineOff = 160 - (lineH>>1);                                               //line offset
  
  //glLineWidth(8);
  //glBegin(GL_LINES);
  
  SDL.SDL_RenderDrawLine(renderer, r*8-3+530,lineOff, r*8-3+530,lineOff+lineH);
  SDL.SDL_RenderDrawLine(renderer, r*8-2+530,lineOff, r*8-2+530,lineOff+lineH);
  SDL.SDL_RenderDrawLine(renderer, r*8-1+530,lineOff, r*8-1+530,lineOff+lineH);
  SDL.SDL_RenderDrawLine(renderer, r*8+530,lineOff, r*8+530,lineOff+lineH);
  SDL.SDL_RenderDrawLine(renderer, r*8+1+530,lineOff, r*8+1+530,lineOff+lineH);
  SDL.SDL_RenderDrawLine(renderer, r*8+2+530,lineOff, r*8+2+530,lineOff+lineH);
  SDL.SDL_RenderDrawLine(renderer, r*8+3+530,lineOff, r*8+3+530,lineOff+lineH);
  SDL.SDL_RenderDrawLine(renderer, r*8+4+530,lineOff, r*8+4+530,lineOff+lineH);

  ra=FixAng(ra-1);                                                              //go to next ray
 }
}//-----------------------------------------------------------------------------

void init(IntPtr renderer)
{
    // Clear the screen (fill with black)
    SDL.SDL_RenderClear(renderer);

    // Set draw color (R, G, B, A)
    SDL.SDL_SetRenderDrawColor(renderer, 77,77, 77, 255); // Red color
    //glClearColor(0.3,0.3,0.3,0);
    //gluOrtho2D(0,1024,510,0);
    px=150; py=400; pa=90;
    pdx=(float)Math.Cos(degToRad(pa)); pdy=-(float)Math.Sin(degToRad(pa)); 
}

void display(IntPtr renderer)
{   
    //glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT); 
    SDL.SDL_SetRenderDrawColor(renderer, 77,77, 77, 255); // Red color
    SDL.SDL_RenderClear(renderer);
    drawMap2D(renderer);
    drawPlayer2D(renderer);
    drawRays2D(renderer);
    //glutSwapBuffers();  
}
    static void Main()
    {
        // Initialize SDL
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            Console.WriteLine("SDL_Init Error: " + SDL.SDL_GetError());
            return;
        }

        // Create a window
        IntPtr window = SDL.SDL_CreateWindow("SDL2 Line Drawing", 
            SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 1024, 510, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
        if (window == IntPtr.Zero)
        {
            Console.WriteLine("SDL_CreateWindow Error: " + SDL.SDL_GetError());
            SDL.SDL_Quit();
            return;
        }

        // Create a renderer
        IntPtr renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
        if (renderer == IntPtr.Zero)
        {
            Console.WriteLine("SDL_CreateRenderer Error: " + SDL.SDL_GetError());
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
            return;
        }

        var program = new Program();
        // Draw a line (start x, start y, end x, end y)
        program.init(renderer);


        // Wait for 3 seconds before closing the window// Event loop to handle input
        bool quit = false;
        SDL.SDL_Event e;

        while (!quit)
        {
            while (SDL.SDL_PollEvent(out e) != 0)
            {
                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        quit = true;
                        break;

                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        program.Buttons(e.key.keysym.sym);
                        // Key down event
                        Console.WriteLine($"Key pressed: {SDL.SDL_GetKeyName(e.key.keysym.sym)}");
                        break;

                    case SDL.SDL_EventType.SDL_KEYUP:
                        // Key up event
                        Console.WriteLine($"Key released: {SDL.SDL_GetKeyName(e.key.keysym.sym)}");
                        break;

                    case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        // Mouse button down event
                        if (e.button.button == SDL.SDL_BUTTON_LEFT)
                        {
                            Console.WriteLine("Left mouse button pressed.");
                        }
                        break;

                    case SDL.SDL_EventType.SDL_MOUSEMOTION:
                        // Mouse motion event
                        Console.WriteLine($"Mouse moved to {e.motion.x}, {e.motion.y}");
                        break;

                    default:
                        break;
                }
                
                program.display(renderer);
                // Present the rendered content to the screen
                SDL.SDL_RenderPresent(renderer);
            }
        }

        // Clean up and quit SDL
        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
    }
}