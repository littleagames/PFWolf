﻿namespace LittleAGames.PFWolf.SDK.Components;

public class Wolf3DRaycastRenderer : Renderer
{
    private readonly Map _map;
    private readonly bool[,] _spotVis;
    
    public Wolf3DRaycastRenderer(Camera camera, Map map, int width, int height) : base(camera, width, height)
    {
        _map = map;
        _result = new byte[width*height];
        _spotVis = new bool[64,64];//map.Width, map.Height]; // TODO: Map width/height not set yet at this time

        BuildTables();

        CalcProjection(0x5700L);

    }

    public int CenterX => (Width / 2) - 1;
    public int CenterY => Height / 2;

    private const int ANGLES = 360;             // must be divisable by 4
    private const int ANGLEQUAD = (ANGLES / 4);
    private const int FINEANGLES = 3600;
    private const int ANG90 = (FINEANGLES / 4);
    private const int ANG180 = (ANG90 * 2);
    private const int ANG270 = (ANG90 * 3);
    private const int ANG360 = (ANG90 * 4);
    private const int VANG90 = (ANGLES / 4);
    private const int VANG180 = (VANG90 * 2);
    private const int VANG270 = (VANG90 * 3);
    private const int VANG360 = (VANG90 * 4);

    private const int TILESHIFT = 16;

    private const int TEXTURESHIFT = 6;

    private const int FIXED2TEXSHIFT = 4;
    private const int TEXTURESIZE = (1 << TEXTURESHIFT);
    private readonly int TEXTUREMASK = (TEXTURESIZE * (TEXTURESIZE - 1));

    private long MINDIST = 0x5800L;
    private long VIEWGLOBAL = 0x10000;
    private const long GLOBAL1 = (1L << 16);
    private readonly long TILEGLOBAL = GLOBAL1;
    
    
private const int WALLSHIFT =      6;
private const int BIT_WALL   =     (1 << WALLSHIFT);
private const int BIT_DOOR    =    (1 << (WALLSHIFT + 1));
private const int BIT_ALLTILES =   (1 << (WALLSHIFT + 2));
    
    private int focalLength;
    private int scale;
    private int heightNumerator;

    private int[] finetangent = new int[FINEANGLES / 4];
    private int[]   sintable = new int[ANGLES+(ANGLES/4)];
    private int[]   costable = new int[ANGLES];

    private short[] pixelAngle = new short[640]; // screenWidth
    private short[] wallHeight = new short[640]; // screenWidth
    
    
    private ushort    tilehit;
    private MapComponent tilehit2;
    private int     pixx;

    private short   xtile,ytile;
    private short   xtilestep = 0,ytilestep = 0;
    private int   xintercept,yintercept;
    private int   xinttile,yinttile;
    private ushort    texdelta;
    //private ushort[]    horizwall = new ushort[90];
    //private ushort[] vertwall = new ushort[90];
    
    private int viewSin;
    private int viewCos;
    private int viewX;
    private int viewY;

    
    private int     postx;
    private byte[] postsource;

    private byte[] _result;
    
    private void BuildTables()
    {
//
// calculate fine tangents
//

        int i;
        for(i=0;i<FINEANGLES/8;i++)
        {
            double tang=Math.Tan((i+0.5)/MathUtilities.RadiansToInteger);
            finetangent[i]=(int)(tang*GLOBAL1);
            finetangent[FINEANGLES/4-1-i]=(int)((1/tang)*GLOBAL1);
        }
        
        //
        // costable overlays sintable with a quarter phase shift
        // ANGLES is assumed to be divisable by four
        //

        float angle=0;
        float anglestep=(float)(Math.PI/2/ANGLEQUAD);
        for(i=0; i<ANGLEQUAD; i++)
        {
            int value=(int)(GLOBAL1*Math.Sin(angle));
            sintable[i]=sintable[i+ANGLES]=sintable[ANGLES/2-i]=value;
            sintable[ANGLES-i]=sintable[ANGLES/2+i]=-value;
            angle+=anglestep;
        }
        sintable[ANGLEQUAD] = 65536;
        sintable[3*ANGLEQUAD] = -65536;
        costable = sintable.Skip(ANGLES / 4).ToArray();
    }
    
