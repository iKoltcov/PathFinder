using System.Collections.Generic;

namespace PathFinder.Entities
{
    public class ResultEntity
    {
        public double Length { get; set; }
        
        public List<VertexEntity> Vertexes { get; set; }
    }
}