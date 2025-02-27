﻿using Models.Entities;

namespace RepositoryPattern.IRepository.Interfaces.ICategory
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int id);

        bool ExistCategory(int id);

        bool ExistCategory(string nameCategory);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);

        bool Save();
    }
}