    private void CalcProjection(long focal)
    {
        int     i;
        int    intang;
        float   angle;
        double  tang;
        int     halfview;
        double  facedist;

        focalLength = (int)focal;
        facedist = focal+MINDIST;
        halfview = Width/2;                                 // half view in pixels

        //
        // calculate scale value for vertical height calculations
        // and sprite x calculations
        //
        scale = (int) (halfview*facedist/(VIEWGLOBAL/2));

        //
        // divide heightnumerator by a posts distance to get the posts height for
        // the heightbuffer.  The pixel height is height>>2
        //
        heightNumerator = (int)((TILEGLOBAL*scale)>>6);

        //
        // calculate the angle offset from view angle of each pixel's ray
        //

        for (i=0;i<halfview;i++)
        {
            // start 1/2 pixel over, so viewangle bisects two middle pixels
            tang = (int)i*VIEWGLOBAL/Width/facedist;
            angle = (float) Math.Atan(tang);
            intang = (int) (angle*MathUtilities.RadiansToInteger);
            pixelAngle[halfview-1-i] = (short)intang;
            pixelAngle[halfview+i] = (short)(-intang);
        }
    }

    public static Wolf3DRaycastRenderer Create(Camera camera, Map map, int width, int height)
        => new(camera, map, width, height);

    public override byte[] Render()
    {
        Array.Fill(_result, (byte)0x19);
        for (var x = 0; x < Width*Height/2; x++)
        {
            _result[x] = 0x1d;
        }
        //return _result;
        Array.Clear(_spotVis);

        var focalLength = 0x5700;

        var viewAngle = Camera.Angle;

        var midAngle = viewAngle * (FINEANGLES / ANGLES);

        viewSin = sintable[(int)viewAngle];
        viewCos = costable[(int)viewAngle];

        viewX = Camera.X - FixedMul(focalLength, viewCos);
        viewY = Camera.Y + FixedMul(focalLength, viewSin);

        var focaltx = (short)(viewX >> (int)TILESHIFT);
        var focalty = (short)(viewY >> (int)TILESHIFT);

        WallRefresh(midAngle, focaltx, focalty);

        DrawScaleds();

        return _result;
    }

    private void WallRefresh(double midAngle, short focaltx, short focalty)
    {
        
        // These are where the player is in a partial tile
        var xpartialdown = viewX & (TILEGLOBAL - 1);
        var xpartialup = xpartialdown ^ (TILEGLOBAL - 1);
        var ypartialdown = viewY & (TILEGLOBAL - 1);
        var ypartialup = ypartialdown ^ (TILEGLOBAL - 1);

        short angle;
        int xstep = 0, ystep = 0;
        uint xpartial = 0, ypartial = 0;

        for (pixx = 0; pixx < Width; pixx++)
        {
            angle = (short)(midAngle + pixelAngle[pixx]);

            if (angle < 0) // -90 - -1 degree arc
                angle += ANG360; // -90 is the same as 270
            if (angle >= ANG360) // 360-449 degree arc
                angle -= ANG360; // -449 is the same as 89

            //
            // setup xstep/ystep based on angle
            //
            if (angle < ANG90) // 0-89 degree arc
            {
                xtilestep = 1;
                ytilestep = -1;
                xstep = finetangent[ANG90 - 1 - angle];
                ystep = -finetangent[angle];
                xpartial = (uint)xpartialup;
                ypartial = (uint)ypartialdown;
            }
            else if (angle < ANG180) // 90-179 degree arc
            {
                xtilestep = -1;
                ytilestep = -1;
                xstep = -finetangent[angle - ANG90];
                ystep = -finetangent[ANG180 - 1 - angle];
                xpartial = (uint)xpartialdown;
                ypartial = (uint)ypartialdown;
            }
            else if (angle < ANG270) // 180-269 degree arc
            {
                xtilestep = -1;
                ytilestep = 1;
                xstep = -finetangent[ANG270 - 1 - angle];
                ystep = finetangent[angle - ANG180];
                xpartial = (uint)xpartialdown;
                ypartial = (uint)ypartialup;
            }
            else if (angle < ANG360) // 270-359 degree arc
            {
                xtilestep = 1;
                ytilestep = 1;
                xstep = finetangent[angle - ANG270];
                ystep = finetangent[ANG360 - 1 - angle];
                xpartial = (uint)xpartialup;
                ypartial = (uint)ypartialup;
            }

            //
            // initialise variables for intersection testing
            //
            yintercept = FixedMul(ystep, (int)xpartial) + viewY;
            yinttile = yintercept >> (int)TILESHIFT;
            xtile = (short)(focaltx + xtilestep);

            xintercept = FixedMul(xstep, (int)ypartial) + viewX;
            xinttile = xintercept >> (int)TILESHIFT;
            ytile = (short)(focalty + ytilestep);

            texdelta = 0;

            //
            // trace along this angle until we hit a wall
            //
            // CORE LOOP!
            //
            var tileFound = false;
            while (!tileFound)
            {
                //
                // check intersections with vertical walls
                //
                if ((xtile - xtilestep) == xinttile && (ytile - ytilestep) == yinttile)
                    yinttile = ytile;

                if ((ytilestep == -1 && yinttile <= ytile) || (ytilestep == 1 && yinttile >= ytile))
                    tileFound = horizentry(xstep);

                if (tileFound) break;
                
                //tileFound = vertentry(ystep);

                //
                // check intersections with horizontal walls
                //
                if ((xtile - xtilestep) == xinttile && (ytile - ytilestep) == yinttile)
                    xinttile = xtile;

                if ((xtilestep == -1 && xinttile <= xtile) || (xtilestep == 1 && xinttile >= xtile))
                    tileFound = vertentry(ystep);
            }
        }
    }

