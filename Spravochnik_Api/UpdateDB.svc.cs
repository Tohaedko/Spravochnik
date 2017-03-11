using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using DB;
using Spravochnik_Api.DictionaryServiceReference;

namespace Spravochnik_Api
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UpdateDB" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UpdateDB.svc or UpdateDB.svc.cs at the Solution Explorer and start debugging.
    public class UpdateDB : IUpdateDB
    {
        public void DoWork()
        {
            using (DBContext db = new DBContext())
            {
                var dictionaryServiceClient = new DictionaryServiceClient();
                dictionaryServiceClient.ClientCredentials.UserName.UserName =
                    ConfigurationManager.AppSettings["api_login"];
                dictionaryServiceClient.ClientCredentials.UserName.Password =
                    ConfigurationManager.AppSettings["api_password"];
                try
                {
                    dictionaryServiceClient.Open();
                    var dddd = dictionaryServiceClient.GetLocations("uk",true);
                }
                catch (Exception c)
                {
                    Console.WriteLine(c);
                    throw;
                }
                finally
                {
                    dictionaryServiceClient.Close();
                }
            }
        }
    }
}
