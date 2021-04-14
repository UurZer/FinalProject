using System;
using Business.Concrete;
using DataAccess.Concrete.InMemory;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete;

namespace ConsoleUI
{
    class Program
    {
        //SOLID
        //Open Closed Principle
        //
        static void Main(string[] args)
        {
            ProductTest();
            //IoC
            //CategoryTest();
        }

        private static void CategoryTest()
        {
            CategoryManager categoryManager = new CategoryManager(new EFCategoryDal());
            foreach (var category in categoryManager.GetAll())
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductTest()
        {
            ProductManager productManager = new ProductManager(new EFProductDal());
            var result = productManager.GetProductDetailDtos();
            if (result.Success)
            {
                foreach (var product in result.Data)
                {
                    Console.WriteLine(product.CategoryName);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
        }
    }
}
