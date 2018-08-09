
namespace HT.CheckerApp.API.Models
{
    public class HelperRepository
    {
        public HelperRepository() { }
        //public List<ShopTrackerModel> GetExcelData()
        //{

        //    var pathToExcel = @"C:\Users\new\Documents\Visual Studio 2015\Projects\ShopTrack\ShopTrackData.xlsx";
        //    var sheetName = "Sheet1";

        //    var connectionString = String.Format(@"
        //        Provider=Microsoft.ACE.OLEDB.12.0;
        //        Data Source={0};
        //        Extended Properties=""Excel 12.0 Xml;HDR=YES""
        //    ", pathToExcel);
        //    //Creating and opening a data connection to the Excel sheet 
        //    using (var conn = new OleDbConnection(connectionString))
        //    {
        //        conn.Open();

        //        var cmd = conn.CreateCommand();
        //        cmd.CommandText = string.Format(
        //            @"SELECT * FROM [{0}$]",
        //            sheetName
        //        );

        //        using (var rdr = cmd.ExecuteReader())
        //        {
        //            //LINQ query - when executed will create anonymous objects for each row
        //            var queries =
        //                from DbDataRecord row in rdr
        //                select new ShopTrackerModel
        //                {
        //                    Company = row[0].ToString().Trim(),
        //                    Location = row[1].ToString().Trim(),
        //                    Address = row[2].ToString().Trim()
        //                };
        //            List<ShopTrackerModel> ShopData = new List<ShopTrackerModel>();
        //            ShopData = queries.ToList();

        //            //if (writeData)
        //            //{
        //            //    //Generates JSON from the LINQ query

        //            //    var destinationPath = @"C:\Users\new\Documents\Visual Studio 2015\Projects\T4D.SmartMonefy\T4D.SmartMonefy\data1.json";
        //            //    var json = "{\"rows\":" + JsonConvert.SerializeObject(mainQuery) + "}";

        //            //    //Write the file to the destination path    
        //            //    System.IO.File.WriteAllText(destinationPath, json);
        //            //}
        //            return ShopData;
        //        }
        //    }
        //}

    }
}