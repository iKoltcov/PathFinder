using System;
using System.Collections.Generic;
using PathFinder.Entities;

namespace PathFinder.Services.Implementation
{
    public class MapService : IMapService
    {
        private List<VertexEntity> Map { get; set; }

        private static Random random = new Random();

        public MapService(int maxVertex, int maxPaths)
        {
            Map = new List<VertexEntity>();
            
            for (var iterator = 0; iterator < maxVertex; iterator++)
            {
                Map.Add(new VertexEntity()
                {
                    Number = iterator,
                    Paths = new List<PathEntity>()
                });
            }

            foreach (var vertex in Map)
            {
                var pathsCount = random.Next(maxPaths) + 1;
                for (var iterator = 0; iterator < pathsCount; iterator++)
                {
                    var intoVertex = Map[random.Next(Map.Count)];
                    while (vertex == intoVertex)
                    {
                        intoVertex = Map[random.Next(Map.Count)];
                    }

                    var length = random.NextDouble();
                    
                    intoVertex.Paths.Add(new PathEntity()
                    {
                        Length = length,
                        VertexNumber = vertex.Number
                    });
                    vertex.Paths.Add(new PathEntity()
                    {
                        Length = length,
                        VertexNumber = intoVertex.Number
                    });
                }
            }
        }

        public List<VertexEntity> GetVertexEntities()
        {
            return Map;
        }

        public int GetMapSize()
        {
            return Map.Count;
        }
    }
}