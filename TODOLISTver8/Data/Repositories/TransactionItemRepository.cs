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
    public class TransactionItemRepository : ITransactionItemRepository
    {
        public readonly ConnectionStrings _connectionString;
        public TransactionItemRepository(ConnectionStrings connectionString)
        {
            _connectionString = connectionString;
        }
        public int Create(TransactionItemVM transactionItemVM)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramItemId", transactionItemVM.itemId);
                parameters.Add("@paramQuantity", transactionItemVM.Quantity);
                var result = conn.Execute("SP_InsertDataTI", parameters, commandType: CommandType.StoredProcedure);
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
                    result = conn.Execute("SP_DeleteDataTransactionItem", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e) { }
            return result;
        }

        public async Task<IEnumerable<TransactionItem>> Get()
        {
            const string query = "SP_GetIsDelFalseTransactionItem";
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var result = await conn.QueryAsync<TransactionItem>(query);
                return result;
            }
        }

        public async Task<IEnumerable<TransactionItemVM>> Get(int Id)
        {
            var result = (IEnumerable<TransactionItemVM>)null;
            try
            {
                using (var conn = new SqlConnection(_connectionString.Value))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@paramId", Id);
                    result = await conn.QueryAsync<TransactionItemVM>("SP_GetbyIdItem", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e) { }
            return result;
        }

        public async Task<IEnumerable<TransactionItemVM>> GetTodoLists(string Id, int status)
        {
            var result = (IEnumerable<TransactionItemVM>)null;
            try
            {
                using (var conn = new SqlConnection(_connectionString.Value))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@paramUserId", Id);
                    parameters.Add("@paramStatus", status);
                    result = await conn.QueryAsync<TransactionItemVM>("SP_GetIsDelUserIdItem", parameters, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception m) { }
            return result;
        }

        public async Task<TransactionItemVM> Paging(string keyword, int pageNumber, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@pageSize", pageSize);
                parameters.Add("@pageNumber", pageNumber);
                parameters.Add("@paramKeyword", keyword);
                parameters.Add("@length", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@filterLength", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = new TransactionItemVM();
                result.data = await conn.QueryAsync<TransactionItemVM>("SP_FilterDataTransactionItem", parameters, commandType: CommandType.StoredProcedure);
                int filterlength = parameters.Get<int>("@filterLength");
                result.filterLength = filterlength;
                int length = parameters.Get<int>("@length");
                result.length = length;
                return result;
            }
        }

        public IEnumerable<TransactionItemVM> Search(string userId, string keyword, int param1)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramUserId", userId);
                parameters.Add("@paramKeyword", keyword);
                parameters.Add("@param1", param1);
                var result = conn.Query<TransactionItemVM>("SP_Search", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        //belum selesai
        public int Update(int Id, TransactionItemVM transactionItemVM)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", Id);
                parameters.Add("@paramName", transactionItemVM.Quantity);
                var result = conn.Execute("SP_UpdateDataItem", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }

        }
    }
}
