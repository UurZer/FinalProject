﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)//ValidatorType == ValidatorProduct (Örnek)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("Doğru Sınıf değil");
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            //Instance Oluşturma (newleme)
            var validator = (IValidator)Activator.CreateInstance(_validatorType);//ValidatorProduct a=new ValidatorProduct (); Aynı işlem Daha performanslı
            //Base classtaki generic içindeki type 'i çeker(Validate edeceği tipi bulmuş olur
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];//Product Örnek
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}
