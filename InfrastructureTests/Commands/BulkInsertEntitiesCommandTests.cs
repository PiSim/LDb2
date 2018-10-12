﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabDbContext;

namespace Infrastructure.Commands.Tests
{
    [TestClass()]
    public class BulkInsertEntitiesCommandTests
    {
        string connectionName = "name=LabDbTest";

        [TestMethod()]
        public void BulkInsertTest()
        {
            List<Aspect> _aspectList = new List<Aspect>()
            {
                new Aspect() { Code = "111", Name = "TEST_ENTITY" },
                new Aspect() { Code = "222", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "333", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "444", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "555", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "666", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "777", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "888", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "999", Name = "TEST_ENTITY"  },
            };

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                foreach (Aspect asp in _aspectList)
                {
                    Aspect tempAspect = testContext.Aspects.FirstOrDefault(aet => aet.Code == asp.Code);
                    if (tempAspect != null)
                        testContext.Entry(tempAspect).State = System.Data.Entity.EntityState.Deleted;
                }
                testContext.SaveChanges();
            }

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                (new BulkInsertEntitiesCommand(_aspectList)).Execute(testContext);
            }

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                foreach (Aspect asp in _aspectList)
                {
                    Aspect tempAspect = testContext.Aspects.FirstOrDefault(aet => aet.Code == asp.Code);
                    Assert.IsNotNull(tempAspect);
                }
            }
        }
    }
}