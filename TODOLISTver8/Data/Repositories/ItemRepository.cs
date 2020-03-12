using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public class ItemRepository : IItemRepository
    {
        public readonly ConnectionStrings _connectionString;
        public ItemRepository(ConnectionStrings connectionString)
        {
            _connectionString = connectionString;
        }
        public int Create(ItemVM itemVM)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramName", itemVM.Name);
                parameters.Add("@paramStock", itemVM.Stock);
                parameters.Add("@paramPrice", itemVM.Price);
                parameters.Add("@paramSupplierId", itemVM.SupplierId);
                var result = conn.Execute("SP_InsertDataItems", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public int Delete(int Id)
        {
            var result = 0;
            try
            {
                using (var conn = new SqlConnection(_connectionString.Value))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@paramId", Id);
                    result = conn.Execute("SP_DeleteDataItem", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch(Exception e) { }
            return result;
        }

        public async Task<IEnumerable<ItemVM>>Get()
        {
            const string query = "SP_GetIsDelFalseItem";
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var result = await conn.QueryAsync<ItemVM>(query);
                return result;
            }
        }

        public async Task<IEnumerable<ItemVM>> Get(int Id)
        {
            var result = (IEnumerable<ItemVM>)null;
            try
            {
                using (var conn = new SqlConnection(_connectionString.Value))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@paramId", Id);
                    result = await conn.QueryAsync<ItemVM>("SP_GetbyIdItem", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch(Exception e) { }
            return result;
        }

        public async Task<IEnumerable<ItemVM>> GetTodoLists(string Id, int status)
        {
            var result = (IEnumerable<ItemVM>)null;
            try
            {
                using (var conn = new SqlConnection(_connectionString.Value))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@paramUserId", Id);
                    parameters.Add("@paramStatus", status);
                    result = await conn.QueryAsync<ItemVM>("SP_GetIsDelUserIdItem", parameters, commandType: CommandType.StoredProcedure);
                    
                    return result;
                }
            }
            catch (Exception m) { }
            return result;
        }

        // IEnumerable<ItemVM> Paging(string userId, int param1, string keyword, int pageNumber, int pageSize)
        public async Task<ItemVM> Paging(string keyword, int pageNumber, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@pageSize", pageSize);
                parameters.Add("@pageNumber", pageNumber);
                parameters.Add("@paramKeyword", keyword);
                parameters.Add("@length", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@filterLength", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = new ItemVM();
                result.data = await conn.QueryAsync<ItemVM>("SP_FilterDataItem", parameters, commandType: CommandType.StoredProcedure);
                int filterlength = parameters.Get<int>("@filterLength");
                result.filterLength = filterlength;
                int length = parameters.Get<int>("@length");
                result.length = length;
                return result;

                //var result = conn.Query<ItemVM>("SP_FilterData", parameters, commandType: CommandType.StoredProcedure);
                //return result;
            }
        }

        public IEnumerable<ItemVM> Search(string userId, string keyword, int param1)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramUserId", userId);
                parameters.Add("@paramKeyword", keyword);
                parameters.Add("@param1", param1);
                var result = conn.Query<ItemVM>("SP_Search", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public int Update(int Id, ItemVM itemVM)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", Id);
                parameters.Add("@paramName", itemVM.Name);
                parameters.Add("@paramStock", itemVM.Stock);
                parameters.Add("@paramPrice", itemVM.Price);
                parameters.Add("@paramSupplier", itemVM.SupplierId);
                var result = conn.Execute("SP_UpdateDataItem", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
