using System;
using System.Collections.Generic;
using System.Linq;
using PathFinder.Entities;

namespace PathFinder.Services.Implementation
{
    public class FloydService : IPathFinderService
    {
        protected readonly IMapService MapService;

        public FloydService(IMapService mapService)
        {
            MapService = mapService;
        }

        public ResultEntity Find(int startVertex, int endVertex)
        {
            var n = MapService.GetMapSize();
            var weights = new double[n, n];
            var paths = new int[n, n];
            
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    weights[i, j] = double.MaxValue;
                    paths[i, j] = n;
                }

                weights[i, i] = 0.0;
                paths[i, i] = i;
            }

            foreach (var vertexEntity in MapService.GetVertexEntities())
            {
                foreach (var pathEntity in vertexEntity.Paths)
                {
                    weights[vertexEntity.Number, pathEntity.VertexNumber] = Math.Min(pathEntity.Length, weights[vertexEntity.Number, pathEntity.VertexNumber]);
                    weights[pathEntity.VertexNumber, vertexEntity.Number] = Math.Min(pathEntity.Length, weights[pathEntity.VertexNumber, vertexEntity.Number]);
                    
                    paths[vertexEntity.Number, pathEntity.VertexNumber] = pathEntity.VertexNumber;
                }
            }
            
            for (var k = 0; k < n; k++)
            {
                for (var i = 0; i < n; i++)
                {
                    for (var j = 0; j < n; j++)
                    {
                        if (weights[i, k] + weights[k, j] < weights[i, j])
                        {
                            weights[i, j] = weights[i, k] + weights[k, j];
                            paths[i, j] = paths[i, k];
                        }
                    }
                }
            }

            var result = RecoveryPath(paths, startVertex, endVertex);

            return result;
        }

        private ResultEntity RecoveryPath(int[,] paths, int start, int end)
        {
            if (paths[start, end] >= MapService.GetMapSize())
            {
                throw new Exception("Path not found");
            }
            
            var result = new ResultEntity()
            {
                Length = 0,
                Vertexes = new List<VertexEntity>()
            };
            
            var current = start;
            while (current != end)
            {
                result.Vertexes.Add(MapService.GetVertexEntities().FirstOrDefault(x => x.Number == current));
                current = paths[current, end];
            }
            result.Vertexes.Add(MapService.GetVertexEntities().FirstOrDefault(x => x.Number == end));

            for (var i = 0; i + 1 < result.Vertexes.Count; i++)
            {
                result.Length += result.Vertexes[i].Paths.First(x => x.VertexNumber == result.Vertexes[i + 1].Number).Length;
            }
            
            return result;
        }
    }
}