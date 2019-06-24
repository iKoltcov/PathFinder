using PathFinder.Entities;

namespace PathFinder.Services
{
    public interface IPathFinderService
    {
        ResultEntity Find(int startVertex, int endVertex);
    }
}