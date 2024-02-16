using System.Data;
using Dapper.Data;
using Dapper.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DataContextDapper dataContextDapper = new DataContextDapper(configuration);

            string tableCreateSql = File.ReadAllText("Users.sql");
            dataContextDapper.ExecuteSQL(tableCreateSql);

            string usersJson = File.ReadAllText("Users.json");

            IEnumerable<Users>? users = JsonConvert.DeserializeObject<IEnumerable<Users>>(usersJson);

            if(users != null)
            {
                using (IDbConnection dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    string sql = "SET IDENTITY_INSERT TutorialAppSchema.Users ON;"
                        + "INSERT INTO TutorialAppSchema.Users (UserId"
                        + ",FirstName"
                        + ",LastName"
                        + ",Email"
                        + ",Gender"
                        + ",Active)"
                        + "VALUES";

                    foreach (Users user in users )
                    {
                        string sqlToAdd = "(" + user.UserId
                            + ", '" + user.FirstName?.Replace("'", "''")
                            + "', '" + user.LastName?.Replace("'", "''")
                            + "', '" + user.Email?.Replace("'", "''")
                            + "', '" + user.Gender
                            + "', '" + user.Active
                            + "'),";

                        if((sql + sqlToAdd).Length > 4000)
                        {
                            dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                            sql = "SET IDENTITY_INSERT TutorialAppSchema.Users ON;"
                                    + "INSERT INTO TutorialAppSchema.Users (UserId"
                                    + ",FirstName "
                                    + ",LastName"
                                    + ",Email"
                                    + ",Gender"
                                    + ",Active)"
                                    + "VALUES";
                        }
                        sql += sqlToAdd;
                    }

                    dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                }
            }

            Console.WriteLine("Data seeded successfully!");
        }
    }
}