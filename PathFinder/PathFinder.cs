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
            long attemptsCount = 10;
            mapService = new MapService(100, 3);
            
            pathFinderService = new DijkstraService(mapService);
            var result = new ResultEntity();
            long elapsedMilliseconds = 0;
            for (var i = 0; i < attemptsCount; i++)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                result = pathFinderService.Find(0, mapService.GetMapSize() - 1);
                stopwatch.Stop();
                
                elapsedMilliseconds += stopwatch.ElapsedMilliseconds;
            }
            result.Print($"DijkstraService: average time of {attemptsCount} iterations [{elapsedMilliseconds / (double)attemptsCount} ms.]");
            
            pathFinderService = new FloydService(mapService);
            result = new ResultEntity();
            elapsedMilliseconds = 0;
            for (var i = 0; i < attemptsCount; i++)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                result = pathFinderService.Find(0, mapService.GetMapSize() - 1);
                stopwatch.Stop();
                
                elapsedMilliseconds += stopwatch.ElapsedMilliseconds;
            }
            result.Print($"FloydService: average time of {attemptsCount} iterations [{elapsedMilliseconds / (double)attemptsCount} ms.]");
            
            pathFinderService = new FordService(mapService);
            result = new ResultEntity();
            elapsedMilliseconds = 0;
            for (var i = 0; i < attemptsCount; i++)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                result = pathFinderService.Find(0, mapService.GetMapSize() - 1);
                stopwatch.Stop();
                
                elapsedMilliseconds += stopwatch.ElapsedMilliseconds;
            }
            result.Print($"FordService: average time of {attemptsCount} iterations [{elapsedMilliseconds / (double)attemptsCount} ms.]");
        }
    }
}