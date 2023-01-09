using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public interface IDocumentItemRepository
    {
        Task<IEnumerable<DocumentItem>> GetDocumentItems();
        Task<DocumentItem> GetDocumentItem(string id);
        Task<IEnumerable<DocumentItem>> GetDocumentItemsByName(string name);
        Task<IEnumerable<DocumentItem>> GetDocumentItemsByNumber(string category);

        Task<bool> CreateDocumentItem(DocumentItem documentItem);
        Task<bool> UpdateDocumentItem(DocumentItem documentItem);
        Task<bool> DeleteDocumentItem(string id);
    }
}
