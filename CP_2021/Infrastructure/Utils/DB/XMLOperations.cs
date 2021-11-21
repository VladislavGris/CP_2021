using CP_2021.Data;
using CP_2021.Infrastructure.Singletons;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Utils.DB
{
    internal class XMLOperations
    {
        private static ApplicationContext _context;

        public static void ExportTasks()
        {
            var data = _context.Database.ExecuteSqlRaw(Procedures.ExportTasksXML);
        }

        public static void ExportTaskById(Guid id)
        {
            var parmReturn = new SqlParameter
            {
                ParameterName = "XmlOutput",
                SqlDbType = System.Data.SqlDbType.Xml,
                Direction = System.Data.ParameterDirection.Output,
            };
            var result = _context.Database.ExecuteSqlRaw(Procedures.ExportTaskWithChildren, id, parmReturn);
            string xmlString = parmReturn.Value.ToString();
            string filename = "Export" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            if (!Directory.Exists("Export"))
                Directory.CreateDirectory("Export");
            string path = "Export\\" + filename + ".xml";
            using(var stream = File.Open(path, FileMode.OpenOrCreate))
            {
                byte[] export = new UTF8Encoding(true).GetBytes(xmlString);
                stream.Write(export, 0, export.Length);
            }

        }

        static XMLOperations()
        {
            _context = ApplicationUnitSingleton.GetApplicationContext();
        }
    }
}
