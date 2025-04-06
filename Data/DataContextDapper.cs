using Dapper;
using DotNetFormApp.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

public class DataContextDapper
{
    private readonly string _connectionString;

    public DataContextDapper(string connectionString)
    {
        _connectionString = connectionString ;
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    public async Task<int> InsertFormData(FormData formData)
    {
        using (var connection = Connection)
        {
            string sql = "INSERT INTO appschema.FormData (Name, Email, Address, Image) VALUES (@Name, @Email, @Address, @Image); SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var result = await connection.QueryAsync<int>(sql, formData);
            return result.Single();
        }
    }

   public async Task<FormData?> GetFormDataById(int id)
{
    using (var connection = Connection)
    {
        try
        {
            string sql = "SELECT * FROM appschema.FormData WHERE Id = @Id";
            var result = await connection.QuerySingleOrDefaultAsync<FormData>(sql, new { Id = id });

            if (result == null)
            {
                // Log or handle the case when no data is found
                return null;
            }

            return result;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            throw new Exception("Error fetching data from database", ex);
        }
    }
}

}
