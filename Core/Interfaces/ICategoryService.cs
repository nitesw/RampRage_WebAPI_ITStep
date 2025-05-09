﻿using Core.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAll();
        Task<CategoryDto> Create(CategoryCreateDto dto, string userId);
        Task Edit(CategoryEditDto dto);
        Task Delete(int id);
    }
}
