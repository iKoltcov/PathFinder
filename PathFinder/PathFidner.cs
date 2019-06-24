using System.Diagnostics;
using PathFinder.Entities;
using PathFinder.Extensions;
using PathFinder.Services;
using PathFinder.Services.Implementation;

namespace PathFinder
{
    class PathFidner
    {
        private static IMapService mapService;
        private static IPathFinderService pathFinderService;

        static void Main(string[] args)
        {
            mapService = new MapService(200, 5);
            pathFinderService = new DijkstraService(mapService);

            var result = new ResultEntity();
            
            long elapsedMilliseconds = 0;
            long attemptsCount = 10000;

            for (var i = 0; i < attemptsCount; i++)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                result = pathFinderService.Find(0, 9);
                stopwatch.Stop();
                
                elapsedMilliseconds += stopwatch.ElapsedMilliseconds;
            }

            result.Print($"{elapsedMilliseconds / (double)attemptsCount}");
        }
    }
}