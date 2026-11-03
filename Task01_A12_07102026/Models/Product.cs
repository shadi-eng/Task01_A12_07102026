//using System.Data.OleDb;

using System.Data.OleDb;

namespace Task01_A12_07102026.Models
{
    public class Product
    {
        /*
         * ProductId
 ProductName
 ProductPrice
 ProductCategory
 ProductPhoto
 ProductVideo

         */
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductCategory { get; set; }
        public string ProductPhoto { get; set; }
        public string ProductVideo { get; set; }
        public bool ProductAvailable { get; set; }

        public int CategoryId { get; set; }



        public static int AddProductToDB(Product p)
        {
            //connnect to db and add the p to the Products Table
            Connector cn=new Connector(Configs.Db_Name);
           
            string sql = $"INSERT INTO Products(ProductName,ProductPrice,CategoryId,ProductPhoto,ProductVideo,ProductAvailable)";
            sql += $" VALUES('{p.ProductName}',{p.ProductPrice},{p.CategoryId},'{p.ProductPhoto}','{p.ProductVideo}',{p.ProductAvailable})";
            int x= cn.RunUpdateInsertDelete(sql);
            return x;
        }

        public static int InsertProduct(Product p)
        {
            Connector connector = new Connector(Configs.Db_Name);
            string sql = $"INSERT INTO Products (ProductName, ProductPrice, ProductCategory, ProductPhoto, ProductVideo, ProductAvailable) " +
                         $"VALUES ('{p.ProductName}', {p.ProductPrice}, '{p.ProductCategory}', '{p.ProductPhoto}', '{p.ProductVideo}', {p.ProductAvailable})";
            return connector.RunUpdateInsertDelete(sql);
        }
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            Connector connector = new Connector(Configs.Db_Name);
            string sql = "SELECT * FROM Products";
            OleDbDataReader reader = connector.RunSelect(sql);
            while (reader.Read())
            {
                Product product = new Product
                {
                    ProductId = int.Parse(reader["ProductId"].ToString()),
                    ProductName = reader["ProductName"].ToString(),
                    ProductPrice = double.Parse(reader["ProductPrice"].ToString()),
                    ProductCategory = reader["ProductCategory"].ToString(),
                    ProductPhoto = reader["ProductPhoto"].ToString(),
                    ProductVideo = reader["ProductVideo"].ToString(),
                    ProductAvailable = bool.Parse(reader["ProductAvailable"].ToString())
                };
                products.Add(product);
            }
            return products;
        }

        public static Product GetProductById(int productId)
        {
            Connector connector = new Connector(Configs.Db_Name);
            string sql = $"SELECT * FROM Products WHERE ProductId = {productId}";
            OleDbDataReader reader = connector.RunSelect(sql);
            if (reader.Read())
            {
                return new Product
                {
                    ProductId = int.Parse(reader["ProductId"].ToString()),
                    ProductName = reader["ProductName"].ToString(),
                    ProductPrice = double.Parse(reader["ProductPrice"].ToString()),
                    ProductCategory = reader["ProductCategory"].ToString(),
                    ProductPhoto = reader["ProductPhoto"].ToString(),
                    ProductVideo = reader["ProductVideo"].ToString(),
                    CategoryId = int.Parse(reader["CategoryId"].ToString()),
                    ProductAvailable = bool.Parse(reader["ProductAvailable"].ToString())
                };
            }
            return null;

        }

        public static int DeleteProductById(int productId)
        {
            Connector connector = new Connector(Configs.Db_Name);
            string sql = $"DELETE FROM Products WHERE ProductId = {productId}";
            return connector.RunUpdateInsertDelete(sql);
        }

        public static List<Product> SearchProductsByName(string searchTerm)
        {
            List<Product> products = new List<Product>();
            Connector connector = new Connector(Configs.Db_Name);
            //string sql2 = "SELECT * FROM Products";
            //sql2 += " WHERE ProductName LIKE '%" + searchTerm + "%'";

            //string sql3 = $"SELECT * FROM Products WHERE ProductName='%{searchTerm}%'";
            string sql = $"SELECT * FROM Products WHERE ProductName LIKE '%{searchTerm}%'";
            OleDbDataReader reader = connector.RunSelect(sql);
            while (reader.Read())
            {
                Product product = new Product
                {
                    ProductId = int.Parse(reader["ProductId"].ToString()),
                    ProductName = reader["ProductName"].ToString(),
                    ProductPrice = double.Parse(reader["ProductPrice"].ToString()),
                    ProductCategory = reader["ProductCategory"].ToString(),
                    ProductPhoto = reader["ProductPhoto"].ToString(),
                    ProductVideo = reader["ProductVideo"].ToString(),
                    ProductAvailable = bool.Parse(reader["ProductAvailable"].ToString())
                };
                products.Add(product);
            }
            return products;
        }


        // This method is a placeholder for updating a product in the database.
        //المنتج موجود من قبل في قاعدة البيانات، ونريد تحديث بياناته.
        //القيمة الوحيدة الثابتة هي ProductId، أما بقية الخصائص يمكن أن تتغير.
        public static int UpdateProduct(Product p)
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql="UPDATE Products SET ";
            sql += $"ProductName='{p.ProductName}', ";
            sql += $"ProductPrice={p.ProductPrice}, ";
            sql += $"CategoryId={p.CategoryId}, ";
            sql += $"ProductPhoto='{p.ProductPhoto}', ";
            sql += $"ProductVideo='{p.ProductVideo}', ";
            sql += $"ProductAvailable={p.ProductAvailable} ";
            sql+= $"WHERE ProductId={p.ProductId}";
            int x=cn.RunUpdateInsertDelete(sql);
            return x;
        }
    }
}
