using OtherSideCore.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Tests
{
    public class TestMappingProfile : GenericMappingProfile
    {
        public TestMappingProfile() : base()
        {
            CreateMap<TestDomainObject, TestEntity>().ReverseMap();
        }
    }
}