    private record VisibleObject
    {
        public short TileX { get; set; }
        public short TileY { get; set; }
        public int ViewX { get; set; }
        public short ViewHeight { get; set; }
        public string AssetName { get; set; }
        //uint32_t   flags;
    }
    
    private IList<VisibleObject> VisibleObjects { get; set; } = new List<VisibleObject>();

    private bool IsSpotVisible(int tileX, int tileY)
    {
        return _spotVis[tileX, tileY];
    }

    private const int FRACBITS = 16;
    private const int ActorSize = 0x4000;
    private const int MinDistance = 0x5800;
    
    private (short ViewHeight, int ViewX, int TransX) TransformActor(Actor actor)
    {
        int gx,gy,gxt,gyt,nx,ny;
//
// translate point to view centered coordinates
//
        gx = actor.X-viewX;
        gy = actor.Y-viewY;

//
// calculate newx
//
        gxt = FixedMul(gx,viewCos);
        gyt = FixedMul(gy,viewSin);
        nx = gxt-gyt-ActorSize;         // fudge the shape forward a bit, because
        // the midpoint could put parts of the shape
        // into an adjacent wall

//
// calculate newy
//
        gxt = FixedMul(gx,viewSin);
        gyt = FixedMul(gy,viewCos);
        ny = gyt+gxt;

//
// calculate perspective ratio
//
        (short ViewHeight, int ViewX, int TransX) result = new();
        result.TransX = nx;

        if (nx<MINDIST)                 // too close, don't overflow the divide
        {
            result.ViewHeight = 0;
            return result;
        }

        result.ViewX = (CenterX + ny*scale/nx);

//
// calculate height (heightnumerator/(nx>>8))
//
        result.ViewHeight = (short)(heightNumerator/(nx>>8));
        return result;
    }
    
    private void DrawScaleds()
    {
        VisibleObjects.Clear();
        
        foreach (var actor in _map.Actors)
        {
            // If there is no asset to draw ignore it
            // Get the visible spot of the actor
            var isSpotVisible =
                IsSpotVisible(actor.TileX, actor.TileY) // 0, 0
                || IsSpotVisible(actor.TileX + 1, actor.TileY) // 1, 0
                || IsSpotVisible(actor.TileX + 1, actor.TileY + 1) // 1, 1
                || IsSpotVisible(actor.TileX + 1, actor.TileY - 1) // 1,-1
                || IsSpotVisible(actor.TileX, actor.TileY - 1) // 0,-1
                || IsSpotVisible(actor.TileX, actor.TileY + 1) // 0, 1
                || IsSpotVisible(actor.TileX - 1, actor.TileY) //-1, 0
                || IsSpotVisible(actor.TileX - 1, actor.TileY + 1) //-1, 1
                || IsSpotVisible(actor.TileX + 1, actor.TileY - 1); //-1,-1
            if (isSpotVisible) 
            {
                actor.IsActive = true;
                var transformed = TransformActor(actor);
                if (transformed.ViewHeight == 0) continue;

                var visibleObject = new VisibleObject
                {
                    ViewX = transformed.ViewX,
                    ViewHeight = transformed.ViewHeight,
                    AssetName = $"{actor.ActorStates.GetCurrentState()?.AssetFrame}{GetDirectionFrame(actor, transformed.ViewX)}",
                    TileX = actor.TileX,
                    TileY = actor.TileY,
                    //Flags = actor.Flags
                };
                VisibleObjects.Add(visibleObject);
                //actor.Flags |= Flags.Visible;
            }
            else
            {
                //actor.Flags &= ~Flags.Visible;
            }
        }
        
        //
        // draw from back to front
        //
        foreach (var visibleObject in VisibleObjects.OrderBy(x => x.ViewHeight))
        {
            ScaleShape(visibleObject);
        }
    }

