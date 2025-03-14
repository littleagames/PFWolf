﻿using System.Reflection;
using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Models;

public class Actor : MapComponent
{
    private float _fineAngle;

    public Actor(int tileX, int tileY)
    {
        X = (tileX<<16)+(1<<16)/2;
        Y = (tileY<<16)+(1<<16)/2;
        IsActive = true;
        ActorStates.CurrentState = "Spawn";
    }

    public Actor(int tileX, int tileY, float angle) 
        : this(tileX, tileY)
    {
        FineAngle = angle;
    }

    public Actor(int tileX, int tileY, float angle, string beginningState) 
        : this(tileX, tileY, angle)
    {
        ActorStates.CurrentState = beginningState;
    }

    public float FineAngle
    {
        get => _fineAngle;
        set => _fineAngle = AngleUtilities.FixAngle(value);
    }
    
    public Direction Direction {
        get
        {
            switch (Angle)
            {
                case short a when a is >= 360 * 15 / 16 or < 360 * 1 / 16:
                    return Direction.East;
                // Northeast
                case short a when a is >= 360 * 1 / 16 and < 360 * 3 / 16:
                    return Direction.NorthEast;
                // North
                case short a when a is >= 360 * 3 / 16 and < 360 * 5 / 16:
                    return Direction.North;
                // Northwest
                case short a when a is >= 360 * 5 / 16 and < 360 * 7 / 16:
                    return Direction.NorthWest;
                // West
                case short a when a is >= 360 * 7 / 16 and < 360 * 9 / 16:
                    return Direction.West;
                // Southwest
                case short a when a is >= 360 * 9 / 16 and < 360 * 11 / 16:
                    return Direction.SouthWest;
                // South
                case short a when a is >= 360 * 11 / 16 and < 360 * 13 / 16:
                    return Direction.South;
                // Southeast
                case short a when a is >= 360 * 13 / 16 and < 360 * 15 / 16:
                    return Direction.SouthEast;
                default:
                    return Direction.NoDirection;
            }
        }
    }

    public short Angle => (short)FineAngle;
    public short TileX => (short)(X >> 16);
    public short TileY => (short)(Y >> 16);
    public float Speed { get; init; } = 0;
    public bool IsActive { get; set; }

    public ActorStates ActorStates { get; init; } = new();

    public void Rotate(float deltaAngle)
    {
        FineAngle += deltaAngle;
    }

    /// <summary>
    /// Moves the actor toward or away from their current angle +/- given angle to allow side-stepping or pushing back without changing the facing angle of the actor
    /// </summary>
    /// <param name="speed">Strength of the movement</param>
    /// <param name="angle">Offset of the current facing angle of the actor</param>
    public void Move(float speed, float angle)
    {
        var pa = FineAngle + angle;
        var pdx = (float)Math.Cos(pa.ToRadians());
        var pdy = (float)-Math.Sin(pa.ToRadians());
        
        X += (int)(pdx * speed);
        Y += (int)(pdy * speed);
    }

    public void Think()
    {
        if (!IsActive)
            return;

        // Non-transitional object
        var state = ActorStates.GetCurrentState();
        if (ActorStates.TickCount == 0)
        {
            //state.Think();
            if (state == null)
            {
                //Remove();
                return;
            }
            
            if (state.Think != null)
            {
                MethodInfo? info = this.GetType()?.GetMethod(state.Think);
                info?.Invoke(this, null);
            }

            return;
        }
        
        // Transitional object

        ActorStates.TickCount -= 1;
        while (ActorStates.TickCount <= 0)
        {
            if (state.Action != null)
            {
                MethodInfo? info = this.GetType()?.GetMethod(state.Action);
                info?.Invoke(this, null);
                // if (state == null)
                // {
                //     //Remove();
                //     return;
                // }
            }

            state = ActorStates.Next();
            
            if (state == null)
            {
                //Remove();
                return;
            }
            
            if (state.Ticks == 0)
            {
                ActorStates.TickCount = 0;
                break;
            }
            
            ActorStates.TickCount += state.Ticks;
        }
        
        // Think
        if (state.Think != null)
        {
            MethodInfo? info = this.GetType()?.GetMethod(state.Think);
            info?.Invoke(this, null);
        }
    }
}

public enum Direction
{
    NoDirection = 0,
    East = 1,
    NorthEast = 2,
    North = 3,
    NorthWest = 4,
    West = 5,
    SouthWest = 6,
    South = 7,
    SouthEast = 8
}

public class ActorStates
{
    public string CurrentState { get; set; } = null!;
    private int _currentStateIndex = 0;

    public int TickCount { get; set; } = 0;
    
    public Dictionary<string, IList<ActorState>> States { get; private set; } = new();

    public ActorState? GetCurrentState()
    {
        if (!States.TryGetValue(CurrentState, out var result))
            return null;

        if (_currentStateIndex < 0)
            _currentStateIndex = 0;

        if (_currentStateIndex >= result.Count)
        {
            if (TickCount == 0)
                _currentStateIndex = 0;
            else
                _currentStateIndex = result.Count - 1;
        }
            
        
        return result[_currentStateIndex];
    }
    
    public ActorState? Next()
    {
        _currentStateIndex++;
        var nextState = GetCurrentState();
        TickCount = nextState?.Ticks ?? 0;
        return nextState;
    }

    public void CreateStates(Dictionary<string, IList<ActorState>> states)
    {
        States = states;
        TickCount = GetCurrentState()?.Ticks ?? 0;
    }
}

public class ActorState
{
    /// <summary>
    /// Asset used for the current frame
    /// </summary>
    public string AssetFrame { get; set; } = null!;
    
    /// <summary>
    /// Number of frames this state runs for
    /// </summary>
    public int Ticks { get; set; } = 0;
    
    /// <summary>
    /// Initial action performed at the start of the state only
    /// </summary>
    public string Action { get; set; } = null!;
    
    /// <summary>
    /// Thinking action performed every frame
    /// </summary>
    public string Think { get; set; } = null!;

    /// <summary>
    /// Defines if actor has rotational frames
    /// </summary>
    public bool Directional { get; set; } = false;
}