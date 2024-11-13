namespace Engine.Renderers;

public interface IViewportRenderer
{
    Position Origin { get; }
    Dimension Size { get; }
    byte[] Render();
}