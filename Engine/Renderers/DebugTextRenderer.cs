namespace Engine.Renderers;

public class DebugTextRenderer : IViewportRenderer
{
    public Position Origin { get; }
    public Dimension Size { get; }
    public byte[] Render()
    {
        throw new NotImplementedException();
    }
}