﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Api;

namespace Support.CategorizedRepository.Support
{
    internal class LookupMapperAdapter<TLookupDatabaseModel, TLookup>
    : ILookupMapperAdapter<TLookupDatabaseModel, TLookup>
        where TLookupDatabaseModel : ILookupDataModel
    {
        private readonly ILookupMapper<TLookupDatabaseModel, TLookup> _adaptee;

        public LookupMapperAdapter(ILookupMapper<TLookupDatabaseModel, TLookup> adaptee)
        {
            _adaptee = adaptee;
        }
        public IEnumerable<TLookup> Map(CategoryIndex<TLookupDatabaseModel> categoryIndex)
        {
            return categoryIndex.Lookups.Select(l => _adaptee.Map(l)).ToList();
        }
    }
}