    // private int[] dirangle = [0,ANGLES/8,2*ANGLES/8,3*ANGLES/8,4*ANGLES/8,
    //     5*ANGLES/8,6*ANGLES/8,7*ANGLES/8,ANGLES];
    //
    private int GetDirectionFrame(Actor actor, int viewX)
    {
        if (!actor.ActorStates.GetCurrentState()?.Directional ?? false)
            return 0;
        
        int angle, viewangle;

        // this isn't exactly correct, as it should vary by a trig value,
        // but it is close enough with only eight rotations

        viewangle = (int)( Camera.Angle + (CenterX - viewX) / (8 * Width / 320.0) );

        angle = (viewangle - 180) - actor.Angle;//dirangle[ob->dir];

        angle+=ANGLES/16;
        while (angle>=ANGLES)
            angle-=ANGLES;
        while (angle<0)
            angle+=ANGLES;

        return (angle/(ANGLES/8)) + 1;
    }
    
    private void ScaleShape(VisibleObject visibleObject)
    {
        if (!_map.SpriteCache.TryGetValue(visibleObject.AssetName, out var sprite))
            return;
        
        var scale = (visibleObject.ViewHeight >> 3);
        if (scale == 0) return;

        
        var fracStep = FixedDiv(scale, TEXTURESIZE / 2);
        var frac = sprite.Offset.X * fracStep;
        var actX = visibleObject.ViewX - scale;
        var topPix = CenterY - scale;

        var x2 = (frac >> FRACBITS) + actX;

        int w, i;
        for (w = 0, i = sprite.Offset.X; w < sprite.Width; w++, i++)
        {
            //
            // calculate edges of the shape
            //
            var x1 = x2;
            if (x1 >= Width)
                break;

            frac += fracStep;
            x2 = (frac >> FRACBITS) + actX;

            if (x2 < 0)
                continue;   // not into the view area

            if (x1 < 0)
                x1 = 0;     // clip left boundary

            if (x2 > Width)
                x2 = Width; // clip right boundary

            while (x1 < x2)
            {
                if (wallHeight[x1] < visibleObject.ViewHeight)
                {
                    var line = new byte[sprite.Height];
                    for (int j = 0; j < sprite.Height; j++)
                    {
                        line[j] = sprite.Data[j*sprite.Width + w];
                    }
                    
                    ScaleLine(x1, topPix, fracStep, line, sprite.Offset.Y);
                }

                x1++;
            }
        }
    }

    private void ScaleLine(int x, int topPix, int fracStep, byte[] line, int offsetX)
    {
        byte    col;
        int startPix;
        
        for (var end = 0; end < line.Length+offsetX; end++)
        {
            var top = 0;
            var start = 0;

            var frac = start + fracStep;

            var endPix = (frac >> FRACBITS) + topPix;

            for (int src = 0; start <= end; start++, src++)
            {
                startPix = endPix;

                if (startPix >= Height)
                    break;                          // off the bottom of the view area

                frac += fracStep;
                endPix = (frac >> FRACBITS) + topPix;

                if (endPix < 0)
                    continue;                       // not into the view area

                if (startPix < 0)
                    startPix = 0;                   // clip upper boundary

                if (endPix > Height)
                    endPix = Height;            // clip lower boundary

                if (start < offsetX)
                    continue;
                
                col = line[src-offsetX];
                
                if (col == 0xff)
                    continue;

                while (startPix < endPix)
                {
                    //_result[x, startPix] = col;
                    _result[startPix*Width + x] = col;
                    startPix++;
                }
            }
        }
    }
    
