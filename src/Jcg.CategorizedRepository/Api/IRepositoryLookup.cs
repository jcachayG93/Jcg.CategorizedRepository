﻿namespace Jcg.CategorizedRepository.Api
{
    /// <summary>
    ///     The basic data needed for the Lookup Data Model which is the model
    ///     that represents the lookup that will be stored in the database.
    /// </summary>
    public interface IRepositoryLookup
    {
        [Obsolete]
// TODO: R200 Remove
        string Key { get; set; }

        [Obsolete]
// TODO: R200 Remove
        bool IsDeleted { get; set; }

        [Obsolete]
// TODO: R200 Remove
        string DeletedTimeStamp { get; set; }
    }
}