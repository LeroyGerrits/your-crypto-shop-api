﻿using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces
{
    public interface ICategoryRepository : IMutableRepository<Category, GetCategoriesParameters> { }
}