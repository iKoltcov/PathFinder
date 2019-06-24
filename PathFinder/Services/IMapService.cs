using System.Collections.Generic;
using PathFinder.Entities;

namespace PathFinder.Services
{
    public interface IMapService
    {
        List<VertexEntity> GetVertexEntities();

        int GetMapSize();
    }
}