namespace LittleAGames.PFWolf.SDK.Components;

public class Wolf3DStatusBar : RenderComponent
{
    private readonly StatusNumbers _level = StatusNumbers.Create(new Position(16, 176), 2);
    private readonly StatusNumbers _score = StatusNumbers.Create(new Position(48, 176), 6);
    private readonly StatusNumbers _lives = StatusNumbers.Create(new Position(14 * 8, 176), 1);
    private readonly StatusNumbers _health = StatusNumbers.Create(new Position(21 * 8, 176), 3);
    private readonly StatusNumbers _ammo = StatusNumbers.Create(new Position(27 * 8, 176), 2);
    
    private Wolf3DStatusBar(Position position)
    {
        Position = position;
    }

    public static Wolf3DStatusBar Create(Position position) => new(position);
    
    public Position Position { get; private set; }

    public override void OnStart()
    {
        Children.Add(Graphic.Create("statusbar", Position));
        
        // Draw Face
        // TODO: Turn this into an animated component
        Children.Add(Graphic.Create("face1a", new Position(136, 164)));
        
        // Draw Level
        Children.Add(_level);
        _level.Value = 1;
        // TODO: This is set from?? The map definition?
        
        // Draw Score
        Children.Add(_score);
        
        // Draw Lives
        Children.Add(_lives);
        _lives.Value = 3;
        
        // Draw Health
        Children.Add(_health);
        _health.Value = 100;
        
        // Draw Ammo
        Children.Add(_ammo);
        _ammo.Value = 8;
        
        // Draw Weapon
        // TODO: From weapon definition data
        Children.Add(Graphic.Create("gun", new Position(32*8, 160+8)));
    }
}