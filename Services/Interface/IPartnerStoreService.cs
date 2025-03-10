﻿using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IPartnerStoreService
    {
        Task<List<BookViewModel>> GetBooksForDisplayAsync();
        Task<int> GetTotalOrdersAsync();

    }
}
