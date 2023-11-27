using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain
{
    public static class Extensions
    {
        public static bool IsAChildOfCategory(this Category newParentCategory, Category categoryToCheck, ref Dictionary<Guid, Category> dictCategories)
        {
            if (!newParentCategory.ParentId.HasValue)
                return false;

            if (newParentCategory.ParentId == categoryToCheck.Id)
                return true;

            if (dictCategories.TryGetValue(newParentCategory.ParentId.Value, out Category? value))
            {
                Category newParentCategoryParent = value;
                return newParentCategoryParent.IsAChildOfCategory(categoryToCheck, ref dictCategories);
            }

            return false;
        }
    }
}