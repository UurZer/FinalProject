using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Cashing;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;//CategoryDal oluşturulmaz bu sebeble servisini çağırmalıyız
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }
        //Claim :Yetki
        //Cross Cutting Concerns
        [SecuredOperation("admin,product.add")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            //İş Kurallarıları ile Validation Arasındaki fark
            /*
              Validation :Veriyi eklerken veya güncellerken o verinin bir takım kurallardan geçilmesi gerekmektedir
                örneğin 10 karakteri geçmez veya girilen bir mail ise @mail.com tarzı ile bitmesi gerekir.
               İş kuralları ise eklenen o veriyi incelememizi sağlar.Veritabanında o verinin adında bir başka
               ürün olup olmadığı örneği verilebilir.
             */
            IResult result= BusinessRules.Run(CheckProductNameIsSame(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CalculateCategoryCount());

            if (result != null)
            {
                return result;
            }
            
                _productDal.Add(product);
                return new SuccessResult(Messages.ProductAdded);
           
        }
        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 17)
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
           return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id),Messages.ProductListed);
        }
        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p=>p.ProductId==productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetailDtos()
        {
            if (DateTime.Now.Hour == 17)
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);

            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProducDetailDtos());
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 10)
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            else
                return new SuccessResult();
        }

        private IResult CheckProductNameIsSame(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            else
                return new SuccessResult();
        }
        private IResult CalculateCategoryCount()
        {
            var result = _categoryService.GetAll();

            if (result.Data.Count > 15)
                return new ErrorResult(Messages.ManyCategories);
            else
                return new SuccessResult();
        }
        //Örnek olması açısından yapılmıştır.Ve add kısmında hata vericektir çünkü
        //add o id'ye sahip bir product zaten mevcuttur.Bu durumda update işlemide
        //gerçekleşmemiş ve Transaction İşlemi yapılmıştır
        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            //_productDal.Update(product);
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }
    }
}
