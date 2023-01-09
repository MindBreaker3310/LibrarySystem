using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities
{
    public class DocumentItem
    {
        /// <summary>
        /// 歸檔編號
        /// </summary>
        [BsonId]
        public string FileId { get; set; }
        /// <summary>
        /// 原始檔案編號
        /// </summary>
        [BsonElement("FileNumber")]
        public string FileNumber { get; set; }
        /// <summary>
        /// 檔案名稱
        /// </summary>
        [BsonElement("FileName")]
        public string FileName { get; set; }
        /// <summary>
        /// 員工編號
        /// </summary>
        [BsonElement("StaffId")]
        public string StaffId { get; set; }
        /// <summary>
        /// 員工姓名
        /// </summary>
        [BsonElement("StaffName")]
        public string StaffName { get; set; }
        /// <summary>
        /// 部門編號
        /// </summary>
        [BsonElement("DepartmentCode")]
        public string DepartmentCode { get; set; }
        /// <summary>
        /// 部門名稱
        /// </summary>
        [BsonElement("DepartmentName")]
        public string DepartmentName { get; set; }
        /// <summary>
        /// 檔案狀態(Y:在庫, N:借出, M:遺失, D:銷毀)
        /// </summary>
        [BsonElement("FileStatus")]
        public string FileStatus { get; set; }
    }
}
