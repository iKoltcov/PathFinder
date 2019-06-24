using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PathFinder.Entities;
using PathFinder.Services;
using PathFinder.Services.Implementation;

namespace PathFinder.Test.Services
{
    [TestClass]
    public class DijkstraServiceTest
    {
        private Mock<IMapService> _mapService;
        private IPathFinderService _pathFinderService;

        private List<VertexEntity> map;
        
        [TestInitialize]
        public void Setup()
        {
            map = new List<VertexEntity>();
            map.AddRange(new List<VertexEntity>
            {
                new VertexEntity()
                {
                    Number = 0,
                    Paths = new List<PathEntity>
                    {
                        new PathEntity()
                        {
                            Length = 1,
                            VertexNumber = 1
                        }
                    }
                },
                new VertexEntity()
                {
                    Number = 1,
                    Paths = new List<PathEntity>
                    {
                        new PathEntity()
                        {
                            Length = 1,
                            VertexNumber = 0
                        },
                        new PathEntity()
                        {
                            Length = 1,
                            VertexNumber = 2
                        }
                    }
                },
                new VertexEntity()
                {
                    Number = 2,
                    Paths = new List<PathEntity>
                    {
                        new PathEntity()
                        {
                            Length = 1,
                            VertexNumber = 1
                        },
                        new PathEntity()
                        {
                            Length = 1,
                            VertexNumber = 3
                        }
                    }
                },
                new VertexEntity()
                {
                    Number = 3,
                    Paths = new List<PathEntity>
                    {
                        new PathEntity()
                        {
                            Length = 1,
                            VertexNumber = 2
                        },
                        new PathEntity()
                        {
                            Length = 1,
                            VertexNumber = 4
                        }
                    }
                },
                new VertexEntity()
                {
                    Number = 4,
                    Paths = new List<PathEntity>
                    {
                        new PathEntity()
                        {
                            Length = 1,
                            VertexNumber = 3
                        }
                    } 
                }
            });
            
            _mapService = new Mock<IMapService>();
            _mapService.Setup(mock => mock.GetVertexEntities())
                .Returns(map);
            _mapService.Setup(mock => mock.GetMapSize())
                .Returns(map.Count);
            
            _pathFinderService = new DijkstraService(_mapService.Object);
        }

        [TestMethod]
        public void FindNormalUsageSuccess()
        {
            var result = _pathFinderService.Find(0, 4);
            
            Assert.IsTrue(Math.Abs(result.Length - 4.0) <= double.Epsilon);
            Assert.AreEqual(result.Vertexes[0].Number, 0);
            Assert.AreEqual(result.Vertexes[1].Number, 1);
            Assert.AreEqual(result.Vertexes[2].Number, 2);
            Assert.AreEqual(result.Vertexes[3].Number, 3);
            Assert.AreEqual(result.Vertexes[4].Number, 4);
        }
        
        [TestMethod]
        public void FindNotFoundPathException()
        {
            map = new List<VertexEntity>
            {
                new VertexEntity()
                {
                    Number = 0,
                    Paths = new List<PathEntity>()
                },
                new VertexEntity()
                {
                    Number = 1,
                    Paths = new List<PathEntity>()
                }
            };
            _mapService.Setup(mock => mock.GetVertexEntities()).Returns(map);
            _mapService.Setup(mock => mock.GetMapSize()).Returns(map.Count);
            
            var exception = Assert.ThrowsException<Exception>(() => _pathFinderService.Find(0, 1));
            Assert.AreEqual(exception.Message, "Path not found");
        }
    }
}