using System.Collections.Generic;

namespace PathFinder.Entities
{
    public class VertexEntity
    {
        public int Number { get; set; }
        
        public List<PathEntity> Paths { get; set; }
    }
}