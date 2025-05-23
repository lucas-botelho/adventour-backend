﻿using Dapper;
using Microsoft.Data.SqlClient;

namespace Adventour.Api.Services.Database
{
    public interface IDatabaseService
    {
        DynamicParameters Parameters { get; set; }
        string StoredProcedure { get; set; }
        SqlConnection Connection { get; set; }
        T QuerySingle<T>();
        IEnumerable<T> QueryMultiple<T>();
        bool Update();
    }

}
