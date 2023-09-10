using DGBCommerce.Domain.Models;

namespace DGBCommerce.Domain
{
    public static class Extensions
    {
        public static bool IsAChildOfCategory(this Category newParentCategory, Category categoryToCheck, ref Dictionary<Guid, Category> dictCategories)
        {
            if (newParentCategory.Parent == null)
                return false;

            if (newParentCategory.Parent.Id == categoryToCheck.Id)
                return true;

            if (dictCategories.ContainsKey(newParentCategory.Parent.Id!.Value))
            {
                Category newParentCategoryParent = dictCategories[newParentCategory.Parent.Id!.Value];
                return newParentCategoryParent.IsAChildOfCategory(categoryToCheck, ref dictCategories);
            }

            return false;
        }
    }
}