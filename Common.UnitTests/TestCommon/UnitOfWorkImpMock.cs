using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Support.UnitOfWork;
using Testing.Common.Types;

namespace Common.UnitTests.TestCommon
{

    internal class UnitOfWorkImMock
    {
        public UnitOfWorkImMock()
        {
            _moq = new Mock<IUnitOfWorkImp<AggregateDatabaseModel, LookupDatabaseModel>>();
        }

        private readonly Mock<IUnitOfWorkImp<AggregateDatabaseModel, LookupDatabaseModel>> _moq;

        public IUnitOfWorkImp<AggregateDatabaseModel, LookupDatabaseModel> Object => _moq.Object;


    }
}
