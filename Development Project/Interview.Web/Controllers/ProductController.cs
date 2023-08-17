using Microsoft.AspNetCore.Mvc;
using Sparcpoint;
using Sparcpoint.SqlServer.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Interview.Web.Controllers
{
    [Route("api/v1/products")]
    public class ProductController : Controller
    {
        private readonly ISqlExecutor sqlExecutor;
        private readonly JsonDataSerializer jsonDataSerializer;
        public ProductController(ISqlExecutor sqlExecutor, JsonDataSerializer jsonDataSerializer)
        {
            this.sqlExecutor = sqlExecutor;
            this.jsonDataSerializer = jsonDataSerializer;

        }
        // NOTE: Sample Action
        [HttpGet]
        public Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = sqlExecutor.ExecuteAsync((con, trans) =>
                {
                    using (var cmd = new SqlCommand("SELECT * FROM PRODUCTS", (SqlConnection)con, (SqlTransaction)trans))
                    {
                        var adapter = new SqlDataAdapter(cmd);
                        var products = new DataSet();
                        adapter.Fill(products, "Products");
                        return Task.FromResult(jsonDataSerializer.Serialize(products.Tables["Products"]));
                    }
                });
                return Task.FromResult((IActionResult)Ok(result));
            }
            catch(Exception ex)
            {
                return Task.FromResult((IActionResult)BadRequest(ex.Message));
            }
        }

        [HttpGet]
        public Task<IActionResult> GetProduct(int instanceId)
        {
            try
            {
                var result = sqlExecutor.ExecuteAsync((con, trans) =>
                {
                    using (var cmd = new SqlCommand("SELECT * FROM PRODUCTS WHERE INSTANCEID = @instanceId", (SqlConnection)con, (SqlTransaction)trans))
                    {
                        cmd.Parameters.AddWithValue("instanceId", instanceId);
                        var adapter = new SqlDataAdapter(cmd);
                        var products = new DataSet();
                        adapter.Fill(products, "Products");
                        return Task.FromResult(jsonDataSerializer.Serialize(products.Tables["Products"]));
                    }
                });
                return Task.FromResult((IActionResult)Ok(result));
            }
            catch (Exception ex)
            {
                return Task.FromResult((IActionResult)BadRequest(ex.Message));
            }
        }

        [HttpGet]
        public Task<IActionResult> GetProductAttributes(int instanceId)
        {
            try
            {
                var result = sqlExecutor.ExecuteAsync((con, trans) =>
                {
                    using (var cmd = new SqlCommand("SELECT * FROM PRODUCTSATTRIBUTES WHERE INSTANCEID = @instanceId", (SqlConnection)con, (SqlTransaction)trans))
                    {
                        cmd.Parameters.AddWithValue("instanceId", instanceId);
                        var adapter = new SqlDataAdapter(cmd);
                        var productAttributes = new DataSet();
                        adapter.Fill(productAttributes, "ProductAttributes");
                        return Task.FromResult(jsonDataSerializer.Serialize(productAttributes.Tables["ProductAttributes"]));
                    }
                });
                return Task.FromResult((IActionResult)Ok(result));
            }
            catch (Exception ex)
            {
                return Task.FromResult((IActionResult)BadRequest(ex.Message));
            }
        }

        [HttpGet]
        public Task<IActionResult> GetProductCategories(int instanceId)
        {
            try
            {
                var result = sqlExecutor.ExecuteAsync((con, trans) =>
                {
                    using (var cmd = new SqlCommand("SELECT * FROM PRODUCTCATEGORIES PC INNER JOIN CATEGORIES C ON PC.CATEGORYINSTANCEID = C.INSTANCEID WHERE PC.INSTANCEID = @instanceId", (SqlConnection)con, (SqlTransaction)trans))
                    {
                        cmd.Parameters.AddWithValue("instanceId", instanceId);
                        var adapter = new SqlDataAdapter(cmd);
                        var productCategories = new DataSet();
                        adapter.Fill(productCategories, "ProductCategories");
                        return Task.FromResult((IActionResult)Ok(jsonDataSerializer.Serialize(productCategories.Tables["ProductCategories"])));
                    }
                });
                return Task.FromResult((IActionResult)Ok(result));
            }
            catch (Exception ex)
            {
                return Task.FromResult((IActionResult)BadRequest(ex.Message));
            }
        }

        [HttpPost]
        public Task<IActionResult> AddProduct(int instanceId, string name, string Description, string ProductImageUris, string ValidSkus)
        {
            try
            {
                var result = sqlExecutor.ExecuteAsync(async (con, trans) =>
                {
                    using (var cmd = new SqlCommand("INSERT INTO PRODUCTS (Name, Description, ProductImageUris, ValidSkus) VALUES (@Name, @Description, @ProductImageUris, @ValidSkus)", (SqlConnection)con, (SqlTransaction)trans))
                    {
                        cmd.Parameters.AddWithValue("Name", name);
                        cmd.Parameters.AddWithValue("Description", name);
                        cmd.Parameters.AddWithValue("ProductImageUris", name);
                        cmd.Parameters.AddWithValue("ValidSkus", name);
                        await cmd.ExecuteNonQueryAsync();
                        
                    }
                });
                return Task.FromResult((IActionResult)Ok(result));
            }
            catch (Exception ex)
            {
                return Task.FromResult((IActionResult)BadRequest(ex.Message));
            }
        }

        [HttpPost]
        public Task<IActionResult> AddProductAttribute(int instanceId, string key, string value)
        {
            try
            {
                var result = sqlExecutor.ExecuteAsync(async (con, trans) =>
                {
                    using (var cmd = new SqlCommand("INSERT INTO PRODUCTATTRIBUTES (InstanceId, Key, Value) VALUES (@InstanceId, @Key, @Value)", (SqlConnection)con, (SqlTransaction)trans))
                    {
                        cmd.Parameters.AddWithValue("InstanceId", instanceId);
                        cmd.Parameters.AddWithValue("Key", key);
                        cmd.Parameters.AddWithValue("Value", value);
                        await cmd.ExecuteNonQueryAsync();
                    }
                });
                return Task.FromResult((IActionResult)Ok(new object { }));
            }
            catch (Exception ex)
            {
                return Task.FromResult((IActionResult)BadRequest(ex.Message));
            }
        }

        [HttpPost]
        public Task<IActionResult> AddProductCategory(int instanceId, int categoryInstanceId)
        {
            try
            {
                var result = sqlExecutor.ExecuteAsync(async (con, trans) =>
                {
                    using (var cmd = new SqlCommand("INSERT INTO PRODUCTATTRIBUTES (InstanceId, CategoryInstanceId) VALUES (@InstanceId, @CategoryInstanceId)", (SqlConnection)con, (SqlTransaction)trans))
                    {
                        cmd.Parameters.AddWithValue("InstanceId", instanceId);
                        cmd.Parameters.AddWithValue("CategoryInstanceId", categoryInstanceId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                });
                return Task.FromResult((IActionResult)Ok(new object { }));
            }
            catch (Exception ex)
            {
                return Task.FromResult((IActionResult)BadRequest(ex.Message));
            }
        }
    }
}
