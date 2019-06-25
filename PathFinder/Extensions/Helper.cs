using System;
using PathFinder.Entities;

namespace PathFinder.Extensions
{
    public static class Helper
    {
        public static ResultEntity Print(this ResultEntity result, string message)
        {
            for (var iterator = 0; iterator < result.Vertexes.Count; iterator++){
                if (iterator + 1 == result.Vertexes.Count)
                {
                    Console.WriteLine($"{result.Vertexes[iterator].Number} = {result.Length} {message}");
                }
                else
                {
                    Console.Write($"{result.Vertexes[iterator].Number}, ");
                }
            }

            return result;
        }
    }
}