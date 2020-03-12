using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.Context;
using Data.Model;
using Data.Repositories.Interface;
using Data.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        public readonly ConnectionStrings _connectionString;

        public SupplierRepository(ConnectionStrings connectionString)
        {
            _connectionString = connectionString;
        }
        MyContext myContext = new MyContext();
        public int Create(SupplierVM supplierVM)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramName", supplierVM.Name);
                parameters.Add("@paramJoinDate", supplierVM.JoinDate);
                var result = conn.Execute("SP_InsertDataSupplier", parameters, commandType: CommandType.StoredProcedure);


                return result;
            }
            //int result = 0;
            //var name = new SqlParameter("@Name", supplierVM.Name);
            //var joindate = new SqlParameter("@JoinDate", supplierVM.JoinDate);
            //myContext.Database.ExecuteSqlRaw("SP_InsertDataSupplier @Name, @JoinDate", name, joindate);

            //return result;
        }

        public int Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", id);
                var result = conn.Execute("SP_DeleteDataSupplier", parameters, commandType: CommandType.StoredProcedure);


                return result;
            }
        }

        public IEnumerable<Supplier> Get()

        {
            //var result = myContext.Suppliers.FromSqlRaw($"sp_getsupplierdeletefalse");
            var result = myContext.Suppliers.FromSqlRaw($"SP_GetIsDelFalseSupplier");
           // var result = myContext.Suppliers.FromSqlRaw($"sp_getsupplierpagination @p0, @p1, @p2", 2, 3, "");
            return result;
        }

        public IEnumerable<Supplier> Get(int id)
        {
            var result = myContext.Suppliers.FromSqlRaw("sp_getbyidsupplier @p0", id);
            return result;
        }

        public async Task<IEnumerable<Supplier>> GetDapper()
        {
            const string query = "sp_getIsDelFalseSupplier";
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var result = await conn.QueryAsync<Supplier>(query);
                return result;
            }
        }

        public int Update(int id, SupplierVM supplierVM)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", id);
                parameters.Add("@paramName", supplierVM.Name);
                parameters.Add("@paramJoinDate", supplierVM.JoinDate);
                var result = conn.Execute("SP_UpdateDataSupplier", parameters, commandType: CommandType.StoredProcedure);


                return result;
            }
            //    int result = 0;
            //    var Id = new SqlParameter("@Id", id);
            //    var name = new SqlParameter("@Name", supplierVM.Name);
            //    var joindate = new SqlParameter("@JoinDate", supplierVM.JoinDate);
            //    myContext.Database.ExecuteSqlRaw("SP_UpdateDataSupplier @Id, @Name, @JoinDate", Id, name, joindate);

            //    return result;
        }

        public async Task<IEnumerable<dynamic>> GetDapper(int id)
        {
            //const string query = "sp_getbyidsupplier Id";
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", id);
                var result = await conn.QueryAsync("SP_getbyidsupplier", parameters, commandType: CommandType.StoredProcedure);

                return result;

            }
        }

        public async Task<SupplierVM> Paging(string keyword, int pageNumber, int pageSize)
        {
            var result = new SupplierVM();
            try
            {
                using (var conn = new SqlConnection(_connectionString.Value))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@pageSize", pageSize);
                    parameters.Add("@pageNumber", pageNumber);
                    parameters.Add("@paramKeyword", keyword);
                    parameters.Add("@length", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@filterLength", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    result.data = await conn.QueryAsync<SupplierVM>("SP_FilterDataSupplier", parameters, commandType: CommandType.StoredProcedure);
                    int filterlength = parameters.Get<int>("@filterLength");
                    result.filterLength = filterlength;
                    int length = parameters.Get<int>("@length");
                    result.length = length;
                    return result;

                    //var result = conn.Query<ToDoListVM>("SP_FilterData", parameters, commandType: CommandType.StoredProcedure);
                    //return result;
                }
            }
            catch(Exception e) { }
            return result;
        }
    }
}
