using System.Data.OleDb;

namespace Task01_A12_07102026.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string CategoryPhoto { get; set; }

        public static List<Category> GetAllFromDB()
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql = "select * from Categories ORDER BY CategoryName DESC";
            OleDbDataReader reader = cn.RunSelect(sql);
            List<Category> categoriesList = new List<Category>();
            if (reader == null) return categoriesList;
            while (reader.Read())
            {
                Category c1 = new Category();
                c1.CategoryId = int.Parse(reader["CategoryId"].ToString());
                c1.CategoryName = reader["CategoryName"].ToString();
                c1.CategoryDescription = reader["CategoryDescription"].ToString();
                c1.CategoryPhoto = reader["CategoryPhoto"].ToString();
                categoriesList.Add(c1);
            }
            cn.CloseConnection();
            return categoriesList;
        }

        public static Category GetCategoryById(int id)
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql = $"select * from Categories where CategoryId={id}";
            OleDbDataReader reader = cn.RunSelect(sql);
            Category c1 = null;
            if (reader != null && reader.Read())
            {
                c1 = new Category();
                c1.CategoryId = int.Parse(reader["CategoryId"].ToString());
                c1.CategoryName = reader["CategoryName"].ToString();
                c1.CategoryDescription = reader["CategoryDescription"].ToString();
                c1.CategoryPhoto = reader["CategoryPhoto"].ToString();
            }
            cn.CloseConnection();
            return c1;
        }

        public static int AddCategoryToDB(Category c1)
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql = $"insert into Categories (CategoryName, CategoryDescription, CategoryPhoto) values ('{c1.CategoryName}', '{c1.CategoryDescription}', '{c1.CategoryPhoto}')";
            int r = cn.RunUpdateInsertDelete(sql);
            cn.CloseConnection();
            return r;
        }


        public static int DeleteCategory(int id)
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql = $"delete from Categories where CategoryId={id}";
            int r = cn.RunUpdateInsertDelete(sql);
            cn.CloseConnection();
            return r;
        }

        public static List<Category> SearchCategoriesByName(string str)
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql = $"select * from Categories where CategoryName like '%{str}%'";
            OleDbDataReader reader = cn.RunSelect(sql);
            List<Category> categoriesList = new List<Category>();
            if (reader == null) return categoriesList;
            while (reader.Read())
            {
                Category c1 = new Category();
                c1.CategoryId = int.Parse(reader["CategoryId"].ToString());
                c1.CategoryName = reader["CategoryName"].ToString();
                c1.CategoryDescription = reader["CategoryDescription"].ToString();
                c1.CategoryPhoto = reader["CategoryPhoto"].ToString();
                categoriesList.Add(c1);
            }
            cn.CloseConnection();
            return categoriesList;
        }

    }
}
