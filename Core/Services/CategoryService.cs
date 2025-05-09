using AutoMapper;
using Core.Dtos.Category;
using Core.Interfaces;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CategoryService(
        RampRageDbContext context,
        IImageService imageService,
        IMapper mapper
        ) : ICategoryService
    {
        public async Task<List<CategoryDto>> GetAll()
        {
            var list = await context.Categories
                .ToListAsync();

            return mapper.Map<List<CategoryDto>>(list);
        }
    }
}
