using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Tests
{
   public class TestEntity : EntityBase
   {
      public TestEntity()
      {
         CreatedById = 1;
         LastModifiedById = 1;
         CreationDate = DateTime.Now;
         LastModifiedDateTime = DateTime.Now;
      }
   }
}
