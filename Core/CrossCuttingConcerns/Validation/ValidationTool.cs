using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
                                    //ProductValidator,Product
       public static void Validate(IValidator validator,object entity)
        {
            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);//Product validate et
            if (!result.IsValid)//Geçerli mi
            {
                throw new ValidationException(result.Errors);
            }
    }
}
}
