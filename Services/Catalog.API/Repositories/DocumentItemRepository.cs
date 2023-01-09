using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class DocumentItemRepository : IDocumentItemRepository
    {
        private readonly ICatalogContext _context;

        public DocumentItemRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentItem>> GetDocumentItems()
        {
            return await _context.DocumentItems.Find(doc => true).ToListAsync();
        }

        public async Task<DocumentItem> GetDocumentItem(string id)
        {
            return await _context.DocumentItems.Find(doc => doc.FileId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DocumentItem>> GetDocumentItemsByName(string fileName)
        {
            return await _context.DocumentItems.Find(doc => doc.FileName == fileName).ToListAsync();
        }

        public async Task<IEnumerable<DocumentItem>> GetDocumentItemsByNumber(string number)
        {
            return await _context.DocumentItems.Find(doc => doc.FileNumber == number).ToListAsync();
        }

        public async Task<bool> CreateDocumentItem(DocumentItem documentItem)
        {
            try
            {
                await _context.DocumentItems.InsertOneAsync(documentItem);
                return true;
            }
            catch (Exception ex)
            {
                //寫入失敗
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> UpdateDocumentItem(DocumentItem documentItem)
        {
            var updateResult = await _context.DocumentItems.ReplaceOneAsync(filter: doc => doc.FileId == documentItem.FileId, replacement: documentItem);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount >= 1;
        }

        public async Task<bool> DeleteDocumentItem(string id)
        {
            var deleteResult = await _context.DocumentItems.DeleteOneAsync(filter: doc => doc.FileId == id);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount >= 1;
        }
    }
}