    private bool vertentry(int ystep)
    {
        int yinttemp;
// #ifdef REVEALMAP
//             mapseen[xtile][yinttile] = true;
// #endif
        tilehit2 = _map.TilePlane[yinttile, xtile];
        tilehit = _map.PlaneIds[0][yinttile, xtile]; // tilemap[xtile][yinttile];

        if (tilehit2 != null)
        {
            if (tilehit2 is Door)
            {
                //
                // hit a vertical door, so find which coordinate the door would be
                // intersected at, and check to see if the door is open past that point
                //
                var door = tilehit2 as Door;

                if (door.Action == DoorAction.Open)
                {
                    passvert(ystep); // door is open, continue tracing
                    return false;
                }

                yinttemp = yintercept + (ystep >> 1);    // add halfstep to current intercept position
                
                //
                // midpoint is outside tile, so it hit the side of the wall before a door
                //
                if (yinttemp >> TILESHIFT != yinttile)
                {
                    passvert(ystep);
                    return false;
                }

                if (door.Action != DoorAction.Closed)
                {
                    //
                    // the trace hit the door plane at pixel position yintercept, see if the door is
                    // closed that much
                    //
                    if ((ushort)yinttemp < door.Position)
                    {
                        passvert(ystep);
                        return false;
                    }
                }
                
                yintercept = yinttemp;
                xintercept = (int)((xtile << (int)TILESHIFT) + (TILEGLOBAL/2));
                
                HitVertDoor();
            }
            else if (tilehit == BIT_WALL)
            {
                // TODO:
            }
            else
            {
                xintercept = xtile << (int)TILESHIFT;

                HitVertWall();
            }

            return true;
        }

        //
        // mark the tile as visible and setup for next step
        //
        //spotvis[xtile][yinttile] = true;
        passvert(ystep);
        return false;
    }

    private void passvert(int ystep)
    {
        _spotVis[xtile, yinttile] = true;
        xtile += xtilestep;
        yintercept += ystep;
        yinttile = yintercept >> (int)TILESHIFT;
    }

    private bool horizentry(int xstep)
    {
        int xinttemp;
// #ifdef REVEALMAP
//             mapseen[xinttile][ytile] = true;
// #endif
        // TODO: Turn tilehit into a Wall vs Door MapComponent check
        tilehit2 = _map.TilePlane[ytile, xinttile];
        tilehit = _map.PlaneIds[0][ytile, xinttile];//tilemap[xinttile][ytile];
        
        if (tilehit2 != null)
        {
            if (tilehit2 is Door)
            {
                //
                // hit a horizontal door, so find which coordinate the door would be
                // intersected at, and check to see if the door is open past that point
                //
                var door = tilehit2 as Door;

                if (door.Action == DoorAction.Open) 
                {
                    passhoriz(xstep); // door is open, continue tracing
                    return false;
                }

                xinttemp = xintercept + (xstep >> 1);    // add half step to current intercept position

                //
                // midpoint is outside tile, so it hit the side of the wall before a door
                //
                if ((xinttemp >> TILESHIFT) != xinttile)
                {
                    passhoriz(xstep);
                    return false;
                }

                if (door.Action != DoorAction.Closed)
                {
                    //
                    // the trace hit the door plane at pixel position xintercept, see if the door is
                    // closed that much
                    //
                    if ((ushort)xinttemp < door.Position)
                    {
                        passhoriz(xstep);
                        return false;
                    }
                }

                xintercept = xinttemp;
                yintercept = (int)((ytile << (int)TILESHIFT) + (TILEGLOBAL/2));

                HitHorizDoor();
            }
            else if (tilehit == BIT_WALL)
            {
                // TODO: Moving pushwalls
            }
            else
            {
                yintercept = ytile << (int)TILESHIFT;

                HitHorizWall();
            }

            return true;
        }

        //
        // mark the tile as visible and setup for next step
        //
        //spotvis[xinttile][ytile] = true;
        passhoriz(xstep);
        return false;
    }

    private void passhoriz(int xstep)
    {
        _spotVis[xinttile, ytile] = true;
        ytile += ytilestep;
        xintercept += xstep;
        xinttile = xintercept >> (int)TILESHIFT;
    }
    
    private void HitHorizWall()
    {
        var texture = ((xintercept - texdelta) >> FIXED2TEXSHIFT) & TEXTUREMASK;

        if (ytilestep == -1)
            yintercept += (int)TILEGLOBAL;
        else
            texture = TEXTUREMASK - texture;

        wallHeight[pixx] = CalcHeight();
        postx = pixx;

        var tex = ytilestep > 0 ? _map.TileCache[tilehit].North : _map.TileCache[tilehit].South;
        if (_map.TilePlane[ytile-ytilestep, xinttile] is Door door)
        {
            tex = ytilestep > 0 ? _map.DoorCache[door.TileId].North : _map.DoorCache[door.TileId].South;
        }

        postsource = tex.Skip(texture).ToArray();
        ScalePost();
    }

