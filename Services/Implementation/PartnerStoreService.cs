using Domain.DTO;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class PartnerStoreService : IPartnerStoreService
    {
        private readonly IPartnerStoreRepository _partnerStoreRepository;

        public PartnerStoreService(IPartnerStoreRepository partnerStoreRepository)
        {
            _partnerStoreRepository = partnerStoreRepository;
        }



        public async Task<List<BookViewModel>> GetBooksForDisplayAsync()
        {
            var books = await _partnerStoreRepository.GetPartnerStoresAsync();

            return books.Select(b => new BookViewModel
            {
                Title = b.Title,
                BookCover=b.BookCover,
                Description= b.Description,
                Rating=(float)b.Rating,
                Genre=b.Genre,
                Price=(float)b.Price
            }).ToList();
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _partnerStoreRepository.GetTotalOrdersAsync(); 
        }

    }
}
