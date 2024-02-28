using Engine.DataModels;

namespace Engine.Managers.Graphics
{
    internal interface IGraphicsManager
    {
        void LoadDataFiles();
        Graphic GetGraphic(string name);
    }
}
