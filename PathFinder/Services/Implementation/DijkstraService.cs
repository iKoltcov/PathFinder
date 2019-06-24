using System;
using System.Collections.Generic;
using System.Linq;
using PathFinder.Entities;

namespace PathFinder.Services.Implementation
{
    public class DijkstraService : IPathFinderService
    {
        protected readonly IMapService MapService;

        public DijkstraService(IMapService mapService)
        {
            MapService = mapService;
        }

        public ResultEntity Find(int startVertex, int endVertex)
        {
            var result = new List<VertexEntity>();
            
            var n = MapService.GetMapSize();
            var map = MapService.GetVertexEntities();
            
            var pathWasFound = false;
            var weights = new double[n];
            var checks = new bool[n];

            for (var i = 0; i < n; i++)
            {
                weights[i] = double.MaxValue;
                checks[i] = false;
            }
            weights[startVertex] = 0;

            double minimalLength;
            int? minimalLengthId;
            do
            {
                minimalLength = double.MaxValue;
                minimalLengthId = null;
                for (var i = 0; i < n; i++)
                {
                    if (!checks[i] && minimalLength > weights[i])
                    {
                        minimalLength = weights[i];
                        minimalLengthId = i;
                    }
                }

                if (minimalLengthId.HasValue)
                {
                    foreach (var path in map[minimalLengthId.Value].Paths)
                    {
                        if (path.Length > 0)
                        {
                            var currentWeight = minimalLength + path.Length;
                            var connectedVertexId = path.VertexNumber;

                            if (currentWeight < weights[connectedVertexId])
                            {
                                weights[connectedVertexId] = currentWeight;
                            }
                        }
                    }

                    checks[minimalLengthId.Value] = true;
                    if (minimalLengthId == endVertex)
                    {
                        pathWasFound = true;
                        break;
                    }
                }
            } while (minimalLength < double.MaxValue);

            if (!pathWasFound)
            {
                throw new Exception("Path not found");
            }

            var resultLength = minimalLength;
            var currentVertexNumber = endVertex;
            for (var i = 0; i < n; i++)
            {
                checks[i] = false;
            }

            do
            {
                result.Add(map.First(x => x.Number == currentVertexNumber));
                checks[currentVertexNumber] = true;
                minimalLength = double.MaxValue;
                minimalLengthId = null;

                foreach (var path in map[currentVertexNumber].Paths)
                {
                    var connectedRoomId = path.VertexNumber;
                    var currentLength = weights[connectedRoomId] + path.Length;

                    if (!checks[connectedRoomId] && minimalLength > currentLength)
                    {
                        minimalLength = currentLength;
                        minimalLengthId = connectedRoomId;
                    }
                }

                currentVertexNumber = minimalLengthId.Value;
            } while (currentVertexNumber != startVertex);
            
            result.Add(map.First(x => x.Number == startVertex));
            result.Reverse();
            
            return new ResultEntity()
            {
                Length = resultLength,
                Vertexes = result
            };
        }
    }
}