    private void HitHorizDoor()
    {
        var door = tilehit2 as Door;
        var texture = ((xintercept - (door?.Position??0)) >> FIXED2TEXSHIFT) & TEXTUREMASK;

        wallHeight[pixx] = CalcHeight();
        postx = pixx;
        var tex = ytilestep > 0 ? _map.DoorCache[tilehit].North : _map.DoorCache[tilehit].South;

        postsource = tex.Skip(texture).ToArray();

        ScalePost ();
    }
    
    private void HitVertWall()
    {
        int texture = ((yintercept - texdelta) >> FIXED2TEXSHIFT) & TEXTUREMASK;

        if (xtilestep == -1)
        {
            texture = TEXTUREMASK - texture;
            xintercept += (int)TILEGLOBAL;
        }

        wallHeight[pixx] = CalcHeight();
        postx = pixx;

        var tex = xtilestep > 0 ? _map.TileCache[tilehit].East : _map.TileCache[tilehit].West;
        
        if (_map.TilePlane[yinttile, xtile - xtilestep] is Door door)
        {
            tex = ytilestep > 0 ? _map.DoorCache[door.TileId].East : _map.DoorCache[door.TileId].West;
        }

        postsource = tex.Skip(texture).ToArray();
        ScalePost();
    }

    private void HitVertDoor()
    {
        var doorPosition = (tilehit2 as Door)?.Position ?? 0;
        // TODO: Look up door and get its object data
        var texture = ((yintercept - doorPosition) >> FIXED2TEXSHIFT) & TEXTUREMASK;
        
        wallHeight[pixx] = CalcHeight();
        postx = pixx;
        
        var tex = xtilestep > 0 ? _map.DoorCache[tilehit].East : _map.DoorCache[tilehit].West;
        postsource = tex.Skip(texture).ToArray();
        ScalePost ();
    }

    private void ScalePost()
    {
        int ywcount, yoffs, yw, yd, yendoffs;
        byte col;

// #ifdef USE_SKYWALLPARALLAX
//         if (tilehit == 16)
//         {
//             ScaleSkyPost();
//             return;
//         }
// #endif

// #ifdef USE_SHADING
//         byte *curshades = shadetable[GetShade(wallheight[postx])];
// #endif

        ywcount = yd = wallHeight[postx] >> 3;
        if(yd <= 0) yd = 100;

        yoffs = (CenterY - ywcount) /* * bufferPitch*/;
        if(yoffs < 0) yoffs = 0;
        //yoffs += postx;

        yendoffs = CenterY + ywcount - 1;
        yw=TEXTURESIZE-1;

        while(yendoffs >= Height)
        {
            ywcount -= TEXTURESIZE/2;
            while(ywcount <= 0)
            {
                ywcount += yd;
                yw--;
            }
            yendoffs--;
        }
        if(yw < 0) return;

// #ifdef USE_SHADING
//         col = curshades[postsource[yw]];
// #else
        col = postsource[yw];
//#endif
        //yendoffs = yendoffs * bufferPitch + postx;
        while(yoffs <= yendoffs)
        {
            //_result[postx, yendoffs] = col;
            _result[yendoffs * Width + postx] = col;
            //vbuf[yendoffs] = col;
            ywcount -= TEXTURESIZE/2;
            if(ywcount <= 0)
            {
                do
                {
                    ywcount += yd;
                    yw--;
                }
                while(ywcount <= 0);
                if(yw < 0) break;
// #ifdef USE_SHADING
//                 col = curshades[postsource[yw]];
// #else
                col = postsource[yw];
//#endif
            }
            yendoffs --;
        }
    }

    [Obsolete("This is used before the time of floating point values")]
    private int FixedMul(int a, int b)
        => (int)(((long)a*b+0x8000) >> 16);
    
    [Obsolete("This is used before the time of floating point values")]
    private int FixedDiv (int a, int b)
        => (int)(((long)a << FRACBITS) / b);

    private short CalcHeight ()
    {
        short height;
        int   gx,gy,gxt,gyt,nx;

//
// translate point to view centered coordinates
//
        gx = xintercept - viewX;
        gy = yintercept - viewY;

//
// calculate nx
//
        gxt = FixedMul(gx,viewCos);
        gyt = FixedMul(gy,viewSin);
        nx = gxt - gyt;

//
// calculate perspective ratio
//
        if (nx < MINDIST)
            nx = (int)MINDIST;             // don't let divide overflow

        height = (short)(heightNumerator / (nx >> 8));

        return height;
    }
}