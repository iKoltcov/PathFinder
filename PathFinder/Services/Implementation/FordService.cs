using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PathFinder.Entities;

namespace PathFinder.Services.Implementation
{
    public class FordService : IPathFinderService
    {
        protected readonly IMapService MapService;

        public FordService(IMapService mapService)
        {
            MapService = mapService;
        }

        public ResultEntity Find(int startVertex, int endVertex)
        {
            var n = MapService.GetMapSize();
            var rawMap = MapService.GetVertexEntities();
            var edges = new List<EdgeEntity>();

            foreach (var vertex in rawMap)
            {
                edges.AddRange(vertex.Paths.Select(
                    x => new EdgeEntity()
                    {
                        Source = vertex.Number,
                        Destination = x.VertexNumber,
                        Length = x.Length
                    }));
            }

            var vertexes = new int[n];
            var distances = new double[n];
            for (var i = 0; i < n; i++)
            {
                distances[i] = double.MaxValue;
                vertexes[i] = int.MaxValue;
            }
            distances[0] = startVertex;

            for (var i = 1; i < n - 1; ++i)
            {
                for (var j = 0; j < edges.Count; ++j)
                {
                    var u = edges[j].Source;
                    var v = edges[j].Destination;
                    var length = edges[j].Length;

                    if (distances[u] < double.MaxValue && distances[u] + length < distances[v])
                    {
                        distances[v] = distances[u] + length;
                        vertexes[v] = u;
                    }
                }
            }

            return ToResultEntity(endVertex, distances, vertexes);
        }

        private ResultEntity ToResultEntity(int endVertex, double[] distances, int[] vertexes)
        {
            if (distances[endVertex] >= double.MaxValue)
            {
                throw new Exception("Path not found");
            }

            var result = new List<VertexEntity>();
            for (int? current = endVertex; current != int.MaxValue; current = vertexes[current.Value])
            {
                result.Add(MapService.GetVertexEntities().First(x => x.Number == current));
            }
            result.Reverse();
            
            return new ResultEntity()
            {
                Length = distances[endVertex],
                Vertexes = result
            };
        }
        
    }
}