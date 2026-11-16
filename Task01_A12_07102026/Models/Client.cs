using System.Data.OleDb;

namespace Task01_A12_07102026.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string ClientPassword { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string ClientActive { get; set; }
        public string ClientLastLogin { get; set; }

        public static List<Client> GetAllFromDB()
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql = "select * from Clients";
            OleDbDataReader reader = cn.RunSelect(sql);
            List<Client> clientsList = new List<Client>();
            if (reader == null) return clientsList;
            while (reader.Read())
            {
                Client c1 = new Client();
                c1.ClientID = int.Parse(reader["ClientID"].ToString());
                c1.ClientName = reader["ClientName"].ToString();
                c1.ClientPassword = reader["ClientPassword"].ToString();
                c1.ClientEmail = reader["ClientEmail"].ToString();
                c1.ClientPhone = reader["ClientPhone"].ToString();
                c1.ClientActive = reader["ClientActive"].ToString();
                c1.ClientLastLogin = reader["ClientLastLogin"].ToString();
                clientsList.Add(c1);
            }
            cn.CloseConnection();
            return clientsList;
        }

        public static Client GetClientById(int id)
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql = $"select * from Clients where ClientID={id}";
            OleDbDataReader reader = cn.RunSelect(sql);
            Client c1 = null;
            if (reader != null && reader.Read())
            {
                c1 = new Client();
                c1.ClientID = int.Parse(reader["ClientID"].ToString());
                c1.ClientName = reader["ClientName"].ToString();
                c1.ClientPassword = reader["ClientPassword"].ToString();
                c1.ClientEmail = reader["ClientEmail"].ToString();
                c1.ClientPhone = reader["ClientPhone"].ToString();
                c1.ClientActive = reader["ClientActive"].ToString();
                c1.ClientLastLogin = reader["ClientLastLogin"].ToString();
            }
            cn.CloseConnection();
            return c1;
        }

        public static int AddClientToDB(Client c1)
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql = $"insert into Clients (ClientName, ClientPassword, ClientEmail, ClientPhone, ClientActive, ClientLastLogin) values ('{c1.ClientName}', '{c1.ClientPassword}', '{c1.ClientEmail}', '{c1.ClientPhone}', '{c1.ClientActive}', '{c1.ClientLastLogin}')";
            int r = cn.RunUpdateInsertDelete(sql);
            cn.CloseConnection();
            return r;
        }

        public static int DeleteClient(int id)
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql = $"delete from Clients where ClientID={id}";
            int r = cn.RunUpdateInsertDelete(sql);
            cn.CloseConnection();
            return r;
        }

        //check if the client exists by email and password
        public static Client CheckClient(string email, string password)
        {
            Connector cn = new Connector(Configs.Db_Name);
            string sql = $"select * from Clients where ClientEmail='{email}' and ClientPassword='{password}'";
            OleDbDataReader reader = cn.RunSelect(sql);
            Client c1 = null;
            if (reader != null && reader.Read())
            {
                c1 = new Client();
                c1.ClientID = int.Parse(reader["ClientID"].ToString());
                c1.ClientName = reader["ClientName"].ToString();
                c1.ClientPassword = reader["ClientPassword"].ToString();
                c1.ClientEmail = reader["ClientEmail"].ToString();
                c1.ClientPhone = reader["ClientPhone"].ToString();
                c1.ClientActive = reader["ClientActive"].ToString();
                c1.ClientLastLogin = reader["ClientLastLogin"].ToString();
            }
            cn.CloseConnection();
            return c1;
        }
    }
}
