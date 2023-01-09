using System;
using System.Collections.Generic;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<DocumentItem> DocumentCollection)
        {
            bool hasData = DocumentCollection.Find(doc => true).Any();
            if (!hasData)
            {
                DocumentCollection.InsertManyAsync(GetFakeData());
            }
        }

        private static IEnumerable<DocumentItem> GetFakeData()
        {
            var fakeData = new List<DocumentItem>()
            {
                new DocumentItem()
                {
                    FileId = "11L000-8001-2022-00005-00",//歸檔編號
                    FileNumber = "TEST-MT-2022-01",//合約編號
                    FileName = "TEST公司維護合約書",
                    StaffId = "FC5233",
                    StaffName = "MAX",
                    DepartmentCode = "11L000",
                    DepartmentName = "資訊部",
                    FileStatus = "Y"
                },
                new DocumentItem()
                {
                    FileId = "11L000-8001-2022-00006-00",//歸檔編號
                    FileNumber = "TEST-DV-2022-02",//合約編號
                    FileName = "BOSS企業開發合約",
                    StaffId = "FC5233",
                    StaffName = "MAX",
                    DepartmentCode = "11L000",
                    DepartmentName = "資訊部",
                    FileStatus = "Y"
                },
            };

            return fakeData;
        }
    }
}